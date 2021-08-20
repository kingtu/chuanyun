using System;
using System.Collections.Generic;
using System.Text;


//class ProcessTask
//{
//    H3.IEngine Engine;
//    //Schema me = null;
//    Schema me = null;
//    ProcessTask(H3.IEngine Engine)
//    {
//        this.Engine = Engine;
//        me = new Schema(Engine, "加工任务记录");
//    }
//    public void InsertProductTask(Schema main, string processName, string taskName, string processorId, string deviceType)
//    {        
//        var scmPara = new Schema(main.Engine,  "产品参数表");        
//        string 订单规格号 = main.Cell("订单规格号");
//        string 工件号 = main.Cell("工件号");
//        var row = me.And("ID", "=", main.Cell("ID"))
//            .And("工序名称", "=", processName)
//            .And("任务名称", "=", taskName)
//            .GetFirst(true);
//        if (row == null) { me.GetNew(); }
//        scmPara.And("产品编号", "=", 订单规格号).GetFirst(true);
//        me.Copy(main);
//        me.Copy(scmPara);
//        me.Cell("ID", main.Cell("ID"));
//        me.Cell("工序名称", processName);
//        me.Cell("任务名称", taskName);
//        ProductTime pt = new ProductTime(main.Engine);
//        var 单件拟定工时 = pt.GetTime(订单规格号, 工件号, deviceType, processName);
//        me.CellAny("单件拟定工时", 单件拟定工时);
//        me.CellAny("上机时间", System.DateTime.Now);
//        me.CellAny("工艺下屑重量", pt.Dust);
//        me.Cell("设备名称", main.Cell("使用设备"));
//        me.Cell("设备类型", deviceType);
//        me.Cell("加工人员", processorId);
//        me.Cell("部门名称", Role.GetDepartmentName(main.Engine, processorId));
//        me.CellAny("加工数量", 0);
//        if (row == null) { me.Create(); return; }
//        me.Update();
//    }
//    public void UpdateProductTask(Schema main, string processName, string taskName, double quantity)
//    {       
//        var row = me.And("ID", "=", main.Cell("ID"))
//            .And("工序名称", "=", processName)
//            .And("任务名称", "=", taskName)
//            .GetFirst(true);
//        if (row == null) { return; }
//        var 上机时间 = Convert.ToDateTime(me.CellAny("上机时间"));
//        var now = System.DateTime.Now;
//        me.CellAny("下机时间", now);
//        TimeSpan t = now.Subtract(上机时间);
//        me.CellAny("实际耗时", t.Seconds);
//        me.CellAny("任务工时", "");
//        me.CellAny("加工数量", quantity);
//        me.Update();
//    }
//    public void UpdateCheckResult(Schema main, string processName)
//    {      
//        var rows = me.And("ID", "=", main.Cell("ID"))
//            .And("工序名称", "=", processName)
//            .GetList();
//        foreach (var row in rows)
//        {
//            me.CurrentRow = row;
//            me.CellAny("实际外径", main.CellAny("实际外径"));
//            me.CellAny("检验结果", main.CellAny("检验结果"));
//            me.Update(true);
//        }
//    }

//}

public class ProcessTask
{
    H3.IEngine Engine;

    H3.SmartForm.SmartFormRequest sr;
    H3.DataModel.BizObject sb;
    //SmartFormTools tool;
    Schema me = null;

    public ProcessTask(H3.SmartForm.SmartFormRequest request)
    {
        this.sr = request;
        this.Engine = request.Engine;
        this.sb = request.BizObject;
        //this.tool = new SmartFormTools(request);

        me = new Schema(Engine, "加工任务记录");
    }



    public object TaskRecord(string processName, string subTabaleName)
    {
        object ObjectId = "";
        Schema main = new Schema(this.Engine, this.sb);
        H3.DataModel.BizObject[] subTable = main[subTabaleName] as H3.DataModel.BizObject[];
        if (subTable.Length > 0)
        {
            Schema taskbo = new Schema(this.Engine, subTable[subTable.Length - 1]);
            string taskName = taskbo["任务名称"] + string.Empty;
            string workerId = taskbo["加工者"] + string.Empty;
            string deviceName = taskbo["设备名称"] + string.Empty;
            string deviceNum = taskbo["设备编号"] + string.Empty;
            string deviceType = taskbo["设备类型"] + string.Empty;
            double workLoad = Convert.ToDouble(taskbo["加工量"]);
            DateTime startTime = Convert.ToDateTime(taskbo["开始时间"]);
            DateTime endTime = Convert.ToDateTime(taskbo["结束时间"]);
            string adjust = taskbo["申请难度调整"] + string.Empty;
            ObjectId = Insert(main, processName, taskName, workerId, deviceName, deviceNum, deviceType, workLoad, startTime, endTime, adjust);
        }

        return ObjectId;
    }

    public object Insert(Schema main, string processName, string taskName, string workerId, string deviceName, string deviceNum, string deviceType, double workLoad, DateTime startTime, DateTime endTime, string adjust)
    {
        string ID = main.Cell("ID");
        var row = me.And("ID", "=", ID)
            .And("工序名称", "=", processName)
            .And("任务名称", "=", taskName)
            .GetFirst(true);
        if (row == null) { me.GetNew(); }

        string 订单规格号 = main.Cell("订单规格号");
        string 工件号 = main.Cell("工件号");
        var scmPara = new Schema(Engine, "产品参数表");
        scmPara.And("订单规格号", "=", 订单规格号).GetFirst(true);
        me.Copy(main);
        me.Copy(scmPara);
        me["ID"] = ID;
        me["工序名称"] = processName;
        me["任务名称"] = taskName;

        ProductTime pt = new ProductTime(Engine);
        var 单件拟定工时 = 0.0;

        单件拟定工时 = pt.GetTime(订单规格号, 工件号, deviceType, processName);

        me["单件拟定工时"] = 单件拟定工时;
        me["工艺下屑重量"] = pt.Dust;
        me["上机时间"] = startTime;
        me["下机时间"] = endTime;
        me["设备名称"] = deviceName;
        me["设备编号"] = deviceNum;
        me["设备类型"] = deviceType;
        me["加工人员"] = workerId;
        me["加工数量"] = workLoad;
        me["部门名称"] = Role.GetDepartmentName(main.Engine, workerId);
        me["申请难度调整"] = adjust;

        if (row == null)
        {
            me.Create();
        }
        else
        {
            me.Update();
        }
        return me["ObjectId"];

    }
    public void Update(Schema main, string processName, string taskName, double quantity)
    {
        string ID = main.Cell("ID");
        var row = me.And("ID", "=", ID)
            .And("工序名称", "=", processName)
            .And("任务名称", "=", taskName)
            .GetFirst(true);
        if (row == null) { return; }

        var 上机时间 = Convert.ToDateTime(me.CellAny("上机时间"));
        var now = System.DateTime.Now;
        me.CellAny("下机时间", now);
        TimeSpan t = now.Subtract(上机时间);
        me["实际耗时"] = t.Seconds;
        me["任务工时"] = 0;
        me["加工数量"] = quantity;
        me.Update();
    }
    public void UpdateCheckResult(Schema main, string processName)
    {
        string ID = main.Cell("ID");
        var rows = me.And("ID", "=", ID)
            .And("工序名称", "=", processName)
            .GetList();
        foreach (var row in rows)
        {
            me.CurrentRow = row;
            me["实际外径"] = main["实际外径"];
            me["检验结果"] = main["检验结果"];
            me.Update(true);
        }
    }

}