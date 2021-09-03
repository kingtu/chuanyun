
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;

public class D001419Siizvpn3x17wj6jj3pifsmbic3 : H3.SmartForm.SmartFormController
{
    H3.DataModel.BizObject thisObj;
    Dictionary<string, bool> boolConfig;
    H3.Workflow.Instance.WorkflowInstance instance;
    public D001419Siizvpn3x17wj6jj3pifsmbic3(H3.SmartForm.SmartFormRequest request) : base(request)
    {   //本表单数据
        thisObj = this.Request.BizObject;
        //获取本流程的实例
        instance = this.Engine.WorkflowInstanceManager.GetWorkflowInstance(thisObj.WorkflowInstanceId);
        //转换工艺配置为布尔值
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
        if (!this.Request.IsCreateMode)
        {
            //当前流程未结束
            if (instance.IsUnfinished)
            {
                //查询工件是否为第一次流入《热处理》工序，如果不是那么就是“重处理”
                IsRehandle();
                //根据当前工件流程追溯工序计划数据
                H3.DataModel.BizObject planObject = SearchPlanningData(instance);
                //加载工艺配置数据
                LoadingProcessConfiguration(planObject, boolConfig);
                //加载质量配置数据
                LoadingQAConfiguration(planObject, boolConfig);

                //当前工序
                thisObj[HeatTreatment.CurrentOperation] = "热处理";
                //更新本表单
                thisObj.Update();
            }

            try
            {       //同步数据至实时制造情况
               // DataSync.instance.RCLSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }
        }
    }



    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        var me = new Schema(this.Engine, "热处理");

        //发起异常 是
        if (this.Request.BizObject["F0000040"] == "是")
        {
            this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
        }

        abnormalStep(me, postValue, response);
        var cmd = "";

        if (postValue != null && postValue.Data != null && postValue.Data.ContainsKey("cmd"))
        {
            cmd = postValue.Data["cmd"] + string.Empty;
        }
        DispatchCommand(actionName, postValue, response, cmd);
    }
    /*
    <summary>
    关于发起异常之后节点中内容的显示与隐藏及默认值，包含“确认调整意见”，“审批确认”节点。
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    */
    protected void abnormalStep(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        var row = me.GetRow(this.Request.BizObjectId);
        //  var postValue = me.CurrentPostValue;
        var code = this.Request.ActivityCode;
        var actName = this.Request.ActivityTemplate.DisplayName;

        //发起异常
        var startException = this.Request.BizObject["F0000040"] + string.Empty;
        if (startException == "是")
        {
            createLog(me, postValue);
        }

        if (code == "Activity127")
        {
            updateLog(me, postValue);
        }
        if (code == "Activity128")
        {
            me.Cell("发起异常", "否");
            // me.Cell("转至工步", "待转运 ");
            me.Cell("异常描述", "操作错误，重新选择节点 ");
            me.Cell("异常类别", "安全异常 ");
            me.Update(false);
        }
    }


    //分别处理命令，该命令在列表中定义，且由弹出的表单传递至该页面。如果需要区分用户意愿按钮，则需要处理actionName所对应的各种情况。
    protected void DispatchCommand(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response, string cmd)
    {
        bool flag = false;
        var me = new Schema(this.Engine, "热处理");
        switch (cmd)
        {
            case "Dispatch":
                //if(actionName == "Submit") { Dispatch(me, response); }
                base.OnSubmit(actionName, postValue, response);
                break;
            case "BtnOpenNew":
                response.ClosePage = false;
                base.OnSubmit(actionName, postValue, response);
                break;
            case "BatchModify":
                //BatchModify(me, response);
                base.OnSubmit(actionName, postValue, response);
                break;
            default:
                if (actionName == "Submit")
                {
                    //设置权限
                    if (this.Request.BizObject["F0000040"] == "是")
                    {
                        this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
                    }

                    //流转结论
                    // liuzhuanResult(me, postValue, response);
                    //轧制方式

                    string zhaZhiStyle = this.Request.BizObject["F0000031"].ToString();
                    //双轧同步校验
                    if (String.Compare(zhaZhiStyle, "双轧") == 0)
                    {
                        doubleZhaCheck(me, postValue, response);
                    }
                    //异常工步节点日志
                    base.OnSubmit(actionName, postValue, response);

                    abnormalStep(me, postValue, response);
                    break;
                }


                if (actionName == "GetProductObjectID")
                {
                    //GetProductObjectID(me, response);
                    base.OnSubmit(actionName, postValue, response);
                    break;
                }
                base.OnSubmit(actionName, postValue, response);
                break;
        }
    }

    /*
    <summary>
    双扎检验   如果双扎中 同一个双扎编号的产品  在一个工序中进行提交的时候  如果同编号的另一件在同一个工步 可提交 否则不可以提交
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    */
    protected void doubleZhaCheck(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //是否弹框flag
        var flag = false;
        var otherID = "";
        var Code = this.Request.ActivityCode;
        var actName = this.Request.ActivityTemplate.DisplayName;
        //双轧编号 
        var doubleZhaNumber = me.PostValue("双轧编号");
        //ABC订单批次规格号
        var ABCNumber = me.PostValue("订单批次规格号");
        //当前ID 
        var currentID = me.PostValue("ID");
        //热处理
        var scmJuQie = new Schema(this.Engine, "热处理");
        //找到同母体毛坯的另一件id
        var idQuery = scmJuQie.ClearFilter()
            .And("订单批次规格号", "=", ABCNumber)
            .And("双轧编号", "=", doubleZhaNumber)
            .And("ID", "!=", currentID)
            .GetFirst(true);

        //如果查询到
        if (idQuery != null)
        {
            otherID = scmJuQie.Cell("ID");
            var ObjectId = scmJuQie.Cell("ObjectId");
            //scmJuQie.Cell("测试用ID", otherID);
            scmJuQie.Cell("测试用objectID", ObjectId);

            //查询流程工作项表InstanceId
            string SelSql = "select InstanceId from H_WorkItem  where BizObjectId = '" + ObjectId + "'";
            DataTable SelDt = this.Request.Engine.Query.QueryTable(SelSql, null);
            if (SelDt != null && SelDt.Rows.Count > 0)
            {
                string instanceId = "";
                //循环数据行
                foreach (DataRow item in SelDt.Rows)
                {
                    //获取查询结果 当前行InstanceId字段值
                    instanceId = item["InstanceId"] + string.Empty;
                }

                if (!string.IsNullOrEmpty(instanceId))
                {
                    //查询流程步骤表
                    string sqlStep = "select * from H_Token  where ParentObjectId = '" + instanceId + "'";
                    DataTable sqlStepDt = this.Request.Engine.Query.QueryTable(sqlStep, null);
                    if (sqlStepDt != null && sqlStepDt.Rows.Count > 0)
                    {
                        //循环数据行
                        foreach (DataRow itemStep in sqlStepDt.Rows)
                        {
                            string activityCode = itemStep["Activity"] + string.Empty;
                            if (Code == activityCode)
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            else
            {

                //查询结果为空

            }


            scmJuQie.Update(false);
        }

        //双轧的另一件产品没有经过这个节点
        if (flag == false)
        {
            //弹出报错窗口
            response.Errors.Add("请把" + otherID + "这件产品提交到此工步再进行提交");
            //response.Message = "请把"+otherID+"这件产品提交到此工步再进行提交"; //弹出成功消息
        }
        //未查询到
    }
    /*
    <summary>
    把本表单的以下信息同步到《异常工步记录表中》。
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    */
    public void createLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {

        var scmAbnormalType = new Schema(this.Engine, "异常工步记录表");
        scmAbnormalType.GetNew();
        //当前工序
        var currentProcess = me.PostValue("当前工序");
        //  当前工步
        var currentWorkStep = me.PostValue("当前工步");
        //异常类别
        var abNormalType = me.PostValue("异常类别");
        //异常类别
        // var abNormalType = this.Request.BizObject["F0000049"]+ string.Empty;
        //ID
        var ID = me.PostValue("ID");
        //异常描述
        //var abNormalDescibe = me.PostValue("异常描述");

        //ID
        scmAbnormalType.Cell("ID", ID);

        //工步来源
        scmAbnormalType.Cell("工步来源", currentWorkStep);
        //工序来源
        scmAbnormalType.Cell("工序来源", currentProcess);

        //异常类别
        scmAbnormalType.Cell("异常类别", abNormalType);
        //异常描述
        //scmAbnormalType.Cell("异常描述", abNormalDescibe);

        scmAbnormalType.Create(true);
    }
    /*
    <summary>
    根据本表单的修改更新到,《异常工步记录表》。
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    */
    public void updateLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {
        //异常工步记录表
        var scmAbnormalType = new Schema(this.Engine, "异常工步记录表");
        //ID
        var ID = me.PostValue("ID");
        //  当前工步
        var currentWorkStep = me.PostValue("当前工步");
        var exceptDscribe = me.PostValue("异常描述");
        scmAbnormalType.ClearFilter()
            .And("ID", "=", ID)
            .And("工步来源", "=", currentWorkStep)
            .GetFirst(true);
        scmAbnormalType.Cell("异常描述", exceptDscribe);
        scmAbnormalType.Update(true);
    }

    /*
     *--Author:fubin
     * 加载质量配置数据
     * @param planObj 工序计划数据
     * @param boolConfig 布尔值字典
     */
    protected void LoadingQAConfiguration(H3.DataModel.BizObject planObj, Dictionary<string, bool> boolConfig)
    {
        if (planObj == null) { return; }
        //质量配置表
        string sql = string.Format("Select * from i_D0014198feb957936e040648d486b034af96597");
        DataTable qcForm = this.Engine.Query.QueryTable(sql, null);
        //读取装炉前检验在《质量配置表》中的优先层级
        string heatTreatment = qcForm.Rows[0][QAConfig.PriorityLevelInspectionBeforeCharging] + string.Empty;
        //读取《质量配置表》装炉前检验配置
        string globalHeatTreatment = qcForm.Rows[0][QAConfig.GlobalInspectionBeforeCharging] + string.Empty;
        //读取《工序计划表》装炉前检验配置
        string planHeatTreatment = planObj[ABCDProcessPlan.GlobalInspectionBeforeFurnaceLoading] + string.Empty;

        if (heatTreatment == "配置表")
        {   //全局装炉前检验
            thisObj[HeatTreatment.CheckBeforeLoading] = boolConfig[globalHeatTreatment];
        }

        if (heatTreatment == "计划表")
        {

            if (planHeatTreatment != string.Empty)
            {   //计划装炉前检验
                thisObj[HeatTreatment.CheckBeforeLoading] = boolConfig[planHeatTreatment] + string.Empty;
            }
            else
            {   //全局装炉前检验
                thisObj[HeatTreatment.CheckBeforeLoading] = boolConfig[globalHeatTreatment] + string.Empty;
            }
        }

    }

    /*
     *--Author:fubin
     * 加载工艺配置数据
     * @param planObj 工序计划数据
     * @param boolConfig 布尔值字典
     */
    protected void LoadingProcessConfiguration(H3.DataModel.BizObject planObj, Dictionary<string, bool> boolConfig)
    {
        if (planObj == null) { return; }
        //工艺配置表
        string mySql = string.Format("Select * from i_D0014194755c7eecbe9410c84cf6640d9cb147b");
        DataTable jzForm = this.Engine.Query.QueryTable(mySql, null);
        //获取工艺配置精整优先级层级
        string finishing = jzForm.Rows[0][ProcessConfig.PriorityLevelFinishing] + string.Empty;
        //读取《配置表》精整配置
        string globalFinishing = jzForm.Rows[0][ProcessConfig.GlobalFinishingConfiguration] + string.Empty;

        string planObjId = planObj[ABCDProcessPlan.OrderSpecificationTable] + string.Empty;
        //加载对应订单规格表的数据
        H3.DataModel.BizObject acObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
            this.Engine, "D001419Skniz33124ryujrhb4hry7md21", planObjId, false);
        //读取《产品表》精整配置
        string productFinishing = acObj != null ? acObj[OrderSpecification.ProductFinishingConfiguration] + string.Empty : string.Empty;

        //读取《计划表》精整设置
        string planFinishing = planObj[ABCDProcessPlan.SinglePieceIgnorePhysicochemicalResults] + string.Empty;

        if (finishing == "配置表")
        {
            //全局精整配置
            thisObj[HeatTreatment.IsFinishing] = boolConfig[globalFinishing] + string.Empty;
        }

        if (finishing == "产品表")
        {
            if (productFinishing != string.Empty)
            {
                //产品精整配置
                thisObj[HeatTreatment.IsFinishing] = boolConfig[productFinishing] + string.Empty;
            }
            else
            {
                //全局精整配置
                thisObj[HeatTreatment.IsFinishing] = boolConfig[globalFinishing] + string.Empty;
            }
        }

        if (finishing == "计划表")
        {
            if (planFinishing != string.Empty)
            {
                //计划精整配置
                thisObj[HeatTreatment.IsFinishing] = boolConfig[planFinishing] + string.Empty;
            }
            else
            {
                if (productFinishing != string.Empty)
                {
                    //产品精整配置
                    thisObj[HeatTreatment.IsFinishing] = boolConfig[productFinishing] + string.Empty;
                }
                else
                {
                    //全局精整配置
                    thisObj[HeatTreatment.IsFinishing] = boolConfig[globalFinishing] + string.Empty;
                }
            }
        }
    }

    /*
     *--Author:fubin
     * 根据当前工件流程追溯工序计划数据
     * @param instance 当前工件流程
     */
    protected H3.DataModel.BizObject SearchPlanningData(H3.Workflow.Instance.WorkflowInstance instance)
    {
        //获取父流程实例对象
        instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);

        var parentId = instance != null ? instance.BizObjectId : "";
        //获取父流程业务对象
        H3.DataModel.BizObject parentObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Sq0biizim9l50i2rl6kgbpo3u4", parentId, false);

        string planId = parentObj != null ? parentObj[ProcessFlow.OperationSchedule] + string.Empty : string.Empty;
        //获取工序计划业务对象
        H3.DataModel.BizObject planObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Szlywopbivyrv1d64301ta5xv4", planId, false);

        return planObj;
    }

    /*
     *--Author:fubin
     * 查询工件是否为第一次流入《热处理》工序，如果不是那么就是“重处理”
     */
    protected void IsRehandle()
    {
        //根据工件ID查询本工序所有的数据
        string myCmd = string.Format("Select * from i_D001419Siizvpn3x17wj6jj3pifsmbic3 where '{0}'= '{1}'", HeatTreatment.ID, thisObj[HeatTreatment.ID] + string.Empty);
        DataTable objCount = this.Engine.Query.QueryTable(myCmd, null);
        if (thisObj[HeatTreatment.TaskName] + string.Empty == string.Empty)
        {   //查询到一条数据
            if (objCount.Rows.Count == 1)
            {
                thisObj[HeatTreatment.TaskName] = "热处理";
            } //查询到多条数据
            if (objCount.Rows.Count > 1)
            {
                thisObj[HeatTreatment.TaskName] = "重处理";
            }

        }
    }

}