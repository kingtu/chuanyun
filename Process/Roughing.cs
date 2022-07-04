using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
//using H3;
using H3.Workflow.Instance;
using H3.DataModel;

public class D001419Szzswrfsp91x3heen4dykgwus0 : H3.SmartForm.SmartFormController
{
    string activityCode = "";//活动节点编码 
    Dictionary<string, bool> boolConfig;//布尔值字典
    BizObject me;
    H3.SmartForm.SmartFormResponseDataItem item;  //用户提示信息
    H3.SmartForm.SmartFormResponseDataItem userId;  //用户Id
    string info = string.Empty;  //值班信息
    string userName = ""; //当前用户
    public D001419Szzswrfsp91x3heen4dykgwus0(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;//本表单业务对象
        activityCode = Request.ActivityCode;//活动节点编码 
         
        item = new H3.SmartForm.SmartFormResponseDataItem();//用户提示信息
        userId = new H3.SmartForm.SmartFormResponseDataItem();//用户Id
        userId.Value = Request.UserContext.UserId;
        userName = Request.UserContext.User.FullName;//当前用户
        boolConfig = new Dictionary<string, bool>();//转换工艺配置为布尔值
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);
    }
    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            //不是创建模式
            if (!Request.IsCreateMode)
            {
                ClearTargetSectiontStep();                          //清空父流程的转至工步与转至工序   TargetSection
                ClearTargetStep();                                  //清除转至工步
                InitTableComponent();                               //初始化控件
                if (Request.WorkflowInstance.IsUnfinished)      //流程处于未完成状态
                {
                    MachiningTime();                                //统计机加工耗时
                    InitFlawDetectionForm();                        //初始化探伤表
                }
                Hashtable workSteps = ProgressManagement.RoughingProgress(Engine, Roughing_TableCode, CurrentWorkStep);//同步数据至实时制造情况
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
        ReturnDataInfo(response); //前端返回信息
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
            if (actionName == "RefreshDisInfo" && activityCode == "cuchexiaji") //下机时允许刷新派工量、工时、下屑量
            {
                RefreshDisInfo(actionName, activityCode, response);//加工中刷新派工量信息
                base.OnSubmit(actionName, postValue, response);
            }
            if (actionName == "Submit")
            {
                string dipatchFlag = Request.BizObject[DispatchingSwitch] + string.Empty;//派工开关
                if (dipatchFlag == "开")
                {
                    DispatchLogic.PullDispatch(Engine, activityCode, me, (string)userId.Value, actionName);//派工逻辑
                    LoadFinishingDispatchInfo();   //加载粗车精车工序的派工信息
                }
                MultistageProcessingLogic(activityCode);//多阶段加工流程逻辑
                Authority.Approver(Request);//审批人追加               
                bool checkedResult = CheckExceptionInfo(response); //校验异常信息是否与数据库保持一致
                if (checkedResult) { return; }
                UnqualifiedSource(); //赋值审批来源
                WhetherTransshipment();//计算下一工序是否转运
                base.OnSubmit(actionName, postValue, response);
                AbnormalStep();   //异常工步
            }
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);//负责人信息
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }
    /**
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void DeleteTargetStep()
    {
        if (activityCode == "Activity92")
        {
            //获取当前流程业务对象
            BizObject currentFlowObj = BizObject.Load(H3.Organization.User.SystemUserId,
                                                      Engine, Request.SchemaCode, Request.BizObjectId, false);
            currentFlowObj[InitiateFlawDetection] = "否";                          //发起异常
            currentFlowObj[TargetStep] = null;                                     //转至工步
            currentFlowObj[AbnormalCategory] = null;                               //异常类别
            currentFlowObj[AbnormalDescription] = null;                            //异常描述
            currentFlowObj[AbnormalRepresentative] = null;                         //异常代表
            currentFlowObj[ProcessAdjustmentRange] = "否";                         //是否调整至其他工序
            currentFlowObj[AssociatedWithOtherAbnormalWorkpieces] = null;          //关联其它异常工件
            currentFlowObj[MachiningQualityTreatment] = null;                      //机加质量处理
            currentFlowObj[DemandApprovalForm] = null;                             //需求审批单
            currentFlowObj[TransferApprovalForm] = null;                           //流转审批单
            currentFlowObj[OtherApprovalDocuments] = null;                         //其它审批单
            currentFlowObj[ApprovalSource] = null;                                 //审批来源
            currentFlowObj.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = Request.UserContext.User.Name;    //当前审批人
        string currentSection = me[CurrentSection] + string.Empty;      //当前工序
        string currentWorkStep = me[CurrentWorkStep] + string.Empty;    //当前工步
        if (me[InitiateFlawDetection] + string.Empty == "是" && activityCode != "Activity92")                          //发起异常
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentSection + "工序的" + currentWorkStep + "工步" + abnormal;
            me[ApprovalSource] = sourceOfApproval;                      //审批来源
            me.Update();
        }
        if (me[InspectionResult] + string.Empty == "不合格" && me[InitiateFlawDetection] + string.Empty == "否")        //检验结果
        {
            string results = "自检结果不合格";
            string sourceOfApproval = currentApprover + "在" + currentSection + "工序的" + currentWorkStep + "工步" + results;
            me[ApprovalSource] = sourceOfApproval;                      //审批来源
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTargetSectiontStep()
    {
        //获取父流程实例对象
        WorkflowInstance parentInstance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        BizObject parentInstanceObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, parentInstance.SchemaCode, parentInstance.BizObjectId, false);
        parentInstanceObj[ProcessFlow_TargetSection] = null;   //转至工序
        parentInstanceObj[ProcessFlow_TargetStep] = null;      //转至工步
        parentInstanceObj.Update();
    }

    /**
    * --Author: nkx
    * 计算下一工序是否转运
    */
    protected void WhetherTransshipment()
    {
        if (activityCode == "cuchexiaji" && me[TotalAmountCompleted] + string.Empty == "1")//节点名称是否为 粗车中 完成总量=1
        {
            bool flag = true;
            if (me[LogisticsSwitch] + string.Empty == "开")//物流开关
            {
                //工序表派工子表信息
                BizObject[] disProcessSubs = me[RoughingFinishingDispatchingInformation_TableCode] as BizObject[];
                //判断工序表派工子表有没有值
                if (disProcessSubs != null && disProcessSubs.Length > 0)
                {
                    for (int i = 0; i < disProcessSubs.Length; i++)
                    {
                        //判断“任务状态” 是否等于 “已完成”
                        if (disProcessSubs[i][RoughingFinishingDispatchingInformation_TaskStatus] + string.Empty != "已完成")
                        {
                            //判断子表信息的车间位置与现在工件所在的车间位置是否一致，不一致则给转至工步控件赋值“待四面光”（跳过待转运）
                            if (disProcessSubs[i][RoughingFinishingDispatchingInformation_FinishingWorkshopName] + string.Empty != me[WorkshopLocation] + string.Empty)
                            {
                                flag = false;
                            }
                        }
                    }
                    if (flag)
                    {
                        //精车是否需要转运
                        me[WhetherFinishingHolesNeedToBeTransferred] = "否";
                    }
                }
                me.Update();
            }
            if (me[LogisticsSwitch] + string.Empty == "关")   //物流开关
            {
                //精车是否需要转运
                me[WhetherFinishingHolesNeedToBeTransferred] = "否";
                me.Update();
            }
        }
    }

    /*
    *--Author:zzx
    *  加工中刷新派工量、工时、下屑量信息
    * @param actionName    表单按钮编码
    * @param activityCode  流程节点编码
    * @param response      流程响应数据
    */
    public void RefreshDisInfo(string actionName, string activityCode, H3.SmartForm.SubmitSmartFormResponse response)
    {
        string[] dispatchInfo = DispatchLogic.PullDispatch(Engine, activityCode, me, (string)userId.Value, actionName);//获取最新派工量、工时、下屑量
        Dictionary<string, object> resData = new Dictionary<string, object>();
        if (dispatchInfo != null && dispatchInfo.Length == 4)
        {
            resData.Add("disQuantity", dispatchInfo[0]);//返回最新派工量、工时、下屑量
            resData.Add("manHour", dispatchInfo[1]);
            resData.Add("chipQuantity", dispatchInfo[2]);
            resData.Add("objectId", dispatchInfo[3]);
        }
        response.ReturnData = resData;
    }

    /**
    * --Author: nkx
    * 粗车精车工序的派工信息赋值
    */
    public void LoadFinishingDispatchInfo()
    {
        //完成总量等于1时
        if (me[TotalAmountCompleted] + string.Empty == "1")
        {
            //读取下一工序的派工信息
            DispatchLogic.DispatchLogicFinishing(Engine, me, activityCode);
        }
    }

    /**
    * Auther：zzx
    * 返回前端的信息
    */
    private void ReturnDataInfo(H3.SmartForm.LoadSmartFormResponse response)
    {
        response.ReturnData.Add("key1", item);
        userId.Value = Request.UserContext.UserId;
        response.ReturnData.Add("userId", userId);
    }
    /**
    * --Author: zzx
    * 初始化控件
    */
    public void InitTableComponent()
    {
        //初始化当前工序:粗车
        if (me[CurrentSection] + string.Empty == string.Empty)
        {
            me[CurrentSection] = "粗车";
        }
        //发起探伤 :否 
        if (me[InitiateFlawDetection] + string.Empty == string.Empty)
        {
            me[InitiateFlawDetection] = "否";
        }
        me[BackToTheComputer] = "否";
        BizObject[] thisLstArray = me[RoughMachiningSubtable_TableCode] as BizObject[];   //获取多阶段加工子表
        me[TaskNumber] = me[TaskNumber] + string.Empty != string.Empty ? me[TaskNumber] + string.Empty : "1";       //初始化任务序号
        if (thisLstArray == null)
        {
            //初始化子表
            CreatSublist(me);
        }
        BizObject[] processList = me[RoughMachiningSubtable_TableCode] as BizObject[];    //粗车机加工子表
        //上机时发起派工变更
        if (activityCode == "Activity14" && (bool)processList[processList.Length - 1][RoughMachiningSubTable_DispatchQuantityAlteration] == true)
        {
            //初始化多阶段加工子表
            CreatSublist(me);
        }
        if (me[ProcessingDifficulty] + string.Empty == "") { me[ProcessingDifficulty] = 1; }//加工难度设置默认值1
        //完成总量  空
        if (me[TotalAmountCompleted] + string.Empty == string.Empty)
        {
            me[TotalAmountCompleted] = 0;
        }
        me.Update();        //更新本表单

    }
    /**
    * --Author: zzx
    * 清空转至工步信息。
    * 
    */
    public void ClearTargetStep()
    {
        //正常节点 转至工步复位
        if (activityCode != "Activity92")
        {
            me[TargetStep] = null;
        }
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        string strInitiateAbnormal = me[InitiateAbnormal] + string.Empty; //表单中发起异常
        if (strInitiateAbnormal == "是")
        {
            return false;
        }
        BizObject thisObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        string sqlInitiateAbnormal = thisObj[InitiateAbnormal] + string.Empty;//数据库中发起异常的值
        if (strInitiateAbnormal != sqlInitiateAbnormal)
        {
            response.Message = "异常数据有更新，请刷新页面！";//发起异常不为是时执行与数据库值是否一致的校验
            return true;
        }
        else
        {

            return false; //与数据库相同
        }
    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作
    */
    protected void AbnormalStep()
    {
        string strInitiateAbnormal = me[InitiateAbnormal] + string.Empty;
        if (strInitiateAbnormal != "是") { return; }
        String[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];//关联其它异常工件
        foreach (string bizObjectID in bizObjectIDArray)  //遍历其他ID
        {
            //加载其他异常ID 的业务对象
            BizObject currentObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, RealTimeDynamicProduction_TableCode, bizObjectID, false);
            string otherExceptionId = currentObj[RealTimeDynamicProduction_SectionTableDataID] + string.Empty;//实时生产动态 - 工序表数据ID
            string currentSchemaCode = currentObj[RealTimeDynamicProduction_CurrentPreviousSectionTableSchemacode] + string.Empty;//实时生产动态 - 工序表SchemaCode
            //加载工序表中的业务对象
            BizObject otherObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, currentSchemaCode, otherExceptionId, false);       
            //传递异常信息
            foreach (PropertySchema activex in otherObj.Schema.Properties)
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

        BizObject exceptionBo = BizObject.Load(H3.Organization.User.SystemUserId, Engine, Roughing.TableCode, Request.BizObjectId, false);

        var strActivityCode = Request.ActivityCode; //当前节点
        if (strActivityCode != "Activity127" && strActivityCode != "Activity128") //工步节点
        {
            exceptionBo[Owner] = Request.UserContext.UserId; //设置异常权限
        }
        DeleteTargetStep();
    }

    /*
    *--Author:fubin
    * 多阶段加工流程逻辑
    * @param activityCode 流程节点编码
    */
    private void MultistageProcessingLogic(string activityCode)
    {
        BizObject[] lstArray = me[RoughMachiningSubtable_TableCode] as BizObject[]; //获取多阶段加工子表
        {
            me[TaskNumber] = lstArray != null ? lstArray.Length + string.Empty : "1";//修正任务数-----任务序号
            int taskNum = me[TaskNumber] + string.Empty != string.Empty ? int.Parse(me[TaskNumber] + string.Empty) - 1 : 0; //获取任务数
            if (activityCode == "Activity14") //粗车上机
            {
                lstArray[taskNum][RoughMachiningSubTable_Processor] = Request.UserContext.UserId; //当前加工者
                lstArray[taskNum][RoughMachiningSubTable_StartTime] = System.DateTime.Now;//加工开始时间 
            }            
            if (activityCode == "cuchexiaji")//粗车下机
            {
                //完成总量小于1时
                if ((me[TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[TotalAmountCompleted] + string.Empty) < 1)
                {
                    me[TaskNumber] = lstArray.Length + 1;//递增计数器，并更新
                    CreatSublist(me);   //创建添加新的子表行数据
                }
                //当前加工者   空
                if (lstArray[taskNum][RoughMachiningSubTable_Processor] + string.Empty == string.Empty)
                {
                    lstArray[taskNum][RoughMachiningSubTable_Processor] = Request.UserContext.UserId;//当前加工者
                }
            }
        }
    }

    /*
    *--Author:fubin
    * 查询更新机加工耗时
    */
    protected void MachiningTime()
    {
        //查询粗车加工中所有耗时
        string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_{0} " +
            " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'cuchexiaji' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", Roughing_TableCode, me.ObjectId);
        DataTable data = Engine.Query.QueryTable(command, null);

        if (data != null && data.Rows != null && data.Rows.Count > 0)  //机加工耗时计算
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
        string crackDetectionId = me[FlawDetectionTable] + string.Empty;//探伤表Id        
        string activityName = Request.WorkItem + string.Empty == string.Empty ? string.Empty : Request.WorkItem.ActivityDisplayName;//流程节点名称
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (crackDetectionId == string.Empty)
        {
            string thisId = me[ID] + string.Empty; //ID
            string mySql = string.Format("Select " + FlawDetectionTable_Objectid + " From i_" + FlawDetectionTable_TableCode + " Where " + FlawDetectionTable_ID + " = '{0}'", thisId);
            DataTable tsData = Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                me[FlawDetectionTable] = thisId = tsData.Rows[0][FlawDetectionTable_Objectid] + string.Empty;
                me.Update();
            }
            else
            {
                BizObject flawDetectionTableObj = Tools.BizOperation.New(Engine, FlawDetectionTable_TableCode);
                flawDetectionTableObj.Status = BizObjectStatus.Effective;          //生效
                flawDetectionTableObj[FlawDetectionTable_ID] = me[ID] + string.Empty;           //ID
                flawDetectionTableObj[FlawDetectionTable_CurrentSection] = "粗车";              //赋值探伤表的当前工序
                flawDetectionTableObj[FlawDetectionTable_RoughCutting] = me.ObjectId;           //赋值探伤表的取样子流程 为 本表单ObjectId
                List<BizObject> lstObject = new List<BizObject>(); //new子表数据集合
                BizObject lstArray = Tools.BizOperation.New(Engine, FlawDetectionSubTable_TableCode);//子表对象
                lstArray[FlawDetectionSubTable_Process] = "粗车";                               //工序
                lstArray[FlawDetectionSubTable_WorkStep] = "";                                  //工步
                lstObject.Add(lstArray);          //将这个子表业务对象添加至子表数据集合中
                flawDetectionTableObj[FlawDetectionSubTable_TableCode] = lstObject.ToArray();   //子表数据赋值
                flawDetectionTableObj.Create();
                me[FlawDetectionTable] = flawDetectionTableObj.ObjectId;                        //探伤表
                me.Update();
            }
        }
        //探伤表不为空时,写入工序信息
        if (crackDetectionId != string.Empty)
        {
            BizObject flawDetectionTableObj = 
                Tools.BizOperation.Load(Engine, FlawDetectionTable_TableCode, crackDetectionId);        //加载探伤表

            BizObject[] flawDetectionSubtableObj =
                flawDetectionTableObj[FlawDetectionSubTable_TableCode] as BizObject[];     //获取子表数据

            flawDetectionTableObj[FlawDetectionTable_CurrentSection] = "粗车";                            //赋值探伤表的当前工序
            flawDetectionTableObj[FlawDetectionTable_RoughCutting] = me.ObjectId;                         //赋值探伤表的取样子流程 为 本表单ObjectId
            if (flawDetectionSubtableObj[flawDetectionSubtableObj.Length - 1][FlawDetectionSubTable_ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
            {
                flawDetectionSubtableObj[flawDetectionSubtableObj.Length - 1][FlawDetectionSubTable_Process] = "粗车"; //工序

                flawDetectionSubtableObj[flawDetectionSubtableObj.Length - 1][FlawDetectionSubTable_WorkStep] =
                    activityName != "待探伤" ? activityName : me[CurrentWorkStep] + string.Empty;//工步
                flawDetectionSubtableObj[flawDetectionSubtableObj.Length - 1].Update();
            }
            flawDetectionTableObj.Update();
        }
    }

    /*
    *--Author:fubin
    * 创建添加新的子表行数据
    * @param thisObj 本表单数据
    */
    protected void CreatSublist(BizObject thisObj)
    {
        BizObject roughMachiningSubtableObj = Tools.BizOperation.New(Engine, RoughMachiningSubtable_TableCode);                                       //new一个子表业务对象 
        roughMachiningSubtableObj[RoughMachiningSubTable_TaskNumber] = 
            thisObj[TaskNumber] + string.Empty == string.Empty ? "1" : thisObj[TaskNumber] + string.Empty;   //任务序号
        string dipatchFlag = Request.BizObject[DispatchingSwitch] + string.Empty;//派工开关
        if (dipatchFlag == "关")
        {
            roughMachiningSubtableObj[RoughMachiningSubTable_TaskName] = "默认派工任务";
        }
        //将这个子表业务对象添加至子表数据集合中
        Tools.BizOperation.AddChildBizObject(Engine, thisObj, RoughMachiningSubtable_TableCode, roughMachiningSubtableObj);
    }
    /**
     * --Author: zzx
     * 派工开关设置粗车中审批人
     */
    protected void DipatchFlag()
    {
        string dipatchFlag = Request.BizObject[DispatchingSwitch] + string.Empty; //派工开关
        if (dipatchFlag == "关")
        {           
            if (activityCode == "cuchexiaji") //判断是否为待上机节点
            {                
                Request.BizObject[RoughingStageProcessingPeople] = (string)userId.Value;//加工节点的取样加工中人为上机权限人
            }
        }
    }

    //工艺流程表
    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";                                             
    string ProcessFlow_TargetSection = "F0000056";                    //转至工序
    string ProcessFlow_TargetStep = "F0000057";                      //转至工步
    //粗车精车派工信息
    string RoughingFinishingDispatchingInformation_TableCode = "D001419F6dd1cc155f624fc494b7335d1b781b75";         
    string RoughingFinishingDispatchingInformation_TaskStatus = "F0000248";  //任务状态
    string RoughingFinishingDispatchingInformation_FinishingWorkshopName = "F0000233"; //精车车间名称
    //粗车表
    string Roughing_TableCode = "D001419Szzswrfsp91x3heen4dykgwus0";  
    string WorkshopLocation = "F0000080";                             //车间位置
    string CurrentWorkStep = "F0000082";                              //当前工步
    string CurrentSection = "F0000083";                               //当前工序  
    string TotalAmountCompleted = "F0000090";                         //完成总量
    string InitiateFlawDetection = "F0000139";                        //发起探伤
    string TaskNumber = "F0000133";                                   //任务序号
    string ProcessingDifficulty = "F0000105";                         //加工难度
    string TargetStep = "F0000068";                                   //转至工步
    string InitiateAbnormal = "F0000075";                             //发起异常  
    string InspectionResult = "F0000023";                             //检验结果   
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";        //关联其他异常工件    
    string AbnormalCategory = "F0000070";                             //异常类别   
    string AbnormalDescription = "F0000140";                          //异常描述 
    string AbnormalRepresentative = "F0000189";                       //异常代表 //已由F0000171改为F0000189。
    string ProcessAdjustmentRange = "F0000079";                       //（异常后）是否跨工序调整流程    
    string ID = "F0000067";                                           //ID   
    string Owner = "OwnerId";                                         //拥有者 
    string ActualProcessingTime = "CountTime";                        //实际加工耗时
    string FlawDetectionTable = "F0000167";                           //探伤表Id
    string DispatchingSwitch = "F0000271";                            //派工开关
    string LogisticsSwitch = "F0000273";                              //物流开关
    string RoughingStageProcessingPeople = "F0000084";                //粗车阶段加工中人
    string BackToTheComputer = "F0000256";                            //回至上机
    string ApprovalSource = "F0000266";                               //审批来源
    string MachiningQualityTreatment = "F0000262";                    //机加质量处理
    string DemandApprovalForm = "F0000263";                           //需求审批单
    string TransferApprovalForm = "F0000264";                         //流转审批单
    string OtherApprovalDocuments = "F0000265";                       //其它审批单
    string WhetherFinishingHolesNeedToBeTransferred = "F0000259";     //精车是否需要转运
    //探伤表
    string FlawDetectionTable_TableCode = "D001419fdcaecf556264750ae2d5684b2a3706e";                               
    string FlawDetectionTable_ID = "F0000001";                        //ID
    string FlawDetectionTable_Objectid = "ObjectId";                  //探伤表ID
    string FlawDetectionTable_CurrentSection = "F0000023";            //当前工序
    string FlawDetectionTable_RoughCutting = "F0000022";              //探伤表粗车 
    //探伤子表 
    string FlawDetectionSubTable_TableCode = "D001419F89050d4fc56d4bf7b41f343f2e3bd5a1";                           
    string FlawDetectionSubTable_Process = "F0000017";                //工序
    string FlawDetectionSubTable_WorkStep = "F0000018";               //工步
    string FlawDetectionSubTable_ThisFlawDetectionResult = "F0000002";//本次探伤结果
    //粗车机加工子表
    string RoughMachiningSubtable_TableCode = "D001419F8cbba24c57a74ad99bd809ab8e262996";                           
    string RoughMachiningSubTable_DispatchQuantityAlteration = "F0000247";  //派工量变更
    string RoughMachiningSubTable_Processor = "F0000157";             //加工者
    string RoughMachiningSubTable_StartTime = "F0000164";             //开始时间
    string RoughMachiningSubTable_TaskNumber = "F0000166";            //任务计数
    string RoughMachiningSubTable_TaskName = "F0000206";              //派工任务
    //实时生产动态
    string RealTimeDynamicProduction_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";                         
    string RealTimeDynamicProduction_SectionTableDataID = "F0000070";//工序表数据ID
    string RealTimeDynamicProduction_CurrentPreviousSectionTableSchemacode = "F0000071"; //工序表数据ID

}