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

    String ProcessName = "精车";
    string activityCode;
    Dispatch dp;
    H3.DataModel.BizObject me;
    H3.SmartForm.SmartFormResponseDataItem item;  //用户提示信息
    string info = string.Empty;  //值班信息
    string userName = ""; //当前用户
    public D001419Sqy2b1uy8h8cahh17u9kn0jk10(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        activityCode = this.Request.ActivityCode;
        dp = new Dispatch(this.Request.Engine, (string)me[Finishing.ID]);//派工信息
        item = new H3.SmartForm.SmartFormResponseDataItem();
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!this.Request.IsCreateMode)
            {
                //初始化控件
                InitTableComponent();
                //同步机加工任务记录
                DataSync.instance.JCSyncData(this.Engine);
                //初始化产品类别
                ProductCategoryUpdate();
                if (this.Request.WorkflowInstance.IsUnfinished)
                {
                    //统计机加工耗时
                    MachiningTime();
                    //初始化探伤表
                    InitFlawDetectionForm();
                    //获取工序计划表数据
                    H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
                }
                me.Update();
            }
            FillDispatchPlan();
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

        response.ReturnData.Add("key1", item);
        base.OnLoad(response);

        //--------------------------加载前后分割线-------------------------//

        try
        {
            if (!this.Request.IsCreateMode)
            {
                //加载后代码
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
            //发起异常
            string strInitiateException = me[Finishing.InitiateException] + string.Empty;
            //提交
            if (actionName == "Submit")
            {
                //清空转至工步信息
                ClearTransferToWorkStep();
                //校验异常信息是否与数据库保持一致
                bool checkedResult = CheckExceptionInfo(response);
                if (checkedResult)
                {
                    return;
                }
                //活动节点编码
                string activityCode = this.Request.ActivityCode;
                //多阶段加工流程逻辑
                MultistageProcessingLogic(activityCode);
                //取得派工信息
                Dispatcher(actionName);

                //审批人追加
                Authority.Approver(this.Request);
                if (activityCode == ActivitySJ)
                {
                    //产品工时
                    TheProductWorkingHours();
                }
                fufuzhi();
                base.OnSubmit(actionName, postValue, response);
            }
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
            //多阶段加工新方案升级机加工任务记录
            UpdateRecordForm(actionName, activityCode);
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

    /**
    * --Author: zzx
    * 初始化控件
    * 
*/
    public void InitTableComponent()
    {
        //当前工序
        if (me[Finishing.CurrentOperation] + string.Empty == string.Empty)
        {
            me[Finishing.CurrentOperation] = "精车";
        }
        //获取多阶段加工子表
        H3.DataModel.BizObject[] thisLstArray = me[Finishing.MachiningInformation] as H3.DataModel.BizObject[];

        if (thisLstArray == null)
        {
            //初始化子表
            CreatSublist(me);
        }
        //初始化任务名称
        me[Finishing.TaskName] = me[Finishing.TaskName] + string.Empty != string.Empty ? me[Finishing.TaskName] + string.Empty : "1";

        //加工难度设置默认值1。
        if (me[Finishing.ProcessingDifficulty] + string.Empty == "") { me[Finishing.ProcessingDifficulty] = 1; }

        if (me[Finishing.QualityInspectionConclusion] + string.Empty == string.Empty)
        {
            me[Finishing.QualityInspectionConclusion] = "合格";
        }
        if (me[Finishing.TotalAmountCompleted] + string.Empty == string.Empty)
        {
            me[Finishing.TotalAmountCompleted] = 0;
            
        }

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
        if (activityCode != "Activity55" && activityCode != "Activity56")
        {
            me[Finishing.TransferToWorkStep] = "";
        }
    }
    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    * 
*/
    protected bool CheckExceptionInfo(H3.SmartForm.SubmitSmartFormResponse response)
    {
        //表单中发起异常
        string strInitiateException = me[Finishing.InitiateException] + string.Empty;
        if (strInitiateException == "是")
        {
            return false;
        }
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库中发起异常的值
        string sqlInitiateException = thisObj[Finishing.InitiateException] + string.Empty;
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
     * Auther：zlm
     * 填写派工计划关联表单
     */
    private void FillDispatchPlan()
    {

        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, Dispatchs.ID, H3.Data.ComparisonOperatorType.Equal, this.Request.BizObject[Finishing.ID]);
        var theDispatch = Tools.BizOperation.GetFirst(this.Engine, Dispatchs.TableCode, filter);
        this.Request.BizObject["DispatchPlan"] = theDispatch[Dispatchs.Objectid] + string.Empty;

    }
    //取得派工信息并分配权限
    //zlm
    protected void Dispatcher(string actionName)
    {

        //var totalLoad = (int) this.Request.BizObject[Finishing.TotalAmountCompleted];

        var theBizObject = this.Request.BizObject;

        var totalLoad = Convert.ToDouble(this.Request.BizObject[Finishing.TotalAmountCompleted]);

        if (activityCode == ActivityRK && actionName == ActionSubmit)
        {
            me[Finishing.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[Finishing.MachiningInformation]);
            FillDispatchInfo();
        }
        if (activityCode == ActivityXJ && actionName == ActionSubmit && totalLoad < 1)
        {
            me[Finishing.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[Finishing.MachiningInformation]);
            FillDispatchInfo();
        }

        if (activityCode == ActivitySJ && actionName == ActionSubmit)
        {
            me[Finishing.Worker] = this.Request.UserContext.UserId;
        }

    }

    /*
             *--Author:zhanglimin
             * 填写派工计划信息
             * @param activityCode 流程节点编码
             */
    private void FillDispatchInfo()
    {

        var theBizObject = this.Request.BizObject;

        Dispatch.PlanData[] planDevices = dp.PlanInfos;
        if (planDevices != null)
        {
            string plansDevice = "";
            Double plansMonHour = 0;
            double plansWorkLoad = 0;
            foreach (Dispatch.PlanData item in planDevices)
            {
                plansDevice = item.DeviceName;
                plansMonHour = item.MonHour;
                plansWorkLoad = item.WorkLoad;

            }
            theBizObject["PlanDevices"] = plansDevice;
            theBizObject["PlanMonHour"] = plansMonHour;
            theBizObject["PlanWorkLoad"] = plansWorkLoad;
        }

    }




    /*
     *--Author:fubin
     * 查询更新机加工耗时
     */
    protected void MachiningTime()
    {
        string bizid = me.ObjectId;
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
                me[Finishing.ActualProcessingTime] = utime;
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
        if (me[Finishing.ProductCategory] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = me[Finishing.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mysql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable typeData = this.Engine.Query.QueryTable(mysql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[Finishing.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                me[Finishing.ProductCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
            }
        }
    }


    /*
     *--Author:fubin
     * 初始化探伤表
     */
    protected void InitFlawDetectionForm()
    {
        //探伤表数据Id
        string tsFormId = me[Finishing.FlawDetectionTable] + string.Empty;
        //流程节点名称
        string activityName = this.Request.WorkItem.ActivityDisplayName;
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (tsFormId == string.Empty)
        {
            string thisId = me[Finishing.ID] + string.Empty; //ID
            string mySql = string.Format("Select {0} From i_{1} Where {2} = '{3}'", InspectionTable.Objectid, InspectionTable.TableCode, InspectionTable.ID, thisId);
            DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                me[Finishing.FlawDetectionTable] = thisId = tsData.Rows[0][InspectionTable.Objectid] + string.Empty;
            }
        }

        //探伤表不为空时,写入工序信息
        if (tsFormId != string.Empty)
        {
            H3.DataModel.BizObject tsForm = Tools.BizOperation.Load(this.Engine, InspectionTable.TableCode, tsFormId);
            //获取探伤子表
            H3.DataModel.BizObject[] lstArray = tsForm[InspectionTable.FlawDetectionRecord] as H3.DataModel.BizObject[];
            tsForm[InspectionTable.CurrentOperation] = "精车";
            tsForm[InspectionTable.RoughCutting] = me.ObjectId;
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
     */
    private void MultistageProcessingLogic(string activityCode)
    {
        H3.DataModel.BizObject[] lstArray = me[Finishing.MachiningInformation] as H3.DataModel.BizObject[];

        //修正任务数-----任务名称
        me[Finishing.TaskName] = lstArray != null ? lstArray.Length + string.Empty : "1";
        //获取任务数
        int taskNum = me[Finishing.TaskName] + string.Empty != string.Empty ? int.Parse(me[Finishing.TaskName] + string.Empty) - 1 : 0;

        if (activityCode == "Activity3") //精车上机
        {   //当前加工者
            lstArray[taskNum][FinishSubTable.Processor] = this.Request.UserContext.UserId;
            //加工开始时间
            lstArray[taskNum][FinishSubTable.StartTime] = System.DateTime.Now;
        }

        if (activityCode == "Activity24") //精车下机
        {
            //完成总量小于1时
            if ((me[Finishing.TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[Finishing.TotalAmountCompleted] + string.Empty) < 1)
            {
                //递增计数器，并更新
                me[Finishing.TaskName] = lstArray.Length + 1;
                //创建添加新的子表行数据
                CreatSublist(me);
            }

            if (lstArray[taskNum][FinishSubTable.Processor] + string.Empty == string.Empty)
            {   //当前加工者
                lstArray[taskNum][FinishSubTable.Processor] = this.Request.UserContext.UserId;
            }
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
        H3.DataModel.BizObject childObj = Tools.BizOperation.New(this.Engine, Finishing.MachiningInformation);
        //任务名称
        childObj[FinishSubTable.TaskName] = thisObj[Finishing.TaskName] + string.Empty == string.Empty ? "1" : thisObj[Finishing.TaskName] + string.Empty;
        //将这个子表业务对象添加至子表数据集合中
        Tools.BizOperation.AddChildBizObject(this.Engine, thisObj, Finishing.MachiningInformation, childObj);
    }

    /*
     *--Author:fubin
     * 更新机加工任务记录
     * @param activityCode 流程节点编码
     * @param actionName 表单按钮名称
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
            // //设备工时系数表-子表
            // H3.DataModel.BizObject[] subObj = null;
            // //设备工时系数表
            // H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask[FinishSubTable.Processor] + string.Empty) as H3.Organization.User;

            // //总下屑量
            // string totalxx = "";
            // //本工序产品工时
            // string productTime = "";
            // //轧制方式
            // //string zzMode = thisObj["F0000122"] + string.Empty;
            // //产品类别
            // string productType = thisObj[Finishing.ProductCategory] + string.Empty;
            // //设备类型
            // string deviceType = currentTask[FinishSubTable.EquipmentType] + string.Empty;
            // //设备工时系数
            // string deviceParam = string.Empty;

            // //产品类别
            // if(productType != string.Empty)
            // {
            //     //获取设备工时系数模块
            //     string command = string.Format("Select {0} From i_{1}  Where {2}= '精车' and {3} = '{4}'",
            //         DeviceWorkingHour.ObjectId, DeviceWorkingHour.TableCode, DeviceWorkingHour.OperationName, DeviceWorkingHour.ProductMachiningCategory, productType);
            //     DataTable data = this.Engine.Query.QueryTable(command, null);
            //     if(data != null && data.Rows != null && data.Rows.Count > 0)
            //     {
            //         mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, DeviceWorkingHour.TableCode, data.Rows[0][DeviceWorkingHour.ObjectId] + string.Empty, true);
            //     }
            // }

            // //设备工时系数表-子表
            // subObj = mtObj != null ? mtObj[DeviceWorkingHour.SubTable] as H3.DataModel.BizObject[] : null;

            // if(subObj != null)
            // {
            //     foreach(H3.DataModel.BizObject item in subObj)
            //     {
            //         if(deviceType != string.Empty)
            //         {        //按设备类型查找
            //             if(item[EquipmentTimeCoefficientSubtabulation.EquipmentType] + string.Empty == deviceType)
            //             {
            //                 if(item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] != null)
            //                 {               //设备工时系数
            //                     deviceParam = item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] + string.Empty;
            //                 }
            //             }
            //         }
            //     }
            // }

            //产品参数表
            H3.DataModel.BizObject productObj = Tools.BizOperation.Load(this.Engine, ProductParameter.TableCode, thisObj[Finishing.ProductParameterTable] + string.Empty);

            // if(productObj != null)
            // {
            //     //产品参数表-单轧工时
            //     if(productObj[ProductParameter.SingleRoughingMaNHour] != null)
            //     {   //根据本表单产品轧制方式从产品参数表中获取"精车工时"
            //         productTime = productObj[ProductParameter.FinishingManHour] + string.Empty;
            //         //精车下屑
            //         totalxx = productObj[ProductParameter.FinishingChip] + string.Empty;
            //     }
            // }

            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(this.Engine, MachiningTaskRecord.TableCode);
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
            recordObj[MachiningTaskRecord.DeviceCoefficient] = currentTask[FinishSubTable.EquipmentTimeCoefficient] + string.Empty; //设备工时系数
                                                                                                                                    //recordObj["F0000024"] = zzMode; //轧制方式
            recordObj[MachiningTaskRecord.ProcessChipWeight] = thisObj[Finishing.TheAmountOfScrap] + string.Empty; //工艺下屑量
            recordObj[MachiningTaskRecord.WorkLoad] = currentTask[FinishSubTable.ProcessingQuantity] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord.EndTime] = DateTime.Now; //加工结束时间
                                                                   // double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
                                                                   // double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
                                                                   // double mScale = currentTask[FinishSubTable.ProcessingQuantity] + string.Empty != string.Empty ? double.Parse(currentTask[FinishSubTable.ProcessingQuantity] + string.Empty) : 0; //加工量转换
            recordObj[MachiningTaskRecord.ProcessManHour] = thisObj[Finishing.ProductStandardWorkingHours] + string.Empty; //本工序产品工时
            recordObj[MachiningTaskRecord.UnitmanHour] = currentTask[FinishSubTable.TheProductWorkingHours] + string.Empty; //单件拟定工时
            recordObj[MachiningTaskRecord.TaskManHour] = currentTask[FinishSubTable.PersonWorkingHours] + string.Empty; //任务工时

            if (productObj != null)
            {
                recordObj[MachiningTaskRecord.ProductName] = productObj[ProductParameter.ProductName] + string.Empty; //产品名称
                recordObj[MachiningTaskRecord.LatheProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品类别
                recordObj[MachiningTaskRecord.DrillingProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品小类
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
                    recordObj = Tools.BizOperation.Load(this.Engine, MachiningTaskRecord.TableCode, lstArray[i][FinishSubTable.ProcessingRecord] + string.Empty);
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

    //检查发起异常控件是否被其它异常代表更改 - fubin
    protected bool checkExceptionInfo()
    {
        //表单发起异常
        string strInitiateException = me[Finishing.InitiateException] + string.Empty;
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库发起异常
        string sqlInitiateException = thisObj[Finishing.InitiateException] + string.Empty;
        if (strInitiateException != sqlInitiateException)
        {
            return true;
        }
        else
        {
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
        //关联其它异常工件
        String[] bizObjectIDArray = me[Finishing.AssociatedWithOtherAbnormalWorkpieces] as string[];
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
                    otherObj[activex.Name] = me[Finishing.ExceptionCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = parentInstance.BizObjectId;
                }
            }

            otherObj.Update();
        }

        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, Finishing.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity127" && strActivityCode != "Activity128")
        {
            //设置异常权限
            exceptionBo["OwnerId"] = this.Request.UserContext.UserId;
            //创建发起异常的日志
            logObjectID = AbnormalLog.CreateLog(Finishing.ID, Finishing.CurrentWorkStep, Finishing.CurrentOperation,
                Finishing.ExceptionCategory, Finishing.ExceptionDescription, this.Request.BizObject, this.Engine);
            exceptionBo[Finishing.ObjectIDForUpdateTheExceptionLog] = logObjectID;
            exceptionBo.Update();
        }
        //确认调整意见
        if (strActivityCode == "Activity127")
        {
            //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
            AbnormalLog.UpdateLog(Finishing.ID, Finishing.CurrentWorkStep, Finishing.ExceptionCategory,
                Finishing.ExceptionDescription, this.Request.BizObject, exceptionBo[Finishing.ObjectIDForUpdateTheExceptionLog] + string.Empty, this.Engine);
        }
        //审批确认
        if (strActivityCode == "Activity128")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[Finishing.InitiateException] = "否";
            //异常描述赋值
            exceptionBo[Finishing.ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[Finishing.ExceptionCategory] = "安全异常";
            //异常代表
            exceptionBo[Finishing.ExceptionRepresentative] = string.Empty;
            exceptionBo.Update();
            
        }
    }

    //产品工时
    public void TheProductWorkingHours()
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
        //产品类别
        string productType = thisObj[Finishing.ProductCategory] + string.Empty;
        //设备工时系数
        string deviceParam = string.Empty;
        //获取子表数据
        H3.DataModel.BizObject[] SubArray = me[FinishSubTable.TableCode] as H3.DataModel.BizObject[];
        foreach (H3.DataModel.BizObject Array in SubArray)
        {
            //设备类型
            string deviceType = Array[FinishSubTable.EquipmentType] + string.Empty;
            //产品类别
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select {0} From i_{1}  Where {2}= '精车' and {3} = '{4}'",
                    DeviceWorkingHour.ObjectId, DeviceWorkingHour.TableCode, DeviceWorkingHour.OperationName, DeviceWorkingHour.ProductMachiningCategory, productType);
                DataTable data = this.Engine.Query.QueryTable(command, null);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, DeviceWorkingHour.TableCode, data.Rows[0][DeviceWorkingHour.ObjectId] + string.Empty, true);
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
            H3.DataModel.BizObject productObj = Tools.BizOperation.Load(this.Engine, ProductParameter.TableCode, thisObj[Finishing.ProductParameterTable] + string.Empty);

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
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品参数表工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
                                                                                         //赋值-设备工时系数
            Array[FinishSubTable.EquipmentTimeCoefficient] = dParam;
            //赋值-产品标准工时
            me[Finishing.ProductStandardWorkingHours] = pTime;
            //赋值-产品工时
            Array[FinishSubTable.TheProductWorkingHours] = pTime * dParam;
            //赋值-下屑量
            me[Finishing.TheAmountOfScrap] = totalxx;
            //AbnormalLog.CreateLog (iD,cu)
        }
    }

    protected void fufuzhi()
    {
        if (me[Finishing.TotalAmountCompleted] + string.Empty == "")
        {
            me[Finishing.TotalAmountCompleted] = 0;
        }
    }
}
