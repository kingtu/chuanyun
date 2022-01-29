
using System;
using System.Collections.Generic;
using System.Text;
using H3;
using System.Data;
using H3.DataModel;

public class D001419Sgljz62e1rneytbqjckbe1vu25 : H3.SmartForm.SmartFormController
{
    private const string ActivityRK = "Activity45"; //入库活动。
    private const string ActivityLH = "Activity256"; //理化数据。    
    private const string ActivitySMGSJ = "Activity261"; //四面光上机
    private const string ActivitySMGXJ = "Activity259";//四面见光下机

    private const string ActivityDZY = "Activity252";//待转运
    private const string ActivityQYSJ = "Activity23";//取样上机
    private const string ActivityQYXJ = "Activity104";//取样下机

    BizObject me;
    string activityCode;
    string ProcessName = "取样";

    Dispatch dp = null;
    Salary slr = null;

    public D001419Sgljz62e1rneytbqjckbe1vu25(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        dp = new Dispatch(this.Request.Engine, (string)me[SamplingSubProcess.ID]);//派工信息
        activityCode = this.Request.ActivityCode;  //活动节点编码   
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        UserId(response);
        //var pp = dp.GetPerson(ProcessName, (string) me[SamplingSubProcess.CurrentWorkshop], (BizObject[]) me[SamplingSubProcess.FourSideLight]);

        if (activityCode == ActivityRK)
        {   //获取转运位置
            GetTransferlocation();
        }

        if (!this.Request.IsCreateMode)
        {
            //当前工序
            if (me[SamplingSubProcess.CurrentOperation] + string.Empty == string.Empty) { me[SamplingSubProcess.CurrentOperation] = "取样子流程"; }
            //初始化产品类别
            ProductCategoryUpdate();
            //获取多阶段加工子表
            H3.DataModel.BizObject[] thisLstArray = me[ProcessOfAppearanceSubtabulation.TableCode] as H3.DataModel.BizObject[];
            //初始化任务名称
            me[SamplingSubProcess.TaskName] = me[SamplingSubProcess.TaskName] + string.Empty != string.Empty ? me[SamplingSubProcess.TaskName] + string.Empty : "1"; //初始化计数器
            if (thisLstArray == null)
            {
                //本表单纲目结构
                H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
                //初始化子表
                CreatSublist(me);
            }
            //当前流程是否处于未完成状态
            // if(this.Request.WorkflowInstance.IsUnfinished)
            // {
            //获取当前流程实例对象
            H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
            var parentId = instance != null ? instance.BizObjectId : "";
            //获取父流程业务对象
            H3.DataModel.BizObject parentObjj = Tools.BizOperation.Load(this.Request.Engine, RoughCast.TableCode, parentId);
            var acac = parentObjj.WorkflowInstanceId;
            H3.Workflow.Instance.WorkflowInstance instances = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(acac);
            //获取工序计划表数据
            H3.DataModel.BizObject planObject = LoadingConfig.GetPlanningData(this.Engine, instances);
            //读取《工序计划表》是否制样
            me[SamplingSubProcess.YesOrNoopenSamplePreparationProcess] = planObject != null ? planObject[ABCDProcessPlan.YesOrNoopenSamplePreparationProcess] + string.Empty : string.Empty;
            // }


            //统计机加工耗时
            MachiningTime();
            //初始化探伤表
            InitFlawDetectionForm();
        }

        //更新本表
        me.Update();
        FillDispatchPlan();

        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {

        Result(postValue, response);

        if (actionName == "Submit")
        {
            //多阶段加工流程逻辑
            MultistageProcessingLogic(activityCode);
        }
        //四面光任务记录
        ProcessRecord(actionName);
        //审批人记录
        Authority.Approver(this.Request);
        //获取派工信息并分配权限
        Dispatcher(actionName);
        base.OnSubmit(actionName, postValue, response);

        // //多阶段加工新方案同步机加工任务记录
        UpdateRecordForm(actionName, activityCode);
    }

    /**
            * Auther：zlm
            * 填充派工计划控件
            */
    private void FillDispatchPlan()
    {

        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, Dispatchs.ID, H3.Data.ComparisonOperatorType.Equal, this.Request.BizObject[SamplingSubProcess.ID]);


        var theDispatch = Tools.BizOperation.GetFirst(this.Engine, Dispatchs.TableCode, filter);
        this.Request.BizObject["DispatchPlan"] = theDispatch[Dispatchs.Objectid] + string.Empty;

    }

    /*
    *--Author:zlm
    * 获取派工数据，并分配权限  
    * @param actionName 按钮名称
    * 2021-11-11
    */
    private void Dispatcher(string actionName)
    {


        if (actionName == "Submit")
        {
            var totalLoad = (int)this.Request.BizObject[SamplingSubProcess.TotalAmountCompleted];


            switch (activityCode)
            {
                case ActivityDZY:
                    me[SamplingSubProcess.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[SamplingSubProcess.FourSideLight]);
                    break;
                case ActivitySMGSJ:
                    me[SamplingSubProcess.Worker] = this.Request.UserContext.UserId;
                    break;
                case ActivitySMGXJ:
                    me[SamplingSubProcess.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[SamplingSubProcess.Sampling]);
                    break;
                case ActivityQYSJ:
                    me[SamplingSubProcess.Worker] = this.Request.UserContext.UserId;
                    break;
                case ActivityLH:
                    slr = new Salary(this.Engine, (string)me[SamplingSubProcess.ID]);
                    slr.Save("四面光", true);
                    break;
                case ActivityQYXJ:
                    if (totalLoad < 0)
                    {
                        me[SamplingSubProcess.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[SamplingSubProcess.Sampling]);
                    }


                    break;
                default:

                    break;
            }
        }

    }


    /*
        *--Author:zlm
        * 获取转运位置
        * @param actionName 按钮名称
        * 2021-11-11
        */
    private void GetTransferlocation()
    {
        string[] r = dp.GetPlanWorkShop(ProcessName);
        if (r.Length >= 2)
        {
            me[SamplingSubProcess.CurrentWorkshop] = r[0];
            me[SamplingSubProcess.CurrentLocation] = r[1];
        }
    }


    private void UserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem sd = new H3.SmartForm.SmartFormResponseDataItem();
        sd.Value = this.Request.UserContext.UserId;
        response.ReturnData.Add("UserId", sd);

    }


    /*
     *--Author:zlm
     * 四面光任务记录  
     * @param actionName 按钮名称
     * 2021-11-11
     */
    private void ProcessRecord(string actionName)
    {
        if (actionName == "Submit" && activityCode == ActivitySMGXJ)
        {
            TaskRecorder taskRecorder = new TaskRecorder(this.Engine, this.Request.BizObject);
            H3.DataModel.BizObject[] subTable = this.Request.BizObject[SamplingSubProcess.FourSideLight] as H3.DataModel.BizObject[];
            if (subTable != null && subTable.Length > 0)
            {
                subTable[subTable.Length - 1][SamplingFourLathe.ProcessRecord] = taskRecorder.TaskRecord("取样四面光", subTable[subTable.Length - 1]);

            }
        }

    }


    //质检结论值的由来
    protected void Result(H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //探伤结果
        if (activityCode == "Activity255")
        {
            //探伤结果
            var unqualified = me[SamplingSubProcess.FlawDetectionResults] + string.Empty;
            me[SamplingSubProcess.QualityInspectionConclusion] = unqualified;
        }
    }

    /*
     *--Author:fubin
     * 产品类别为空时，查询产品参数表中的车加工类别
     */
    protected void ProductCategoryUpdate()
    {
        //产品类别更新
        if (me[SamplingSubProcess.ProductCategory] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = me[SamplingSubProcess.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable typeData = this.Engine.Query.QueryTable(mySql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[SamplingSubProcess.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                me[SamplingSubProcess.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
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
            " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity104' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", SamplingSubProcess.TableCode, bizid);
        DataTable data = this.Engine.Query.QueryTable(command, null);
        //机加工耗时计算
        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                double utime = double.Parse(utimestr) / 10000000 / 60;
                me[SamplingSubProcess.ActualProcessingTime] = utime;
            }
        }
    }

    /*
     *--Author:fubin
     * 初始化探伤表
     */
    protected void InitFlawDetectionForm()
    {   //探伤表
        string tsFormId = me[SamplingSubProcess.FlawDetectionTable] + string.Empty;
        //当前用户ID
        string userId = this.Request.UserContext.UserId;
        //活动节点
        string activityName = this.Request.ActivityCode;
        if (tsFormId == string.Empty)  //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        {
            H3.DataModel.BizObject tsForm = Tools.BizOperation.New(this.Engine, InspectionTable.TableCode);
            tsForm.Status = H3.DataModel.BizObjectStatus.Effective; //生效
            tsForm[InspectionTable.ID] = me[SamplingSubProcess.ID] + string.Empty;  //ID
            tsForm[InspectionTable.CurrentOperation] = "取样子流程";
            tsForm[InspectionTable.SampleProcess] = me.ObjectId;
            List<H3.DataModel.BizObject> lstObject = new List<H3.DataModel.BizObject>();   //new子表数据集合
            //new一个子表业务对象
            H3.DataModel.BizObject lstArray = Tools.BizOperation.New(this.Engine, InspectionSubTable.TableCode);//子表对象
            lstArray[InspectionSubTable.Process] = "取样子流程"; //工序
            lstArray[InspectionSubTable.WorkStep] = "";     //工步
            lstObject.Add(lstArray);//将这个子表业务对象添加至子表数据集合中
            tsForm[InspectionTable.FlawDetectionRecord] = lstObject.ToArray(); //子表数据赋值
            tsForm.Create();
            me[SamplingSubProcess.FlawDetectionTable] = tsFormId = tsForm.ObjectId; //探伤表
            me.Update();
        }

        if (tsFormId != string.Empty) //探伤表不为空时,写入工序信息
        {
            H3.DataModel.BizObject tsForm = Tools.BizOperation.Load(this.Engine, InspectionTable.TableCode, tsFormId);//加载探伤表
            H3.DataModel.BizObject[] lstArray = tsForm[InspectionSubTable.TableCode] as H3.DataModel.BizObject[];  //获取子表数据
            tsForm[InspectionTable.CurrentOperation] = "取样子流程";
            tsForm[InspectionTable.SampleProcess] = me.ObjectId;
            if (lstArray[lstArray.Length - 1][InspectionSubTable.ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
            {
                lstArray[lstArray.Length - 1][InspectionSubTable.Process] = "取样子流程"; //工序
                lstArray[lstArray.Length - 1][InspectionSubTable.WorkStep] = me[SamplingSubProcess.CurrentWorkStep] + string.Empty;//工步
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
        H3.DataModel.BizObject ChildObj = Tools.BizOperation.New(this.Engine, ProcessOfAppearanceSubtabulation.TableCode);
        ChildObj[ProcessOfAppearanceSubtabulation.TaskName] = thisObj[SamplingSubProcess.TaskName] + string.Empty == string.Empty ? "1" : thisObj[SamplingSubProcess.TaskName] + string.Empty; //任务名称
        Tools.BizOperation.AddChildBizObject(this.Engine, thisObj, ProcessOfAppearanceSubtabulation.TableCode, ChildObj);
    }

    /*
     *--Author:fubin
     * 多阶段加工流程逻辑
     * @param activityCode 流程节点编码
     */
    public void MultistageProcessingLogic(string activityCode)
    {
        //获取多阶段加工子表
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = me[ProcessOfAppearanceSubtabulation.TableCode] as H3.DataModel.BizObject[];

        {   //修正任务数-----任务名称
            me[SamplingSubProcess.TaskName] = lstArray != null ? lstArray.Length + string.Empty : "1";
            //获取任务数
            int taskNum = me[SamplingSubProcess.TaskName] + string.Empty != string.Empty ? int.Parse(me[SamplingSubProcess.TaskName] + string.Empty) - 1 : 0; //获取任务数

            if (activityCode == "Activity23") //本取上机
            {    //当前加工者
                lstArray[taskNum][ProcessOfAppearanceSubtabulation.Processor] = this.Request.UserContext.UserId;
                //加工开始时间
                lstArray[taskNum][ProcessOfAppearanceSubtabulation.StartTime] = System.DateTime.Now;
            }

            if (activityCode == "Activity104") //本取下机
            {
                //任务递增至本取下机
                if (me[SamplingSubProcess.CompletionCost] + string.Empty != "已本取")
                {
                    //递增计数器，并更新
                    me[SamplingSubProcess.TaskName] = lstArray.Length + 1;
                    //创建添加新的子表行数据
                    CreatSublist(me);
                }

                if (lstArray[taskNum][ProcessOfAppearanceSubtabulation.Processor] + string.Empty == string.Empty)
                {   //当前加工者
                    lstArray[taskNum][ProcessOfAppearanceSubtabulation.Processor] = this.Request.UserContext.UserId;
                }
            }

        }
        //探伤表Id
        string objId = me[SamplingSubProcess.FlawDetectionTable] + string.Empty;
        //返回探伤结果
        if (activityCode == "Activity105" && objId != string.Empty)
        {
            H3.DataModel.BizObject tsForm = Tools.BizOperation.Load(this.Engine, InspectionTable.TableCode, objId);
            //赋值探伤认定
            me[SamplingSubProcess.FlawDetectionIdentification] = tsForm[InspectionTable.FlawDetectionIdentification] + string.Empty;
        }
    }

    //同步数据至“机加工任务记录”
    public void UpdateRecordForm(string actionName, string activityCode)
    {
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = me[ProcessOfAppearanceSubtabulation.TableCode] as H3.DataModel.BizObject[];  //获取取样子表
        //任务计数器
        int taskNum = me[SamplingSubProcess.CompletionCost] + string.Empty != "已本取" ? int.Parse(me[SamplingSubProcess.TaskName] + string.Empty) - 2 : int.Parse(me[SamplingSubProcess.TaskName] + string.Empty) - 1;
        if (actionName == "Submit" && activityCode == "Activity104" && lstArray != null) //四面见光下机\本取下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask[ProcessOfAppearanceSubtabulation.Processor] + string.Empty) as H3.Organization.User;

            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //轧制方式
            string zzMode = me[SamplingSubProcess.RollingMethod] + string.Empty;
            //产品类别
            string productType = me[SamplingSubProcess.ProductCategory] + string.Empty;
            //设备类型
            string deviceType = currentTask[ProcessOfAppearanceSubtabulation.EquipmentType] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;

            //产品类别
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From i_{0} Where {1} = '粗车' and {2} = '{3}'", DeviceWorkingHour.TableCode, DeviceWorkingHour.OperationName, DeviceWorkingHour.ProductMachiningCategory, productType);//产品类别
                DataTable data = this.Engine.Query.QueryTable(command, null);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, DeviceWorkingHour.TableCode, data.Rows[0]["ObjectId"] + string.Empty, true);
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
                    break;
                }

            }

            //产品参数表
            H3.DataModel.BizObject productObj = Tools.BizOperation.Load(this.Engine, ProductParameter.TableCode, me[SamplingSubProcess.ProductParameterTable] + string.Empty);

            if (productObj != null)
            {
                //产品参数表-单轧工时
                if (zzMode == "单轧" && productObj[ProductParameter.SingleRoughingMaNHour] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"单轧毛坯工时"
                    productTime = productObj[ProductParameter.SingleRoughingMaNHour] + string.Empty;
                    //单轧毛坯下屑
                    totalxx = productObj[ProductParameter.SingleRollingRoughTurningChip] + string.Empty;
                }
                //产品参数表-双轧工时
                if (zzMode == "双轧" && productObj[ProductParameter.DoubleRoughingManhour] != null)
                {//根据本表单产品轧制方式从产品参数表中获取"双轧毛坯工时"
                    productTime = productObj[ProductParameter.DoubleRoughingManhour] + string.Empty;
                    //双轧毛坯下屑
                    totalxx = productObj[ProductParameter.DoubleRollingRoughingChip] + string.Empty;
                }
            }

            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(this.Engine, MachiningTaskRecord.TableCode);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态

            recordObj[MachiningTaskRecord.OperationName] = "取样"; //工序
            recordObj[MachiningTaskRecord.ProductSpecification] = me[SamplingSubProcess.ProductSpecification] + string.Empty; //产品规格
            recordObj[MachiningTaskRecord.ID] = me[SamplingSubProcess.ID] + string.Empty; //工件ID
            recordObj[MachiningTaskRecord.WorkPieceNumber] = me[SamplingSubProcess.WorkpieceNumber] + string.Empty; //工件号
            recordObj[MachiningTaskRecord.TaskName] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord.SampleType] = me[SamplingSubProcess.SampleType] + string.Empty; //试样类型

            recordObj[MachiningTaskRecord.Processor] = currentTask[ProcessOfAppearanceSubtabulation.Processor] + string.Empty; //加工者
            recordObj[MachiningTaskRecord.DepartmentName] = employee != null ? employee.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord.StartTime] = currentTask[ProcessOfAppearanceSubtabulation.StartTime] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord.DeviceName] = currentTask[ProcessOfAppearanceSubtabulation.EquipmentName] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord.DeviceNumber] = currentTask[ProcessOfAppearanceSubtabulation.EquipmentNumber] + string.Empty; //设备编号
            recordObj[MachiningTaskRecord.DeviceType] = currentTask[ProcessOfAppearanceSubtabulation.EquipmentType] + string.Empty; //设备类型
            recordObj[MachiningTaskRecord.DeviceCoefficient] = deviceParam; //设备工时系数
            recordObj[MachiningTaskRecord.RollingMethod] = zzMode; //轧制方式
            recordObj[MachiningTaskRecord.ProcessChipWeight] = totalxx; //工艺下屑量
            recordObj[MachiningTaskRecord.WorkLoad] = currentTask[ProcessOfAppearanceSubtabulation.ProcessingQuantity] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord.EndTime] = DateTime.Now; //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
            double mScale = currentTask[ProcessOfAppearanceSubtabulation.ProcessingQuantity] + string.Empty != string.Empty ? double.Parse(currentTask[ProcessOfAppearanceSubtabulation.ProcessingQuantity] + string.Empty) : 0; //加工量转换
            recordObj[MachiningTaskRecord.ProcessManHour] = pTime; ////本工序产品工时
            recordObj[MachiningTaskRecord.UnitmanHour] = pTime * dParam; //单件拟定工时
            recordObj[MachiningTaskRecord.TaskManHour] = pTime * dParam * mScale; //任务工时

            if (productObj != null)
            {
                recordObj[MachiningTaskRecord.ProductName] = productObj[ProductParameter.ProductName] + string.Empty; //产品名称
                recordObj[MachiningTaskRecord.LatheProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品类别
                recordObj[MachiningTaskRecord.DrillingProcessingCategory] = productObj[ProductParameter.ProductDrillingCategory] + string.Empty; //产品小类
                recordObj[MachiningTaskRecord.OrderSpecifications] = productObj[ProductParameter.OrderSpecificationNumber] + string.Empty; //订单规格号
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
            recordObj[MachiningTaskRecord.DataCode] = me[SamplingSubProcess.DataCode] + string.Empty; //数据代码
            recordObj.Create();

            //thisObj["F0000106"] = actionName == "Activity24" && decimal.Parse(currentTask["F0000162"] + string.Empty) > 0 ? "已本取" : "未本取";
            currentTask[ProcessOfAppearanceSubtabulation.ProcessingRecord] = recordObj.ObjectId;  //当前任务加工记录
            currentTask.Update();
        }

        string systemUserId = H3.Organization.User.SystemUserId; //系统用户

        if (lstArray != null && lstArray.Length > 0)
        {
            for (int i = taskNum - 1; i >= 0; i--)
            {
                H3.DataModel.BizObject recordObj = null; //清空机加工任务记录数据值
                //循环加载机加工任务记录数据
                recordObj = Tools.BizOperation.Load(this.Engine, MachiningTaskRecord.TableCode, lstArray[i][ProcessOfAppearanceSubtabulation.ProcessingRecord] + string.Empty);
                if (recordObj != null)
                {
                    recordObj[MachiningTaskRecord.InspectionResults] = i == taskNum ? me[SamplingSubProcess.InspectionResults] + string.Empty : "合格";  //检验结果
                    recordObj[MachiningTaskRecord.UltrasonicResults] = me[SamplingSubProcess.FlawDetectionResults] + string.Empty; //探伤结果
                    recordObj.Update();
                }
            }
        }
    }
    /*
     *--Author:fubin
     * 子流程中根据当前工件流程追溯工序计划数据
     * @param instance 当前工件流程
     */
    // public static H3.DataModel.BizObject GetPlanningDatas(H3.IEngine engine, H3.Workflow.Instance.WorkflowInstance instance)
    // {
    //     //获取父流程实例对象
    //     instance = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.ParentInstanceId);

    //     var parentId = instance != null ? instance.BizObjectId : "";

    //     //获取父流程业务对象
    //     H3.DataModel.BizObject parentObjj = Tools.BizOperation.Load(engine, RoughCast.TableCode, parentId);
    //     var acac = parentObjj.WorkflowInstanceId;

    //      //获取当前流程实例对象

    //     var acaca = instances != null ? instances.BizObjectId : "";

    //     //获取父流程业务对象
    //     H3.DataModel.BizObject parentObj = Tools.BizOperation.Load(engine, ProcessFlow.TableCode, acaca);

    //     string planId = parentObj != null ? parentObj[ProcessFlow.OperationSchedule] + string.Empty : string.Empty;
    //     //获取工序计划业务对象
    //     H3.DataModel.BizObject planObj = Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planId);

    //     return planObj;
    // }
}
