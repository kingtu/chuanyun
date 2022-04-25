using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using H3;
using H3.Workflow.Instance;
using H3.DataModel;

public class D001419Szzswrfsp91x3heen4dykgwus0 : H3.SmartForm.SmartFormController
{
    private const string ActivityFQ = "Activity28"; //发起节点
    private const string ActivityRK = "Activity75"; //转运活动
    private const string ActivitySJ = "Activity14"; //上机活动
    private const string ActivityXJ = "cuchexiaji"; //下机活动
    private const string ActivityJY = "jianyan";     //检验活动
    private const string ActivitySMGXJ = "Activity140";//四面光下机
    private const string ActivitySMGSJ = "Activity152";// 四面光上机
    private const string ProcessName = "粗车";
    string activityCode = "";
    Dispatch dp = null;
    //布尔值字典
    Dictionary<string, bool> boolConfig;
    H3.DataModel.BizObject me;
    H3.DataModel.BizObject[] lstArray;
    H3.SmartForm.SmartFormResponseDataItem item;  //用户提示信息
    H3.SmartForm.SmartFormResponseDataItem userId;  //用户Id
    string info = string.Empty;  //值班信息
    string userName = ""; //当前用户
    public D001419Szzswrfsp91x3heen4dykgwus0(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        //活动节点编码 
        activityCode = this.Request.ActivityCode;
        //获取粗加工子表  
        lstArray = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
        //派工信息 
        dp = new Dispatch(this.Request.Engine, (string)me[Roughing.ID]);
        item = new H3.SmartForm.SmartFormResponseDataItem();
        userId = new H3.SmartForm.SmartFormResponseDataItem();
        userId.Value = this.Request.UserContext.UserId;
        userName = this.Request.UserContext.User.FullName;
        //转换工艺配置为布尔值
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);
    }
    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!this.Request.IsCreateMode)
            {
                if (!this.Request.IsCreateMode)
                {
                    //初始化控件
                    InitTableComponent();
                    //初始化产品类别规格
                    ProductCategoryUpdate();
                    if (this.Request.WorkflowInstance.IsUnfinished)
                    {
                        //统计机加工耗时
                        MachiningTime();
                        //初始化探伤表
                        InitFlawDetectionForm();
                    }
                    //同步数据至实时制造情况
                    Hashtable workSteps = ProgressManagement.RoughingProgress(this.Engine, Roughing.TableCode, Roughing.CurrentWorkStep);
                    me[Roughing.CurrentWorkStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        //前端返回信息
        ReturnDataInfo(response);
        base.OnLoad(response);

        //--------------------------加载前后分割线-------------------------//

        try
        {
            if (!this.Request.IsCreateMode)
            {
                //加载后代码
                this.Request.BizObject["PlanDevices"] = "1295a4bd-1033-4425-922d-f5071349bacf";
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //下机时允许刷新派工量、工时、下屑量
            if (actionName == "RefreshDisInfo" && activityCode == "cuchexiaji")
            {
                //加工中刷新派工量信息
                RefreshDisInfo(actionName, activityCode, response);
            }
            if (actionName == "Submit")
            {
                //清除转至工步
                ClearTransferToWorkStep();
                //派工逻辑
                DispatchLogic.PullDispatch(this.Engine, activityCode, me, (string)userId.Value, actionName);
                //四面光的审批人赋值
                SMGApprover();
                //多阶段加工流程逻辑
                MultistageProcessingLogic(activityCode);
                //四面光任务记录
                //ProcessRecord(actionName);
                //审批人追加
                Authority.Approver(this.Request);
                //计算产品工时
                TheProductWorkingHours(actionName);
                //校验异常信息是否与数据库保持一致
                bool checkedResult = CheckExceptionInfo(response);
                if (checkedResult) { return; }
                //加载粗车精车工序的派工信息
                LoadFinishingDispatchInfo();
                //获取工序计划表数据
                H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
                //加载四面光配置数据
                LoadingFourSideLightConfiguration(planObj, boolConfig);
            }
            base.OnSubmit(actionName, postValue, response);
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

        //-------------------------提交前后分割线-----------------------//

        try
        {
            //工资计算
            SalaryCalculation(actionName);
            SyncParticipants();
            //异常工步
            AbnormalStep();
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
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
        //获取最新派工量、工时、下屑量
        string[] dispatchInfo = DispatchLogic.PullDispatch(Engine, activityCode, me, userId.Value, actionName);
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
    * --Author: nkx
    * 粗车精车工序的派工信息赋值
    */
    public void LoadFinishingDispatchInfo()
    {
        //完成总量等于1时，读取下一工序的派工信息
        if (me[Roughing.TotalAmountCompleted] + string.Empty == "1")
        {
            //读取下一工序的派工信息
            DispatchLogic.DispatchLogicFinishing(this.Engine, me, activityCode);
        }
    }

    /**
    * --Author: nkx
    * 四面光的审批人赋值
    */
    public void SMGApprover()
    {
        if (activityCode == "Activity14")
        {
            string aaa = userName;
            //粗车机加工子表
            H3.DataModel.BizObject[] taskList = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
            if (taskList != null && taskList.Length > 0)
            {
                foreach (H3.DataModel.BizObject item in taskList)
                {
                    if (item[RoughSubTable.CountingTask] + string.Empty == taskList.Length + string.Empty)
                    {
                        if (item[RoughSubTable.TaskName] + string.Empty == "四面光")
                        {
                            me[Roughing.Worker] = userName;
                        }
                    }
                }
            }
        }
    }

    /**
    * Auther：zlm
    * Create: 2021-11-10
    * Last modified：2021-12-9
    */
    private void FillWorkShop()

    {
        if (activityCode != ActivityRK)
        {
            return;
        }
        string[] r = dp.GetPlanWorkShop(ProcessName);
        if (r.Length >= 2)
        {
            me[Roughing.CurrentWorkshop] = r[0];
            me[Roughing.CurrentLocation] = r[1];
        }
    }

    /**
    * Auther：zlm
    */
    private void UserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem sd = new H3.SmartForm.SmartFormResponseDataItem();
        sd.Value = this.Request.UserContext.UserId;
        response.ReturnData.Add("UserId", sd);
    }

    void SyncParticipants()
    {
        string p = GetParticipantsBy(this.Request.WorkflowInstance.InstanceId);
        BizObject currentObj = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, this.Request.BizObjectId);
        currentObj["CurrentAuditors"] = p;
        currentObj.Update();
    }
    string GetParticipantsBy(string WorkflowId)
    {
        H3.Workflow.Instance.IToken tok = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(WorkflowId).GetLastToken();
        string users = "";
        if (tok.Participants.Length > 0)
        {
            foreach (string p in tok.Participants)
            {
                DataRow dr = GetRow("User", "ObjectId='" + p + "'");
                users += dr["Name"] + ";";
            }
        }
        return users;
    }
    private DataRow GetRow(string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "H_" + table + (where == "" ? "" : " where " + where);

        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        int Count = dt.Rows.Count;
        if (Count > 0)
        {
            return dt.Rows[0];
        }
        else
        {
            return null;
        }
    }
    /**
    * Auther：zzx
    * 返回前端的信息
    */
    private void ReturnDataInfo(H3.SmartForm.LoadSmartFormResponse response)
    {
        response.ReturnData.Add("key1", item);
        userId.Value = this.Request.UserContext.UserId;
        response.ReturnData.Add("userId", userId);
    }
    //zlm
    private void SalaryCalculation(string actionName)
    {
        if (actionName == "Submit" && activityCode == ActivityJY)
        {
            ActivePayroll(MachiningTaskRecord.TableCode, lstArray, H3.Organization.User.SystemUserId);
        }
    }
    /**
    * --Author: zzx
    * 初始化控件
    */
    public void InitTableComponent()
    {
        //初始化当前工序:粗车
        if (me[Roughing.CurrentOperation] + string.Empty == string.Empty)
        {
            me[Roughing.CurrentOperation] = "粗车";
        }
        //发起探伤 :否
        if (me[Roughing.InitiateFlawDetection] + string.Empty == string.Empty)
        {
            me[Roughing.InitiateFlawDetection] = "否";
        }
        //质检结论赋予默认值:合格
        if (me[Roughing.QualityInspectionConclusion] + string.Empty == string.Empty)
        {
            me[Roughing.QualityInspectionConclusion] = "合格";
        }
        me["F0000256"] = "否";
        //获取多阶段加工子表
        H3.DataModel.BizObject[] thisLstArray = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
        //初始化任务名称
        me[Roughing.TaskBusinessName] = me[Roughing.TaskBusinessName] + string.Empty != string.Empty ? me[Roughing.TaskBusinessName] + string.Empty : "1";
        if (thisLstArray == null)
        {
            //初始化子表
            CreatSublist(me);
        }
        ///粗车机加工子表
        H3.DataModel.BizObject[] processList = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
        //上机时发起派工变更
        if (activityCode == "Activity14" && (bool)processList[processList.Length - 1][RoughSubTable.MakeDifference] == true)
        {
            //初始化多阶段加工子表
            CreatSublist(me);
        }
        //加工难度设置默认值1。
        if (me[Roughing.ProcessingDifficulty] + string.Empty == "") { me[Roughing.ProcessingDifficulty] = 1; }
        //更新本表单
        me.Update();
    }
    /**
    * --Author: zzx
    * 清空转至工步信息。
    * 
    */
    public void ClearTransferToWorkStep()
    {             //正常节点 转至工步复位
        if (activityCode != "Activity92" && activityCode != "Activity93")
        {
            me[Roughing.GoToWorkStep] = null;
        }
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateException = me[Roughing.InitiateException] + string.Empty;
        if (strInitiateException == "是")
        {
            return false;
        }
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库中发起异常的值
        string sqlInitiateException = thisObj[Roughing.InitiateException] + string.Empty;
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
        string strInitiateException = me[Roughing.InitiateException] + string.Empty;
        if (strInitiateException != "是") { return; }
        //关联其它异常工件
        String[] bizObjectIDArray = me[Roughing.AssociatedWithOtherAbnormalWorkpieces] as string[];
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
            //获取父流程实例对象
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
                    otherObj[activex.Name] = me[Roughing.ExceptionCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = me[Roughing.ID];
                }
            }
            otherObj.Update();
        }

        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, Roughing.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity127" && strActivityCode != "Activity128")
        {
            //设置异常权限
            exceptionBo[Roughing.Owner] = this.Request.UserContext.UserId;
            //创建发起异常的日志
            logObjectID = AbnormalLog.CreateLog(Roughing.ID, Roughing.CurrentWorkStep, Roughing.CurrentOperation,
                Roughing.ExceptionCategory, Roughing.ExceptionDescription, this.Request.BizObject, this.Engine);
            exceptionBo[Roughing.ObjectIDForUpdateTheExceptionLog] = logObjectID;
            exceptionBo.Update();
        }
        //确认调整意见
        if (strActivityCode == "Activity127")
        {
            //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
            AbnormalLog.UpdateLog(Roughing.ID, Roughing.CurrentWorkStep, Roughing.ExceptionCategory,
                Roughing.ExceptionDescription, this.Request.BizObject, exceptionBo[Roughing.ObjectIDForUpdateTheExceptionLog] + string.Empty, this.Engine);
        }
        //审批确认
        if (strActivityCode == "Activity93")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[Roughing.InitiateException] = "否";
            //异常描述赋值
            exceptionBo[Roughing.ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[Roughing.ExceptionCategory] = "安全异常";
            //异常代表
            exceptionBo[Roughing.ExceptionRepresentative] = string.Empty;
            exceptionBo.Update();
        }
    }

    /*
    *--Author:zhanglimin
    * 四面光任务记录
    * @param activityCode 流程节点编码
    */

    private void ProcessRecord(string actionName)
    {
        if ((activityCode == ActivitySMGSJ) && actionName == "Submit")
        {
            ProductManHour manHourComputer = new ProductManHour(this.Request.Engine);
            H3.DataModel.BizObject[] fourLightSubTable = this.Request.BizObject[Roughing.FourSideLight] as H3.DataModel.BizObject[];
            string deviceType = fourLightSubTable[0][RoughFourLathe.DeviceType] + string.Empty;
            double manHour = manHourComputer.GetTime("四面光", this.Request.BizObject[Roughing.ID] + string.Empty, deviceType, false);
            fourLightSubTable[0][RoughFourLathe.WorkingHours] = manHour;
        }
        if ((activityCode == ActivitySMGXJ) && actionName == "Submit")
        {
            TaskRecorder taskRecorder = new TaskRecorder(this.Request.Engine, this.Request.BizObject);
            H3.DataModel.BizObject[] subTable = this.Request.BizObject[Roughing.FourSideLight] as H3.DataModel.BizObject[];

            if (subTable != null && subTable.Length > 0)
            {
                subTable[subTable.Length - 1][RoughFourLathe.ProcessRecord] = taskRecorder.TaskRecord("粗车四面光", subTable[subTable.Length - 1]);
            }
        }
    }

    /*
    *--Author:fubin
    * 多阶段加工流程逻辑
    * @param activityCode 流程节点编码
    */
    private void MultistageProcessingLogic(string activityCode)
    {
        //获取多阶段加工子表
        H3.DataModel.BizObject[] lstArray = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];

        {   //修正任务数-----任务名称
            me[Roughing.TaskBusinessName] = lstArray != null ? lstArray.Length + string.Empty : "1";
            //获取任务数
            int taskNum = me[Roughing.TaskBusinessName] + string.Empty != string.Empty ? int.Parse(me[Roughing.TaskBusinessName] + string.Empty) - 1 : 0;

            if (activityCode == "Activity14") //粗车上机
            {   //当前加工者
                lstArray[taskNum][RoughSubTable.Processor] = this.Request.UserContext.UserId;
                //加工开始时间 
                lstArray[taskNum][RoughSubTable.StartTime] = System.DateTime.Now;
            }
            if (activityCode == "cuchexiaji") //粗车下机
            {
                //多阶段加工新方案升级机加工任务记录
                UpdateRecordForm(activityCode);
                //完成总量小于1时
                if ((me[Roughing.TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[Roughing.TotalAmountCompleted] + string.Empty) < 1)
                {
                    //递增计数器，并更新
                    me[Roughing.TaskBusinessName] = lstArray.Length + 1;
                    //创建添加新的子表行数据
                    CreatSublist(me);
                }
                if (lstArray[taskNum][RoughSubTable.Processor] + string.Empty == string.Empty)
                {   //当前加工者
                    lstArray[taskNum][RoughSubTable.Processor] = this.Request.UserContext.UserId;
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
        string bizid = me.ObjectId;
        //查询粗车加工中所有耗时
        string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_{0} " +
            " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'cuchexiaji' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", Roughing.TableCode, bizid);
        DataTable data = this.Engine.Query.QueryTable(command, null);
        //机加工耗时计算
        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                double utime = double.Parse(utimestr) / 10000000 / 60;
                me[Roughing.ActualProcessingTime] = utime;
            }
        }
    }

    /*
    *--Author:fubin
    * 产品类别、规格为空时，查询产品参数表中的车加工类别、规格
    */
    protected void ProductCategoryUpdate()
    {
        //产品类别更新
        if (me[Roughing.ProductCategory] + string.Empty == string.Empty || me[Roughing.Specification] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = me[Roughing.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select * From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable typeData = this.Engine.Query.QueryTable(mySql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[Roughing.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                me[Roughing.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
                //赋值产品规格
                me[Roughing.Specification] = typeData.Rows[0][ProductParameter.ProductSpecification] + string.Empty;
            }
        }
    }

    /*
    *--Author:fubin
    * 初始化探伤表
    */
    protected void InitFlawDetectionForm()
    {   //探伤表Id
        string tsFormId = me[Roughing.FlawDetectionTable] + string.Empty;
        //当前用户ID
        string userId = this.Request.UserContext.UserId;
        //流程节点名称
        string activityName = this.Request.WorkItem + string.Empty == string.Empty
            ? string.Empty : this.Request.WorkItem.ActivityDisplayName;
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (tsFormId == string.Empty)
        {
            string thisId = me[Roughing.ID] + string.Empty; //ID
            string mySql = string.Format("Select " + InspectionTable.Objectid + " From i_" + InspectionTable.TableCode + " Where " + InspectionTable.ID + " = '{0}'", thisId);
            DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                me[Roughing.FlawDetectionTable] = thisId = tsData.Rows[0][InspectionTable.Objectid] + string.Empty;
            }
            else
            {
                H3.DataModel.BizObject tsForm = Tools.BizOperation.New(this.Engine, InspectionTable.TableCode);
                tsForm.Status = H3.DataModel.BizObjectStatus.Effective; //生效
                tsForm[InspectionTable.ID] = me[Roughing.ID] + string.Empty;  //ID
                tsForm[InspectionTable.CurrentOperation] = "粗车";
                tsForm[InspectionTable.SampleProcess] = me.ObjectId;
                //new子表数据集合
                List<H3.DataModel.BizObject> lstObject = new List<H3.DataModel.BizObject>();
                //new一个子表业务对象
                H3.DataModel.BizObject lstArray = Tools.BizOperation.New(this.Engine, InspectionSubTable.TableCode);//子表对象
                lstArray[InspectionSubTable.Process] = "粗车"; //工序
                lstArray[InspectionSubTable.WorkStep] = "";     //工步
                lstObject.Add(lstArray);//将这个子表业务对象添加至子表数据集合中
                tsForm[InspectionSubTable.TableCode] = lstObject.ToArray(); //子表数据赋值
                tsForm.Create();
                me[Roughing.FlawDetectionTable] = tsFormId = tsForm.ObjectId; //探伤表
                me.Update();
            }
        }

        //探伤表不为空时,写入工序信息
        if (tsFormId != string.Empty)
        {
            H3.DataModel.BizObject tsForm = Tools.BizOperation.Load(this.Engine, InspectionTable.TableCode, tsFormId);
            H3.DataModel.BizObject[] lstArray = tsForm[InspectionSubTable.TableCode] as H3.DataModel.BizObject[];  //获取子表数据
            tsForm[InspectionTable.CurrentOperation] = "粗车";
            tsForm[InspectionTable.RoughCutting] = me.ObjectId;
            if (lstArray[lstArray.Length - 1][InspectionSubTable.ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
            {
                lstArray[lstArray.Length - 1][InspectionSubTable.Process] = "粗车"; //工序
                lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] = activityName != "Activity85"
                    ? me[Roughing.CurrentWorkStep] + string.Empty : lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] + string.Empty; //工步
                lstArray[lstArray.Length - 1].Update();
            }
            tsForm.Update();
        }
    }

    /*
    *--Author:fubin
    * 创建添加新的子表行数据
    * @param thisObj 本表单数据
    */
    protected void CreatSublist(H3.DataModel.BizObject thisObj)
    {
        //new一个子表业务对象
        H3.DataModel.BizObject childObj = Tools.BizOperation.New(this.Engine, RoughSubTable.TableCode);
        childObj[RoughSubTable.CountingTask] = thisObj[Roughing.TaskBusinessName] + string.Empty == string.Empty ? "1" : thisObj[Roughing.TaskBusinessName] + string.Empty; //任务名称
        //任务计数
        // childObj[RoughSubTable.CountingTask] =  1 ; 
        //将这个子表业务对象添加至子表数据集合中
        Tools.BizOperation.AddChildBizObject(this.Engine, thisObj, RoughSubTable.TableCode, childObj);
    }

    protected void LoadingFourSideLightConfiguration(H3.DataModel.BizObject planObj, Dictionary<string, bool> boolConfig)
    {
        //获取工艺配置四面光优先级层级
        string lighting = LoadingConfig.GetProcessForm(this.Engine, ProcessConfig.PriorityLevelFourSideLight);
        //读取《配置表》四面光配置
        string globalLighting = LoadingConfig.GetProcessForm(this.Engine, ProcessConfig.GlobalFourSideLightConfiguration);
        //加载对应订单规格表的数据
        H3.DataModel.BizObject productObj = LoadingConfig.GetProductData(this.Engine, planObj);
        //读取《规格表》四面光配置
        string productLighting = productObj != null ? productObj[OrderSpecification.WhetherRoughTurningIsSmoothOnAllSides] + string.Empty : string.Empty;
        //读取《计划表》四面光设置
        string planLighting = planObj != null ? planObj[ABCDProcessPlan.SinglePieceFourSideLightConfiguration] + string.Empty : string.Empty;
        switch (lighting)
        {
            case "配置表":
                //全局四面光配置
                me[Roughing.WhetherFourSidesArePolished] = boolConfig[globalLighting] + string.Empty;
                break;
            case "规格表":
                if (productLighting != string.Empty)
                {
                    //规格四面光配置
                    me[Roughing.WhetherFourSidesArePolished] = boolConfig[productLighting] + string.Empty;
                }
                else
                {
                    //全局四面光配置
                    me[Roughing.WhetherFourSidesArePolished] = boolConfig[globalLighting] + string.Empty;
                }
                break;
            case "计划表":
                if (planLighting != string.Empty)
                {
                    //计划四面光配置
                    me[Roughing.WhetherFourSidesArePolished] = boolConfig[planLighting] + string.Empty;
                }
                else
                {
                    if (productLighting != string.Empty)
                    {
                        //规格四面光配置
                        me[Roughing.WhetherFourSidesArePolished] = boolConfig[productLighting] + string.Empty;
                    }
                    else
                    {
                        //全局四面光配置
                        me[Roughing.WhetherFourSidesArePolished] = boolConfig[globalLighting] + string.Empty;
                    }
                }
                break;
        }
    }

    // //权限检测，与派工有关 //备用暂不用。
    // protected void AutherCheck(H3.SmartForm.SubmitSmartFormResponse response) {
    //     try
    //     {
    //         bool flag = false;                   //是否发起异常
    //         string YesNo = this.Request.BizObject["F0000075"] as string;
    //         if(YesNo != "否")
    //         {
    //             return;
    //         }//是否发起异常
    //         string Active = this.Request.ActivityCode;
    //         //待上机节点
    //         if(Active == "Activity14")
    //         {                                                                                //ID
    //             H3.DataModel.BizObject ABCD = FormTools.GetABCD();
    //             //粗车人员  
    //             string[] ABCDWorkers = ((string[]) ABCD["F0000129"]);
    //             string  thePat = this.Request.WorkItem.Participant;
    //             foreach(string item in ABCDWorkers)
    //             {
    //                 if(item == thePat)
    //                 {
    //                     flag = true;
    //                 }
    //             }
    //             if(!flag)
    //             {
    //                 response.Message = "本工件加工权限已修改,无此工件的加工权限";
    //             }
    //         }
    //     }
    //     catch(Exception ex)
    //     {
    //         response.Errors.Add("ABCD工序计划内无此对象");
    //     }
    // }

    //同步机加工任务信息
    public void UpdateRecordForm(string activityCode)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        //获取本表单子表
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = thisObj[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
        //完成总量
        decimal count = thisObj[Roughing.TotalAmountCompleted] + string.Empty != string.Empty ? decimal.Parse(thisObj[Roughing.TotalAmountCompleted] + string.Empty) : 0;
        //任务计数器
        //int taskNum = count < 1 ? int.Parse(thisObj[Roughing.TaskBusinessName] + string.Empty) - 2 : int.Parse(thisObj[Roughing.TaskBusinessName] + string.Empty) - 1;
        //当前子表行数
        int taskNum = lstArray.Length - 1;
        if (activityCode == "cuchexiaji" && lstArray != null) //粗车下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            // //设备工时系数表-子表
            // H3.DataModel.BizObject[] subObj = null;
            // //设备工时系数表
            // H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask[RoughSubTable.Processor] + string.Empty) as H3.Organization.User;
            //产品参数表
            H3.DataModel.BizObject productObj = Tools.BizOperation.Load(this.Engine, ProductParameter.TableCode, thisObj[Roughing.ProductParameterTable] + string.Empty);
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(this.Engine, MachiningTaskRecord.TableCode);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态
            recordObj[MachiningTaskRecord.OperationName] = "粗车"; //工序
            recordObj[MachiningTaskRecord.TaskType] = thisObj[Roughing.CompleteThisAcquisition] + string.Empty == "已本取" ? "本取后粗车" : "正常粗车"; //任务类型
            recordObj[MachiningTaskRecord.ProductSpecification] = thisObj[Roughing.Specification] + string.Empty; //产品规格
            recordObj[MachiningTaskRecord.ID] = thisObj[Roughing.ID] + string.Empty; //工件ID
            recordObj[MachiningTaskRecord.WorkPieceNumber] = thisObj[Roughing.WorkpieceNumber] + string.Empty; //工件号
            recordObj[MachiningTaskRecord.TaskName] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord.Processor] = currentTask[RoughSubTable.Processor] + string.Empty; //加工者
            recordObj[MachiningTaskRecord.DepartmentName] = employee != null ? employee.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord.StartTime] = currentTask[RoughSubTable.StartTime] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord.DeviceName] = currentTask[RoughSubTable.EquipmentName] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord.DeviceNumber] = currentTask[RoughSubTable.EquipmentNumber] + string.Empty; //设备编号
            recordObj[MachiningTaskRecord.DeviceType] = currentTask[RoughSubTable.EquipmentType] + string.Empty; //设备类型
            recordObj[MachiningTaskRecord.DeviceCoefficient] = currentTask[RoughSubTable.EquipmentTimeCoefficient] + string.Empty; //设备工时系数
            recordObj[MachiningTaskRecord.RollingMethod] = thisObj[Roughing.RollingMethod] + string.Empty; //轧制方式
            recordObj[MachiningTaskRecord.ProcessChipWeight] = thisObj[Roughing.TheAmountOfScrap] + string.Empty; //工艺下屑量
            recordObj[MachiningTaskRecord.WorkLoad] = currentTask[RoughSubTable.ProcessingQuantity] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord.EndTime] = DateTime.Now; //加工结束时间
            recordObj[MachiningTaskRecord.ProcessManHour] = thisObj[Roughing.ProductStandardWorkingHours] + string.Empty; ////本工序产品标准工时
            recordObj[MachiningTaskRecord.UnitmanHour] = currentTask[RoughSubTable.TheProductWorkingHours] + string.Empty; //单件拟定工时
            recordObj[MachiningTaskRecord.TaskManHour] = currentTask[RoughSubTable.PersonWorkingHours] + string.Empty; //任务工时
            if (productObj != null)
            {
                recordObj[MachiningTaskRecord.ProductName] = productObj[ProductParameter.ProductName] + string.Empty; //产品名称
                recordObj[MachiningTaskRecord.LatheProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品类别
                recordObj[MachiningTaskRecord.DrillingProcessingCategory] = productObj[ProductParameter.ProductDrillingCategory] + string.Empty; //产品小类
                recordObj[MachiningTaskRecord.OrderSpecifications] = productObj[ProductParameter.OrderSpecificationNumber] + string.Empty; //产品编号
                recordObj[MachiningTaskRecord.UnitWeightofFinish] = productObj[ProductParameter.FinishedProductUnitWeight] + string.Empty; //成品单重
                recordObj[MachiningTaskRecord.OutsideDiameter] = productObj[ProductParameter.OuterDiameter] + string.Empty; //工件外径
                recordObj[MachiningTaskRecord.InsideDiameter] = productObj[ProductParameter.InnerDiameter] + string.Empty; //工件内径
                recordObj[MachiningTaskRecord.TotalHeight] = productObj[ProductParameter.TotalHeight] + string.Empty; //工件总高
                recordObj[MachiningTaskRecord.Thickness] = productObj[ProductParameter.SliceThickness] + string.Empty; //工件片厚
                recordObj[MachiningTaskRecord.HoleAmount] = productObj[ProductParameter.NumberOfHoles] + string.Empty; //工件孔数
                recordObj[MachiningTaskRecord.Aperture] = productObj[ProductParameter.HoleDiameter] + string.Empty; //工件孔径
            }
            DateTime startTime = recordObj[MachiningTaskRecord.StartTime] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj[MachiningTaskRecord.StartTime] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime); //与现在时间的差值
            recordObj[MachiningTaskRecord.ActualElapsedTime] = delayTime.TotalHours; //实际耗时
            recordObj[MachiningTaskRecord.ApplyAdjust] = currentTask[RoughSubTable.ApplicationDifficultyAdjustment] + string.Empty; //申请调整加工难度
            recordObj[MachiningTaskRecord.DataCode] = thisObj[Roughing.DataCode] + string.Empty; //数据代码
            recordObj.Create();
            currentTask[RoughSubTable.ProcessingRecord] = recordObj.ObjectId;  //当前任务加工记录
            currentTask.Update();
        }

        if (activityCode == "jianyan") //质量检验
        {
            H3.DataModel.BizObject recordObj = null; //机加工任务记录
            string systemUserId = H3.Organization.User.SystemUserId; //系统用户
            if (lstArray != null && lstArray.Length > 0)
            {
                for (int i = taskNum - 1; i >= 0; i--)
                {
                    recordObj = null; //清空机加工任务记录数据值
                    //循环加载机加工任务记录数据
                    recordObj = Tools.BizOperation.Load(this.Engine, MachiningTaskRecord.TableCode, lstArray[i][RoughSubTable.ProcessingRecord] + string.Empty);
                    if (recordObj != null)
                    {
                        recordObj[MachiningTaskRecord.InspectionResults] = i == taskNum ? thisObj[Roughing.InspectionResult] + string.Empty : "合格";  //检验结果
                        recordObj[MachiningTaskRecord.UltrasonicResults] = thisObj[Roughing.FlawDetectionIdentification] + string.Empty; //探伤结果
                        recordObj[MachiningTaskRecord.ActualOutsideDiameter] = thisObj[Roughing.ActualOuterDiameter] + string.Empty; //实际外径
                        recordObj[MachiningTaskRecord.ActualInsideDiameter] = thisObj[Roughing.ActualInnerDiameter] + string.Empty; //实际内径
                        recordObj[MachiningTaskRecord.ActualTotalHeight] = thisObj[Roughing.ActualTotalHeight] + string.Empty; //实际总高
                        recordObj[MachiningTaskRecord.ActualThickness] = thisObj[Roughing.ActualSheetThickness] + string.Empty; //实际片厚
                        recordObj[MachiningTaskRecord.Actualunitweight] = thisObj[Roughing.ActualUnitWeight] + string.Empty; //实际单重
                        recordObj.Update();
                    }
                }
            }
        }
    }

    public void ActivePayroll(string schemaCode, H3.DataModel.BizObject[] bizObjects, string userId)
    {
        H3.IEngine Engine = this.Request.Engine;
        if (activityCode == ActivityJY)
        {
            foreach (H3.DataModel.BizObject bizObject in bizObjects)
            {
                H3.DataModel.BizObject bizObj = H3.DataModel.BizObject.Load(userId, Engine, schemaCode, bizObject[RoughSubTable.ProcessingRecord] + string.Empty, false);

                if (bizObj != null)
                {
                    Tools.WorkFlow.StartWorkflow(Engine, bizObj, userId, true);
                }
            }
        }
    }

    //产品工时
    public void TheProductWorkingHours(string actionName)
    {
        if (actionName == "Submit" && activityCode == ActivitySJ)
        {
            H3.DataModel.BizObject thisObj = this.Request.BizObject;
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //总下屑量
            string totalxx = "";
            //本工序产品参数表工时
            string productTime = "";
            //轧制方式
            string zzMode = thisObj[Roughing.RollingMethod] + string.Empty;
            //车加工类别
            string productType = thisObj[Roughing.ProductCategory] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;
            //获取子表数据
            H3.DataModel.BizObject[] SubArray = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
            foreach (H3.DataModel.BizObject Array in SubArray)
            {
                //设备类型
                string deviceType = Array[RoughSubTable.EquipmentType] + string.Empty;
                //产品类别
                if (productType != string.Empty)
                {
                    //获取设备工时系数模块
                    string command = string.Format("Select {0} From i_{1} Where {2} = '粗车' and {3} = '{4}'",
                        DeviceWorkingHour.ObjectId, DeviceWorkingHour.TableCode, DeviceWorkingHour.OperationName, DeviceWorkingHour.ProductMachiningCategory, productType);
                    DataTable data = this.Engine.Query.QueryTable(command, null);
                    if (data != null && data.Rows != null && data.Rows.Count > 0)
                    {
                        mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, DeviceWorkingHour.TableCode, data.Rows[0][DeviceWorkingHour.ObjectId] + string.Empty, true);
                    }
                }
                //设备工时系数表-子表
                subObj = mtObj != null ? mtObj[EquipmentTimeCoefficientSubtabulation.TableCode] as H3.DataModel.BizObject[] : null;
                if (subObj != null)
                {
                    foreach (H3.DataModel.BizObject item in subObj)
                    {
                        if (deviceType != string.Empty)
                        {        //按设备类型查找
                            if (item[EquipmentTimeCoefficientSubtabulation.EquipmentType] + string.Empty == deviceType)
                            {
                                if (zzMode == "单轧" && item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] != null)
                                {               //单轧工时系数
                                    deviceParam = item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] + string.Empty;
                                }
                                else if (zzMode == "双轧" && item[EquipmentTimeCoefficientSubtabulation.DoubleRollingManHourCoefficient] != null)
                                {               //双轧工时系数
                                    deviceParam = item[EquipmentTimeCoefficientSubtabulation.DoubleRollingManHourCoefficient] + string.Empty;
                                }
                            }
                        }
                    }
                }
                //产品参数表
                H3.DataModel.BizObject productObj = Tools.BizOperation.Load(this.Engine, ProductParameter.TableCode, thisObj[Roughing.ProductParameterTable] + string.Empty);
                if (productObj != null)
                {
                    //产品参数表-单轧工时
                    if (zzMode == "单轧" && productObj[ProductParameter.SingleRoughingMaNHour] != null)
                    {   //根据本表单产品轧制方式从产品参数表中获取"单轧粗车工时"
                        productTime = productObj[ProductParameter.SingleRoughingMaNHour] + string.Empty;
                        //单轧粗车下屑
                        totalxx = productObj[ProductParameter.SingleRollingRoughTurningChip] + string.Empty;
                    }
                    //产品参数表-双轧工时
                    if (zzMode == "双轧" && productObj[ProductParameter.DoubleRoughingManhour] != null)
                    {//根据本表单产品轧制方式从产品参数表中获取"双轧粗车工时"
                        productTime = productObj[ProductParameter.DoubleRoughingManhour] + string.Empty;
                        //双轧粗车下屑
                        totalxx = productObj[ProductParameter.DoubleRollingRoughingChip] + string.Empty;
                    }
                }
                double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品参数表工时转换
                double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
                //赋值-设备工时系数
                Array[RoughSubTable.EquipmentTimeCoefficient] = dParam;
                //赋值-产品标准工时
                me[Roughing.ProductStandardWorkingHours] = pTime;
                //赋值-产品工时
                Array[RoughSubTable.TheProductWorkingHours] = pTime * dParam;
                //赋值-下屑量
                me[Roughing.TheAmountOfScrap] = totalxx;
            }
        }
    }
}