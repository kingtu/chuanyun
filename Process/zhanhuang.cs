using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using H3;

public class D001419Saesg17flbcod0mvbdha0kkk44 : H3.SmartForm.SmartFormController
{
    string activityCode;        //当前节点   
    H3.DataModel.BizObject me;  //本表单对象    
    H3.SmartForm.SmartFormResponseDataItem item;  //用户提示信息

    public D001419Saesg17flbcod0mvbdha0kkk44(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;//本表单对象
        activityCode = Request.ActivityCode;//当前节点         
        item = new H3.SmartForm.SmartFormResponseDataItem();//用户提示信息
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!Request.IsCreateMode)
            {
                //清空父流程的转至工步与转至工序
                ClearTheTransitionStepsAndSectionOfTheParentProcess();
                //清空转至工步信息
                ClearTheGoToWorkStepInformation();
                //初始化表单控件信息
                InitializeTheFormControlInformation();
                Hashtable workSteps = ProgressManagement.RollingRingProgress(Engine, TableCode, CurrentStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    // 当前工步
                    me[CurrentStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, Request.UserContext.User.FullName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        response.ReturnData.Add("key1", item);
        base.OnLoad(response);
        //--------------------------加载前后分割线-------------------------//
        try
        {
            if (!Request.IsCreateMode)
            {
                //加载后代码
            }
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, Request.UserContext.User.FullName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            if (actionName == "Submit")
            {
                //校验异常信息是否与数据库保持一致               
                if (ExceptionIsChanged()) { response.Message = "异常数据有更新，请刷新页面！"; return; }               
                MultistageProcessingLogic(); //多阶段加工逻辑               
                AssignmentApprovalSource(); //赋值审批来源
            }
            Authority.Approver(Request);
            base.OnSubmit(actionName, postValue, response);           
            AbnormalWorkingStep(); //异常工步
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, Request.UserContext.User.FullName);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment()
    {
        if (activityCode == "Activity113")
        {
            //获取当前流程业务对象
            H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, 
                                                                         Request.SchemaCode, Request.BizObjectId, false);
            current[TargetStep] = null;              //转至工步
            current[InitiateAbnormal] = "否";        //发起异常
            current[AbnormalCategory] = null;        //异常类别
            current[AbnormalDescription] = null;     //异常描述
            current[AbnormalRepresentative] = null;  //异常代表
            current[IsAdjustToOtherSection] = "否";  //是否调整至其他工序
            current[QualityApprovalList] = null;     //质量审批单
            current[DemandApprovalForm] = null;      //需求审批单
            current[CirculationApprovalSheet] = null;//流转审批单
            current[OtherApprovalDocuments] = null;  //其它审批单
            current[SourceOfApproval] = null;        //审批来源
            current[AssociatedWithOtherAbnormalWorkpieces] = null;  //关联其它异常工件
            current.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void AssignmentApprovalSource()
    {
        string currentApprover = Request.UserContext.User.Name;      //当前审批人
        string currentProcess = me[CurrentOperation] + string.Empty; //当前工序
        string currentWorkStep = me[CurrentStep] + string.Empty;     //当前工步
        //发起异常     是
        if (me[InitiateAbnormal] + string.Empty == "是" && activityCode != "Activity113")
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" +
                currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me[SourceOfApproval] = sourceOfApproval; //审批来源
            me.Update();
        }
        //检验结果      不合格
        if (me[InspectionResult] + string.Empty == "不合格" && me[InitiateAbnormal] + string.Empty == "否")
        {
            string results = "检验结果不合格";
            string sourceOfApproval = currentApprover + "在" +
                currentProcess + "工序的" + currentWorkStep + "工步" + results;
            me[SourceOfApproval] = sourceOfApproval; //审批来源
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTheTransitionStepsAndSectionOfTheParentProcess()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance =
            Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                                     instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow_TargetSection] = null; //转至工序
        current[ProcessFlow_TargetStep] = null;    //转至工步
        current.Update();
    }


    /*
    * --Author: zzx
    * 初始化表单控件信息
    */
    public void InitializeTheFormControlInformation()
    {
        //当前工序  空
        if (me[CurrentOperation] + string.Empty == string.Empty) me[CurrentOperation] = "辗环";//当前工序       
        H3.DataModel.BizObject[] RingRollingInformation = me[Information] as H3.DataModel.BizObject[]; //获取子表数据
        //子表（辗环信息）  空
        if (RingRollingInformation == null) CreateAddNewSubtableRowData(me, RingRollingInformation); //创建添加新的子表行数据
        //完成总量  空
        if (me[TotalProcessingQuantity] + string.Empty == "") me[TotalProcessingQuantity] = 0;//完成总量       
        //更新本表单
        me.Update();
    }

    /** <summary>
    * 检查发起异常控件是否被其它异常代表更改
    * Author: zzx
    */
    protected bool ExceptionIsChanged()
    {       
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty; //表单中发起异常
        if (AnExceptionWasRaisedInTheForm == "是") return false;        
        //获取当前流程业务对象
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                                     Request.SchemaCode, Request.BizObjectId, false);       
        string AnExceptionIsGeneratedInTheDatabase = thisObj[InitiateAbnormal] + string.Empty; //数据库中发起异常的值
        return AnExceptionWasRaisedInTheForm != AnExceptionIsGeneratedInTheDatabase;
    }

    /*
    * --Author: zzx
    * 清空转至工步信息
    */
    public void ClearTheGoToWorkStepInformation()
    {            
        if (activityCode != "Activity113") //正常节点 转至工步清空
        {
            me[TargetStep] = null;//转至工步
        }
    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作   异常工步
    */
    protected void AbnormalWorkingStep()
    {
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty;//表单中发起异常
        if (AnExceptionWasRaisedInTheForm != "是") { return; }        
        string[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];//关联其它异常工件
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            H3.DataModel.BizObject otherIdObj = H3.DataModel.BizObject.Load(
                H3.Organization.User.SystemUserId,
                Engine, ScheduleManagement_TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = otherIdObj[ScheduleManagement_SectionTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = otherIdObj[ScheduleManagement_CurrentSectionTableSchemacode] + string.Empty;
            //加载工序表中的业务对象
            H3.DataModel.BizObject sectionObj = H3.DataModel.BizObject.Load(
                H3.Organization.User.SystemUserId,
                Engine, currentSchemaCode, otherExceptionId, false);
            //传递异常信息
            foreach (H3.DataModel.PropertySchema activex in sectionObj.Schema.Properties)
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
        if (activityCode != "Activity113" && activityCode != "Activity114")
        {
            this.Request.BizObject[Owner] = Request.UserContext.UserId; //设置异常权限
        }
        //清空转至工步和发起异常赋值“否”
        AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment();
    }

    //创建添加新的子表行数据
    protected void CreateAddNewSubtableRowData(H3.DataModel.BizObject me, H3.DataModel.BizObject[] lstArray)
    {
        //new子表数据集合
        List<H3.DataModel.BizObject> lstObject = new List<H3.DataModel.BizObject>();

        if (lstArray != null)
        {
            foreach (H3.DataModel.BizObject obj in lstArray)
            {
                lstObject.Add(obj);
            }
        }
        //new一个子表业务对象
        H3.DataModel.BizObject RingRollingInformationObj = Tools.BizOperation.New(Engine, Information);
        //任务名称
        string theTaskName = me[TaskName] + string.Empty == string.Empty ? "0" : me[TaskName] + string.Empty;
        //根据主表任务名称 + 1
        int taskNameNum = int.Parse(theTaskName) + 1;
        //子表任务名称赋值
        RingRollingInformationObj[RingRollingInformation_TaskName] = taskNameNum + string.Empty;
        //主表任务名称赋值
        me[TaskName] = taskNameNum + string.Empty;
        //将子表业务对象添加至集合中
        lstObject.Add(RingRollingInformationObj);
        //赋值子表
        me[Information] = lstObject.ToArray();
        //更新对象
        me.Update();
    }
    //多阶段加工逻辑
    protected void MultistageProcessingLogic()
    {
        //获取子表数据
        H3.DataModel.BizObject[] RingRollingInformation = me[Information] as H3.DataModel.BizObject[];
        //获取任务数
        int taskNameNum = me[TaskName] + string.Empty != string.Empty ? 
                          int.Parse(me[TaskName] + string.Empty) - 1 : 0;
        //辗环上机
        if (Request.ActivityCode == "Activity44")
        {
            //补充当前用户
            RingRollingInformation[taskNameNum][RingRollingInformation_ProcessingGroupMember] = Request.UserContext.UserId;
            //获取用户所在部门的部门对象
            H3.Organization.Unit unit = Request.Engine.Organization.GetParentUnit(Request.UserContext.UserId);
            //补充当前用户的部门
            RingRollingInformation[taskNameNum][RingRollingInformation_Department] = unit.ObjectId;
            //加工开始时间
            RingRollingInformation[taskNameNum][RingRollingInformation_StartTime] = System.DateTime.Now;
        }
        //辗环下机
        if (Request.ActivityCode == "Activity30")
        {
            //加工结束时间
            RingRollingInformation[taskNameNum][RingRollingInformation_EndTime] = System.DateTime.Now;
            //创建添加新的子表行数据
            CreateAddNewSubtableRowData(me, RingRollingInformation);
        }
    }

    // 生产制造流程,辗环
    string TableCode = "D001419Saesg17flbcod0mvbdha0kkk44";
    string ID = "F0000044";              // ID
    string Owner = "OwnerId";            // 拥有者
    string TaskName = "F0000077";        // 任务名称
    string TargetStep = "F0000047";      // 转至工步
    string CurrentStep = "F0000054";     // 当前工步
    string CurrentOperation = "F0000056";// 当前工序
    string InspectionResult = "F0000045";// 检验结果
    string TotalProcessingQuantity = "F0000078";// 加工总量
    string Information = "D001419Fc33fc9abe5f2451e83ce06a5edc1669f"; // 辗环信息子表

    string InitiateAbnormal = "F0000048";        // 发起异常
    string AbnormalCategory = "F0000055";        // 异常类别
    string AbnormalDescription = "F0000062";     // 异常描述
    string AbnormalRepresentative = "F0000082";  // 异常代表
    string QualityApprovalList = "F0000200";     // 质量审批单
    string DemandApprovalForm = "F0000201";      // 需求审批单
    string CirculationApprovalSheet = "F0000202";// 流转审批单
    string OtherApprovalDocuments = "F0000203";  // 其它审批单
    string SourceOfApproval = "F0000204";        // 审批来源
    string IsAdjustToOtherSection = "F0000060";  // 是否调整至其他工序
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";  // 关联其它异常工件

    // 生产制造流程,辗环信息子表
    string RingRollingInformation_TableCode = "D001419Fc33fc9abe5f2451e83ce06a5edc1669f";
    string RingRollingInformation_EndTime = "F0000096";     // 结束时间
    string RingRollingInformation_StartTime = "F0000076";   // 开始时间
    string RingRollingInformation_TaskName = "F0000073";    // 任务名称
    string RingRollingInformation_Department = "F0000079";  // 部门
    string RingRollingInformation_ProcessingGroupMember = "F0000070"; // 加工组成员

    //生产数据分析,实时生产动态
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    string ScheduleManagement_SectionTableDataID = "F0000070";//工序表数据ID    
    string ScheduleManagement_CurrentSectionTableSchemacode = "F0000071";// 当前工序表SchemaCode

    string ProcessFlow_TargetStep = "F0000057";     // 转至工步
    string ProcessFlow_TargetSection = "F0000056";  // 转至工序

}
