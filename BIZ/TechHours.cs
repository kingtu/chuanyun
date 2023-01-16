
using System;
using System.Data;
using System.Collections.Generic;

public class TechAPI : H3.SmartForm.RestApiController
{

    //此构造方法必须有，但是方法体内不需要写任何代码
    public TechAPI(H3.SmartForm.RestApiRequest request) : base(request) { }
    //第三方请求本自定义接口时，会触发本事件
    protected override void OnInvoke(string actionName, H3.SmartForm.RestApiResponse response)
    {
        try
        {

            //“CreateOneData”可以为任意非空字符串，用来判断第三方请求的动作
            /*注：actionName   不能为已公开的ActionName，如下：LoadBizObject  加载业务数据;LoadBizObjects  批量加载业务数据;CreateBizObject  创建业务数据
             CreateBizObjects 批量创建业务数据;UpdateBizObject  更新业务数据;RemoveBizObject 删除业务数据
            */
            if (actionName == "GetHours")
            {

                //从传入的参数里获取 key为“para1” 的值，若未传，则得到默认值“defaultValue”，若传的值无法转换成string类型，则报错
                string stringValue = this.Request.GetValue<string>("para1", "defaultValue");
                //从传入的参数里获取 key为“para2” 的值，若未传，则得到默认值0，若传的值无法转换成int类型，则报错
                int intValue = this.Request.GetValue<int>("para2", 0);

                //当找到了对应的actionName时，回复给第三方请求一个 key为“result”值为“success” 的结果
                response.ReturnData.Add("result", "success");
                //当找到了对应的actionName时，回复给第三方请求一个 key为“message”值为空字符串 的结果
                response.ReturnData.Add("message", string.Empty);
                //以下代码演示在自定义接口中查询业务对象的范例，如实际用不上请删除
                H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
                H3.Data.Filter.And andMatcher = new H3.Data.Filter.And();
                andMatcher.Add(new H3.Data.Filter.ItemMatcher("Status", H3.Data.ComparisonOperatorType.Equal, 1));//筛选出数据状态为生效的数据
                filter.Matcher = andMatcher;
                // H3.DataModel.BizObjectSchema   schema = this.Request.Engine.BizObjectManager.GetPublishedSchema("D00021MyApiTest");
                // //查询“D00021MyApiTest”表单里所有符合筛选条件的业务对象，得到一个业务对象数组
                // //注：H3.Organization.User.SystemUserId   为系统默认用户Id，在定时器中、自定义接口中由于没有当前登录人，所以用这个代替this.Request.UserContext.UserId
                // H3.DataModel.BizObject[] arr = H3.DataModel.BizObject.GetList(this.Request.Engine, H3.Organization.User.SystemUserId,
                //     schema, H3.DataModel.GetListScopeType.GlobalAll, filter);

                Dictionary<string, object> dic = new Dictionary<string, object>();
                //从第三方传入参数里拿出传回给第三方
                dic.Add("StringValue", stringValue);
                //传回给第三方一个小数
                dic.Add("DoubleValue", 5.24343);
                //从第三方传入参数里拿出传回给第三方
                dic.Add("ArrayValue", new int[4] { intValue, intValue + 1, intValue - 2, intValue + 3 });
                //回复给第三方请求一个 key为“data”值为对象 的结果
                response.ReturnData.Add("data", dic);

            }
            else
            {
                //当传入一个未找到的actionName时，回复给第三方请求一个 key为“result”值为“error” 的结果
                response.ReturnData.Add("result", "error");
                //当传入一个未找到的actionName时，回复给第三方请求一个 key为“message”值为字符串 的异常提示
                response.ReturnData.Add("message", "无法处理actionName为“" + actionName + "”的请求！");
            }
        }
        catch (Exception ex)
        {
            //当执行的代码发生异常时，回复给第三方请求一个 key为“result”值为“error”   的结果
            response.ReturnData.Add("result", "error");
            //当执行的代码发生异常时，回复给第三方请求一个 key为“message”值为捕获到的异常原因
            response.ReturnData.Add("message", ex.Message);
        }
    }


}
public class TechHours//: H3.SmartForm.RestApiController
{
    /*查询工时和下屑量
    * @param engine 引擎
    * @param OperationName 工序名
    * @param productObj 产品参数表业务对象
    * @param rollingMode 轧制方式
    * @param equipmentCoeff  设备工时系数
    * @param quantity  派工/加工量
    **/
    public Dictionary<string, double> QueryTechHoursAndFilings(H3.IEngine engine, string OperationName,
        H3.DataModel.BizObject productParameterObj, string rollingMode, double equipmentCoeff, double quantity = 1)
    {
        double techChip = 0;        //工艺下屑量       
        double noDeviceHours = 0;   //无设备工时       
        //工时和下屑量
        Dictionary<string, double> techHoursAndChip = new Dictionary<string, double>();
        if (productParameterObj != null)
        {
            if (OperationName == "粗车")
            {
                //产品参数表-单轧工时
                if (rollingMode == "单轧" && productParameterObj[ProductParameter_SingeRollingTechHours] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"单轧粗车无设备工时"
                    noDeviceHours = Convert.ToDouble(productParameterObj[ProductParameter_SingeRollingTechHours]);
                    //单轧粗车工艺下屑量
                    techChip = Convert.ToDouble(productParameterObj[ProductParameter_SingleRollingRougingFillingWeight]);
                }
                //产品参数表-双轧工时
                if (rollingMode == "双轧" && productParameterObj[ProductParameter_DoubleRoughingTechHours] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"双轧粗车无设备工时"
                    noDeviceHours = Convert.ToDouble(productParameterObj[ProductParameter_DoubleRoughingTechHours]);
                    //双轧粗车工艺下屑量
                    techChip = Convert.ToDouble(productParameterObj[ProductParameter_DoubleRollingRoughingFillingWeight]);
                }
            }
            if (OperationName == "精车")
            {
                //产品参数表-单轧工时
                if (rollingMode == "单轧" && productParameterObj[ProductParameter_FinishingTechHours] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"精车无设备工时"
                    noDeviceHours = Convert.ToDouble(productParameterObj[ProductParameter_FinishingTechHours]);
                    //精车工艺下屑量
                    techChip = Convert.ToDouble(productParameterObj[ProductParameter_FinishingFillingWeight]);
                }
            }
            if (OperationName == "钻孔")
            {
                //产品参数表-单轧工时
                if (rollingMode == "单轧" && productParameterObj[ProductParameter_DrillingTechHours] != null)
                {   //根据本表单产品轧制方式从产品参数表中获取"钻孔无设备工时"
                    noDeviceHours = Convert.ToDouble(productParameterObj[ProductParameter_DrillingTechHours]);
                    //钻孔工艺下屑量
                    techChip = Convert.ToDouble(productParameterObj[ProductParameter_DrillingFillingWeight]);
                }
            }
        }
        techHoursAndChip.Add("NoDeviceTechHours", noDeviceHours);                           //本工序产品无设备工时
        techHoursAndChip.Add("techHours", noDeviceHours * equipmentCoeff);                  //工时        
        techHoursAndChip.Add("StageTechHours", noDeviceHours * equipmentCoeff * quantity);  //派工or派工任务工时
        techHoursAndChip.Add("FillingWeight", techChip * quantity);                         //任务下销量       

        return techHoursAndChip;
    }
    /*
    * 查询设备工时系数
    * @param engine 引擎
    * @param OperationName 工序名
    * @param deviceType 设备类型
    * @param refParameterForm 产品参数表数据Id
    * @param processingQuantity 派工量
    * @param rollingMode 轧制方式
    *return 设备工时系数  
    */
    public double EquipmentTechHoursCoeff(H3.IEngine engine, string OperationName, string deviceType, H3.DataModel.BizObject productParameterObj, string rollingMode)
    {
        double equipmentCoeff = 1;                      //设备工时系数        
        double strOuterDiameter = 0;                    //本工序外径 
        double drilDiameter = 0;                        //产品孔径 

        H3.DataModel.BizObject[] subObj = null;         //设备工时系数表-子表       
        string productType = productParameterObj[ProductParameter_MachiningProcessingCategory] + string.Empty;          //车加工类别       
        string ProductDrillingType = productParameterObj[ProductParameter_DrillingProcessingCategory] + string.Empty;   //钻加工类别        
        string processingCategory = DeviceTechHours_MachiningProcessingCategory;                                        //加工类别编码
        //获取外径
        if (productParameterObj != null)
        {
            strOuterDiameter = Convert.ToDouble(productParameterObj[ProductParameter.OuterDiameter]);
            strOuterDiameter = Convert.ToDouble(productParameterObj[ProductParameter.OuterDiameter]);
        }
        if (OperationName == "钻孔")
        {
            productType = ProductDrillingType;
            processingCategory = DeviceTechHours_DrillingProcessingCategory;

        }
        //车加工类别
        if (productType != string.Empty)
        {
            //获取设备工时系数模块
            string s = @"";
            string command = string.Format("Select {0} From i_{1} Where {2} = '{3}' and {4} = '{5}'",
                DeviceTechHours_ObjectId, DeviceTechHours_TableCode, DeviceTechHours_OperationName,
                OperationName, processingCategory, productType);
            DataTable data = engine.Query.QueryTable(command, null);
            if (data != null && data.Rows != null && data.Rows.Count > 0)
            {
                H3.DataModel.BizObject mtObj = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId,
                    engine, DeviceTechHours_TableCode, data.Rows[0][DeviceTechHours_ObjectId] + string.Empty, true);
                if (mtObj == null)
                {
                    throw new Exception("未找到设备工时相关数据！");
                }
                //设备工时系数表-子表
                subObj = mtObj[DeviceTechHoursSubTable_TableCode] as H3.DataModel.BizObject[];
                if (subObj == null)
                {
                    throw new Exception("未找到设备工时系数相关数据！");
                }
            }
        }
        // 设备工时系数表-子表
        if (subObj != null)
        {
            foreach (H3.DataModel.BizObject item in subObj)
            {
                if (deviceType != string.Empty)
                {        //按设备类型查找
                    if (item[DeviceTechHoursSubTable_DeviceType] + string.Empty == deviceType)
                    {
                        if (OperationName == "钻孔")
                        {
                            //按外径上下限匹配
                            if (strOuterDiameter > Convert.ToDouble(item[DeviceTechHoursSubTable_LowerOuterDiameterLimit]) &&
                                strOuterDiameter <= Convert.ToDouble(item[DeviceTechHoursSubTable_UpperOuterDiameterLimit]))
                            {
                                if (rollingMode == "单轧" && item[DeviceTechHoursSubTable_SingleRollingTechHoursCoefficient] != null)
                                {   //单轧工时系数
                                    equipmentCoeff = Convert.ToDouble(item[DeviceTechHoursSubTable_SingleRollingTechHoursCoefficient]);
                                }
                                else if (rollingMode == "双轧" && item[DeviceTechHoursSubTable_DoubleRollingTechHoursCoefficient] != null)
                                {   //双轧工时系数
                                    equipmentCoeff = Convert.ToDouble(item[DeviceTechHoursSubTable_DoubleRollingTechHoursCoefficient]);
                                }
                            }
                        }
                        else
                        {

                            //按外径上下限匹配
                            if (strOuterDiameter > Convert.ToDouble(item[DeviceTechHoursSubTable_LowerOuterDiameterLimit]) &&
                                strOuterDiameter <= Convert.ToDouble(item[DeviceTechHoursSubTable_UpperOuterDiameterLimit]) &&
                                OperationName == "钻孔" ? drilDiameter > Convert.ToDouble(item[DeviceTechHoursSubTable_LowerDrillDiameterLimit]) : true &&
                                    OperationName == "钻孔" ? drilDiameter <= Convert.ToDouble(item[DeviceTechHoursSubTable_UpperDrillDiameterLimit]) : true)
                            {
                                if (rollingMode == "单轧" && item[DeviceTechHoursSubTable_SingleRollingTechHoursCoefficient] != null)
                                {   //单轧工时系数
                                    equipmentCoeff = Convert.ToDouble(item[DeviceTechHoursSubTable_SingleRollingTechHoursCoefficient]);
                                }
                                else if (rollingMode == "双轧" && item[DeviceTechHoursSubTable_DoubleRollingTechHoursCoefficient] != null)
                                {   //双轧工时系数
                                    equipmentCoeff = Convert.ToDouble(item[DeviceTechHoursSubTable_DoubleRollingTechHoursCoefficient]);
                                }
                            }
                        }

                    }
                }
            }

        }
        return equipmentCoeff;
    }


 

    //产品参数表
    string ProductParameter_TableCode = "D0014196b62f7decd924e1e8713025dc6a39aa5";
    string ProductParameter_SingeRollingTechHours = "F0000048";             //单轧粗车工时   
    string ProductParameter_SingleRollingRougingFillingWeight = "F0000045"; //单轧粗车工艺下屑量   
    string ProductParameter_DoubleRoughingTechHours = "F0000049";           //双轧粗车工时   
    string ProductParameter_DoubleRollingRoughingFillingWeight = "F0000046";//双轧粗车工艺下屑量   
    string ProductParameter_FinishingTechHours = "F0000051";                //单轧精车工时   
    string ProductParameter_FinishingFillingWeight = "F0000047";            //单轧精车工艺下屑量   
    string ProductParameter_DrillingTechHours = "F0000051";                 //单轧钻孔工时   
    string ProductParameter_DrillingFillingWeight = "F0000047";             //单轧钻孔工艺下屑量   
    string ProductParameter_MachiningProcessingCategory = "F0000004";       //产品车加工类别    
    string ProductParameter_DrillingProcessingCategory = "F0000006";        //产品钻加工类别    
    string ProductParameter_OuterDiameter = "F0000076";                     //外径
    string ProductParameter_DrillDiameter = "F0000081"; //孔径  

    //设备工时系数表
    string DeviceTechHours_TableCode = "D0014195ed7e837ecee4f97800877820d9a2f05";
    string DeviceTechHours_MachiningProcessingCategory = "F0000002";         //车加工类别   
    string DeviceTechHours_DrillingProcessingCategory = "F0000003";          //钻加工类别   
    string DeviceTechHours_OperationName = "F0000001";                       //工序名称   
    string DeviceTechHours_ObjectId = "ObjectId";                            //objectID

    //设备工时系数表子表
    string DeviceTechHoursSubTable_TableCode = "D001419Fbb7854d117af4bba8eff4de46d128f63";
    string DeviceTechHoursSubTable_DeviceType = "F0000004";                 //设备类型    
    string DeviceTechHoursSubTable_LowerOuterDiameterLimit = "F0000013";    //外径下限 
    string DeviceTechHoursSubTable_UpperOuterDiameterLimit = "F0000012";    //外径上限  
    //孔径下限
    string DeviceTechHoursSubTable_LowerDrillDiameterLimit = "F0000010";
    //孔径上限
    string DeviceTechHoursSubTable_UpperDrillDiameterLimit = "F0000011";
    string DeviceTechHoursSubTable_SingleRollingTechHoursCoefficient = "F0000007";  //单轧工时系数   
    string DeviceTechHoursSubTable_DoubleRollingTechHoursCoefficient = "F0000008";  //双轧工时系数
}