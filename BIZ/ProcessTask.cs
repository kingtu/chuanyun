using System;
using System.Collections.Generic;
using System.Text;


class ProcessTask
{
    H3.IEngine Engine;
    //Schema me = null;
    Schema me = null;
    ProcessTask(H3.IEngine Engine)
    {
        this.Engine = Engine;
        me = new Schema(Engine, (H3.SmartForm.SmartFormPostValue)null, "加工任务记录");
    }
    public void InsertProductTask(Schema main, string processName, string taskName, string processorId, string deviceType)
    {        
        var scmPara = new Schema(main.Engine, (H3.SmartForm.SmartFormPostValue)null, "产品参数表");        
        string 订单规格号 = main.Cell("订单规格号");
        string 工件号 = main.Cell("工件号");
        var row = me.And("ID", "=", main.Cell("ID"))
            .And("工序名称", "=", processName)
            .And("任务名称", "=", taskName)
            .GetFirst(true);
        if (row == null) { me.GetNew(); }
        scmPara.And("产品编号", "=", 订单规格号).GetFirst(true);
        me.Copy(main);
        me.Copy(scmPara);
        me.Cell("ID", main.Cell("ID"));
        me.Cell("工序名称", processName);
        me.Cell("任务名称", taskName);
        ProductTime pt = new ProductTime(main.Engine);
        var 单件拟定工时 = pt.GetTime(订单规格号, 工件号, deviceType, processName);
        me.CellAny("单件拟定工时", 单件拟定工时);
        me.CellAny("上机时间", System.DateTime.Now);
        me.CellAny("工艺下屑重量", pt.Dust);
        me.Cell("设备名称", main.Cell("使用设备"));
        me.Cell("设备类型", deviceType);
        me.Cell("加工人员", processorId);
        me.Cell("部门名称", Role.GetDepartmentName(main.Engine, processorId));
        me.CellAny("加工数量", 0);
        if (row == null) { me.Create(); return; }
        me.Update();
    }
    public void UpdateProductTask(Schema main, string processName, string taskName, double quantity)
    {       
        var row = me.And("ID", "=", main.Cell("ID"))
            .And("工序名称", "=", processName)
            .And("任务名称", "=", taskName)
            .GetFirst(true);
        if (row == null) { return; }
        var 上机时间 = Convert.ToDateTime(me.CellAny("上机时间"));
        var now = System.DateTime.Now;
        me.CellAny("下机时间", now);
        TimeSpan t = now.Subtract(上机时间);
        me.CellAny("实际耗时", t.Seconds);
        me.CellAny("任务工时", "");
        me.CellAny("加工数量", quantity);
        me.Update();
    }
    public void UpdateCheckResult(Schema main, string processName)
    {      
        var rows = me.And("ID", "=", main.Cell("ID"))
            .And("工序名称", "=", processName)
            .GetList();
        foreach (var row in rows)
        {
            me.CurrentRow = row;
            me.CellAny("实际外径", main.CellAny("实际外径"));
            me.CellAny("检验结果", main.CellAny("检验结果"));
            me.Update(true);
        }
    }

}

