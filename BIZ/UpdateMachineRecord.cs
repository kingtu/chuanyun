
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using H3;
//存在大量控件编码的使用，需要将其替换为变量。
class UpdateMacineRecord
{
    
    public void UpdateMacineRecordTable(H3.IEngine engine, string processName, string objectID, string processPlanObjectid, H3.DataModel.BizObject automaticCal)
    {
        if (processName == "取样子流程")
        {
            UpdateMacineRecordSamplingSubFlow(engine, objectID, processPlanObjectid, automaticCal);
        }
        if (processName == "粗车")
        {
            UpdateMacineRecordRoughing(engine, objectID, processPlanObjectid, automaticCal);
        }
        if (processName == "精车")
        {
            UpdateMacineRecordFinishing(engine, objectID, processPlanObjectid, automaticCal);
        }
        if (processName == "钻孔")
        {
            UpdateMacineRecordDrill(engine, objectID, processPlanObjectid, automaticCal);
        }
    }
    public void AddTechHoursToRecord(H3.IEngine engine, string processName, string objectID, string processPlanObjectid)
    {
        if (processName == "粗车")
        {
            AddTechHoursToRecordRoughing(engine, objectID, processPlanObjectid);
        }
        if (processName == "精车")
        {
            AddTechHoursToRecordFinishing(engine, objectID, processPlanObjectid);
        }
        if (processName == "钻孔")
        {
            AddTechHoursToRecordDrill(engine, objectID, processPlanObjectid);
        }
    }
    /*
   *--Author:zzx
   * 同步取样机加工任务记录表
   * @param objectID    工序表objectid     ProcessingRecord
   */
    public void UpdateMacineRecordSamplingSubFlow(H3.IEngine engine, string objectID, string processPlanObjectid, H3.DataModel.BizObject automaticCal)
    {
        H3.DataModel.BizObject samplingSubFlow = Tools.BizOperation.Load(engine, SamplingSubFlow_TableCode, objectID);        //取样子流程
        H3.DataModel.BizObject[] samplingMachiningSubtable = samplingSubFlow[SamplingSubFlow_Sampling] as H3.DataModel.BizObject[];        //取样机加工子表   
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectid);        //工序计划表objectID        //ac订单规格表
        //ac订单规格表
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj[OrderSpecificationObjectId] + string.Empty);
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;        //产品参数表objectId
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);        //产品参数表
        //完成总量
        decimal totalAmountCompleted = samplingSubFlow[SamplingSubFlow_TotalAmountCompleted] + string.Empty
            != string.Empty ? decimal.Parse(samplingSubFlow[SamplingSubFlow_TotalAmountCompleted] + string.Empty) : 0;

        int taskNum = 0;
        if (totalAmountCompleted >= 1)        //当前子表行数
        {
            taskNum = samplingMachiningSubtable.Length - 1;
        }
        else
        {
            taskNum = samplingMachiningSubtable.Length - 2;
        }
        if (samplingMachiningSubtable != null)         //取样下机
        {
            H3.DataModel.BizObject currentTask = samplingMachiningSubtable[taskNum];            //当前任务记录
            H3.Organization.User processor = engine.Organization.GetUnit(currentTask["F0000157"] + string.Empty) as H3.Organization.User;            //加工者
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(engine, MachiningTaskRecord_TableCode);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态
            recordObj[MachiningTaskRecord_SectionName] = "毛坯取样";   //工序
            recordObj[MachiningTaskRecord_ProductSpecification] = productParameterObj[ProductParameter_ProductSpecification] + string.Empty;            //产品规格
            recordObj[MachiningTaskRecord_ID] = samplingSubFlow["F0000058"] + string.Empty;            //工件ID
            recordObj[MachiningTaskRecord_WorkPieceNumber] = samplingSubFlow["F0000025"] + string.Empty;            //工件号
            recordObj[MachiningTaskRecord_TaskNumber] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord_Processor] = currentTask["F0000157"] + string.Empty;            //加工者
            recordObj[MachiningTaskRecord_DepartmentName] = processor != null ? processor.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord_StartTime] = currentTask["F0000164"] + string.Empty;            //加工开始时间
            recordObj[MachiningTaskRecord_DeviceName] = currentTask["F0000158"] + string.Empty;            //设备名称
            recordObj[MachiningTaskRecord_DeviceNumber] = currentTask["F0000159"] + string.Empty;                    //设备编号
            recordObj[MachiningTaskRecord_DeviceType] = currentTask["F0000160"] + string.Empty;            //设备类型
            recordObj[MachiningTaskRecord_RollingMode] = samplingSubFlow["F0000039"] + string.Empty;            //轧制方式
            recordObj[MachiningTaskRecord_WorkLoad] = currentTask["F0000162"] + string.Empty;            //任务加工量
            recordObj[MachiningTaskRecord_EndTime] = DateTime.Now; //加工结束时间
            if (productParameterObj != null)
            {
                recordObj[MachiningTaskRecord_ProductName] = productParameterObj[ProductParameter.ProductName] + string.Empty; //产品名称
                //车加工类别
                recordObj[MachiningTaskRecord_MachiningProcessingCategory] = productParameterObj[ProductParameter.ProductMachiningCategory] + string.Empty;
                //钻加工类别
                recordObj[MachiningTaskRecord_DrillingProcessingCategory] = productParameterObj[ProductParameter.ProductDrillingCategory] + string.Empty;
                //订单规格号
                recordObj[MachiningTaskRecord_OrderSpecifications] = productParameterObj[ProductParameter.OrderSpecificationNumber] + string.Empty;
                //成品单重
                recordObj[MachiningTaskRecord_FinishedProductUnitWeight] = productParameterObj[ProductParameter.FinishedProductUnitWeight] + string.Empty;
                //工件外径
                recordObj[MachiningTaskRecord_OutsideDiameter] = productParameterObj[ProductParameter.OuterDiameter] + string.Empty;
                //工件内径
                recordObj[MachiningTaskRecord_InsideDiameter] = productParameterObj[ProductParameter.InnerDiameter] + string.Empty;
                //工件总高
                recordObj[MachiningTaskRecord_TotalHeight] = productParameterObj[ProductParameter.TotalHeight] + string.Empty;
                //工件片厚
                recordObj[MachiningTaskRecord_Thickness] = productParameterObj[ProductParameter.SliceThickness] + string.Empty;
                //工件孔数
                recordObj[MachiningTaskRecord_HoleAmount] = productParameterObj[ProductParameter.NumberOfHoles] + string.Empty;
                //工件孔径
                recordObj[MachiningTaskRecord_Aperture] = productParameterObj[ProductParameter.HoleDiameter] + string.Empty;                 

            }
            DateTime startTime = recordObj[MachiningTaskRecord_StartTime] + string.Empty != string.Empty ?
                Convert.ToDateTime(recordObj[MachiningTaskRecord_StartTime] + string.Empty) : DateTime.Now;
            //与现在时间的差值
            TimeSpan delayTime = DateTime.Now.Subtract(startTime);
            recordObj[MachiningTaskRecord_ActualProcessingTime] = delayTime.TotalHours;            //实际耗时  actual time consuming
            recordObj[MachiningTaskRecord_ApplyAdjust] = currentTask["F0000185"] + string.Empty;            //申请调整加工难度
            recordObj[MachiningTaskRecord_DataCode] = samplingSubFlow["F0000064"] + string.Empty;            //数据代码
            recordObj[MachiningTaskRecord_DepartmentCode] = samplingSubFlow["F0000195"] + string.Empty;            //部门代码
            recordObj[MachiningTaskRecord_VersionNumber] = samplingSubFlow["F0000196"] + string.Empty;            //版本号
            recordObj[MachiningTaskRecord_MultipleDepartments] = samplingSubFlow["F0000194"];            //部门多选
            recordObj.Create();
            //任务加工记录
            currentTask["F0000161"] = recordObj.ObjectId;
            //工资自动计算赋值 当前加工记录
            automaticCal["F0000013"] = recordObj.ObjectId;
            automaticCal.Update();
            currentTask.Update();
        }
    }
    /*
     *--Author:zzx
     * 同步粗车机加工任务记录表
     * @param objectID    工序表objectid
     */
    public void UpdateMacineRecordRoughing(H3.IEngine engine, string objectID, string processPlanObjectid, H3.DataModel.BizObject automaticCal)
    {
        H3.DataModel.BizObject roughingObj = Tools.BizOperation.Load(engine, Roughing_TableCode, objectID);
        //粗车机加表
        H3.DataModel.BizObject[] roughingMachiningObj = roughingObj[Roughing_Machining] as H3.DataModel.BizObject[];
        //工序计划表objectID
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectid);
        //ac订单规格表
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj[OrderSpecificationObjectId] + string.Empty);
        //产品参数表objectId
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;
        //产品参数表
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);
        //完成总量
        decimal totalAmountCompleted = roughingObj["F0000090"] + string.Empty
            != string.Empty ? decimal.Parse(roughingObj["F0000090"] + string.Empty) : 0;

        int taskNum = 0;
        if (totalAmountCompleted >= 1)        //当前子表行数
        {
            taskNum = roughingMachiningObj.Length - 1;
        }
        else
        {
            taskNum = roughingMachiningObj.Length - 2;
        }

        if (roughingMachiningObj != null)         //粗车下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = roughingMachiningObj[taskNum];

            H3.Organization.User processor = engine.Organization.GetUnit(currentTask["F0000157"] + string.Empty) as H3.Organization.User;

            H3.DataModel.BizObject recordObj = null;
            //新建机加工记录数据对象
            recordObj = Tools.BizOperation.New(engine, MachiningTaskRecord_TableCode);

            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态
            recordObj[MachiningTaskRecord_SectionName] = "粗车"; //工序
            if (roughingObj["F0000134"] + string.Empty == "已本取")
            {
                //任务类型
                if ((bool)currentTask["F0000260"])
                {
                    recordObj[MachiningTaskRecord_TaskType] = "本取后粗车四面光";
                }
                else
                {
                    //任务类型
                    recordObj[MachiningTaskRecord_TaskType] = "本取后粗车";
                }
            }
            else
            {
                if ((bool)currentTask["F0000260"])
                {
                    recordObj[MachiningTaskRecord_TaskType] = "正常粗车四面光";//任务类型
                }
                else
                {
                    recordObj[MachiningTaskRecord_TaskType] = "正常粗车";//任务类型
                }
            }
            recordObj[MachiningTaskRecord_ProductSpecification] = productParameterObj[ProductParameter.ProductSpecification] + string.Empty;//产品规格
            recordObj[MachiningTaskRecord_ID] = roughingObj["F0000067"] + string.Empty;//工件ID
            recordObj[MachiningTaskRecord_WorkPieceNumber] = roughingObj["F0000033"] + string.Empty;//工件号
            recordObj[MachiningTaskRecord_TaskNumber] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord.Processor] = currentTask["F0000157"] + string.Empty; //加工者
            recordObj[MachiningTaskRecord_DepartmentName] = processor != null ? processor.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord_StartTime] = currentTask["F0000164"] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord_DeviceName] = currentTask["F0000158"] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord_DeviceNumber] = currentTask["F0000159"] + string.Empty; //设备编号
            recordObj[MachiningTaskRecord_DeviceType] = currentTask["F0000160"] + string.Empty; //设备类型
            recordObj[MachiningTaskRecord_RollingMode] = roughingObj["F0000122"] + string.Empty;//轧制方式
            recordObj[MachiningTaskRecord_WorkLoad] = currentTask["F0000162"] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord_EndTime] = DateTime.Now; //加工结束时间
            if (productParameterObj != null)
            {
                recordObj[MachiningTaskRecord_ProductName] = productParameterObj[ProductParameter_ProductName] + string.Empty; //产品名称
                recordObj[MachiningTaskRecord_MachiningProcessingCategory] = productParameterObj[ProductParameter_ProductMachiningCategory] + string.Empty;    //车加工类别
                recordObj[MachiningTaskRecord_DrillingProcessingCategory] = productParameterObj[ProductParameter_ProductDrillingCategory] + string.Empty;     //钻加工类别
                recordObj[MachiningTaskRecord_OrderSpecifications] = productParameterObj[ProductParameter_OrderSpecificationNumber] + string.Empty;    //订单规格号
                recordObj[MachiningTaskRecord_FinishedProductUnitWeight] = productParameterObj[ProductParameter_FinishedProductUnitWeight] + string.Empty;    //成品单重
                recordObj[MachiningTaskRecord_OutsideDiameter] = productParameterObj[ProductParameter_OuterDiameter] + string.Empty;    //工件外径
                recordObj[MachiningTaskRecord_InsideDiameter] = productParameterObj[ProductParameter_InnerDiameter] + string.Empty;    //工件内径
                recordObj[MachiningTaskRecord_TotalHeight] = productParameterObj[ProductParameter_TotalHeight] + string.Empty;    //工件总高
                recordObj[MachiningTaskRecord_Thickness] = productParameterObj[ProductParameter_SliceThickness] + string.Empty;    //工件片厚
                recordObj[MachiningTaskRecord_HoleAmount] = productParameterObj[ProductParameter_NumberOfHoles] + string.Empty;    //工件孔数
                recordObj[MachiningTaskRecord_Aperture] = productParameterObj[ProductParameter_HoleDiameter] + string.Empty;    //工件孔径
            }
            DateTime startTime = recordObj[MachiningTaskRecord_StartTime] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj[MachiningTaskRecord_StartTime] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime); //与现在时间的差值
            recordObj[MachiningTaskRecord.ActualElapsedTime] = delayTime.TotalHours; //实际耗时
            recordObj[MachiningTaskRecord.ApplyAdjust] = currentTask["F0000181"] + string.Empty; //申请调整加工难度
            recordObj[MachiningTaskRecord_DataCode] = roughingObj["F0000076"] + string.Empty;//数据代码
            recordObj[MachiningTaskRecord_DepartmentCode] = roughingObj["F0000205"] + string.Empty;//部门代码
            recordObj[MachiningTaskRecord_VersionNumber] = roughingObj["F0000203"] + string.Empty;//版本号
            recordObj[MachiningTaskRecord_MultipleDepartments] = roughingObj["F0000204"];//部门多选
            recordObj.Create();
            //任务加工记录
            currentTask["F0000161"] = recordObj.ObjectId;
            //工资自动计算赋值 当前加工记录
            automaticCal["F0000013"] = recordObj.ObjectId;
            automaticCal.Update();
            currentTask.Update();

        }
    }
    /*
    *--Author:zzx
    * 同步精车机加工任务记录表
    * @param objectID    工序表objectid
    */
    public void UpdateMacineRecordFinishing(H3.IEngine engine, string objectID, string processPlanObjectid, H3.DataModel.BizObject automaticCal)
    {
        H3.DataModel.BizObject finishingObj = Tools.BizOperation.Load(engine, Finishing_TableCode, objectID);
        H3.DataModel.BizObject[] finishingMachiningObj = finishingObj[Finishing_Machining] as H3.DataModel.BizObject[];    //粗车机加表
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectid);    //工序计划表objectID
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj["F0000145"] + string.Empty);    //ac订单规格表
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;    //产品参数表objectId
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);    //产品参数表
                                                                                                                                         //完成总量
        decimal totalAmountCompleted = finishingObj["F0000086"] + string.Empty
            != string.Empty ? decimal.Parse(finishingObj["F0000086"] + string.Empty) : 0;
        int taskNum = 0;
        if (totalAmountCompleted >= 1)    //当前子表行数
        {
            taskNum = finishingMachiningObj.Length - 1;
        }
        else
        {
            taskNum = finishingMachiningObj.Length - 2;
        }
        if (finishingMachiningObj != null)     //粗车下机
        {
            //当前任务记录
            H3.DataModel.BizObject currentTask = finishingMachiningObj[taskNum];

            H3.Organization.User processor = engine.Organization.GetUnit(currentTask["F0000143"] + string.Empty) as H3.Organization.User;
            //新建机加工记录数据对象
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(engine, MachiningTaskRecord_TableCode);
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective; //设置为生效状态
            recordObj[MachiningTaskRecord_SectionName] = "精车"; //工序
            recordObj[MachiningTaskRecord_TaskType] = "正常精车";        //任务类型
            recordObj[MachiningTaskRecord_ProductSpecification] = productParameterObj[ProductParameter_ProductSpecification] + string.Empty;        //产品规格
            recordObj[MachiningTaskRecord_ID] = finishingObj["F0000053"] + string.Empty;        //工件ID
            recordObj[MachiningTaskRecord_WorkPieceNumber] = finishingObj["F0000001"] + string.Empty;        //工件号
            recordObj[MachiningTaskRecord_TaskNumber] = taskNum;        //任务计数器
            recordObj[MachiningTaskRecord_Processor] = currentTask["F0000143"] + string.Empty;        //加工者
            recordObj[MachiningTaskRecord_DepartmentName] = processor != null ? processor.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord_StartTime] = currentTask["F0000142"] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord_DeviceName] = currentTask["F0000137"] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord_DeviceNumber] = currentTask["F0000138"] + string.Empty;        //设备编号
            recordObj[MachiningTaskRecord_DeviceType] = currentTask["F0000139"] + string.Empty;        //设备类型
            recordObj[MachiningTaskRecord_WorkLoad] = currentTask["F0000140"] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord_EndTime] = DateTime.Now; //加工结束时间
            if (productParameterObj != null)
            {
                recordObj[MachiningTaskRecord_ProductName] = productParameterObj[ProductParameter_ProductName] + string.Empty;            //产品名称
                recordObj[MachiningTaskRecord_MachiningProcessingCategory] = productParameterObj[ProductParameter_ProductMachiningCategory] + string.Empty;            //车加工类别
                recordObj[MachiningTaskRecord_DrillingProcessingCategory] = productParameterObj[ProductParameter_ProductDrillingCategory] + string.Empty;            //钻加工类别
                recordObj[MachiningTaskRecord_OrderSpecifications] = productParameterObj[ProductParameter_OrderSpecificationNumber] + string.Empty;            //订单规格编号
                recordObj[MachiningTaskRecord_FinishedProductUnitWeight] = productParameterObj[ProductParameter_FinishedProductUnitWeight] + string.Empty;            //成品单重
                recordObj[MachiningTaskRecord_OutsideDiameter] = productParameterObj[ProductParameter_OuterDiameter] + string.Empty;            //工件外径
                recordObj[MachiningTaskRecord_InsideDiameter] = productParameterObj[ProductParameter_InnerDiameter] + string.Empty;            //工件内径
                recordObj[MachiningTaskRecord_TotalHeight] = productParameterObj[ProductParameter_TotalHeight] + string.Empty;            //工件总高
                recordObj[MachiningTaskRecord_Thickness] = productParameterObj[ProductParameter_SliceThickness] + string.Empty;            //工件片厚
                recordObj[MachiningTaskRecord_HoleAmount] = productParameterObj[ProductParameter_NumberOfHoles] + string.Empty;            //工件孔数
                recordObj[MachiningTaskRecord_Aperture] = productParameterObj[ProductParameter_HoleDiameter] + string.Empty;            //工件孔径
            }
            DateTime startTime = recordObj[MachiningTaskRecord.StartTime] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj[MachiningTaskRecord.StartTime] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime); //与现在时间的差值
            recordObj[MachiningTaskRecord.ActualElapsedTime] = delayTime.TotalHours; //实际耗时
            recordObj[MachiningTaskRecord_DataCode] = finishingObj["F0000062"] + string.Empty;        //数据代码
            recordObj[MachiningTaskRecord_DepartmentCode] = finishingObj["F0000192"] + string.Empty;        //部门代码
            recordObj[MachiningTaskRecord_VersionNumber] = finishingObj["F0000190"] + string.Empty;        //版本号
            recordObj[MachiningTaskRecord_MultipleDepartments] = finishingObj["F0000191"];        //部门多选
            recordObj.Create();
            currentTask["F0000144"] = recordObj.ObjectId;        //当前任务加工记录
            automaticCal["F0000013"] = recordObj.ObjectId;        //工资自动计算赋值 当前加工记录
            automaticCal.Update();
            currentTask.Update();
        }
    }
    /*
    *--Author:zzx
    * 同步钻孔机加工任务记录表
    * @param objectID    工序表objectid
    */
    public void UpdateMacineRecordDrill(H3.IEngine engine, string objectID, string processPlanObjectid, H3.DataModel.BizObject automaticCal)
    {
        H3.DataModel.BizObject drillObj = Tools.BizOperation.Load(engine, Drill_TableCode, objectID);
        H3.DataModel.BizObject[] drillMachiningObj = drillObj[Drill_Machining] as H3.DataModel.BizObject[];    //钻孔车机加表
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectid);    //工序计划表objectID
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj[OrderSpecificationObjectId] + string.Empty);    //ac订单规格表
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;    //产品参数表objectId
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);    //产品参数表
        decimal totalAmountCompleted = drillObj["F0000073"] + string.Empty    //完成总量
            != string.Empty ? decimal.Parse(drillObj["F0000073"] + string.Empty) : 0;
        int taskNum = 0;
        if (totalAmountCompleted >= 1)    //当前子表行数
        {
            taskNum = drillMachiningObj.Length - 1;
        }
        else
        {
            taskNum = drillMachiningObj.Length - 2;
        }
        if (drillMachiningObj != null)     //   钻孔下机
        {
            H3.DataModel.BizObject currentTask = drillMachiningObj[taskNum];        //当前任务记录
            H3.Organization.User processor = engine.Organization.GetUnit(currentTask["F0000143"] + string.Empty) as H3.Organization.User;
            H3.DataModel.BizObject recordObj = Tools.BizOperation.New(engine, MachiningTaskRecord_TableCode);        //新建机加工记录数据对象
            recordObj.Status = H3.DataModel.BizObjectStatus.Effective;        //设置为生效状态
            recordObj[MachiningTaskRecord_SectionName] = "钻孔";        //工序
            recordObj[MachiningTaskRecord_TaskType] = "正常钻孔"; //任务类型
            recordObj[MachiningTaskRecord_ProductSpecification] = productParameterObj[ProductParameter.ProductSpecification] + string.Empty;        //产品规格
            recordObj[MachiningTaskRecord_ID] = drillObj[Drill.ID] + string.Empty; //工件ID          
            recordObj[MachiningTaskRecord_WorkPieceNumber] = drillObj[Drill.WorkpieceNumber] + string.Empty;  //工件号
            recordObj[MachiningTaskRecord_TaskNumber] = taskNum; //任务计数器
            recordObj[MachiningTaskRecord_Processor] = currentTask["F0000143"] + string.Empty; //加工者
            recordObj[MachiningTaskRecord_DepartmentName] = processor != null ? processor.DepartmentName : ""; //部门名称
            recordObj[MachiningTaskRecord_StartTime] = currentTask["F0000142"] + string.Empty; //加工开始时间
            recordObj[MachiningTaskRecord_DeviceName] = currentTask["F0000137"] + string.Empty; //设备名称
            recordObj[MachiningTaskRecord_DeviceNumber] = currentTask["F0000138"] + string.Empty; //设备编号
            recordObj[MachiningTaskRecord_DeviceType] = currentTask["F0000139"] + string.Empty; //设备类型
            recordObj[MachiningTaskRecord_WorkLoad] = currentTask["F0000140"] + string.Empty; //任务加工量
            recordObj[MachiningTaskRecord_EndTime] = DateTime.Now; //加工结束时间
            if (productParameterObj != null)
            {
                recordObj[MachiningTaskRecord_ProductName] = productParameterObj[ProductParameter_ProductName] + string.Empty;            //产品名称
                recordObj[MachiningTaskRecord_MachiningProcessingCategory] = productParameterObj[ProductParameter_ProductMachiningCategory] + string.Empty;            //产品类别
                recordObj[MachiningTaskRecord_DrillingProcessingCategory] = productParameterObj[ProductParameter_ProductDrillingCategory] + string.Empty;            //产品小类
                recordObj[MachiningTaskRecord_OrderSpecifications] = productParameterObj[ProductParameter_OrderSpecificationNumber] + string.Empty;            //产品编号
                recordObj[MachiningTaskRecord_FinishedProductUnitWeight] = productParameterObj[ProductParameter_FinishedProductUnitWeight] + string.Empty;            //成品单重
                recordObj[MachiningTaskRecord_OutsideDiameter] = productParameterObj[ProductParameter_OuterDiameter] + string.Empty;            //工件外径
                recordObj[MachiningTaskRecord_InsideDiameter] = productParameterObj[ProductParameter_InnerDiameter] + string.Empty;            //工件内径
                recordObj[MachiningTaskRecord_TotalHeight] = productParameterObj[ProductParameter_TotalHeight] + string.Empty;            //工件总高
                recordObj[MachiningTaskRecord_Thickness] = productParameterObj[ProductParameter_SliceThickness] + string.Empty;            //工件片厚 
                recordObj[MachiningTaskRecord_HoleAmount] = productParameterObj[ProductParameter_NumberOfHoles] + string.Empty;            //工件孔数
                recordObj[MachiningTaskRecord_Aperture] = productParameterObj[ProductParameter_HoleDiameter] + string.Empty;            //工件孔径
            }
            //加工开始时间
            DateTime startTime = recordObj[MachiningTaskRecord_StartTime] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj[MachiningTaskRecord_StartTime] + string.Empty) : DateTime.Now;
            TimeSpan delayTime = DateTime.Now.Subtract(startTime);        //与现在时间的差值
            recordObj[MachiningTaskRecord_ActualProcessingTime] = delayTime.TotalHours;        //实际耗时
            recordObj[MachiningTaskRecord_ApplyAdjust] = currentTask["F0000168"] + string.Empty;        //申请调整加工难度
            recordObj[MachiningTaskRecord_DataCode] = drillObj["F0000048"] + string.Empty;        //数据代码
            recordObj[MachiningTaskRecord_DepartmentCode] = drillObj["F0000186"] + string.Empty;    //部门代码                                                                                                    
            recordObj[MachiningTaskRecord_VersionNumber] = drillObj["F0000184"] + string.Empty;//版本号
            //部门多选
            recordObj[MachiningTaskRecord_MultipleDepartments] = drillObj["F0000185"];
            recordObj.Create();
            //当前任务加工记录
            currentTask["F0000144"] = recordObj.ObjectId;
            //工资自动计算赋值 当前加工记录
            automaticCal["F0000013"] = recordObj.ObjectId;
            automaticCal.Update();
            currentTask.Update();
        }
    }


    //粗车工序  工序记录表中添加工时和下屑量
    /*
    * @param engine 引擎
    * @param OperationName 工序名
    * @param productParameter 产品参数表业务对象
    * @param rollingMode 轧制方式
    * @param equipmentCoeff  设备工时系数
    * @param quantity  派工/加工量
    **/
    public void AddTechHoursToRecordRoughing(H3.IEngine engine, string processObjectID, string processPlanObjectID, string plannedRollingMode = "单轧")
    {
        H3.DataModel.BizObject roughingObj = Tools.BizOperation.Load(engine, Roughing_TableCode, processObjectID);
        //粗车机加表
        H3.DataModel.BizObject[] roughingMachiningObj = roughingObj[Roughing_Machining] as H3.DataModel.BizObject[];
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectID);
        //ac订单规格表
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj[OrderSpecificationObjectId] + string.Empty);
        //产品参数表objectId
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);    //产品参数表
                                                                                                                                         //完成总量
        decimal totalAmountCompleted = roughingObj["F0000090"] + string.Empty
            != string.Empty ? decimal.Parse(roughingObj["F0000090"] + string.Empty) : 0;
        int taskNum = 0;
        //当前子表行数
        if (totalAmountCompleted >= 1)
        {
            taskNum = roughingMachiningObj.Length - 1;
        }
        else
        {
            taskNum = roughingMachiningObj.Length - 2;
        }
        //当前任务记录
        H3.DataModel.BizObject currentTask = roughingMachiningObj[taskNum];
        H3.DataModel.BizObject recordObj = Tools.BizOperation.Load(engine, MachiningTaskRecord.TableCode, currentTask["F0000161"] + string.Empty);    //加工记录
        string deviceType = currentTask["F0000160"] + string.Empty;    //设备类型  
        double processQuantity = Convert.ToDouble(currentTask["F0000162"]);    //加工量
        TechHours th = new TechHours();
        double equipmentCoeff = th.EquipmentTechHoursCoeff(engine, "粗车", deviceType, productParameterObj, plannedRollingMode);    //设备工时系数
        Dictionary<string, double> hoursAndChip = th.QueryTechHoursAndFilings(engine, "粗车", productParameterObj, plannedRollingMode, equipmentCoeff, processQuantity);
        recordObj[MachiningTaskRecord_DeviceCoefficient] = equipmentCoeff;    //设备工时系数
        recordObj[MachiningTaskRecord_NoDeviceTechHours] = hoursAndChip["NoDeviceTechHours"];    //无设备工时
        recordObj[MachiningTaskRecord_TechHours] = hoursAndChip["techHours"];    //粗车工时
        recordObj[MachiningTaskRecord_StageTechHours] = hoursAndChip["StageTechHours"]; //阶段加工任务工时
        recordObj[MachiningTaskRecord_FillingWeight] = hoursAndChip["FillingWeight"];    //任务下屑量
        recordObj.Update();
    }
    //精车工序工序记录表中添加工时和下屑量
    /*
    * @param engine 引擎
    * @param OperationName 工序名
    * @param productParameter 产品参数表业务对象
    * @param rollingMode 轧制方式
    * @param equipmentCoeff  设备工时系数
    * @param quantity  派工/加工量
    **/
    public void AddTechHoursToRecordFinishing(H3.IEngine engine, string processObjectID, string processPlanObjectID, string plannedRollingMode = "单轧")
    {
        H3.DataModel.BizObject finishingObj = Tools.BizOperation.Load(engine, Finishing_TableCode, processObjectID);
        H3.DataModel.BizObject[] finishingMachiningObj = finishingObj[Finishing_Machining] as H3.DataModel.BizObject[];    //粗车机加表
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectID);
        //ac订单规格表
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj[OrderSpecificationObjectId] + string.Empty);
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;    //产品参数表objectId
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);    //产品参数表
                                                                                                                                         //完成总量
        decimal totalAmountCompleted = finishingObj["F0000086"] + string.Empty
            != string.Empty ? decimal.Parse(finishingObj["F0000086"] + string.Empty) : 0;
        int taskNum = 0;
        //当前子表行数
        if (totalAmountCompleted >= 1)
        {
            taskNum = finishingMachiningObj.Length - 1;
        }
        else
        {
            taskNum = finishingMachiningObj.Length - 2;
        }
        H3.DataModel.BizObject currentTask = finishingMachiningObj[taskNum];    //当前任务记录
        H3.DataModel.BizObject recordObj = Tools.BizOperation.Load(engine, MachiningTaskRecord.TableCode, currentTask["F0000144"] + string.Empty);    //加工记录
        string deviceType = currentTask["F0000139"] + string.Empty;    //设备类型 
        double processQuantity = Convert.ToDouble(currentTask["F0000140"]);    //加工量
        TechHours th = new TechHours();
        double equipmentCoeff = th.EquipmentTechHoursCoeff(engine, "粗车", deviceType, productParameterObj, plannedRollingMode);    //设备工时系数
        Dictionary<string, double> hoursAndChip = th.QueryTechHoursAndFilings(engine, "粗车", productParameterObj, plannedRollingMode, equipmentCoeff, processQuantity);
        recordObj[MachiningTaskRecord_DeviceCoefficient] = equipmentCoeff;    //设备工时系数
        recordObj[MachiningTaskRecord_NoDeviceTechHours] = hoursAndChip["NoDeviceTechHours"];    //无设备工时
        recordObj[MachiningTaskRecord_TechHours] = hoursAndChip["techHours"];    //粗车工时
        recordObj[MachiningTaskRecord_StageTechHours] = hoursAndChip["StageTechHours"];    //阶段加工任务工时
        recordObj[MachiningTaskRecord_FillingWeight] = hoursAndChip["FillingWeight"];    //任务下屑量
        recordObj.Update();
    }
    //钻孔工序工序记录表中添加工时和下屑量
    /*
    * @param engine 引擎
    * @param OperationName 工序名
    * @param productParameter 产品参数表业务对象
    * @param rollingMode 轧制方式
    * @param equipmentCoeff  设备工时系数
    * @param quantity  派工/加工量
    **/
    public void AddTechHoursToRecordDrill(H3.IEngine engine, string processObjectID, string processPlanObjectID, string plannedRollingMode = "单轧")
    {
        H3.DataModel.BizObject drillObj = Tools.BizOperation.Load(engine, Drill_TableCode, processObjectID);
        H3.DataModel.BizObject[] drillMachiningObj = drillObj[Drill_Machining] as H3.DataModel.BizObject[];    //粗车机加表
                                                                                                               //工序计划表objectID
        H3.DataModel.BizObject processPlanObj = Tools.BizOperation.Load(engine, ABCDProcessPlan_TableCode, processPlanObjectID);
        //ac订单规格表
        H3.DataModel.BizObject orderSpecificationObj = Tools.BizOperation.Load(engine, OrderSpecification_TableCode, processPlanObj[OrderSpecificationObjectId] + string.Empty);
        string productParameterId = orderSpecificationObj[ParameterObjectId] + string.Empty;    //产品参数表objectId
        H3.DataModel.BizObject productParameterObj = Tools.BizOperation.Load(engine, ProductParameter_TableCode, productParameterId);    //产品参数表
                                                                                                                                         //完成总量
        decimal totalAmountCompleted = drillObj["F0000073"] + string.Empty
            != string.Empty ? decimal.Parse(drillObj["F0000073"] + string.Empty) : 0;
        int taskNum = 0;
        //当前子表行数
        if (totalAmountCompleted >= 1)
        {
            taskNum = drillMachiningObj.Length - 1;
        }
        else
        {
            taskNum = drillMachiningObj.Length - 2;
        }
        H3.DataModel.BizObject currentTask = drillMachiningObj[taskNum];    //当前任务记录
        H3.DataModel.BizObject recordObj = Tools.BizOperation.Load(engine, MachiningTaskRecord_TableCode, currentTask["F0000144"] + string.Empty);
        string deviceType = currentTask["F0000139"] + string.Empty;    //设备类型 
        double processQuantity = Convert.ToDouble(currentTask["F0000140"]);    //加工量
        TechHours th = new TechHours();
        double equipmentCoeff = th.EquipmentTechHoursCoeff(engine, "粗车", deviceType, productParameterObj, plannedRollingMode);    //设备工时系数
        Dictionary<string, double> hoursAndChip = th.QueryTechHoursAndFilings(engine, "粗车", productParameterObj, plannedRollingMode, equipmentCoeff, processQuantity);
        recordObj[MachiningTaskRecord_DeviceCoefficient] = equipmentCoeff;    //设备工时系数
        recordObj[MachiningTaskRecord_NoDeviceTechHours] = hoursAndChip["NoDeviceTechHours"];    //无设备工时
        recordObj[MachiningTaskRecord_TechHours] = hoursAndChip["techHours"];    //粗车工时
        recordObj[MachiningTaskRecord_StageTechHours] = hoursAndChip["StageTechHours"];    //阶段加工任务工时
        recordObj[MachiningTaskRecord_FillingWeight] = hoursAndChip["FillingWeight"];    //任务下屑量
        recordObj.Update();
    }

    string SamplingSubFlow_TableCode = "D001419Sgljz62e1rneytbqjckbe1vu25";//取样子流程
    string SamplingSubFlow_Sampling = "D001419Fj7nrmbgha1j10v5zst0zg7hi1";//取样子流程 子表
    string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";//工序计划表ABCDProcessPlan
    string OrderSpecificationObjectId = "F0000145";//工序计划表中订单规格表的objectId
    string OrderSpecification_TableCode = "D001419Skniz33124ryujrhb4hry7md21";//ac订单规格表
    string ParameterObjectId = "F0000142";//产品参数表objectId
    string SamplingSubFlow_TotalAmountCompleted = "F0000104";//完成总量    
    //粗车
    string Roughing_TableCode = "D001419Szzswrfsp91x3heen4dykgwus0";   
    string Roughing_Machining = "D001419F8cbba24c57a74ad99bd809ab8e262996"; //粗车机加工
    //精车
    string Finishing_TableCode = "D001419Sqy2b1uy8h8cahh17u9kn0jk10";   
    string Finishing_Machining = "D001419Fd25eb8064b424ed9855ced1923841f1c"; //精车机加工
    //钻孔
    string Drill_TableCode = "D001419Sugyf7m5q744eyhe45o26haop4";
    //钻孔机加工
    string Drill_Machining = "D001419F790f3a6b004e4988abe9511380792293";
    //加工任务记录   ActualProcessingTime
    string MachiningTaskRecord_TableCode = "D0014194963919529e44d60be759656d4a16b63";   
    string MachiningTaskRecord_SectionName = "F0000001"; //工序名称   
    string MachiningTaskRecord_ProductSpecification = "F0000003"; //产品规格    
    string MachiningTaskRecord_Processor = "F0000011";//加工者   
    string MachiningTaskRecord_ID = "ID"; //ID  
    string MachiningTaskRecord_WorkPieceNumber = "F0000040";  //工件号   
    string MachiningTaskRecord_TaskNumber = "F0000002"; //任务计数器   
    string MachiningTaskRecord_TaskType = "F0000031"; //任务类别   
    string MachiningTaskRecord_DepartmentName = "F0000030"; //部门名称   
    string MachiningTaskRecord_StartTime = "StartTime"; //开始时间    
    string MachiningTaskRecord_DeviceName = "F0000007";//设备名称    
    string MachiningTaskRecord_DeviceNumber = "F0000014";//设备编号    
    string MachiningTaskRecord_DeviceType = "F0000041";//设备类型   
    string MachiningTaskRecord_RollingMode = "F0000024"; //轧制方式    
    string MachiningTaskRecord_WorkLoad = "F0000010";//任务加工量  
    string MachiningTaskRecord_EndTime = "EndTime";  //加工结束时间   
    string MachiningTaskRecord_ProductName = "F0000026"; //产品名称   
    string MachiningTaskRecord_MachiningProcessingCategory = "F0000027"; //车加工类别   
    string MachiningTaskRecord_DrillingProcessingCategory = "F0000025"; //钻加工类别    
    string MachiningTaskRecord_OrderSpecifications = "ProductNum";//订单规格号    
    string MachiningTaskRecord_FinishedProductUnitWeight = "F0000017";//成品单重    
    string MachiningTaskRecord_OutsideDiameter = "F0000016";//工件外径    
    string MachiningTaskRecord_InsideDiameter = "F0000018";//工件内径    
    string MachiningTaskRecord_TotalHeight = "F0000020";//工件总高    
    string MachiningTaskRecord_Thickness = "F0000019";//工件片厚    
    string MachiningTaskRecord_HoleAmount = "F0000021";//工件孔数    
    string MachiningTaskRecord_Aperture = "F0000022";//工件孔径 
    string MachiningTaskRecord_DataCode = "F0000058";//数据代码  
    string MachiningTaskRecord_DepartmentCode = "F0000055";  //部门代码    
    string MachiningTaskRecord_VersionNumber = "F0000056";//版本号    
    string MachiningTaskRecord_MultipleDepartments = "F0000057";//部门多选   
    string MachiningTaskRecord_ActualProcessingTime = "F0000013"; //实际加工耗时   
    string MachiningTaskRecord_ApplyAdjust = "F0000044"; //申请加工难度  
    string MachiningTaskRecord_DeviceCoefficient = "F0000008";  //设备系数    
    string MachiningTaskRecord_NoDeviceTechHours = "F0000004";//无设备工时   
    string MachiningTaskRecord_TechHours = "F0000005"; //工时  
    string MachiningTaskRecord_StageTechHours = "F0000006";  //阶段加工任务工时    
    string MachiningTaskRecord_FillingWeight = "F0000023";//任务下屑量
    //产品参数表
    string ProductParameter_TableCode = "D0014196b62f7decd924e1e8713025dc6a39aa5";   
    string ProductParameter_ProductName = "F0000067"; //产品名称    
    string ProductParameter_ProductMachiningCategory = "F0000004";//产品车加工类别   
    string ProductParameter_ProductSpecification = "F0000003"; //产品规格   
    string ProductParameter_ProductDrillingCategory = "F0000006"; //产品钻加工类别
    string ProductParameter_OrderSpecificationNumber = "F0000073";//订单规格编号   
    string ProductParameter_FinishedProductUnitWeight = "F0000014"; //成品单重   
    string ProductParameter_OuterDiameter = "F0000076"; //工件外径   
    string ProductParameter_InnerDiameter = "F0000077"; //工件内径    
    string ProductParameter_TotalHeight = "F0000078";//工件总高    
    string ProductParameter_SliceThickness = "F0000079";//工件片厚    
    string ProductParameter_NumberOfHoles = "F0000080";//工件孔数    
    string ProductParameter_HoleDiameter = "F0000081";//工件孔径
}
