
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using H3;

public class D001419So3cw528p3w543tqpt12v28o31 : H3.SmartForm.SmartFormController
{
    string activityCode = "";       //活动节点   
    H3.DataModel.BizObject me;      //本记录
    string ProcessName = "锯切";    //本工序名称
    H3.SmartForm.SmartFormResponseDataItem message;  //用户提示信息
    string info = string.Empty;                   //值班信息
    string userName = "";                         //当前用户


    // 生产制造流程,锯切
    string SawCut_TableCode = "D001419So3cw528p3w543tqpt12v28o31";
    // 更新异常日志objectID
    string SawCut_ObjectIDForUpdateTheExceptionLog = "F0000080";
    // 车间位置
    string SawCut_WorkshopLocation = "F0000007";
    // 锯切需求期
    string SawCut_DemandPeriodOfThisProcedureSawing = "F0000097";
    // 版本号
    string SawCut_TheVersionNumber = "F0000106";
    // 部门代码
    string SawCut_DepartmentCode = "F0000107";
    // 区域位置
    string SawCut_ProductLocation = "F0000065";
    // 部门多选
    string SawCut_DepartmentOfAlternative = "F0000105";
    // 锯加工记录
    string SawCut_SawProcessingRecord = "D001419Fff99042207274f8da1c422c807b2e7f0";
    // 错误消息
    string SawCut_TheErrorMessage = "Message";
    // 工件号
    string SawCut_WorkpieceNumber = "F0000018";
    // 当前工序
    string SawCut_CurrentOperation = "F0000067";
    // 订单规格号
    string SawCut_OrderSpecificationNumber = "F0000016";
    // 进度管理信息
    string SawCut_ScheduleManagementInformation = "Progress";
    // ID
    string SawCut_ID = "F0000030";
    // 数量_切割前
    string SawCut_NumberOfDoubleRolledProducts = "F0000102";
    // 审批人
    string SawCut_Approver = "Approver";
    // 异常代表
    string SawCut_ExceptionRepresentative = "F000085";
    // 双轧关联表单
    string SawCut_DoubleRolledAssociativeForm = "F0000100";
    // 测试用ID
    string SawCut_TestID = "F0000078";
    // 工序计划表
    string SawCut_OperationSchedule = "F0000096";
    // 异常类别
    string SawCut_ExceptionCategory = "F0000058";
    // 订单批次号
    string SawCut_OrderBatchNumber = "F0000014";
    // 关联其它异常工件
    string SawCut_AssociatedWithOtherAbnormalWorkpieces = "F0000199";
    // 轧制方式
    string SawCut_RollingMethod = "F0000031";
    // 异常描述
    string SawCut_ExceptionDescription = "F0000071";
    // 厂区位置
    string SawCut_FactoryLocation = "F0000086";
    // 计划炉次编号
    string SawCut_PlannedHeatNumber = "F0000070";
    // 加工单位
    string SawCut_ProcessingUnit = "F0000045";
    // 是否调整至其他工序
    string SawCut_WhetherToAdjustToOtherOperations = "F0000063";
    // 流程参与者
    string SawCut_TheCirculationDepartment = "F0000103";
    // 节点名称
    string SawCut_NodeName = "F0000076";
    // 质检结论
    string SawCut_QualityInspectionConclusion = "F0000077";
    // 本工序需求期
    string SawCut_DemandPeriodOfThisOperation = "F0000068";
    // 市场需求变更
    string SawCut_hangeInMarketDemand = "F0000104";
    // 检验结果
    string SawCut_InspectionResult = "F0000043";
    // 数据代码
    string SawCut_DataCode = "F0000061";
    // 产品名称
    string SawCut_ProductName = "F0000002";
    // 当前工步
    string SawCut_CurrentWorkStep = "F0000056";
    // 转至工步
    string SawCut_TransferToWorkStep = "F0000073";
    // 审批单代表
    string SawCut_ApprovalRepresentative = "F0000200";
    // 加工者
    string SawCut_Processor = "Processor";
    // 发起异常
    string SawCut_InitiateException = "F0000057";
    // 产品规格
    string SawCut_ProductSpecification = "F0000003";
    // 订单批次规格号
    string SawCut_OrderBatchSpecificationNumber = "F0000040";
    // 原材料号
    string SawCut_RawMaterialNumber = "F0000009";
    // 单重
    string SawCut_SingleWeight = "F0000032";
    // 订单号
    string SawCut_OrderNumber = "F0000012";
    string ProcessFlow_TransferToOperation = "F0000056";
    string ProcessFlow_TransferToStep = "F0000057";
    string RealTimeDynamicProduction_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    string RealTimeDynamicProduction_OperationTableDataID = "F0000070";
    private string RealTimeDynamicProduction_CurrentPreviousOperationTableSchemacode  = "F0000071";

    public D001419So3cw528p3w543tqpt12v28o31(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        activityCode = this.Request.ActivityCode;
        message = new H3.SmartForm.SmartFormResponseDataItem();
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            //缺少注释
            UserId(response);
            if (!this.Request.IsCreateMode)
            {
                //清空父流程的转至工步与转至工序
                ClearTransferToOperationStep();
                //清空转至工步信息
                ClearTransferToWorkStep();
                InitTableComponent();
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.SawCutProgress(this.Engine, SawCut_TableCode, SawCut_CurrentWorkStep);
                me[SawCut_CurrentWorkStep] = workSteps[me.ObjectId];

            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

        response.ReturnData.Add("message", message);
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
            message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //提交
            if (actionName == "Submit")
            {
                //审批人列表权限，使得在列表内能看到自己审批过的业务
                Authority.Approver(this.Request);
                //校验异常信息是否与数据库保持一致
                bool checkedResult = CheckExceptionInfo(response);
                if (checkedResult) { return; }
                //赋值审批来源
                UnqualifiedSource();
            }
            base.OnSubmit(actionName, postValue, response);
            if (actionName == "Submit")
            {
                //异常工步
                AbnormalStep();
            }
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: zzx
    * 初始化表單控件信息
    * 
    */
    public void InitTableComponent()
    {
        //当前工序
        if (me[SawCut_CurrentOperation] + string.Empty == string.Empty) { me[SawCut_CurrentOperation] = "锯切"; }
        //更新本表单
        me.Update();
    }

    //待上机显示当前用户
    private void UserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem userId = new H3.SmartForm.SmartFormResponseDataItem();
        userId.Value = this.Request.UserContext.UserId;
        response.ReturnData.Add("UserId", userId);

    }
    /**
    * --Author: zzx
    * 清空转至工步信息。
    * 
    */
    public void ClearTransferToWorkStep()
    {   //正常节点 转至工步复位
        if (activityCode != "Activity93")
        {
            me[SawCut_TransferToWorkStep] = null;
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void DeleteTransferToWorkStep()
    {
        if (activityCode == "Activity93")
        {
            //获取当前流程业务对象
            H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, 
                this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
            current[SawCut_InitiateException] = "否";                            //发起异常
            current[SawCut_TransferToWorkStep] = null;                           //转至工步
            current[SawCut_ExceptionCategory] = null;                            //异常类别
            current[SawCut_ExceptionDescription] = null;                         //异常描述
            current[SawCut_ExceptionRepresentative] = null;                      //异常代表
            current[SawCut_WhetherToAdjustToOtherOperations] = "否";             //是否调整至其他工序
            current[SawCut_AssociatedWithOtherAbnormalWorkpieces] = null;        //关联其它异常工件
            current["F0000202"] = null;                                          //质量审批单
            current["F0000203"] = null;                                          //需求审批单
            current["F0000204"] = null;                                          //流转审批单
            current["F0000205"] = null;                                          //其它审批单
            current["F0000206"] = null;                                          //审批来源
            current.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = this.Request.UserContext.User.Name;                 //当前审批人
        string currentProcess = me[SawCut_CurrentOperation] + string.Empty;         //当前工序
        string currentWorkStep = me[SawCut_CurrentWorkStep] + string.Empty;          //当前工步
        if (me[SawCut_InitiateException] + string.Empty == "是" && activityCode != "Activity93")          //发起异常
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me["F0000206"] = sourceOfApproval;                                        //审批来源
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTransferToOperationStep()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance = 
            this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
            this.Engine, instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow_TransferToOperation] = null;            //转至工序
        current[ProcessFlow_TransferToStep] = null;                 //转至工步
        current.Update();
    }

    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    * 
    */
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateException = me[SawCut_InitiateException] + string.Empty;
        if (strInitiateException == "是")
        {
            return false;
        }
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库中发起异常的值
        string sqlInitiateException = thisObj[SawCut_InitiateException] + string.Empty;
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
        string strInitiateException = me[SawCut_InitiateException] + string.Empty;
        if (strInitiateException != "是") { return; }
        //关联其它异常工件
        String[] bizObjectIDArray = me[SawCut_AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, RealTimeDynamicProduction_TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = currentObj[RealTimeDynamicProduction_OperationTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = currentObj[RealTimeDynamicProduction_CurrentPreviousOperationTableSchemacode] + string.Empty;

            //加载工序表中的业务对象
            H3.DataModel.BizObject otherObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, 
                this.Engine, currentSchemaCode, otherExceptionId, false);

            //父流程实例ID
            string parentInstanceId = this.Request.WorkflowInstance.ParentInstanceId;
            //获取父流程实例对象
            H3.Workflow.Instance.WorkflowInstance parentInstance =
                this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(parentInstanceId);

            if (otherObj != null)
            {
                //传递异常信息
                foreach (H3.DataModel.PropertySchema activex in otherObj.Schema.Properties)
                {
                    if (activex.DisplayName.Contains("发起异常"))
                    {
                        otherObj[activex.Name] = "是";
                    }

                    if (activex.DisplayName.Contains("异常类别"))
                    {
                        otherObj[activex.Name] = me[SawCut_ExceptionCategory] + string.Empty;
                    }

                    if (activex.DisplayName.Contains("异常代表"))
                    {
                        otherObj[activex.Name] = me[SawCut_ID];
                    }
                }

                otherObj.Update();
            }
        }

        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, 
            this.Engine, SawCut_TableCode, this.Request.BizObjectId, false);

        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity93" && strActivityCode != "Activity98")
        {
            //设置异常权限
            //  exceptionBo["OwnerId"] = this.Request.UserContext.UserId;
        }
        //审批确认
        if (strActivityCode == "Activity98")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[SawCut_InitiateException] = "否";
            //异常描述赋值
            exceptionBo[SawCut_ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[SawCut_ExceptionCategory] = "安全异常";
            //异常代表
            exceptionBo[SawCut_ExceptionRepresentative] = string.Empty;
            exceptionBo.Update();
        }
        DeleteTransferToWorkStep();
    }
}



//using System;
//using System.Collections.Generic;
//using System.Text;
//using H3;
//using System.Data;
//using H3.DataModel;

public class D001419So3cw528p3w543tqpt12v28o31_ListViewController : H3.SmartForm.ListViewController
{
    string activityCode = "锯切列表加载";
    string userName = ""; //当前用户

     // 生产制造流程,锯切
    string SawCut_TableCode = "D001419So3cw528p3w543tqpt12v28o31";
    //当前工步
    string SawCut_CurrentWorkStep = "F0000056";
    //工序计划表
    private string SawCut_OperationSchedule = "F0000096";
    //生产计划,ABCD工序计划表
    private string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";
    // 本工序需求期-锯切
    private string ABCDProcessPlan_DemandPeriodOfThisProcedureSawing = "F0000059";

    public D001419So3cw528p3w543tqpt12v28o31_ListViewController(H3.SmartForm.ListViewRequest request) : base(request)
    {
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {

        try
        {
            //同步实时制造情况
            ProgressManagement.SawCutProgress(this.Engine, SawCut_TableCode, SawCut_CurrentWorkStep);
            //更新列表中的本工序需求期
            RenewalDemandPeriod(response);
        }
        catch (Exception ex)
        {
            Tools.Log.ErrorLog(this.Engine, null, ex, activityCode, userName);
        }

        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }


    //更新列表中的本工序需求期
    protected void RenewalDemandPeriod(H3.SmartForm.LoadListViewResponse response)
    {
        //查询本工序中未完成的流程且当前时间与修改时间差在1天之内的流程实例的“ObjectId”
        string sql = "select ObjectId from i_" + response.SchemaCode + " where status = 2 and now() - ModifiedTime <= 86400";
        System.Data.DataTable table = this.Request.Engine.Query.QueryTable(sql, null);   ///////////aQuerry可以优化/////////////
        if (table != null && table.Rows != null && table.Rows.Count > 0)
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                string id = row["ObjectId"] + string.Empty;
                //获取当前表单的业务对象
                H3.DataModel.BizObject objectt = Tools.BizOperation.Load(this.Request.Engine, response.SchemaCode, id);
                string planId = objectt[SawCut_OperationSchedule] + string.Empty;
                //获取工序计划业务对象
                H3.DataModel.BizObject planObj = Tools.BizOperation.Load(this.Request.Engine, ABCDProcessPlan_TableCode, planId);
                //计划表中的需求期赋值给当前表单的本工序需求期
                objectt[SawCut.DemandPeriodOfThisOperation] = planObj != null ? planObj[ABCDProcessPlan_DemandPeriodOfThisProcedureSawing] + string.Empty : string.Empty;
                objectt.Update();
            }
        }
    }
}