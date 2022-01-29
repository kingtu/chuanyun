
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;

public class D001419Sgx7flbvwu9r0u3hail6512uq4 : H3.SmartForm.SmartFormController
{
    //本表单数据
    H3.DataModel.BizObject me;
    //本表单纲目结构
    H3.DataModel.BizObjectSchema schema;
    //当前节点
    string activityCode;
    //布尔值转换
    Dictionary<string, bool> boolConfig;
    public D001419Sgx7flbvwu9r0u3hail6512uq4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        activityCode = this.Request.ActivityCode;
        me = this.Request.BizObject;
        schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {

            //初始化控件
            InitTableComponent();
            //产品类别更新
            ProductCategoryUpdate();

            if (this.Request.WorkflowInstance.IsUnfinished)
            {
                //获取工序计划表数据
                H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
                //加载质量配置数据
                LoadingQAConfiguration(planObj);
                //读取《工序计划表》计划本取
                me[RoughCast.PlanBookRetrieval] = planObj != null ? planObj[ABCDProcessPlan.PlanThisOptionTakes] + string.Empty : string.Empty;
            }

            //更新本表单
            me.Update();
        }
        base.OnLoad(response);

        try
        {   //同步数据至实时制造情况
            DataSync.instance.MPSyncData(this.Engine);
        }
        catch (Exception ex)
        {
            response.Errors.Add(System.Convert.ToString(ex));
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //提交
        if (actionName == "Submit")
        {
            //清空转至工步信息
            ClearTransferToWorkStep();
            //清空双扎分割前工件信息
            CllearSegmentation();
            //校验异常信息是否与数据库保持一致
            bool checkedResult = CheckExceptionInfo(response);
            if (checkedResult)
            {
                return;
            }
            Authority.Approver(this.Request);

            base.OnSubmit(actionName, postValue, response);
            //异常工步
            AbnormalStep();
            //待切割
            if (activityCode == "Activity36")
            {
                //轧制方式
                string strRollingMethod = this.Request.BizObject[RoughCast.RollingMethod].ToString();
                //双轧同步校验
                if (String.Compare(strRollingMethod, "双轧") == 0)
                {
                    DoubleRollingCheck(this.Request.BizObject, this.Request.Engine, RoughCast.TableCode, response);
                }
            }
        }
    }
    /**
    * --Author: zzx
    * 初始化控件
    * 
    */
    public void InitTableComponent()
    {
        //初始化当前工序
        if (me[RoughCast.CurrentOperation] + string.Empty == string.Empty) { me[RoughCast.CurrentOperation] = "毛坯"; }
        //更新本表单
        me.Update();
    }
    /**
    /**
           * --Author: zzx
           * 清空转至工步信息。
           * 
    */
    public void ClearTransferToWorkStep()
    {             //正常节点 转至工步复位
        if (activityCode != "Activity100" && activityCode != "Activity101")
        {
            me[RoughCast.TransferToWorkStep] = null;
        }
    }
    /**
   * --Author: zzx
   * 双轧校验
   * 
*/
    public void DoubleRollingCheck(H3.DataModel.BizObject currentBizobject, H3.IEngine engine, string tableCode, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //是否弹框flag
        var flag = true;

        //双环关联表单   双轧另一件产品在工序计划表中的objectId
        string strABCDProcessPlanObjectId = currentBizobject[RoughCast.DoubleRolledAssociativeForm] + string.Empty;

        //加载工序计划表数据
        H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, ABCDProcessPlan.TableCode, strABCDProcessPlanObjectId, false);
        if (currentObj == null)
        {
            //弹出报错窗口
            response.Errors.Add("双轧关联表单为空");
            return;
        }
        //工序计划表中的  关联工艺流程表中的objectId
        string strProcessFlowTableId = currentObj[ABCDProcessPlan.ProcessFlowTable] + string.Empty;

        //双轧另一件ID
        string strID = currentObj[ABCDProcessPlan.ID] + string.Empty;
        //加载工艺流程表中的业务对象
        H3.DataModel.BizObject currentProcessFlowObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, ProcessFlow.TableCode, strProcessFlowTableId, false);
        if (currentProcessFlowObj == null)
        {
            //弹出报错窗口
            response.Errors.Add("工序计划表中的工艺流程表为空");
            return;
        }
        //获取objectID
        string strObjectId = currentProcessFlowObj[ProcessFlow.ObjectId] + string.Empty;

        //查询流程工作项表InstanceId
        string SelSql = "select * from H_WorkItem  where BizObjectId = '" + strObjectId + "'";
        DataTable SelDt = engine.Query.QueryTable(SelSql, null);
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
                string sqlStep = "select * from H_Token  where ParentObjectId = '" + instanceId + "' ";
                DataTable sqlStepDt = engine.Query.QueryTable(sqlStep, null);
                if (sqlStepDt != null && sqlStepDt.Rows.Count > 0)
                {
                    // // 查询数据条目 是否经过当前节点
                    // if(sqlStepDt.Rows.Count == 2)
                    // {
                    //     flag = true;
                    // }
                    //循环数据行 是否经过毛坯节点
                    foreach (DataRow itemStep in sqlStepDt.Rows)
                    {
                        string currentActivityCode = itemStep["Activity"] + string.Empty;
                        //
                        if (currentActivityCode == "Activity23")
                        {
                            flag = false;
                        }
                    }
                }
            }
        }
        //双轧的另一件产品没有到工艺流程表中的等待节点
        if (flag == true)
        {
            //弹出报错窗口
            response.Errors.Add("请把" + strID + "这件产品提交到毛坯工序再进行提交");
        }
    }

    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    * 
    */
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateException = me[RoughCast.InitiateException] + string.Empty;
        if (strInitiateException == "是")
        {
            return false;
        }
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库中发起异常的值
        string sqlInitiateException = thisObj[RoughCast.InitiateException] + string.Empty;
        if (strInitiateException != sqlInitiateException)
        {
            //发起异常不为是时执行与数据库值是否一致的校验
            response.Message = "异常数据有更新，请刷新页面！";
            return true;
        }
        else
        {
            //与数据库相同
            return false;
        }
    }
    /**
           * --Author: zzx
           * 关于发起异常之后各个节点进行的操作。
           * 
           */
    protected void AbnormalStep()
    {
        //发起异常
        string strInitiateException = me[RoughCast.InitiateException] + string.Empty;
        if (strInitiateException != "是") { return; }
        //关联其它异常工件
        String[] bizObjectIDArray = me[RoughCast.AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, RealTimeDynamicProduction.TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = currentObj[RealTimeDynamicProduction.OperationTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = currentObj[RealTimeDynamicProduction.CurrentPreviousOperationTableSchemacode] + string.Empty;

            //加载工序表中的业务对象
            H3.DataModel.BizObject otherObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, currentSchemaCode, otherExceptionId, false);

            //父流程实例ID
            string parentInstanceId = this.Request.WorkflowInstance.ParentInstanceId;
            ///获取父流程实例对象
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
                    otherObj[activex.Name] = me[RoughCast.ExceptionCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = parentInstance.BizObjectId;
                }
            }

            otherObj.Update();
        }

        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, RoughCast.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity100" && strActivityCode != "Activity101")
        {
            //设置异常权限
            this.Request.BizObject[RoughCast.Owner] = this.Request.UserContext.UserId;
            //创建发起异常的日志
            logObjectID = AbnormalLog.CreateLog(RoughCast.ID, RoughCast.CurrentWorkStep, RoughCast.CurrentOperation,
                RoughCast.ExceptionCategory, RoughCast.ExceptionDescription, this.Request.BizObject, this.Engine);
            exceptionBo[RoughCast.ObjectIDForUpdateTheExceptionLog] = logObjectID;
            exceptionBo.Update();
        }
        //确认调整意见
        if (strActivityCode == "Activity100")
        {
            //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
            AbnormalLog.UpdateLog(RoughCast.ID, RoughCast.CurrentWorkStep, RoughCast.ExceptionCategory,
                RoughCast.ExceptionDescription, this.Request.BizObject, exceptionBo[RoughCast.ObjectIDForUpdateTheExceptionLog] + string.Empty, this.Engine);
        }
        //审批确认
        if (strActivityCode == "Activity101")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[RoughCast.InitiateException] = "否";
            //异常描述赋值
            exceptionBo[RoughCast.ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[RoughCast.ExceptionCategory] = "安全异常";
            //异常代表
            exceptionBo[RoughCast.ExceptionRepresentative] = string.Empty;
            exceptionBo.Update();
        }
    }


    //质检结论值的由来（可删前端有写）
    // protected void Result(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    // {
    //     var Code = this.Request.ActivityCode;
    //     //理化结果
    //     if(Code == "Activity61")
    //     {
    //         var inspectionResults = this.Request.BizObject[RoughCast.InspectionResults] + string.Empty;
    //         this.Request.BizObject[RoughCast.QualityInspectionConclusion] = inspectionResults;
    //     }
    //     //待理化结果
    //     if(Code == "Activity262")
    //     {
    //         //理化结果
    //         var unqualified = this.Request.BizObject[RoughCast.PhysicalAndChemicalResults] + string.Empty;
    //         this.Request.BizObject[RoughCast.QualityInspectionConclusion] = unqualified;
    //     }

    // }

    /*
     *--Author:fubin
     * 加载质量配置数据
     * @param planObj 工序计划数据
     */
    protected void LoadingQAConfiguration(H3.DataModel.BizObject planObj)
    {
        //读取《质量配置表》是否忽略理化结果顺序
        string crossCheck = LoadingConfig.GetQualityConfigForm(this.Engine, QAConfig.PriorityLevelphysicochemical);
        //读取《质量配置表》全局忽略理化结果配置
        string globalCrossCheck = LoadingConfig.GetQualityConfigForm(this.Engine, QAConfig.GlobalIgnorePhysicochemicalResults);
        //读取《工序计划表》单件忽略理化结果配置
        string planCrossCheck = planObj != null ? planObj[ABCDProcessPlan.SinglePieceIgnorePhysicochemicalResults] + string.Empty : string.Empty;

        switch (crossCheck)
        {
            case "配置表":
                //全局忽略理化结果
                me[RoughCast.WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = globalCrossCheck;
                break;
            case "计划表":
                if (planCrossCheck != string.Empty)
                {   //单件忽略理化结果
                    me[RoughCast.WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = planCrossCheck;
                }
                else
                {   //全局忽略理化结果
                    me[RoughCast.WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = globalCrossCheck;
                }
                break;
        }

        //加载《订单规格表》数据
        H3.DataModel.BizObject productObj = LoadingConfig.GetProductData(this.Engine, planObj);
        //获取《订单规格表》试样尺寸配置
        string specimanSize = productObj != null ? productObj[OrderSpecification.SampleSize] + string.Empty : string.Empty;
        //赋值试样尺寸
        me[RoughCast.SampleType] = specimanSize;
    }

    /*
     *--Author:fubin
     * 产品类别为空时，查询产品参数表中的车加工类别
     */
    protected void ProductCategoryUpdate()
    {
        //产品类别更新
        if (me[RoughCast.ProductCategory] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = me[RoughCast.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable typeData = this.Engine.Query.QueryTable(mySql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[RoughCast.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                me[RoughCast.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
            }
        }
    }
    /*
     *--Author:nkx
     * 待检验提交时，清空双扎分割前工件信息
     */
    protected void CllearSegmentation()
    {
        if (activityCode == "Activity61")
        {
            me[RoughCast.DoubleTieTheWorkpieceBeforeSegmentation] = "";
        }
    }
}

