
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.Workflow.Instance;
using H3.DataModel;

public class D001419Szzswrfsp91x3heen4dykgwus0 : H3.SmartForm.SmartFormController
{

    private const string ActivityRK = "Activity75"; //转运活动
    private const string ActivitySJ = "Activity14"; //上机活动
    private const string ActivityXJ = "cuchexiaji"; //下机活动
    private const string ActivityJY = "jianyan";     //检验活动
    private const string ActivitySMGXJ = "Activity140";     //四面光下机
    private const string ActivitySMGSJ = "Activity152";// 四面光上机

    string activityCode = "";    
    string ProcessName = "粗车";
    Dispatch dp = null;    
    H3.DataModel.BizObject me;

    public D001419Szzswrfsp91x3heen4dykgwus0(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        activityCode = this.Request.ActivityCode;  //活动节点编码  
        dp = new Dispatch(this.Request.Engine, (string)me[Roughing.ID]);//派工信息 
    }
    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        var code = this.Request.ActivityCode;
        if (code == ActivityRK)
        {
            string[] r = dp.GetWorkShop(ProcessName);
            if (r.Length >= 2)
            {
                me[Roughing.CurrentWorkshop] = r[0];
                me[Roughing.CurrentLocation] = r[1];
            }
        }
        UserId(response);

        if (!this.Request.IsCreateMode)
        {
            //当前工序
            if (me[Roughing.CurrentOperation] + string.Empty == string.Empty) { me[Roughing.CurrentOperation] = "粗车"; }
            //初始化产品类别
            ProductCategoryUpdate();
            //获取多阶段加工子表
            H3.DataModel.BizObject[] thisLstArray = me[RoughSubTable.TableCode] as H3.DataModel.BizObject[];
            //初始化任务名称
            me[Roughing.TaskBusinessName] = me[Roughing.TaskBusinessName] + string.Empty != string.Empty ? me[Roughing.TaskBusinessName] + string.Empty : "1";

            if (thisLstArray == null)
            {
                //本表单纲目结构
                H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
                //初始化子表
                CreatSublist(me, schema, thisLstArray);
            }

            //统计机加工耗时
            MachiningTime();
            //初始化探伤表
            InitFlawDetectionForm();

            if (this.Request.WorkflowInstance.IsUnfinished)
            {
                //获取工序计划表数据
                H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
                //布尔值转换
                //Dictionary < string, bool > boolConfig;
                var boolConfig = new Dictionary<string, bool>();
                boolConfig.Add("是", true);
                boolConfig.Add("否", false);
                //加载质量配置
                LoadingQAConfiguration(planObj, boolConfig);
            }

            //更新本表单
            me.Update();
        }

        base.OnLoad(response);

        try
        {            //同步实时制造情况
            DataSync.instance.CCSyncData(this.Engine);
        }
        catch (Exception ex)
        {
            response.Errors.Add(System.Convert.ToString(ex));
        }
    }
    private void UserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem sd = new H3.SmartForm.SmartFormResponseDataItem();
        sd.Value = this.Request.UserContext.UserId;
        response.ReturnData.Add("UserId", sd);

    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {

        //提交
        if (actionName == "Submit")
        {
            //66-87  做整理
            //me.CurrentPostValue = postValue;
            MultistageProcessingLogic(activityCode);
            //me.CurrentRow = this.Request.BizObject;
            Dispatcher(activityCode, actionName);
            ProcessRecord(activityCode, actionName);//加工记录


            base.OnSubmit(actionName, postValue, response);



            //多阶段加工新方案升级机加工任务记录
            UpdateRecordForm(actionName, activityCode);

            if (activityCode == ActivityJY && actionName == "Submit")
            {
                Salary slr = new Salary(this.Engine, (string)postValue.Data[Roughing.ID]);
                slr.Save(ProcessName);
                slr.Save("四面光");
            }


            //发起异常
            string strInitiateException = this.Request.BizObject[Roughing.InitiateException] + string.Empty;
            if (strInitiateException == "是")
            {
                //异常工步
                AbnormalStep();
            }
        }

    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作。
    * 
    */
    protected void AbnormalStep()
    {
        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, Roughing.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity127" && strActivityCode != "Activity128")
        {
            //设置异常权限
            this.Request.BizObject[Roughing.Owner] = this.Request.UserContext.UserId;
            //创建发起异常的日志
            logObjectID = ExceptionLog.CreateLog(Roughing.ID, Roughing.CurrentWorkStep, Roughing.CurrentOperation,
                Roughing.ExceptionCategory, Roughing.ExceptionDescription, this.Request.BizObject, this.Engine);
            exceptionBo[Roughing.ObjectIDForUpdateTheExceptionLog] = logObjectID;
            exceptionBo.Update();
        }
        //确认调整意见
        if (strActivityCode == "Activity127")
        {
            //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
            ExceptionLog.UpdateLog(Roughing.ID, Roughing.CurrentWorkStep, Roughing.ExceptionCategory,
                Roughing.ExceptionDescription, this.Request.BizObject, exceptionBo[Roughing.ObjectIDForUpdateTheExceptionLog] + string.Empty, this.Engine);
        }
        //审批确认
        if (strActivityCode == "Activity128")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[Roughing.InitiateException] = "否";
            //异常描述赋值
            exceptionBo[Roughing.ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[Roughing.ExceptionCategory] = "安全异常";
            exceptionBo.Update();
        }
    }

    //派工
    private void Dispatcher(string activityCode, string actionName)
    {

        var b = this.Request.BizObject;

        if (activityCode == ActivityRK && actionName == "Submit")
        {
            b[Roughing.Worker] = dp.GetPerson(ProcessName, (string)b[Roughing.CurrentWorkshop], (BizObject[])b[Roughing.RoughProcessing]);
            //dp.FillPerson(me, ProcessName, "四面光");
        }
        if (activityCode == ActivityXJ && actionName == "Submit")
        {
            b[Roughing.Worker] = dp.GetPerson(ProcessName, (string)b[Roughing.CurrentWorkshop], (BizObject[])b[Roughing.RoughProcessing]);
        }

        if ((activityCode == ActivitySJ) && actionName == "Submit")
        {
            b[Roughing.Worker] = this.Request.ParticipantId;
        }
        if ((activityCode == ActivitySMGSJ) && actionName == "Submit")
        {
            b[Roughing.Worker] = this.Request.ParticipantId;
        }


    }
    //-- Author:zlm
    // 四面光任务记录     
    private void ProcessRecord(string activityCode, string actionName)
    {


        if ((activityCode == ActivitySMGXJ) && actionName == "Submit")
        {
            TaskRecorder taskRecorder = new TaskRecorder(this.Request.Engine, this.Request.BizObject);

            H3.DataModel.BizObject[] subTable = this.Request.BizObject[Roughing.FourSideLight] as H3.DataModel.BizObject[];

            subTable[0][RoughFourLathe.ProcessRecord] = taskRecorder.TaskRecord("粗车", subTable[0]);
        }

    }

    /*
     *--Author:fubin
     * 多阶段加工流程逻辑
     * @param activityCode 流程节点编码
     * @param actionName 按钮名称
     */
    private void MultistageProcessingLogic(string activityCode)
    {
        //获取多阶段加工子表
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
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
                //完成总量小于1时
                if ((me[Roughing.TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[Roughing.TotalAmountCompleted] + string.Empty) < 1)
                {
                    //递增计数器，并更新
                    me[Roughing.TaskBusinessName] = lstArray.Length + 1;
                    //创建添加新的子表行数据
                    CreatSublist(me, schema, lstArray);

                }

                if (lstArray[taskNum][RoughSubTable.Processor] + string.Empty == string.Empty)
                {   //当前加工者
                    lstArray[taskNum][RoughSubTable.Processor] = this.Request.UserContext.UserId;
                }
            }

            //探伤表Id
            string objId = me[Roughing.FlawDetectionTable] + string.Empty;
            //返回探伤结果
            if (activityCode == "Activity85" && objId != string.Empty)
            {
                H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, InspectionTable.TableCode, objId, false);
                //赋值探伤认定
                me[Roughing.FlawDetectionIdentification] = tsForm[InspectionTable.FlawDetectionIdentification] + string.Empty;
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
     * 产品类别为空时，查询产品参数表中的车加工类别
     */
    protected void ProductCategoryUpdate()
    {
        //产品类别更新
        if (me[Roughing.ProductCategory] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = me[Roughing.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable typeData = this.Engine.Query.QueryTable(mySql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[Roughing.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                me[Roughing.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
            }
        }
    }

    /*
    *--Author:fubin
    * 加载质量配置数据
    * @param planObj 工序计划数据
    * @param boolConfig 布尔值字典
    */
    protected void LoadingQAConfiguration(H3.DataModel.BizObject planObj, Dictionary<string, bool> boolConfig)
    {
        //读取《质量配置表》上机前互检优先级顺序
        string crossCheck = LoadingConfig.GetQualityConfigForm(this.Engine, QAConfig.PriorityLevelRoughCarMutualInspection);
        //读取《质量配置表》全局上机前互检配置
        string globalCrossCheck = LoadingConfig.GetQualityConfigForm(this.Engine, QAConfig.RoughCarMutualInspectionBeforeLoading);
        //读取《工序计划表》单件上机前互检配置
        string planCrossCheck = planObj != null ? planObj[ABCDProcessPlan.MutualInspectionBeforeSinglePieceMachineOperation] + string.Empty : string.Empty;
        //加载《订单规格表》数据
        H3.DataModel.BizObject productObj = LoadingConfig.GetProductData(this.Engine, planObj);
        //获取《订单规格表》产品上机前互检配置
        string productCrossCheck = productObj != null ? productObj[OrderSpecification.MutualInspectionBeforeProductOnMachine] + string.Empty : string.Empty;
        switch (crossCheck)
        {
            case "配置表":
                //全局上机前互检
                me[Roughing.MutualInspectionBeforeStartingTheMachine] = boolConfig[globalCrossCheck] + string.Empty;
                break;
            case "计划表":
                if (planCrossCheck != string.Empty)
                {   //计划上机前互检
                    me[Roughing.MutualInspectionBeforeStartingTheMachine] = boolConfig[planCrossCheck] + string.Empty;
                }
                else
                {   //全局上机前互检
                    me[Roughing.MutualInspectionBeforeStartingTheMachine] = boolConfig[globalCrossCheck] + string.Empty;
                }
                break;
            case "产品表":
                if (productCrossCheck != string.Empty)
                {
                    //产品上机前互检
                    me[Roughing.MutualInspectionBeforeStartingTheMachine] = boolConfig[productCrossCheck] + string.Empty;
                }
                else
                {
                    if (planCrossCheck != string.Empty)
                    {   //计划上机前互检
                        me[Roughing.MutualInspectionBeforeStartingTheMachine] = boolConfig[planCrossCheck] + string.Empty;
                    }
                    else
                    {   //全局上机前互检
                        me[Roughing.MutualInspectionBeforeStartingTheMachine] = boolConfig[globalCrossCheck] + string.Empty;
                    }
                }
                break;
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
        string activityName = this.Request.WorkItem.ActivityDisplayName;
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (tsFormId == string.Empty)
        {
            string thisId = me[Roughing.ID] + string.Empty; //ID
            string mySql = string.Format("Select ObjectId From " + InspectionTable.TableCode + " Where " + InspectionTable.ID + " = '{0}'", thisId);
            DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                me[Roughing.FlawDetectionTable] = thisId = tsData.Rows[0][InspectionTable.Objectid] + string.Empty;
            }
            else
            {
                H3.DataModel.BizObjectSchema aSchema = this.Engine.BizObjectManager.GetPublishedSchema(InspectionTable.TableCode);
                H3.DataModel.BizObject tsForm = new H3.DataModel.BizObject(this.Engine, aSchema, userId);
                tsForm.Status = H3.DataModel.BizObjectStatus.Effective; //生效
                tsForm[InspectionTable.ID] = me[Roughing.ID] + string.Empty;  //ID
                tsForm[InspectionTable.CurrentOperation] = "粗车";
                tsForm[InspectionTable.SampleProcess] = me.ObjectId;
                //new子表数据集合
                List<H3.DataModel.BizObject> lstObject = new List<H3.DataModel.BizObject>();
                //new一个子表业务对象
                H3.DataModel.BizObject lstArray = new H3.DataModel.BizObject(this.Engine, aSchema.GetChildSchema(InspectionSubTable.TableCode), H3.Organization.User.SystemUserId);//子表对象
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
            H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, InspectionTable.TableCode, tsFormId, false);
            H3.DataModel.BizObject[] lstArray = tsForm[InspectionSubTable.TableCode] as H3.DataModel.BizObject[];  //获取子表数据
            tsForm[InspectionTable.CurrentOperation] = "粗车";
            tsForm[InspectionTable.RoughCutting] = me.ObjectId;
            if (lstArray[lstArray.Length - 1][InspectionSubTable.ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
            {
                lstArray[lstArray.Length - 1][InspectionSubTable.Process] = "粗车"; //工序
                lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] = activityName != "Activity85" ? me[Roughing.CurrentWorkStep] + string.Empty : lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] + string.Empty;     //工步
                lstArray[lstArray.Length - 1].Update();
            }
            tsForm.Update();
        }
    }

    /*
   *--Author:fubin
   * 创建添加新的子表行数据
   * @param thisObj 本表单数据
   * @param schema 多阶段加工子表纲目结构
   * @param lstArray 多阶段加工子表
   */
    protected void CreatSublist(H3.DataModel.BizObject thisObj, H3.DataModel.BizObjectSchema schema, H3.DataModel.BizObject[] lstArray)
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
        H3.DataModel.BizObject zibiao = new H3.DataModel.BizObject(this.Request.Engine, schema.GetChildSchema(RoughSubTable.TableCode), H3.Organization.User.SystemUserId);//子表对象
        zibiao[RoughSubTable.TaskName] = thisObj[Roughing.TaskBusinessName] + string.Empty == string.Empty ? "1" : thisObj[Roughing.TaskBusinessName] + string.Empty; //任务名称
        lstObject.Add(zibiao);//将这个子表业务对象添加至子表数据集合中
        thisObj[RoughSubTable.TableCode] = lstObject.ToArray(); //子表数据赋值

        thisObj.Update();   //更新对象
    }

    protected void Init(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {
            H3.DataModel.BizObject thisObj = this.Request.BizObject;
            string userId = this.Request.UserContext.UserId; //当前用户ID
            string activityName = this.Request.ActivityCode; //活动节点
            string tsFormId = thisObj[Roughing.FlawDetectionTable] + string.Empty;  //探伤表
            H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode); //子表schema
            H3.DataModel.BizObject[] thisLstArray = thisObj[RoughSubTable.TableCode] as H3.DataModel.BizObject[];  //获取子表数据

            thisObj[Roughing.TaskBusinessName] = thisObj[Roughing.TaskBusinessName] + string.Empty != string.Empty ? thisObj[Roughing.TaskBusinessName] + string.Empty : "1"; //初始化计数器
            if (thisLstArray == null)
            {   //初始化子表
                CreatSublist(thisObj, schema, thisLstArray);
            }

            string bizid = thisObj.ObjectId;

            string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From " + Roughing.TableCode + " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'cuchexiaji' and b.BizObjectId = '{0}'  group by b.bizobjectid", bizid);
            DataTable data = this.Engine.Query.QueryTable(command, null);
            //机加工耗时计算
            if (data != null && data.Rows != null && data.Rows.Count > 0)
            {
                if (data.Rows[0]["utime"] != null)
                {
                    string utimestr = data.Rows[0]["utime"] + string.Empty;
                    double utime = double.Parse(utimestr) / 10000000 / 60;
                    thisObj[Roughing.ActualProcessingTime] = utime;
                }
            }

            //当前工序
            if (thisObj[Roughing.CurrentOperation] + string.Empty == string.Empty)
            {           //当前工序
                thisObj[Roughing.CurrentOperation] = "粗车";
            }
            //产品类别更新
            if (thisObj[Roughing.ProductCategory] + string.Empty == string.Empty)
            {
                string orderSpec = thisObj[Roughing.OrderSpecificationNumber] + string.Empty; //订单规格号
                string mySql = string.Format("Select ObjectId,F0000004 From " + ProductParameter.TableCode + " Where F0000073 = '{0}'", orderSpec);
                DataTable typeData = this.Engine.Query.QueryTable(mySql, null);
                if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
                {
                    thisObj[Roughing.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty; //产品参数表
                    thisObj[Roughing.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty; //产品类型
                }
            }

            if (tsFormId == string.Empty)  //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
            {
                string thisId = thisObj[Roughing.ID] + string.Empty; //ID
                string mySql = string.Format("Select ObjectId From " + InspectionTable.TableCode + " Where F0000001 = '{0}'", thisId);
                DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
                if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
                {
                    thisObj[Roughing.FlawDetectionTable] = thisId = tsData.Rows[0][InspectionTable.Objectid] + string.Empty;
                }
                else
                {
                    H3.DataModel.BizObjectSchema aSchema = this.Engine.BizObjectManager.GetPublishedSchema(InspectionTable.TableCode);
                    H3.DataModel.BizObject tsForm = new H3.DataModel.BizObject(this.Engine, aSchema, userId);
                    tsForm.Status = H3.DataModel.BizObjectStatus.Effective; //生效
                    tsForm[InspectionTable.ID] = thisObj[Roughing.ID] + string.Empty;  //ID
                    tsForm[InspectionTable.CurrentOperation] = "粗车";
                    tsForm[InspectionTable.SampleProcess] = thisObj.ObjectId;
                    List<H3.DataModel.BizObject> lstObject = new List<H3.DataModel.BizObject>();   //new子表数据集合
                    //new一个子表业务对象
                    H3.DataModel.BizObject lstArray = new H3.DataModel.BizObject(this.Engine, aSchema.GetChildSchema(InspectionSubTable.TableCode), H3.Organization.User.SystemUserId);//子表对象
                    lstArray[InspectionSubTable.Process] = "粗车"; //工序
                    lstArray[InspectionSubTable.WorkStep] = "";     //工步
                    lstObject.Add(lstArray);//将这个子表业务对象添加至子表数据集合中
                    tsForm[InspectionSubTable.TableCode] = lstObject.ToArray(); //子表数据赋值
                    tsForm.Create();
                    thisObj[Roughing.FlawDetectionTable] = tsFormId = tsForm.ObjectId; //探伤表
                    thisObj.Update();
                }
            }

            if (tsFormId != string.Empty) //探伤表不为空时,写入工序信息
            {
                H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, InspectionTable.TableCode, tsFormId, false);
                H3.DataModel.BizObject[] lstArray = tsForm[InspectionSubTable.TableCode] as H3.DataModel.BizObject[];  //获取子表数据
                tsForm[InspectionTable.CurrentOperation] = "粗车";
                tsForm[InspectionTable.RoughCutting] = thisObj.ObjectId;
                if (lstArray[lstArray.Length - 1][InspectionSubTable.ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
                {
                    lstArray[lstArray.Length - 1][InspectionSubTable.Process] = "粗车"; //工序
                    lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] = activityName != "Activity85" ? thisObj[Roughing.CurrentWorkStep] + string.Empty : lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] + string.Empty;     //工步
                    lstArray[lstArray.Length - 1].Update();
                }
                tsForm.Update();
            }

            //上机前互检字段
            if (this.Request.WorkflowInstance.IsUnfinished)
            {
                //获取父流程实例对象
                H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);

                var parentId = instance != null ? instance.BizObjectId : "";
                //获取父流程业务对象
                H3.DataModel.BizObject parentObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, ProcessFlow.TableCode, parentId, false);
                string planId = parentObj != null ? parentObj[ProcessFlow.OperationSchedule] + string.Empty : string.Empty;
                //获取工序计划业务对象
                H3.DataModel.BizObject planObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, ABCDProcessPlan.TableCode, planId, false);
                //获取产品的数据Id
                string acObjId = planObj != null && planObj[ABCDProcessPlan.OrderSpecificationTable] != null ? planObj[ABCDProcessPlan.OrderSpecificationTable] + string.Empty : string.Empty;
                //加载产品数据
                H3.DataModel.BizObject acObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, OrderSpecification.TableCode, acObjId, false);
                //查询质量配置
                string mySql = string.Format("Select * from " + QAConfig.TableCode + " ");
                DataTable qcForm = this.Engine.Query.QueryTable(mySql, null);
                //查询订单规格号相同的AC表配置
                string mySqlAC = string.Format("Select F0000136 from " + OrderSpecification.TableCode + " where F0000076 = '{0}'", thisObj[Roughing.OrderSpecificationNumber] + string.Empty);
                DataTable gyFormAC = this.Engine.Query.QueryTable(mySqlAC, null);
                //转换工艺配置为布尔值
                Dictionary<string, bool> qcConfig = new Dictionary<string, bool>();
                qcConfig.Add("是", true);
                qcConfig.Add("否", false);
                //获取ac表是否四面光的值
                string smgConfig = gyFormAC != null && gyFormAC.Rows != null && gyFormAC.Rows.Count > 0 ? gyFormAC.Rows[0][OrderSpecification.WhetherRoughTurningIsSmoothOnAllSides] + string.Empty : string.Empty;
                //获取质量配置上机前互检优先级顺序
                string hjOrder = qcForm.Rows[0][QAConfig.PriorityLevelRoughCarMutualInspection] + string.Empty;
                //是否四面光
                thisObj[Roughing.WhetherFourSidesArePolished] = smgConfig != string.Empty ? qcConfig[smgConfig] + string.Empty : string.Empty;

                if (hjOrder == "配置表")
                {
                    //获取《质量配置表》中“全局上机前互检”并赋值
                    string hjConfig = qcForm.Rows[0][QAConfig.RoughCarMutualInspectionBeforeLoading] + string.Empty;
                    thisObj[Roughing.MutualInspectionBeforeStartingTheMachine] = qcConfig[hjConfig] + string.Empty;
                }

                if (planObj != null)
                {
                    if (hjOrder == "计划表")
                    {
                        //获取《工序计划表》中“单件上机前互检”并赋值
                        string hjConfig = planObj != null ? planObj[ABCDProcessPlan.MutualInspectionBeforeSinglePieceMachineOperation] + string.Empty : string.Empty;
                        if (hjConfig != "")
                        {
                            thisObj[Roughing.MutualInspectionBeforeStartingTheMachine] = qcConfig[hjConfig] + string.Empty;
                        }
                        else
                        {
                            //获取《订单规格表》中“产品上机前互检”并赋值
                            hjConfig = acObj != null ? acObj[OrderSpecification.MutualInspectionBeforeProductOnMachine] + string.Empty : string.Empty;
                            if (hjConfig != "")
                            {
                                thisObj[Roughing.MutualInspectionBeforeStartingTheMachine] = qcConfig[hjConfig] + string.Empty;
                            }
                            else
                            {
                                //获取《质量配置表》中“全局上机前互检”并赋值
                                hjConfig = qcForm.Rows[0][QAConfig.RoughCarMutualInspectionBeforeLoading] + string.Empty;
                                thisObj[Roughing.MutualInspectionBeforeStartingTheMachine] = qcConfig[hjConfig] + string.Empty;
                            }
                        }
                    }
                }
            }


            //更新本表单
            thisObj.Update();

            try
            {            //同步实时制造情况
                DataSync.instance.CCSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }
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
    public void UpdateRecordForm(string actionName, string activityCode)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = thisObj[RoughSubTable.TableCode] as H3.DataModel.BizObject[];  //获取粗加工子表

        //完成总量
        decimal count = thisObj[Roughing.TotalAmountCompleted] + string.Empty != string.Empty ? decimal.Parse(thisObj[Roughing.TotalAmountCompleted] + string.Empty) : 0;
        //任务计数器
        int taskNum = count < 1 ? int.Parse(thisObj[Roughing.TaskBusinessName] + string.Empty) - 2 : int.Parse(thisObj[Roughing.TaskBusinessName] + string.Empty) - 1;
        if (actionName == "Submit" && activityCode == "cuchexiaji" && lstArray != null) //粗车下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask[RoughSubTable.Processor] + string.Empty) as H3.Organization.User;

            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //轧制方式
            string zzMode = thisObj[Roughing.RollingMethod] + string.Empty;
            //产品类别
            string productType = thisObj[Roughing.ProductCategory] + string.Empty;
            //设备类型
            string deviceType = currentTask[RoughSubTable.EquipmentType] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;

            //产品类别
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From " + DeviceWorkingHour.TableCode + " Where F0000001 = '粗车' and F0000002 = '{0}'", productType);//产品类别
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
            H3.DataModel.BizObject productObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, ProductParameter.TableCode, thisObj[Roughing.ProductParameterTable] + string.Empty, false);

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

            //机加工记录模块
            H3.DataModel.BizObjectSchema recordSchema = this.Engine.BizObjectManager.GetPublishedSchema(MachiningTaskRecord.TableCode);
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = new H3.DataModel.BizObject(this.Engine, recordSchema, H3.Organization.User.SystemUserId);
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
            recordObj[MachiningTaskRecord.DeviceCoefficient] = deviceParam; //设备工时系数
            recordObj[MachiningTaskRecord.RollingMethod] = zzMode; //轧制方式
            recordObj[MachiningTaskRecord.ProcessChipWeight] = totalxx; //工艺下屑量
            recordObj[MachiningTaskRecord.WorkLoad] = currentTask[RoughSubTable.ProcessingQuantity] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord.EndTime] = DateTime.Now; //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
            double mScale = currentTask[RoughSubTable.ProcessingQuantity] + string.Empty != string.Empty ? double.Parse(currentTask[RoughSubTable.ProcessingQuantity] + string.Empty) : 0; //加工量转换
            recordObj[MachiningTaskRecord.ProcessManHour] = pTime; ////本工序产品工时
            recordObj[MachiningTaskRecord.UnitmanHour] = pTime * dParam; //单件拟定工时
            recordObj[MachiningTaskRecord.TaskManHour] = pTime * dParam * mScale; //任务工时

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


        if (actionName == "Submit" && activityCode == "jianyan") //质量检验
        {
            H3.DataModel.BizObject recordObj = null; //机加工任务记录
            string systemUserId = H3.Organization.User.SystemUserId; //系统用户

            if (lstArray != null && lstArray.Length > 0)
            {
                for (int i = taskNum - 1; i >= 0; i--)
                {
                    recordObj = null; //清空机加工任务记录数据值
                    //循环加载机加工任务记录数据
                    recordObj = H3.DataModel.BizObject.Load(systemUserId, this.Engine, MachiningTaskRecord.TableCode, lstArray[i][RoughSubTable.ProcessingRecord] + string.Empty, false);
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
}