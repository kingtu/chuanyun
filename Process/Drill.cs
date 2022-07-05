using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using H3;
using H3.DataModel;

public class D001419Sugyf7m5q744eyhe45o26haop4 : H3.SmartForm.SmartFormController
{
    string activityCode;                            //活动节点编码
    BizObject me;                      //本表单数据
    Dictionary<string, bool> boolConfig;         //布尔值转换
    H3.SmartForm.SmartFormResponseDataItem item;    //用户提示信息
    H3.SmartForm.SmartFormResponseDataItem userId;  //用户Id
    string info = string.Empty;                     //值班信息
    string userName = "";                           //当前用户
    public D001419Sugyf7m5q744eyhe45o26haop4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);

        me = Request.BizObject;                            //本表单数据
        activityCode = Request.ActivityCode;               //活动节点编码
        item = new H3.SmartForm.SmartFormResponseDataItem();    //用户提示信息
        userId = new H3.SmartForm.SmartFormResponseDataItem();  //用户Id
        userId.Value = Request.UserContext.UserId;
        userName = Request.UserContext.User.FullName;      //当前用户
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!Request.IsCreateMode) //不是创建模式
            {
                ClearTargetSection();//清空父流程的转至工步与转至工序               
                ClearTargetStep(); //清空转至工步信息                
                if (Request.WorkflowInstance.IsUnfinished)//流程处于未完成状态
                {                    
                    MachiningTime();//统计机加工耗时
                }                
                InitTableComponent();//初始化控件
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.DrillProgress(Engine, Drilling_TableCode, CurrentWorkStep);
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[CurrentWorkStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        ReturnDataInfo(response);//返回前端信息
        base.OnLoad(response);
        //try
        //{
        //    if (!Request.IsCreateMode)
        //    {
        //        //加载后代码
        //    }
        //}
        //catch (Exception ex)
        //{
        //    info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
        //    item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        //}
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //下机时允许刷新派工量、工时、下屑量
            if (actionName == "RefreshDisInfo" && activityCode == "Activity35")
            {                
                RefreshDisInfo(actionName, activityCode, response);//加工中刷新派工量信息
            }            
            if (actionName == "Submit")
            {                
                if (me[InitiateException] + string.Empty == "是")//发起异常为是
                {
                    me[Owner] = Request.UserContext.UserId;
                }
                //校验异常信息是否与数据库保持一致                
                if (CheckExceptionInfo()) { response.Message = ""; return; }                
                string dipatchFlag = Request.BizObject[DispatchingSwitch] + string.Empty;//派工开关
                if (dipatchFlag == "开")
                {                   
                    DispatchLogic.PullDispatch(Engine, activityCode, me, (string)userId.Value, actionName); //派工逻辑
                }
                MultistageProcessingLogic(activityCode); //多阶段加工流程逻辑                
                UnqualifiedSource();//赋值审批来源
                base.OnSubmit(actionName, postValue, response);              
                AbnormalStep();  //异常工步
            }
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void DeleteTransferToWorkStep()
    {
        if (activityCode == "Activity87")
        {
            //获取当前流程业务对象
            BizObject current = BizObject.Load(H3.Organization.User.SystemUserId, Engine, 
                                               Request.SchemaCode, Request.BizObjectId, false);
            current[InitiateException] = "否";                      //发起异常
            current[TargetStep] = null;                             //转至工步
            current[AbnormalCategory] = null;                       //异常类别
            current[AbnormalDescription] = null;                    //异常描述
            current[AbnormalRepresentative] = null;                 //异常代表
            current[ProcessAdjustmentRange] = "否";                 //是否调整至其他工序
            current[AssociatedWithOtherAbnormalWorkpieces] = null;  //关联其它异常工件
            current[MachiningQualityTreatment] = null;              //机加质量处理
            current[DemandApprovalForm] = null;                     //需求审批单
            current[TransferApprovalForm] = null;                   //流转审批单
            current[OtherApprovalDocuments] = null;                 //其它审批单
            current[ApprovalSource] = null;                         //审批来源
            current.Update();
        }
    }

    /*
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = Request.UserContext.User.Name;                            //当前审批人
        string currentProcess = me[CurrentSection] + string.Empty;                              //当前工序
        string currentWorkStep = me[CurrentWorkStep] + string.Empty;                            //当前工步
        if (me[Drill.InitiateException] + string.Empty == "是" && activityCode != "Activity87")  //发起异常
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me[ApprovalSource] = sourceOfApproval;                                                //审批来源
            me.Update();
        }
        if (me[InspectionResult] + string.Empty == "不合格" && me[InitiateException] + string.Empty == "否") //检验结果
        {
            string results = "自检结果不合格";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results;
            me[ApprovalSource] = sourceOfApproval;                                                //审批来源
            me.Update();
        }
    }

    /*
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTargetSection()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance parentInstance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        BizObject parentInstanceObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, parentInstance.SchemaCode, parentInstance.BizObjectId, false);
        parentInstanceObj[ProcessFlow_TargetSection] = null;        //转至工序
        parentInstanceObj[ProcessFlow_TargetStep] = null;           //转至工步
        parentInstanceObj.Update();
    }

    /*
    *--Author:fubin
    *  加工中刷新派工量、工时、下屑量信息
    * @param actionName    表单按钮编码
    * @param activityCode  流程节点编码
    * @param response      流程响应数据
    */
    public void RefreshDisInfo(string actionName, string activityCode, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //获取最新派工量、工时、下屑量
        string[] dispatchInfo = DispatchLogic.PullDispatch(Engine, activityCode, me, (string)userId.Value, actionName);
        Dictionary<string, object> resData = new Dictionary<string, object>();
        if (dispatchInfo != null && dispatchInfo.Length == 4)
        {   //返回最新派工量、工时、下屑量
            resData.Add("disQuantity", dispatchInfo[0]);
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
    /*
    *--Author:fubin
    * 多阶段加工流程逻辑
    * @param activityCode 流程节点编码
    */
    private void MultistageProcessingLogic(string activityCode)
    {   //多阶段加工子表 
        BizObject[] lstDrillMachiningSubTable = me[DrillMachiningSubTable_TableCode] as BizObject[];
        //修正任务数-----任务名称
        me[TaskNumber] = lstDrillMachiningSubTable != null ? lstDrillMachiningSubTable.Length + string.Empty : "1";      
        int taskNum = int.Parse(me[TaskNumber] + string.Empty) - 1;//获取任务数
        //钻孔上机     
        if (activityCode == "Activity3")
        {
            lstDrillMachiningSubTable[taskNum][DrillMachiningSubTable_Processor] = Request.UserContext.UserId; //当前加工者
            lstDrillMachiningSubTable[taskNum][DrillMachiningSubTable_StartTime] = System.DateTime.Now;        //加工开始时间
        }
        //钻孔下机
        if (activityCode == "Activity35")
        {
            if (lstDrillMachiningSubTable[taskNum][DrillMachiningSubTable_Processor] + string.Empty == string.Empty)
            {
                lstDrillMachiningSubTable[taskNum][DrillMachiningSubTable_StartTime] = userId;  //当前加工者
            }
            //完成总量小于1时
            if ((me[TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[TotalAmountCompleted] + string.Empty) < 1)
            {
                me[TaskNumber] = lstDrillMachiningSubTable.Length + 1;  //递增计数器
                CreatSublist(me);                                       //添加新的子表行数据
            }
        }
    }
    /*
    *--Author:fubin
    * 更新机加工耗时
    */
    protected void MachiningTime()
    {   //查询钻孔加工中所有耗时
        string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime From i_{0} " +
            "a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity35' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", Drilling_TableCode, me.ObjectId);
        DataTable data = Engine.Query.QueryTable(command, null);
        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                double utime = double.Parse(utimestr) / 10000000 / 60;   //转换时间单位为秒
                me[ActualProcessingTime] = utime;                        //更新实际加工耗时
            }
        }
    }
    /*
    *--Author:fubin
    * 创建添加新的子表行数据
    * @param me 本表单数据
    */
    protected void CreatSublist(BizObject me)
    {
        BizObject[] subList = me[DrillMachiningSubTable_TableCode] as BizObject[]; //多阶段加工子表        
        BizObject drillMachiningSubTabledObj = Tools.BizOperation.New(Engine, DrillMachiningSubTable_TableCode);
        //任务名称   
        drillMachiningSubTabledObj[DrillMachiningSubTable_TaskNumber] = subList == null ? "1" : subList.Length + 1 + string.Empty; 
        string dipatchFlag = Request.BizObject[DispatchingSwitch] + string.Empty; //派工开关  
        if (dipatchFlag == "关")
        {
            drillMachiningSubTabledObj[DrillMachiningSubTable_TaskName] = "默认派工任务";
        }
        //将这个子表业务对象添加至子表数据集合中
        Tools.BizOperation.AddChildBizObject(Engine, me, DrillMachiningSubTable_TableCode, drillMachiningSubTabledObj);
    }
    /**
    * Author: zzx
    * 初始化控件
    */
    public void InitTableComponent()
    {
        if (me[CurrentSection] + string.Empty == string.Empty) { me[CurrentSection] = "钻孔"; }                        //当前工序
        if (me[TotalAmountCompleted] + string.Empty == string.Empty) { me[TotalAmountCompleted] = 0; }                //完成总量
        BizObject[] lstArray = me[DrillMachiningSubTable_TableCode] as BizObject[];        //多阶段加工子表
        me[TaskNumber] = me[TaskNumber] + string.Empty != string.Empty ? me[TaskNumber] + string.Empty : "1";        //初始化任务名称
        me[BackToTheComputer] = "否";                                                                                //回至上机

        if (lstArray == null)
        {            
            CreatSublist(me);//初始化多阶段加工子表
        }
        BizObject[] drillMachiningSubTableList = me[DrillMachiningSubTable_TableCode] as BizObject[];
        //上机时发起派工变更
        if (activityCode == "Activity3" && (bool)drillMachiningSubTableList[drillMachiningSubTableList.Length - 1][DrillMachiningSubTable_DispatchQuantityAlteration] == true)
        {            
            CreatSublist(me);//初始化多阶段加工子表
        }            
        if (me[TotalAmountCompleted] + string.Empty == "")//完成总量为空
        {
            me[TotalAmountCompleted] = 0;
        }
        me.Update(); 
    }
    /*
    * --Author: zzx
    * 清空转至工步信息
    */
    public void ClearTargetStep()
    {
        if (activityCode != "Activity87") //正常节点转至工步复位
        {           
            me[TargetStep] = null; //转至工步置空
        }
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool CheckExceptionInfo()
    {
        string strInitiateException = me[InitiateException] + string.Empty;//表单中发起异常
        if (strInitiateException == "是") { return false; }
        BizObject thisObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        string sqlInitiateException = thisObj[InitiateException] + string.Empty;//数据库中发起异常的值
        return strInitiateException != sqlInitiateException;       
    }

    /*
    * Author: zzx
    * 关于发起异常之后各个节点进行的操作
    */
    protected void AbnormalStep()
    {
        string strInitiateException = me[InitiateException] + string.Empty;                 //发起异常
        if (strInitiateException != "是") { return; }       
        string[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];  //关联其它异常工件
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            BizObject currentObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                  RealTimeDynamicProduction_TableCode, bizObjectID, false);
            string otherExceptionId = currentObj[RealTimeDynamicProduction_SectionTableDataID] + string.Empty;                      //实时生产动态 - 工序表数据ID
            string currentSchemaCode = currentObj[RealTimeDynamicProduction_CurrentPreviousSectionTableSchemacode] + string.Empty;  //实时生产动态 - 工序表SchemaCode
            //加载工序表中的业务对象
            BizObject otherObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, currentSchemaCode, otherExceptionId, false);
            //传递异常信息
            foreach (PropertySchema activex in otherObj.Schema.Properties)
            {
                if (activex.DisplayName.Contains("发起异常"))
                {
                    otherObj[activex.Name] = "是";
                }
                else if (activex.DisplayName.Contains("异常类别"))
                {
                    otherObj[activex.Name] = me[AbnormalCategory] + string.Empty;
                }
                else if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = me[ID];
                }
            }
            otherObj.Update();
        }
        var strActivityCode = Request.ActivityCode;                          //当前节点
        if (strActivityCode != "Activity87" && strActivityCode != "Activity88")    //工步节点
        {
            Request.BizObject[Owner] = Request.UserContext.UserId;      //设置异常权限
        }
        //确认调整后转至工步清空和发起异常赋值“否”
        DeleteTransferToWorkStep();
    }

    /**
    * Author: zzx
    * 派工开关设置粗车中审批人
    */
    protected void DipatchFlag()
    {
        string dipatchFlag = Request.BizObject[DispatchingSwitch] + string.Empty;   //派工开关
        if (dipatchFlag == "关" && activityCode == "Activity35")
        {             
            Request.BizObject[DrillingStageProcessingPeople] = (string)userId.Value;  //钻孔阶段加工中人设为上机权限人      
        }
    }
    //工艺流程表ID
    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";    
    string ProcessFlow_TargetSection = "F0000056";                          //转至工序
    string ProcessFlow_TargetStep = "F0000057";                             //转至工步
    //钻孔表ID
    string Drilling_TableCode = "D001419Sugyf7m5q744eyhe45o26haop4";        
    string WorkshopLocation = "F0000052";                                   //车间位置
    string CurrentWorkStep = "F0000054";                                    //当前工步
    string CurrentSection = "F0000056";                                     //当前工序
    string TotalAmountCompleted = "F0000073";                               //完成总量
    string ProcessingDifficulty = "F0000182";                               //加工难度
    string TargetStep = "F0000046";                                         //转至工步
    string InitiateException = "F0000045";                                  //发起异常
    string InspectionResult = "F0000020";                                   //检验结果
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";              //关联其他异常工件
    string AbnormalCategory = "F0000055";                                   //异常类别
    string AbnormalDescription = "F0000100";                                //异常描述
    string AbnormalRepresentative = "F0000171";                             //异常代表
    string ProcessAdjustmentRange = "F0000051";                             //是否调整至其他工序
    string ID = "F0000029";                                                 //ID  
    string Owner = "OwnerId";                                               //拥有者
    string ActualProcessingTime = "CountTime";                              //实际加工耗时 
    string TaskNumber = "F0000166";                                         //任务序号
    string DispatchingSwitch = "F0000241";                                  //派工开关
    string DrillingStageProcessingPeople = "F0000060";                      //钻孔阶段加工中人
    string BackToTheComputer = "F0000225";                                  //回至上机
    string ApprovalSource = "F0000238";                                     //审批来源
    string MachiningQualityTreatment = "F0000234";                          //机加质量处理
    string DemandApprovalForm = "F0000235";                                 //需求审批单
    string TransferApprovalForm = "F0000236";                               //流转审批单
    string OtherApprovalDocuments = "F0000237";                             //其它审批单
    //钻孔机加工子表表ID
    string DrillMachiningSubTable_TableCode = "D001419F790f3a6b004e4988abe9511380792293";   
    string DrillMachiningSubTable_DispatchQuantityAlteration = "F0000212";  //派工量变更
    string DrillMachiningSubTable_Processor = "F0000143";                   //加工者
    string DrillMachiningSubTable_StartTime = "F0000142";                   //开始时间
    string DrillMachiningSubTable_TaskNumber = "F0000141";                  //任务计数
    string DrillMachiningSubTable_TaskName = "F0000218";                    //派工任务
    //实时生产动态表ID
    string RealTimeDynamicProduction_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";     
    string RealTimeDynamicProduction_SectionTableDataID = "F0000070";                           //工序表数据ID
    string RealTimeDynamicProduction_CurrentPreviousSectionTableSchemacode = "F0000071";        //工序表数据ID
}