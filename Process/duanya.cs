using System;
using System.Collections.Generic;
using System.Collections;
using H3.DataModel;
using System.Text;
using System.Data;
using H3;
public class D001419Sdoly16pnqd5z66wl60hc4y1u1 : H3.SmartForm.SmartFormController
{
    string activityCode; //当前节点    
    BizObject me;        //本表单对象
    public D001419Sdoly16pnqd5z66wl60hc4y1u1(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;                //本表单对象
        activityCode = Request.ActivityCode;   //当前节点         
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem message   = new H3.SmartForm.SmartFormResponseDataItem();//用户提示信息
        try
        {
            if (!Request.IsCreateMode)
            {
                ClearTheTransitionStepsAndSectionOfTheParentProcess(); //清空父流程的转至工步与转至工序               
                ClearTheGoToWorkStepInformation(); //清空转至工步信息               
                InitializeControls(); //初始化表单控件信息
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.ForgetProgress(Engine, TableCode, CurrentWorkStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[CurrentWorkStep] = workSteps[me.ObjectId];  // 当前工步
                }
            }
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, Request.UserContext.User.FullName);
            message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        response.ReturnData.Add("message", message);
        base.OnLoad(response);
       
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //提交前
            if (actionName == "Submit")
            {
                //校验异常信息是否与数据库保持一致               
                if (ExceptionIsChanged()) { response.Message = "异常数据有更新，请刷新页面！"; return; }
                //多阶段加工逻辑
                MultistageProcessingLogic();
                //赋值审批来源
                AssignmentApprovalSource();
            }
            Authority.Approver(Request);
            base.OnSubmit(actionName, postValue, response);
            //异常工步
            AbnormalWorkingStep(response);
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, Request.UserContext.User.FullName);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: nkx
    * 确认调整后清空转至工步和发起异常赋值“否”
    */
    protected void AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment()
    {
        if (activityCode == "Activity127")
        {
            //获取当前流程业务对象
            BizObject currentFlowObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                      Request.SchemaCode, Request.BizObjectId, false);
            currentFlowObj[InitiateAbnormal] = "否";        //发起异常
            currentFlowObj[TargetStep] = null;              //转至工步
            currentFlowObj[AbnormalCategory] = null;        //异常类别
            currentFlowObj[AbnormalDescription] = null;     //异常描述
            currentFlowObj[AbnormalRepresentative] = null;  //异常代表
            currentFlowObj[IsAdjustToOtherSection] = "否";  //是否调整至其他工序
            currentFlowObj[AssociatedWithOtherAbnormalWorkpieces] = null; //关联其它异常工件
            currentFlowObj[QualityApprovalList] = null;     //质量审批单
            currentFlowObj[DemandApprovalForm] = null;      //需求审批单
            currentFlowObj[CirculationApprovalSheet] = null;//流转审批单
            currentFlowObj[OtherApprovalDocuments] = null;  //其它审批单
            currentFlowObj[SourceOfApproval] = null;        //审批来源
            currentFlowObj.Update();
        }
    }

    /**
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTheTransitionStepsAndSectionOfTheParentProcess()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance parentInstance =
            Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(
                Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        BizObject parentInstanceObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                     parentInstance.SchemaCode, parentInstance.BizObjectId, false);
        parentInstanceObj[ProcessFlow.TransferToOperation] = null;   //转至工序
        parentInstanceObj[ProcessFlow.TransferToStep] = null;        //转至工步
        parentInstanceObj.Update();
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void AssignmentApprovalSource()
    {
        string currentApprover = Request.UserContext.User.Name;          //当前审批人
        string currentProcess = me[CurrentSection] + string.Empty;       //当前工序
        string currentWorkStep = me[CurrentWorkStep] + string.Empty;     //当前工步
        //发起异常  是
        if (me[InitiateAbnormal] + string.Empty == "是" && activityCode != "Activity127")
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" +
                                      currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me[SourceOfApproval] = sourceOfApproval;     //审批来源
            me.Update();
        }
        //检验结果  不合格
        if (me[InspectionResult] + string.Empty == "不合格" && me[InitiateAbnormal] + string.Empty == "否")
        {
            string results = "检验结果不合格";
            string sourceOfApproval = currentApprover + "在" +
                                      currentProcess + "工序的" + currentWorkStep + "工步" + results;
            me[SourceOfApproval] = sourceOfApproval;     //审批来源
            me.Update();
        }
    }

    /**
    * Author: zzx
    * 初始化控件
    */
    public void InitializeControls()
    {
        //当前工序  空
        if (me[CurrentSection] + string.Empty == string.Empty) me[CurrentSection] = "锻压";//当前工序        
        //完成总量  空
        if (me[TotalAmountCompleted] + string.Empty == string.Empty) me[TotalAmountCompleted] = 0;//完成总量        
        BizObject[] ForgeInformation = me[Information] as BizObject[]; //获取子表数据
        //子表（锻压信息）  空
        if (ForgeInformation == null) CreateAddNewSubtableRowData(me, ForgeInformation); //创建添加新的子表行数据       
        //更新本表单
        me.Update();
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool ExceptionIsChanged()
    {
        //表单中发起异常
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty;
        if (AnExceptionWasRaisedInTheForm == "是")
        {
            return false;
        }
        //获取当前流程业务对象
        BizObject thisObj = BizObject.Load(
            H3.Organization.User.SystemUserId, Engine,
            Request.SchemaCode, Request.BizObjectId, false);
        //数据库中发起异常的值
        string AnExceptionIsGeneratedInTheDatabase = thisObj[InitiateAbnormal] + string.Empty;
        return AnExceptionWasRaisedInTheForm != AnExceptionIsGeneratedInTheDatabase;
      
    }

    /**
    * Author: zzx
    * 清空转至工步信息
    */
    public void ClearTheGoToWorkStepInformation()
    {
        //正常节点
        if (activityCode != "Activity127") me[TargetStep] = null;//清空转至工步        
    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作——异常工步
    */
    protected void AbnormalWorkingStep(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty;
        if (AnExceptionWasRaisedInTheForm != "是") { return; }
        //关联其它异常工件
        string[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            BizObject otherIdObj = BizObject.Load(
                H3.Organization.User.SystemUserId, Engine, ScheduleManagement_TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = otherIdObj[ScheduleManagement_SectionTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = otherIdObj[ScheduleManagement_CurrentPreviousSectionTableSchemacode] + string.Empty;
            //加载工序表中的业务对象
            BizObject sectionObj = BizObject.Load(
                H3.Organization.User.SystemUserId, Engine, currentSchemaCode, otherExceptionId, false);
            //传递异常信息
            foreach (PropertySchema activex in sectionObj.Schema.Properties)
            {
                if (activex.DisplayName.Contains("发起异常"))
                {
                    sectionObj[activex.Name] = "是";
                }
                else if (activex.DisplayName.Contains("异常类别"))
                {
                    sectionObj[activex.Name] = me[AbnormalCategory] + string.Empty;
                }
                else if (activex.DisplayName.Contains("异常代表"))
                {
                    sectionObj[activex.Name] = me[ID];
                }
            }
            sectionObj.Update();
        }
        //工步节点
        if (activityCode != "Activity127")
        {
            Request.BizObject[Owner] = Request.UserContext.UserId;  //设置异常权限
        }
        //清空转至工步和发起异常赋值“否”
        AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment();
    }

    /**
    * --Author: zzx
    * 创建添加新的子表行数据
    */
    protected void CreateAddNewSubtableRowData(BizObject me, BizObject[] lstArray)
    {
        //new子表数据集合
        List<BizObject> lstObject = new List<BizObject>();
        if (lstArray != null)
        {
            foreach (BizObject obj in lstArray)
            {
                lstObject.Add(obj);
            }
        }
        //new一个子表业务对象
        BizObject ForgeInformationObj = Tools.BizOperation.New(Engine, Information);
        //任务名称
        string theTaskName = me[TaskName] + string.Empty == string.Empty ? "0" : me[TaskName] + string.Empty;
        //根据主表任务名称 + 1
        int taskNum = int.Parse(theTaskName) + 1;
        //子表任务名称赋值
        ForgeInformationObj[ForgeInformation_TaskName] = taskNum + string.Empty;
        //主表任务名称赋值
        me[TaskName] = taskNum + string.Empty;
        //将子表业务对象添加至数据集合中
        lstObject.Add(ForgeInformationObj);
        //赋值子表
        me[Information] = lstObject.ToArray();
        me.Update();   //更新对象
    }

    /**
    * --Author: zzx
    * 多阶段加工逻辑
    */
    protected void MultistageProcessingLogic()
    {
        //获取子表数据
        BizObject[] ForgeInformation = me[Information] as BizObject[];
        //获取任务数
        int taskNum = me[TaskName] + string.Empty != string.Empty ?
                      int.Parse(me[TaskName] + string.Empty) - 1 : 0;
        if (activityCode == "Activity41") //待装炉
        {
            me[FurnaceChargingState] = "已装炉";// 装炉状态
        }
        if (activityCode == "Activity33") //锻压上机
        {
            //补充当前用户
            ForgeInformation[taskNum][ForgeInformation_ProcessingGroupMember] = Request.UserContext.UserId;
            //获取用户所在部门的部门对象
            H3.Organization.Unit unit = Request.Engine.Organization.GetParentUnit(Request.UserContext.UserId);
            //补充当前用户的部门
            ForgeInformation[taskNum][ForgeInformation_Department] = unit.ObjectId;
            //加工开始时间
            ForgeInformation[taskNum][ForgeInformation_StartTime] = System.DateTime.Now;
        }
        if (activityCode == "Activity106")  //锻压下机
        {
            //加工结束时间
            ForgeInformation[taskNum][ForgeInformation_EndTime] = System.DateTime.Now;
            //创建添加新的子表行数据
            CreateAddNewSubtableRowData(me, ForgeInformation);
        }
    }

    string TableCode = "D001419Sdoly16pnqd5z66wl60hc4y1u1";           // 生产制造流程,锻压
    string Information = "D001419Fe6ad4c9956ed4788927c31123893dc9e";  // 锻压信息子表
    string ID = "F0000041";               // ID
    string TargetStep = "F0000044";       // 转至工步
    string CurrentSection = "F0000053";   // 当前工序
    string CurrentWorkStep = "F0000052";  // 当前工步

    string InitiateAbnormal = "F0000045";        // 发起异常
    string AbnormalCategory = "F0000046";        // 异常类别
    string AbnormalDescription = "F0000058";     // 异常描述
    string AbnormalRepresentative = "F0000200";  // 异常代表
    string IsAdjustToOtherSection = "F0000049";  // 是否调整至其他工序
    string QualityApprovalList = "F0000201";     //质量审批单
    string DemandApprovalForm = "F0000202";      //需求审批单
    string CirculationApprovalSheet = "F0000203";//流转审批单
    string OtherApprovalDocuments = "F0000204";  //其它审批单
    string SourceOfApproval = "F0000205";        //审批来源   
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";  // 关联其他异常工件

    string TotalAmountCompleted = "F0000082";// 完成总量
    string FurnaceChargingState = "F0000091";// 装炉状态
    string TaskName = "F0000081";            // 任务名称
    string InspectionResult = "F0000042";    // 检验结果
    string Owner = "OwnerId";                // 拥有者


    //生产数据分析,实时生产动态
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    string ScheduleManagement_SectionTableDataID = "F0000070";                      // 工序表数据ID
    string ScheduleManagement_CurrentPreviousSectionTableSchemacode = "F0000071";   // 当前工序表SchemaCode

    // 锻压信息子表
    string ForgeInformation_ProcessingGroupMember = "F0000066";  // 加工组成员
    string ForgeInformation_TaskName = "F0000080";  // 任务名称
    string ForgeInformation_StartTime = "F0000083"; // 开始时间
    string ForgeInformation_Department = "F0000084";// 部门
    string ForgeInformation_EndTime = "F0000101";   // 结束时间
}
