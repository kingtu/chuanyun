using H3.Data.Filter;
using H3.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public class Salary
{
    H3.IEngine Engine;  
    string ID;
    string ProcessName;

    /// <summary>
    /// 填写任务绩效表
    /// </summary>
    /// <param name="Engine"></param>
    /// <param name="ID">产品ID</param>
    public Salary(H3.IEngine Engine, string ID)
    {
        this.Engine = Engine;
        this.ID = ID;
    }

    /// <summary>
    /// 根据加工任务记录表计算工资并保存
    /// </summary>
    /// <param name="sectionName">工序名称</param>
    /// <param name="isSpecTask">是否特殊工序</param>
    public void Save(string sectionName, string macineRecordID, bool isSpecTask = false)
    {
        try
        {
            //取机加记录表的objectID
            // string macineRecordID = automaticCal["F0000013"] + string.Empty;
            //机加工任务记录表objectID
            H3.DataModel.BizObject macineRecordObj = Tools.BizOperation.Load(Engine, MachiningTaskRecordTableCode, macineRecordID);
            this.ProcessName = sectionName;

            //取工资绩效表的objectid
            string salaryComputation = macineRecordObj[MachiningTaskRecord_SalaryPerformance] + string.Empty;
            H3.DataModel.BizObject salaryComObj = null;
         
            if (string.IsNullOrEmpty(salaryComputation))
            {
                salaryComObj = Tools.BizOperation.New(Engine, TaskPerformanceTableCode);
            }
            else
            {
                salaryComObj = Tools.BizOperation.Load(Engine, TaskPerformanceTableCode, salaryComputation);
            }

            var taskName = (string)macineRecordObj[MachiningTaskRecord_TaskName];            //"任务名称"
            var taskType = macineRecordObj[MachiningTaskRecord_TaskType];                     //"任务类别"
            salaryComObj[TaskPerformance_Section] = sectionName; //"工序名称"
            salaryComObj[TaskPerformance_TaskName] = taskName;//"任务名称"
            salaryComObj[TaskPerformance_TaskType] = taskType;//"任务类别"
            salaryComObj[TaskPerformance_ID] = ID;//"ID"
            ///"订单规格号"
            salaryComObj[TaskPerformance_OrderSpecificationNumber] = macineRecordObj[MachiningTaskRecord_OrderSpecifications];
            salaryComObj[TaskPerformance_WorkPieceNumber] = macineRecordObj[MachiningTaskRecord_WorkPieceNumber]; //"工件号"
            salaryComObj[TaskPerformance_Processor] = macineRecordObj[MachiningTaskRecord_Processor];//加工人员
            salaryComObj[TaskPerformance_DepartmentName] = macineRecordObj[MachiningTaskRecord_DepartmentName];//"部门名称"
            salaryComObj[TaskPerformance_DeviceName] = macineRecordObj[MachiningTaskRecord_DeviceName];//"设备名称"
            salaryComObj[TaskPerformance_DeviceType] = macineRecordObj[MachiningTaskRecord_DeviceType];//"设备类型"
            var techHours = Convert.ToDouble(macineRecordObj[MachiningTaskRecord_TechHours]);            //工时
            var taskStageTechHours = Convert.ToDouble(macineRecordObj[MachiningTaskRecord_StageTechHours]);            //报工阶段工时
            var workLoad = Convert.ToDouble(macineRecordObj[MachiningTaskRecord_WorkLoad]);            //加工量

            //工艺下屑重量
            var FillingWeight = Convert.ToDouble(macineRecordObj[MachiningTaskRecord_FillingWeight]);
            macineRecordObj[MachiningTaskRecord_ProcessDifficulty] = macineRecordObj[MachiningTaskRecord_ProcessDifficulty] + string.Empty == string.Empty
                //为加工难度赋予默认值 -- fubin
                ? "1" : macineRecordObj[MachiningTaskRecord_ProcessDifficulty] + string.Empty;
            double processDifficulty = Convert.ToDouble(macineRecordObj[MachiningTaskRecord_ProcessDifficulty]);//加工难度
            salaryComObj[TaskPerformance_TechHours] = techHours;            //工时
            salaryComObj[TaskPerformance_StageTechHours] = taskStageTechHours;            //报工阶段工时
            salaryComObj[TaskPerformance_DataCode] = macineRecordObj[MachiningTaskRecord_DataCode] + string.Empty;            //数据代码
            salaryComObj[TaskPerformance_DepartmentCode] = macineRecordObj[MachiningTaskRecord_DepartmentCode] + string.Empty;            //部门代码
            salaryComObj[TaskPerformance_VersionNumber] = macineRecordObj[MachiningTaskRecord_VersionNumber] + string.Empty;            //版本号
            salaryComObj[TaskPerformance_MultipleDepartments] = macineRecordObj[MachiningTaskRecord_MultipleDepartments];            //部门多选
            salaryComObj[TaskPerformance_Workload] = workLoad;            //加工量
            salaryComObj[TaskPerformance_FillingWeight] = FillingWeight;            //"工艺下屑重量"


            if (sectionName == "粗车")
            {
                UpdateSalaryCC(salaryComObj, macineRecordObj, techHours, workLoad, FillingWeight, processDifficulty);
            }
            else if (sectionName == "四面光")
            {
                UpdateSalarySMG(salaryComObj, macineRecordObj, techHours, workLoad, FillingWeight, processDifficulty);
            }
            else if (sectionName == "精车")
            {
                UpdateSalaryJC(salaryComObj, macineRecordObj, techHours, workLoad, FillingWeight, processDifficulty);
            }
            else if (sectionName == "钻孔")
            {
                UpdateSalaryZK(salaryComObj, macineRecordObj, techHours, workLoad, FillingWeight, processDifficulty);
            }

            if (string.IsNullOrEmpty(salaryComputation))
            {
                salaryComObj.Status = H3.DataModel.BizObjectStatus.Effective;//设置为生效状态
                salaryComObj.Create();
                //机加记录表 中赋值  工资绩效表的objectid
                macineRecordObj["F0000059"] = salaryComObj.ObjectId;  
                macineRecordObj.Update();
            }
            else
            {
                salaryComObj.Update();
            }
        }
        catch (Exception e)
        {
            Log(sectionName, ID, e.Message);
        }
    }
    /// <summary>
    /// 更新四面光工资
    /// </summary>
    /// <param name="salaryComObj">工资绩效 记录</param>
    /// <param name="macineRecordObj">加工任务记录的一条记录</param>
    /// <param name="techHour">工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalarySMG(H3.DataModel.BizObject salaryComObj, H3.DataModel.BizObject macineRecordObj, double techHours, double quantity, double dustWeight, double processingDifficulty = 1)
    {
        //string sectionName = "四面光";//四面见光
        var workPrice = 28;//"工价"   
        //"工时工资"
        var techHoursSalary = techHours * quantity * workPrice * processingDifficulty;
        var shareRate = 0.2;        //共享率
        var totalFillingWeight = dustWeight * quantity * shareRate;        //"总下屑量"
        //"外径"
        var outsideDiameter = Convert.ToDouble(macineRecordObj[MachiningTaskRecord.OutsideDiameter]);    
        //补助标准
        var subsidy = outsideDiameter >= 0 && outsideDiameter < 4000 ? 18 : (outsideDiameter >= 4000 && outsideDiameter < 5000 ? 23 : (outsideDiameter >= 5000 && outsideDiameter < 6000 ? 30 : 30));
        var ToolReplenishmentAmount = subsidy * totalFillingWeight / 1000;        //"补刀金额"
        var totalWorkload = 0;        //"总工作量"

        salaryComObj[TaskPerformance_WorkPrice] = workPrice;        //工价
        salaryComObj[TaskPerformance_TechHoursSalary] = techHoursSalary;        //工时工资
        salaryComObj[TaskPerformance_TotalFillingWeight] = totalFillingWeight;        //"总下屑量"
        salaryComObj[TaskPerformance_TotalWorkload] = totalWorkload;        //"总工作量"
        salaryComObj[TaskPerformance_ToolReplenishmentAmount] = ToolReplenishmentAmount;//"补刀金额"
    }

    /// <summary>
    /// 更新粗车工资
    /// </summary>
    /// <param name="salaryComObj">工资绩效 记录</param>
    /// <param name="macineRecordObj">加工任务记录的一条记录</param>
    /// <param name="techHour">工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalaryCC(H3.DataModel.BizObject salaryComObj, H3.DataModel.BizObject macineRecordObj, double techHours, double quantity, double dustWeight, double processingDifficulty = 1)
    {        
        var WorkPrice = 28;
        //工时工资  
        var techHoursSalary = techHours * quantity * WorkPrice * processingDifficulty;
        var shareRate = 1.0;        //分配比例
        string w = MachiningTaskRecord.ID + "='" + ID + "' and " + MachiningTaskRecord.OperationName + "='" + "四面光" + "'";
        var smRow = GetRow(MachiningTaskRecord.TableCode, w);
        if (smRow != null) { shareRate = 0.8; }
        var OutsideDiameter = Convert.ToDouble(macineRecordObj[MachiningTaskRecord.OutsideDiameter] + string.Empty);//"外径" 
        var totalFillingWeight = dustWeight * quantity * shareRate;        //总下屑量

        var subsidy = OutsideDiameter >= 0 && OutsideDiameter < 4000 ? 18 : (OutsideDiameter >= 4000 && OutsideDiameter < 5000 ? 23 : (OutsideDiameter >= 5000 && OutsideDiameter < 6000 ? 30 : 30));
        var ToolReplenishmentAmount = subsidy * totalFillingWeight / 1000;
        var totalWorkload = 0;
    
        salaryComObj[TaskPerformance_WorkPrice] = WorkPrice;        //"工价"
        salaryComObj[TaskPerformance_TechHoursSalary] = techHoursSalary;        //"工时工资"
        salaryComObj[TaskPerformance_TotalFillingWeight] = totalFillingWeight;//"总下屑量"
        salaryComObj[TaskPerformance_TotalWorkload] = totalWorkload;//"总工作量"
        salaryComObj[TaskPerformance_ToolReplenishmentAmount] = ToolReplenishmentAmount;//"补刀金额"
    }

    /// <summary>
    /// 更新精车工资
    /// </summary>
    /// <param name="salaryComObj">工资绩效 记录</param>
    /// <param name="macineRecordObj">加工任务记录的一条记录</param>
    /// <param name="techHour">单件拟定工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalaryJC(H3.DataModel.BizObject salaryComObj, H3.DataModel.BizObject macineRecordObj, double techHours, double quantity, double dustWeight, double processingDifficulty = 1)
    {       
        var workPrice = 28;         
        var techHoursSalary = techHours * quantity * workPrice * processingDifficulty;
        double totalFillingWeight = dustWeight * quantity;        //* 分配比例;
        double toolReplenishmentAmount = techHoursSalary * 0.0875;
        double totalWorkload = 0;

        salaryComObj[TaskPerformance_WorkPrice] = workPrice; //"工价"
        salaryComObj[TaskPerformance_TechHoursSalary] = techHoursSalary; //"工时工资"
        salaryComObj[TaskPerformance_TotalFillingWeight] = totalFillingWeight;  //"总下屑量"
        salaryComObj[TaskPerformance_TotalWorkload] = totalWorkload;   //"总工作量"
        salaryComObj[TaskPerformance_ToolReplenishmentAmount] = toolReplenishmentAmount;  //"补刀金额"

    }

    /// <summary>
    /// 更新钻孔工资
    /// </summary>
    /// <param name="salaryComObj">工资绩效 记录</param>
    /// <param name="macineRecordObj">加工任务记录的一条记录</param>
    /// <param name="techHour">单件拟定工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalaryZK(H3.DataModel.BizObject salaryComObj, H3.DataModel.BizObject macineRecordObj, double techHours, double quantity, double dustWeight, double processingDifficulty = 1)
    {
        var workPrice = 39;
        var totalManHours = techHours * quantity;
        var techHoursSalary = totalManHours * workPrice * processingDifficulty; 
        var totalFillingWeight = dustWeight * quantity;   //* 分配比例;            

        var thickness = Convert.ToDouble(macineRecordObj[MachiningTaskRecord.Thickness]); //"片厚"
        var holeAmount = Convert.ToDouble(macineRecordObj[MachiningTaskRecord.HoleAmount]); //"孔数"
        var drillingProcessingCategory = (string)macineRecordObj[MachiningTaskRecord.DrillingProcessingCategory];  //钻加工类别

        var workloadFactor = drillingProcessingCategory == "深孔钻顶法兰" ? 3 : drillingProcessingCategory == "大孔径产品" ? 2 : 1;
        double totalWorkload = thickness * holeAmount * quantity * workloadFactor / 1000;
        var toolReplenishmentAmount = 0;

        salaryComObj[TaskPerformance_WorkPrice] = workPrice; //"工价"
        salaryComObj[TaskPerformance_TechHoursSalary] = techHoursSalary;//"工时工资"
        salaryComObj[TaskPerformance_TotalFillingWeight] = totalFillingWeight;//"总下屑量"
        salaryComObj[TaskPerformance_TotalWorkload] = totalWorkload;//"总工作量"
        salaryComObj[TaskPerformance_ToolReplenishmentAmount] = toolReplenishmentAmount;//"补刀金额"
    }
    //构建查询语句  返回数据中的首行
    private DataRow GetRow(string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "i_" + table + (where == "" ? "" : " where " + where);
        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        int Count = dt.Rows.Count;
        if (Count > 0)
        {
            return dt.Rows[0];
        }
        else
        {
            return null;
        }
    }
    //日志
    private void Log(string sectionName, string id, string message = "")
    {
        BizObject ar = Tools.BizOperation.New(this.Engine, AbnormalRecordOfPayrollCalculation.TableCode); //工资计算异常记录
        ar[AbnormalRecordOfPayrollCalculation.OperationName] = sectionName;
        ar[AbnormalRecordOfPayrollCalculation.ExceptionDescription] = message;
        ar[AbnormalRecordOfPayrollCalculation.OrderSpecificationNumber] = id;
        ar.Create();
    }

    //机加工任务记录表    
    string MachiningTaskRecordTableCode = "D0014194963919529e44d60be759656d4a16b63";
    string MachiningTaskRecord_SalaryPerformance = "F0000059";  //工资绩效   
    string MachiningTaskRecord_TaskType = "F0000031"; //任务类别   
    string MachiningTaskRecord_TaskName = "F0000002"; //任务名称    
    string MachiningTaskRecord_TechHours = "F0000005";//工时    
    string MachiningTaskRecord_StageTechHours = "F0000006";//报工阶段工时   
    string MachiningTaskRecord_OrderSpecifications = "ProductNum"; //订单规格号       
    string MachiningTaskRecord_WorkPieceNumber = "F0000014"; //工件号   
    string MachiningTaskRecord_Processor = "F0000011"; //加工人员    
    string MachiningTaskRecord_WorkLoad = "F0000010";//加工量   
    string MachiningTaskRecord_FillingWeight = "F0000023"; //工艺下屑重量    
    string MachiningTaskRecord_ProcessDifficulty = "F0000043";//加工难度    
    string MachiningTaskRecord_DepartmentName = "F0000030";//"部门名称"    
    string MachiningTaskRecord_DeviceName = "F0000007";//"设备名称"    
    string MachiningTaskRecord_DeviceType = "F0000041";//"设备类型"  
    string MachiningTaskRecord_DataCode = "F0000058";  //数据代码   
    string MachiningTaskRecord_DepartmentCode = "F0000055"; //部门代码    
    string MachiningTaskRecord_VersionNumber = "F0000056";//版本号   
    string MachiningTaskRecord_MultipleDepartments = "F0000057"; //部门多选

    //任务绩效表  
    string TaskPerformanceTableCode = "D00141922a4f64f7fd74aed89a85e018fca456d";
    string TaskPerformance_Section = "F0000003"; //工序名称    
    string TaskPerformance_TaskName = "F0000004";//任务名称   
    string TaskPerformance_TaskType = "F0000017"; //任务类别    
    string TaskPerformance_OrderSpecificationNumber = "ProductNum";//订单规格    
    string TaskPerformance_WorkPieceNumber = "F0000014";//"工件号"   
    string TaskPerformance_ID = "F0000016"; //ID   
    string TaskPerformance_Processor = "F0000019"; //加工人员    
    string TaskPerformance_DepartmentName = "F0000018";//部门名称   
    string TaskPerformance_DeviceName = "F0000002"; //"设备名称"   
    string TaskPerformance_DeviceType = "F0000020"; //"设备类型"    
    string TaskPerformance_TechHours = "F0000005";//工时    
    string TaskPerformance_StageTechHours = "F0000006";//报工阶段工时    
    string TaskPerformance_DataCode = "F0000024";//数据代码
    string TaskPerformance_DepartmentCode = "F0000021";//部门代码   
    string TaskPerformance_VersionNumber = "F0000022"; //版本号    
    string TaskPerformance_MultipleDepartments = "F0000023";//部门多选    
    string TaskPerformance_Workload = "F0000010";//加工量    
    string TaskPerformance_FillingWeight = "F0000011";//"工艺下屑重量"  
    string TaskPerformance_WorkPrice = "F0000009";  //工价    
    string TaskPerformance_TechHoursSalary = "F0000008";//工时工资    
    string TaskPerformance_TotalFillingWeight = "F0000012";//总下屑量    
    string TaskPerformance_TotalWorkload = "gongzuoliang";//总工作量    
    string TaskPerformance_ToolReplenishmentAmount = "F0000013";//补刀金额
}