using System;
using System.Collections.Generic;
using System.Text;

namespace Chuanyun.BIZ
{
    class Salary
    {
    }
}
public class Salary
{
    H3.IEngine Engine;
    string OrderNum;
    //string BathNum;
    string SpecNum;
    string PID;
    double Quantity;

    public double ProcessTime;

    double _OutsideDiameter;
    public double OutsideDiameter
    {
        get { return _OutsideDiameter; }
        set { _OutsideDiameter = value; }
    }

    public Salary() { }

    public Salary(H3.IEngine Engine, string OrderNum, string SpecNum, string PID)
    {
        this.Engine = Engine;
        this.OrderNum = OrderNum;
        this.SpecNum = SpecNum;
        this.PID = PID;

        //this.Quantity=Quantity;
    }
    public double GetSalaryOfCC(string DeviceType, string peity, double price = 28.0, double difficulty = 1.0)
    {

        var ptime = new ProductTime(this.Engine);
        ProcessTime = ptime.GetTime(OrderNum, SpecNum, PID, DeviceType, "粗车", peity);
        _OutsideDiameter = ptime.OutsideDiameter;
        return ProcessTime * difficulty * price;
        //public double GetTime(string 订单号, string 规格号, string 工件号, string 设备类型, string 工序名称,string 胚体类型 ) 
        //peity = peity.Split('-');
        //var psn = peity.Split('-');

    }
    public double GetSalaryOfJC(string DeviceType, double price = 28.0, double difficulty = 1.0)
    {
        var ptime = new ProductTime(this.Engine);
        ProcessTime = ptime.GetTime(OrderNum, SpecNum, PID, DeviceType, "精车", "单轧");
        _OutsideDiameter = ptime.OutsideDiameter;
        return ProcessTime * difficulty * price;
    }
    public double GetSalaryOfZK(string DeviceType, double price = 28.0, double difficulty = 1.0)
    {
        var ptime = new ProductTime(this.Engine);
        ProcessTime = ptime.GetTime(OrderNum, SpecNum, PID, DeviceType, "钻孔", "单轧");
        _OutsideDiameter = ptime.OutsideDiameter;
        return ProcessTime * difficulty * price;
    }

}