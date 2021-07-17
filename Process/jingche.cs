using System;
using H3;

public class D001419Sqy2b1uy8h8cahh17u9kn0jk10 : H3.SmartForm.SmartFormController
{
    IEngine Engine;
    H3.SmartForm.SmartFormRequest Request;

    static string ActivitySJ = "Activity3";//上机活动。
    static string ActivityXJ = "Activity24";//下机活动。
    static string ActivityJY = "Activity17";//检验活动。
    private H3.DataModel.BizObject B;
    private String RID = null;

    public D001419Sqy2b1uy8h8cahh17u9kn0jk10(H3.SmartForm.SmartFormRequest request) : base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        B = this.Request.BizObject;
        RID = this.Request.BizObjectId;
        //base.OnLoad(response);
        var me = new Schema(this.Request.Engine, this.Request.SchemaCode, true);
        var code = this.Request.ActivityCode;
        if (code == ActivitySJ) { SetWorkerWithCurrentUser(me); }
        SyncDispatch("精车");
        Init(response);
        me.CellAny("Test", 1, B);
        //me.Update(false);
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        B = this.Request.BizObject;
        RID = this.Request.BizObjectId;
        var code = this.Request.ActivityCode;
        var me = new Schema(this.Engine, postValue, "精车");
        var personTask = GetPersonTask(me);

        if (code == ActivitySJ && actionName == "Submit")
        {
            InsertProductTask(me, "精车", personTask[0], personTask[1], personTask[2]);
            //UpdateRecordForm(actionName);
        }
        if (code == ActivityXJ && actionName == "Submit")
        {
            SetWorkerWithPreParticipants(me);
            UpdateProductTask(me, "精车", personTask[0], Convert.ToDouble(personTask[3]));
            //UpdateRecordForm(actionName);
        }
        if (code == ActivityJY && actionName == "Submit")
        {
            //UpdateRecordForm(actionName);
            UpdateCheckResult(me, "精车");
            UpdateSalaryByJGJL(me, response, "精车");
        }
        mustHaveAName(actionName, postValue, response);
        base.OnSubmit(actionName, postValue, response);
        //异常工步节点日志
        abnormalStep(me, postValue, response);
    }

    protected void UpdateSalaryByJGJL(Schema me, H3.SmartForm.SubmitSmartFormResponse response, string 工序名称)
    {
        //JGJL=加工记录          
        //var 工序名称 = "精车";
        var ID = me.PostValue("ID");
        var scmJGJL = new Schema(me.Engine, me.CurrentPostValue, "加工任务记录");
        var rows = scmJGJL.ClearFilter()
            .Or("检验结果", "=", "利用")
            .Or("检验结果", "=", "合格")
            .And("ID", "=", ID)
            .And("工序名称", "=", 工序名称)
            .GetList();
        foreach (var row in rows)
        {
            scmJGJL.CurrentRow = row;
            var 单件拟定工时 = Convert.ToDouble(scmJGJL.CellAny("单件拟定工时"));
            var 加工数量 = Convert.ToDouble(scmJGJL.CellAny("加工数量"));
            var 工价 = 28;
            var 加工难度 = 1;
            var 加工人员 = scmJGJL.Cell("加工人员");
            var 设备名称 = scmJGJL.Cell("设备名称");
            var 部门名称 = scmJGJL.Cell("部门名称");
            var 检验结果 = scmJGJL.Cell("检验结果");
            var 任务名称 = scmJGJL.Cell("任务名称");
            var 任务类别 = scmJGJL.Cell("任务类别");

            var 总工时 = 单件拟定工时 * 加工数量;
            var 工时工资 = 总工时 * 工价 * 加工难度;

            var 工艺下屑重量 = Convert.ToDouble(scmJGJL.CellAny("工艺下屑重量"));
            var 总下屑量 = 工艺下屑重量 * 加工数量;//* 分配比例;

            var 补刀金额 = 工时工资 * 0.0875;
            var 总工作量 = 0;

            var scmGZ = new Schema(me.Engine, me.CurrentPostValue, "任务绩效表");
            var rowGZ = scmGZ.And("ID", "=", me.Cell("ID"))
                .And("工序名称", "=", 工序名称)
                .And("任务类别", "=", 任务类别)
                .GetFirst(true);
            if (rowGZ == null) { scmGZ.GetNew(); }
            scmGZ.CellAny("工序名称", 工序名称);
            scmGZ.CellAny("任务名称", 任务名称);
            scmGZ.CellAny("任务类别", 任务类别);

            scmGZ.CellAny("ID", ID);
            scmGZ.CellAny("检验结果", 检验结果);
            scmGZ.CellAny("加工人员", 加工人员);
            scmGZ.CellAny("部门名称", 部门名称);
            scmGZ.CellAny("设备名称", 设备名称);
            scmGZ.CellAny("单件拟定工时", 单件拟定工时);
            scmGZ.CellAny("加工数量", 加工数量);
            scmGZ.CellAny("总工时", 总工时);
            scmGZ.CellAny("工价", 工价);
            scmGZ.CellAny("工时工资", 工时工资);
            scmGZ.CellAny("工艺下屑重量", 工艺下屑重量);
            scmGZ.CellAny("总下屑量", 总下屑量);
            scmGZ.CellAny("总工作量", 总工作量);
            scmGZ.CellAny("补刀金额", 补刀金额);
            if (rowGZ == null) { scmGZ.Create(true); return; }
            scmGZ.Update(true);
        }

    }
    public void SyncDispatch(string 工序名称)
    {
        Schema me = new Schema(this.Request.Engine, this.Request.SchemaCode, true);
        me.GetRow(RID);

        Dispatch dp = new Dispatch(this.Request.Engine, me.Cell("ID"));//派工信息
        bool order = dp.GetOrder(工序名称);
        string[] person = dp.GetPerson(工序名称);
        string[] manager = dp.GetManager(工序名称, me.Cell("车间位置"));
        string[] productor = null;

        if (order)
        {
            var p1 = me.CellAny("加工量1");
            var p2 = me.CellAny("加工量2");
            var p3 = me.CellAny("加工量3");
            if (person == null || person.Length == 0)
            {
                productor = manager;
            }
            else if (p1 == null)
            {
                if (person.Length > 0)
                {
                    productor = new string[] { person[0] };
                }
            }
            else if (p2 == null)
            {
                if (person.Length > 1)
                {
                    productor = new string[] { person[1] };
                }
            }
            else if (p3 == null) //|| string.IsNullOrEmpty(p3.ToString())
            {
                if (person.Length > 2)
                {
                    productor = new string[] { person[2] };
                }
            }
        }
        else
        {
            if (person == null || person.Length == 0)
            {
                productor = manager;
            }
            else
            {
                productor = person;
            }
        }
        me.CellAny("工人", productor);
        me.CellAny("工人", productor, B);
        me.Update(false);

    }
    public void SetWorkerWithPreParticipants(Schema me)
    {
        me.GetRow(RID);
        var person = Role.Participants(this.Request, -1);
        me.CellAny("工人", person);
        me.CellAny("工人", person, B);
        me.Update(false);
    }
    public void SetWorkerWithCurrentUser(Schema me)
    {
        me.GetRow(RID);
        string currentUserId = this.Request.UserContext.UserId;
        me.CellAny("工人", currentUserId);
        me.CellAny("工人", currentUserId, B);
        me.Update(false);
        FillProcessor(me, currentUserId);
    }
    public void FillProcessor(Schema me, string Processor)
    {
        me.GetRow(RID);
        var p1 = me.Cell("第一加工者");
        var p2 = me.Cell("第二加工者");
        var p3 = me.Cell("第三加工者");
        if (string.IsNullOrEmpty(p1))
        {
            me.CellAny("第一加工者", Processor, B);
        }
        else if (string.IsNullOrEmpty(p2))
        {
            me.CellAny("第二加工者", Processor, B);
        }
        else if (string.IsNullOrEmpty(p3))
        {
            me.CellAny("第三加工者", Processor, B);
        }
    }
    public string[] GetPersonTask(Schema me)
    {
        string[] personTask = new string[4] { null, null, null, null };
        me.CurrentRow = B;
        var p1 = me.Cell("第一加工者");
        var p2 = me.Cell("第二加工者");
        var p3 = me.Cell("第三加工者");
        if (!string.IsNullOrEmpty(p3))
        {
            personTask[0] = "3";
            personTask[1] = p3;
            personTask[2] = me.Cell("设备类型3");
            personTask[3] = Convert.ToString(me.CellAny("加工量3"));
        }
        else if (!string.IsNullOrEmpty(p2))
        {
            personTask[0] = "2";
            personTask[1] = p2;
            personTask[2] = me.Cell("设备类型2");
            personTask[3] = Convert.ToString(me.CellAny("加工量2"));
        }
        else if (!string.IsNullOrEmpty(p1))
        {
            personTask[0] = "1";
            personTask[1] = p1;
            personTask[2] = me.Cell("设备类型1");
            personTask[3] = Convert.ToString(me.CellAny("加工量1"));
        }
        return personTask;
    }

    public void InsertProductTask(Schema me, string processName, string taskType, string processorId, string deviceType)
    {
        var scmTask = new Schema(me.Engine, (H3.SmartForm.SmartFormPostValue)null, "加工任务记录");
        var scmPara = new Schema(me.Engine, (H3.SmartForm.SmartFormPostValue)null, "产品参数表");
        me.CurrentRow = B;
        string 订单规格号 = me.Cell("订单规格号");
        string 工件号 = me.Cell("工件号");
        var row = scmTask.And("ID", "=", me.Cell("ID"))
            .And("工序名称", "=", processName)
            .And("任务类别", "=", taskType)
            .GetFirst(true);
        if (row == null) { scmTask.GetNew(); }
        scmPara.And("产品编号", "=", 订单规格号).GetFirst(true);
        scmTask.Copy(me);
        scmTask.Copy(scmPara);
        scmTask.Cell("ID", me.Cell("ID"));
        scmTask.Cell("工序名称", processName);
        scmTask.Cell("任务类别", taskType);
        ProductTime pt = new ProductTime(me.Engine);
        var 单件拟定工时 = pt.GetTime(订单规格号, 工件号, deviceType, processName);
        scmTask.CellAny("单件拟定工时", 单件拟定工时);
        scmTask.CellAny("上机时间", System.DateTime.Now);
        scmTask.CellAny("工艺下屑重量", pt.Dust);
        scmTask.Cell("设备名称", me.Cell("使用设备"));
        scmTask.Cell("加工人员", processorId);
        scmTask.Cell("部门名称", Role.GetDepartmentName(me.Engine, processorId));
        scmTask.CellAny("加工数量", 0);
        if (row == null) { scmTask.Create(); return; }
        scmTask.Update();
    }
    public void UpdateProductTask(Schema me, string processName, string taskType, double quantity)
    {
        var scmTask = new Schema(me.Engine, (H3.SmartForm.SmartFormPostValue)null, "加工任务记录");
        me.CurrentRow = B;
        var row = scmTask.And("ID", "=", me.Cell("ID"))
            .And("工序名称", "=", processName)
            .And("任务类别", "=", taskType)
            .GetFirst(true);
        if (row == null) { return; }
        var 上机时间 = Convert.ToDateTime(scmTask.CellAny("上机时间"));
        var now = System.DateTime.Now;
        scmTask.CellAny("下机时间", now);
        TimeSpan t = now.Subtract(上机时间);
        scmTask.CellAny("实际耗时", t.Seconds);
        scmTask.CellAny("任务工时", "");
        scmTask.CellAny("加工数量", quantity);
        scmTask.Update();
    }
    public void UpdateCheckResult(Schema me, string processName)
    {
        var scmTask = new Schema(me.Engine, (H3.SmartForm.SmartFormPostValue)null, "加工任务记录");
        me.CurrentRow = B;
        var rows = scmTask.And("ID", "=", me.Cell("ID"))
            .And("工序名称", "=", processName)
            .GetList();
        foreach (var row in rows)
        {
            scmTask.CurrentRow = row;
            scmTask.CellAny("实际外径", me.CellAny("实际外径"));
            scmTask.CellAny("检验结果", me.CellAny("检验结果"));
            scmTask.Update(true);
        }
    }

    protected void Init(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {                         //本表单数据
            H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId,
                this.Engine, this.Request.SchemaCode, RID, false);
            if (thisObj != null)
            {
                //当前工序
                // if(B["F0000069"] == null)
                // {           //当前工序
                //     thisObj["F0000069"] = "精车";
                // }
                //产品类别更新
                if (B["F0000111"] == null)
                {                          //产品参数表ObjectId
                    if (B["F0000104"] != null)
                    {                      //产品参数表
                        H3.DataModel.BizObject pfObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", B["F0000104"].ToString(), false);
                        if (pfObj != null)
                        {
                            if (pfObj["F0000004"] != null)
                            {           //产品类别-本表    //产品类别-产品参数表
                                thisObj["F0000111"] = pfObj["F0000004"].ToString();
                            }
                        }
                    }

                }

                if (thisObj["F0000118"] == null)
                {   //任务名称
                    thisObj["F0000118"] = 0;
                }
                //更新本表单
                thisObj.Update();
            }
        }

        try
        {
            //DataSync.instance.JCSyncData(this.Engine);
        }
        catch (Exception ex)
        {
            response.Errors.Add(System.Convert.ToString(ex));
        }
    }
    private void mustHaveAName(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (B["F0000059"] == "是")
        {
            B["OwnerId"] = this.Request.UserContext.UserId;
        }
    }

    protected void abnormalStep(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        var row = me.GetRow(RID);
        var Code = this.Request.ActivityCode;
        //发起异常
        var startException = B["F0000059"] + string.Empty;
        if (startException == "是")
        {
            //createLog(me, postValue);
        }
        if (Code == "Activity55")
        {
            //updateLog(me, postValue);
        }
        if (Code == "Activity56")
        {
            me.Cell("发起异常", "否");
            me.Cell("转至工步", "待转运 ");
            me.Cell("异常描述", "操作错误，重新选择节点");
            me.Cell("异常类别", "安全异常");
            me.Update(false);
        }
    }
    public void createLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {

        var scmAbnormalType = new Schema(this.Engine, postValue, "异常工步记录表");
        scmAbnormalType.GetNew();
        //当前工序
        var currentProcess = me.PostValue("当前工序");
        //  当前工步
        var currentWorkStep = me.PostValue("当前工步");
        //异常类别
        var abNormalType = me.PostValue("异常类别");
        //ID
        var ID = me.PostValue("ID");
        //异常描述
        var abNormalDescibe = me.PostValue("异常描述");

        //ID
        scmAbnormalType.Cell("ID", ID);

        //工步来源
        scmAbnormalType.Cell("工步来源", currentWorkStep);
        //工序来源
        scmAbnormalType.Cell("工序来源", currentProcess);

        //异常类别
        scmAbnormalType.Cell("异常类别", abNormalType);
        //异常描述
        scmAbnormalType.Cell("异常描述", abNormalDescibe);

        scmAbnormalType.Create(true);
    }
    public void updateLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {

        //异常工步记录表
        var scmAbnormalType = new Schema(this.Engine, postValue, "异常工步记录表");
        //ID
        var ID = me.PostValue("ID");
        //  当前工步
        var currentWorkStep = me.PostValue("当前工步");
        var exceptDscribe = me.PostValue("异常描述");
        scmAbnormalType.ClearFilter()
            .And("ID", "=", ID)
            .And("工步来源", "=", currentWorkStep)
            .GetFirst(true);
        scmAbnormalType.Cell("异常描述", exceptDscribe);
        scmAbnormalType.Update(true);


    }
    public H3.DataModel.BizObject AddRecord(string employeeId, string machine)
    {       //向产品参数表新增机加工记录
        H3.DataModel.BizObjectSchema rSchema = this.Engine.BizObjectManager.GetPublishedSchema("D0014194963919529e44d60be759656d4a16b63");

        H3.DataModel.BizObject rBo = new H3.DataModel.BizObject(this.Engine, rSchema, H3.Organization.User.SystemUserId);

        rBo.Status = H3.DataModel.BizObjectStatus.Effective;

        H3.DataModel.BizObject pfObj = null;  //产品参数表

        if (B["F0000104"] != null)
        {   //加载产品参数表
            pfObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", B["F0000104"].ToString(), false);
        }

        if (pfObj != null)
        {
            if (pfObj["F0000014"] != null)
            {                   //产品参数表-成品单重
                rBo["F0000017"] = pfObj["F0000014"].ToString();
            }

            //产品编号
            rBo["ProductNum"] = pfObj["F0000073"] != null ? pfObj["F0000073"] + string.Empty : "";

            if (pfObj["F0000067"] != null)
            {                   //产品参数表-产品名称
                rBo["F0000026"] = pfObj["F0000067"].ToString();
            }

            if (pfObj["F0000004"] != null)
            {                   //产品参数表-产品类别
                rBo["F0000027"] = pfObj["F0000004"].ToString();
            }

            if (pfObj["F0000006"] != null)
            {                   //产品参数表-产品小类
                rBo["F0000025"] = pfObj["F0000006"].ToString();
            }

            if (pfObj["F0000008"] != null)
            {                   //产品参数表-外径
                rBo["F0000016"] = pfObj["F0000008"].ToString();
            }

            if (pfObj["F0000009"] != null)
            {                   //产品参数表-内径
                rBo["F0000018"] = pfObj["F0000009"].ToString();
            }

            if (pfObj["F0000010"] != null)
            {                   //产品参数表-总高
                rBo["F0000020"] = pfObj["F0000010"].ToString();
            }

            if (pfObj["F0000011"] != null)
            {                   //产品参数表-片厚
                rBo["F0000019"] = pfObj["F0000011"].ToString();
            }

            if (pfObj["F0000012"] != null)
            {                   //产品参数表-孔数
                rBo["F0000021"] = pfObj["F0000012"].ToString();
            }

            if (pfObj["F0000013"] != null)
            {                   //产品参数表-孔径
                rBo["F0000022"] = pfObj["F0000013"].ToString();
            }

        }

        if (B["F0000003"] != null)
        {
            //产品规格
            rBo["F0000003"] = B["F0000003"].ToString();
        }
        if (B["F0000062"] != null)
        {        //数据代码                         //数据代码-本表
            rBo["F0000015"] = B["F0000062"].ToString();
        }

        rBo["ID"] = B["F0000053"].ToString();
        //工序
        rBo["F0000001"] = "精车";
        //任务类型
        rBo["F0000031"] = "正常精车";
        //加工者
        rBo["F0000011"] = employeeId;

        H3.Organization.User employee = this.Engine.Organization.GetUnit(employeeId) as H3.Organization.User;

        if (employee != null)
        {   //部门名称
            rBo["F0000030"] = employee.DepartmentName;
        }
        //设备名称
        rBo["F0000007"] = machine;
        //加工开始时间
        rBo["startTime"] = DateTime.Now;

        rBo.Create();

        return rBo;
    }

    public void UpdateRecordForm(string actionName)
    {
        //节点编码
        string code = this.Request.ActivityCode;
        //上机机-提交
        if (actionName == "Submit" && code == "Activity3")
        {    //加工设备
            string machine = "";
            //第一加工者/设备名称
            // if(B["F0000087"] + string.Empty == "" && B["F0000092"] + string.Empty != "")
            // {
            //     B["F0000087"] = this.Request.UserContext.UserId;
            // }                            //第二加工者/设备名称
            // else if(B["F0000088"] + string.Empty == "" && B["F0000094"] + string.Empty != "")
            // {
            //     B["F0000088"] = this.Request.UserContext.UserId;
            // }                            //第三加工者/设备名称
            // else if(B["F0000090"] + string.Empty == "" && B["F0000096"] + string.Empty != "")
            // {
            //     B["F0000090"] = this.Request.UserContext.UserId;
            // }
            //第三加工者
            if (B["F0000090"] != null && B["F0000090"].ToString() != "")
            {
                if (B["F0000096"] != null)
                {                                   //第三设备
                    machine = B["F0000096"] + string.Empty;
                }

                H3.DataModel.BizObject item = AddRecord(B["F0000090"].ToString(), machine);
                //在本表单记录加工者的机加工记录
                B["F0000112"] = item.ObjectId;
                // item["F0000013"] = System.Convert.ToInt32(DateTime.Now.Substract(B["ModifiedTime"]))
            }                           //第二加工者
            else if (B["F0000088"] != null && B["F0000088"].ToString() != "")
            {
                if (B["F0000094"] != null)
                {                                   //第二设备
                    machine = B["F0000094"] + string.Empty;
                }
                //向产品参数表新增机加工记录
                H3.DataModel.BizObject item = AddRecord(B["F0000088"].ToString(), machine);
                //在本表单记录加工者的机加工记录
                B["F0000113"] = item.ObjectId;

            }                           //第一加工者
            else if (B["F0000087"] != null && B["F0000087"].ToString() != "")
            {
                if (B["F0000092"] != null)
                {                               //第一设备                     
                    machine = B["F0000092"] + string.Empty;
                }
                //向产品参数表新增机加工记录
                H3.DataModel.BizObject item = AddRecord(B["F0000087"].ToString(), machine);
                //在本表单记录加工者的机加工记录
                B["F0000114"] = item.ObjectId;

            }
        }
        //下机提交
        if (actionName == "Submit" && code == "Activity24")
        {
            H3.DataModel.BizObject record = null;  //机加工记录表
            H3.DataModel.BizObject[] mtObj = null;   //设备工时系数表
            H3.DataModel.BizObject[] sObj = null;   //设备工时系数表-子表
            H3.DataModel.BizObject pfObj = null;  //产品参数表

            string mtNum = ""; //设备工时系数
            string pTime = ""; //本工序产品工时
            string outerd = "";//外径

            if (B["F0000104"] != null)
            {   //加载产品参数表
                pfObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", B["F0000104"].ToString(), false);
            }

            if (pfObj != null)
            {        //从产品参数表中获取本工序产品工时
                pTime = pfObj["F0000051"].ToString();

                if (pfObj["F0000008"] != null)
                {               //产品参数表表-外径
                    outerd = pfObj["F0000008"].ToString();
                }
            }
            //产品类型  ？？？产品参数表
            if (B["F0000111"] != null)
            {
                H3.DataModel.BizObjectSchema schema = this.Engine.BizObjectManager.GetPublishedSchema("D0014195ed7e837ecee4f97800877820d9a2f05");   //获取设备工时系数模块

                H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();  //构建过滤器

                H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();    //构造And匹配器
                //当前工序
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("F0000001", H3.Data.ComparisonOperatorType.Equal, "精车")); //添加查询条件
                //产品类型                                                            //产品类型-本表
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("F0000002", H3.Data.ComparisonOperatorType.Equal, B["F0000111"].ToString())); //添加查询条件            

                filter.Matcher = andMatcher;
                //设备工时系数表
                mtObj = H3.DataModel.BizObject.GetList(this.Engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
            }

            if (mtObj != null)
            {           //设备工时系数表-子表
                sObj = mtObj[0]["D001419Fbb7854d117af4bba8eff4de46d128f63"] as H3.DataModel.BizObject[];
            }
            //加工记录3 ObjectId                            //加工量3
            if (B["F0000112"] != null && B["F0000091"] != null)
            {                                           //加工量3
                string quantity = B["F0000091"].ToString();
                //加载机加工记录表
                record = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014194963919529e44d60be759656d4a16b63", B["F0000112"].ToString(), false);
                if (record != null)
                {
                    //外径
                    record["F0000016"] = outerd;
                    //加工量3
                    record["F0000010"] = quantity;
                    //任务名称
                    record["F0000002"] = B["F0000118"].ToString();
                    //设备编号3
                    record["F0000014"] = B["F0000097"].ToString();
                    //遍列设备工时系数-子表
                    foreach (H3.DataModel.BizObject item in sObj)
                    {
                        if (pfObj != null && B["F0000103"] != null)
                        {        //设备类型- 设备工时系数表                           //设备类型-本表
                            if (item["F0000004"].ToString() == B["F0000103"].ToString())
                            {             //设备工时系数
                                mtNum = item["F0000007"].ToString();
                            }
                        }
                    }
                    //加工结束时间
                    record["EndTime"] = DateTime.Now;
                    DateTime startTime = Convert.ToDateTime(record["StartTime"].ToString());
                    DateTime now = DateTime.Now;
                    TimeSpan delayTime = now.Subtract(startTime);
                    //实际耗时
                    double min = delayTime.TotalHours;
                    //机加工记录表-实际耗时
                    record["F0000013"] = min;

                    if (mtNum != "")
                    {       //设备工时系数
                        record["F0000008"] = mtNum;
                    }

                    if (pTime != "")
                    {       //本工序产品工时
                        record["F0000004"] = pTime;

                        if (mtNum != "")
                        {       //预计工时               本工序产品工时 * 设备工时系数
                            record["F0000005"] = double.Parse(pTime) * double.Parse(mtNum);
                            //任务工时               本工序产品工时 * 设备工时系数 * 加式量
                            record["F0000006"] = double.Parse(pTime) * double.Parse(mtNum) * double.Parse(quantity);
                        }
                    }

                    record.Update();
                }
            }                             //加工记录2 ObjectId                            //加工量2
            else if (B["F0000113"] != null && B["F0000089"] != null)
            {

                //加工量2
                string quantity = B["F0000089"].ToString();
                //加载机加工记录表
                record = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014194963919529e44d60be759656d4a16b63", B["F0000113"].ToString(), false);
                if (record != null)
                {
                    //外径
                    record["F0000016"] = outerd;
                    //加工量2
                    record["F0000010"] = quantity;
                    //任务名称
                    record["F0000002"] = B["F0000118"].ToString(); ;
                    //设备编号2
                    record["F0000014"] = B["F0000095"].ToString();
                    //遍列设备工时系数-子表
                    foreach (H3.DataModel.BizObject item in sObj)
                    {
                        if (pfObj != null && B["F0000102"] != null)
                        {          //设备类型- 设备工时系数表                         //设备类型-本表
                            if (item["F0000004"].ToString() == B["F0000102"].ToString())
                            {           //设备工时系数
                                mtNum = item["F0000007"].ToString();
                            }
                        }
                    }
                    //加工结束时间
                    record["EndTime"] = DateTime.Now;
                    DateTime startTime = Convert.ToDateTime(record["StartTime"].ToString());
                    DateTime now = DateTime.Now;
                    TimeSpan delayTime = now.Subtract(startTime);
                    //实际耗时
                    double min = delayTime.TotalHours;
                    //机加工记录表表-实际耗时
                    record["F0000013"] = min;

                    if (mtNum != "")
                    {       //设备工时系数
                        record["F0000008"] = mtNum;
                    }

                    if (pTime != "")
                    {   //本工序产品工时
                        record["F0000004"] = pTime;

                        if (mtNum != "")
                        {          //预计工时               本工序产品工时 * 设备工时系数
                            record["F0000005"] = double.Parse(pTime) * double.Parse(mtNum);
                            //任务工时               本工序产品工时 * 设备工时系数 * 加式量
                            record["F0000006"] = double.Parse(pTime) * double.Parse(mtNum) * double.Parse(quantity);
                        }
                    }

                    record.Update();
                }
            }                             //加工记录1 ObjectId                            //加工量1
            else if (B["F0000114"] != null && B["F0000085"] != null)
            {

                //加工量1
                string quantity = B["F0000085"].ToString();
                //加载机加工记录表
                record = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014194963919529e44d60be759656d4a16b63", B["F0000114"].ToString(), false);
                if (record != null)
                {
                    //外径
                    record["F0000016"] = outerd;
                    //加工量1
                    record["F0000010"] = quantity;
                    //任务名称
                    record["F0000002"] = B["F0000118"].ToString();
                    //设备编号1
                    record["F0000014"] = B["F0000093"].ToString();
                    //遍列设备工时系数-子表
                    foreach (H3.DataModel.BizObject item in sObj)
                    {
                        if (pfObj != null && B["F0000101"] != null)
                        {        //设备类型- 设备工时系数表                        //设备类型-本表      
                            if (item["F0000004"].ToString() == B["F0000101"].ToString())
                            {           //设备工时系数
                                mtNum = item["F0000007"].ToString();
                            }
                        }
                    }
                    //加工结束时间
                    record["EndTime"] = DateTime.Now;
                    DateTime startTime = Convert.ToDateTime(record["StartTime"].ToString());
                    DateTime now = DateTime.Now;
                    TimeSpan delayTime = now.Subtract(startTime);
                    //实际耗时
                    double min = delayTime.TotalHours;
                    //机加工记录表表-实际耗时
                    record["F0000013"] = min;

                    if (mtNum != "")
                    {       //设备工时系数
                        record["F0000008"] = mtNum;
                    }

                    if (pTime != "")
                    {   //本工序产品工时
                        record["F0000004"] = pTime;

                        if (mtNum != "")
                        {          //预计工时               本工序产品工时 * 设备工时系数
                            record["F0000005"] = double.Parse(pTime) * double.Parse(mtNum);
                            //任务工时               本工序产品工时 * 设备工时系数 * 加式量
                            record["F0000006"] = double.Parse(pTime) * double.Parse(mtNum) * double.Parse(quantity);
                        }
                    }
                    //更新机加工记录表
                    record.Update();
                }
            }

        }

        //质量检验
        if (actionName == "Submit" && code == "Activity17")
        {
            //加工记录ObjectIds
            string[] records = { "F0000114", "F0000113", "F0000112" };

            H3.DataModel.BizObject record1 = null; //加工记录1
            H3.DataModel.BizObject record2 = null; //加工记录2
            H3.DataModel.BizObject record3 = null; //加工记录3

            int count = 0; //加工者数量

            foreach (string item in records)
            {
                if (B[item] != null && B[item].ToString().Length > 6)
                {
                    ++count; //统计加工者数量

                    if (count == 1)
                    {  //加载加工记录1
                        record1 = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014194963919529e44d60be759656d4a16b63", B[item].ToString(), false);
                    }

                    if (count == 2)
                    {   //加载加工记录2
                        record2 = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014194963919529e44d60be759656d4a16b63", B[item].ToString(), false);
                    }

                    if (count == 3)
                    {   //加载加工记录3
                        record3 = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014194963919529e44d60be759656d4a16b63", B[item].ToString(), false);
                    }
                }
            }

            H3.DataModel.BizObject[] bizRecords = { record1, record2, record3 };

            foreach (H3.DataModel.BizObject record in bizRecords)
            {
                if (record != null)
                {
                    //实际外径
                    record["F0000033"] = B["F0000105"] != null ? B["F0000105"].ToString() : "";
                    //实际内径
                    record["F0000034"] = B["F0000106"] != null ? B["F0000106"].ToString() : "";
                    //实际总高
                    record["F0000035"] = B["F0000107"] != null ? B["F0000107"].ToString() : "";
                    //实际片厚
                    record["F0000036"] = B["F0000108"] != null ? B["F0000108"].ToString() : "";
                    //实际单重
                    record["F0000037"] = B["F0000109"] != null ? B["F0000109"].ToString() : "";
                    //更新数据
                    record.Update();
                }
            }

            //多人加工检验结果逻辑
            switch (count)
            {
                case 1:   //一人加工
                    if (B["F0000085"] != null)
                    {                                                    //加工量1
                        double num = double.Parse(B["F0000085"].ToString());
                        if (num > 0) //加工量1大于0时
                        {
                            record1["F0000009"] = B["F0000018"].ToString();
                            record1.Update();
                        }
                    }

                    break;
                case 2: //两人加工
                    if (B["F0000089"] != null)
                    {                                                   //加工量2
                        double num = double.Parse(B["F0000089"].ToString());
                        if (num > 0) //加工量2大于0时
                        {
                            record1["F0000009"] = "合格";
                            record2["F0000009"] = B["F0000018"].ToString();
                        }
                        else
                        {
                            record1["F0000009"] = B["F0000018"].ToString();
                        }
                        record1.Update();
                        record2.Update();
                    }
                    break;
                case 3: //三人加工
                    if (B["F0000091"] != null)
                    {                                                   //加工量3
                        double num = double.Parse(B["F0000091"].ToString());
                        if (num > 0)  //加工量3大于0时
                        {
                            record1["F0000009"] = "合格";
                            record2["F0000009"] = "合格";
                            record3["F0000009"] = B["F0000018"].ToString();
                        }
                        else
                        {
                            record1["F0000009"] = "合格";
                            record2["F0000009"] = B["F0000018"].ToString();
                        }
                        record1.Update();
                        record2.Update();
                        record3.Update();
                    }
                    break;
            }
        }
    }

}

