﻿using System;
using System.Collections.Generic;
using System.Text;


public class ProductTime
{
    private Schema pScm;
    private Schema idScm;
    private Schema dScm;
    private Schema dScm_1;

    double _OutsideDiameter;
    public double OutsideDiameter
    {
        get { return _OutsideDiameter; }
        set { _OutsideDiameter = value; }
    }
    double _Dust;//下屑
    public double Dust
    {
        get { return _Dust; }
        set { _Dust = value; }
    }

    double _FourSmoothTime;//四面光工时
    public double FourSmoothTime
    {
        get { return _FourSmoothTime; }
        set { _FourSmoothTime = value; }
    }

    public ProductTime(H3.IEngine Engine)
    {
        if (pScm == null) { pScm = new Schema(Engine, "产品参数表"); }
        if (idScm == null) { idScm = new Schema(Engine, "工序计划表"); } //产品ID表--工序计划表：Szlywopbivyrv1d64301ta5xv4；锯切表:ba65c69eb46c4156b0eb25930bf50b93 
        if (dScm == null) { dScm = new Schema(Engine, "设备工时系数表"); }
        if (dScm_1 == null) { dScm_1 = dScm.GetSubSchema("子表"); } //设备工时系数表子表       
    }

    public double GetTime(string 产品编号, string 工件号, string 设备类型, string 工序名称)
    {
        var psn = 产品编号.Split('-');
        return GetTime(psn[0], psn[1], 工件号, 设备类型, 工序名称);
    }

    public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称)
    {
        idScm.ClearFilter()
            .And("订单号", "=", 订单号)
            .And("规格号", "=", 规格号)
            .And("工件号", "=", 工件号)
            .GetFirst(true);
        var 胚体类型 = idScm.Cell("计划轧制方式");
        return GetTime(订单号, 规格号, 工件号, 设备类型, 工序名称, 胚体类型);
    }

    public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称, string 胚体类型)
    {
        double 所需工时 = 0;
        pScm.ClearFilter()
            .And("订单号", "=", 订单号)
            .And("规格号", "=", 规格号)
            .GetFirst(true);

        var test = pScm;

        var 产品车加工类别 = pScm.Cell("产品车加工类别");
        var 产品钻加工类别 = pScm.Cell("产品钻加工类别");
        var 外径 = pScm["外径"];
        var 孔径 = pScm["孔径"];
        OutsideDiameter = Convert.ToDouble(外径);
        bool isZK = 工序名称 == "钻孔";

        dScm.ClearFilter()
            .And("工序名称", "=", 工序名称 == "四面光" ? "粗车" : 工序名称)
            .And(isZK ? "产品钻加工类别" : "产品车加工类别", "=", isZK ? 产品钻加工类别 : 产品车加工类别)
            .GetFirst(true);

        test = dScm;
        dScm_1.ClearFilter()
            .And(Schema.PID, "=", dScm.Cell(Schema.RID))
            .And("设备类型", "=", 设备类型)
            .And(isZK ? "孔径下限" : "外径下限", "<", isZK ? 孔径 : 外径)
            .And(isZK ? "孔径上限" : "外径上限", ">=", isZK ? 孔径 : 外径)
            .GetFirst(true);

        bool isDZ = 胚体类型 == "单轧";

        if (工序名称 == "四面光")
        {
            所需工时 = Convert.ToDouble(pScm["四面见光粗车工时"]);
            _FourSmoothTime = Convert.ToDouble(pScm["四面见光粗车工时"]);

            var d = isDZ ? "单轧粗车下屑" : "双轧粗车下屑";
            _Dust = Convert.ToDouble(pScm[d]);
        }
        else if (工序名称 == "粗车")
        {
            var 粗车工时 = Convert.ToDouble(pScm[isDZ ? "单轧粗车工时" : "双轧粗车工时"]);
            var 工时系数 = Convert.ToDouble(dScm_1[isDZ ? "单轧工时系数" : "双轧工时系数"]);

            所需工时 = (double)粗车工时 * (double)工时系数;
            _FourSmoothTime = Convert.ToDouble(pScm["四面见光粗车工时"]);
            var d = isDZ ? "单轧粗车下屑" : "双轧粗车下屑";
            _Dust = Convert.ToDouble(pScm[d]);

        }
        else if (工序名称 == "精车")
        {
            var 精车工时 = Convert.ToDouble(pScm["精车工时"]);
            var 单轧工时系数 = Convert.ToDouble(dScm_1["单轧工时系数"]);
            所需工时 = (double)精车工时 * (double)单轧工时系数;
            _Dust = Convert.ToDouble(pScm["精车下屑"]);
        }
        else if (工序名称 == "钻孔")
        {
            var 钻孔工时 = Convert.ToDouble(pScm["钻孔工时"]);
            var 单轧工时系数 = Convert.ToDouble(dScm_1["单轧工时系数"]);
            所需工时 = (double)钻孔工时 * (double)单轧工时系数;
            _Dust = Convert.ToDouble(pScm["钻孔下屑"]);
        }
        return 所需工时;
    }

}