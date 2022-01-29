using System;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;






/*
*author:zhanglimin
*工时计算类
*
*/
public class ProductManHour
{

    private H3.IEngine engine;
    private H3.DataModel.BizObject ProductParametersBizobject; //产品参数表   
    private H3.DataModel.BizObject deviceHoursBizobject; //设备工时系数表
    private H3.DataModel.BizObject deviceHourSubTableBizObject; //设备工时系数表子表
    private H3.DataModel.BizObject processFlowBizObject; //工艺流程表
    private bool IsPlan = false;

    //外径
    private double _OutsideDiameter;
    public double OutsideDiameter
    {
        get { return _OutsideDiameter; }
        set { _OutsideDiameter = value; }

    }
    // 孔径
    private double _HoleDiameter;
    public double HoleDiameter
    {
        get { return _HoleDiameter; }
        set { HoleDiameter = value; }
    }

    //下屑量
    private double _Dust;
    public double Dust
    {
        get { return _Dust; }
        set { _Dust = value; }
    }
    //本工序产品工时
    private double _ProcessManHour;
    public double ProcessManHour
    {
        get { return _ProcessManHour; }
        set { _ProcessManHour = value; }

    }

    public ProductManHour(H3.IEngine Engine)
    {
        engine = Engine;
    }




    /*
*author:zhanglimin
*根据计划阶段或加工阶段取得不同的工时
*@param:processName 工序名称
*@param : id 产品 ID
*@param: deviceType 设备类型
*@Param: isPlan 是否计划内的请求
*/
    public double GetTime(string processName, string id, string deviceType, bool isPlan)
    {
        IsPlan = isPlan;
        var idStructItem = id.Split('-');
        string orderNumber = idStructItem[0];
        string specificationNumber = idStructItem[2];
        string workpieceNumber = idStructItem[3];
        string blankType = GetBlankType(isPlan, id);
        return GetTime(orderNumber, specificationNumber, workpieceNumber, deviceType, processName, blankType);
    }

    //取得轧制方式，如果是计划取得计划的轧制方式，如果不是计划则取得确认后的轧制方式
    private string GetBlankType(bool isPlan, string id)
    {
        string resultblankType = null;
        string fieldIDCode = ABCDProcessPlan.ID;
        string schemacode = ABCDProcessPlan.TableCode;

        if (!isPlan)
        {
            fieldIDCode = ProcessFlow.ID;
            schemacode = ProcessFlow.TableCode;

        }
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, fieldIDCode, H3.Data.ComparisonOperatorType.Equal, id);
        H3.DataModel.BizObject filterFlowBizObject = Tools.BizOperation.GetFirst(engine, schemacode, filter);
        //坯体类型，是双轧坯体，还是单轧坯体

        if (filterFlowBizObject == null)
        {
            // if(IsPlan)
            // {
            //     return null;
            // }
            throw (new Exception("指定的查询条件不能取得结果"));
        }
        else
        {
            if (!isPlan)
            {
                resultblankType = filterFlowBizObject[ProcessFlow.DeterminedRollingMethod].ToString();
            }
            else
            {
                resultblankType = filterFlowBizObject[ABCDProcessPlan.PlannedRollingMethod].ToString();
            }

        }
        return resultblankType;
    }


    //根据轧制方式取得不同工序的工时
    private double GetTime(string orderNumber, string specificationNumber, string workpieceNumber, string deviceType, string processName, string blankType)
    {
        double ResultmanHour = 0.0;
        try
        {
            string orderSpecificationNumber = orderNumber + '-' + specificationNumber;
            H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
            Tools.Filter.And(filter, ProductParameter.OrderNumber, H3.Data.ComparisonOperatorType.Equal, orderNumber);
            Tools.Filter.And(filter, ProductParameter.SpecificationNumber, H3.Data.ComparisonOperatorType.Equal, specificationNumber);
            Tools.Filter.Or(filter, ProductParameter.OrderSpecificationNumber, H3.Data.ComparisonOperatorType.Equal, orderSpecificationNumber);
            ProductParametersBizobject = Tools.BizOperation.GetFirst(engine, ProductParameter.TableCode, filter);
            string productMachiningCategory = null;
            string productDrillingCategory = null;
            if (ProductParametersBizobject != null)
            {
                //车加工类别
                productMachiningCategory = ProductParametersBizobject[ProductParameter.ProductMachiningCategory] + string.Empty;
                //钻加工类别
                productDrillingCategory = ProductParametersBizobject[ProductParameter.ProductDrillingCategory] + string.Empty;
                //外径
                _OutsideDiameter = Convert.ToDouble(ProductParametersBizobject[ProductParameter.OuterDiameter] + string.Empty);
                //孔径
                _HoleDiameter = Convert.ToDouble(ProductParametersBizobject[ProductParameter.HoleDiameter] + string.Empty);


            }
            else
            {
                // if(IsPlan)
                // {
                //     return 0;
                // }
                throw new Exception("产品产数表没有所需的数据");
            }

            bool isDrillHole = (processName == "Drill");
            string machiningTypeCode = isDrillHole ? DeviceWorkingHour.ProductDrillingCategory : DeviceWorkingHour.ProductMachiningCategory;
            string machiningTypeValue = isDrillHole ? productDrillingCategory : productMachiningCategory;
            string tprocessName = processName == "四面光" ? "粗车" : processName;

            H3.Data.Filter.Filter devFilter = new H3.Data.Filter.Filter();
            Tools.Filter.And(devFilter, DeviceWorkingHour.OperationName, H3.Data.ComparisonOperatorType.Equal, tprocessName);

            Tools.Filter.And(devFilter, machiningTypeCode, H3.Data.ComparisonOperatorType.Equal, machiningTypeValue);
            deviceHoursBizobject = Tools.BizOperation.GetFirst(engine, DeviceWorkingHour.TableCode, devFilter);
            if (deviceHoursBizobject == null)
            {
                // if(IsPlan)
                // {
                //     return 0;
                // }
                throw new Exception("给定查询条件在\"设备工时系数表\"内无记录:" + machiningTypeCode + machiningTypeValue);
            }

            H3.Data.Filter.Filter DeviceHourSubFilter = new H3.Data.Filter.Filter();
            var debugdeviceHoursBizobject = deviceHoursBizobject;

            var debug1 = deviceHoursBizobject[DeviceWorkingHour.ObjectId];
            Tools.Filter.And(DeviceHourSubFilter,
                //设备工时系数子表.父表的Objectid
                EquipmentTimeCoefficientSubtabulation.Parentobjectid,
                H3.Data.ComparisonOperatorType.Equal,
                //设备工时系数表.Objectid           
                deviceHoursBizobject[DeviceWorkingHour.ObjectId] + string.Empty);
            var debugDeviceType = deviceType;
            Tools.Filter.And(DeviceHourSubFilter,
                //设备工时系数子表.设备类型
                EquipmentTimeCoefficientSubtabulation.EquipmentType,
                H3.Data.ComparisonOperatorType.Equal, deviceType);
            // 设备工时系数子表.孔径下限                                                //设备工时系数子表.外径下限
            string Lowerlimit = isDrillHole ? EquipmentTimeCoefficientSubtabulation.LowerApertureLimit : EquipmentTimeCoefficientSubtabulation.LowerOuterDiameterLimit;
            double diameter = isDrillHole ? _HoleDiameter : _OutsideDiameter;
            Tools.Filter.And(DeviceHourSubFilter, Lowerlimit, H3.Data.ComparisonOperatorType.Below, diameter);
            // 设备工时系数子表.孔径上限                                               //设备工时系数子表.外径下限
            string upperlimit = isDrillHole ? EquipmentTimeCoefficientSubtabulation.UpperApertureLimit : EquipmentTimeCoefficientSubtabulation.UpperOuterDiameterLimit;
            double upperlimitvalue = isDrillHole ? _HoleDiameter : _OutsideDiameter;
            Tools.Filter.And(DeviceHourSubFilter, upperlimit, H3.Data.ComparisonOperatorType.NotBelow, upperlimitvalue);

            deviceHourSubTableBizObject = Tools.BizOperation.GetFirst(engine, EquipmentTimeCoefficientSubtabulation.TableCode, DeviceHourSubFilter);

            if (deviceHourSubTableBizObject == null) { throw new Exception("设备工时系数表不能找到对应的数据"); }



            bool isSingleRolling = (blankType == "单轧");
            if (processName == "四面光")
            {   //工时
                var fourLightManHour = Convert.ToDouble(ProductParametersBizobject[ProductParameter.FoursidesRoughingManHour]);
                _ProcessManHour = ResultmanHour;
                //同粗车工时系数
                var manHourCoefficient = Convert.ToDouble(deviceHourSubTableBizObject[isSingleRolling ? EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient : EquipmentTimeCoefficientSubtabulation.DoubleRollingManHourCoefficient]);
                ResultmanHour = (double)fourLightManHour * (double)manHourCoefficient;

                //粗车下屑量
                var RoughTurningChip = isSingleRolling ? ProductParameter.SingleRollingRoughTurningChip : ProductParameter.DoubleRollingRoughingChip;
                //四面光的下屑量不等于粗车的下屑量
                // _Dust = Convert.ToDouble(ProductParametersBizobject[RoughTurningChip]);
                _Dust = 0;
            }


            else if (processName == "粗车")
            {
                //粗车工时
                var roughManHour = Convert.ToDouble(ProductParametersBizobject[isSingleRolling ? ProductParameter.SingleRoughingMaNHour : ProductParameter.DoubleRoughingManhour]);
                _ProcessManHour = roughManHour;

                var debugT1 = deviceHourSubTableBizObject;

                //粗车工时系数           
                var manHourCoefficient = Convert.ToDouble(deviceHourSubTableBizObject[isSingleRolling ? EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient : EquipmentTimeCoefficientSubtabulation.DoubleRollingManHourCoefficient]);
                ResultmanHour = (double)roughManHour * (double)manHourCoefficient;


                var d = isSingleRolling ? ProductParameter.SingleRollingRoughTurningChip : ProductParameter.DoubleRollingRoughingChip;
                _Dust = Convert.ToDouble(ProductParametersBizobject[d]);

            }
            else if (processName == "精车")
            {
                var finishManhour = Convert.ToDouble(ProductParametersBizobject[ProductParameter.FinishingManHour]);
                _ProcessManHour = finishManhour;


                //精车工时系数           
                var singleRollingManHourCoefficient = Convert.ToDouble(deviceHourSubTableBizObject[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient]);
                ResultmanHour = (double)finishManhour * (double)singleRollingManHourCoefficient;

                _Dust = Convert.ToDouble(ProductParametersBizobject[ProductParameter.FinishingChip]);

            }
            else if (processName == "钻孔")
            {
                var drillManhour = Convert.ToDouble(ProductParametersBizobject[ProductParameter.DrillingManHour]);
                _ProcessManHour = drillManhour;

                //钻孔工时系数           
                var singleRollingManHourCoefficient = Convert.ToDouble(deviceHourSubTableBizObject[EquipmentTimeCoefficientSubtabulation.SingleRollingManHourCoefficient]);
                ResultmanHour = (double)drillManhour * (double)singleRollingManHourCoefficient;


                _Dust = Convert.ToDouble(ProductParametersBizobject[ProductParameter.DrillingChip]);

            }
        }

        catch (Exception e)
        {
            throw e;
        }

        return ResultmanHour;

    }
}
