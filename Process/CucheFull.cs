
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.Workflow;
using Chuanyun;
using H3.Organization;

public class D001419Szzswrfsp91x3heen4dykgwus0 : H3.SmartForm.SmartFormController
{
    //H3.SmartForm.LoadSmartFormResponse h3response;
    H3.IEngine Engine;
    Request Request;

    public D001419Szzswrfsp91x3heen4dykgwus0(H3.SmartForm.SmartFormRequest request) : base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        NormalDispatch();
        base.OnLoad(response);
        Init(response);
    }
    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        string dispatchtest = DispatchFlag;
        AutherCheck(response);
        //更新机加工记录表
        UpdateRecordForm(actionName);
        Schema me = new Schema(this.Engine, postValue, "粗车");
        //var cd=this.Engine.ViewManager.GetBehindCodesByAppCode("D001419SXSH",out t);
        //var FormSt=H3.SmartForm.FormSetting(this.Request.SchemaCode,"粗车"); 
        //this.Engine.ViewManager.UpdateForm() ;

        string code = this.Request.ActivityCode; //节点编码     
        if (actionName == "Submit" && code == "jianyan")//指检验过程 
        {
            UpdateSalaryByJGJL(me, response, "粗车");
        }
        var row = me.GetRow(this.Request.BizObjectId);
        var zl = me.Cell("质检结论");
        // //待检验
        // if(code == "jianyan") 
        // {
        //     //探伤结果
        //     var t = me.Cell("探伤结果");
        //     //检验结果
        //     var j = me.Cell("检验结果"); 
        //     zl = j;
        //     if(t == "不合格" && j == "合格")
        //     {               
        //         zl = "返修";
        //     }          
        // }        


        mustHaveAName(actionName, postValue, response);
        DispatchAnalys();
        currentlworker();
        base.OnSubmit(actionName, postValue, response);
        abnormalStep(me, postValue, response);
    }
    private void mustHaveAName(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (this.Request.BizObject["F0000075"] == "是")
        {
            this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
        }
    }
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

        if (Code == "Activity127")   //???
        {
            createLog(me, postValue);
        }
        if (Code == "Activity132")
        {
            //me.Cell("发起异常","是");
            me.Cell("发起异常", "否");
            me.Cell("转至工步", "待转运");
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
    protected void UpdateSalaryByJGJL(Schema me, H3.SmartForm.SubmitSmartFormResponse response, string 工序名称)
    {
        //JGJL=加工记录          
        //var 工序名称 = "粗车";
        var ID = me.PostValue("ID");
        var scmJGJL = new Schema(me.Engine, me.CurrentPostValue, "加工任务记录");
        var rows = scmJGJL.ClearFilter()
            .And("ID", "=", ID)
            .And("工序名称", "=", 工序名称)
            .And("检验结果", "=", "合格")
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

            var 总工时 = 单件拟定工时 * 加工数量;
            var 工时工资 = 总工时 * 工价 * 加工难度;

            var 分配比例 = 1.0;
            var smRow = scmJGJL.ClearFilter()
                .And("ID", "=", ID)
                .And("工序名称", "=", 工序名称)
                .And("任务名称", "=", "四面见光")
                .GetFirst();
            if (smRow != null) { 分配比例 = 0.8; }

            var 工艺下屑重量 = Convert.ToDouble(scmJGJL.CellAny("工艺下屑重量"));
            var 总下屑量 = 工艺下屑重量 * 加工数量 * 分配比例;
            var 外径 = Convert.ToDouble(scmJGJL.CellAny("外径"));
            var 补助标准 = 外径 >= 0 && 外径 < 4000 ? 18 : (外径 >= 4000 && 外径 < 5000 ? 23 : (外径 >= 5000 && 外径 < 6000 ? 30 : 30));
            var 补刀金额 = 补助标准 * 总下屑量 / 1000;

            var 总工作量 = 0;

            var scmGZ = new Schema(me.Engine, me.CurrentPostValue, "任务绩效表");
            scmGZ.GetNew();


            scmGZ.CellAny("工序名称", 工序名称);
            scmGZ.CellAny("任务名称", 任务名称);

            scmGZ.CellAny("ID", ID);
            scmGZ.CellAny("产品编号", scmJGJL.CellAny("产品编号"));
            scmGZ.CellAny("工件号", scmJGJL.CellAny("工件号"));
            scmGZ.CellAny("检验结果", 检验结果);
            scmGZ.CellAny("加工人员", 加工人员);
            scmGZ.CellAny("部门名称", 部门名称);
            scmGZ.CellAny("设备名称", 设备名称);
            scmGZ.CellAny("单件拟定工时", 单件拟定工时);
            scmGZ.CellAny("总工时", 总工时);
            scmGZ.CellAny("加工数量", 加工数量);
            scmGZ.CellAny("工价", 工价);
            scmGZ.CellAny("工时工资", 工时工资);
            scmGZ.CellAny("工艺下屑重量", 工艺下屑重量);
            scmGZ.CellAny("总下屑量", 总下屑量);
            scmGZ.CellAny("总工作量", 总工作量);
            scmGZ.CellAny("补刀金额", 补刀金额);
            scmGZ.Create(true);
        }
    }
    protected void Init(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {                         //本表单数据
            H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId,
                this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
            if (thisObj != null)
            {
                string bizid = thisObj.ObjectId;

                string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_D001419Szzswrfsp91x3heen4dykgwus0  a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'cuchexiaji' and b.BizObjectId = '{0}'  group by b.bizobjectid", bizid);
                DataTable data = this.Engine.Query.QueryTable(command, null);

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
                if (thisObj["F0000083"] == null)
                {           //当前工序
                    thisObj["F0000083"] = "粗车";
                }
                //产品类别更新
                if (thisObj["F0000121"] == null)
                {            //产品参数表ObjectId
                    if (thisObj["F0000116"] != null)
                    {                      //产品参数表
                        H3.DataModel.BizObject pfObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", this.Request.BizObject["F0000116"].ToString(), false);
                        if (pfObj != null)
                        {
                            if (pfObj["F0000004"] != null)
                            {           //产品类别-本表    //产品类别-产品参数表
                                thisObj["F0000121"] = pfObj["F0000004"].ToString();
                            }
                        }
                    }
                }

                if (thisObj["F0000133"] == null)
                {   //任务名称
                    thisObj["F0000133"] = "0";
                }
                //更新本表单
                thisObj.Update();
            }

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


    protected string RoleOfPosition()
    {

        string value = (string)this.Request.BizObject["F0000080"];
        string[] userIds = this.Request.Engine.Organization.GetChildren(this.Request.Engine.Organization.Company.CompanyId, UnitType.User, true, State.Active);
        string roleName = ""; //SQL.Select(this.Request.Engine, "F0000001", "i_D00141919b80510f0e24d8695f5e80f8c485fa8", "F0000004", value);
        string Result = null;
        if (userIds.Length > 0 && !string.IsNullOrEmpty(roleName))
        {
            foreach (var item in userIds)
            {
                H3.Organization.OrgRole[] roles = this.Request.Engine.Organization.GetUserRoles(item, true);
                foreach (var i in roles)
                {

                    if (roleName == i.Name)
                    {
                        Result = item;
                    }


                }
            }
        }


        return Result;

    }
    protected void AutherCheck(H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            bool flag = false;
            if (this.Request.BizObject["F0000075"] != "否") { return; }//是否发起异常
            string id = this.Request.BizObject["F0000067"].ToString();//id
            string sql = string.Format("select ObjectId  from i_D001419Szlywopbivyrv1d64301ta5xv4 where F0000007='{0}'", id);//ID
            DataTable dt = this.Engine.Query.QueryTable(sql, null);
            int Count = dt.Rows.Count;
            string ABCDObj = (string)dt.Rows[0][0];
            H3.DataModel.BizObject ABCD = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Szlywopbivyrv1d64301ta5xv4", ABCDObj, true);
            string[] ABCDWorkers = ((string[])ABCD["F0000129"]);//粗车人员
            string thePat = this.Request.WorkItem.Participant;
            foreach (string item in ABCDWorkers)
            {
                if (item == thePat)
                {
                    flag = true;
                }
            }

            if (!flag)
            {
                response.Errors.Add("派工信息已更新，您无权上机");
            }


        }
        catch (Exception ex)
        {
            response.Errors.Add("ABCD工序计划内无此对象");
        }
    }
    protected void currentlworker()
    {
        string activitycode = this.Request.ActivityCode;
        H3.DataModel.BizObject thiObj = this.Request.BizObject;

        if (DispatchFlag == "Normal")
        {
            H3.DataModel.BizObject ABCD = GetABCDWorker();
            string[] workers = (string[])ABCD["F0000129"]; //粗车人员
            if (activitycode == "Activity14")
            {

                string particant = this.Request.WorkItem.Participant;
                foreach (string i in workers)
                {

                    if (particant == i)
                    {
                        this.Request.BizObject["F0000084"] = i;//工人

                    }
                }
            }
            if (activitycode == "cuchexiaji")
            {
                this.Request.BizObject["F0000084"] = workers;//工人
            }


        }

        if (DispatchFlag == "Order")
        {

            OrderlyDispatch();

        }
        if (DispatchFlag == "Undo")
        {
            UnDispath();

        }


        if (activitycode == "Activity14")
        {
            string participantid = this.Request.ParticipantId;   //工人                   
            thiObj["F0000084"] = new string[] { participantid }; ;//工人

        }


    }

    public H3.DataModel.BizObject GetABCDWorker()
    {
        string id = this.Request.BizObject["F0000067"].ToString();//ID
        string sql = string.Format("select ObjectId  from i_D001419Szlywopbivyrv1d64301ta5xv4 where F0000007='{0}'", id);
        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        if (dt.Rows.Count == 0) { throw new Exception("ABCD工序计划表无此对象"); }
        string ABCDObj = (string)dt.Rows[0][0];
        H3.DataModel.BizObject ABCD = H3.DataModel.BizObject.Load(this.Request.UserContext.UserId, this.Engine, "D001419Szlywopbivyrv1d64301ta5xv4", ABCDObj, true);
        Workers = (string[])ABCD["F0000129"];
        return ABCD;
    }
    protected string[] ProActiveworker()
    {
        var t = Request.WorkflowInstance.GetRunningToken(this.Request.ActivityCode);      
        Token preTokens = this.Request.WorkflowInstance.Tokens[t.PreTokens[0] - 1];
        return preTokens.Participants;

    }

    public void NormalDispatch()
    {

        try
        {
            H3.DataModel.BizObject ABCD = GetABCDWorker();
            string[] workers = (string[])ABCD["F0000129"];//工人=粗车人员      
            if (workers.Length > 0)
            {
                this.Request.BizObject["F0000084"] = ABCD["F0000129"];//工人=粗车人员                                       
                this.Request.BizObject["F0000085"] = ABCD["F0000055"];//工时=粗车所需工时
                this.Request.BizObject["F0000107"] = ABCD["F0000097"];//设备类型=粗车设备类型
                this.Request.BizObject["F0000086"] = ABCD["F0000094"];//设备编号=粗车设备编号
                this.Request.BizObject["F0000009"] = ABCD["F0000096"];//粗车使用设备=粗车设备名称  
            }

        }
        catch (Exception e)
        {
            if (e.GetType().ToString() != "System.NullReferenceException")
            {
                throw e;
            }
        }
    }


    public void OrderlyDispatch()
    {

        string Firstuser = (string)this.Request.BizObject["F0000091"];//第一加工者
        string Seconduser = (string)this.Request.BizObject["F0000092"];
        string Threeuser = (string)this.Request.BizObject["F0000094"];
        var First = this.Request.BizObject["F0000089"];//第一加工量
        var Second = this.Request.BizObject["F0000093"];
        var Three = this.Request.BizObject["F0000095"];
        bool state = true;
        if (!string.IsNullOrEmpty(Firstuser) && First == null && state)
        {
            this.Request.BizObject["F0000084"] = Firstuser;
            state = false;
        }
        if (!string.IsNullOrEmpty(Seconduser) && Second == null && state)
        {
            this.Request.BizObject["F0000084"] = Seconduser;

        }
        if (!string.IsNullOrEmpty(Threeuser) && Three == null && state)
        {
            this.Request.BizObject["F0000084"] = Threeuser;

        }


    }
    string[] Workers = null;
    string DispatchFlag = "Normal";

    public void UnDispath()
    {
        string activitycode = this.Request.ActivityCode;

        if (DispatchFlag == "Undo")
        {
            var ABCD = GetABCDWorker();
            string[] Workers = (string[])ABCD["F0000129"];
            if (Workers.Length > 0) { return; }
            string roleUserid = RoleOfPosition();


            if (!string.IsNullOrEmpty(roleUserid))
            {
                this.Request.BizObject["F0000084"] = roleUserid;//工人

            }
            else
            {
                if (activitycode != "Activity75")//待上机
                {
                    throw new Exception("无派工时请配置责任区域并给区域指定管理角色和责任人");
                }

            }
            if (activitycode == "cuchexiaji")
            {
                this.Request.BizObject["F0000084"] = ProActiveworker();

            }
        }


    }



    public void DispatchAnalys()
    {
        H3.DataModel.BizObject ABCD = GetABCDWorker();
        bool OrderFlag = (bool)ABCD["F0000154"];
        string[] Workers = (string[])ABCD["F0000129"];

        if (Workers.Length == 0)
        {
            DispatchFlag = "Undo";

        }
        if (Workers.Length > 0 && !OrderFlag)
        {
            DispatchFlag = "Normal";
        }


        if (Workers.Length > 1 && Workers.Length <= 3 && OrderFlag)
        {
            DispatchFlag = "Order";
            this.Request.BizObject["F0000084"] = ABCD["F0000129"];//工人=粗车人员   
            int count = 1;

            foreach (string i in Workers)
            {

                switch (count)
                {
                    case 1:
                        this.Request.BizObject["F0000091"] = i;
                        count++;
                        break;
                    case 2:
                        this.Request.BizObject["F0000092"] = i;
                        count++;
                        break;
                    case 3:
                        this.Request.BizObject["F0000094"] = i;
                        break;
                }

            }

        }


    }



    public H3.DataModel.BizObject AddRecord(string employeeId, string machine)
    {       //向产品参数表新增机加工记录
        H3.DataModel.BizObjectSchema rSchema = this.Engine.BizObjectManager.GetPublishedSchema("D0014194963919529e44d60be759656d4a16b63");

        H3.DataModel.BizObject rBo = new H3.DataModel.BizObject(this.Engine, rSchema, H3.Organization.User.SystemUserId);

        rBo.Status = H3.DataModel.BizObjectStatus.Effective;

        H3.DataModel.BizObject pfObj = null;  //产品参数表

        if (this.Request.BizObject["F0000116"] != null)
        {   //加载产品参数表
            pfObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", this.Request.BizObject["F0000116"].ToString(), false);
        }

        if (pfObj != null)
        {
            if (pfObj["F0000014"] != null)
            {                   //产品参数表-成品单重
                rBo["F0000017"] = pfObj["F0000014"].ToString();
            }

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

        if (this.Request.BizObject["F0000076"] != null)
        {         //数据代码                         //数据代码-本表
            rBo["F0000015"] = this.Request.BizObject["F0000076"].ToString();
        }

        if (this.Request.BizObject["F0000003"] != null)
        {
            //产品规格
            rBo["F0000003"] = this.Request.BizObject["F0000003"].ToString();
        }

        if (this.Request.BizObject["F0000122"] != null)
        {                                         //轧制方式
            rBo["F0000024"] = this.Request.BizObject["F0000122"].ToString();
        }

        rBo["ID"] = this.Request.BizObject["F0000067"].ToString();
        //工序
        rBo["F0000001"] = "粗车";
        //加工者
        rBo["F0000011"] = employeeId;
        //任务类型
        rBo["F0000031"] = "正常粗车";

        if (this.Request.BizObject["F0000134"] != null)
        {                           //完成本取
            if (this.Request.BizObject["F0000134"].ToString() == "已本取")
            {   //任务类型
                rBo["F0000031"] = "本取后粗车";
            }
        }

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
        if (actionName == "Submit" && code == "Activity14")
        {        //加工设备
            string machine = "";
            //第一加工者/设备名称
            if (this.Request.BizObject["F0000091"] + string.Empty == "" && this.Request.BizObject["F0000099"] + string.Empty != "")
            {
                this.Request.BizObject["F0000091"] = this.Request.UserContext.UserId;
            }                            //第二加工者/设备名称
            else if (this.Request.BizObject["F0000092"] + string.Empty == "" && this.Request.BizObject["F0000101"] + string.Empty != "")
            {
                this.Request.BizObject["F0000092"] = this.Request.UserContext.UserId;
            }                            //第三加工者/设备名称
            else if (this.Request.BizObject["F0000094"] + string.Empty == "" && this.Request.BizObject["F0000103"] + string.Empty != "")
            {
                this.Request.BizObject["F0000094"] = this.Request.UserContext.UserId;
            }


            //第三加工者
            if (this.Request.BizObject["F0000094"] != null && this.Request.BizObject["F0000094"].ToString() != "")
            {
                if (this.Request.BizObject["F0000103"] != null)
                {                                   //第三设备
                    machine = this.Request.BizObject["F0000103"] + string.Empty;
                }
                //向产品参数表新增机加工记录
                H3.DataModel.BizObject item = AddRecord(this.Request.BizObject["F0000094"].ToString(), machine);
                //在本表单记录加工者的机加工记录
                this.Request.BizObject["F0000120"] = item.ObjectId;

            }                           //第二加工者
            else if (this.Request.BizObject["F0000092"] != null && this.Request.BizObject["F0000092"].ToString() != "")
            {
                if (this.Request.BizObject["F0000101"] != null)
                {                                   //第二设备
                    machine = this.Request.BizObject["F0000101"] + string.Empty;
                }
                //向产品参数表新增机加工记录
                H3.DataModel.BizObject item = AddRecord(this.Request.BizObject["F0000092"].ToString(), machine);
                //在本表单记录加工者的机加工记录
                this.Request.BizObject["F0000119"] = item.ObjectId;

            }                           //第一加工者
            else if (this.Request.BizObject["F0000091"] != null && this.Request.BizObject["F0000091"].ToString() != "")
            {
                if (this.Request.BizObject["F0000099"] != null)
                {                               //第一设备                     
                    machine = this.Request.BizObject["F0000099"] + string.Empty;
                }
                //向产品参数表新增机加工记录
                H3.DataModel.BizObject item = AddRecord(this.Request.BizObject["F0000091"].ToString(), machine);
                //在本表单记录加工者的机加工记录
                this.Request.BizObject["F0000118"] = item.ObjectId;

            }
        }
        //下机提交
        if (actionName == "Submit" && code == "cuchexiaji")
        {
            H3.DataModel.BizObject record = null;  //机加工记录表
            H3.DataModel.BizObject[] mtObj = null;   //设备工时系数表
            H3.DataModel.BizObject[] sObj = null;   //设备工时系数表-子表
            H3.DataModel.BizObject pfObj = null;  //产品参数表

            string mtNum = ""; //设备工时系数
            string zzMode = ""; //轧制方式
            string pTime = ""; //本工序产品工时
            string totalxx = "";//总下屑量

            if (this.Request.BizObject["F0000122"] != null)
            {   //获取本工序产品轧制方式
                zzMode = this.Request.BizObject["F0000122"].ToString();
            }

            if (this.Request.BizObject["F0000116"] != null)
            {   //加载产品参数表
                pfObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", this.Request.BizObject["F0000116"].ToString(), false);
            }

            if (pfObj != null)
            {                             //产品参数表-单轧工时
                if (zzMode == "单轧" && pfObj["F0000048"] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取设备工时系数
                    pTime = pfObj["F0000048"].ToString();
                    //单轧粗车下屑
                    totalxx = pfObj["F0000045"].ToString();
                }
                //产品参数表-双轧工时
                if (zzMode == "双轧" && pfObj["F0000049"] != null)
                {//根据本表单产品轧制方式从产品参数表中获取设备工时系数
                    pTime = pfObj["F0000049"].ToString();
                    //双轧粗车下屑
                    totalxx = pfObj["F0000046"].ToString();
                }
            }
            //产品类别
            if (this.Request.BizObject["F0000121"] != null)
            {
                H3.DataModel.BizObjectSchema schema = this.Engine.BizObjectManager.GetPublishedSchema("D0014195ed7e837ecee4f97800877820d9a2f05");   //获取设备工时系数模块

                H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();  //构建过滤器

                H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();    //构造And匹配器
                //当前工序
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("F0000001", H3.Data.ComparisonOperatorType.Equal, "粗车")); //添加查询条件
                //产品类型                                                               //产品类型-本表
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("F0000002", H3.Data.ComparisonOperatorType.Equal, this.Request.BizObject["F0000121"].ToString())); //添加查询条件            

                filter.Matcher = andMatcher;
                //设备工时系数表
                mtObj = H3.DataModel.BizObject.GetList(this.Engine, H3.Organization.User.SystemUserId, schema, H3.DataModel.GetListScopeType.GlobalAll, filter);
            }

            if (mtObj != null)
            {           //设备工时系数表-子表
                sObj = mtObj[0]["D001419Fbb7854d117af4bba8eff4de46d128f63"] as H3.DataModel.BizObject[];
            }

            //加工记录3 ObjectId　　　　　　　　　　　　　　　　　//加工量3　　
            if (this.Request.BizObject["F0000120"] != null && this.Request.BizObject["F0000095"] != null)
            {                                           //加工量3
                string quantity = this.Request.BizObject["F0000095"].ToString();
                //加载机加工记录表
                record = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014194963919529e44d60be759656d4a16b63", this.Request.BizObject["F0000120"].ToString(), false);
                if (record != null)
                {
                    //加工量3
                    record["F0000010"] = quantity;
                    //下屑量
                    record["F0000023"] = totalxx;
                    //任务名称
                    record["F0000002"] = this.Request.BizObject["F0000133"].ToString();
                    //设备编号3
                    record["F0000014"] = this.Request.BizObject["F0000104"].ToString();
                    //遍列设备工时系数-子表
                    foreach (H3.DataModel.BizObject item in sObj)
                    {
                        if (pfObj != null && this.Request.BizObject["F0000110"] != null)
                        {        //设备类型- 设备工时系数表                           //设备类型-本表
                            if (item["F0000004"].ToString() == this.Request.BizObject["F0000110"].ToString())
                            {
                                if (zzMode == "单轧" && item["F0000007"] != null)
                                {               //设备工时系数表-单轧工时
                                    mtNum = item["F0000007"].ToString();
                                }
                                else if (zzMode == "双轧" && item["F0000008"] != null)
                                {               //设备工时系数表-双轧工时
                                    mtNum = item["F0000008"].ToString();
                                }
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
            }                            //加工记录2 ObjectId                            //加工量2
            else if (this.Request.BizObject["F0000119"] != null && this.Request.BizObject["F0000093"] != null)
            {

                //加工量2
                string quantity = this.Request.BizObject["F0000093"].ToString();
                //加载机加工记录表
                record = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014194963919529e44d60be759656d4a16b63", this.Request.BizObject["F0000119"].ToString(), false);
                if (record != null)
                {
                    //加工量2
                    record["F0000010"] = quantity;
                    //下屑量
                    record["F0000023"] = totalxx;
                    //任务名称
                    record["F0000002"] = this.Request.BizObject["F0000133"].ToString();
                    //设备编号2
                    record["F0000014"] = this.Request.BizObject["F0000102"].ToString();
                    //遍列设备工时系数-子表
                    foreach (H3.DataModel.BizObject item in sObj)
                    {
                        if (pfObj != null && this.Request.BizObject["F0000109"] != null)
                        {        //设备类型- 设备工时系数表                           //设备类型-本表
                            if (item["F0000004"].ToString() == this.Request.BizObject["F0000109"].ToString())
                            {
                                if (zzMode == "单轧" && item["F0000007"] != null)
                                {               //设备工时系数表-单轧工时
                                    mtNum = item["F0000007"].ToString();
                                }
                                else if (zzMode == "双轧" && item["F0000008"] != null)
                                {               //设备工时系数表-双轧工时
                                    mtNum = item["F0000008"].ToString();
                                }
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
                        {         //预计工时               本工序产品工时 * 设备工时系数
                            record["F0000005"] = double.Parse(pTime) * double.Parse(mtNum);
                            //任务工时               本工序产品工时 * 设备工时系数 * 加式量
                            record["F0000006"] = double.Parse(pTime) * double.Parse(mtNum) * double.Parse(quantity);
                        }
                    }

                    record.Update();
                }
            }                             //加工记录1 ObjectId                            //加工量1
            else if (this.Request.BizObject["F0000118"] != null && this.Request.BizObject["F0000089"] != null)
            {
                //加工量1
                string quantity = this.Request.BizObject["F0000089"].ToString();
                //加载机加工记录表
                record = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    this.Engine, "D0014194963919529e44d60be759656d4a16b63", this.Request.BizObject["F0000118"].ToString(), false);
                if (record != null)
                {
                    //加工量1
                    record["F0000010"] = quantity;
                    //下屑量
                    record["F0000023"] = totalxx;
                    //任务名称
                    record["F0000002"] = this.Request.BizObject["F0000133"].ToString();
                    //设备编号1
                    record["F0000014"] = this.Request.BizObject["F0000100"].ToString();
                    //遍列设备工时系数-子表
                    foreach (H3.DataModel.BizObject item in sObj)
                    {
                        if (pfObj != null && this.Request.BizObject["F0000108"] != null)
                        {        //设备类型- 设备工时系数表                           //设备类型-本表
                            if (item["F0000004"].ToString() == this.Request.BizObject["F0000108"].ToString())
                            {
                                if (zzMode == "单轧" && item["F0000007"] != null)
                                {               //设备工时系数表-单轧工时
                                    mtNum = item["F0000007"].ToString();
                                }
                                else if (zzMode == "双轧" && item["F0000008"] != null)
                                {               //设备工时系数表-双轧工时
                                    mtNum = item["F0000008"].ToString();
                                }
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
        if (actionName == "Submit" && code == "jianyan")
        {
            //加工记录ObjectIds
            string[] records = { "F0000118", "F0000119", "F0000120" };

            H3.DataModel.BizObject record1 = null; //加工记录1
            H3.DataModel.BizObject record2 = null; //加工记录2
            H3.DataModel.BizObject record3 = null; //加工记录3



            int count = 0; //加工者数量

            foreach (string item in records)
            {
                if (this.Request.BizObject[item] != null && this.Request.BizObject[item].ToString().Length > 6)
                {
                    ++count; //统计加工者数量

                    if (count == 1)
                    {  //加载加工记录1
                        record1 = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014194963919529e44d60be759656d4a16b63", this.Request.BizObject[item].ToString(), false);
                    }

                    if (count == 2)
                    {   //加载加工记录2
                        record2 = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014194963919529e44d60be759656d4a16b63", this.Request.BizObject[item].ToString(), false);
                    }

                    if (count == 3)
                    {   //加载加工记录3
                        record3 = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                            this.Engine, "D0014194963919529e44d60be759656d4a16b63", this.Request.BizObject[item].ToString(), false);
                    }
                }
            }

            H3.DataModel.BizObject[] bizRecords = { record1, record2, record3 };

            foreach (H3.DataModel.BizObject record in bizRecords)
            {
                if (record != null)
                {
                    //实际外径
                    record["F0000033"] = this.Request.BizObject["F0000111"] != null ? this.Request.BizObject["F0000111"].ToString() : "";
                    //实际内径
                    record["F0000034"] = this.Request.BizObject["F0000112"] != null ? this.Request.BizObject["F0000112"].ToString() : "";
                    //实际总高
                    record["F0000035"] = this.Request.BizObject["F0000113"] != null ? this.Request.BizObject["F0000113"].ToString() : "";
                    //实际片厚
                    record["F0000036"] = this.Request.BizObject["F0000114"] != null ? this.Request.BizObject["F0000114"].ToString() : "";
                    //实际单重
                    record["F0000037"] = this.Request.BizObject["F0000115"] != null ? this.Request.BizObject["F0000115"].ToString() : "";
                    //探伤结果
                    record["F0000029"] = this.Request.BizObject["F0000138"] != null ? this.Request.BizObject["F0000138"].ToString() : "";
                    //更新数据
                    record.Update();
                }
            }

            //多人加工检验结果逻辑
            switch (count)
            {
                case 1:   //一人加工
                    if (this.Request.BizObject["F0000089"] != null)
                    {                                                    //加工量1
                        double num = double.Parse(this.Request.BizObject["F0000089"].ToString());
                        if (num > 0) //加工量1大于0时
                        {                                               //检验结果
                            record1["F0000009"] = this.Request.BizObject["F0000023"] != null ? this.Request.BizObject["F0000023"].ToString() : "";
                            record1.Update();
                        }
                    }

                    break;
                case 2: //两人加工
                    if (this.Request.BizObject["F0000093"] != null)
                    {                                                   //加工量2
                        double num = double.Parse(this.Request.BizObject["F0000093"].ToString());
                        if (num > 0) //加工量2大于0时
                        {
                            record1["F0000009"] = "合格";               //检验结果
                            record2["F0000009"] = this.Request.BizObject["F0000023"] != null ? this.Request.BizObject["F0000023"].ToString() : "";
                        }
                        else
                        {                                               //检验结果
                            record1["F0000009"] = this.Request.BizObject["F0000023"] != null ? this.Request.BizObject["F0000023"].ToString() : "";
                        }
                        record1.Update();
                        record2.Update();
                    }
                    break;
                case 3: //三人加工
                    if (this.Request.BizObject["F0000095"] != null)
                    {                                                   //加工量3
                        double num = double.Parse(this.Request.BizObject["F0000095"].ToString());
                        if (num > 0)  //加工量3大于0时
                        {
                            record1["F0000009"] = "合格";
                            record2["F0000009"] = "合格";                //检验结果
                            record3["F0000009"] = this.Request.BizObject["F0000023"] != null ? this.Request.BizObject["F0000023"].ToString() : "";
                        }
                        else
                        {
                            record1["F0000009"] = "合格";                //检验结果
                            record2["F0000009"] = this.Request.BizObject["F0000023"] != null ? this.Request.BizObject["F0000023"].ToString() : "";
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