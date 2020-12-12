using System;
using System.Collections.Generic;
using System.Text;

namespace Chuanyun.BIZ
{
    class ProductTime
    {
    }
}
//public class ProductTime
//{
//    private Schema pScm;
//    private Schema idScm;
//    private Schema dScm;
//    private Schema dScm_1;
//    //private Schema device;
//    double _OutsideDiameter;
//    public double OutsideDiameter
//    {
//        get { return _OutsideDiameter; }
//        set { _OutsideDiameter = value; }
//    }

//    static ProductTime()
//    {

//    }

//    public ProductTime(H3.IEngine Engine)
//    {
//        if (pScm == null) { pScm = new Schema(Engine, "6b62f7decd924e1e8713025dc6a39aa5"); } //产品参数表:6b62f7decd924e1e8713025dc6a39aa5  6b62f7decd924e1e8713025dc6a39aa5                                                                                          
//        if (idScm == null) { idScm = new Schema(Engine, "Szlywopbivyrv1d64301ta5xv4"); }     //产品ID表--工序计划表：Szlywopbivyrv1d64301ta5xv4；锯切表:ba65c69eb46c4156b0eb25930bf50b93 
//        if (dScm == null) { dScm = new Schema(Engine, "5ed7e837ecee4f97800877820d9a2f05"); } //设备工时系数表
//        if (dScm_1 == null) { dScm_1 = new Schema(Engine, dScm.Columns["子表"], true); }     //设备工时系数表子表
//        //if(device == null) { device = new Schema(Engine, "D000886SBTZ", true); }          //设备档案 D000886SBTZ
//    }

//    public double GetTime(string 产品编号, string 工件号, string 设备类型, string 工序名称)
//    {
//        var psn = 产品编号.Split('-');
//        return GetTime(psn[0], psn[1], 工件号, 设备类型, 工序名称);
//    }

//    public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称)
//    {
//        idScm.ClearFilter()
//            .AndFilter("订单号", "=", 订单号)
//            .AndFilter("规格号", "=", 规格号)
//            .AndFilter("工件号", "=", 工件号)
//            .GetFirst(true);
//        var 胚体类型 = idScm.Cell("轧制方式");
//        return GetTime(订单号, 规格号, 工件号, 设备类型, 工序名称, 胚体类型);
//    }

//    public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称, string 胚体类型)
//    {
//        double 所需工时 = 0;
//        pScm.ClearFilter()
//            .AndFilter("订单号", "=", 订单号)
//            .AndFilter("规格号", "=", 规格号)
//            .GetFirst(true);

//        var test = pScm;

//        var 产品类别 = pScm.Cell("产品类别");
//        var 产品小类 = pScm.Cell("产品小类");
//        var 外径 = pScm.Cell("外径");
//        OutsideDiameter = Convert.ToDouble(外径);


//        if (工序名称 == "钻孔")
//        {
//            dScm.ClearFilter()
//                .AndFilter("工序名称", "=", 工序名称)
//                .AndFilter("产品小类", "=", 产品小类)
//                .GetFirst(true);
//        }
//        else
//        {
//            dScm.ClearFilter()
//                .AndFilter("工序名称", "=", 工序名称)
//                .AndFilter("产品类别", "=", 产品类别)
//                .GetFirst(true);
//        }

//        test = dScm;

//        if (工序名称 == "钻孔")
//        {
//            dScm_1.ClearFilter()
//                .AndFilter(Schema.PID, "=", dScm.Cell(Schema.RID))
//                .AndFilter("设备类型", "=", 设备类型)
//                .GetFirst(true);
//        }
//        else
//        {
//            dScm_1.ClearFilter()
//                .AndFilter(Schema.PID, "=", dScm.Cell(Schema.RID))
//                .AndFilter("设备类型", "=", 设备类型)
//                .AndFilter("外径下限", "<", 外径)
//                .AndFilter("外径上限", ">", 外径)
//                .GetFirst(true);

//        }

//        if (工序名称 == "粗车")
//        {
//            var 单轧粗车工时 = Convert.ToDouble(pScm.Cell("单轧粗车工时"));
//            var 双轧粗车工时 = Convert.ToDouble(pScm.Cell("双轧粗车工时"));
//            var 单轧工时系数 = Convert.ToDouble(dScm_1.Cell("单轧工时系数"));
//            var 双轧工时系数 = Convert.ToDouble(dScm_1.Cell("双轧工时系数"));
//            所需工时 = (double)(胚体类型 == "单轧" ? 单轧粗车工时 : 双轧粗车工时) * (double)(胚体类型 == "单轧" ? 单轧工时系数 : 双轧工时系数) * (double)1;
//        }

//        if (工序名称 == "精车")
//        {
//            var 精车工时 = Convert.ToDouble(pScm.Cell("精车工时"));
//            var 单轧工时系数 = Convert.ToDouble(dScm_1.Cell("单轧工时系数"));
//            所需工时 = (double)精车工时 * (double)单轧工时系数 * (double)1;
//        }
//        if (工序名称 == "钻孔")
//        {
//            var 钻孔工时 = Convert.ToDouble(pScm.Cell("钻孔工时"));
//            var 单轧工时系数 = Convert.ToDouble(dScm_1.Cell("单轧工时系数"));
//            所需工时 = (double)钻孔工时 * (double)单轧工时系数 * (double)1;
//        }
//        return 所需工时;
//    }

//}

public class ProductTime
{
    private Schema pScm;
    private Schema idScm;
    private Schema dScm;
    private Schema dScm_1;

    public string OrderNum { get; set; }   
    public string SpecNum { get; set; }
    public string PID { get; set; } 


    double _OutsideDiameter;
    public double OutsideDiameter
    {
        get { return _OutsideDiameter; }
        set { _OutsideDiameter = value; }
    }
    double _Dust;
    public double Dust
    {
        get { return _Dust; }
        set { _Dust = value; }
    }

    double _FourSmoothTime;
    public double FourSmoothTime
    {
        get { return _FourSmoothTime; }
        set { _FourSmoothTime = value; }
    }

    static ProductTime()
    {

    }

    public ProductTime(H3.IEngine Engine)
    {
        if (pScm == null) { pScm = new Schema(Engine, "6b62f7decd924e1e8713025dc6a39aa5"); } //产品参数表:6b62f7decd924e1e8713025dc6a39aa5  6b62f7decd924e1e8713025dc6a39aa5                                                                                          
        if (idScm == null) { idScm = new Schema(Engine, "Szlywopbivyrv1d64301ta5xv4"); }     //产品ID表--工序计划表：Szlywopbivyrv1d64301ta5xv4；锯切表:ba65c69eb46c4156b0eb25930bf50b93 
        if (dScm == null) { dScm = new Schema(Engine, "5ed7e837ecee4f97800877820d9a2f05"); } //设备工时系数表
        if (dScm_1 == null) { dScm_1 = new Schema(Engine, dScm.Columns["子表"], true); }     //设备工时系数表子表
        //if(device == null) { device = new Schema(Engine, "D000886SBTZ", true); }          //设备档案 D000886SBTZ
    }

    public double GetTime(string 产品编号, string 工件号, string 设备类型, string 工序名称)
    {
        var psn = 产品编号.Split('-');
        return GetTime(psn[0], psn[1], 工件号, 设备类型, 工序名称);
    }

    public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称)
    {
        idScm.ClearFilter()
            .AndFilter("订单号", "=", 订单号)
            .AndFilter("规格号", "=", 规格号)
            .AndFilter("工件号", "=", 工件号)
            .GetFirst(true);
        var 胚体类型 = idScm.Cell("轧制方式");
        return GetTime(订单号, 规格号, 工件号, 设备类型, 工序名称, 胚体类型);
    }

    public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称, string 胚体类型)
    {
        double 所需工时 = 0;
        pScm.ClearFilter()
            .AndFilter("订单号", "=", 订单号)
            .AndFilter("规格号", "=", 规格号)
            .GetFirst(true);

        var test = pScm;

        var 产品类别 = pScm.Cell("产品类别");
        var 产品小类 = pScm.Cell("产品小类");
        var 外径 = pScm.Cell("外径");
        OutsideDiameter = Convert.ToDouble(外径);


        if (工序名称 == "钻孔")
        {
            dScm.ClearFilter()
                .AndFilter("工序名称", "=", 工序名称)
                .AndFilter("产品小类", "=", 产品小类)
                .GetFirst(true);
        }
        else
        {
            dScm.ClearFilter()
                .AndFilter("工序名称", "=", 工序名称)
                .AndFilter("产品类别", "=", 产品类别)
                .GetFirst(true);
        }

        test = dScm;

        if (工序名称 == "钻孔")
        {
            dScm_1.ClearFilter()
                .AndFilter(Schema.PID, "=", dScm.Cell(Schema.RID))
                .AndFilter("设备类型", "=", 设备类型)
                .GetFirst(true);
        }
        else
        {
            dScm_1.ClearFilter()
                .AndFilter(Schema.PID, "=", dScm.Cell(Schema.RID))
                .AndFilter("设备类型", "=", 设备类型)
                .AndFilter("外径下限", "<", 外径)
                .AndFilter("外径上限", ">", 外径)
                .GetFirst(true);

        }

        if (工序名称 == "粗车")
        {
            var 单轧粗车工时 = Convert.ToDouble(pScm.Cell("单轧粗车工时"));
            var 双轧粗车工时 = Convert.ToDouble(pScm.Cell("双轧粗车工时"));
            var 单轧工时系数 = Convert.ToDouble(dScm_1.Cell("单轧工时系数"));
            var 双轧工时系数 = Convert.ToDouble(dScm_1.Cell("双轧工时系数"));
            所需工时 = (double)(胚体类型 == "单轧" ? 单轧粗车工时 : 双轧粗车工时) * (double)(胚体类型 == "单轧" ? 单轧工时系数 : 双轧工时系数) * (double)1;

            var d = Convert.ToDouble(pScm.Cell("单轧粗车下屑"));
            var s = Convert.ToDouble(pScm.Cell("双轧粗车下屑"));
            _Dust = (胚体类型 == "单轧" ? d : s);
            _FourSmoothTime = Convert.ToDouble(pScm.Cell("四面见光粗车工时"));

        }
        if (工序名称 == "精车")
        {
            var 精车工时 = Convert.ToDouble(pScm.Cell("精车工时"));
            var 单轧工时系数 = Convert.ToDouble(dScm_1.Cell("单轧工时系数"));
            所需工时 = (double)精车工时 * (double)单轧工时系数 * (double)1;
            _Dust = Convert.ToDouble(pScm.Cell("精车下屑"));
        }
        if (工序名称 == "钻孔")
        {
            var 钻孔工时 = Convert.ToDouble(pScm.Cell("钻孔工时"));
            var 单轧工时系数 = Convert.ToDouble(dScm_1.Cell("单轧工时系数"));
            所需工时 = (double)钻孔工时 * (double)单轧工时系数 * (double)1;
            _Dust = Convert.ToDouble(pScm.Cell("钻孔下屑"));
        }
        return 所需工时;
    }

}