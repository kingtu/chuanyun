using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public class D001419Sugyf7m5q744eyhe45o26haop4 : H3.SmartForm.SmartFormController
{
    const string ActivityHJ = "Activity95";//互检活动。
    const string ActivityRK = "Activity72";//入库。
    const string ActivitySJ = "Activity3";//上机活动。
    const string ActivityXJ = "Activity35";//下机活动。
    const string ActivityJY = "Activity68";//检验活动。    

    //private H3.DataModel.BizObject B;
    private String RID = null;
    Schema me = null;
    string ProcessName = "钻孔";
    Dispatch dp = null;//派工信息

    public D001419Sugyf7m5q744eyhe45o26haop4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        //B = this.Request.BizObject;
        RID = this.Request.BizObjectId;
        me = new Schema(this.Request.Engine, this.Request.SchemaCode, true);
        me.CurrentRow = this.Request.BizObject;
        dp = new Dispatch(this.Request.Engine, me.Cell("ID"));//派工信息 
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        var code = this.Request.ActivityCode;
        if (code == ActivityRK) { dp.FillWorkShop(me, ProcessName); }
        Init(response);//初始化
        base.OnLoad(response);

    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        me.CurrentPostValue = postValue;

        string activityCode = this.Request.ActivityCode;  //活动节点编码    
        if (activityCode == ActivityRK && actionName == "Submit")
        {
            dp.FillPerson(me, ProcessName);
        }
        if (activityCode == ActivityXJ && actionName == "Submit")
        {
            dp.FillPerson(me, ProcessName);
        }
        if (activityCode == ActivitySJ && actionName == "Submit")
        {
            me["工人"] = this.Request.ParticipantId;
        }

        updatePR(activityCode, actionName);
        //发起异常 是
        if (me["发起异常"] == "是")
        {
            me["OwnerId"] = this.Request.UserContext.UserId;
        }

        base.OnSubmit(actionName, postValue, response);
        //多阶段加工新方案升级机加工任务记录
        UpdateRecordForm(actionName, activityCode);

        if (activityCode == ActivityJY && actionName == "Submit")
        {
            Salary slr = new Salary(me.Engine, me.PostValue("ID"));
            slr.Save(ProcessName, false);
        }
        abnormalStep(me, postValue, response);
    }
    private void updatePR(string activityCode, string actionName)
    {
        H3.DataModel.BizObject[] rows = me["机加工信息"] as H3.DataModel.BizObject[];
        int index = rows != null ? rows.Length - 1 : -1;
        me["任务名称"] = index + 1;
        Schema s = me.GetSubSchema("机加工信息");
        s.CurrentRow = rows[index];

        if (actionName == "Submit" && activityCode == "Activity3") //钻孔上机
        {
            s["加工者"] = this.Request.UserContext.UserId;
            s["开始时间"] = System.DateTime.Now;
        }

        if (actionName == "Submit" && activityCode == "Activity35") //钻孔下机
        {
            double t = Convert.ToDouble(me["完成总量"]);
            if (t < 1)
            {
                var schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
                CreatSublist(me.CurrentRow, schema, rows);
                // if((int) me["任务名称"] >= rows.Length)
                // {   //根据计数器创建添加新的子表行数据
                //     var schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
                //     CreatSublist(me.CurrentRow, schema, rows);
                // }
            }
            if (s["加工者"] + string.Empty == string.Empty)
            {
                s["加工者"] = this.Request.UserContext.UserId; //当前加工者
            }
        }
    }
    private void updatePR2(string activityCode, string actionName)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = me["机加工信息"] as H3.DataModel.BizObject[];  //获取子表
        //多阶段加工新方案
        thisObj["F0000166"] = lstArray != null ? lstArray.Length + string.Empty : "1";  //修正任务数-----任务名称
        int taskNum = thisObj["F0000166"] + string.Empty != string.Empty ? int.Parse(thisObj["F0000166"] + string.Empty) - 1 : 0; //获取任务数

        if (actionName == "Submit" && activityCode == "Activity3") //钻孔上机
        {
            lstArray[taskNum]["F0000143"] = this.Request.UserContext.UserId; //当前加工者
            lstArray[taskNum]["F0000142"] = System.DateTime.Now; //加工开始时间
        }

        if (actionName == "Submit" && activityCode == "Activity35") //钻孔下机
        {
            //完成总量小于1时
            if ((thisObj["F0000073"] + string.Empty) != string.Empty && decimal.Parse(thisObj["F0000073"] + string.Empty) < 1)
            {
                //递增计数器，并更新
                thisObj["F0000166"] = taskNum + 2;

                if (int.Parse(thisObj["F0000166"] + string.Empty) >= lstArray.Length)
                {   //根据计数器创建添加新的子表行数据
                    CreatSublist(thisObj, schema, lstArray);
                }
            }

            if (lstArray[taskNum]["F0000143"] + string.Empty == string.Empty)
            {
                lstArray[taskNum]["F0000143"] = this.Request.UserContext.UserId; //当前加工者
            }
        }
    }


    protected void Init(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {
            H3.DataModel.BizObject thisObj = this.Request.BizObject;
            string tsFormId = thisObj["F0000167"] + string.Empty;  //探伤表
            H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
            H3.DataModel.BizObject[] lstArray = thisObj["D001419F790f3a6b004e4988abe9511380792293"] as H3.DataModel.BizObject[];  //获取子表数据

            thisObj["F0000166"] = thisObj["F0000166"] + string.Empty != string.Empty ? thisObj["F0000166"] + string.Empty : "1"; //初始化计数器
            if (lstArray == null)
            {   //初始化子表
                CreatSublist(thisObj, schema, lstArray);
            }

            string bizid = thisObj.ObjectId;
            //查询钻孔加工中所有耗时
            string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime From i_D001419Sugyf7m5q744eyhe45o26haop4 " +
                "a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity35' and b.BizObjectId = '{0}'  group by b.bizobjectid", bizid);
            DataTable data = this.Engine.Query.QueryTable(command, null);

            if (data != null && data.Rows != null && data.Rows.Count > 0)
            {
                if (data.Rows[0]["utime"] != null)
                {
                    string utimestr = data.Rows[0]["utime"] + string.Empty;
                    //转换时间单位为秒
                    double utime = double.Parse(utimestr) / 10000000 / 60;
                    //实际加工耗时
                    thisObj["CountTime"] = utime;
                }
            }

            //当前工序
            if (thisObj["F0000056"] + string.Empty == string.Empty)
            {           //当前工序
                thisObj["F0000056"] = "钻孔";
            }
            //产品类别更新
            if (thisObj["F0000090"] + string.Empty == string.Empty)
            {
                string orderSpec = thisObj["F0000016"] + string.Empty; //订单规格号
                string mysql = string.Format("Select ObjectId,F0000004,F0000006 From i_D0014196b62f7decd924e1e8713025dc6a39aa5 Where F0000073 = '{0}'", orderSpec);
                DataTable typeData = this.Engine.Query.QueryTable(mysql, null);
                if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
                {
                    thisObj["F0000090"] = typeData.Rows[0]["ObjectId"] + string.Empty; //产品参数表
                    thisObj["F0000107"] = typeData.Rows[0]["F0000004"] + string.Empty; //产品类型
                    thisObj["F0000103"] = typeData.Rows[0]["F0000006"] + string.Empty; //产品小型
                }
            }

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
                //转换工艺配置为布尔值
                Dictionary<string, bool> qcConfig = new Dictionary<string, bool>();
                qcConfig.Add("是", true);
                qcConfig.Add("否", false);

                //获取质量配置上机前互检优先级顺序
                string hjOrder = qcForm.Rows[0]["F0000007"] + string.Empty;

                if (hjOrder == "配置表")
                {
                    //获取《质量配置表》中“全局上机前互检”并赋值
                    string hjConfig = qcForm.Rows[0]["F0000002"] + string.Empty;
                    thisObj["F0000108"] = qcConfig[hjConfig] + string.Empty;
                }

                if (planObj != null)
                {
                    if (hjOrder == "计划表")
                    {
                        //获取《工序计划表》中“单件上机前互检”并赋值
                        string hjConfig = planObj != null ? planObj["F0000175"] + string.Empty : string.Empty;
                        if (hjConfig != "")
                        {
                            thisObj["F0000108"] = qcConfig[hjConfig] + string.Empty;
                        }
                        else
                        {
                            //获取《订单规格表》中“产品上机前互检”并赋值
                            hjConfig = acObj != null ? acObj["F0000135"] + string.Empty : string.Empty;
                            if (hjConfig != "")
                            {
                                thisObj["F0000108"] = qcConfig[hjConfig] + string.Empty;
                            }
                            else
                            {
                                //获取《质量配置表》中“全局上机前互检”并赋值
                                hjConfig = qcForm.Rows[0]["F0000002"] + string.Empty;
                                thisObj["F0000108"] = qcConfig[hjConfig] + string.Empty;
                            }
                        }
                    }

                    //加载划线绞扣配置
                    string jqConfig = acObj != null ? acObj["F0000133"] + string.Empty : string.Empty;

                    if (jqConfig.Length > 0)
                    {   //赋值划线绞扣
                        thisObj["F0000022"] = jqConfig;
                    }
                }
            }

            if (tsFormId == string.Empty)  //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
            {
                string thisId = thisObj["F0000029"] + string.Empty; //ID
                string mySql = string.Format("Select ObjectId From i_D001419fdcaecf556264750ae2d5684b2a3706e Where F0000001 = '{0}'", thisId);
                DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
                if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
                {
                    thisObj["F0000167"] = thisId = tsData.Rows[0]["ObjectId"] + string.Empty;
                }
            }

            //更新本表单
            thisObj.Update();

            try
            {
                //DataSync.instance.ZKSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }
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
        H3.DataModel.BizObject zibiao = new H3.DataModel.BizObject(this.Request.Engine, schema.GetChildSchema("D001419F790f3a6b004e4988abe9511380792293"), H3.Organization.User.SystemUserId);//子表对象
        zibiao["F0000141"] = thisObj["F0000166"] + string.Empty == string.Empty ? "1" : thisObj["F0000166"] + string.Empty; //任务名称
        lstObject.Add(zibiao);//将这个子表业务对象添加至子表数据集合中
        thisObj["D001419F790f3a6b004e4988abe9511380792293"] = lstObject.ToArray(); //子表数据赋值

        thisObj.Update();   //更新对象
    }

    public void UpdateRecordForm(string actionName, string activityCode)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = thisObj["D001419F790f3a6b004e4988abe9511380792293"] as H3.DataModel.BizObject[];  //获取子表
        //完成总量
        decimal count = thisObj["F0000073"] + string.Empty != string.Empty ? decimal.Parse(thisObj["F0000073"] + string.Empty) : 0;
        //任务计数器
        int taskNum = count < 1 ? int.Parse(thisObj["F0000166"] + string.Empty) - 2 : int.Parse(thisObj["F0000166"] + string.Empty) - 1;

        if (actionName == "Submit" && activityCode == "Activity35" && lstArray != null) //钻孔下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask["F0000143"] + string.Empty) as H3.Organization.User;

            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //轧制方式
            //string zzMode = thisObj["F0000122"] + string.Empty;
            //产品小类
            string productType = thisObj["F0000103"] + string.Empty;
            //设备类型
            string deviceType = currentTask["F0000139"] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;

            //产品小类
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From i_D0014195ed7e837ecee4f97800877820d9a2f05 Where F0000001 = '钻孔' and F0000003 = '{0}'", productType);//产品小类
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
                            if (item["F0000007"] != null)
                            {               //设备工时系数
                                deviceParam = item["F0000007"] + string.Empty;
                            }
                        }
                    }
                }
            }

            //产品参数表
            H3.DataModel.BizObject productObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, "D0014196b62f7decd924e1e8713025dc6a39aa5", thisObj["F0000090"] + string.Empty, false);

            if (productObj != null)
            {
                //根据本表单产品轧制方式从产品参数表中获取"钻孔工时"
                productTime = productObj["F0000052"] + string.Empty;
                //钻孔下屑
                totalxx = productObj["F0000074"] + string.Empty;

            }

            //机加工记录模块
            H3.DataModel.BizObjectSchema recordSchema = this.Engine.BizObjectManager.GetPublishedSchema("D0014194963919529e44d60be759656d4a16b63");
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = new H3.DataModel.BizObject(this.Engine, recordSchema, H3.Organization.User.SystemUserId);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态

            recordObj["F0000001"] = "钻孔"; //工序
            recordObj["F0000031"] = "正常钻孔"; //任务类型
            recordObj["F0000003"] = thisObj["F0000003"] + string.Empty; //产品规格
            recordObj["ID"] = thisObj["F0000029"] + string.Empty; //工件ID
            recordObj["F0000040"] = thisObj["F0000001"] + string.Empty; //工件号
            recordObj["F0000002"] = taskNum; //任务计数器
            recordObj["F0000011"] = currentTask["F0000143"] + string.Empty; //加工者
            recordObj["F0000030"] = employee != null ? employee.DepartmentName : ""; //部门名称
            recordObj["startTime"] = currentTask["F0000142"] + string.Empty; //加工开始时间
            recordObj["F0000007"] = currentTask["F0000137"] + string.Empty; //设备名称
            recordObj["F0000014"] = currentTask["F0000138"] + string.Empty; //设备编号
            recordObj["F0000041"] = currentTask["F0000139"] + string.Empty; //设备类型
            recordObj["F0000008"] = deviceParam; //设备工时系数
            recordObj["F0000023"] = totalxx; //工艺下屑量
            recordObj["F0000010"] = currentTask["F0000140"] + string.Empty; //任务加工量
            recordObj["EndTime"] = DateTime.Now; //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
            double mScale = currentTask["F0000140"] + string.Empty != string.Empty ? double.Parse(currentTask["F0000140"] + string.Empty) : 0; //加工量转换
            recordObj["F0000004"] = pTime; //本工序产品工时
            recordObj["F0000005"] = pTime * dParam; //单件拟定工时
            recordObj["F0000006"] = pTime * dParam * mScale; //任务工时

            if (productObj != null)
            {
                recordObj["F0000026"] = productObj["F0000067"] + string.Empty; //产品名称
                recordObj["F0000027"] = productObj["F0000004"] + string.Empty; //产品类别
                recordObj["F0000025"] = productObj["F0000006"] + string.Empty; //产品小类
                recordObj["ProductNum"] = productObj["F0000073"] + string.Empty; //产品编号
                recordObj["F0000017"] = productObj["F0000014"] + string.Empty; //成品单重
                recordObj["F0000016"] = productObj["F0000008"] + string.Empty; //工件外径
                recordObj["F0000018"] = productObj["F0000009"] + string.Empty; //工件内径
                recordObj["F0000020"] = productObj["F0000010"] + string.Empty; //工件总高
                recordObj["F0000019"] = productObj["F0000011"] + string.Empty; //工件片厚
                recordObj["F0000021"] = productObj["F0000012"] + string.Empty; //工件孔数
                recordObj["F0000022"] = productObj["F0000013"] + string.Empty; //工件孔径
            }

            DateTime startTime = recordObj["StartTime"] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj["StartTime"] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime); //与现在时间的差值
            recordObj["F0000013"] = delayTime.TotalHours; //实际耗时

            recordObj["F0000015"] = thisObj["F0000048"] + string.Empty; //数据代码
            recordObj.Create();

            currentTask["F0000144"] = recordObj.ObjectId;  //当前任务加工记录
            currentTask.Update();

        }

        if (actionName == "Submit" && activityCode == "Activity68") //质量检验
        {
            H3.DataModel.BizObject recordObj = null; //机加工任务记录
            string systemUserId = H3.Organization.User.SystemUserId; //系统用户

            if (lstArray != null && lstArray.Length > 0)
            {
                for (int i = taskNum - 1; i >= 0; i--)
                {
                    recordObj = null; //清空机加工任务记录数据值
                    //循环加载机加工任务记录数据
                    recordObj = H3.DataModel.BizObject.Load(systemUserId, this.Engine, "D0014194963919529e44d60be759656d4a16b63", lstArray[i]["F0000144"] + string.Empty, false);
                    if (recordObj != null)
                    {
                        recordObj["F0000009"] = i == taskNum ? thisObj["F0000020"] + string.Empty : "合格";  //检验结果
                        //recordObj["F0000029"] = thisObj["F0000138"] + string.Empty; //探伤结果

                        recordObj["F0000033"] = thisObj["F0000091"] + string.Empty; //实际外径
                        recordObj["F0000034"] = thisObj["F0000092"] + string.Empty; //实际内径
                        recordObj["F0000035"] = thisObj["F0000093"] + string.Empty; //实际总高
                        recordObj["F0000036"] = thisObj["F0000094"] + string.Empty; //实际片厚
                        recordObj["F0000038"] = thisObj["F0000095"] + string.Empty; //实际孔数
                        recordObj["F0000039"] = thisObj["F0000096"] + string.Empty; //实际孔径
                        recordObj["F0000037"] = thisObj["F0000097"] + string.Empty; //实际单重

                        recordObj.Update();
                    }
                }
            }
        }
    }
    /*
    <summary>
    关于发起异常之后节点的显示与隐藏，包含“确认调整意见”，“审批确认”节点。
    Param： Schema me  本参数是本工序表单的对象
    方法的入口参数;
    Version:1.0
    Date:2021/6/9
    Author：zzx   
    </summary>
    */
    protected void abnormalStep(Schema me, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        var row = me.GetRow(this.Request.BizObjectId);
        //  var postValue = me.CurrentPostValue;
        var Code = this.Request.ActivityCode;
        var actName = this.Request.ActivityTemplate.DisplayName;

        //发起异常
        var startException = this.Request.BizObject["F0000045"] + string.Empty;
        if (startException == "是")
        {
            createLog(me, postValue);
        }

        if (Code == "Activity87")
        {
            updateLog(me, postValue);
        }
        if (Code == "Activity88")
        {
            me.Cell("发起异常", "否");
            // me.Cell("转至工步", "待转运 ");
            me.Cell("异常描述", "操作错误，重新选择节点 ");
            me.Cell("异常类别", "安全异常 ");
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
    </summary>
   */
    public void createLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {
        var scmAbnormalType = new Schema(this.Engine, "异常工步记录表");
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
    /*
   <summary>
   根据本表单的修改更新到,《异常工步记录表》。
   Param： Schema me  本参数是本工序表单的对象
   方法的入口参数;
   Version:1.0
   Date:2021/6/9
   Author：zzx   
    </summary>
   */
    public void updateLog(Schema me, H3.SmartForm.SmartFormPostValue postValue)
    {
        //异常工步记录表
        var scmAbnormalType = new Schema(this.Engine, "异常工步记录表");
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
}
