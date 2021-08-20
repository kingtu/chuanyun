
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.Workflow;
using H3.Organization;

public class D001419Szzswrfsp91x3heen4dykgwus0 : H3.SmartForm.SmartFormController
{
    //H3.SmartForm.LoadSmartFormResponse h3response;

    private const string ActivityRK = "Activity75"; //转运活动
    private const string ActivitySJ = "Activity14"; //上机活动
    private const string ActivityXJ = "cuchexiaji"; //下机活动
    private const string ActivityJY = "jianyan";     //检验活动
    private const string ActivitySMGXJ = "Activity140";     //四面光下机
    private const string ActivitySMGSJ = "Activity152";// 四面光上机

    private const string ID = "F0000067";
    private const string worker = "F0000084";
    private const string currentworkshop = "F0000080";
    private const string currentlocation = "F0000081";

    //sh.FourLathe FourLathe=null;
    ProcessTask pt;
    // TaskRecorder theTaskRecorder;
    string activityCode = "";
    //private H3.DataModel.BizObject B;
    private String RID = null;
    //SmartFormTools FormTools;
    Schema me = null;
    string ProcessName = "粗车";
    string test = null;
    Dispatch dp = null;

    public D001419Szzswrfsp91x3heen4dykgwus0(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        activityCode = this.Request.ActivityCode;  //活动节点编码  
        RID = this.Request.BizObjectId;
        me = new Schema(this.Request.Engine, this.Request.BizObject);
        dp = new Dispatch(this.Request.Engine, (string)me["ID"]);//派工信息 
        pt = new ProcessTask(this.Request);
        // theTaskRecorder = new TaskRecorder(this.Request.Engine);

        //FormTools = new SmartFormTools(this.Request);
        //  var dd=this.Request.BizObject[粗车.ID];
        // FourLathe= new sh.FourLathe(this.Request.BizObject);




    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {


        var code = this.Request.ActivityCode;
        if (code == ActivityRK) { dp.FillWorkShop(me, ProcessName); }
        Init(response); //初始化    
        UserId(response);
        base.OnLoad(response);
        me.GetNew();
        var t1 = me.CurrentRow;
        var t2 = me.And(Schema.RID, "=", this.Request.BizObjectId).GetFirst(false);
        var t3 = this.Request.BizObject;

    }
    private void UserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        //H3.SmartForm.SmartFormResponseDataItem sd = new H3.SmartForm.SmartFormResponseDataItem();
        //sd.Value = this.Request.UserContext.UserId;
        //response.ReturnData.Add("UserId", sd);

    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {

        me.CurrentPostValue = postValue;
        mustHaveAName(actionName, postValue, response);
        updatePR(activityCode, actionName);
        me.CurrentRow = this.Request.BizObject;
        Dispatcher(activityCode, actionName);
        ProcessRecord(activityCode, actionName);//加工记录

        base.OnSubmit(actionName, postValue, response);
        //多阶段加工新方案升级机加工任务记录
        UpdateRecordForm(actionName, activityCode);

        if (activityCode == ActivityJY && actionName == "Submit")
        {
            Salary slr = new Salary(me.Engine, me.PostValue("ID"));
            slr.Save(ProcessName);
            slr.Save("四面光");
        }
        abnormalStep(me, postValue, response);
    }

    //派工
    private void Dispatcher(string activityCode, string actionName)
    {

        if ((activityCode == ActivityRK) && actionName == "Submit")
        {
            dp.FillPerson(me, ProcessName, "四面光");
        }
        if (activityCode == ActivityXJ && actionName == "Submit")
        {
            dp.FillPerson(me, ProcessName);
        }

        if ((activityCode == ActivitySJ) && actionName == "Submit")
        {
            me["工人"] = this.Request.ParticipantId;
        }
        if ((activityCode == ActivitySMGSJ) && actionName == "Submit")
        {
            me["工人"] = this.Request.ParticipantId;
        }


    }
    //任务记录
    private void ProcessRecord(string activityCode, string actionName)
    {


        if ((activityCode == ActivitySMGXJ) && actionName == "Submit")
        {
            H3.DataModel.BizObject[] subTable = this.Request.BizObject["D001419Sgx7flbvwu9r0u3hail6512uq4"] as H3.DataModel.BizObject[];
            // subTable[0]["D0014199e58919544424654bcc75ef1dc953be6.F0000161"] = pt.TaskRecord("四面光", "四面光");
            // subTable[0]["D0014199e58919544424654bcc75ef1dc953be6.F0000161"] = theTaskRecorder.TaskRecord("四面光",subTable);
        }

    }


    private void updatePR(string activityCode, string actionName)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = thisObj["D001419F8cbba24c57a74ad99bd809ab8e262996"] as H3.DataModel.BizObject[];  //获取子表
        string objId = thisObj["F0000167"] + string.Empty; //探伤表

        {       //多阶段加工新方案
            thisObj["F0000133"] = lstArray != null ? lstArray.Length + string.Empty : "1";  //修正任务数
            int taskNum = thisObj["F0000133"] + string.Empty != string.Empty ? int.Parse(thisObj["F0000133"] + string.Empty) - 1 : 0; //获取任务数

            // if(actionName == "Submit" && activityCode == "Activity140") //四面见光
            // {
            //     thisObj["F0000179"] = this.Request.UserContext.UserId; //当前加工者
            //     thisObj["F0000178"] = System.DateTime.Now; //加工开始时间
            // }

            if (actionName == "Submit" && activityCode == "Activity14") //粗车上机
            {
                lstArray[taskNum]["F0000157"] = this.Request.UserContext.UserId; //当前加工者
                lstArray[taskNum]["F0000164"] = System.DateTime.Now; //加工开始时间
            }

            if (actionName == "Submit" && activityCode == "cuchexiaji") //粗车下机
            {
                //完成总量小于1时
                if ((thisObj["F0000090"] + string.Empty) != string.Empty && decimal.Parse(thisObj["F0000090"] + string.Empty) < 1)
                {
                    //递增计数器，并更新
                    thisObj["F0000133"] = taskNum + 2;

                    if (int.Parse(thisObj["F0000133"] + string.Empty) >= lstArray.Length)
                    {   //根据计数器创建添加新的子表行数据
                        CreatSublist(thisObj, schema, lstArray);
                    }
                }

                if (lstArray[taskNum]["F0000157"] + string.Empty == string.Empty)
                {
                    lstArray[taskNum]["F0000157"] = this.Request.UserContext.UserId; //当前加工者
                }
            }

            if (actionName == "Submit" && activityCode == "Activity85" && objId != string.Empty)  //    返回探伤结果
            {
                H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, "D001419fdcaecf556264750ae2d5684b2a3706e", objId, false);
                thisObj["F0000138"] = tsForm["F0000004"] + string.Empty;  //探伤认定
            }

        }

    }



    /*
    <summary>
    关于发起异常之后节点中内容的显示与隐藏及默认值，包含“确认调整意见”，“审批确认”节点。
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    */
    protected void abnormalStep(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {

        var row = me.GetRow(this.Request.BizObjectId);
        //  var postValue = me.CurrentPostValue;
        var Code = this.Request.ActivityCode;
        var actName = this.Request.ActivityTemplate.DisplayName;

        //发起异常
        var startException = this.Request.BizObject["F0000075"] + string.Empty;
        if (startException == "是")
        {
            createLog(me, postValue);
        }

        if (Code == "Activity92")   //???
        {
            createLog(me, postValue);
        }
        if (Code == "Activity93")
        {
            me["发起异常"] = "否";
            me["异常描述"] = "操作错误，重新选择节点";
            me["异常类别"] = "安全异常";
            me.Update(false);
        }
    }
    /*
    <summary>
    把本表单的以下信息同步到《异常工步记录表中》。
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    */
    public void createLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {

        var yc = new Schema(this.Engine, "异常工步记录表");
        yc.GetNew();
        yc["ID"] = me.PostValue("ID");
        yc["工步来源"] = me.PostValue("当前工步");
        yc["工序来源"] = me.PostValue("当前工序");
        yc["异常类别"] = me.PostValue("异常类别");
        yc["异常描述"] = me.PostValue("异常描述");
        yc.Create(true);
    }


    private void mustHaveAName(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (this.Request.BizObject["F0000075"] == "是")
        {
            this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
        }
    }
    protected void Init(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {
            H3.DataModel.BizObject thisObj = this.Request.BizObject;
            string userId = this.Request.UserContext.UserId; //当前用户ID
            string activityName = this.Request.ActivityCode; //活动节点
            string tsFormId = thisObj["F0000167"] + string.Empty;  //探伤表
            H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode); //子表schema
            H3.DataModel.BizObject[] thisLstArray = thisObj["D001419F8cbba24c57a74ad99bd809ab8e262996"] as H3.DataModel.BizObject[];  //获取子表数据

            thisObj["F0000133"] = thisObj["F0000133"] + string.Empty != string.Empty ? thisObj["F0000133"] + string.Empty : "1"; //初始化计数器
            if (thisLstArray == null)
            {   //初始化子表
                CreatSublist(thisObj, schema, thisLstArray);
            }

            string bizid = thisObj.ObjectId;

            string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_D001419Szzswrfsp91x3heen4dykgwus0  a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'cuchexiaji' and b.BizObjectId = '{0}'  group by b.bizobjectid", bizid);
            DataTable data = this.Engine.Query.QueryTable(command, null);
            //机加工耗时计算
            if (data != null && data.Rows != null && data.Rows.Count > 0)
            {
                if (data.Rows[0]["utime"] != null)
                {
                    string utimestr = data.Rows[0]["utime"] + string.Empty;
                    double utime = double.Parse(utimestr) / 10000000 / 60;
                    thisObj["CountTime"] = utime;
                }
            }

            //当前工序
            if (thisObj["F0000083"] + string.Empty == string.Empty)
            {           //当前工序
                thisObj["F0000083"] = "粗车";
            }
            //产品类别更新
            if (thisObj["F0000121"] + string.Empty == string.Empty)
            {
                string orderSpec = thisObj["F0000016"] + string.Empty; //订单规格号
                string mySql = string.Format("Select ObjectId,F0000004 From i_D0014196b62f7decd924e1e8713025dc6a39aa5 Where F0000073 = '{0}'", orderSpec);
                DataTable typeData = this.Engine.Query.QueryTable(mySql, null);
                if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
                {
                    thisObj["F0000116"] = typeData.Rows[0]["ObjectId"] + string.Empty; //产品参数表
                    thisObj["F0000121"] = typeData.Rows[0]["F0000004"] + string.Empty; //产品类型
                }
            }

            if (tsFormId == string.Empty)  //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
            {
                string thisId = thisObj["F0000067"] + string.Empty; //ID
                string mySql = string.Format("Select ObjectId From i_D001419fdcaecf556264750ae2d5684b2a3706e Where F0000001 = '{0}'", thisId);
                DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
                if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
                {
                    thisObj["F0000167"] = thisId = tsData.Rows[0]["ObjectId"] + string.Empty;
                }
                else
                {
                    H3.DataModel.BizObjectSchema aSchema = this.Engine.BizObjectManager.GetPublishedSchema("D001419fdcaecf556264750ae2d5684b2a3706e");
                    H3.DataModel.BizObject tsForm = new H3.DataModel.BizObject(this.Engine, aSchema, userId);
                    tsForm.Status = H3.DataModel.BizObjectStatus.Effective; //生效
                    tsForm["F0000001"] = thisObj["F0000067"] + string.Empty;  //ID
                    tsForm["F0000023"] = "粗车";
                    tsForm["F0000021"] = thisObj.ObjectId;
                    List<H3.DataModel.BizObject> lstObject = new List<H3.DataModel.BizObject>();   //new子表数据集合
                    //new一个子表业务对象
                    H3.DataModel.BizObject lstArray = new H3.DataModel.BizObject(this.Engine, aSchema.GetChildSchema("D001419F89050d4fc56d4bf7b41f343f2e3bd5a1"), H3.Organization.User.SystemUserId);//子表对象
                    lstArray["F0000017"] = "粗车"; //工序
                    lstArray["F0000018"] = "";     //工步
                    lstObject.Add(lstArray);//将这个子表业务对象添加至子表数据集合中
                    tsForm["D001419F89050d4fc56d4bf7b41f343f2e3bd5a1"] = lstObject.ToArray(); //子表数据赋值
                    tsForm.Create();
                    thisObj["F0000167"] = tsFormId = tsForm.ObjectId; //探伤表
                    thisObj.Update();
                }
            }

            if (tsFormId != string.Empty) //探伤表不为空时,写入工序信息
            {
                H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, "D001419fdcaecf556264750ae2d5684b2a3706e", tsFormId, false);
                H3.DataModel.BizObject[] lstArray = tsForm["D001419F89050d4fc56d4bf7b41f343f2e3bd5a1"] as H3.DataModel.BizObject[];  //获取子表数据
                tsForm["F0000023"] = "粗车";
                tsForm["F0000022"] = thisObj.ObjectId;
                if (lstArray[lstArray.Length - 1]["F0000002"] + string.Empty == string.Empty) //探伤结果
                {
                    lstArray[lstArray.Length - 1]["F0000017"] = "粗车"; //工序
                    lstArray[lstArray.Length - 1]["F0000018"] = activityName != "Activity85" ? thisObj["F0000082"] + string.Empty : lstArray[lstArray.Length - 1]["F0000018"] + string.Empty;     //工步
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
                H3.DataModel.BizObject parentObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Sq0biizim9l50i2rl6kgbpo3u4", parentId, false);
                string planId = parentObj != null ? parentObj["F0000126"] + string.Empty : string.Empty;
                //获取工序计划业务对象
                H3.DataModel.BizObject planObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Szlywopbivyrv1d64301ta5xv4", planId, false);
                //获取产品的数据Id
                string acObjId = planObj != null && planObj["F0000145"] != null ? planObj["F0000145"] + string.Empty : string.Empty;
                //加载产品数据
                H3.DataModel.BizObject acObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, "D001419Skniz33124ryujrhb4hry7md21", acObjId, false);
                //查询质量配置
                string mySql = string.Format("Select * from i_D0014198feb957936e040648d486b034af96597");
                DataTable qcForm = this.Engine.Query.QueryTable(mySql, null);
                //查询订单规格号相同的AC表配置
                string mySqlAC = string.Format("Select F0000136 from i_D001419Skniz33124ryujrhb4hry7md21 where F0000076 = '{0}'", thisObj["F0000016"] + string.Empty);
                DataTable gyFormAC = this.Engine.Query.QueryTable(mySqlAC, null);
                //转换工艺配置为布尔值
                Dictionary<string, bool> qcConfig = new Dictionary<string, bool>();
                qcConfig.Add("是", true);
                qcConfig.Add("否", false);
                //获取ac表是否四面光的值
                string smgConfig = gyFormAC != null && gyFormAC.Rows != null && gyFormAC.Rows.Count > 0 ? gyFormAC.Rows[0]["F0000136"] + string.Empty : string.Empty;
                //获取质量配置上机前互检优先级顺序
                string hjOrder = qcForm.Rows[0]["F0000011"] + string.Empty;
                //是否四面光
                thisObj["F0000186"] = smgConfig != string.Empty ? qcConfig[smgConfig] + string.Empty : string.Empty;

                if (hjOrder == "配置表")
                {
                    //获取《质量配置表》中“全局上机前互检”并赋值
                    string hjConfig = qcForm.Rows[0]["F0000012"] + string.Empty;
                    thisObj["F0000184"] = qcConfig[hjConfig] + string.Empty;
                }

                if (planObj != null)
                {
                    if (hjOrder == "计划表")
                    {
                        //获取《工序计划表》中“单件上机前互检”并赋值
                        string hjConfig = planObj != null ? planObj["F0000175"] + string.Empty : string.Empty;
                        if (hjConfig != "")
                        {
                            thisObj["F0000184"] = qcConfig[hjConfig] + string.Empty;
                        }
                        else
                        {
                            //获取《订单规格表》中“产品上机前互检”并赋值
                            hjConfig = acObj != null ? acObj["F0000135"] + string.Empty : string.Empty;
                            if (hjConfig != "")
                            {
                                thisObj["F0000184"] = qcConfig[hjConfig] + string.Empty;
                            }
                            else
                            {
                                //获取《质量配置表》中“全局上机前互检”并赋值
                                hjConfig = qcForm.Rows[0]["F0000012"] + string.Empty;
                                thisObj["F0000184"] = qcConfig[hjConfig] + string.Empty;
                            }
                        }
                    }
                }
            }


            //更新本表单
            thisObj.Update();

            try
            {            //同步实时制造情况
               // DataSync.instance.CCSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }
        }
    }


    //权限检测，与派工有关 //备用暂不用。
    protected void AutherCheck(H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            bool flag = false;                   //是否发起异常
            string YesNo = this.Request.BizObject["F0000075"] as string;
            if (YesNo != "否")
            {
                return;
            }//是否发起异常
            string Active = this.Request.ActivityCode;
            //待上机节点
            if (Active == "Activity14")
            {                                                                                //ID
                //H3.DataModel.BizObject ABCD = FormTools.GetABCD();
                //粗车人员  
                //string[] ABCDWorkers = ((string[])ABCD["F0000129"]);
                //string thePat = this.Request.WorkItem.Participant;
                //foreach (string item in ABCDWorkers)
                //{
                //    if (item == thePat)
                //    {
                //        flag = true;
                //    }
                //}

                //if (!flag)
                //{
                //    response.Message = "本工件加工权限已修改,无此工件的加工权限";

                //}
            }
        }
        catch (Exception ex)
        {
            response.Errors.Add("ABCD工序计划内无此对象");
        }

    }


    //创建添加新的子表行数据
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
        H3.DataModel.BizObject zibiao = new H3.DataModel.BizObject(this.Request.Engine, schema.GetChildSchema("D001419F8cbba24c57a74ad99bd809ab8e262996"), H3.Organization.User.SystemUserId);//子表对象
        zibiao["F0000166"] = thisObj["F0000133"] + string.Empty == string.Empty ? "1" : thisObj["F0000133"] + string.Empty; //任务名称
        lstObject.Add(zibiao);//将这个子表业务对象添加至子表数据集合中
        thisObj["D001419F8cbba24c57a74ad99bd809ab8e262996"] = lstObject.ToArray(); //子表数据赋值

        thisObj.Update();   //更新对象
    }

    //同步机加工任务信息
    public void UpdateRecordForm(string actionName, string activityCode)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = thisObj["D001419F8cbba24c57a74ad99bd809ab8e262996"] as H3.DataModel.BizObject[];  //获取粗加工子表

        //完成总量
        decimal count = thisObj["F0000090"] + string.Empty != string.Empty ? decimal.Parse(thisObj["F0000090"] + string.Empty) : 0;
        //任务计数器
        int taskNum = count < 1 ? int.Parse(thisObj["F0000133"] + string.Empty) - 2 : int.Parse(thisObj["F0000133"] + string.Empty) - 1;
        if (actionName == "Submit" && activityCode == "cuchexiaji" && lstArray != null) //粗车下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask["F0000157"] + string.Empty) as H3.Organization.User;

            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //轧制方式
            string zzMode = thisObj["F0000122"] + string.Empty;
            //产品类别
            string productType = thisObj["F0000121"] + string.Empty;
            //设备类型
            string deviceType = currentTask["F0000160"] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;

            //产品类别
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From i_D0014195ed7e837ecee4f97800877820d9a2f05 Where F0000001 = '粗车' and F0000002 = '{0}'", productType);//产品类别
                DataTable data = this.Engine.Query.QueryTable(command, null);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, "D0014195ed7e837ecee4f97800877820d9a2f05", data.Rows[0]["ObjectId"] + string.Empty, true);
                }

            }

            //设备工时系数表-子表
            subObj = mtObj != null ? mtObj["D001419Fbb7854d117af4bba8eff4de46d128f63"] as H3.DataModel.BizObject[] : null;

            if (subObj != null)
            {
                foreach (H3.DataModel.BizObject item in subObj)
                {
                    if (deviceType != string.Empty)
                    {        //按设备类型查找
                        if (item["F0000004"] + string.Empty == deviceType)
                        {
                            if (zzMode == "单轧" && item["F0000007"] != null)
                            {               //单轧工时系数
                                deviceParam = item["F0000007"] + string.Empty;
                            }
                            else if (zzMode == "双轧" && item["F0000008"] != null)
                            {               //双轧工时系数
                                deviceParam = item["F0000008"] + string.Empty;
                            }
                        }
                    }
                }
            }

            //产品参数表
            H3.DataModel.BizObject productObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", thisObj["F0000116"] + string.Empty, false);

            if (productObj != null)
            {
                //产品参数表-单轧工时
                if (zzMode == "单轧" && productObj["F0000048"] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"单轧粗车工时"
                    productTime = productObj["F0000048"] + string.Empty;
                    //单轧粗车下屑
                    totalxx = productObj["F0000045"] + string.Empty;
                }
                //产品参数表-双轧工时
                if (zzMode == "双轧" && productObj["F0000049"] != null)
                {//根据本表单产品轧制方式从产品参数表中获取"双轧粗车工时"
                    productTime = productObj["F0000049"] + string.Empty;
                    //双轧粗车下屑
                    totalxx = productObj["F0000046"] + string.Empty;
                }
            }

            //机加工记录模块
            H3.DataModel.BizObjectSchema recordSchema = this.Engine.BizObjectManager.GetPublishedSchema("D0014194963919529e44d60be759656d4a16b63");
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = new H3.DataModel.BizObject(this.Engine, recordSchema, H3.Organization.User.SystemUserId);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态

            recordObj["F0000001"] = "粗车"; //工序
            recordObj["F0000031"] = thisObj["F0000134"] + string.Empty == "已本取" ? "本取后粗车" : "正常粗车"; //任务类型
            recordObj["F0000003"] = thisObj["F0000003"] + string.Empty; //产品规格
            recordObj["ID"] = thisObj["F0000067"] + string.Empty; //工件ID
            recordObj["F0000040"] = thisObj["F0000033"] + string.Empty; //工件号
            recordObj["F0000002"] = taskNum; //任务计数器
            recordObj["F0000011"] = currentTask["F0000157"] + string.Empty; //加工者
            recordObj["F0000030"] = employee != null ? employee.DepartmentName : ""; //部门名称
            recordObj["startTime"] = currentTask["F0000164"] + string.Empty; //加工开始时间
            recordObj["F0000007"] = currentTask["F0000158"] + string.Empty; //设备名称
            recordObj["F0000014"] = currentTask["F0000159"] + string.Empty; //设备编号
            recordObj["F0000041"] = currentTask["F0000160"] + string.Empty; //设备类型
            recordObj["F0000008"] = deviceParam; //设备工时系数
            recordObj["F0000024"] = zzMode; //轧制方式
            recordObj["F0000023"] = totalxx; //工艺下屑量
            recordObj["F0000010"] = currentTask["F0000162"] + string.Empty; //任务加工量
            recordObj["EndTime"] = DateTime.Now; //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
            double mScale = currentTask["F0000162"] + string.Empty != string.Empty ? double.Parse(currentTask["F0000162"] + string.Empty) : 0; //加工量转换
            recordObj["F0000004"] = pTime; ////本工序产品工时
            recordObj["F0000005"] = pTime * dParam; //单件拟定工时
            recordObj["F0000006"] = pTime * dParam * mScale; //任务工时

            if (productObj != null)
            {
                recordObj["F0000026"] = productObj["F0000067"] + string.Empty; //产品名称
                recordObj["F0000027"] = productObj["F0000004"] + string.Empty; //产品类别
                recordObj["F0000025"] = productObj["F0000006"] + string.Empty; //产品小类
                recordObj["ProductNum"] = productObj["F0000073"] + string.Empty; //产品编号
                recordObj["F0000017"] = productObj["F0000014"] + string.Empty; //成品单重
                recordObj["F0000016"] = productObj["F0000076"] + string.Empty; //工件外径
                recordObj["F0000018"] = productObj["F0000077"] + string.Empty; //工件内径
                recordObj["F0000020"] = productObj["F0000078"] + string.Empty; //工件总高
                recordObj["F0000019"] = productObj["F0000079"] + string.Empty; //工件片厚
                recordObj["F0000021"] = productObj["F0000080"] + string.Empty; //工件孔数
                recordObj["F0000022"] = productObj["F0000081"] + string.Empty; //工件孔径
            }

            DateTime startTime = recordObj["StartTime"] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj["StartTime"] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime); //与现在时间的差值
            recordObj["F0000013"] = delayTime.TotalHours; //实际耗时
            recordObj["F0000044"] = currentTask["F0000181"] + string.Empty; //申请调整加工难度
            recordObj["F0000015"] = thisObj["F0000076"] + string.Empty; //数据代码
            recordObj.Create();

            currentTask["F0000161"] = recordObj.ObjectId;  //当前任务加工记录
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
                    recordObj = H3.DataModel.BizObject.Load(systemUserId, this.Engine, "D0014194963919529e44d60be759656d4a16b63", lstArray[i]["F0000161"] + string.Empty, false);
                    if (recordObj != null)
                    {
                        recordObj["F0000009"] = i == taskNum ? thisObj["F0000023"] + string.Empty : "合格";  //检验结果
                        recordObj["F0000029"] = thisObj["F0000138"] + string.Empty; //探伤结果

                        recordObj["F0000033"] = thisObj["F0000111"] + string.Empty; //实际外径
                        recordObj["F0000034"] = thisObj["F0000112"] + string.Empty; //实际内径
                        recordObj["F0000035"] = thisObj["F0000113"] + string.Empty; //实际总高
                        recordObj["F0000036"] = thisObj["F0000114"] + string.Empty; //实际片厚
                        recordObj["F0000037"] = thisObj["F0000115"] + string.Empty; //实际单重

                        recordObj.Update();
                    }
                }
            }
        }
    }
}