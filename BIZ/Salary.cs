using H3.Data.Filter;
using H3.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public class Salary
{
    H3.IEngine Engine;
    BizObject me = null;
       
    string ID;
    string ProcessName;

    /// <summary>
    /// 填写任务绩效表
    /// </summary>
    /// <param name="Engine"></param>
    /// <param name="ID"></param>
    public Salary(H3.IEngine Engine, string ID)
    {
        this.Engine = Engine;
        this.ID = ID;        
    }
    
    /// <summary>
    /// 根据加工任务记录表计算工资并保存
    /// </summary>
    /// <param name="processName">工序名称</param>
    /// <param name="isSpecTask">是否特殊工序</param>
    public void Save(string processName, bool isSpecTask = false)
    {
        this.ProcessName = processName;
        //this.TaskName = taskName;
        DataRowCollection rows = null;
        if (GetTaskSrc(processName, out rows)) { return; }
        foreach (DataRow row in rows)
        {
            DataRow scmJGJL = row;
            var taskName = (string)scmJGJL[MachiningTaskRecord.TaskName];  //"任务名称"
            var taskType = scmJGJL[MachiningTaskRecord.TaskType]; //"任务类别"

            me = Create(processName, taskName);

            me[TaskPerformance.OperationName] = processName; //"工序名称"
            me[TaskPerformance.TaskName] = taskName;//"任务名称"
            me[TaskPerformance.TaskCategory] = taskType;//"任务类别"

            me[TaskPerformance.ID] = ID;//"ID"
            me[TaskPerformance.OrderSpecificationNumber] = scmJGJL[MachiningTaskRecord.OrderSpecifications]; //"订单规格号"
            me[TaskPerformance.PieceNumber] = scmJGJL[MachiningTaskRecord.WorkPieceNumber]; //"工件号"
            me[TaskPerformance.InspectionResult] = scmJGJL[MachiningTaskRecord.InspectionResults]; //"检验结果"
            me[TaskPerformance.Processor] = scmJGJL[MachiningTaskRecord.Processor];//加工人员
            me[TaskPerformance.DepartmentName] = scmJGJL[MachiningTaskRecord.DepartmentName];//"部门名称"
            me[TaskPerformance.EquipmentName] = scmJGJL[MachiningTaskRecord.DeviceName];//"设备名称"

            var unitmanHour = Convert.ToDouble(scmJGJL[MachiningTaskRecord.UnitmanHour]); //单件拟定工时
            var workLoad = Convert.ToDouble(scmJGJL[MachiningTaskRecord.WorkLoad]); //加工数量
            var processChipWeight = Convert.ToDouble(scmJGJL[MachiningTaskRecord.ProcessChipWeight]); //工艺下屑重量
            double processDifficulty = Convert.ToDouble(scmJGJL[MachiningTaskRecord.ProcessDifficulty]);//加工难度

            me[TaskPerformance.PlannedManHoursForASinglePiece] = unitmanHour; //单件拟定工时
            me[TaskPerformance.ProcessingQuantity] = workLoad;//加工数量
            me[TaskPerformance.ProcessChipWeight] = processChipWeight; //"工艺下屑重量"

            if (processName == "粗车")
            {
                UpdateSalaryCC(scmJGJL, unitmanHour, workLoad, processChipWeight, processDifficulty);
            }
            else if (processName == "四面光")
            {
                UpdateSalarySMG(scmJGJL, unitmanHour, workLoad, processChipWeight, processDifficulty);
            }
            else if (processName == "精车")
            {
                UpdateSalaryJC(scmJGJL, unitmanHour, workLoad, processChipWeight, processDifficulty);
            }
            else if (processName == "钻孔")
            {
                UpdateSalaryZK(scmJGJL, unitmanHour, workLoad, processChipWeight, processDifficulty);
            }

            //me.Save();
            if (me.State == H3.DataModel.BizObjectState.Unloaded)
            {
                me.Create();
            }
            else
            {
                me.Update();
            }

        }
    }
    /// <summary>
    /// 找出或创建一条符合条件的任务绩效表的记录
    /// </summary>
    /// <param name="processName">工序名称</param>
    /// <param name="taskName">任务名称</param>
    /// <returns>任务绩效表的记录</returns>
    private BizObject Create(string processName, string taskName)
    {
        this.ProcessName = processName;
        //this.TaskName = taskName;
        //var rowGZ = me.And("ID", "=", ID).And("工序名称", "=", processName).And("任务名称", "=", taskName).GetFirst(true);
        //if (rowGZ == null) { me.GetNew(); }

        H3.Data.Filter.Filter f = new Filter();
        Tools.Filter.And(f, TaskPerformance.ID, H3.Data.ComparisonOperatorType.Equal, ID);
        Tools.Filter.And(f, TaskPerformance.OperationName, H3.Data.ComparisonOperatorType.Equal, processName);
        Tools.Filter.And(f, TaskPerformance.TaskName, H3.Data.ComparisonOperatorType.Equal, taskName);
        BizObject rowGZ = Tools.BizOperation.GetFirst(this.Engine, TaskPerformance.TableCode, f);
        if (rowGZ == null)
        {
            rowGZ = Tools.BizOperation.New(this.Engine, TaskPerformance.TableCode);
        }
        return rowGZ;
    }

    /// <summary>
    /// 找出所有符合条件的加工任务记录
    /// </summary>
    /// <param name="processName">工序名称</param>
    /// <param name="rows">结果</param>
    /// <returns>true:成功;false:失败</returns>
    private bool GetTaskSrc(string processName, out DataRowCollection rows)
    {
        //scmJGJL = new Schema(this.Engine, "加工任务记录");
        //rows = scmJGJL.ClearFilter()
        //    .Or("检验结果", "=", "合格")
        //    .Or("检验结果", "=", "利用")
        //    .And("ID", "=", ID)
        //    .And("工序名称", "=", processName)
        //    .GetList();
        string w = MachiningTaskRecord.ID + "='" + ID + "' and " +
                   MachiningTaskRecord.OperationName + "='" + processName + "' and (" +
                   MachiningTaskRecord.InspectionResults + "='合格' or " +
                   MachiningTaskRecord.InspectionResults + "='利用'" + ")";
        rows = GetRows(MachiningTaskRecord.TableCode, w);
        return (rows == null);
    }
    /// <summary>
    /// 更新四面光工资
    /// </summary>
    /// <param name="scmJGJL">加工任务记录的一条记录</param>
    /// <param name="singleProcessTime">单件拟定工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalarySMG(DataRow scmJGJL, double singleProcessTime, double quantity, double dustWeight, double processingDifficulty = 1)
    {
        string processName = "四面光";//四面见光
        var workPrice = 28;//"工价"
        //var 加工难度 = 1;

        var totalManHours = singleProcessTime * quantity;//"总工时"
        var ManHoursSalary = totalManHours * workPrice * processingDifficulty;//"工时工资"

        var shareRate = 0.2;

        var TotalScrap = dustWeight * quantity * shareRate;//"总下屑量"
        var outsideDiameter = Convert.ToDouble(scmJGJL[MachiningTaskRecord.OutsideDiameter]);//"外径"
        //补助标准
        var subsidy = outsideDiameter >= 0 && outsideDiameter < 4000 ? 18 : (outsideDiameter >= 4000 && outsideDiameter < 5000 ? 23 : (outsideDiameter >= 5000 && outsideDiameter < 6000 ? 30 : 30));
        var ToolReplenishmentAmount = subsidy * TotalScrap / 1000;//"补刀金额"

        var TotalWorkload = 0;//"总工作量"

        me[TaskPerformance.TotalManHours] = totalManHours; 
        me[TaskPerformance.WorkPrice] = workPrice;
        me[TaskPerformance.ManHoursSalary] = ManHoursSalary; 
        me[TaskPerformance.TotalScrap] = TotalScrap;
        me[TaskPerformance.TotalWorkload] = TotalWorkload;
        me[TaskPerformance.ToolReplenishmentAmount] = ToolReplenishmentAmount;

    }

    /// <summary>
    /// 更新粗车工资
    /// </summary>
    /// <param name="scmJGJL">加工任务记录的一条记录</param>
    /// <param name="singleProcessTime">单件拟定工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalaryCC(DataRow scmJGJL, double singleProcessTime, double quantity, double dustWeight, double processingDifficulty = 1)
    {
        string processName = "粗车";
        var WorkPrice = 28;
        //var 加工难度 = 1;

        var TotalManHours = singleProcessTime * quantity;
        var ManHoursSalary = TotalManHours * WorkPrice * processingDifficulty;

        var shareRate = 1.0;
        //var smRow = scmJGJL.ClearFilter()
        //    .And("ID", "=", ID)
        //    .And("工序名称", "=", "四面光")
        //    .GetFirst();
        string w = MachiningTaskRecord.ID + "='" + ID + "' and " + MachiningTaskRecord.OperationName + "='" + "四面光" + "'";
        var smRow = GetRow(MachiningTaskRecord.TableCode, w);
        if (smRow != null) { shareRate = 0.8; }

        var OutsideDiameter = Convert.ToDouble(scmJGJL[MachiningTaskRecord.OutsideDiameter]);//"外径" 
        var TotalScrap = dustWeight * quantity * shareRate;
        var subsidy = OutsideDiameter >= 0 && OutsideDiameter < 4000 ? 18 : (OutsideDiameter >= 4000 && OutsideDiameter < 5000 ? 23 : (OutsideDiameter >= 5000 && OutsideDiameter < 6000 ? 30 : 30));
        var ToolReplenishmentAmount = subsidy * TotalScrap / 1000;

        var TotalWorkload = 0;

        me[TaskPerformance.TotalManHours] = TotalManHours; //"总工时"
        me[TaskPerformance.WorkPrice] = WorkPrice; //"工价"
        me[TaskPerformance.ManHoursSalary] = ManHoursSalary;//"工时工资"
        me[TaskPerformance.TotalScrap] = TotalScrap;//"总下屑量"
        me[TaskPerformance.TotalWorkload] = TotalWorkload;//"总工作量"
        me[TaskPerformance.ToolReplenishmentAmount] = ToolReplenishmentAmount;//"补刀金额"
    }

    /// <summary>
    /// 更新精车工资
    /// </summary>
    /// <param name="scmJGJL">加工任务记录的一条记录</param>
    /// <param name="singleProcessTime">单件拟定工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalaryJC(DataRow scmJGJL, double singleProcessTime, double quantity, double dustWeight, double processingDifficulty = 1)
    {
        string processName = "精车";
        var workPrice = 28;
        //var 加工难度 = 1;
        var totalManHours = singleProcessTime * quantity;
        var manHoursSalary = totalManHours * workPrice * processingDifficulty;

        double totalScrap = dustWeight * quantity;  //* 分配比例;
        double toolReplenishmentAmount = manHoursSalary * 0.0875;
        double TotalWorkload = 0;

        me[TaskPerformance.TotalManHours] = totalManHours; //"总工时"
        me[TaskPerformance.WorkPrice] = workPrice; //"工价"
        me[TaskPerformance.ManHoursSalary] = manHoursSalary;//"工时工资"
        me[TaskPerformance.TotalScrap] = totalScrap;//"总下屑量"
        me[TaskPerformance.TotalWorkload] = TotalWorkload;//"总工作量"
        me[TaskPerformance.ToolReplenishmentAmount] = toolReplenishmentAmount;//"补刀金额"
    }

    /// <summary>
    /// 更新钻孔工资
    /// </summary>
    /// <param name="scmJGJL">加工任务记录的一条记录</param>
    /// <param name="singleProcessTime">单件拟定工时</param>
    /// <param name="quantity">加工量</param>
    /// <param name="dustWeight">工艺下屑重量</param>
    /// <param name="processingDifficulty">加工难度</param>
    public void UpdateSalaryZK(DataRow scmJGJL, double singleProcessTime, double quantity, double dustWeight, double processingDifficulty = 1)
    {
        string processName = "钻孔";
        var workPrice = 39;

        var totalManHours = singleProcessTime * quantity;
        var manHoursSalary = totalManHours * workPrice * processingDifficulty; //processingDifficulty

        var totalScrap = dustWeight * quantity;   //* 分配比例;            

        var thickness = Convert.ToDouble(scmJGJL[MachiningTaskRecord.Thickness]); //"片厚"
        var holeAmount = Convert.ToDouble(scmJGJL[MachiningTaskRecord.HoleAmount]); //"孔数"
        var drillingProcessingCategory = (string)scmJGJL[MachiningTaskRecord.DrillingProcessingCategory];  //钻加工类别

        var workloadFactor = drillingProcessingCategory == "深孔钻顶法兰" ? 3 : drillingProcessingCategory == "大孔径产品" ? 2 : 1;
        double totalWorkload = thickness * holeAmount * quantity * workloadFactor / 1000;
        var toolReplenishmentAmount = 0;

        me[TaskPerformance.TotalManHours] = totalManHours; //"总工时"
        me[TaskPerformance.WorkPrice] = workPrice; //"工价"
        me[TaskPerformance.ManHoursSalary] = manHoursSalary;//"工时工资"
        me[TaskPerformance.TotalScrap] = totalScrap;//"总下屑量"
        me[TaskPerformance.TotalWorkload] = totalWorkload;//"总工作量"
        me[TaskPerformance.ToolReplenishmentAmount] = toolReplenishmentAmount;//"补刀金额"
    }

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
    private DataRowCollection GetRows(string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "i_" + table + (where == "" ? "" : " where " + where);
        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        return dt.Rows;
    }




}
