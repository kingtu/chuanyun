
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using H3;

public class D001419Saesg17flbcod0mvbdha0kkk44 : H3.SmartForm.SmartFormController
{
    //本表单对象
    H3.DataModel.BizObject me;
    //本表单子表（辗环信息）
    H3.DataModel.BizObject[] subForm;
    //当前节点
    string activityCode;
    H3.SmartForm.SmartFormResponseDataItem item;  //用户提示信息
    string info = string.Empty;  //值班信息
    string userName = ""; //当前用户


    // 生产制造流程,辗环
    string RollingRing_TableCode = "D001419Saesg17flbcod0mvbdha0kkk44";
  
    // 加工总量
    string RollingRing_TotalProcessingQuantity = "F0000078";
       // 关联其它异常工件
    string RollingRing_AssociatedWithOtherAbnormalWorkpieces = "F0000199";
    // 当前工序
    string RollingRing_CurrentOperation = "F0000056";
   
    // 任务名称
    string RollingRing_TaskName = "F0000077";
    
    // 辗环信息
    string RollingRing_HotProcessingInformation = "D001419Fc33fc9abe5f2451e83ce06a5edc1669f";
    // 异常代表
    string RollingRing_ExceptionRepresentative = "F0000082";
    
    // 转至工步
    string RollingRing_TargetStep = "F0000047";
   
    // 当前工步
    string RollingRing_CurrentStep = "F0000054";
    // 发起异常
    string RollingRing_InitiateException = "F0000048";
       // 是否调整至其他工序
    string RollingRing_IsAdjustToOtherSection = "F0000060";
    // ID
    string RollingRing_ID = "F0000044";
  
    // 异常描述
    string RollingRing_ExceptionDescription = "F0000062";
   
    // 检验结果
    string RollingRing_InspectionResult = "F0000045";
  
    // 异常类别
    string RollingRing_ExceptionCategory = "F0000055";
  
    //拥有者
    string RollingRing_Owner = "OwnerId";

    //转至工序
    string ProcessFlow_TargetSection = "F0000056";
    //转至工步
    string ProcessFlow_TargetStep = "F0000057";

    // 生产制造流程,辗环信息子表
    string RingRollingInformation_TableCode = "D001419Fc33fc9abe5f2451e83ce06a5edc1669f";
      // 部门
    string RingRollingInformation_Department = "F0000079";
    // 加工组成员
    string RingRollingInformation_ProcessingGroupMember = "F0000070";
    // 开始时间
    string RingRollingInformation_StartTime = "F0000076";
      // 任务名称
    string RingRollingInformation_TaskName = "F0000073";
    // 结束时间
    string RingRollingInformation_EndTime = "F0000096";

    //生产数据分析,实时生产动态
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    //工序表数据ID
    string ScheduleManagement_SectionTableDataID = "F0000070";
    // 当前工序表SchemaCode
    string ScheduleManagement_CurrentSectionTableSchemacode = "F0000071";

    public D001419Saesg17flbcod0mvbdha0kkk44(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;
        subForm = me[RollingRing_HotProcessingInformation] as H3.DataModel.BizObject[];
        activityCode = Request.ActivityCode;
        item = new H3.SmartForm.SmartFormResponseDataItem();
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
                Hashtable workSteps = ProgressManagement.RollingRingProgress(Engine, RollingRing_TableCode, RollingRing_CurrentStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[RollingRing_CurrentStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
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
            info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
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
                if (checkedResult) { return; }
                //多阶段加工逻辑
                MultistageMachining();
                //赋值审批来源
                UnqualifiedSource();
            }
            Authority.Approver(Request);
            base.OnSubmit(actionName, postValue, response);
            //异常工步
            AbnormalStep();
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        try
        {

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
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void DeleteTransferToWorkStep()
    {
        if (activityCode == "Activity113")
        {
            //获取当前流程业务对象
            H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
            current[RollingRing_InitiateException] = "否";                               //发起异常
            current[RollingRing_TargetStep] = null;                              //转至工步
            current[RollingRing_ExceptionCategory] = null;                               //异常类别
            current[RollingRing_ExceptionDescription] = null;                            //异常描述
            current[RollingRing_ExceptionRepresentative] = null;                         //异常代表
            current[RollingRing_IsAdjustToOtherSection] = "否";                          //是否调整至其他工序
            current[RollingRing_AssociatedWithOtherAbnormalWorkpieces] = null;           //关联其它异常工件
            current["F0000200"] = null;                                                  //质量审批单
            current["F0000201"] = null;                                                  //需求审批单
            current["F0000202"] = null;                                                  //流转审批单
            current["F0000203"] = null;                                                  //其它审批单
            current["F0000204"] = null;                                                  //审批来源
            current.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = Request.UserContext.User.Name;                      //当前审批人
        string currentProcess = me[RollingRing_CurrentOperation] + string.Empty;         //当前工序
        string currentWorkStep = me[RollingRing_CurrentStep] + string.Empty;          //当前工步
        if (me[RollingRing_InitiateException] + string.Empty == "是" && activityCode != "Activity113")          //发起异常
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me["F0000204"] = sourceOfApproval;                                             //审批来源
            me.Update();
        }
        if (me[RollingRing_InspectionResult] + string.Empty == "不合格" && me[RollingRing_InitiateException] + string.Empty == "否")       //检验结果
        {
            string results = "检验结果不合格";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results;
            me["F0000204"] = sourceOfApproval;                                             //审批来源
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
        H3.Workflow.Instance.WorkflowInstance instance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow_TargetSection] = null;            //转至工序
        current[ProcessFlow_TargetStep] = null;                 //转至工步
        current.Update();
    }


    /*
     * --Author: zzx
     * 初始化控件
     * 
     */
    public void InitTableComponent()
    {
        //初始化当前工序
        if (me[RollingRing_CurrentOperation] + string.Empty == string.Empty)
        {
            me[RollingRing_CurrentOperation] = "辗环";
        }
        //初始化子表（辗环信息）
        if (subForm == null)
        {
            CreatSublist(me, subForm);
        }
        //初始化完成总量
        if (me[RollingRing_TotalProcessingQuantity] + string.Empty == "")
        {
            me[RollingRing_TotalProcessingQuantity] = 0;
        }
        //更新本表单
        me.Update();
    }

    /// <summary>
    /// 检查发起异常控件是否被其它异常代表更改
    /// Author: zzx
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateException = me[RollingRing_InitiateException] + string.Empty;
        if (strInitiateException == "是")
        {
            return false;
        }
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        //数据库中发起异常的值
        string sqlInitiateException = thisObj[RollingRing_InitiateException] + string.Empty;
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

    /*
    * --Author: zzx
    * 清空转至工步信息。
    */
    public void ClearTransferToWorkStep()
    {             //正常节点 转至工步复位
        if (activityCode != "Activity113")
        {
            me[RollingRing_TargetStep] = null;
        }
    }
    //检查发起异常控件是否被其它异常代表更改 - fubin
    protected bool checkExceptionInfo()
    {
        //表单发起异常
        string strInitiateException = me[RollingRing_InitiateException] + string.Empty;
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        //数据库发起异常
        string sqlInitiateException = thisObj[RollingRing_InitiateException] + string.Empty;
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
    protected void AbnormalStep()
    {
        //发起异常
        string strInitiateException = me[RollingRing_InitiateException] + string.Empty;
        if (strInitiateException != "是")
        { return; }
        //关联其它异常工件
        String[] bizObjectIDArray = me[RollingRing_AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            H3.DataModel.BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                Engine, ScheduleManagement_TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = currentObj[ScheduleManagement_SectionTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = currentObj[ScheduleManagement_CurrentSectionTableSchemacode] + string.Empty;

            //加载工序表中的业务对象
            H3.DataModel.BizObject otherObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                Engine, currentSchemaCode, otherExceptionId, false);


            //传递异常信息
            foreach (H3.DataModel.PropertySchema activex in otherObj.Schema.Properties)
            {
                if (activex.DisplayName.Contains("发起异常"))
                {
                    otherObj[activex.Name] = "是";
                }

                if (activex.DisplayName.Contains("异常类别"))
                {
                    otherObj[activex.Name] = me[RollingRing_ExceptionCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = me[RollingRing_ID];
                }
            }

            otherObj.Update();
        }

        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, Engine,
            RollingRing_TableCode, Request.BizObjectId, false);

        //当前节点
        var strActivityCode = Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity113" && strActivityCode != "Activity114")
        {
            //设置异常权限
            exceptionBo[RollingRing_Owner] = Request.UserContext.UserId;
        }
        //审批确认
        if (strActivityCode == "Activity114")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[RollingRing_InitiateException] = "否";
            //异常描述赋值
            exceptionBo[RollingRing_ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[RollingRing_ExceptionCategory] = "安全异常";
            //异常代表
            exceptionBo[RollingRing_ExceptionRepresentative] = string.Empty;
            exceptionBo.Update();
        }
        DeleteTransferToWorkStep();
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

        //new一个子表业务对象   ///// 任务计数可统一
        H3.DataModel.BizObject subData = Tools.BizOperation.New(Engine, RollingRing_HotProcessingInformation);
        string taskName = me[RollingRing_TaskName] + string.Empty == string.Empty ? "0" : me[RollingRing_TaskName] + string.Empty; //任务名称
        int taskNameNum = int.Parse(taskName) + 1; //根据主表任务名称 + 1
        subData[RingRollingInformation_TaskName] = taskNameNum + string.Empty; //子表任务名称赋值
        me[RollingRing_TaskName] = taskNameNum + string.Empty; //主表任务名称赋值
        lstObject.Add(subData);//将这个子表业务对象添加至子表数据集合中
        me[RollingRing_HotProcessingInformation] = lstObject.ToArray(); //子表数据赋值

        me.Update();   //更新对象
    }
    //多阶段加工逻辑
    protected void MultistageMachining()
    {
        H3.DataModel.BizObject[] subForm = me[RollingRing_HotProcessingInformation] as H3.DataModel.BizObject[]; //获取子表数据
        int taskNum = me[RollingRing_TaskName] + string.Empty != string.Empty ? int.Parse(me[RollingRing_TaskName] + string.Empty) - 1 : 0; //获取任务数
        if (Request.ActivityCode == "Activity44") //辗环上机
        {
            //补充当前用户
            subForm[taskNum][RingRollingInformation_ProcessingGroupMember] = Request.UserContext.UserId;
            //获取用户所在部门的部门对象
            H3.Organization.Unit unit = Request.Engine.Organization.GetParentUnit(Request.UserContext.UserId);
            //补充当前用户的部门
            subForm[taskNum][RingRollingInformation_Department] = unit.ObjectId;
            //加工开始时间
            subForm[taskNum][RingRollingInformation_StartTime] = System.DateTime.Now;
        }

        if (Request.ActivityCode == "Activity30")  //辗环下机
        {
            //加工结束时间
            subForm[taskNum][RingRollingInformation_EndTime] = System.DateTime.Now;
            //创建添加新的子表行数据
            CreatSublist(me, subForm);
        }
    }
}
