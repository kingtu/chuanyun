
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;

public class D001419Sqy2b1uy8h8cahh17u9kn0jk10 : H3.SmartForm.SmartFormController
{

    static string ActivityRK = "Activity45"; //入库活动。
    static string ActivitySJ = "Activity3";  //上机活动。
    static string ActivityXJ = "Activity24"; //下机活动。
    static string ActivityJY = "Activity17"; //检验活动。
    static string ActionSubmit = "Submit"; //检验活动。

    private String RID = null;
    H3.DataModel.BizObject me = null;
    String ProcessName = "精车";
    Dispatch dp = null;//派工信息
    //本表单数据
    H3.DataModel.BizObject thisObj;
    //布尔值转换
    Dictionary<string, bool> boolConfig;
    public D001419Sqy2b1uy8h8cahh17u9kn0jk10(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        thisObj = this.Request.BizObject;
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);
        RID = this.Request.BizObjectId;
        //me = new Schema(this.Request.Engine, this.Request.BizObject);
        me=this.Request.BizObject;
        dp = new Dispatch(this.Request.Engine, (string)me[Finishing.ID]);//派工信息 
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        var code = this.Request.ActivityCode;

        //var t=this.JavaScriptSerializer; //.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


        //var r = this.Serialize(this.Request.BizObject);
        //var t=Newtonsoft.Json.JsonConvert;

        if (code == ActivityRK) 
        {
            string[] r = dp.GetWorkShop(ProcessName);
            if (r.Length >= 2)
            {
                me[Finishing.CurrentWorkshop] = r[0];
                me[Finishing.CurrentLocation] = r[1];
            }
            //dp.FillWorkShop(me, ProcessName);
        }

        if (!this.Request.IsCreateMode)
        {
            //当前工序
            if (thisObj[Finishing.CurrentOperation] + string.Empty == string.Empty) { thisObj[Finishing.CurrentOperation] = "精车"; }
            //获取多阶段加工子表
            H3.DataModel.BizObject[] thisLstArray = thisObj[Finishing.MachiningInformation] as H3.DataModel.BizObject[];
            //初始化产品类别
            ProductCategoryUpdate();
            //初始化任务名称
            thisObj[Finishing.TaskName] = thisObj[Finishing.TaskName] + string.Empty != string.Empty ? thisObj[Finishing.TaskName] + string.Empty : "1";

            if (thisLstArray == null)
            {
                //本表单纲目结构
                H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
                //初始化子表
                CreatSublist(thisObj, schema, thisLstArray);
            }

            //统计机加工耗时
            MachiningTime();
            //初始化探伤表
            InitFlawDetectionForm();

            if (this.Request.WorkflowInstance.IsUnfinished)
            {
                //获取工序计划表数据
                H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
                //加载质量配置
                LoadingQAConfiguration(planObj, boolConfig);
            }

            //更新本表单
            thisObj.Update();
        }

        base.OnLoad(response);

        if (!this.Request.IsCreateMode)
        {
            try
            {   //同步机加工任务记录
                DataSync.instance.JCSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //提交
        if (actionName == "Submit")
        {
            //me.CurrentPostValue = postValue;
            //活动节点编码
            string activityCode = this.Request.ActivityCode;
            //多阶段加工流程逻辑
            MultistageProcessingLogic(activityCode);
            var b = this.Request.BizObject;
            if (activityCode == ActivityRK && actionName == ActionSubmit)
            {              
                b[Finishing.Worker] = dp.GetPerson(ProcessName, (string)b[Finishing.CurrentWorkshop], (BizObject[])b[Finishing.MachiningInformation]);
            }
            if (activityCode == ActivityXJ && actionName == ActionSubmit)
            {
                b[Finishing.Worker] = dp.GetPerson(ProcessName, (string)b[Finishing.CurrentWorkshop], (BizObject[])b[Finishing.MachiningInformation]);
            }
            if (activityCode == ActivitySJ && actionName == ActionSubmit)
            {
                b[Finishing.Worker] = this.Request.ParticipantId;                          
            }

            base.OnSubmit(actionName, postValue, response);
            //多阶段加工新方案升级机加工任务记录
            UpdateRecordForm(actionName, activityCode);

            if (activityCode == ActivityJY && actionName == "Submit")
            {
                Salary slr = new Salary(this.Engine, (string)postValue.Data[Finishing.ID]);
                slr.Save(ProcessName, false);
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

    /*
     *--Author:fubin
     * 查询更新机加工耗时
     */
    protected void MachiningTime()
    {
        string bizid = thisObj.ObjectId;
        //查询精车加工中所有耗时
        string command = string.Format
            ("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_{0}" +
            " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity24' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", Finishing.TableCode, bizid);
        DataTable data = this.Engine.Query.QueryTable(command, null);
        //机加工耗时计算
        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                double utime = double.Parse(utimestr) / 10000000 / 60;
                thisObj[Finishing.ActualProcessingTime] = utime;
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
        if (thisObj[Finishing.ProductCategory] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = thisObj[Finishing.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mysql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable typeData = this.Engine.Query.QueryTable(mysql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                thisObj[Finishing.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                thisObj[Finishing.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
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
        string crossCheck = LoadingConfig.GetQualityConfigForm(this.Engine, QAConfig.PriorityLevelfineCarMutualInspection);
        //读取《质量配置表》全局上机前互检配置
        string globalCrossCheck = LoadingConfig.GetQualityConfigForm(this.Engine, QAConfig.FineCarMutualInspectionBeforeLoading);
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
                thisObj[Finishing.MutualInspectionBeforeMachineOperation] = boolConfig[globalCrossCheck] + string.Empty;
                break;
            case "计划表":
                if (planCrossCheck != string.Empty)
                {   //计划上机前互检
                    thisObj[Finishing.MutualInspectionBeforeMachineOperation] = boolConfig[planCrossCheck] + string.Empty;
                }
                else
                {   //全局上机前互检
                    thisObj[Finishing.MutualInspectionBeforeMachineOperation] = boolConfig[globalCrossCheck] + string.Empty;
                }
                break;
            case "产品表":
                if (productCrossCheck != string.Empty)
                {
                    //产品上机前互检
                    thisObj[Finishing.MutualInspectionBeforeMachineOperation] = boolConfig[productCrossCheck] + string.Empty;
                }
                else
                {
                    if (planCrossCheck != string.Empty)
                    {   //计划上机前互检
                        thisObj[Finishing.MutualInspectionBeforeMachineOperation] = boolConfig[planCrossCheck] + string.Empty;
                    }
                    else
                    {   //全局上机前互检
                        thisObj[Finishing.MutualInspectionBeforeMachineOperation] = boolConfig[globalCrossCheck] + string.Empty;
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
    {
        //探伤表数据Id
        string tsFormId = thisObj[Finishing.FlawDetectionTable] + string.Empty;
        //流程节点名称
        string activityName = this.Request.WorkItem.ActivityDisplayName;
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (tsFormId == string.Empty)
        {
            string thisId = thisObj[Finishing.ID] + string.Empty; //ID
            string mySql = string.Format("Select ObjectId From i_" + InspectionTable.TableCode + " Where F0000001 = '{0}'", thisId);
            DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                thisObj[Finishing.FlawDetectionTable] = thisId = tsData.Rows[0][InspectionTable.Objectid] + string.Empty;
            }
        }

        //探伤表不为空时,写入工序信息
        if (tsFormId != string.Empty)
        {
            H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, InspectionTable.TableCode, tsFormId, false);
            //获取探伤子表
            H3.DataModel.BizObject[] lstArray = tsForm[InspectionTable.FlawDetectionRecord] as H3.DataModel.BizObject[];
            tsForm[InspectionTable.CurrentOperation] = "精车";
            tsForm[InspectionTable.RoughCutting] = thisObj.ObjectId;
            if (lstArray[lstArray.Length - 1][InspectionSubTable.ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
            {   //写入工序
                lstArray[lstArray.Length - 1][InspectionSubTable.Process] = "精车";
                lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] = activityName != "待探伤" ? activityName : lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] + string.Empty;     //工步
                lstArray[lstArray.Length - 1].Update();
            }
            tsForm.Update();
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
        H3.DataModel.BizObject[] lstArray = thisObj[Finishing.MachiningInformation] as H3.DataModel.BizObject[];

        //修正任务数-----任务名称
        thisObj[Finishing.TaskName] = lstArray != null ? lstArray.Length + string.Empty : "1";
        //获取任务数
        int taskNum = thisObj[Finishing.TaskName] + string.Empty != string.Empty ? int.Parse(thisObj[Finishing.TaskName] + string.Empty) - 1 : 0;

        if (activityCode == "Activity3") //精车上机
        {   //当前加工者
            lstArray[taskNum][FinishSubTable.Processor] = this.Request.UserContext.UserId;
            //加工开始时间
            lstArray[taskNum][FinishSubTable.StartTime] = System.DateTime.Now;
        }

        if (activityCode == "Activity24") //精车下机
        {
            //完成总量小于1时
            if ((thisObj[Finishing.TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(thisObj[Finishing.TotalAmountCompleted] + string.Empty) < 1)
            {
                //递增计数器，并更新
                thisObj[Finishing.TaskName] = lstArray.Length + 1;
                //创建添加新的子表行数据
                CreatSublist(thisObj, schema, lstArray);
            }

            if (lstArray[taskNum][FinishSubTable.Processor] + string.Empty == string.Empty)
            {   //当前加工者
                lstArray[taskNum][FinishSubTable.Processor] = this.Request.UserContext.UserId;
            }
        }

        //探伤表
        string objId = thisObj[Finishing.FlawDetectionTable] + string.Empty;
        //返回探伤结果
        if (activityCode == "Activity86" && objId != string.Empty)
        {
            H3.DataModel.BizObject tsForm = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, Finishing.MachiningInformation, objId, false);
            //赋值探伤认定
            thisObj[Finishing.FlawDetectionIdentification] = tsForm[InspectionTable.FlawDetectionIdentification] + string.Empty;
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
        H3.DataModel.BizObject zibiao = new H3.DataModel.BizObject(this.Request.Engine, schema.GetChildSchema(Finishing.MachiningInformation), H3.Organization.User.SystemUserId);//子表对象
        zibiao[FinishSubTable.TaskName] = thisObj[Finishing.TaskName] + string.Empty == string.Empty ? "1" : thisObj[Finishing.TaskName] + string.Empty; //任务名称
        lstObject.Add(zibiao);//将这个子表业务对象添加至子表数据集合中
        thisObj[Finishing.MachiningInformation] = lstObject.ToArray(); //子表数据赋值

        thisObj.Update();   //更新对象
    }

    /*
     *--Author:fubin
     * 更新机加工任务记录
     * @param activityCode 流程节点编码
     */
    public void UpdateRecordForm(string actionName, string activityCode)
    {
        H3.DataModel.BizObject thisObj = this.Request.BizObject;
        H3.DataModel.BizObject[] lstArray = thisObj[Finishing.MachiningInformation] as H3.DataModel.BizObject[];  //获取子表
        //完成总量
        decimal count = thisObj[Finishing.TotalAmountCompleted] + string.Empty != string.Empty ? decimal.Parse(thisObj[Finishing.TotalAmountCompleted] + string.Empty) : 0;
        //任务计数器-1
        int taskNum = count < 1 ? int.Parse(thisObj[Finishing.TaskName] + string.Empty) - 2 : int.Parse(thisObj[Finishing.TaskName] + string.Empty) - 1;

        if (actionName == "Submit" && activityCode == "Activity24" && lstArray != null) //精车下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask[FinishSubTable.Processor] + string.Empty) as H3.Organization.User;

            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //轧制方式
            //string zzMode = thisObj["F0000122"] + string.Empty;
            //产品类别
            string productType = thisObj[Finishing.ProductCategory] + string.Empty;
            //设备类型
            string deviceType = currentTask[FinishSubTable.EquipmentType] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;

            //产品类别
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From i_" + DeviceWorkingHour.TableCode + "  Where " + DeviceWorkingHour.OperationName + "= '精车' and " + DeviceWorkingHour.ProductMachiningCategory + " = '{0}'", productType);//产品类别
                DataTable data = this.Engine.Query.QueryTable(command, null);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, DeviceWorkingHour.TableCode, data.Rows[0]["ObjectId"] + string.Empty, true);
                }

            }

            //设备工时系数表-子表
            subObj = mtObj != null ? mtObj[DeviceWorkingHour.SubTable] as H3.DataModel.BizObject[] : null;

            if (subObj != null)
            {
                foreach (H3.DataModel.BizObject item in subObj)
                {
                    if (deviceType != string.Empty)
                    {        //按设备类型查找
                        if (item[EquipmentTimeCoefficientSubtabulation.EquipmentType] + string.Empty == deviceType)
                        {
                            if (item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] != null)
                            {               //设备工时系数
                                deviceParam = item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] + string.Empty;
                            }
                        }
                    }
                }
            }

            //产品参数表
            H3.DataModel.BizObject productObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                this.Engine, ProductParameter.TableCode, thisObj[Finishing.ProductParameterTable] + string.Empty, false);

            if (productObj != null)
            {
                //产品参数表-单轧工时
                if (productObj[ProductParameter.SingleRoughingMaNHour] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"精车工时"
                    productTime = productObj[ProductParameter.FinishingManHour] + string.Empty;
                    //精车下屑
                    totalxx = productObj[ProductParameter.FinishingChip] + string.Empty;
                }
            }

            //机加工记录模块
            H3.DataModel.BizObjectSchema recordSchema = this.Engine.BizObjectManager.GetPublishedSchema(MachiningTaskRecord.TableCode);
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = new H3.DataModel.BizObject(this.Engine, recordSchema, H3.Organization.User.SystemUserId);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态

            recordObj[MachiningTaskRecord.OperationName] = "精车"; //工序
            recordObj[MachiningTaskRecord.TaskType] = "正常精车"; //任务类型
            recordObj[MachiningTaskRecord.ProductSpecification] = thisObj[Finishing.ProductSpecification] + string.Empty; //产品规格
            recordObj[MachiningTaskRecord.ID] = thisObj[Finishing.ID] + string.Empty; //工件ID
            recordObj[MachiningTaskRecord.WorkPieceNumber] = thisObj[Finishing.WorkpieceNumber] + string.Empty; //工件号
            recordObj[MachiningTaskRecord.TaskName] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord.Processor] = currentTask[FinishSubTable.Processor] + string.Empty; //加工者
            recordObj[MachiningTaskRecord.DepartmentName] = employee != null ? employee.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord.StartTime] = currentTask[FinishSubTable.StartTime] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord.DeviceName] = currentTask[FinishSubTable.EquipmentName] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord.DeviceNumber] = currentTask[FinishSubTable.EquipmentNumber] + string.Empty; //设备编号
            recordObj[MachiningTaskRecord.DeviceType] = currentTask[FinishSubTable.EquipmentType] + string.Empty; //设备类型
            recordObj[MachiningTaskRecord.DeviceCoefficient] = deviceParam; //设备工时系数
            //recordObj["F0000024"] = zzMode; //轧制方式
            recordObj[MachiningTaskRecord.ProcessChipWeight] = totalxx; //工艺下屑量
            recordObj[MachiningTaskRecord.WorkLoad] = currentTask[FinishSubTable.ProcessingQuantity] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord.EndTime] = DateTime.Now; //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
            double mScale = currentTask[FinishSubTable.ProcessingQuantity] + string.Empty != string.Empty ? double.Parse(currentTask[FinishSubTable.ProcessingQuantity] + string.Empty) : 0; //加工量转换
            recordObj[MachiningTaskRecord.ProcessManHour] = pTime; //本工序产品工时
            recordObj[MachiningTaskRecord.UnitmanHour] = pTime * dParam; //单件拟定工时
            recordObj[MachiningTaskRecord.TaskManHour] = pTime * dParam * mScale; //任务工时

            if (productObj != null)
            {
                recordObj[MachiningTaskRecord.ProductName] = productObj[ProductParameter.ProductName] + string.Empty; //产品名称
                recordObj[MachiningTaskRecord.LatheProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品类别
                recordObj[MachiningTaskRecord.DrillingProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品小类
                recordObj[MachiningTaskRecord.OrderSpecifications] = productObj[ProductParameter.OrderSpecificationNumber] + string.Empty; //产品编号
                recordObj[MachiningTaskRecord.UnitWeightofFinish] = productObj[ProductParameter.FinishedProductUnitWeight] + string.Empty; //成品单重
                recordObj[MachiningTaskRecord.OutsideDiameter] = productObj[ProductParameter.ACOuterDiameter] + string.Empty; //工件外径
                recordObj[MachiningTaskRecord.InsideDiameter] = productObj[ProductParameter.ACInnerDiameter] + string.Empty; //工件内径
                recordObj[MachiningTaskRecord.TotalHeight] = productObj[ProductParameter.ACTotalHeight] + string.Empty; //工件总高
                recordObj[MachiningTaskRecord.Thickness] = productObj[ProductParameter.ACSheetThickness] + string.Empty; //工件片厚
                recordObj[MachiningTaskRecord.HoleAmount] = productObj[ProductParameter.ACNumberOfHoles] + string.Empty; //工件孔数
                recordObj[MachiningTaskRecord.Aperture] = productObj[ProductParameter.ACHoleDiameter] + string.Empty; //工件孔径
            }

            DateTime startTime = recordObj[MachiningTaskRecord.StartTime] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj[MachiningTaskRecord.StartTime] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime); //与现在时间的差值
            recordObj[MachiningTaskRecord.ActualElapsedTime] = delayTime.TotalHours; //实际耗时

            recordObj[MachiningTaskRecord.DataCode] = thisObj[Finishing.DataCode] + string.Empty; //数据代码
            recordObj.Create();

            currentTask[FinishSubTable.ProcessingRecord] = recordObj.ObjectId;  //当前任务加工记录
            currentTask.Update();

        }

        if (actionName == "Submit" && activityCode == "Activity17") //质量检验
        {
            H3.DataModel.BizObject recordObj = null; //机加工任务记录
            string systemUserId = H3.Organization.User.SystemUserId; //系统用户

            if (lstArray != null && lstArray.Length > 0)
            {
                for (int i = taskNum - 1; i >= 0; i--)
                {
                    recordObj = null; //清空机加工任务记录数据值
                    //循环加载机加工任务记录数据
                    recordObj = H3.DataModel.BizObject.Load(systemUserId, this.Engine, MachiningTaskRecord.TableCode, lstArray[i][FinishSubTable.ProcessingRecord] + string.Empty, false);
                    if (recordObj != null)
                    {
                        recordObj[MachiningTaskRecord.InspectionResults] = i == taskNum ? thisObj[Finishing.InspectionResults] + string.Empty : "合格";  //检验结果
                        //recordObj["F0000029"] = thisObj["F0000138"] + string.Empty; //探伤结果

                        recordObj[MachiningTaskRecord.ActualOutsideDiameter] = thisObj[Finishing.ActualOuterDiameter] + string.Empty; //实际外径
                        recordObj[MachiningTaskRecord.ActualInsideDiameter] = thisObj[Finishing.ActualInnerDiameter] + string.Empty; //实际内径
                        recordObj[MachiningTaskRecord.ActualTotalHeight] = thisObj[Finishing.ActualTotalHeight] + string.Empty; //实际总高
                        recordObj[MachiningTaskRecord.ActualThickness] = thisObj[Finishing.ActualFilmThickness] + string.Empty; //实际片厚
                        recordObj[MachiningTaskRecord.Actualunitweight] = thisObj[Finishing.ActualUnitWeight] + string.Empty; //实际单重

                        recordObj.Update();
                    }
                }
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
            this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
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
}
