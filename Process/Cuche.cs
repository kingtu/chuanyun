
using H3.Workflow;
using System;

class Cuche
{
    protected void UpdateSalary(Schema me, H3.SmartForm.SubmitSmartFormResponse response,string 任务名称 )
    {
        var 工序名称 = me.PostValue("当前工序");
        //var 任务名称 = me.PostValue("任务名称");  
        var 订单号 = me.PostValue("订单号");
        var 产品编号 = me.PostValue("订单规格号");
        var 规格号 = 产品编号.Split('-')[1];
        var 工件号 = me.PostValue("工件号");
        var 轧制方式 = me.PostValue("轧制方式");

        var 加工者 = me.PostValue("第一加工者");
        var 加工设备 = me.PostValue("第一设备");
        var 设备类型 = me.PostValue("设备类型1");
        var 加工数量 = Convert.ToDouble(me.PostValue("加工量1"));


        var pro = new ProductTime(me.Engine);
        var 拟定工时 = pro.GetTime(订单号, 规格号, 工件号, 设备类型, "粗车", 轧制方式);
        var 工价 = 28.0;
        var 粗车工资 = 拟定工时 * 加工数量 * 工价 * 1.0;

        var slyScm = new Schema(me.Engine, me.CurrentPostValue, "任务绩效表");
        var smRow = slyScm.ClearFilter()
                          .AndFilter("产品编号", "=", 产品编号)
                          .AndFilter("工件号", "=", 工件号)
                          .AndFilter("工序名称", "=", 工序名称)
                          .AndFilter("任务名称", "=", "四面见光")
                          .GetFirst();
        var 分配比例 = 1.0;
        if (smRow != null) { 分配比例 = 0.8; }

        var 工艺下屑重量 = pro.Dust;
        var 实际下屑重量 = 工艺下屑重量 * 加工数量 * 分配比例;
        var 实际用时 = 拟定工时 * 加工数量;

        var 外径 = pro.OutsideDiameter;
        var 补助标准 = 外径 >= 0 && 外径 < 4000 ? 18 : (外径 >= 4000 && 外径 < 5000 ? 23 : (外径 >= 5000 && 外径 < 6000 ? 30 : 30));
        var 补刀金额 = 补助标准 * 实际下屑重量 / 1000; //粗车补刀金额

        var existRow = slyScm.ClearFilter()
                             .AndFilter("产品编号", "=", 产品编号)
                             .AndFilter("工件号", "=", 工件号)
                             .AndFilter("工序名称", "=", 工序名称)
                             .AndFilter("任务名称", "=", 任务名称)
                             .GetFirst();
        if (existRow == null) { slyScm.GetNew(); }

        slyScm.Cell("工序名称", 工序名称);
        slyScm.Cell("任务名称", 任务名称);
        slyScm.Cell("产品编号", 产品编号);
        slyScm.Cell("工件号", 工件号);

        slyScm.Cell("加工者", 加工者);
        slyScm.Cell("加工设备", 加工设备);
        slyScm.CellAny("拟定工时", 拟定工时);
        slyScm.CellAny("实际用时", 实际用时);

        slyScm.CellAny("加工数量", 加工数量);
        slyScm.CellAny("工价", 工价);
        slyScm.CellAny("工资", 粗车工资);
        slyScm.CellAny("工艺下屑重量", 工艺下屑重量);
        slyScm.CellAny("实际下屑重量", 实际下屑重量);
        slyScm.CellAny("补刀金额", 补刀金额);
        if (existRow == null)
        { slyScm.Create(true); }
        else 
        { slyScm.Update(true); } 
    }

    protected void UpdateSalaryByJGJL(Schema me, H3.SmartForm.SubmitSmartFormResponse response, string 工序名称)
    {
        //JGJL=加工记录          
        //var 工序名称 = "粗车";

        var ID = me.PostValue("ID");
        var scmJGJL = new Schema(me.Engine, me.CurrentPostValue, "加工任务记录");
        var rows=scmJGJL.ClearFilter()
                        .AndFilter("ID", "=", ID)
                        .AndFilter("工序名称", "=", 工序名称)
                        .AndFilter("检验结论", "=","合格")
                        .GetList();
        foreach(var row in rows  )
        {
            scmJGJL.CurrentRow = row;
            var 单件拟定工时 = Convert.ToDouble(scmJGJL.Cell("单件拟定工时"));
            var 加工数量 = Convert.ToDouble(scmJGJL.Cell("单件拟定工时"));
            var 工价 = 28; 
            var 加工难度 = 1;
            var 加工人员 = scmJGJL.Cell("加工人员");
            var 设备名称= scmJGJL.Cell("设备名称");
            var 所属部门 = scmJGJL.Cell("所属部门");
            var 检验结果 = scmJGJL.Cell("检验结果");
            var 任务名称= scmJGJL.Cell("任务名称"); 

            var 总工时 = 单件拟定工时 * 加工数量;
            var 总工资 = 总工时 * 工价 * 加工难度;

            var smRow = scmJGJL.ClearFilter()
                               .AndFilter("ID", "=", ID)
                               .AndFilter("工序名称", "=", 工序名称)
                               .AndFilter("任务名称", "=", "四面见光")
                               .GetFirst();
            var 分配比例 = 1.0;
            if (smRow != null) { 分配比例 = 0.8; }

            var 工艺下屑重量 = Convert.ToDouble(scmJGJL.Cell("下屑量"));
            var 总下屑量 = 工艺下屑重量 * 加工数量 * 分配比例;
            var 外径 = Convert.ToDouble(scmJGJL.Cell("外径"));
            var 补助标准 = 外径 >= 0 && 外径 < 4000 ? 18 : (外径 >= 4000 && 外径 < 5000 ? 23 : (外径 >= 5000 && 外径 < 6000 ? 30 : 30));
            var 补刀金额 = 补助标准 * 总下屑量 / 1000;

            var 总工作量 = 0;

            var scmGZ = new Schema(me.Engine, me.CurrentPostValue, "任务绩效表");
            scmGZ.GetNew();
           
            
            scmGZ.CellAny("工序名称", 工序名称);
            scmGZ.CellAny("任务名称", 任务名称);

            scmGZ.CellAny("ID", ID);
            scmGZ.CellAny("检验结果", 检验结果);
            scmGZ.CellAny("加工人员", 加工人员);
            scmGZ.CellAny("所属部门", 所属部门);
            scmGZ.CellAny("设备名称", 设备名称);
            scmGZ.CellAny("单件拟定工时", 单件拟定工时);
            scmGZ.CellAny("总工时", 总工时);
            scmGZ.CellAny("加工数量", 加工数量);
            scmGZ.CellAny("工价", 工价);
            scmGZ.CellAny("总工资", 总工资);
            scmGZ.CellAny("工艺下屑重量", 工艺下屑重量);
            scmGZ.CellAny("总下屑量", 总下屑量);
            scmGZ.CellAny("总工作量", 总工作量);
            scmGZ.CellAny("补刀金额", 补刀金额);
            scmGZ.Create(true);

        }

    }

    protected void UpdateSalaryOfSMG(Schema me, H3.SmartForm.SubmitSmartFormResponse response, string 工序名称)
    {
        //JGJL=加工记录          
        //var 工序名称 = "粗车";

        var ID = me.PostValue("ID");
        var scmJGJL = new Schema(me.Engine, me.CurrentPostValue, "加工任务记录");
        var rows = scmJGJL.ClearFilter()
                        .AndFilter("ID", "=", ID)
                        .AndFilter("工序名称", "=", 工序名称)
                        .AndFilter("检验结论", "=", "合格")
                        .GetList();
        foreach (var row in rows)
        {
            scmJGJL.CurrentRow = row;
            var 单件拟定工时 = Convert.ToDouble(scmJGJL.Cell("单件拟定工时"));
            var 加工数量 = Convert.ToDouble(scmJGJL.Cell("单件拟定工时"));
            var 工价 = 28;
            var 加工难度 = 1;
            var 加工人员 = scmJGJL.Cell("加工人员");
            var 设备名称 = scmJGJL.Cell("设备名称");
            var 所属部门 = scmJGJL.Cell("所属部门");
            var 检验结果 = scmJGJL.Cell("检验结果");
            var 任务名称 = scmJGJL.Cell("任务名称");

            var 总工时 = 单件拟定工时 * 加工数量;
            var 总工资 = 总工时 * 工价 * 加工难度;

            var smRow = scmJGJL.ClearFilter()
                               .AndFilter("ID", "=", ID)
                               .AndFilter("工序名称", "=", 工序名称)
                               .AndFilter("任务名称", "=", "四面见光")
                               .GetFirst();
            var 分配比例 = 1.0;
            if (smRow != null) { 分配比例 = 0.8; }

            var 工艺下屑重量 = Convert.ToDouble(scmJGJL.Cell("下屑量"));
            var 总下屑量 = 工艺下屑重量 * 加工数量 * 分配比例;
            var 外径 = Convert.ToDouble(scmJGJL.Cell("外径"));
            var 补助标准 = 外径 >= 0 && 外径 < 4000 ? 18 : (外径 >= 4000 && 外径 < 5000 ? 23 : (外径 >= 5000 && 外径 < 6000 ? 30 : 30));
            var 补刀金额 = 补助标准 * 总下屑量 / 1000;

            var 总工作量 = 0;

            var scmGZ = new Schema(me.Engine, me.CurrentPostValue, "任务绩效表");
            scmGZ.GetNew();


            scmGZ.CellAny("工序名称", 工序名称);
            scmGZ.CellAny("任务名称", 任务名称);

            scmGZ.CellAny("ID", ID);
            scmGZ.CellAny("检验结果", 检验结果);
            scmGZ.CellAny("加工人员", 加工人员);
            scmGZ.CellAny("所属部门", 所属部门);
            scmGZ.CellAny("设备名称", 设备名称);
            scmGZ.CellAny("单件拟定工时", 单件拟定工时);
            scmGZ.CellAny("总工时", 总工时);
            scmGZ.CellAny("加工数量", 加工数量);
            scmGZ.CellAny("工价", 工价);
            scmGZ.CellAny("总工资", 总工资);
            scmGZ.CellAny("工艺下屑重量", 工艺下屑重量);
            scmGZ.CellAny("总下屑量", 总下屑量);
            scmGZ.CellAny("总工作量", 总工作量);
            scmGZ.CellAny("补刀金额", 补刀金额);
            scmGZ.Create(true);

        }

    }
    protected void AutoUpdateTask(Schema me, H3.SmartForm.SubmitSmartFormResponse response)
    {
        var row = me.GetRow(Request.BizObjectId);
        var 任务名称 = me.PostValue("任务名称");
        if(任务名称 ==null || 任务名称 == "")
        {任务名称 = "第一次粗车"; }
        else if(任务名称 == "第一次粗车")
        { 任务名称 = "第二次粗车"; }
        else if (任务名称 == "第二次粗车")
        { 任务名称 = "第三次粗车"; }
        else if (任务名称 == "第三次粗车")
        { 任务名称 = "第四次粗车"; }

    }
}

internal class Request
{
    public static string BizObjectId { get; internal set; }
    public static string ActivityCode { get; internal set; }
    public static ActivityTemplate ActivityTemplate { get; internal set; }
}