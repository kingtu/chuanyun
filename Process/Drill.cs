using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;

public class D001419Sugyf7m5q744eyhe45o26haop4 : H3.SmartForm.SmartFormController
{
    const string ActivityHJ = "Activity95";//互检活动。
    const string ActivityRK = "Activity72";//入库。
    const string ActivitySJ = "Activity3";//上机活动。
    const string ActivityXJ = "Activity35";//下机活动。
    const string ActivityJY = "Activity68";//检验活动。    

    string ProcessName = "钻孔";
    Dispatch dp = null;//派工信息
    string activityCode;
    //本表单数据
    H3.DataModel.BizObject me;
    //布尔值转换
    Dictionary<string, bool> boolConfig;
    public D001419Sugyf7m5q744eyhe45o26haop4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);

        me = this.Request.BizObject;
        activityCode = this.Request.ActivityCode;
        dp = new Dispatch(this.Request.Engine, (string)me[Drill.ID]);//派工信息 
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {

        if (!this.Request.IsCreateMode)
        {
            //当前工序
            if (me[Drill.CurrentOperation] + string.Empty == string.Empty) { me[Drill.CurrentOperation] = "钻孔"; }
            //初始化产品类别
            ProductCategoryUpdate();
            //获取多阶段加工子表
            H3.DataModel.BizObject[] lstArray = me[DrillSubTable.TableCode] as H3.DataModel.BizObject[];
            //初始化任务名称
            me[Drill.TaskName] = me[Drill.TaskName] + string.Empty != string.Empty ? me[Drill.TaskName] + string.Empty : "1";

            if (lstArray == null)
            {
                //本表单纲目结构
                H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
                //初始化多阶段加工子表
                CreatSublist(me);
            }

            if (this.Request.WorkflowInstance.IsUnfinished)
            {
                //统计机加工耗时
                MachiningTime();
                //初始化探伤表
                InitFlawDetectionForm();
                //获取工序计划表数据
                H3.DataModel.BizObject planObj = LoadingConfig.GetPlanningData(this.Engine, this.Request.WorkflowInstance);
            }

            //更新本表单
            me.Update();
        }
        FillDispatchPlan();
        base.OnLoad(response);

        if (!this.Request.IsCreateMode)
        {
            try
            {   //同步机加工任务记录
                DataSync.instance.ZKSyncData(this.Engine);
            }
            catch (Exception ex)
            {
                response.Errors.Add(System.Convert.ToString(ex));
            }
        }

    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        //发起异常
        string strInitiateException = me[Drill.InitiateException] + string.Empty;
        //活动节点编码  
        if (actionName == "Submit")
        {
            bool checkedResult = checkExceptionInfo();
            //发起异常不为是时执行与数据库值是否一致的校验
            if (strInitiateException != "是" && checkedResult)
            {
                response.Message = "异常数据有更新，请刷新页面！";
                return;
            }

            //多阶段加工流程逻辑
            MultistageProcessingLogic(activityCode);
        }

        //发起异常 是
        if (me[Drill.InitiateException] == "是")
        {
            me[Drill.Owner] = this.Request.UserContext.UserId;
        }
        //审批人追加
        Authority.Approver(this.Request);
        //取得派工数据并分配权限
        Dispatcher(actionName);
        base.OnSubmit(actionName, postValue, response);

        if (actionName == "Submit")
        {
            //多阶段加工新方案升级机加工任务记录
            UpdateRecordForm(activityCode);
        }

        if (activityCode == ActivityJY && actionName == "Submit")
        {
            Salary slr = new Salary(this.Engine, (string)postValue.Data[Drill.ID]);
            slr.Save(ProcessName, "", false);
        }

        if (strInitiateException == "是")
        {
            //异常工步
            AbnormalStep();
        }
    }

    /**
         * Auther：zlm
         * 填充派工计划控件
         */
    private void FillDispatchPlan()
    {

        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, Dispatchs.ID, H3.Data.ComparisonOperatorType.Equal, this.Request.BizObject[Drill.ID]);
        var theDispatch = Tools.BizOperation.GetFirst(this.Engine, Dispatchs.TableCode, filter);
        this.Request.BizObject["DiapatchPlan"] = theDispatch[Dispatchs.Objectid] + string.Empty;

    }
    /*
    *--Author:zlm
    * 取得派工信息并分配权限
    * @param actionName 被点击按钮名称
    */
    private void Dispatcher(string actionName)
    {
        var totalLoad = (int)this.Request.BizObject[Drill.TotalAmountCompleted];
        //在待转运节点
        if (activityCode == ActivityRK && actionName == "Submit")
        {   //从派工表取得派工信息，赋给工人字段
            me[Drill.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[Drill.MachiningInformation]);
        }
        //在下机节点
        if (activityCode == ActivityXJ && actionName == "Submit" && totalLoad < 1)
        {

            me[Drill.Worker] = dp.GetPerson(ProcessName, (BizObject[])me[Drill.MachiningInformation]);
        }
        //在上机节点
        if (activityCode == ActivitySJ && actionName == "Submit")
        {                      //当前参与者
            me[Drill.Worker] = this.Request.ParticipantId;
        }
    }
    /*
    *--Author:zlm
    * 取得转运任务信息
    * @param 
    */
    private void FillWorkShop()
    {
        if (activityCode == ActivityRK)
        {   //填充车间位置
            string[] r = dp.GetPlanWorkShop(ProcessName);
            if (r.Length >= 2)
            {
                me[Drill.CurrentWorkshop] = r[0];
                me[Drill.CurrentLocation] = r[1];
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
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = me[DrillSubTable.TableCode] as H3.DataModel.BizObject[];
        //修正任务数-----任务名称
        me[Drill.TaskName] = lstArray != null ? lstArray.Length + string.Empty : "1";
        //获取任务数
        int taskNum = me[Drill.TaskName] + string.Empty != string.Empty ? int.Parse(me[Drill.TaskName] + string.Empty) - 1 : 0;

        if (activityCode == "Activity3") //钻孔上机
        {   //当前加工者
            lstArray[taskNum][DrillSubTable.Processor] = this.Request.UserContext.UserId;
            //加工开始时间
            lstArray[taskNum][DrillSubTable.StartTime] = System.DateTime.Now;
        }

        if (activityCode == "Activity35") //钻孔下机
        {
            //完成总量小于1时
            if ((me[Drill.TotalAmountCompleted] + string.Empty) != string.Empty && decimal.Parse(me[Drill.TotalAmountCompleted] + string.Empty) < 1)
            {
                //递增计数器，并更新
                me[Drill.TaskName] = lstArray.Length + 1;
                //创建添加新的子表行数据
                CreatSublist(me);
            }

            if (lstArray[taskNum][DrillSubTable.Processor] + string.Empty == string.Empty)
            {   //当前加工者
                lstArray[taskNum][DrillSubTable.Processor] = this.Request.UserContext.UserId;
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
        string tsFormId = me[Drill.FlawDetectionTable] + string.Empty;
        //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        if (tsFormId == string.Empty)
        {
            string thisId = me[Drill.ID] + string.Empty; //ID
            string mySql = string.Format("Select ObjectId From i_{0} Where {1} = '{2}'", InspectionTable.TableCode, InspectionTable.ID, thisId);
            DataTable tsData = this.Engine.Query.QueryTable(mySql, null);
            if (tsData != null && tsData.Rows != null && tsData.Rows.Count > 0)
            {
                me[Drill.FlawDetectionTable] = thisId = tsData.Rows[0][InspectionTable.Objectid] + string.Empty;
            }
        }
    }

    /*
     *--Author:fubin
     * 产品类别为空时，查询产品参数表中的车加工类别与钻加工类别
     */
    protected void ProductCategoryUpdate()
    {
        if (me[Drill.ProductParameterTable] + string.Empty == string.Empty)
        {
            string orderSpec = me[Drill.OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别与钻加工类别
            string mysql = string.Format("Select ObjectId,{0},{1} From i_{2} Where {3} = '{4}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.ProductDrillingCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);

            DataTable typeData = this.Engine.Query.QueryTable(mysql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[Drill.ProductParameterTable] = typeData.Rows[0][ProductParameter.Objectid] + string.Empty;
                //赋值车加工类别
                me[Drill.MachiningCategory] = typeData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
                //赋值钻加工类别
                me[Drill.DrillingCategory] = typeData.Rows[0][ProductParameter.ProductDrillingCategory] + string.Empty;
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
        //查询钻孔加工中所有耗时
        string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime From i_{0} " +
            "a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity35' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", Drill.TableCode, bizid);
        DataTable data = this.Engine.Query.QueryTable(command, null);

        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                //转换时间单位为秒
                double utime = double.Parse(utimestr) / 10000000 / 60;
                //实际加工耗时
                me[Drill.ActualProcessingTime] = utime;
            }
        }
    }


    /*
     *--Author:fubin
     * 创建添加新的子表行数据
     * @param me 本表单数据
     */
    protected void CreatSublist(H3.DataModel.BizObject me)
    {
        //new一个子表业务对象
        H3.DataModel.BizObject childObj = Tools.BizOperation.New(this.Engine, DrillSubTable.TableCode);
        //任务名称
        childObj[DrillSubTable.TaskName] = me[Drill.TaskName] + string.Empty == string.Empty ? "1" : me[Drill.TaskName] + string.Empty;
        //将这个子表业务对象添加至子表数据集合中
        Tools.BizOperation.AddChildBizObject(this.Engine, me, DrillSubTable.TableCode, childObj);
    }

    /*
     *--Author:fubin
     * 更新机加工任务记录
     * @param activityCode 流程节点编码
     */
    public void UpdateRecordForm(string activityCode)
    {
        //获取本表单子表
        H3.DataModel.BizObjectSchema schema = this.Request.Engine.BizObjectManager.GetPublishedSchema(this.Request.SchemaCode);
        H3.DataModel.BizObject[] lstArray = me[DrillSubTable.TableCode] as H3.DataModel.BizObject[];
        //完成总量
        decimal count = me[Drill.TotalAmountCompleted] + string.Empty != string.Empty ? decimal.Parse(me[Drill.TotalAmountCompleted] + string.Empty) : 0;
        //当前子表行数
        int taskNum = lstArray.Length - 1;

        if (activityCode == "Activity35" && lstArray != null) //钻孔下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            H3.DataModel.BizObject[] subObj = null;
            //设备工时系数表
            H3.DataModel.BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = this.Engine.Organization.GetUnit(currentTask[DrillSubTable.Processor] + string.Empty) as H3.Organization.User;

            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //产品小类
            string productType = me[Drill.DrillingCategory] + string.Empty;
            //设备类型
            string deviceType = currentTask[DrillSubTable.EquipmentName] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;

            //产品小类
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From i_{0} Where {1} = '钻孔' and {2} = '{3}'", DeviceWorkingHour.TableCode, DeviceWorkingHour.OperationName, DeviceWorkingHour.ProductDrillingCategory, productType);//产品小类
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
                            if (item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] != null)
                            {               //设备工时系数
                                deviceParam = item[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient] + string.Empty;
                            }
                        }
                    }
                }
            }

            //产品参数表
            H3.DataModel.BizObject productObj = Tools.BizOperation.Load(this.Engine, ProductParameter.TableCode, me[Drill.ProductParameterTable] + string.Empty);

            if (productObj != null)
            {
                //根据本表单产品轧制方式从产品参数表中获取"钻孔工时"
                productTime = productObj[ProductParameter.DrillingManHour] + string.Empty;
                //钻孔下屑
                totalxx = productObj[ProductParameter.DrillingChip] + string.Empty;

            }

            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(this.Engine, MachiningTaskRecord.TableCode);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态

            recordObj[MachiningTaskRecord.OperationName] = "钻孔"; //工序
            recordObj[MachiningTaskRecord.TaskType] = "正常钻孔"; //任务类型
            recordObj[MachiningTaskRecord.ProductSpecification] = me[Drill.ProductSpecification] + string.Empty; //产品规格
            recordObj[MachiningTaskRecord.ID] = me[Drill.ID] + string.Empty; //工件ID
            recordObj[MachiningTaskRecord.WorkPieceNumber] = me[Drill.WorkpieceNumber] + string.Empty; //工件号
            recordObj[MachiningTaskRecord.TaskName] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord.Processor] = currentTask[DrillSubTable.Processor] + string.Empty; //加工者
            recordObj[MachiningTaskRecord.DepartmentName] = employee != null ? employee.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord.StartTime] = currentTask[DrillSubTable.StartTime] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord.DeviceName] = currentTask[DrillSubTable.EquipmentName] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord.DeviceNumber] = currentTask[DrillSubTable.EquipmentNumber] + string.Empty; //设备编号
            recordObj[MachiningTaskRecord.DeviceType] = currentTask[DrillSubTable.EquipmentType] + string.Empty; //设备类型
            recordObj[MachiningTaskRecord.DeviceCoefficient] = deviceParam; //设备工时系数
            recordObj[MachiningTaskRecord.ProcessChipWeight] = totalxx; //工艺下屑量
            recordObj[MachiningTaskRecord.WorkLoad] = currentTask[DrillSubTable.ProcessingQuantity] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord.EndTime] = DateTime.Now; //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0; //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0; //设备工时系数转换
            double mScale = currentTask[DrillSubTable.ProcessingQuantity] + string.Empty != string.Empty ? double.Parse(currentTask[DrillSubTable.ProcessingQuantity] + string.Empty) : 0; //加工量转换
            recordObj[MachiningTaskRecord.ProcessManHour] = pTime; //本工序产品工时
            recordObj[MachiningTaskRecord.UnitmanHour] = pTime * dParam; //单件拟定工时
            recordObj[MachiningTaskRecord.TaskManHour] = pTime * dParam * mScale; //任务工时

            if (productObj != null)
            {
                recordObj[MachiningTaskRecord.ProductName] = productObj[ProductParameter.ProductName] + string.Empty; //产品名称
                recordObj[MachiningTaskRecord.LatheProcessingCategory] = productObj[ProductParameter.ProductMachiningCategory] + string.Empty; //产品类别
                recordObj[MachiningTaskRecord.DrillingProcessingCategory] = productObj[ProductParameter.ProductDrillingCategory] + string.Empty; //产品小类
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

            recordObj[MachiningTaskRecord.DataCode] = me[Drill.DataCode] + string.Empty; //数据代码
            recordObj.Create();

            currentTask[DrillSubTable.ProcessingRecord] = recordObj.ObjectId;  //当前任务加工记录
            currentTask.Update();
        }

        if (activityCode == "Activity68") //质量检验
        {
            H3.DataModel.BizObject recordObj = null; //机加工任务记录
            string systemUserId = H3.Organization.User.SystemUserId; //系统用户

            if (lstArray != null && lstArray.Length > 0)
            {
                for (int i = taskNum - 1; i >= 0; i--)
                {
                    recordObj = null; //清空机加工任务记录数据值
                    //循环加载机加工任务记录数据
                    recordObj = Tools.BizOperation.Load(this.Engine, MachiningTaskRecord.TableCode, lstArray[i][DrillSubTable.ProcessingRecord] + string.Empty);
                    if (recordObj != null)
                    {
                        recordObj[MachiningTaskRecord.InspectionResults] = i == taskNum ? me[Drill.InspectionResults] + string.Empty : "合格";  //检验结果
                        recordObj[MachiningTaskRecord.ActualOutsideDiameter] = me[Drill.ActualHoleDiameter] + string.Empty; //实际外径
                        recordObj[MachiningTaskRecord.ActualInsideDiameter] = me[Drill.ActualInnerDiameter] + string.Empty; //实际内径
                        recordObj[MachiningTaskRecord.ActualTotalHeight] = me[Drill.ActualTotalHeight] + string.Empty; //实际总高
                        recordObj[MachiningTaskRecord.ActualThickness] = me[Drill.ActualSheetThickness] + string.Empty; //实际片厚
                        recordObj[MachiningTaskRecord.ActualHoleCount] = me[Drill.ActualNumberOfHoles] + string.Empty; //实际孔数
                        recordObj[MachiningTaskRecord.ActualAperture] = me[Drill.ActualInnerDiameter] + string.Empty; //实际孔径
                        recordObj[MachiningTaskRecord.Actualunitweight] = me[Drill.ActualUnitWeight] + string.Empty; //实际单重
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
        string strInitiateException = me[Drill.InitiateException] + string.Empty;
        H3.DataModel.BizObject thisObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, this.Request.SchemaCode, this.Request.BizObjectId, false);
        //数据库发起异常
        string sqlInitiateException = thisObj[Drill.InitiateException] + string.Empty;
        if (strInitiateException != sqlInitiateException)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * --Author: zzx
     * 关于发起异常之后各个节点进行的操作。
     * 
     */
    protected void AbnormalStep()
    {
        //关联其它异常工件
        String[] bizObjectIDArray = me[Drill.AssociatedWithOtherAbnormalWorkpieces] as string[];
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
                    otherObj[activex.Name] = me[Drill.ExceptionCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = parentInstance.BizObjectId;
                }
            }

            otherObj.Update();
        }

        H3.DataModel.BizObject exceptionBo = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, Drill.TableCode, this.Request.BizObjectId, false);
        //写日志返回记录id
        string logObjectID = null;
        //当前节点
        var strActivityCode = this.Request.ActivityCode;
        //工步节点
        if (strActivityCode != "Activity87" && strActivityCode != "Activity88")
        {
            //设置异常权限
            this.Request.BizObject["OwnerId"] = this.Request.UserContext.UserId;
            //创建发起异常的日志
            logObjectID = ExceptionLog.CreateLog(Drill.ID, Drill.CurrentWorkStep, Drill.CurrentOperation,
                Drill.ExceptionCategory, Drill.ExceptionDescription, this.Request.BizObject, this.Engine);
            exceptionBo[Drill.ObjectIDForUpdateTheExceptionLog] = logObjectID;
            exceptionBo.Update();
        }
        //确认调整意见
        if (strActivityCode == "Activity87")
        {
            //更新发起异常创建的日志记录，异常类型，异常描述进行同步更新
            ExceptionLog.UpdateLog(Drill.ID, Drill.CurrentWorkStep, Drill.ExceptionCategory,
                Drill.ExceptionDescription, this.Request.BizObject, exceptionBo[Drill.ObjectIDForUpdateTheExceptionLog] + string.Empty, this.Engine);
        }
        //审批确认
        if (strActivityCode == "Activity88")
        {
            //清空异常信息
            //发起异常赋值
            exceptionBo[Drill.InitiateException] = "否";
            //异常描述赋值
            exceptionBo[Drill.ExceptionDescription] = "误流入本节点，修正本工序操作错误";
            //异常类型赋值
            exceptionBo[Drill.ExceptionCategory] = "安全异常";
            //异常代表
            exceptionBo[Drill.ExceptionRepresentative] = string.Empty;
            exceptionBo.Update();
        }
    }
}

