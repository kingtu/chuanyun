using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using H3;
public class D001419Sdoly16pnqd5z66wl60hc4y1u1 : H3.SmartForm.SmartFormController
{
    //本表单对象
    H3.DataModel.BizObject me;
    //本表单子表（锻压信息）
    H3.DataModel.BizObject[] subForm;
    //当前节点
    string activityCode;
    H3.SmartForm.SmartFormResponseDataItem message;  //用户提示信息
    string info = string.Empty;  //值班信息
    string userName = ""; //当前用户
    string toException = string.Empty;


    // 生产制造流程,锻压
    string Forge_TableCode = "D001419Sdoly16pnqd5z66wl60hc4y1u1";
    
    // 锻压信息
    string Forge_Information = "D001419Fe6ad4c9956ed4788927c31123893dc9e";
    
    // 当前工序
    string Forge_CurrentSection = "F0000053";
   
    // 异常类别
    string Forge_ExceptionCategory = "F0000046";
      // 转至工步
    string Forge_TargetStep = "F0000044";
   
    // 完成总量
    string Forge_TotalAmountCompleted = "F0000082";
   
    // 异常描述
    string Forge_ExceptionDescription = "F0000058";
 
    // ID
    string Forge_ID = "F0000041";
   
    // 更新异常日志objectID
    string Forge_ObjectIDForUpdateTheExceptionLog = "F0000085";
   
    // 是否调整至其他工序
    string Forge_IsAdjustToOtherSection = "F0000049";
    // 装炉状态
    string Forge_FurnaceChargingState = "F0000091";
   
    // 任务名称
    string Forge_TaskName = "F0000081";
    // 当前工步
    string Forge_CurrentWorkStep = "F0000052";
    // 关联其他异常工件
    string Forge_AssociatedWithOtherAbnormalWorkpieces = "F0000199";
    // 检验结果
    string Forge_InspectionResult = "F0000042";
  
    // 发起异常
    string Forge_InitiateException = "F0000045";  
    // 异常代表
    string Forge_ExceptionRepresentative = "F0000200";
    //拥有者
    string Forge_Owner = "OwnerId";


    //生产数据分析,实时生产动态
    private string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    // 工序表数据ID
    private string ScheduleManagement_SectionTableDataID = "F0000070";

    // 当前工序表SchemaCode
    private string ScheduleManagement_CurrentPreviousSectionTableSchemacode = "F0000071";


  
    // 加工组成员
    string ProcessingGroupMember = "F0000066";
    // 任务名称
    string TaskName = "F0000080"; 
    
    // 开始时间
    string StartTime = "F0000083";
    // 部门
    string Department = "F0000084";
    // 结束时间
    string EndTime = "F0000101";


    public D001419Sdoly16pnqd5z66wl60hc4y1u1(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;
        toException = me[Forge_InitiateException] + string.Empty;
        subForm = me[Forge_Information] as H3.DataModel.BizObject[];
        activityCode = Request.ActivityCode;
        message = new H3.SmartForm.SmartFormResponseDataItem();
        userName = Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!Request.IsCreateMode)
            {
                //清空父流程的转至工步与转至工序
                ClearTransferToOperationStep();
                //清空转至工步信息
                ClearTransferToWorkStep();
                InitTableComponent();
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.ForgetProgress(Engine, Forge_TableCode, Forge_CurrentWorkStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[Forge_CurrentWorkStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

        response.ReturnData.Add("message", message);
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
            info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //提交前
            if (actionName == "Submit")
            {
                //校验异常信息是否与数据库保持一致
                bool checkedResult = CheckExceptionInfo(response);
                if (checkedResult)
                {
                    return;
                }

                //多阶段加工逻辑
                MultistageMachining();
                //赋值审批来源
                UnqualifiedSource();
            }
            Authority.Approver(Request);
            base.OnSubmit(actionName, postValue, response);
            //异常工步
            AbnormalStep(response);

        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: nkx
    * 确认调整后清空转至工步和发起异常赋值“否”
    */
    protected void DeleteTransferToWorkStep()
    {
        if (activityCode == "Activity127")
        {
            //获取当前流程业务对象
            H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
            current[Forge_InitiateException] = "否";                                 //发起异常
            current[Forge_TargetStep] = null;                                //转至工步
            current[Forge_ExceptionCategory] = null;                                 //异常类别
            current[Forge_ExceptionDescription] = null;                              //异常描述
            current[Forge_ExceptionRepresentative] = null;                           //异常代表
            current[Forge_IsAdjustToOtherSection] = "否";                           //是否调整至其他工序
            current[Forge_AssociatedWithOtherAbnormalWorkpieces] = null;             //关联其它异常工件
            current["F0000201"] = null;                                              //质量审批单
            current["F0000202"] = null;                                              //需求审批单
            current["F0000203"] = null;                                              //流转审批单
            current["F0000204"] = null;                                              //其它审批单
            current["F0000205"] = null;                                              //审批来源
            current.Update();
        }
    }

    /**
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTransferToOperationStep()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow.TransferToOperation] = null;            //转至工序
        current[ProcessFlow.TransferToStep] = null;                 //转至工步
        current.Update();
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = Request.UserContext.User.Name;                //当前审批人
        string currentProcess = me[Forge_CurrentSection] + string.Empty;         //当前工序
        string currentWorkStep = me[Forge_CurrentWorkStep] + string.Empty;          //当前工步
        if (me[Forge_InitiateException] + string.Empty == "是" && activityCode != "Activity127")          //发起异常
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me["F0000205"] = sourceOfApproval;                                       //审批来源
            me.Update();
        }
        if (me[Forge_InspectionResult] + string.Empty == "不合格" && me[Forge_InitiateException] + string.Empty == "否")        //检验结果
        {
            string results = "检验结果不合格";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results;
            me["F0000205"] = sourceOfApproval;                                       //审批来源
            me.Update();
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
        if (me[Forge_CurrentSection] + string.Empty == string.Empty)
        {
            me[Forge_CurrentSection] = "锻压";
        }
        //质检结论赋予默认值合格
        // if(me[Forge_QualityInspectionConclusion] + string.Empty == string.Empty) 
        // {
        //     me[Forge_QualityInspectionConclusion] = "合格";
        // }
        if (me[Forge_TotalAmountCompleted] + string.Empty == string.Empty)
        {
            me[Forge_TotalAmountCompleted] = 0;
        }
        //初始化子表（锻压信息）
        if (subForm == null)
        {
            CreatSublist(me, subForm);
        }
        //更新本表单
        me.Update();
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    * 
    */
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateException = toException;
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        //数据库中发起异常的值
        string sqlInitiateException = thisObj[Forge_InitiateException] + string.Empty;
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
     * 清空转至工步信息。
     */
    public void ClearTransferToWorkStep()
    {             //正常节点 转至工步复位
        if (activityCode != "Activity127")
        {
            me[Forge_TargetStep] = null;
        }
    }
    //检查发起异常控件是否被其它异常代表更改 - fubin
    protected bool checkExceptionInfo()
    {
        //表单发起异常
        string strInitiateException = me[Forge_InitiateException] + string.Empty;
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        //数据库发起异常
        string sqlInitiateException = thisObj[Forge_InitiateException] + string.Empty;
        if (strInitiateException != sqlInitiateException)
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
       * 关于发起异常之后各个节点进行的操作。
       * 
     */
    protected void AbnormalStep(H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //发起异常
            string strInitiateException = me[Forge_InitiateException] + string.Empty;
            if (strInitiateException != "是")
            { return; }
            //关联其它异常工件
            String[] bizObjectIDArray = me[Forge_AssociatedWithOtherAbnormalWorkpieces] as string[];
            //遍历其他ID
            foreach (string bizObjectID in bizObjectIDArray)
            {
                //加载其他异常ID 的业务对象
                H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, ScheduleManagement_TableCode, bizObjectID, false);
                //实时生产动态 - 工序表数据ID
                string otherExceptionId = currentObj[ScheduleManagement_SectionTableDataID] + string.Empty;
                //实时生产动态 - 工序表SchemaCode
                string currentSchemaCode = currentObj[ScheduleManagement_CurrentPreviousSectionTableSchemacode] + string.Empty;

                //加载工序表中的业务对象
                H3.DataModel.BizObject otherObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, currentSchemaCode, otherExceptionId, false);

                //父流程实例ID
                string parentInstanceId = Request.WorkflowInstance.ParentInstanceId;
                //获取父流程实例对象
                H3.Workflow.Instance.WorkflowInstance parentInstance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(parentInstanceId);

                //传递异常信息
                foreach (H3.DataModel.PropertySchema activex in otherObj.Schema.Properties)
                {
                    if (activex.DisplayName.Contains("发起异常"))
                    {
                        otherObj[activex.Name] = "是";
                    }

                    if (activex.DisplayName.Contains("异常类别"))
                    {
                        otherObj[activex.Name] = me[Forge_ExceptionCategory] + string.Empty;
                    }

                    if (activex.DisplayName.Contains("异常代表"))
                    {
                        otherObj[activex.Name] = me[Forge_ID];
                    }
                }

                otherObj.Update();
            }

            H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Forge_TableCode, Request.BizObjectId, false);
            //写日志返回记录id
            string logObjectID = null;
            //当前节点
            var strActivityCode = Request.ActivityCode;
            //工步节点
            if (strActivityCode != "Activity127" && strActivityCode != "Activity132")
            {
                //设置异常权限
                exceptionBo[Forge_Owner] = Request.UserContext.UserId;
                //创建发起异常的日志返回日志的objectID
                logObjectID = AbnormalLog.CreateLog(Forge_ID, Forge_CurrentWorkStep, Forge_CurrentSection,
                    Forge_ExceptionCategory, Forge_ExceptionDescription, Request.BizObject, Engine);
                //写日志之后返回的objectId
                exceptionBo[Forge_ObjectIDForUpdateTheExceptionLog] = logObjectID;
                exceptionBo.Update();
            }
            //确认调整意见
            if (strActivityCode == "Activity127")
            {
                //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
                AbnormalLog.UpdateLog(Forge_ID, Forge_CurrentWorkStep, Forge_ExceptionCategory, Forge_ExceptionDescription,
                    Request.BizObject, exceptionBo[Forge_ObjectIDForUpdateTheExceptionLog] + string.Empty, Engine);
            }
            //审批确认
            if (strActivityCode == "Activity132")
            {
                //清空异常信息
                //发起异常赋值
                exceptionBo[Forge_InitiateException] = "否";
                //异常描述赋值
                exceptionBo[Forge_ExceptionDescription] = "误流入本节点，修正本工序操作错误";
                //异常类型赋值
                exceptionBo[Forge_ExceptionCategory] = "安全异常";
                //异常代表
                exceptionBo[Forge_ExceptionRepresentative] = string.Empty;
                exceptionBo.Update();
            }
            //清空转至工步和发起异常赋值“否”
            DeleteTransferToWorkStep();
        }
        catch (Exception ex)
        {

            //存入log记录
            Tools.BusinessExcepiton.ErrorLog(Engine, "锻压", "AbnormalStep", ex.Message + ex.StackTrace + ex.ToString());
            //提示用户
            response.Message = "异常数据有更新！";
        }
    }

    //创建添加新的子表行数据
    protected void CreatSublist(H3.DataModel.BizObject me, H3.DataModel.BizObject[] lstArray)
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
        H3.DataModel.BizObject subData = Tools.BizOperation.New(Engine, Forge_Information);
        string taskName = me[Forge_TaskName] + string.Empty == string.Empty ? "0" : me[Forge_TaskName] + string.Empty; //任务名称
        int num = int.Parse(taskName) + 1; //根据主表任务名称 + 1
        subData[TaskName] = num + string.Empty; //子表任务名称赋值
        me[Forge_TaskName] = num + string.Empty; //主表任务名称赋值
        lstObject.Add(subData);//将这个子表业务对象添加至子表数据集合中
        me[Forge_Information] = lstObject.ToArray(); //子表数据赋值

        me.Update();   //更新对象
    }
    //多阶段加工逻辑
    protected void MultistageMachining()
    {
        H3.DataModel.BizObject[] subForm = me[Forge_Information] as H3.DataModel.BizObject[]; //获取子表数据
        int taskNum = me[Forge_TaskName] + string.Empty != string.Empty ? int.Parse(me[Forge_TaskName] + string.Empty) - 1 : 0; //获取任务数
        //Activity41
        if (Request.ActivityCode == "Activity41") //锻压上机
        {
            me[Forge_FurnaceChargingState] = "已装炉";
        }
        if (Request.ActivityCode == "Activity33") //锻压上机
        {
            //补充当前用户
            subForm[taskNum][ProcessingGroupMember] = Request.UserContext.UserId;
            //获取用户所在部门的部门对象
            H3.Organization.Unit unit = Request.Engine.Organization.GetParentUnit(Request.UserContext.UserId);
            //补充当前用户的部门
            subForm[taskNum][Department] = unit.ObjectId;
            //加工开始时间
            subForm[taskNum][StartTime] = System.DateTime.Now;
        }

        if (Request.ActivityCode == "Activity106")  //锻压下机
        {
            //加工结束时间
            subForm[taskNum][EndTime] = System.DateTime.Now;
            //创建添加新的子表行数据
            CreatSublist(me, subForm);
        }
    }
}
