
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using H3;

public class D001419So3cw528p3w543tqpt12v28o31 : H3.SmartForm.SmartFormController
{


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
                Hashtable workSteps = ProgressManagement.SawCutProgress(this.Engine, SawCut_TableCode, SawCut_CurrentStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[SawCut_CurrentStep] = workSteps[me.ObjectId];
                }
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
        if (me[SawCut_CurrentSection] + string.Empty == string.Empty) { me[SawCut_CurrentSection] = "锯切"; }
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
            me[SawCut_TargetStep] = null;
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
            current[SawCut_TargetStep] = null;                           //转至工步
            current[SawCut_ExceptionCategory] = null;                            //异常类别
            current[SawCut_ExceptionDescription] = null;                         //异常描述
            current[SawCut_ExceptionRepresentative] = null;                      //异常代表
            current[SawCut_IsAdjustToOtherSection] = "否";             //是否调整至其他工序
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
        string currentProcess = me[SawCut_CurrentSection] + string.Empty;         //当前工序
        string currentWorkStep = me[SawCut_CurrentStep] + string.Empty;          //当前工步
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
        if (instance == null) { return; }
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
            this.Engine, instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow_TargetSection] = null;            //转至工序
        current[ProcessFlow_TargetStep] = null;                 //转至工步
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
                this.Engine, ScheduleManagement_TableCode, bizObjectID, false);
            //生产进度管理 - 工序表数据ID
            string otherExceptionId = currentObj[ScheduleManagement_OperationTableDataID] + string.Empty;
            //生产进度管理 - 工序表SchemaCode
            string currentSchemaCode = currentObj[ScheduleManagement_CurrentPreviousSectionTableSchemacode] + string.Empty;

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

    string activityCode = "";       //活动节点   
    H3.DataModel.BizObject me;      //本记录
    string ProcessName = "锯切";    //本工序名称
    H3.SmartForm.SmartFormResponseDataItem message;  //用户提示信息
    string info = string.Empty;                   //值班信息
    string userName = "";                         //当前用户


    // 生产制造流程,锯切
    string SawCut_TableCode = "D001419So3cw528p3w543tqpt12v28o31";
   
    // 当前工序
    string SawCut_CurrentSection = "F0000067";
   
    // ID
    string SawCut_ID = "F0000030";
  
    // 异常代表
    string SawCut_ExceptionRepresentative = "F000085";
   
    // 异常类别
    string SawCut_ExceptionCategory = "F0000058";
   
    // 关联其它异常工件
    string SawCut_AssociatedWithOtherAbnormalWorkpieces = "F0000199";
   
    // 异常描述
    string SawCut_ExceptionDescription = "F0000071";
   
    // 是否调整至其他工序
    string SawCut_IsAdjustToOtherSection = "F0000063";
   
    // 当前工步
    string SawCut_CurrentStep = "F0000056";
    // 转至工步
    string SawCut_TargetStep = "F0000073";
   
    // 发起异常
    string SawCut_InitiateException = "F0000057";
   
    
    string ProcessFlow_TargetSection = "F0000056";
    string ProcessFlow_TargetStep = "F0000057";

    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";

    string ScheduleManagement_OperationTableDataID = "F0000070";
    //生产进度管理 - 工序表SchemaCode
    string ScheduleManagement_CurrentPreviousSectionTableSchemacode = "F0000071";
}
