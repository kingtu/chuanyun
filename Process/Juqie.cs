using System;
using System.Collections.Generic;
using System.Collections;
using H3.DataModel;
using System.Text;
using System.Data;
using H3;

public class D001419So3cw528p3w543tqpt12v28o31 : H3.SmartForm.SmartFormController
{
    string activityCode = "";     //活动节点   
    BizObject me;    //本工序数据      
    H3.SmartForm.SmartFormResponseDataItem message; //用户提示信息

    public D001419So3cw528p3w543tqpt12v28o31(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;                            //本工序数据
        activityCode = Request.ActivityCode;               //活动节点       
        message = new H3.SmartForm.SmartFormResponseDataItem(); //用户提示信息
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            SetUserId(response);
            if (!Request.IsCreateMode)
            {
                //清空父流程的转至工步与转至工序
                ClearTheTransitionStepsAndSectionOfTheParentProcess();
                //清空转至工步信息
                ClearTheGoToWorkStepInformation();
                //初始化表单控件信息
                InitializeControls();
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.SawCutProgress(Engine, TableCode, CurrentStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {                   
                    me[CurrentStep] = workSteps[me.ObjectId]; //当前工步
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
            if (actionName == "Submit")
            {
                //审批人列表权限，使得在列表内能看到自己审批过的业务
                Authority.Approver(Request);
                //校验异常信息是否与数据库保持一致               
                if (ExceptionIsChanged()) { response.Message = "异常数据有更新，请刷新页面！"; return; }
                //赋值审批来源
                AssignmentApprovalSource();
            }
            base.OnSubmit(actionName, postValue, response);
            if (actionName == "Submit")
            {               
                AbnormalWorkingStep(); //异常工步
            }
        }
        catch (Exception ex)
        {		   //负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, Request.UserContext.User.FullName);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: zzx
    * 初始化表单控件信息
    */
    public void InitializeControls()
    {
        //当前工序
        if (me[CurrentSection] + string.Empty == string.Empty) { me[CurrentSection] = "锯切"; }
        //更新本表单
        me.Update();
    }

    //待上机显示当前用户   mark:注释不完整
    private void SetUserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem userId = new H3.SmartForm.SmartFormResponseDataItem();
        userId.Value = Request.UserContext.UserId;
        response.ReturnData.Add("UserId", userId);

    }
    /**
    * --Author: zzx
    * 清空转至工步信息
    */
    public void ClearTheGoToWorkStepInformation()
    {   //正常节点 
        if (activityCode != "Activity93")
        {
            me[TargetStep] = null;//转至工步复位
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 确认调整后“转至工步”清空和“发起异常”赋值——否
    */
    protected void AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment()
    {
        if (activityCode == "Activity93")
        {
            //获取当前流程业务对象
            BizObject current = BizObject.Load(H3.Organization.User.SystemUserId,
                                               Engine, Request.SchemaCode, Request.BizObjectId, false);
            current[InitiateAbnormal] = "否";        //发起异常
            current[TargetStep] = null;              //转至工步
            current[AbnormalCategory] = null;        //异常类别
            current[AbnormalDescription] = null;     //异常描述
            current[AbnormalRepresentative] = null;  //异常代表
            current[IsAdjustToOtherSection] = "否";  //是否调整至其他工序
            current[AssociatedWithOtherAbnormalWorkpieces] = null;   //关联其它异常工件
            current[QualityApprovalList] = null;        //质量审批单
            current[DemandApprovalForm] = null;         //需求审批单
            current[CirculationApprovalSheet] = null;   //流转审批单
            current[OtherApprovalDocuments] = null;     //其它审批单
            current[SourceOfApproval] = null;           //审批来源
            current.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void AssignmentApprovalSource()
    {
        string currentApprover = Request.UserContext.User.Name;//当前审批人
        string currentProcess = me[CurrentSection] + string.Empty;  //当前工序
        string currentWorkStep = me[CurrentStep] + string.Empty;    //当前工步
        //发起异常
        if (me[InitiateAbnormal] + string.Empty == "是" && activityCode != "Activity93")
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" +
                                      currentWorkStep + "工步" + abnormal;
            me[SourceOfApproval] = sourceOfApproval;//审批来源
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
            Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(
                Request.WorkflowInstance.ParentInstanceId);
        if (instance == null) { return; }
        //获取父流程业务对象
        BizObject current = BizObject.Load(
            H3.Organization.User.SystemUserId,
            Engine, instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow_TargetSection] = null;  //转至工序
        current[ProcessFlow_TargetStep] = null;     //转至工步
        current.Update();
    }

    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool ExceptionIsChanged()
    {
        //表单中发起异常
        string raised = me[InitiateAbnormal] + string.Empty;
        if (raised == "是") return false;        
        BizObject thisObj = BizObject.Load(
            H3.Organization.User.SystemUserId, Engine,
            Request.SchemaCode, Request.BizObjectId, false);
        //数据库中发起异常的值
        string generated = thisObj[InitiateAbnormal] + string.Empty;
        //发起异常不为是时执行与数据库值是否一致的校验
        return raised != generated;
    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作  //异常工步
    */
    protected void AbnormalWorkingStep()
    {
        //表单中发起异常
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty;
        if (AnExceptionWasRaisedInTheForm != "是") { return; }
        //关联其它异常工件
        String[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            BizObject otherIdObj = BizObject.Load(
                H3.Organization.User.SystemUserId,
                Engine, ScheduleManagement_TableCode, bizObjectID, false);
            //生产进度管理 - 工序表数据ID
            string otherExceptionId = otherIdObj[ScheduleManagement_SectionTableDataID] + string.Empty;
            //生产进度管理 - 工序表SchemaCode
            string currentSchemaCode = otherIdObj[ScheduleManagement_CurrentPreviousSectionTableSchemacode] + string.Empty;
            //加载工序表中的业务对象
            BizObject sectionObj = BizObject.Load(
                H3.Organization.User.SystemUserId,
                Engine, currentSchemaCode, otherExceptionId, false);
            if (sectionObj != null)
            {
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
        }
       
        var strActivityCode = Request.ActivityCode; //当前节点       
        if (strActivityCode != "Activity93" && strActivityCode != "Activity98") //工步节点
        {            
            Request.BizObject[Owner] = Request.UserContext.UserId;//设置异常权限
        }
        //确认调整后“转至工步”清空和“发起异常”赋值——否
        AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment();
    }

    // 生产制造流程,锯切
    string TableCode = "D001419So3cw528p3w543tqpt12v28o31";
    string ID = "F0000030";              // ID
    string CurrentStep = "F0000056";     // 当前工步
    string CurrentSection = "F0000067";  // 当前工序
    string TargetStep = "F0000073";      // 转至工步
    string Owner = "OwnerId";            //拥有者

    string InitiateAbnormal = "F0000057";        // 发起异常
    string AbnormalCategory = "F0000058";        // 异常类别
    string AbnormalDescription = "F0000071";     // 异常描述
    string AbnormalRepresentative = "F000085";   // 异常代表
    string IsAdjustToOtherSection = "F0000063";  // 是否调整至其他工序
    string QualityApprovalList = "F0000202";     //质量审批单
    string DemandApprovalForm = "F0000203";      //需求审批单
    string CirculationApprovalSheet = "F0000204";//流转审批单
    string OtherApprovalDocuments = "F0000205";  //其它审批单
    string SourceOfApproval = "F0000206";        //审批来源
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";// 关联其它异常工件
    //工艺流程表
    string ProcessFlow_TargetSection = "F0000056";  // 转至工序
    string ProcessFlow_TargetStep = "F0000057";     // 转至工步
    //进度管理
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";//生产进度管理
    string ScheduleManagement_SectionTableDataID = "F0000070";                      //工序表数据ID
    string ScheduleManagement_CurrentPreviousSectionTableSchemacode = "F0000071";   //生产进度管理 - 工序表SchemaCode
}
