using H3.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

public class Salary
{
    H3.IEngine Engine;
    Schema me = null;

    string OrderNum;
    string SpecNum;
    string PID;
    string ID;

    string ProcessName;
    string TaskName;
    double Quantity;

    public double ProcessTime;

    double _OutsideDiameter;
    public double OutsideDiameter
    {
        get { return _OutsideDiameter; }
        set { _OutsideDiameter = value; }
    }

    public Salary(H3.IEngine Engine)
    {
        me = new Schema(Engine, "任务绩效表");
    }
    public Salary(H3.IEngine Engine, string ID)
    {
        this.Engine = Engine;
        this.ID = ID;
        me = new Schema(Engine, "任务绩效表");
    }
    public Salary(H3.IEngine Engine, string OrderNum, string SpecNum, string PID, string ID)
    {
        this.Engine = Engine;
        this.OrderNum = OrderNum;
        this.SpecNum = SpecNum;
        this.PID = PID;
        this.ID = ID;
        me = new Schema(Engine, "任务绩效表");
    }
    public void Save(string processName, bool isSpecTask = false)
    {
        this.ProcessName = processName;
        //this.TaskName = taskName;
        BizObject[] rows = null;
        Schema scmJGJL = null;
        if (GetTaskSrc(processName, out rows, out scmJGJL)) { return; }
        foreach (var row in rows)
        {
            scmJGJL.CurrentRow = row;
            var taskName = scmJGJL.Cell("任务名称");
            var taskType = scmJGJL.Cell("任务类别");

            var rowGZ = Create(processName, taskName);

            me["工序名称"] = processName;
            me["任务名称"] = taskName;
            me["任务类别"] = taskType;

            me["ID"] = ID;
            me["订单规格号"] = scmJGJL["订单规格号"];
            me["工件号"] = scmJGJL["工件号"];
            me["检验结果"] = scmJGJL["检验结果"];
            me["加工人员"] = scmJGJL["加工人员"];
            me["部门名称"] = scmJGJL["部门名称"];
            me["设备名称"] = scmJGJL["设备名称"];

            var 单件拟定工时 = Convert.ToDouble(scmJGJL["单件拟定工时"]);
            var 加工数量 = Convert.ToDouble(scmJGJL["加工数量"]);
            var 工艺下屑重量 = Convert.ToDouble(scmJGJL["工艺下屑重量"]);
            double 加工难度 = Convert.ToDouble(scmJGJL["加工难度"]);

            me["单件拟定工时"] = 单件拟定工时;
            me["加工数量"] = 加工数量;
            me["工艺下屑重量"] = 工艺下屑重量;

            if (processName == "粗车")
            {
                UpdateSalaryCC(scmJGJL, 单件拟定工时, 加工数量, 工艺下屑重量, 加工难度);
            }
            else if (processName == "四面光")
            {
                UpdateSalarySMG(scmJGJL, 单件拟定工时, 加工数量, 工艺下屑重量, 加工难度);
            }
            else if (processName == "精车")
            {
                UpdateSalaryJC(scmJGJL, 单件拟定工时, 加工数量, 工艺下屑重量, 加工难度);
            }
            else if (processName == "钻孔")
            {
                UpdateSalaryZK(scmJGJL, 单件拟定工时, 加工数量, 工艺下屑重量, 加工难度);
            }

            if (rowGZ == null) { me.Create(true); continue; }
            me.Update(true);

        }
    }

    private BizObject Create(string processName, string taskName)
    {
        this.ProcessName = processName;
        this.TaskName = taskName;
        var rowGZ = me.And("ID", "=", ID)
            .And("工序名称", "=", processName)
            .And("任务名称", "=", taskName)
            .GetFirst(true);
        if (rowGZ == null)
        {
            me.GetNew();
        }
        return rowGZ;
    }
    private bool GetTaskSrc(string processName, out BizObject[] rows, out Schema scmJGJL)
    {
        scmJGJL = new Schema(this.Engine, "加工任务记录");
        rows = scmJGJL.ClearFilter()
            .Or("检验结果", "=", "合格")
            .Or("检验结果", "=", "利用")
            .And("ID", "=", ID)
            .And("工序名称", "=", processName)
            .GetList();
        return (rows == null);
    }
    public void UpdateSalarySMG(Schema scmJGJL, double singleProcessTime, double quantity, double dustWeight, double 加工难度 = 1)
    {
        string processName = "四面光";//四面见光
        var 工价 = 28;
        //var 加工难度 = 1;

        var 总工时 = singleProcessTime * quantity;
        var 工时工资 = 总工时 * 工价 * 加工难度;

        var 分配比例 = 0.2;

        var 总下屑量 = dustWeight * quantity * 分配比例;
        var 外径 = Convert.ToDouble(scmJGJL.Cell("外径"));
        var 补助标准 = 外径 >= 0 && 外径 < 4000 ? 18 : (外径 >= 4000 && 外径 < 5000 ? 23 : (外径 >= 5000 && 外径 < 6000 ? 30 : 30));
        var 补刀金额 = 补助标准 * 总下屑量 / 1000;

        var 总工作量 = 0;

        me["总工时"] = 总工时;
        me["工价"] = 工价;
        me["工时工资"] = 工时工资;
        me["总下屑量"] = 总下屑量;
        me["总工作量"] = 总工作量;
        me["补刀金额"] = 补刀金额;
    }
    public void UpdateSalaryCC(Schema scmJGJL, double singleProcessTime, double quantity, double dustWeight, double 加工难度 = 1)
    {
        string processName = "粗车";
        var 工价 = 28;
        //var 加工难度 = 1;

        var 总工时 = singleProcessTime * quantity;
        var 工时工资 = 总工时 * 工价 * 加工难度;

        var 分配比例 = 1.0;
        var smRow = scmJGJL.ClearFilter()
            .And("ID", "=", ID)
            .And("工序名称", "=", "四面光")
            .GetFirst();
        if (smRow != null) { 分配比例 = 0.8; }

        var 外径 = Convert.ToDouble(scmJGJL["外径"]);
        var 总下屑量 = dustWeight * quantity * 分配比例;
        var 补助标准 = 外径 >= 0 && 外径 < 4000 ? 18 : (外径 >= 4000 && 外径 < 5000 ? 23 : (外径 >= 5000 && 外径 < 6000 ? 30 : 30));
        var 补刀金额 = 补助标准 * 总下屑量 / 1000;

        var 总工作量 = 0;

        me["总工时"] = 总工时;
        me["工价"] = 工价;
        me["工时工资"] = 工时工资;
        me["总下屑量"] = 总下屑量;
        me["总工作量"] = 总工作量;
        me["补刀金额"] = 补刀金额;
    }
    public void UpdateSalaryJC(Schema scmJGJL, double singleProcessTime, double quantity, double dustWeight, double 加工难度 = 1)
    {
        string processName = "精车";
        var 工价 = 28;
        //var 加工难度 = 1;
        var 总工时 = singleProcessTime * quantity;
        var 工时工资 = 总工时 * 工价 * 加工难度;

        double 总下屑量 = dustWeight * quantity;  //* 分配比例;
        double 补刀金额 = 工时工资 * 0.0875;
        double 总工作量 = 0;

        me["总工时"] = 总工时;
        me["工价"] = 工价;
        me["工时工资"] = 工时工资;
        me["总下屑量"] = 总下屑量;
        me["总工作量"] = 总工作量;
        me["补刀金额"] = 补刀金额;
    }
    public void UpdateSalaryZK(Schema scmJGJL, double singleProcessTime, double quantity, double dustWeight, double 加工难度 = 1)
    {
        string processName = "钻孔";
        var 工价 = 39;
        //var 加工难度 = 1;

        var 总工时 = singleProcessTime * quantity;
        var 工时工资 = 总工时 * 工价 * 加工难度;

        var 总下屑量 = dustWeight * quantity;   //* 分配比例;            

        var 片厚 = Convert.ToDouble(scmJGJL["片厚"]);
        var 孔数 = Convert.ToDouble(scmJGJL["孔数"]);
        var 产品小类 = scmJGJL.Cell("产品小类");

        var 工作量系数 = 产品小类 == "深孔钻顶法兰" ? 3 : 产品小类 == "大孔径产品" ? 2 : 1;
        double 总工作量 = 片厚 * 孔数 * quantity * 工作量系数 / 1000;
        var 补刀金额 = 0;

        me["总工时"] = 总工时;
        me["工价"] = 工价;
        me["工时工资"] = 工时工资;
        me["总下屑量"] = 总下屑量;
        me["总工作量"] = 总工作量;
        me["补刀金额"] = 补刀金额;
    }
    public double GetSalaryOfCC(string ProcessName, string DeviceType, string peity, double price = 28.0, double difficulty = 1.0)
    {

        var ptime = new ProductTime(this.Engine);
        ProcessTime = ptime.GetTime(OrderNum, SpecNum, PID, DeviceType, ProcessName, peity);
        _OutsideDiameter = ptime.OutsideDiameter;
        return ProcessTime * difficulty * price;

    }
}

