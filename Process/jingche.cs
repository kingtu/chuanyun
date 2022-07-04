using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using H3;
using H3.DataModel;

public class D001419Sqy2b1uy8h8cahh17u9kn0jk10 : H3.SmartForm.SmartFormController
{
    string activityCode;                            //活动节点编码
    H3.DataModel.BizObject me;                      //本表单业务对象
    H3.SmartForm.SmartFormResponseDataItem item;    //用户提示信息
    H3.SmartForm.SmartFormResponseDataItem userId;  //用户Id
    string info = string.Empty;                     //值班信息
    string userName = "";                           //当前用户
    public D001419Sqy2b1uy8h8cahh17u9kn0jk10(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;                        //本表单业务对象
        activityCode = this.Request.ActivityCode;           //活动节点编码
        item = new H3.SmartForm.SmartFormResponseDataItem();//用户提示信息
        userName = this.Request.UserContext.User.FullName;  //当前用户
        userId = new H3.SmartForm.SmartFormResponseDataItem();//用户Id
        userId.Value = this.Request.UserContext.UserId;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            //不是创建模式
            if (!this.Request.IsCreateMode)
            {
                //清空父流程的转至工步与转至工序
                ClearTargetSection();
                //清空转至工步信息
                ClearTargetStep();
                //初始化控件
                InitTableComponent();
                //流程处于未完成状态
                if (this.Request.WorkflowInstance.IsUnfinished)
                {
                    //统计机加工耗时
                    MachiningTime();
                    //初始化探伤表
                    InitFlawDetectionForm();
                    //获取工序计划表数据
                    H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
                }
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.FinishingProgress(this.Engine, Finishing_TableCode, CurrentWorkStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[CurrentWorkStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        //返回前端信息
        ReturnDataInfo(response);
        base.OnLoad(response);

        //--------------------------加载前后分割线-------------------------//

        try
        {
            if (!this.Request.IsCreateMode)
            {
                //加载后代码
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //发起异常
            string strInitiateAbnormal = me[InitiateFlawDetection] + string.Empty;
            //提交
            if (actionName == "Submit")
            {
                //派工开关
                string dipatchFlag = this.Request.BizObject[DispatchingSwitch] + string.Empty;
                if (dipatchFlag == "开")
                {
                    //派工逻辑
                    DispatchLogic.PullDispatch(this.Engine, activityCode, me, (string)userId.Value, actionName);
                    //加载精车钻孔工序的派工信息
                    LoadDrillDispatchInfo();
                }
                //多阶段加工流程逻辑
                MultistageProcessingLogic(activityCode);
                //校验异常信息是否与数据库保持一致
                bool checkedResult = CheckExceptionInfo(response);
                if (checkedResult) { return; }
                //赋值审批来源
                UnqualifiedSource();
                //计算下一工序是否转运
                WhetherTransshipment();
                base.OnSubmit(actionName, postValue, response);
            }
            //下机时允许刷新派工量、工时、下屑量
            if (actionName == "RefreshDisInfo" && activityCode == "Activity24")
            {
                //加工中刷新派工量信息
                RefreshDisInfo(actionName, activityCode, response);
                base.OnSubmit(actionName, postValue, response);
            }
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

        //-------------------------提交前后分割线-----------------------//

        try
        {
            //异常工步
            AbnormalStep();
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);//负责人信息
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }


    /*
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTargetSection()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance parentInstance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject parentInstanceObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, parentInstance.SchemaCode, parentInstance.BizObjectId, false);
        parentInstanceObj[ProcessFlow_TargetSection] = null;        //转至工序
        parentInstanceObj[ProcessFlow_TargetStep] = null;           //转至工步 
        parentInstanceObj.Update();
    }

    /*
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void DeleteTargetStep()
    {
        if (activityCode == "Activity55")
        {
            //获取当前流程业务对象
            H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
            currentObj[InitiateAbnormal] = "否";                      //发起异常
            currentObj[TargetStep] = null;                            //转至工步
            currentObj[AbnormalCategory] = null;                      //异常类别
            currentObj[AbnormalDescription] = null;                   //异常描述
            currentObj[AbnormalRepresentative] = null;                //异常代表
            currentObj[ProcessAdjustmentRange] = "否";                //是否调整至其他工序
            currentObj[AssociatedWithOtherAbnormalWorkpieces] = null; //关联其它异常工件
            currentObj[MachiningQualityTreatment] = null;             //机加质量处理
            currentObj[DemandApprovalForm] = null;                    //需求审批单
            currentObj[TransferApprovalForm] = null;                  //流转审批单
            currentObj[OtherApprovalDocuments] = null;                //其它审批单
            currentObj[ApprovalSource] = null;                        //审批来源
            currentObj.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = this.Request.UserContext.User.Name;   //当前审批人
        string currentProcess = me[CurrentSection] + string.Empty;     //当前工序
        string currentWorkStep = me[CurrentWorkStep] + string.Empty;   //当前工步
        //发起异常  否
        if (me[InitiateAbnormal] + string.Empty == "是" && activityCode != "Activity55")
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me[ApprovalSource] = sourceOfApproval;                     //审批来源
            me.Update();
        }
        //检验结果  不合格  发起异常  否
        if (me[InspectionResult] + string.Empty == "不合格" && me[InitiateAbnormal] + string.Empty == "否")
        {
            string results = "自检结果不合格";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results;
            me[ApprovalSource] = sourceOfApproval;                     //审批来源
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 计算下一工序是否转运
    */
    protected void WhetherTransshipment()
    {
        //节点名称是否为 精车中 完成总量=1
        if (activityCode == "Activity24" && me[TotalAmountCompleted] + string.Empty == "1")
        {
            bool flag = true;
            if (me[LogisticsSwitch] + string.Empty == "开")//物流开关
            {
                //工序表钻孔派工子表信息
                H3.DataModel.BizObject[] disProcessSubs = me[FinishingDrillingDispatchingInformation_TableCode] as H3.DataModel.BizObject[];
                //判断工序表钻孔派工子表有没有值
                if (disProcessSubs != null && disProcessSubs.Length > 0)
                {
                    for (int i = 0; i < disProcessSubs.Length; i++)
                    {
                        //判断“任务状态” 是否等于 “已完成”
                        if (disProcessSubs[i][FinishingDrillingDispatchingInformation_TaskStatus] + string.Empty != "已完成")
                        {
                            //判断子表信息的车间位置与现在工件所在的车间位置是否一致
                            if (disProcessSubs[i][FinishingDrillingDispatchingInformation_DrillingWorkshopName] + string.Empty != me[WorkshopLocation] + string.Empty)
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        //钻孔是否需要转运
                        me[WhetherDrillingHolesNeedToBeTransferred] = "否";
                    }
                }
                me.Update();
            }
            if (me[LogisticsSwitch] + string.Empty == "关")//物流开关
            {
                //钻孔是否需要转运
                me[WhetherDrillingHolesNeedToBeTransferred] = "是";
                me.Update();
            }
        }
    }


    /*
    * Author:zzx
    *  加工中刷新派工量、工时、下屑量信息
    * @param actionName    表单按钮编码
    * @param activityCode  流程节点编码
    * @param response      流程响应数据
    */
    public void RefreshDisInfo(string actionName, string activityCode, H3.SmartForm.SubmitSmartFormResponse response)
    {
        string[] dispatchInfo = DispatchLogic.PullDispatch(this.Engine, activityCode, me, (string)userId.Value, actionName); //获取最新派工量、工时、下屑量
        Dictionary<string, object> resData = new Dictionary<string, object>();
        if (dispatchInfo != null && dispatchInfo.Length == 4)
        {
            resData.Add("disQuantity", dispatchInfo[0]); //返回最新派工量、工时、下屑量
            resData.Add("manHour", dispatchInfo[1]);
            resData.Add("chipQuantity", dispatchInfo[2]);
            resData.Add("objectId", dispatchInfo[3]);
        }
        response.ReturnData = resData;
    }
    /**
    * Auther：zzx
    * 返回前端的信息
    */
    private void ReturnDataInfo(H3.SmartForm.LoadSmartFormResponse response)
    {
        response.ReturnData.Add("key1", item);
        response.ReturnData.Add("userId", userId);
    }
    /**
    * Author: zzx
    * 初始化控件
    */
    public void InitTableComponent()
    {
        if (me[CurrentSection] + string.Empty == string.Empty)//当前工序
        {
            me[CurrentSection] = "精车";
        }
        H3.DataModel.BizObject[] thisLstArray = me[FinishMachiningSubTable_TableCode] as H3.DataModel.BizObject[];//获取多阶段加工子表
        if (thisLstArray == null || (bool)thisLstArray[thisLstArray.Length - 1][FinishMachiningSubTable_DispatchQuantityAlteration] == true)
        {
            CreatSublist(me);//初始化子表
        }
        me[TaskNumber] = me[TaskNumber] + string.Empty != string.Empty ? me[TaskNumber] + string.Empty : "1";//初始化任务名称
        me[BackToTheComputer] = "否";                                                                        //回至上机
        if (me[ProcessingDifficulty] + string.Empty == "") { me[ProcessingDifficulty] = 1; }                  //加工难度设置默认值1。
        if (me[TotalAmountCompleted] + string.Empty == string.Empty)                                          //完成总量
        {
            me[TotalAmountCompleted] = 0;
        }
        me.Update();//更新本表单

    }

    /**
    * --Author: nkx
    * 精车钻孔工序的派工信息赋值
    * 
    */
    public void LoadDrillDispatchInfo()
    {
        //完成总量等于1时，读取下一工序的派工信息
        if (me[TotalAmountCompleted] + string.Empty == "1")
        {
            //读取下一工序的派工信息
            DispatchLogic.DispatchLogicDrill(this.Engine, me, activityCode);

        }
    }
    /*
    * --Author: zzx
    * 清空转至工步信息
    */
    public void ClearTargetStep()
    {
        //正常节点 转至工步复位
        if (activityCode != "Activity55")
        {
            me[TargetStep] = "";
        }
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateAbnormal = me[InitiateAbnormal] + string.Empty;
        if (strInitiateAbnormal == "是")
        {
            return false;
        }
        //本表单业务对象
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        string sqlInitiateAbnormal = thisObj[InitiateAbnormal] + string.Empty;//数据库中发起异常的值
        if (strInitiateAbnormal != sqlInitiateAbnormal)
        {
            response.Message = "异常数据有更新，请刷新页面！";//发起异常不为是时执行与数据库值是否一致的校验
            return true;
        }
        else
        {
            return false;//与数据库相同
        }
    }

    /*
    *--Author:fubin
    * 查询更新机加工耗时
    */
    protected void MachiningTime()
    {
        string bizid = me.ObjectId;
        //查询精车加工中所有耗时
        string command = string.Format
            ("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_{0}" +
            " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity24' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", Finishing_TableCode, bizid);
        DataTable data = this.Engine.Query.QueryTable(command, null);
        //机加工耗时计算
        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                double utime = double.Parse(utimestr) / 10000000 / 60;  //转换时间单位为秒
                me[ActualProcessingTime] = utime;                       //实际加工耗时
            }
        }
    }

    /*
    *--Author:fubin
    * 初始化探伤表
    */
    protected void InitFlawDetectionForm()
    {
        //探伤表数据Id
        string flawDetectionId = me[FlawDetectionTable] + string.Empty;
        //流程节点名称
        string activityName = this.Request.WorkItem.ActivityDisplayName;
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (flawDetectionId == string.Empty)
        {
            //ID
            string thisId = me[ID] + string.Empty;
            //sql语句  查询
            string mySql = string.Format("Select {0} From i_{1} Where {2} = '{3}'", FlawDetectionTable_Objectid, FlawDetectionSubTable_TableCode, FlawDetectionTable_ID, thisId);
            DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                //探伤表Id
                me[FlawDetectionTable] = tsData.Rows[0][FlawDetectionTable_Objectid] + string.Empty;
                me.Update();
            }
        }
        //探伤表不为空时,写入工序信息
        if (flawDetectionId != string.Empty)
        {
            //获取探伤表业务对象
            H3.DataModel.BizObject flawDetectionTableObj = Tools.BizOperation.Load(this.Engine, FlawDetectionSubTable_TableCode, flawDetectionId);
            //获取探伤子表
            H3.DataModel.BizObject[] flawDetectionSubTableObj = flawDetectionTableObj[FlawDetectionTable_FlawDetectionRecord] as H3.DataModel.BizObject[];
            flawDetectionTableObj[FlawDetectionTable_CurrentSection] = "精车";//工序
            flawDetectionTableObj[FlawDetectionTable_Finishing] = me.ObjectId;//精车关联表单
            //探伤结果  为空
            if (flawDetectionSubTableObj[flawDetectionSubTableObj.Length - 1][FlawDetectionTable_ThisFlawDetectionResult] + string.Empty == string.Empty)
            {
                flawDetectionSubTableObj[flawDetectionSubTableObj.Length - 1][FlawDetectionTable_Section] = "精车";//工序
                flawDetectionSubTableObj[flawDetectionSubTableObj.Length - 1][FlawDetectionTable_WorkStep] =
                        activityName != "待探伤" ? activityName : me[CurrentWorkStep] + string.Empty;              //工步
                flawDetectionSubTableObj[flawDetectionSubTableObj.Length - 1].Update();
            }
            flawDetectionTableObj.Update();
        }
    }

    /*
    *--Author:fubin
    * 多阶段加工流程逻辑
    * @param activityCode 流程节点编码
    */
    private void MultistageProcessingLogic(string activityCode)
    {
        //精车机加工子表
        H3.DataModel.BizObject[] finishingMachiningInformationObj = me[FinishingMachiningInformation] as H3.DataModel.BizObject[];
        //修正任务数-----任务名称
        me[TaskNumber] = finishingMachiningInformationObj != null ? finishingMachiningInformationObj.Length + string.Empty : "1";
        //获取任务数
        int taskNum = me[TaskNumber] + string.Empty != string.Empty ? int.Parse(me[TaskNumber] + string.Empty) - 1 : 0;
        //精车上机
        if (activityCode == "Activity3")
        {
            //当前加工者
            finishingMachiningInformationObj[taskNum][FinishMachiningSubTable_Processor] = this.Request.UserContext.UserId;
            //加工开始时间  
            finishingMachiningInformationObj[taskNum][FinishMachiningSubTable_StartTime] = System.DateTime.Now;
        }
        //精车下机
        if (activityCode == "Activity24")
        {
            //完成总量小于1时
            if ((me[TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[TotalAmountCompleted] + string.Empty) < 1)
            {
                //递增计数器，并更新
                me[TaskNumber] = finishingMachiningInformationObj.Length + 1;
                //创建添加新的子表行数据
                CreatSublist(me);
            }
            //精车机加工子表  加工者  为空
            if (finishingMachiningInformationObj[taskNum][FinishMachiningSubTable_Processor] + string.Empty == string.Empty)
            {
                //当前加工者
                finishingMachiningInformationObj[taskNum][FinishMachiningSubTable_Processor] = this.Request.UserContext.UserId;
            }
        }
    }

    /*
    *--Author:fubin
    * 创建添加新的子表行数据
    * @param thisObj 本表单数据
    */
    protected void CreatSublist(H3.DataModel.BizObject thisObj)
    {
        //new一个子表业务对象
        H3.DataModel.BizObject roughMachiningSubtableObj = Tools.BizOperation.New(this.Engine, FinishingMachiningInformation);
        //任务名称
        roughMachiningSubtableObj[FinishMachiningSubTable_TaskNumber] = thisObj[TaskNumber] + string.Empty == string.Empty ? "1" : thisObj[TaskNumber] + string.Empty;
        //派工开关
        string dipatchFlag = this.Request.BizObject[DispatchingSwitch] + string.Empty;
        if (dipatchFlag == "关")
        {
            roughMachiningSubtableObj[FinishMachiningSubTable_TaskName] = "默认派工任务";
        }
        roughMachiningSubtableObj.Update();
        //将这个子表业务对象添加至子表数据集合中
        Tools.BizOperation.AddChildBizObject(this.Engine, thisObj, FinishingMachiningInformation, roughMachiningSubtableObj);
    }
    //检查发起异常控件是否被其它异常代表更改 - fubin
    protected bool checkExceptionInfo()
    {
        //表单发起异常
        string strInitiateAbnormal = me[InitiateAbnormal] + string.Empty;
        //本表单业务对象
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库发起异常
        string sqlInitiateAbnormal = thisObj[InitiateAbnormal] + string.Empty;
        if (strInitiateAbnormal != sqlInitiateAbnormal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作
    */
    protected void AbnormalStep()
    {
        //关联其它异常工件
        String[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID         
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, RealTimeDynamicProduction_TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = currentObj[RealTimeDynamicProduction_SectionTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = currentObj[RealTimeDynamicProduction_CurrentPreviousSectionTableSchemacode] + string.Empty;
            //加载工序表中的业务对象
            H3.DataModel.BizObject otherObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, currentSchemaCode, otherExceptionId, false);
            //父流程实例ID
            string parentInstanceId = this.Request.WorkflowInstance.ParentInstanceId;
            //获取父流程实例对象
            H3.Workflow.Instance.WorkflowInstance parentInstance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(parentInstanceId);
            //传递异常信息
            foreach (H3.DataModel.PropertySchema activex in otherObj.Schema.Properties)
            {
                if (activex.DisplayName.Contains("发起异常"))
                {
                    otherObj[activex.Name] = "是";
                }

                if (activex.DisplayName.Contains("异常类别"))
                {
                    otherObj[activex.Name] = me[AbnormalCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = me[ID];
                }
            }

            otherObj.Update();
        }
        //本表单业务对象
        H3.DataModel.BizObject abnormalBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, Finishing.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity127" && strActivityCode != "Activity128")
        {
            //设置异常权限
            abnormalBo[Owner] = this.Request.UserContext.UserId;

        }
        DeleteTargetStep();
    }
    /**
    * --Author: zzx
    * 派工开关设置粗车中审批人
    */
    protected void DipatchFlag()
    {
        //派工开关
        string dipatchFlag = this.Request.BizObject[DispatchingSwitch] + string.Empty;
        if (dipatchFlag == "关")
        {
            //判断是否为待上机节点
            if (activityCode == "Activity24")
            {
                //精车阶段加工中人为  上机权限人
                this.Request.BizObject[DrillingStageProcessingPeople] = (string)userId.Value;
            }
        }
    }

    string FinishingDrillingDispatchingInformation_TableCode = "D001419F06c8fa4adf4c443f927667fb6f01a714";  //精车钻孔派工信息
    string FinishingDrillingDispatchingInformation_TaskStatus = "F0000244";                                 //任务状态
    string FinishingDrillingDispatchingInformation_DrillingWorkshopName = "F0000227";                       //钻孔车间名称

    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";    //工艺流程表
    string ProcessFlow_TargetSection = "F0000056";                         //转至工序
    string ProcessFlow_TargetStep = "F0000057";                            //转至工步

    string Finishing_TableCode = "D001419Sqy2b1uy8h8cahh17u9kn0jk10";      //精车
    string WorkshopLocation = "F0000066";                                  //车间位置
    string CurrentWorkStep = "F0000068";                                   //当前工步
    string CurrentSection = "F0000069";                                    //当前工序
    string TotalAmountCompleted = "F0000086";                              //完成总量
    string InitiateFlawDetection = "F0000168";                             //发起探伤
    string TaskNumber = "F0000118";                                        //任务序号
    string ProcessingDifficulty = "F0000188";                              //加工难度
    string TargetStep = "F0000117";                                        //转至工步
    string InitiateAbnormal = "F0000059";                                  //发起异常
    string InspectionResult = "F0000018";                                  //检验结果
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";             //关联其他异常工件
    string AbnormalCategory = "F0000055";                                  //异常类别
    string AbnormalDescription = "F0000115";                               //异常描述
    string AbnormalRepresentative = "F0000176";                            //异常代表
    string ProcessAdjustmentRange = "F0000065";                            //（异常后）是否跨工序调整流程
    string ID = "F0000053";                                                //ID
    string Owner = "OwnerId";                                              //拥有者
    string ActualProcessingTime = "CountTime";                             //实际加工耗时
    string FlawDetectionTable = "F0000167";                                //探伤表Id
    string DispatchingSwitch = "F0000265";                                 //派工开关
    string LogisticsSwitch = "F0000267";                                   //物流开关
    string DrillingStageProcessingPeople = "F0000073";                     //精车阶段加工中人
    string BackToTheComputer = "F0000213";                                 //回至上机
    string ApprovalSource = "F0000260";                                    //审批来源
    string MachiningQualityTreatment = "F0000256";                         //机加质量处理
    string DemandApprovalForm = "F0000257";                                //需求审批单
    string TransferApprovalForm = "F0000258";                              //流转审批单
    string OtherApprovalDocuments = "F0000259";                            //其它审批单
    string WhetherDrillingHolesNeedToBeTransferred = "F0000254";           //钻孔是否需要转运

    string FinishingMachiningInformation = "D001419Fd25eb8064b424ed9855ced1923841f1c";  //精车机加工子表
    string Finishing_StartTime = "F0000164";                                            //开始时间
    string Finishing_Processor = "F0000157";                                            //加工者

    string FlawDetectionSubTable_TableCode = "D001419fdcaecf556264750ae2d5684b2a3706e";            //探伤表tableCode
    string FlawDetectionTable_Objectid = "ObjectId";                                               //探伤表Objectid
    string FlawDetectionTable_ID = "F0000001";                                                     //ID
    string FlawDetectionTable_CurrentSection = "F0000023";                                         //当前工序
    string FlawDetectionTable_Finishing = "F0000026";                                              //精车
    string FlawDetectionTable_FlawDetectionRecord = "D001419F89050d4fc56d4bf7b41f343f2e3bd5a1";    //探伤记录

    string FlawDetectionTable_TableCode = "D001419F89050d4fc56d4bf7b41f343f2e3bd5a1"; //探伤子表
    string FlawDetectionTable_ThisFlawDetectionResult = "F0000002";                   //本次探伤结果
    string FlawDetectionTable_Section = "F0000017";                                   //工序
    string FlawDetectionTable_WorkStep = "F0000018";                                  //工步



    string FinishMachiningSubTable_TableCode = "D001419Fd25eb8064b424ed9855ced1923841f1c"; //精车机加工子表
    string FinishMachiningSubTable_DispatchQuantityAlteration = "F0000242";                //派工量变更
    string FinishMachiningSubTable_Processor = "F0000143";                                 //加工者
    string FinishMachiningSubTable_StartTime = "F0000142";                                 //开始时间
    string FinishMachiningSubTable_TaskNumber = "F0000141";                                //任务计数
    string FinishMachiningSubTable_TaskName = "F0000229";                                  //派工任务


    string RealTimeDynamicProduction_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd"; //实时生产动态
    string RealTimeDynamicProduction_SectionTableDataID = "F0000070";                      //工序表数据ID
    string RealTimeDynamicProduction_CurrentPreviousSectionTableSchemacode = "F0000071";   //工序表数据ID


}