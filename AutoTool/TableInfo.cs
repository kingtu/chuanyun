//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Chuanyun.AutoTool
//{
//    class TableInfo
//    {
//    }
//}

using System;
using System.Collections.Generic;
using System.Text;
using H3;


/// <summary>
/// 生产外协管理,外协合同
/// </summary>
[Table("外协合同")]
public class OutsourcingContract
{
    public static readonly string TableCode = "39df0a783f8d4124bb9ee8119e4e1147";
    public OutsourcingContract() { }
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //锻造工艺
    public static readonly string ForgingProcess = "F0000007";
    //甲方
    public static readonly string PartyA = "F0000002";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //产品列表
    public static readonly string ProductList = "D001419Faebf741cae044316a15f4db3db5ebc18";
    //Status
    public static readonly string Status = "Status";
    //数据标题
    public static readonly string Name = "Name";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //乙方
    public static readonly string PartyB = "F0000004";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //数据代码
    public static readonly string DataCode = "F0000008";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //合同编号
    public static readonly string ContractNumber = "F0000003";
    //签订时间
    public static readonly string SigningTime = "F0000005";
}
/// <summary>
/// 生产外协管理,外协流程
/// </summary>
[Table("外协流程")]
public class Outsource
{
    public static readonly string TableCode = "Spznzknfgeoyf3x14nmw70r7h2";
    public Outsource() { }
    //不平
    public static readonly string Unevenness = "F0000024";
    //外协工序1
    public static readonly string OutsourcingProcedure1 = "F0000089";
    //外协合同2
    public static readonly string OutsourcingContract2 = "F0000087";
    //椭圆
    public static readonly string Ellipse = "F0000021";
    //外协合同1
    public static readonly string OutsourcingContract1 = "F0000086";
    //数据标题
    public static readonly string Name = "Name";
    //外协合同3
    public static readonly string OutsourcingContract3 = "F0000088";
    //当前工步
    public static readonly string CurrentStep = "F0000078";
    //发出工序
    public static readonly string SendingOperation = "F0000005";
    //工艺流程表
    public static readonly string ProcessFlowTable = "F0000048";
    //Status
    public static readonly string Status = "Status";
    //工件号
    public static readonly string WorkpieceNumber = "F0000004";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000010";
    //内径
    public static readonly string InnerDiameter = "F0000019";
    //发出单位
    public static readonly string SendingCompany = "F0000006";
    //检验结果
    public static readonly string InspectionResults = "F0000025";
    //外协工序3
    public static readonly string OutsourcingProcedure3 = "F0000091";
    //接收工序
    public static readonly string ReceivingOperation = "F0000009";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000003";
    //ID
    public static readonly string ID = "F0000011";
    //当前工序
    public static readonly string CurrentOperation = "F0000079";
    //总高
    public static readonly string TotalHeight = "F0000023";
    //外协状态
    public static readonly string OutsourcingStatus = "F0000028";
    //标题
    public static readonly string Title = "F0000017";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //其他物流数据分类
    public static readonly string OtherLogisticsDataClassifications = "F0000092";
    //片厚
    public static readonly string SheetThickness = "F0000022";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //外协工序2
    public static readonly string OutsourcingProcedure2 = "F0000090";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //咀内径
    public static readonly string NozzleInnerDiameter = "F0000020";
    //数据代码
    public static readonly string DataCode = "F0000077";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000002";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //外径
    public static readonly string OuterDiameter = "F0000018";
    //供应商
    public static readonly string Supplier = "F0000013";
    //接收单位
    public static readonly string ReceivingCompany = "F0000008";
    //审核
    public static readonly string Review = "F0000027";
    //检验员
    public static readonly string Inspector = "F0000026";
    //订单号
    public static readonly string OrderNumber = "F0000001";
}
/// <summary>
/// 市场需求,A销售订单表
/// </summary>
[Table("A销售订单表")]
public class Order
{
    public static readonly string TableCode = "Shla6mrsjywq2pl57mjs5x80y4";
    public Order() { }
    //塔筒动态
    public static readonly string TowerDynamics = "F0000046";
    //第三地址
    public static readonly string ThirdAddress = "F0000069";
    //订单回款情况
    public static readonly string OrderPaymentCollection = "F0000047";
    //客户编号
    public static readonly string CustomerNumber = "F0000061";
    //项目编号
    public static readonly string ProjectNumber = "F0000062";
    //塔筒调查
    public static readonly string TowerSurvey = "F0000054";
    //项目调查
    public static readonly string ProjectSurvey = "F0000031";
    //主机客户
    public static readonly string HostCustomer = "F0000005";
    //项目名称
    public static readonly string ProjectName = "F0000020";
    //默认联系人
    public static readonly string DefaultContact = "F0000052";
    //Status
    public static readonly string Status = "Status";
    //技术规范
    public static readonly string TechnicalSpecifications = "F0000050";
    //分公司名称
    public static readonly string BranchName = "F0000073";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //客户评级
    public static readonly string CustomerRating = "F0000042";
    //第二伙伴
    public static readonly string SecondPartner = "F0000006";
    //需求达成
    public static readonly string DemandFulfillment = "F0000064";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //订单总吨位
    public static readonly string TotalOrderTonnage = "F0000057";
    //第二地址
    public static readonly string SecondAddress = "F0000068";
    //生产完成
    public static readonly string ProductionComplete = "F0000065";
    //订单履行情况
    public static readonly string OrderFulfillment = "F0000010";
    //发运完成
    public static readonly string ShipmentCompletion = "F0000036";
    //执行日期
    public static readonly string ExecutionDate = "F0000018";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //默认地址
    public static readonly string DefaultAddress = "F0000063";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //提议备注历史
    public static readonly string ProposalRemarksHistory = "F0000070";
    //数据代码
    public static readonly string DataCode = "F0000041";
    //供应压力
    public static readonly string SupplyPressure = "F0000060";
    //承诺人
    public static readonly string Promisor = "F0000072";
    //订单吨单价
    public static readonly string OrderTonUnitPrice = "F0000058";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //订单总价
    public static readonly string TotalOrderPrice = "F0000056";
    //合同编号
    public static readonly string ContractNumber = "F0000048";
    //订单号
    public static readonly string OrderNumber = "F0000028";
    //付款交货条款
    public static readonly string PaymentAndDeliveryTerms = "F0000059";
    //签订日期
    public static readonly string SigningDate = "F0000017";
    //战略意义
    public static readonly string StrategicSignificance = "F0000022";
    //订单套数
    public static readonly string OrderSets = "F0000055";
    //项目经理
    public static readonly string ProjectManager = "F0000049";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //关联客户表
    public static readonly string AssociatedCustomerTable = "F0000032";
    //订单数
    public static readonly string NumberOfOrders = "F0000043";
    //项目动态
    public static readonly string ProjectDynamics = "F0000045";
    //总公司名称
    public static readonly string HeadOfficeName = "F0000033";
    //数据标题
    public static readonly string Name = "Name";
    //承诺备注历史
    public static readonly string CommitmentRemarksHistory = "F0000071";
    //订单评级
    public static readonly string OrderRating = "F0000044";
    //默认电话
    public static readonly string DefaultTelephone = "F0000053";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
}
/// <summary>
/// 市场需求,AB订单批次表
/// </summary>
[Table("AB订单批次表")]
public class OrderBatch
{
    public static readonly string TableCode = "Sv3ey3zxy6sufw6mrqg0p3rv76";
    public OrderBatch() { }
    //合同需求套数
    public static readonly string NumberOfContractDemandSets = "F0000059";
    //默认发运地址
    public static readonly string DefaultShippingAddress = "F0000072";
    //成品需求套数
    public static readonly string NumberOfFinishedProductDemandSets = "F0000058";
    //Status
    public static readonly string Status = "Status";
    //订单付款情况
    public static readonly string OrderPaymentStatus = "F0000054";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //默认电话
    public static readonly string DefaultTelephone = "F0000084";
    //提议交期
    public static readonly string ProposedDeliveryDate = "F0000079";
    //生产提前天数
    public static readonly string ProductionAdvanceDays = "F0000055";
    //协商套数
    public static readonly string NegotiatedSets = "F0000066";
    //承诺交期
    public static readonly string CommittedDeliveryDate = "F0000081";
    //在产备注
    public static readonly string InProductionRemarks = "F0000067";
    //合同交期
    public static readonly string ContractDeliveryDate = "F0000048";
    //发运日期
    public static readonly string ShippingDate = "F0000023";
    //发运省份
    public static readonly string ShippingProvince = "F0000085";
    //发运详情
    public static readonly string ShippingDetails = "F0000076";
    //生产延期天数
    public static readonly string ProductionDelayDays = "F0000038";
    //塔筒动态
    public static readonly string TowerDynamics = "F0000063";
    //客户付款信誉
    public static readonly string CustomerPaymentReputation = "F0000050";
    //数据代码
    public static readonly string DataCode = "F0000042";
    //承诺数量
    public static readonly string CommittedQuantity = "F0000082";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //准时完工
    public static readonly string OntimeCompletion = "F0000041";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //发运完成
    public static readonly string ShippingCompletion = "F0000024";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //承诺备注
    public static readonly string CommitmentRemarks = "F0000078";
    //成品需求期
    public static readonly string FinishedProductDemandPeriod = "F0000016";
    //项目动态
    public static readonly string ProjectDynamics = "F0000062";
    //合同批次
    public static readonly string ContractBatch = "F0000071";
    //协商交期
    public static readonly string NegotiatedDeliveryDate = "F0000049";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //客户编号
    public static readonly string CustomerNumber = "F0000069";
    //提议数量
    public static readonly string ProposalQuantity = "F0000080";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //发运备注
    public static readonly string ShippingRemarks = "F0000068";
    //项目编号
    public static readonly string ProjectNumber = "F0000070";
    //默认联系人
    public static readonly string DefaultContact = "F0000083";
    //总公司名称
    public static readonly string HeadOfficeName = "F0000026";
    //数据标题
    public static readonly string Name = "Name";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //协商备忘
    public static readonly string NegotiationNotes = "F0000047";
    //发运区域
    public static readonly string ShippingRegion = "F0000022";
    //生产完成日期
    public static readonly string ProductionCompletionDate = "FinishDate";
    //生产完成
    public static readonly string ProductionProductionCompletion = "F0000074";
    //订单评级
    public static readonly string OrderRating = "F0000051";
    //提议备注
    public static readonly string ProposalRemarks = "F0000077";
    //批次号
    public static readonly string BatchNumber = "F0000002";
    //销售订单
    public static readonly string SalesOrder = "F0000009";
    //需求达成
    public static readonly string DemandFulfillment = "F0000073";
    //客户评级
    public static readonly string CustomerRating = "F0000036";
    //成品发出耗时
    public static readonly string TimeconsumingOfFinishedProducts = "F0000043";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //项目名称
    public static readonly string ProjectName = "F0000020";
    //合同需求件数
    public static readonly string NumberOfContractDemandPieces = "F0000064";
    //生产红绿灯
    public static readonly string ProductionTrafficLight = "F0000013";
}
/// <summary>
/// 市场需求,AC订单规格表
/// </summary>
[Table("AC订单规格表")]
public class OrderSpecification
{
    public static readonly string TableCode = "Skniz33124ryujrhb4hry7md21";
    public OrderSpecification() { }
    //产品精整配置
    public static readonly string ProductFinishingConfiguration = "F0000130";
    //规格已提需求数
    public static readonly string SpecificationDemandQuantity = "F0000095";
    //总公司名称
    public static readonly string HeadOfficeName = "F0000101";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //在产备注
    public static readonly string InProductionRemarks = "F0000114";
    //钻加工类别
    public static readonly string DrillingCategory = "F0000006";
    //总高
    public static readonly string TotalHeight = "F0000107";
    //能否双轧
    public static readonly string WhetherDoubleRollingCanBeCarriedOut = "F0000128";
    //试样尺寸
    public static readonly string SampleSize = "F0000134";
    //钻加工类别代号
    public static readonly string DrillingCategoryCode = "F0000007";
    //车加工类别代号
    public static readonly string MachiningCategoryCode = "F0000005";
    //产品种类
    public static readonly string ProductCategory = "F0000118";
    //技术规范
    public static readonly string TechnicalSpecification = "F0000072";
    //全局精整
    public static readonly string GlobalFinishing = "F0000132";
    //外径文本
    public static readonly string OuterDiameterText = "F0000113";
    //片厚
    public static readonly string SheetThickness = "F0000108";
    //粗车是否四面光
    public static readonly string WhetherRoughTurningIsSmoothOnAllSides = "F0000136";
    //合同重量
    public static readonly string ContractWeight = "F0000100";
    //规格在库总重
    public static readonly string TotalWeightOfSpecificationInStock = "F0000115";
    //图纸
    public static readonly string Drawing = "F0000117";
    //项目编号
    public static readonly string ItemNumber = "F0000103";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //订单在库总重文本
    public static readonly string TotalWeightOfOrderInStockText = "F0000112";
    //规格未提需求数文本
    public static readonly string SpecificationNotMentionedDemandQuantityText = "F0000093";
    //工艺配置表
    public static readonly string ProcessConfigurationTable = "F0000131";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //外径
    public static readonly string OuterDiameter = "F0000105";
    //客户编号
    public static readonly string CustomerNumber = "F0000102";
    //合同成品单重
    public static readonly string UnitWeightOfContractFinishedProduct = "F0000104";
    //订单号
    public static readonly string OrderDocNo = "ProductCode";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //产品名称
    public static readonly string ProductName = "F0000067";
    //订单数文本
    public static readonly string OrderQuantityText = "F0000092";
    //数据标题
    public static readonly string Name = "Name";
    //孔径
    public static readonly string HoleDiameter = "F0000110";
    //车加工类别
    public static readonly string MachiningCategory = "F0000004";
    //规格号
    public static readonly string SpecificationNumber = "F0000066";
    //内径
    public static readonly string InnerDiameter = "F0000106";
    //材质
    public static readonly string Material = "F0000068";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000076";
    //技术规范2
    public static readonly string TechnicalSpecification2 = "F0000116";
    //数据代码
    public static readonly string DataCode = "F0000098";
    //IconId
    public static readonly string Iconid = "IconId";
    //下料单重
    public static readonly string BlankingUnitWeight = "F0000015";
    //成品单重
    public static readonly string UnitWeightOfFinishedProduct = "F0000014";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //销售订单
    public static readonly string SalesOrder = "F0000089";
    //产品上机前互检
    public static readonly string MutualInspectionBeforeProductOnMachine = "F0000135";
    //订单在库总重
    public static readonly string TotalWeightOfOrderInStock = "F0000111";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //规格未提需求数
    public static readonly string SpecificationDemandQuantityNotMentioned = "F0000087";
    //Status
    public static readonly string Status = "Status";
    //划线绞扣
    public static readonly string ScribingTwistBuckle = "F0000133";
    //坡口图
    public static readonly string GrooveDiagram = "F0000071";
    //孔数
    public static readonly string NumberOfHoles = "F0000109";
    //合同数
    public static readonly string ContractQuantity = "F0000077";
    //规格合同数文本
    public static readonly string SpecificationContractQuantityText = "F0000099";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
}
/// <summary>
/// 市场需求,ABC订单批次规格表
/// </summary>
[Table("ABC订单批次规格表")]
public class OrderBatchSpecification
{
    public static readonly string TableCode = "Sh8z1xnes2iju59dzn4ett4bb2";
    public OrderBatchSpecification() { }
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //订单规格表
    public static readonly string OrderSpecificationTable = "F0000019";
    //产品批次规格ABC
    public static readonly string ProductBatchSpecificationAbc = "F0000066";
    //本批后需求数
    public static readonly string DemandQuantityAfterThisBatch = "F0000062";
    //变更毛坯数
    public static readonly string ChangeBlankQuantity = "F0000115";
    //达成成品数
    public static readonly string NumberOfFinishedProductsReached = "F0000122";
    //生产完成
    public static readonly string ProductionCompletion = "F0000125";
    //规格号
    public static readonly string SpecificationNumber = "F0000021";
    //变更成品标识
    public static readonly string ChangeFinishedProductIdentification = "F0000136";
    //在库成品重量
    public static readonly string WeightOfFinishedProductsInStock = "F0000045";
    //生产差数重量
    public static readonly string ProductionDifferenceWeight = "F0000093";
    //规格未提需求数
    public static readonly string DemandQuantityNotMentionedInTheSpecification = "F0000103";
    //成品发出耗小时
    public static readonly string FinishedProductDeliveryHours = "F0000129";
    //在库热加工数
    public static readonly string HotWorkInStock = "F0000111";
    //产品名称
    public static readonly string ProductName = "F0000022";
    //成品需求差数重量
    public static readonly string DemandDifferenceWeightOfFinishedProducts = "F0000132";
    //批次号
    public static readonly string BatchNumber = "F0000020";
    //生产延期天数
    public static readonly string ProductionDelayDays = "DelayedDays";
    //需求达成
    public static readonly string DemandFulfillment = "F0000124";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //产品规格
    public static readonly string ProductSpecification = "F0000026";
    //成品发出耗分钟数
    public static readonly string FinishedProductDeliveryMinutes = "F0000130";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //变更前成品需求数
    public static readonly string FinishedProductDemandBeforeChange = "F0000106";
    //本批后需求重量
    public static readonly string DemandQuantityAfterThisBatchWeightCalculation = "F0000099";
    //合同成品单重
    public static readonly string ContractFinishedProductUnitWeight = "F0000046";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //项目编号
    public static readonly string ProjectNumber = "F0000101";
    //成品发出耗时
    public static readonly string FinishedProductDeliveryTime = "F0000076";
    //成品发出耗秒数
    public static readonly string FinishedProductDeliverySeconds = "F0000134";
    //订单批次表
    public static readonly string OrderBatchTable = "F0000012";
    //产品编号AC
    public static readonly string ProductNumberAc = "F0000058";
    //冷加工计划
    public static readonly string ColdProcessingPlan = "F0000139";
    //累计成品入库重量
    public static readonly string CumulativeFinishedProductWarehousingWeight = "F0000092";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //在库冷加工数
    public static readonly string ColdWorkInStock = "F0000112";
    //成品需求期
    public static readonly string FinishedProductDemandPeriod = "F0000027";
    //成品需求差数
    public static readonly string DemandDifferenceOfFinishedProducts = "F0000131";
    //累计发运重量
    public static readonly string CumulativeShipmentWeight = "F0000091";
    //累计成品数
    public static readonly string CumulativeFinishedProductQuantity = "F0000071";
    //生产延期秒数
    public static readonly string ProductionDelaySeconds = "F0000127";
    //条数计数
    public static readonly string NumberOfPiecesCount = "F0000135";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //数据标题
    public static readonly string Name = "Name";
    //需求外成品数
    public static readonly string NumberOfFinishedProductsOutsideTheDemand = "F0000121";
    //变更成品备注
    public static readonly string RemarksOfFinishedProductsChanged = "F0000120";
    //在产详情
    public static readonly string ProductionDetails = "F0000098";
    //生产完成日期
    public static readonly string ProductionCompletionDate = "FinishDate";
    //产品批次AB
    public static readonly string ProductBatchAB = "F0000067";
    //数据代码
    public static readonly string DataCode = "F0000079";
    //规格未提需求重量
    public static readonly string DemandWeightNotMentionedInTheSpecification = "F0000104";
    //在库成品数
    public static readonly string NumberOfFinishedProductsInStock = "F0000064";
    //生产延期小时
    public static readonly string ProductionDelayHours = "F0000133";
    //发运日期
    public static readonly string ShipmentDate = "F0000077";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //生产提前天数
    public static readonly string ProductionAdvanceDays = "F0000097";
    //规格已提需求重量
    public static readonly string DemandWeightRaisedInTheSpecification = "F0000105";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //Status
    public static readonly string Status = "Status";
    //变更毛坯备注
    public static readonly string ChangeBlankRemarks = "F0000116";
    //变更毛坯标识
    public static readonly string ChangeBlankIdentification = "F0000137";
    //订单号
    public static readonly string OrderNumber = "F0000017";
    //累计发运数
    public static readonly string CumulativeShipmentNumber = "F0000063";
    //累计毛坯数
    public static readonly string CumulativeBlankQuantity = "F0000108";
    //发运完成
    public static readonly string ShipmentCompletion = "F0000123";
    //热加工计划
    public static readonly string HotProcessingPlan = "F0000138";
    //达成毛坯数
    public static readonly string AchievedBlankQuantity = "F0000117";
    //生产延期分钟
    public static readonly string ProductionDelayMinutes = "F0000126";
    //生产差数
    public static readonly string ProductionDifference = "F0000065";
    //规格已提需求数
    public static readonly string DemandQuantityRaisedInTheSpecification = "F0000102";
    //成品需求数
    public static readonly string FinishedProductDemandQuantity = "F0000047";
    //准时完工
    public static readonly string OntimeCompletion = "F0000069";
    //变更成品数
    public static readonly string ChangeNumberOfFinishedProducts = "F0000119";
    //成品需求重量
    public static readonly string FinishedProductDemandWeight = "F0000081";
}
/// <summary>
/// 市场需求,客户表
/// </summary>
[Table("客户表")]
public class Customers
{
    public static readonly string TableCode = "cace8d77e69a4113b425d16711ee46e9";
    public Customers() { }
    //数据代码
    public static readonly string DataCode = "F0000020";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //总公司名称
    public static readonly string HeadOfficeName = "F0000003";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //平均付款时长
    public static readonly string AveragePaymentDuration = "F0000016";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //Status
    public static readonly string Status = "Status";
    //联系人
    public static readonly string ContactPerson = "F0000024";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //总已欠款额
    public static readonly string TotalAmountOwed = "F0000023";
    //客户全称
    public static readonly string FullNameOfCustomer = "F0000028";
    //客户地址
    public static readonly string CustomerAddress = "F0000006";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //客户名称
    public static readonly string CustomerName = "F0000029";
    //客户信誉
    public static readonly string CustomerReputation = "F0000022";
    //电话
    public static readonly string Telephone = "F0000009";
    //主营行业
    public static readonly string MainIndustry = "F0000010";
    //客户发展潜力
    public static readonly string CustomerDevelopmentPotential = "F0000026";
    //客户类别
    public static readonly string CustomerCategory = "F0000027";
    //发票信息
    public static readonly string InvoiceInformation = "F0000025";
    //允许客户欠款额度
    public static readonly string AllowableAmountOwedByCustomer = "F0000015";
    //客户评级
    public static readonly string CustomerRating = "F0000004";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //主营产品
    public static readonly string MainProduct = "F0000011";
    //数据标题
    public static readonly string Name = "Name";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //客户编号
    public static readonly string CustomerNumber = "SeqNo";
}
/// <summary>
/// 生产制造流程,成品库
/// </summary>
[Table("成品库")]
public class FinishedStore
{
    public static readonly string TableCode = "Sazlj5e6epn2ek3eiukcbzt321";
    public FinishedStore() { }
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //单重
    public static readonly string UnitWeight = "F0000008";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //出库时间
    public static readonly string IssueTime = "F0000014";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000015";
    //车间位置
    public static readonly string WorkshopLocation = "F0000019";
    //当前工步
    public static readonly string CurrentStep = "F0000021";
    //ID
    public static readonly string ID = "F0000016";
    //完成时间
    public static readonly string CompletionTime = "F0000009";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000002";
    //产品规格
    public static readonly string ProductSpecification = "F0000006";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //数据代码
    public static readonly string DataCode = "F0000018";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //产品位置
    public static readonly string ProductLocation = "F0000020";
    //产品名称
    public static readonly string ProductName = "F0000005";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000003";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //数据标题
    public static readonly string Name = "Name";
    //当前工序
    public static readonly string CurrentOperation = "F0000022";
    //工件号
    public static readonly string PieceNumber = "F0000004";
    //入库时间
    public static readonly string ReceiptTime = "F0000013";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //Status
    public static readonly string Status = "Status";
}
/// <summary>
/// 生产制造流程,人工调整工序
/// </summary>
[Table("人工调整工序")]
public class ManualAdjustProcess
{
    public static readonly string TableCode = "Se0zvmpq4f9zpi894bxzf5sz35";
    public ManualAdjustProcess() { }
    //ID
    public static readonly string ID = "F0000006";
    //当前工步
    public static readonly string CurrentJobStep = "F0000029";
    //是否取样
    public static readonly string SamplingOrNot = "F0000019";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000004";
    //工人
    public static readonly string Worker = "F0000030";
    //产品名称
    public static readonly string ProductName = "F0000007";
    //其它来源
    public static readonly string OtherSources = "F0000023";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //原材料号
    public static readonly string RawMaterialNumber = "F0000013";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //工件号
    public static readonly string WorkpieceNumber = "F0000005";
    //数据标题
    public static readonly string Name = "Name";
    //数据分类
    public static readonly string DataClassification = "F0000024";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000022";
    //产品位置
    public static readonly string ProductLocation = "F0000027";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //Status
    public static readonly string Status = "Status";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //单重
    public static readonly string UnitWeight = "F0000017";
    //车间位置
    public static readonly string WorkshopLocation = "F0000026";
    //轧制方式
    public static readonly string RollingMethod = "F0000014";
    //转至工序
    public static readonly string TransferToOperation = "F0000020";
    //数据代码
    public static readonly string DataCode = "F0000028";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //批次号
    public static readonly string BatchNumber = "F0000002";
    //规格号
    public static readonly string SpecificationNumber = "F0000003";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
}
/// <summary>
/// 生产计划,ABCD工序计划表
/// </summary>
[Table("ABCD工序计划表")]
public class ABCDProcessPlan
{
    public static readonly string TableCode = "Szlywopbivyrv1d64301ta5xv4";
    public ABCDProcessPlan() { }
    //能否双轧
    public static readonly string DoubleRollingPossible = "F0000153";
    //Status
    public static readonly string Status = "Status";
    //粗车
    public static readonly string RoughTurning = "D001419e4ec5c3c47594922975c8553366c47d0";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //取样
    public static readonly string Sampling = "D001419Fef5c946a47b04101889b43bf290ada42";
    //订单批次表
    public static readonly string OrderBatchTable = "F0000144";
    //精车转运车间名称
    public static readonly string NameOfFinishTurningTransferWorkshop = "F0000195";
    //外协合同表精车
    public static readonly string OutsourcingContractFormFinishTurning = "F0000087";
    //成品需求期
    public static readonly string FinishedProductDemandPeriod = "F0000021";
    //外协合同表锻压
    public static readonly string OutsourcingContractTableForgingPressing = "F0000065";
    //炉次计划
    public static readonly string HeatCountPlan = "F0000138";
    //热处理炉号
    public static readonly string HeatTreatmentFurnaceNumber = "F0000140";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //加工单位锻压
    public static readonly string ProcessingUnitForging = "F0000062";
    //精车
    public static readonly string FinishTurning = "D001419e351edfd0ae44d3e960e3f1c14991f82";
    //全局装炉前检验
    public static readonly string GlobalInspectionBeforeFurnaceLoading = "F0000191";
    //热加工计划
    public static readonly string HotProcessingPlan = "F0000023";
    //外协合同表热处理
    public static readonly string OutsourcingContractTableHeatTreatment = "F0000073";
    //全部利用
    public static readonly string AllUtilization = "Finish";
    //单件装炉前检验
    public static readonly string InspectionBeforeSinglePieceFurnaceLoading = "F0000148";
    //当前用户
    public static readonly string CurrentUser = "F0000143";
    //工艺配置表
    public static readonly string ProcessConfigurationTable = "F0000158";
    //单件忽略理化结果
    public static readonly string SinglePieceIgnorePhysicochemicalResults = "F0000161";
    //批次号
    public static readonly string BatchNumber = "F0000125";
    //开启流程
    public static readonly string OpenProcess = "F0000020";
    //工件号
    public static readonly string WorkpieceNumber = "F0000054";
    //加工单位锯切
    public static readonly string ProcessingUnitSawing = "F0000058";
    //计划本取
    public static readonly string PlanThisOptionTakes = "F0000141";
    //加工单位粗车
    public static readonly string ProcessingUnitRoughTurning = "F0000078";
    //冷加工科室
    public static readonly string ColdProcessingDepartment = "F0000135";
    //产品上机前互检
    public static readonly string MutualInspectionBeforeProductMachineOperation = "F0000189";
    //钻孔
    public static readonly string Drilling = "D001419f342384a36db4bd2ac53ffbc5b86d8b4";
    //质量配置表
    public static readonly string QualityConfigurationTable = "F0000173";
    //外协合同表钻孔
    public static readonly string OutsourcingContractFormDrilling = "F0000089";
    //精车计划完成时间
    public static readonly string FinishTurningPlannedCompletionTime = "F0000098";
    //取样转运车间位置
    public static readonly string LocationOfSamplingTransferWorkshop = "F0000214";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000006";
    //工艺流程表
    public static readonly string ProcessFlowTable = "F0000190";
    //加工单位精车
    public static readonly string ProcessingUnitFinishTurning = "F0000082";
    //热加工科室
    public static readonly string HotProcessingDepartment = "F0000134";
    //计划轧制方式
    public static readonly string PlannedRollingMethod = "F0000152";
    //粗车计划完成时间
    public static readonly string RoughTurningPlanCompletionTime = "F0000095";
    //单件上机前互检
    public static readonly string MutualInspectionBeforeSinglePieceMachineOperation = "F0000175";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000004";
    //数据代码
    public static readonly string DataCode = "shuj";
    //本工序需求期辗环
    public static readonly string DemandPeriodOfThisOperationRingRolling = "F0000067";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //精车转运车间位置
    public static readonly string LocationOfFinishTurningTransferWorkshop = "F0000196";
    //加工单位热处理
    public static readonly string ProcessingUnitHeatTreatment = "F0000070";
    //产品名称
    public static readonly string ProductName = "F0000025";
    //数据标题
    public static readonly string Name = "Name";
    //计划炉次编号
    public static readonly string PlannedFurnaceNumber = "F0000139";
    //ID
    public static readonly string ID = "F0000007";
    //外协合同表锯切
    public static readonly string OutsourcingContractTableSawing = "F0000061";
    //订单规格表
    public static readonly string OrderSpecificationTable = "F0000145";
    //成品单重
    public static readonly string FinishedProductUnitWeight = "F0000031";
    //订单批次规格表
    public static readonly string OrderBatchSpecificationTable = "F0000017";
    //原材料编号
    public static readonly string RawMaterialNumber = "F0000192";
    //全局忽略理化结果
    public static readonly string GlobalIgnorePhysicochemicalResults = "F0000174";
    //外协合同表粗车
    public static readonly string OutsourcingContractTableRoughTurning = "F0000081";
    //粗车转运车间名称
    public static readonly string RoughTurningTransferWorkshopName = "F0000193";
    //取样计划完成时间
    public static readonly string CompletionTimeOfSamplingPlan = "F0000212";
    //本工序需求期锯切
    public static readonly string DemandPeriodOfThisProcedureSawing = "F0000059";
    //再生库表
    public static readonly string RegenerationWarehouseTable = "F0000018";
    //本工序需求期锻压
    public static readonly string DemandPeriodOfThisOperationForging = "F0000063";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //加工单位钻孔
    public static readonly string ProcessingUnitDrilling = "F0000084";
    //全局上机前互检
    public static readonly string GlobalMutualInspectionBeforeMachineOperation = "F0000185";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //规格号
    public static readonly string SpecificationNumber = "F0000093";
    //再生工序
    public static readonly string RegenerationProcess = "F0000142";
    //冷加工计划
    public static readonly string ColdProcessingPlan = "F0000024";
    //质量状态
    public static readonly string QualityStatus = "F0000114";
    //全局精整配置
    public static readonly string GlobalFinishingConfiguration = "F0000160";
    //取样转运车间名称
    public static readonly string NameOfSamplingTransferWorkshop = "F0000213";
    //本工序需求期热处理
    public static readonly string DemandPeriodOfThisOperationHeatTreatment = "F0000071";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000003";
    //单件精整配置
    public static readonly string SinglePieceFinishingConfiguration = "F0000146";
    //钻孔计划完成时间
    public static readonly string DrillingPlannedCompletionTime = "F0000102";
    //加工单位辗环
    public static readonly string ProcessingUnitRingRolling = "F0000066";
    //钻孔转运车间名称
    public static readonly string NameOfDrillingTransferWorkshop = "F0000197";
    //外协合同表辗环
    public static readonly string OutsourcingContractTableRingRolling = "F0000069";
    //粗车转运车间位置
    public static readonly string RoughTurningLocationOfTransferWorkshop = "F0000194";
    //再生品ID
    public static readonly string RecycledProductID = "F0000137";
    //外协合同表毛坯
    public static readonly string OutsourcingContractTableBlank = "F0000077";
    //产品精整配置
    public static readonly string ProductFinishingConfiguration = "F0000157";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //规格参数
    public static readonly string SpecificationParameters = "F0000028";
    //本工序需求期毛坯
    public static readonly string DemandPeriodOfThisOperationBlank = "F0000075";
    //加工单位毛坯
    public static readonly string ProcessingUnitBlank = "F0000074";
    //钻孔转运车间位置
    public static readonly string LocationOfDrillingTransferWorkshop = "F0000198";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //成品需求数
    public static readonly string FinishedProductDemandQuantity = "F0000039";
}
/// <summary>
/// ABCD计划,ABCD钻孔子表
/// </summary>
[Table("ABCD钻孔子表")]
public class ABCDDrillSubTable
{
    public static readonly string TableCode = "f342384a36db4bd2ac53ffbc5b86d8b4";
    public ABCDDrillSubTable() { }
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //设备编号
    public static readonly string DeviceNum = "F0000217";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //加工量
    public static readonly string WorkLoad = "F0000219";
    //设备名称
    public static readonly string DeviceName = "F0000216";
    //姓名
    public static readonly string FullName = "F0000215";
    //数据标题
    public static readonly string Name = "Name";
    //工时
    public static readonly string WorkingHours = "F0000220";
}
/// <summary>
/// ABCD计划,ABCD精车子表
/// </summary>
[Table("ABCD精车子表")]
public class ABCDFinishSubTable
{
    public static readonly string TableCode = "e351edfd0ae44d3e960e3f1c14991f82";
    public ABCDFinishSubTable() { }
    //姓名
    public static readonly string FullName = "F0000215";
    //设备编号
    public static readonly string DeviceNum = "F0000217";
    //工时
    public static readonly string WorkingHours = "F0000220";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //数据标题
    public static readonly string Name = "Name";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //加工量
    public static readonly string WorkLoad = "F0000219";
    //设备名称
    public static readonly string DeviceName = "F0000216";
}
/// <summary>
/// ABCD计划,ABCD粗车子表
/// </summary>
[Table("ABCD粗车子表")]
public class ABCDRoughSubTable
{
    public static readonly string TableCode = "e4ec5c3c47594922975c8553366c47d0";
    public ABCDRoughSubTable() { }
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //设备名称
    public static readonly string DeviceName = "F0000216";
    //数据标题
    public static readonly string Name = "Name";
    //工时
    public static readonly string WorkingHours = "F0000220";
    //姓名
    public static readonly string FullName = "F0000215";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //设备编号
    public static readonly string DeviceNum = "F0000217";
    //加工量
    public static readonly string WorkLoad = "F0000219";
}
/// <summary>
/// ABCD工序计划表,ABCD取样子表
/// </summary>
[Table("ABCD取样子表")]
public class ABCDSimpleSubTable
{
    public static readonly string TableCode = "Fef5c946a47b04101889b43bf290ada42";
    public ABCDSimpleSubTable() { }
    //工时
    public static readonly string WorkingHours = "F0000220";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //数据标题
    public static readonly string Name = "Name";
    //加工量
    public static readonly string WorkLoad = "F0000219";
    //设备编号
    public static readonly string DeviceNum = "F0000217";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //设备名称
    public static readonly string DeviceName = "F0000216";
    //姓名
    public static readonly string FullName = "F0000215";
}
/// <summary>
/// 生产制造流程,取样四面光子表
/// </summary>
[Table("取样四面光子表")]
public class SamplingFourLathe
{
    public static readonly string TableCode = "a955856dedeb4b27b86c6424f525bbeb";
    public SamplingFourLathe() { }
    //加工量
    public static readonly string WorkLoad = "F0000162";
    //加工记录
    public static readonly string ProcessRecord = "F0000161";
    //申请难度调整
    public static readonly string Adjustment = "F0000184";
    //设备编号
    public static readonly string DeviceNum = "F0000159";
    //加工者
    public static readonly string Worker = "F0000157";
    //设备类型
    public static readonly string DeviceType = "F0000160";
    //结束时间
    public static readonly string EndTime = "F0000187";
    //设备选择
    public static readonly string DeviceSelect = "F0000163";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //开始时间
    public static readonly string StartTime = "F0000164";
    //是否探伤
    public static readonly string IsUt = "F0000188";
    //设备名称
    public static readonly string DeviceName = "F0000158";
    //任务名称
    public static readonly string TaskName = "F0000166";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //数据标题
    public static readonly string Name = "Name";
}
/// <summary>
/// 生产制造流程,粗车四面光子表
/// </summary>
[Table("粗车四面光子表")]
public class RoughFourLathe
{
    public static readonly string TableCode = "9e58919544424654bcc75ef1dc953be6";
    public RoughFourLathe() { }
    //设备类型
    public static readonly string DeviceType = "F0000160";
    //设备编号
    public static readonly string DeviceNum = "F0000159";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //申请难度调整
    public static readonly string Adjustment = "F0000181";
    //是否探伤
    public static readonly string IsUt = "F0000168";
    //开始时间
    public static readonly string StartTime = "F0000164";
    //数据标题
    public static readonly string Name = "Name";
    //加工者
    public static readonly string Worker = "F0000157";
    //任务名称
    public static readonly string TaskName = "F0000166";
    //结束时间
    public static readonly string EndTime = "F0000185";
    //设备选择
    public static readonly string DeviceSelect = "F0000163";
    //加工量
    public static readonly string WorkLoad = "F0000162";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //设备名称
    public static readonly string DeviceName = "F0000158";
    //加工记录
    public static readonly string ProcessRecord = "F0000161";
}
/// <summary>
/// 生产制造流程,取样子流程
/// </summary>
[Table("取样子流程")]
public class SamplingSubProcess
{
    public static readonly string TableCode = "Sgljz62e1rneytbqjckbe1vu25";
    public SamplingSubProcess() { }
    //返修类型
    public static readonly string RepairType = "F0000127";
    //当前工序
    public static readonly string CurrentProcess = "F0000071";
    //任务名称
    public static readonly string TaskName = "F0000082";
    //工人
    public static readonly string Worker = "F0000135";
    //转运位置
    public static readonly string TransferLocation = "F0000134";
    //四面光
    public static readonly string FourSideLight = "D001419a955856dedeb4b27b86c6424f525bbeb";
    //轧制方式
    public static readonly string RollingMethod = "F0000039";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000057";
    //是否调整至其他工序
    public static readonly string WhetherToAdjustToOtherProcesses = "F0000066";
    //Status
    public static readonly string Status = "Status";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //本工序需求期
    public static readonly string DemandPeriodOfThisProcess = "F0000073";
    //理化结果不合格
    public static readonly string UnqualifiedPhysicalAndChemicalResults = "F0000170";
    //数据代码
    public static readonly string DataCode = "F0000064";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //理化数据
    public static readonly string PhysicalAndChemicalData = "D001419F74390f3bba284177a2924f383ae069eb";
    //发起探伤
    public static readonly string InitiateFlawDetection = "F0000116";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //探伤认定
    public static readonly string FlawDetectionIdentification = "F0000174";
    //四面光是否探伤
    public static readonly string FourSideLightFlawDetection = "F0000183";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //计划本取
    public static readonly string PlanBookRetrieval = "F0000077";
    //确认本取
    public static readonly string ConfirmationBookRetrieval = "F0000040";
    //班组长
    public static readonly string TeamLeader = "F0000128";
    //炉次计划
    public static readonly string HeatPlan = "F0000075";
    //探伤表
    public static readonly string FlawDetectionTable = "F0000173";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000069";
    //质检结论
    public static readonly string QualityInspectionConclusion = "qualityResult";
    //确认炉次编号
    public static readonly string ConfirmationFurnaceNumber = "F0000074";
    //探伤结果
    public static readonly string FlawDetectionResults = "F0000105";
    //订单规格表
    public static readonly string OrderSpecificationTable = "F0000102";
    //是否探伤
    public static readonly string FlawDetection = "F0000123";
    //确认热处理炉号
    public static readonly string ConfirmationHeatTreatmentFurnaceno = "F0000076";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //是否开启制样流程
    public static readonly string YesOrNoopenSamplePreparationProcess = "F0000124";
    //发起异常
    public static readonly string InitiateException = "F0000060";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //加工单位
    public static readonly string ProcessingUnit = "F0000061";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ID
    public static readonly string ID = "F0000058";
    //转运车间
    public static readonly string TransferWorkshop = "F0000133";
    //工件号
    public static readonly string WorkpieceNumber = "F0000025";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //备注
    public static readonly string Remarks = "F0000080";
    //异常类别
    public static readonly string ExceptionCategory = "F0000070";
    //单重
    public static readonly string UnitWeight = "F0000045";
    //实际加工耗时
    public static readonly string ActualProcessingTime = "CountTime";
    //完成本取
    public static readonly string CompletionCost = "F0000106";
    //完成总量
    public static readonly string TotalAmountCompleted = "F0000104";
    //取样
    public static readonly string Sampling = "D001419Fj7nrmbgha1j10v5zst0zg7hi1";
    //数据标题
    public static readonly string Name = "Name";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //当前本取加工者
    public static readonly string CurrentProcessor = "F0000129";
    //产品参数表
    public static readonly string ProductParameterTable = "F0000103";
    //所有者
    public static readonly string Owner = "F0000126";
    //重处理类型
    public static readonly string ReprocessingType = "F0000140";
    //理化结果
    public static readonly string PhysicalAndChemicalResults = "F0000122";
    //当前车间
    public static readonly string CurrentWorkshop = "F0000067";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000072";
    //检验结果
    public static readonly string InspectionResults = "F0000041";
    //当前位置
    public static readonly string CurrentLocation = "F0000068";
    //异常描述
    public static readonly string ExceptionDescription = "F0000079";
    //试样类型
    public static readonly string SampleType = "F0000119";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //产品类别
    public static readonly string ProductCategory = "F0000088";
}
/// <summary>
/// 生产计划,派工表
/// </summary>
[Table("派工表")]
public class Dispatchs
{
    public static readonly string TableCode = "c08bb982ac44481a9439076269a8f783";
    public Dispatchs() { }
    //粗车计划完成时间
    public static readonly string RoughTurningPlanCompletionTime = "F0000004";
    //粗车车间名称
    public static readonly string RoughTurningWorkshopName = "F0000028";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ID
    public static readonly string ID = "F0000025";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //精车不限制派工顺序
    public static readonly string FinishTurningUnlimitedDispatchSequence = "F0000043";
    //粗车四面光车间位置
    public static readonly string RoughFourSideLightWorkshopLocation = "F0000082";
    //粗车四面光计划完成时间
    public static readonly string RoughFourSideLightPlannedCompletionTime = "F0000075";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //取样四面光
    public static readonly string samplingTetrahedralLight = "D001419Fbb3556b399a44f998b82f9aa74624afd";
    //粗车四面光车间名称
    public static readonly string RoughTurningTetrahedralLightWorkshopName = "F0000081";
    //取样四面光不限制派工顺序
    public static readonly string SamplingTetrahedralLightUnlimitedDispatchSequence = "F0000074";
    //钻孔车间位置
    public static readonly string DrillingWorkshopLocation = "F0000033";
    //取样车间名称
    public static readonly string SamplingWorkshopName = "F0000026";
    //钻孔车间名称
    public static readonly string DrillingWorkshopName = "F0000032";
    //规格号
    public static readonly string SpecificationNumber = "F0000040";
    //取样不限制派工顺序
    public static readonly string SamplingUnlimitedDispatchSequence = "F0000041";
    //取样四面光车间位置
    public static readonly string SamplingFourSideLightWorkshopLocation = "F0000080";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //粗车
    public static readonly string RoughTurning = "D001419Ffb3f2e583e31421e8aaa5a085bbada58";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //粗车四面光
    public static readonly string RoughTurningTetrahedralLight = "D001419F694a0d18773d4a329ad4e145ccee2bb7";
    //取样四面光计划完成时间
    public static readonly string SamplingTetrahedralLightPlanCompletionTime = "F0000073";
    //数据标题
    public static readonly string Name = "Name";
    //精车车间位置
    public static readonly string FinishTurningWorkshopLocation = "F0000031";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //粗车车间位置
    public static readonly string RoughTurningWorkshopLocation = "F0000029";
    //订单号
    public static readonly string orderNumber = "F0000038";
    //取样
    public static readonly string Sampling = "D001419Fc9380612ad364043a33702a36bf5fde9";
    //Status
    public static readonly string Status = "Status";
    //取样四面光车间名称
    public static readonly string SamplingTetrahedralLightWorkshopNameweighing = "F0000079";
    //粗车不限制派工顺序
    public static readonly string RoughTurningUnlimitedDispatchSequence = "F0000042";
    //精车
    public static readonly string FinishTurning = "D001419F4a23f2f26a01428f952a593da3d99fe5";
    //粗车四面光不限制派工顺序
    public static readonly string RoughFourSideLightUnlimitedDispatchSequence = "F0000076";
    //取样车间位置
    public static readonly string SamplingWorkshopLocation = "F0000027";
    //钻孔不限制派工顺序
    public static readonly string DrillingUnlimitedDispatchSequence = "F0000044";
    //精车计划完成时间
    public static readonly string FinishTurningPlanCompletionTime = "F0000005";
    //精车车间名称
    public static readonly string FinishTurningWorkshopName = "F0000030";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //钻孔
    public static readonly string Drilling = "D001419F5ccfa7d5acad41bf98c640057f2570ae";
    //取样计划完成时间
    public static readonly string SamplingPlanCompletionTime = "F0000003";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //工件号
    public static readonly string WorkpieceNumber = "F0000039";
    //钻孔计划完成时间
    public static readonly string DrillingPlanCompletionTime = "F0000006";
}
/// <summary>
/// 生产制造流程,返修
/// </summary>
[Table("返修")]
public class Repair
{
    public static readonly string TableCode = "Sz2y3t1gjtgld76g5dcl3edj24";
    public Repair() { }
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //转至工序
    public static readonly string TransferToProcess = "F0000006";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //数据代码
    public static readonly string DataCode = "F0000024";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //Status
    public static readonly string Status = "Status";
    //工人
    public static readonly string Worker = "F0000017";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000031";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000020";
    //工序来源
    public static readonly string OperationSource = "F0000035";
    //车间位置
    public static readonly string WorkshopLocation = "F0000022";
    //数据标题
    public static readonly string Name = "Name";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //产品位置
    public static readonly string ProductLocation = "F0000023";
    //理化结果
    public static readonly string PhysicalAndChemicalResults = "F0000032";
    //炉次编号
    public static readonly string HeatNumber = "F0000027";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //炉次计划
    public static readonly string HeatPlan = "F0000026";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //检验后处理
    public static readonly string PostInspectionProcessing = "F0000034";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //热处理炉号
    public static readonly string HeatTreatmentFurnaceNumber = "F0000028";
    //质检结论
    public static readonly string QualityInspectionConclusion = "F0000036";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //ID
    public static readonly string ID = "F0000021";
    //子表
    public static readonly string SubTable = "D001419F24790e6009c14d8a9b7e1473ad7d8db7";
    //工件号
    public static readonly string WorkpieceNumber = "F0000001";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //使用设备
    public static readonly string EquipmentUsed = "F0000009";
    //返修类型
    public static readonly string RepairType = "F0000025";
    //检验结果
    public static readonly string InspectionResults = "F0000033";
}
/// <summary>
/// 生产计划,车间区域责任划分
/// </summary>
[Table("车间区域责任划分")]
public class WorkshopManager
{
    public static readonly string TableCode = "19b80510f0e24d8695f5e80f8c485fa8";
    public WorkshopManager() { }
    //钻孔人员角色
    public static readonly string DrillingPersonnelAngleColor = "F0000005";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //钻孔派工角色
    public static readonly string DrillingDispatchingRole = "F0000003";
    //数据标题
    public static readonly string Name = "Name";
    //精车人员角色
    public static readonly string FineTurningPersonnelRole = "F0000007";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //车间
    public static readonly string Workshop = "F0000004";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //Status
    public static readonly string Status = "Status";
    //粗车人员角色
    public static readonly string RoughTurningPersonnelRole = "F0000006";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //精车派工角色
    public static readonly string FineTurningDispatchingRole = "F0000002";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //检验人员角色
    public static readonly string InspectorRole = "F0000008";
    //取样派工角色
    public static readonly string SamplingDispatchingRole = "F0000009";
    //粗车派工角色
    public static readonly string RoughTurningDispatchingRole = "F0000001";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
}

/// <summary>
/// 机加工绩效,任务绩效表
/// </summary>
[Table("任务绩效表")]
public class TaskPerformance
{
    public static readonly string TableCode = "22a4f64f7fd74aed89a85e018fca456d";
    public TaskPerformance() { }
    //订单规格号
    public static readonly string OrderSpecificationNumber = "ProductNum";
    //工时工资
    public static readonly string WorkhourlyWage = "F0000008";
    //工艺下屑重量
    public static readonly string ProcessChipWeight = "F0000011";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //总工时
    public static readonly string TotalManHours = "F0000006";
    //工序名称
    public static readonly string OperationName = "F0000003";
    //检验结果
    public static readonly string InspectionResult = "F0000015";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //补刀金额
    public static readonly string ToolReplenishmentAmount = "F0000013";
    //设备名称
    public static readonly string EquipmentName = "F0000002";
    //单件拟定工时
    public static readonly string PlannedManHoursForASinglePiece = "F0000005";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ID
    public static readonly string ID = "F0000016";
    //总工作量
    public static readonly string TotalWorkload = "gongzuoliang";
    //任务名称
    public static readonly string TaskName = "F0000004";
    //部门名称
    public static readonly string DepartmentName = "F0000018";
    //设备类型
    public static readonly string EquipmentType = "F0000020";
    //加工数量
    public static readonly string ProcessingQuantity = "F0000010";
    //Status
    public static readonly string Status = "Status";
    //总下屑量
    public static readonly string TotalScrap = "F0000012";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //工件号
    public static readonly string WorkpieceNumber = "F0000014";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //工价
    public static readonly string WorkPrice = "F0000009";
    //任务类别
    public static readonly string TaskCategory = "F0000017";
    //加工人员
    public static readonly string Processor = "F0000019";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //数据标题
    public static readonly string Name = "Name";
}
/// <summary>
/// 机加工绩效,加工任务记录
/// </summary>
[Table("加工任务记录")]
public class MachiningTaskRecord
{
    public static readonly string TableCode = "4963919529e44d60be759656d4a16b63";
    public MachiningTaskRecord() { }
    //总高
    public static readonly string TotalHeight = "F0000020";
    //车加工类别
    public static readonly string DrillingProcessingCategory = "F0000025";
    //下机时间
    public static readonly string EndTime = "EndTime";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //任务名称
    public static readonly string TaskName = "F0000002";
    //本工序产品工时
    public static readonly string ProcessManHour = "F0000004";
    //内径
    public static readonly string InsideDiameter = "F0000018";
    //孔数
    public static readonly string HoleAmount = "F0000021";
    //Status
    public static readonly string Status = "Status";
    //工件号
    public static readonly string WorkPieceNumber = "F0000040";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //任务工时
    public static readonly string TaskManHour = "F0000006";
    //实际片厚
    public static readonly string ActualThickness = "F0000036";
    //订单规格号
    public static readonly string OrderSpecifications = "ProductNum";
    //实际总高
    public static readonly string ActualTotalHeight = "F0000035";
    //实际孔径
    public static readonly string ActualAperture = "F0000039";
    //设备名称
    public static readonly string DeviceName = "F0000007";
    //产品名称
    public static readonly string ProductName = "F0000026";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //任务类别
    public static readonly string TaskType = "F0000031";
    //实际耗时
    public static readonly string ElapsedTime = "F0000013";
    //探伤结果
    public static readonly string UltrasonicResults = "F0000029";
    //孔径
    public static readonly string Aperture = "F0000022";
    //ID
    public static readonly string ID = "ID";
    //加工数量
    public static readonly string WorkLoad = "F0000010";
    //实际内径
    public static readonly string ActualInsideDiameter = "F0000034";
    //工资已算
    public static readonly string IsCumpute = "F0000028";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //钻加工类别
    public static readonly string LatheProcessingCategory = "F0000027";
    //记录标识
    public static readonly string RecordGuid = "F0000054";
    //加工难度
    public static readonly string ProcessDifficulty = "F0000043";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //实际外径
    public static readonly string ActualOutsideDiameter = "F0000033";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //检验结果
    public static readonly string InspectionResults = "F0000009";
    //部门名称
    public static readonly string DepartmentName = "F0000030";
    //备注
    public static readonly string Remarks = "F0000042";
    //工艺下屑重量
    public static readonly string CraftScrapWeight = "F0000023";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //单件拟定工时
    public static readonly string UnitmanHour = "F0000005";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //加工人员
    public static readonly string Worker = "F0000011";
    //试样类型
    public static readonly string SampleType = "F0000032";
    //工序名称
    public static readonly string ProcessName = "F0000001";
    //上机时间
    public static readonly string StartTime = "StartTime";
    //成品单重
    public static readonly string UnitWeightofFinish = "F0000017";
    //轧制方式
    public static readonly string RollingMethod = "F0000024";
    //设备类型
    public static readonly string DeviceType = "F0000041";
    //实际孔数
    public static readonly string ActualHoleCount = "F0000038";
    //申请难度调整
    public static readonly string ApplyAdjust = "F0000044";
    //设备系数
    public static readonly string DeviceCoefficient = "F0000008";
    //实际单重
    public static readonly string Actualunitweight = "F0000037";
    //外径
    public static readonly string OutsideDiameter = "F0000016";
    //片厚
    public static readonly string Thickness = "F0000019";
    //设备编号
    public static readonly string DeviceNumber = "F0000014";
}
/// <summary>
/// 生产计划,再生库
/// </summary>
[Table("再生库")]
public class ReviveWarehouse
{
    public static readonly string TableCode = "Sfb3zsjf4iglhv1sjs995pmho4";
    public ReviveWarehouse() { }
    //材料型号
    public static readonly string MaterialModel = "F0000020";
    //报废工序
    public static readonly string ScrappingProcedure = "F0000005";
    //检验结论热处理
    public static readonly string InspectionInspectionConclusionHeatTreatment = "F0000023";
    //ID
    public static readonly string ID = "F0000018";
    //再生原因
    public static readonly string RegenerationReason = "F0000007";
    //报废原因
    public static readonly string ScrappingReason = "F0000032";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000017";
    //数据代码
    public static readonly string DataCode = "F0000031";
    //Status
    public static readonly string Status = "Status";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000003";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000002";
    //数据标题
    public static readonly string Name = "Name";
    //检验结论辗环
    public static readonly string InspectionConclusionRingRolling = "F0000022";
    //工件号
    public static readonly string WorkpieceNumber = "F0000004";
    //检验结论毛坯
    public static readonly string InspectionConclusionBlank = "F0000024";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //质量异常单
    public static readonly string QualityExceptionForm = "F0000016";
    //检验结论锻压
    public static readonly string InspectionConclusionForging = "F0000021";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //检验结论粗车
    public static readonly string InspectionConclusionRoughTurning = "F0000025";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //已利用
    public static readonly string Used = "IsUsed";
    //检验结论精车
    public static readonly string InspectionConclusionFineTurning = "F0000027";
    //检验结论钻孔
    public static readonly string InspectionConclusionDrilling = "F0000029";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //检验结论锯切
    public static readonly string InspectionConclusionSawing = "F0000019";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
}
/// <summary>
/// 生产设备管理,设备档案
/// </summary>
[Table("设备档案")]
public class DeviceArchives
{
    public static readonly string TableCode = "Sq0biizim9l50i2rl6kgbpo3u4";
    public DeviceArchives() { }
    //热处理转移至人工调整工序
    public static readonly string HeatTreatmentTransferredToManualAdjustmentOperation = "F0000105";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //本工序需求期精车
    public static readonly string DemandPeriodOfThisProcessFinishTurning = "F0000088";
    //粗车加工单位
    public static readonly string RoughTurningUnit = "F0000039";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000003";
    //完成本取
    public static readonly string CompletedTake = "F0000120";
    //产品种类
    public static readonly string ProductType = "F0000023";
    //工件号
    public static readonly string WorkpieceNumber = "F0000005";
    //计划热处理炉号
    public static readonly string PlannedHeatTreatmentHeatNumber = "F0000124";
    //确定轧制方式
    public static readonly string DeterminedRollingMethod = "F0000111";
    //确认炉次计划
    public static readonly string ConfirmedHeatPlan = "F0000125";
    //炉次计划
    public static readonly string HeatPlan = "F0000115";
    //转至工步
    public static readonly string TransferToStep = "F0000057";
    //转至工序
    public static readonly string TransferToOperation = "F0000056";
    //粗车工序转移至人工调整工序
    public static readonly string RoughTurningOperationTransferredToManualAdjustmentOperation = "F0000107";
    //工序计划表
    public static readonly string OperationSchedule = "F0000126";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000002";
    //人工调整工序数据分类
    public static readonly string ManuallyAdjustOperationDataClassification = "F0000110";
    //精车转移至人工调整工序
    public static readonly string FinishTurningTransferredToManualAdjustmentProcess = "F0000108";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //原材料编号
    public static readonly string RawMaterialNumber = "F0000058";
    //辗环转移至人工调整工序
    public static readonly string RingRollingTransferredToManualAdjustmentProcedure = "F0000104";
    //质检结论锯切
    public static readonly string QualityinspectionConclusionSawing = "F0000026";
    //精车已加工量
    public static readonly string FinishTurningProcessedQuantity = "F0000134";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //确认热处理炉号
    public static readonly string ConfirmedHeatTreatmentHeatNumber = "F0000116";
    //辗环外协合同表
    public static readonly string RingRollingOutsourcingContractForm = "F0000075";
    //钻孔加工单位
    public static readonly string DrillingUnit = "F0000041";
    //锯切外协合同表
    public static readonly string SawingOutsourcingContractForm = "F0000070";
    //本工序需求期粗车
    public static readonly string DemandPeriodOfThisOperationRoughTurning = "F0000085";
    //钻孔工序转移至人工调整工序
    public static readonly string DrillingProcessTransferredToManualAdjustmentProcess = "F0000109";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //锻压外协合同表
    public static readonly string ForgingOutsourcingContractForm = "F0000072";
    //粗车已加工量
    public static readonly string RoughTurningProcessedQuantity = "F0000133";
    //粗车外协合同表
    public static readonly string RoughTurningOutsourcingContractTable = "F0000084";
    //双轧编号
    public static readonly string DoubleRollingNumber = "F0000127";
    //计划轧制方式
    public static readonly string PlannedRollingMethod = "F0000112";
    //锻压加工单位
    public static readonly string ForgingProcessingUnit = "F0000027";
    //热处理任务类型
    public static readonly string HeatTreatmentUnitbusinessType = "F0000122";
    //原材料类型
    public static readonly string RawMaterialType = "F0000062";
    //质检结论锻压
    public static readonly string QualityInspectionConclusionForging = "F0000035";
    //质检结论精车
    public static readonly string QualityInspectionConclusionFinishTurning = "F0000045";
    //成品单重
    public static readonly string FinishedProductUnitWeight = "F0000017";
    //钻孔外协合同表
    public static readonly string DrillingOutsourcingContractTable = "F0000096";
    //毛坯外协合同表
    public static readonly string BlankOutsourcingContractTable = "F0000081";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //精车外协合同表
    public static readonly string FinishTurningOutsourcingContractTable = "F0000087";
    //数据代码
    public static readonly string DataCode = "F0000069";
    //计划炉次编号
    public static readonly string PlannedHeatNumber = "F0000113";
    //产品名称
    public static readonly string ProductName = "F0000007";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //锻压转移至人工调整工序
    public static readonly string ForgingTransferredToManualAdjustmentProcedure = "F0000103";
    //质检结论钻孔
    public static readonly string QualityInspectionConclusionDrilling = "F0000046";
    //质量状态
    public static readonly string QualityStatus = "F0000065";
    //确认炉次编号
    public static readonly string ConfirmedHeatNumber = "F0000114";
    //辗环加工单位
    public static readonly string RingRollingProcessingUnit = "F0000028";
    //是否忽略理化结果
    public static readonly string IgnorePhysicalAndChemicalResults = "F0000123";
    //粗车发起探伤
    public static readonly string RoughTurningInitiatedFlawDetection = "F0000131";
    //再生工序
    public static readonly string regenerationProcess = "F0000121";
    //热处理加工单位
    public static readonly string HeatTreatmentProcessingUnit = "F0000029";
    //双轧切割
    public static readonly string DoubleRollingCutting = "F0000132";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000004";
    //本工序需求期毛坯
    public static readonly string DemandPeriodOfThisOperationblank = "F0000082";
    //计划本取
    public static readonly string PlannedTake = "F0000118";
    //Status
    public static readonly string Status = "Status";
    //返修类型
    public static readonly string RepairType = "F0000130";
    //质检结论粗车
    public static readonly string QualityInspectionconclusionRoughTurning = "F0000047";
    //本工序需求期锻压
    public static readonly string DemandPeriodOfThisProcedureForging = "F0000073";
    //确定本取
    public static readonly string DeterminedTake = "F0000019";
    //锯切加工单位
    public static readonly string SawingProcessingUnit = "F0000024";
    //本工序需求期锯切
    public static readonly string DemandPeriodOfThisProcessSawing = "F0000068";
    //质检结论返修
    public static readonly string QualityInspectionConclusionReturnrepair = "F0000129";
    //热处理外协合同表
    public static readonly string HeatTreatmentOutsourcingContractForm = "F0000078";
    //ID
    public static readonly string ID = "F0000006";
    //质检结论辗环
    public static readonly string QualityInspectionConclusionRingRolling = "F0000036";
    //质检结论热处理
    public static readonly string QualityInspectionConclusionHeatTreatment = "F0000037";
    //本工序需求期钻孔
    public static readonly string DemandPeriodOfThisProcessdrilling = "F0000097";
    //本工序需求期辗环
    public static readonly string DemandPeriodOfThisProcedureRingRolling = "F0000076";
    //毛坯工序转移至人工调整工序
    public static readonly string BlankOperationTransferredToManualAdjustmentOperation = "F0000106";
    //当前工序
    public static readonly string CurrentOperation = "F0000018";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //上一工步
    public static readonly string PreviousStep = "F0000128";
    //精车加工单位
    public static readonly string FinishTurningUnit = "F0000040";
    //质检结论毛坯
    public static readonly string QualityInspectionConclusionblank = "F0000038";
    //本工序需求期热处理
    public static readonly string DemandPeriodOfThisProcedureHeatTreatment = "F0000079";
    //再生品ID
    public static readonly string RecycledProductid = "F0000060";
    //锯切转移至人工调整工序
    public static readonly string SawingTransferredToManualAdjustmentProcess = "F0000102";
    //数据标题
    public static readonly string Name = "Name";
    //毛坯加工单位
    public static readonly string BlankProcessingUnit = "F0000030";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //原料库
    public static readonly string RawMaterialWarehouse = "F0000119";
}
/// <summary>
/// 生产制造流程,工艺流程表
/// </summary>
[Table("工艺流程表")]
public class ProcessFlow
{
    public static readonly string TableCode = "Sq0biizim9l50i2rl6kgbpo3u4";
    public ProcessFlow() { }
    //计划本取
    public static readonly string PlannedTake = "F0000118";
    //确定轧制方式
    public static readonly string DeterminedRollingMethod = "F0000111";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000002";
    //粗车已加工量
    public static readonly string RoughTurningProcessedQuantity = "F0000133";
    //确认炉次计划
    public static readonly string ConfirmedHeatPlan = "F0000125";
    //双轧编号
    public static readonly string DoubleRollingNumber = "F0000127";
    //本工序需求期锻压
    public static readonly string DemandPeriodOfThisProcedureForging = "F0000073";
    //质量状态
    public static readonly string QualityStatus = "F0000065";
    //本工序需求期毛坯
    public static readonly string DemandPeriodOfThisOperationBlank = "F0000082";
    //原材料类型
    public static readonly string RawMaterialType = "F0000062";
    //辗环转移至人工调整工序
    public static readonly string RingRollingTransferredToManualAdjustmentProcedure = "F0000104";
    //质检结论辗环
    public static readonly string QualityInspectionConclusionRingRolling = "F0000036";
    //毛坯工序转移至人工调整工序
    public static readonly string BlankOperationTransferredToManualAdjustmentOperation = "F0000106";
    //计划热处理炉号
    public static readonly string PlannedHeatTreatmentHeatNumber = "F0000124";
    //本工序需求期辗环
    public static readonly string DemandPeriodOfThisProcedureRingRolling = "F0000076";
    //本工序需求期锯切
    public static readonly string DemandPeriodOfThisProcessSawing = "F0000068";
    //炉次计划
    public static readonly string HeatPlan = "F0000115";
    //计划炉次编号
    public static readonly string PlannedHeatNumber = "F0000113";
    //热处理转移至人工调整工序
    public static readonly string HeatTreatmentTransferredToManualAdjustmentOperation = "F0000105";
    //精车加工单位
    public static readonly string FinishTurningUnit = "F0000040";
    //质检结论毛坯
    public static readonly string QualityInspectionConclusionBlank = "F0000038";
    //精车转移至人工调整工序
    public static readonly string FinishTurningTransferredToManualAdjustmentProcess = "F0000108";
    //ID
    public static readonly string ID = "F0000006";
    //粗车工序转移至人工调整工序
    public static readonly string RoughTurningOperationTransferredToManualAdjustmentOperation = "F0000107";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //钻孔加工单位
    public static readonly string DrillingUnit = "F0000041";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000004";
    //订单号
    public static readonly string OrderNumber = "F0000001";
    //完成本取
    public static readonly string CompletedTake = "F0000120";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //质检结论热处理
    public static readonly string QualityInspectionConclusionHeatTreatment = "F0000037";
    //锯切加工单位
    public static readonly string SawingProcessingUnit = "F0000024";
    //原材料编号
    public static readonly string RawMaterialNumber = "F0000058";
    //数据代码
    public static readonly string DataCode = "F0000069";
    //Status
    public static readonly string Status = "Status";
    //再生品ID
    public static readonly string RecycledProductID = "F0000060";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //确认炉次编号
    public static readonly string ConfirmedHeatNumber = "F0000114";
    //确定本取
    public static readonly string DeterminedTake = "F0000019";
    //返修类型
    public static readonly string RepairType = "F0000130";
    //计划轧制方式
    public static readonly string PlannedRollingMethod = "F0000112";
    //本工序需求期精车
    public static readonly string DemandPeriodOfThisProcessFinishTurning = "F0000088";
    //辗环加工单位
    public static readonly string RingRollingProcessingUnit = "F0000028";
    //数据标题
    public static readonly string Name = "Name";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //热处理任务类型
    public static readonly string HeatTreatmentUnitBusinessType = "F0000122";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000003";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //成品单重
    public static readonly string FinishedProductUnitWeight = "F0000017";
    //粗车加工单位
    public static readonly string RoughTurningUnit = "F0000039";
    //热处理外协合同表
    public static readonly string HeatTreatmentOutsourcingContractForm = "F0000078";
    //粗车外协合同表
    public static readonly string RoughTurningOutsourcingContractTable = "F0000084";
    //工件号
    public static readonly string WorkpieceNumber = "F0000005";
    //本工序需求期钻孔
    public static readonly string DemandPeriodOfThisProcessDrilling = "F0000097";
    //辗环外协合同表
    public static readonly string RingRollingOutsourcingContractForm = "F0000075";
    //锻压转移至人工调整工序
    public static readonly string ForgingTransferredToManualAdjustmentProcedure = "F0000103";
    //双轧切割
    public static readonly string DoubleRollingCutting = "F0000132";
    //质检结论锻压
    public static readonly string QualityInspectionConclusionForging = "F0000035";
    //毛坯加工单位
    public static readonly string BlankProcessingUnit = "F0000030";
    //锻压加工单位
    public static readonly string ForgingProcessingUnit = "F0000027";
    //锯切外协合同表
    public static readonly string SawingOutsourcingContractForm = "F0000070";
    //质检结论精车
    public static readonly string QualityInspectionConclusionFinishTurning = "F0000045";
    //转至工序
    public static readonly string TransferToOperation = "F0000056";
    //转至工步
    public static readonly string TransferToStep = "F0000057";
    //质检结论锯切
    public static readonly string QualityInspectionConclusionSawing = "F0000026";
    //工序计划表
    public static readonly string OperationSchedule = "F0000126";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //原料库
    public static readonly string RawMaterialWarehouse = "F0000119";
    //人工调整工序数据分类
    public static readonly string ManuallyAdjustOperationDataClassification = "F0000110";
    //是否忽略理化结果
    public static readonly string IgnorePhysicalAndChemicalResults = "F0000123";
    //再生工序
    public static readonly string RegenerationProcess = "F0000121";
    //钻孔外协合同表
    public static readonly string DrillingOutsourcingContractTable = "F0000096";
    //热处理加工单位
    public static readonly string HeatTreatmentProcessingUnit = "F0000029";
    //精车外协合同表
    public static readonly string FinishTurningOutsourcingContractTable = "F0000087";
    //上一工步
    public static readonly string PreviousStep = "F0000128";
    //质检结论返修
    public static readonly string QualityInspectionConclusionReturnRepair = "F0000129";
    //粗车发起探伤
    public static readonly string RoughTurningInitiatedFlawDetection = "F0000131";
    //本工序需求期粗车
    public static readonly string DemandPeriodOfThisOperationRoughTurning = "F0000085";
    //当前工序
    public static readonly string CurrentOperation = "F0000018";
    //产品种类
    public static readonly string ProductType = "F0000023";
    //钻孔工序转移至人工调整工序
    public static readonly string DrillingProcessTransferredToManualAdjustmentProcess = "F0000109";
    //精车已加工量
    public static readonly string FinishTurningProcessedQuantity = "F0000134";
    //锯切转移至人工调整工序
    public static readonly string SawingTransferredToManualAdjustmentProcess = "F0000102";
    //质检结论钻孔
    public static readonly string QualityInspectionConclusionDrilling = "F0000046";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //锻压外协合同表
    public static readonly string ForgingOutsourcingContractForm = "F0000072";
    //产品名称
    public static readonly string ProductName = "F0000007";
    //本工序需求期热处理
    public static readonly string DemandPeriodOfThisProcedureHeatTreatment = "F0000079";
    //毛坯外协合同表
    public static readonly string BlankOutsourcingContractTable = "F0000081";
    //确认热处理炉号
    public static readonly string ConfirmedHeatTreatmentHeatNumber = "F0000116";
    //质检结论粗车
    public static readonly string QualityInspectionConclusionRoughTurning = "F0000047";
}
/// <summary>
/// 技术质量管理,质量异议
/// </summary>
[Table("质量异议")]
public class QualityObjection
{
    public static readonly string TableCode = "Sw5x3oekte0p227mti924vmf21";
    public QualityObjection() { }
    //工序计划表
    public static readonly string OperationPlan = "F0000030";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //订单号
    public static readonly string OrderNumber = "F0000004";
    //检验类型
    public static readonly string InspectionType = "F0000009";
    //检验数量
    public static readonly string InspectionQuantity = "F0000010";
    //合格率%
    public static readonly string QualifiedRate = "F0000013";

    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //工件号
    public static readonly string WorkpieceNumber = "F0000007";
    //关联辗环
    public static readonly string AssociatedRingRolling = "F0000022";
    //产品来源
    public static readonly string ProductSource = "F0000028";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ID
    public static readonly string ID = "F0000024";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000005";
    //不合格数量
    public static readonly string UnqualifiedQuantity = "F0000011";
    //关联锯切
    public static readonly string AssociatedSawing = "F0000020";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000006";
    //备注
    public static readonly string Remarks = "F0000015";
    //关联锻压
    public static readonly string AssociatedForging = "F0000021";
    //关联异常
    public static readonly string AssociatedAbnormality = "F0000027";
    //关联热处理
    public static readonly string AssociatedHeatTreatment = "F0000023";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //转至工序
    public static readonly string TransferToWorkSequence = "F0000016";
    //所在工序
    public static readonly string Operation = "F0000025";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000026";
    //检验员
    public static readonly string Inspector = "F0000003";
    //转至工步
    public static readonly string GoToWorkStep = "F0000017";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //数据标题
    public static readonly string Name = "Name";
    //检验结论
    public static readonly string InspectionConclusion = "F0000018";
    //日期
    public static readonly string Date = "F0000002";
    //Status
    public static readonly string Status = "Status";
    //合格数量
    public static readonly string QualifiedQuantity = "F0000012";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //数据代码
    public static readonly string DataCode = "F0000029";
}
/// <summary>
/// 生产制造流程,产品参数表
/// </summary>
[Table("产品参数表")]
public class ProductParameter
{
    public static readonly string TableCode = "6b62f7decd924e1e8713025dc6a39aa5";
    public ProductParameter() { }
    //AC孔数
    public static readonly string ACNumberOfHoles = "F0000086";
    //单轧粗车下屑
    public static readonly string SingleRollingRoughTurningChip = "F0000045";
    //侧孔径5（立面）
    public static readonly string SideHoleDiameter5 = "F0000063";
    //合同成品单重
    public static readonly string ContractFinishedProductUnitWeight = "F0000088";
    //侧孔数4（立面）
    public static readonly string NumberOfSideHoles4 = "F0000061";
    //侧孔深1（平面）
    public static readonly string SideSoleSepth1 = "F0000053";
    //产品车加工类别代号
    public static readonly string ProductMachiningCategoryCode = "F0000005";
    //技术规范
    public static readonly string TechnicalSpecification = "F0000072";
    //坡口图
    public static readonly string GrooveDiagram = "F0000071";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //孔数
    public static readonly string NumberOfHoles = "F0000080";
    //孔径
    public static readonly string HoleDiameter = "F0000081";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //材质
    public static readonly string Material = "F0000068";
    //下料单重
    public static readonly string BlankingUnitWeight = "F0000015";
    //规格号
    public static readonly string SpecificationNumber = "F0000066";
    //钻孔下屑
    public static readonly string DrillingChip = "F0000074";
    //产品车加工类别
    public static readonly string ProductMachiningCategory = "F0000004";
    //双轧粗车下屑
    public static readonly string DoubleRollingRoughingChip = "F0000046";
    //侧孔数5（立面）
    public static readonly string NumberOfSideHoles5 = "F0000064";
    //侧孔径2（平面）
    public static readonly string SideHoleDiameter2 = "F0000054";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //侧孔深2（平面）
    public static readonly string SideHoleDepth2 = "F0000056";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //AC外径
    public static readonly string ACOuterDiameter = "F0000082";
    //AC孔径
    public static readonly string ACHoleDiameter = "F0000087";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //精车下屑
    public static readonly string FinishingChip = "F0000047";
    //产品名称
    public static readonly string ProductName = "F0000067";
    //侧孔径3（平面）
    public static readonly string SideHoleDiameter3 = "F0000057";
    //Status
    public static readonly string Status = "Status";
    //内径
    public static readonly string InnerDiameter = "F0000077";
    //侧孔数3（平面）
    public static readonly string SideHoleNumber3 = "F0000058";
    //单轧粗车工时
    public static readonly string SingleRoughingMaNHour = "F0000048";
    //片厚
    public static readonly string SliceThickness = "F0000079";
    //四面见光粗车工时
    public static readonly string FoursidesRoughingManHour = "F0000050";
    //侧孔深5（立面）
    public static readonly string SideHoleDepth5 = "F0000065";
    //钻孔工时
    public static readonly string DrillingManHour = "F0000052";
    //外径
    public static readonly string OuterDiameter = "F0000076";
    //侧孔数2（平面）
    public static readonly string SideHoleNumber2 = "F0000055";
    //双轧粗车工时
    public static readonly string DoubleRoughingManhour = "F0000049";
    //侧孔深3（平面）
    public static readonly string SideHoleDepth3 = "F0000059";
    //成品单重
    public static readonly string FinishedProductUnitWeight = "F0000014";
    //侧孔径4（立面）
    public static readonly string SideHoleDiameter4 = "F0000060";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000073";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //AC总高
    public static readonly string ACTotalHeight = "F0000084";
    //产品钻加工类别代号
    public static readonly string ProductDrillingCategoryCode = "F0000007";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //产品钻加工类别
    public static readonly string ProductDrillingCategory = "F0000006";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //图片
    public static readonly string Picture = "MainDrawing";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //订单号
    public static readonly string OrderNumber = "ProductCode";
    //侧孔深4（立面）
    public static readonly string SIdeHoleDepth4 = "F0000062";
    //精车工时
    public static readonly string FinishingManHour = "F0000051";
    //总高
    public static readonly string TotalHeight = "F0000078";
    //AC内径
    public static readonly string ACInnerDiameter = "F0000083";
    //AC片厚
    public static readonly string ACSheetThickness = "F0000085";
}
/// <summary>
/// 技术工艺管理,设备工时系数表
/// </summary>
[Table("设备工时系数表")]
public class DeviceWorkingHour
{
    public static readonly string TableCode = "5ed7e837ecee4f97800877820d9a2f05";
    public DeviceWorkingHour() { }
    //产品车加工类别
    public static readonly string ProductMachiningCategory = "F0000002";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //产品钻加工类别
    public static readonly string ProductDrillingCategory = "F0000003";
    //数据标题
    public static readonly string Name = "Name";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //工序名称
    public static readonly string OperationName = "F0000001";
    //子表
    public static readonly string SubTable = "D001419Fbb7854d117af4bba8eff4de46d128f63";
    //Status
    public static readonly string Status = "Status";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //单行文本
    public static readonly string SingleLineText = "F0000009";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
}
/// <summary>
/// 技术工艺管理,工序表
/// </summary>
[Table("工序表")]
public class ProcessTable
{
    public static readonly string TableCode = "9016d53506b44f7d95ebbab5a05faf50";
    public ProcessTable() { }
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //工序编号
    public static readonly string OperationNumber = "F0000001";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //Status
    public static readonly string Status = "Status";
    //工序名称
    public static readonly string OperationName = "F0000002";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //数据标题
    public static readonly string Name = "Name";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
}
/// <summary>
/// 技术工艺管理,钻加工类别
/// </summary>
[Table("钻加工类别")]
public class DrillMachiningType
{
    public static readonly string TableCode = "31e1fc7e25d8417dbe2f54a5bf6218bf";
    public DrillMachiningType() { }
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //小类代号
    public static readonly string SubclassCode = "F0000001";
    //Status
    public static readonly string Status = "Status";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //小类名称
    public static readonly string SubclassName = "F0000002";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //数据标题
    public static readonly string Name = "Name";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
}
/// <summary>
/// 技术工艺管理,车加工类别
/// </summary>
[Table("车加工类别")]
public class LatheMachiningType
{
    public static readonly string TableCode = "50a743c942da4709821d273780730402";
    public LatheMachiningType() { }
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //数据标题
    public static readonly string Name = "Name";
    //Status
    public static readonly string Status = "Status";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //类别名称
    public static readonly string CategoryName = "F0000002";
    //类别代号
    public static readonly string CategoryCode = "F0000001";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
}
/// <summary>
/// 生产制造流程,钻孔
/// </summary>
[Table("钻孔")]
public class Drill
{
    public static readonly string TableCode = "Sugyf7m5q744eyhe45o26haop4";
    public Drill() { }
    //备注
    public static readonly string Remarks = "F0000101";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //实际外径
    public static readonly string ActualOuterDiameter = "F0000091";
    //返修类型
    public static readonly string RepairType = "F0000119";
    //产品参数表
    public static readonly string ProductParameterTable = "F0000090";
    //工人
    public static readonly string Worker = "F0000060";
    //当前工序
    public static readonly string CurrentOperation = "F0000056";
    //探伤表
    public static readonly string FlawDetectionTable = "F0000167";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //质检结论
    public static readonly string QualityInspectionConclusion = "F0000111";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000054";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //工件号
    public static readonly string WorkpieceNumber = "F0000001";
    //当前位置
    public static readonly string CurrentLocation = "F0000053";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //加工单位
    public static readonly string ProcessingUnit = "F0000049";
    //实际内径
    public static readonly string ActualInnerDiameter = "F0000092";
    //实际总高
    public static readonly string ActualTotalHeight = "F0000093";
    //发起异常
    public static readonly string InitiateException = "F0000045";
    //转至工步
    public static readonly string GoToWorkStep = "F0000046";
    //异常描述
    public static readonly string ExceptionDescription = "F0000100";
    //钻加工类别
    public static readonly string DrillingCategory = "F0000103";
    //完成总量
    public static readonly string TotalAmountCompleted = "F0000073";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //检验结果
    public static readonly string InspectionResults = "F0000020";
    //当前车间
    public static readonly string CurrentWorkshop = "F0000052";
    //ID
    public static readonly string ID = "F0000029";
    //车加工类别
    public static readonly string MachiningCategory = "F0000107";
    //实际孔径
    public static readonly string ActualHoleDiameter = "F0000096";
    //是否划线绞扣
    public static readonly string ScribingAndWringing = "F0000022";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //数据标题
    public static readonly string Name = "Name";
    //实际加工耗时
    public static readonly string ActualProcessingTime = "CountTime";
    //是否调整至其他工序
    public static readonly string WhetherToAdjustToOtherOperation = "F0000051";
    //工时
    public static readonly string ManHour = "F0000058";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //任务名称
    public static readonly string TaskName = "F0000166";
    //实际片厚
    public static readonly string ActualSheetThickness = "F0000094";
    //转运车间
    public static readonly string TransferWorkshop = "F0000117";
    //实际孔数
    public static readonly string ActualNumberOfHoles = "F0000095";
    //上机互检结果
    public static readonly string ResultsOfMutualInspectionOnMachine = "F0000109";
    //Status
    public static readonly string Status = "Status";
    //本工序需求期
    public static readonly string DemandPeriodOfThisOperation = "F0000071";
    //订单规格表
    public static readonly string OrderSpecificationTable = "F0000098";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //班组
    public static readonly string Team = "F0000070";
    //实际单重
    public static readonly string ActualUnitWeight = "F0000097";
    //数据代码
    public static readonly string DataCode = "F0000048";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000042";
    //单重
    public static readonly string UnitWeight = "F0000089";
    //异常类别
    public static readonly string ExceptionCategory = "F0000055";
    //机加工信息
    public static readonly string MachiningInformation = "D001419F790f3a6b004e4988abe9511380792293";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //上机前互检钻孔
    public static readonly string MutualInspectionBeforeMachineDrilling = "F0000108";
    //转运位置
    public static readonly string TransferTransportationLocation = "F0000118";
}
/// <summary>
/// 生产制造流程,精车
/// </summary>
[Table("精车")]
public class Finishing
{
    public static readonly string TableCode = "Sqy2b1uy8h8cahh17u9kn0jk10";
    public Finishing() { }
    //当前位置
    public static readonly string CurrentLocation = "F0000067";
    //发起异常
    public static readonly string InitiateException = "F0000059";
    //转运位置
    public static readonly string TransferLocation = "F0000128";
    //实际加工耗时
    public static readonly string ActualProcessingTime = "CountTime";
    //产品类别2
    public static readonly string ProductCategory2 = "F0000124";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //互检人
    public static readonly string MutualInspector = "F0000173";
    //完成总量
    public static readonly string TotalAmountCompleted = "F0000086";
    //工人
    public static readonly string Worker = "F0000073";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //实际单重
    public static readonly string ActualUnitWeight = "F0000109";
    //实际总高
    public static readonly string ActualTotalHeight = "F0000107";
    //订单规格表
    public static readonly string OrderSpecificationTable = "F0000110";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //实际内径
    public static readonly string ActualInnerDiameter = "F0000106";
    //实际片厚
    public static readonly string ActualFilmThickness = "F0000108";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000117";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000052";
    //质检结论
    public static readonly string QualityInspectionConclusion = "F0000120";
    //发起探伤
    public static readonly string InitiateFlawDetection = "F0000168";
    //数据标题
    public static readonly string Name = "Name";
    //异常描述
    public static readonly string ExceptionDescription = "F0000115";
    //当前车间
    public static readonly string CurrentWorkshop = "F0000066";
    //产品参数表
    public static readonly string ProductParameterTable = "F0000104";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //互检结果
    public static readonly string MutualInspectionResults = "F0000174";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //是否调整至其他工序
    public static readonly string YesnoAdjustToOtherOperation = "F0000065";
    //ID
    public static readonly string ID = "F0000053";
    //任务名称
    public static readonly string TaskName = "F0000118";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //数据代码
    public static readonly string DataCode = "F0000062";
    //检验结果
    public static readonly string InspectionResults = "F0000018";
    //当前工序
    public static readonly string CurrentOperation = "F0000069";
    //返修类型
    public static readonly string RepairType = "F0000121";
    //重处理类型
    public static readonly string ReprocessingType = "F0000129";
    //班组
    public static readonly string Team = "F0000083";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //产品类别
    public static readonly string ProductCategory = "F0000111";
    //已加工量
    public static readonly string ProcessedQuantity = "F0000170";
    //加工单位
    public static readonly string ProcessingUnit = "F0000063";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //本工序需求期
    public static readonly string ThisoperationDemandPeriod = "F0000084";
    //异常类别
    public static readonly string ExceptionCategory = "F0000055";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //工件号
    public static readonly string WorkpieceNumber = "F0000001";
    //机加工信息
    public static readonly string MachiningInformation = "D001419Fd25eb8064b424ed9855ced1923841f1c";
    //转运车间
    public static readonly string TransferWorkshop = "F0000127";
    //备注
    public static readonly string Remarks = "F0000116";
    //探伤认定
    public static readonly string FlawDetectionIdentification = "F0000138";
    //上机前互检
    public static readonly string MutualInspectionBeforeMachineOperation = "F0000172";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000068";
    //探伤表
    public static readonly string FlawDetectionTable = "F0000167";
    //Status
    public static readonly string Status = "Status";
    //实际外径
    public static readonly string ActualOuterDiameter = "F0000105";
}
/// <summary>
/// 生产制造流程,粗车
/// </summary>
[Table("粗车")]
public class Roughing
{
    public static readonly string TableCode = "Szzswrfsp91x3heen4dykgwus0";
    public Roughing() { }
    //转运车间
    public static readonly string TransportWorkshop = "F0000154";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //当前位置
    public static readonly string CurrentPosition = "F0000081";
    //ID
    public static readonly string ID = "F0000067";
    //备注
    public static readonly string Remarks = "F0000141";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //发起异常
    public static readonly string ApplyException = "F0000075";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //实际单重
    public static readonly string ActualUintWeight = "F0000115";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //任务名称
    public static readonly string TaskName = "F0000133";
    //工件号
    public static readonly string WorkpieceNumber = "F0000033";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //当前工序
    public static readonly string CurrentProcess = "F0000083";
    //互检结果
    public static readonly string MutualInspectionResult = "F0000183";
    //实际内径
    public static readonly string ActualInsideDiameter = "F0000112";
    //转运位置
    public static readonly string TransportPlace = "F0000155";
    //异常描述
    public static readonly string ExceptionDescription = "F0000140";
    //产品参数表
    public static readonly string ParameterList = "F0000116";
    //是否四面光
    public static readonly string IsFourScale = "F0000186";
    //完成本取
    public static readonly string FinishSampling = "F0000134";
    //质检结论
    public static readonly string CheckoutConclusion = "F0000142";
    //重处理类型
    public static readonly string ReprocessingType = "F0000156";
    //异常类别
    public static readonly string ExceptionType = "F0000070";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //本工序需求期
    public static readonly string ProcessNecessityPeriod = "F0000088";
    //互检人
    public static readonly string MutualInspection = "F0000182";
    //已加工量
    public static readonly string Manufactured = "F0000169";
    //Message
    public static readonly string Message = "Message";
    //实际加工耗时
    public static readonly string ActualTimeConsuming = "CountTime";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //实际总高
    public static readonly string ActualTotalHeight = "F0000113";
    //四面光
    public static readonly string FourScale = "D0014199e58919544424654bcc75ef1dc953be6";
    //完成总量
    public static readonly string TotalManufactured = "F0000090";
    //当前工步
    public static readonly string CurrentProcessStep = "F0000082";
    //加工难度
    public static readonly string ProcessingDifficulty = "F0000105";
    //粗加工
    public static readonly string RoughMachining = "D001419F8cbba24c57a74ad99bd809ab8e262996";
    //发起探伤
    public static readonly string ApplyUltrasonic = "F0000139";
    //返修类型
    public static readonly string RepairType = "F0000149";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //产品规格
    public static readonly string Specification = "F0000003";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //产品类别
    public static readonly string ProductType = "F0000121";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //实际外径
    public static readonly string ActualOutsideDiameter = "F0000111";
    //检验结果
    public static readonly string InspectionResult = "F0000023";
    //工人
    public static readonly string Worker = "F0000084";
    //当前车间
    public static readonly string CurrentWorkshop = "F0000080";
    //轧制方式
    public static readonly string RollingMethod = "F0000122";
    //Status
    public static readonly string Status = "Status";
    //实际片厚
    public static readonly string ActualThickness = "F0000114";
}
/// <summary>
/// 生产制造流程,毛坯
/// </summary>
[Table("毛坯")]
public class RoughCast
{
    public static readonly string TableCode = "Sgx7flbvwu9r0u3hail6512uq4";
    public RoughCast() { }
    //工时
    public static readonly string ManHour = "F0000139";
    //异常描述
    public static readonly string ExceptionDescription = "F0000079";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //探伤结果
    public static readonly string FlawDetectionResults = "F0000105";
    //ID
    public static readonly string ID = "F0000058";
    //设备类型
    public static readonly string EquipmentType = "F0000137";
    //异常类别
    public static readonly string ExceptionCategory = "F0000070";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //产品参数表
    public static readonly string ProductParameterTable = "F0000103";
    //使用设备
    public static readonly string EquipmentUsed = "F0000136";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //重处理类型
    public static readonly string ReprocessingType = "F0000140";
    //Status
    public static readonly string Status = "Status";
    //当前位置
    public static readonly string CurrentLocation = "F0000068";
    //本工序需求期
    public static readonly string DemandPeriodOfThisProcedure = "F0000073";
    //所有者
    public static readonly string Owner = "F0000126";
    //当前车间
    public static readonly string CurrentWorkshop = "F0000067";
    //理化结果
    public static readonly string PhysicalAndChemicalResults = "F0000122";
    //确认炉次编号
    public static readonly string ConfirmationHeatNumber = "F0000074";
    //发起异常
    public static readonly string InitiateException = "F0000060";
    //加工单位
    public static readonly string ProcessingUnit = "F0000061";
    //实际加工耗时
    public static readonly string ActualProcessingTime = "CountTime";
    //订单规格表
    public static readonly string OrderSpecificationTable = "F0000102";
    //完成总量
    public static readonly string TotalCompletion = "F0000104";
    //设备编号
    public static readonly string EquipmentNumber = "F0000138";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //返修类型
    public static readonly string RepairType = "F0000127";
    //备注
    public static readonly string Remarks = "F0000080";
    //质检结论
    public static readonly string QualityInspectionConclusion = "qualityResult";
    //检验结果
    public static readonly string InspectionResults = "F0000041";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000072";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000069";
    //班组长
    public static readonly string TeamLeader = "F0000128";
    //工人
    public static readonly string Worker = "F0000135";
    //单重
    public static readonly string UnitWeight = "F0000045";
    //双轧切割
    public static readonly string DoubleRollingCutting = "F0000173";
    //数据标题
    public static readonly string Name = "Name";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //当前本取加工者
    public static readonly string CurrentProcessor = "F0000129";
    //当前工序
    public static readonly string CurrentProcedure = "F0000071";
    //确认热处理炉号
    public static readonly string ConfirmationheatTreatmentFurnaceNumber = "F0000076";
    //转运车间
    public static readonly string TransferWorkshop = "F0000133";
    //完成本取
    public static readonly string CostOfCompletion = "F0000106";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //检验记录
    public static readonly string InspectionRecord = "D001419Fbae2fac51c2f4957aaa45430960bfda8";
    //产品类别
    public static readonly string ProductCategory = "F0000088";
    //确认本取
    public static readonly string ConfirmationBookRetrieval = "F0000040";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000057";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //数据代码
    public static readonly string DataCode = "F0000064";
    //工件号
    public static readonly string WorkpieceNumber = "F0000025";
    //转运位置
    public static readonly string TransferLocation = "F0000134";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //是否开启制样流程
    public static readonly string WhetherToStartTheSamplePreparationProcess = "F0000124";
    //是否忽略理化结果流转
    public static readonly string WhetherToIgnorePhysicalAndChemicalResultsresultFlow = "advanceTransfer";
    //试样类型
    public static readonly string SampleType = "F0000119";
    //轧制方式
    public static readonly string RollingMethod = "F0000039";
    //是否调整至其他工序
    public static readonly string WhetherToAdjustToOtherProcesses = "F0000066";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //理化结果不合格
    public static readonly string UnqualifiedPhysicalAndChemicalResults = "F0000170";
    //计划本取
    public static readonly string PlanBookRetrieval = "F0000077";
    //炉次计划
    public static readonly string HeatPlan = "F0000075";
}
/// <summary>
/// 生产制造流程,热处理
/// </summary>
[Table("热处理")]
public class HeatTreatment
{
    public static readonly string TableCode = "Siizvpn3x17wj6jj3pifsmbic3";
    public HeatTreatment() { }
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //确认热处理炉号
    public static readonly string ConfirmedHeatTreatmentHeatNumber = "F0000069";
    //测试用objectID
    public static readonly string DataCode = "F0000080";
    //计划本取
    public static readonly string PlanEntry = "F0000056";
    //加工单位
    public static readonly string ProcessingUnit = "F0000041";
    //异常类别
    public static readonly string IssueStartException = "F0000049";
    //质检结论
    public static readonly string InspectionBeforeCharging = "F0000072";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //车间位置
    public static readonly string WorkshopLocation = "F0000046";
    //确认炉次编号
    public static readonly string ConfirmedHeatNumber = "F0000068";
    //所有者
    public static readonly string Owner = "F0000077";
    //报废原因
    public static readonly string ScrapReason = "F0000064";
    //炉次计划
    public static readonly string HeatPlan = "F0000052";
    //本工序需求期
    public static readonly string DemandPeriodOfThisOperation = "F0000051";
    //工人
    public static readonly string Worker = "F0000028";
    //备注
    public static readonly string ExceptionDescription = "F0000059";
    //检验结果
    public static readonly string InspectionResult = "F0000067";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //检验确认本取
    public static readonly string CheckAndConfirmTheCopy = "F0000061";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //装炉前检验
    public static readonly string CheckBeforeLoading = "F0000074";
    //轧制方式
    public static readonly string RollingMethod = "F0000031";
    //计划炉次编号
    public static readonly string PlannedHeatNumber = "F0000054";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //计划热处理炉号
    public static readonly string PlannedHeatTreatmentHeatNumber = "F0000055";
    //数据标题
    public static readonly string Name = "Name";
    //检验结果处理前
    public static readonly string InspectionResultBeforeTreatment = "F0000075";
    //检验结果2
    public static readonly string InspectionResult2 = "F0000065";
    //是否调整至其他工序
    public static readonly string Remarks = "F0000045";
    //Status
    public static readonly string Status = "Status";
    //转至工步
    public static readonly string WhetherToAdjustToOtherOperation = "F0000039";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //单重
    public static readonly string SingleWeight = "F0000035";
    //工件号
    public static readonly string WorkpieceNumber = "F0000018";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000037";
    //双轧编号
    public static readonly string DoubleRollingNumber = "F0000079";
    //产品位置
    public static readonly string ProductLocation = "F0000047";
    //检验结果1
    public static readonly string InspectionResult1 = "F0000042";
    //装炉确认本取
    public static readonly string ChargingConfirmationCopy = "F0000062";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //当前工序
    public static readonly string CurrentOperation = "F0000050";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000048";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //返修类型
    public static readonly string RepairType = "F0000078";
    //ID
    public static readonly string ID = "F0000038";
    //返修类型1
    public static readonly string RepairType1 = "F0000060";
    //返修类型2
    public static readonly string RepairType2 = "F0000066";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //任务名称
    public static readonly string TaskName = "F0000081";
    //发起异常
    public static readonly string QualityInspectionConclusion = "F0000040";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //异常描述
    public static readonly string ExceptionCategory = "F0000058";
    //数据代码
    public static readonly string GoToWorkStep = "F0000044";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //(是/否)精整
    public static readonly string IsFinishing = "F0000073";

    //炉内位置号
    public static readonly string HeatInternalPositionNumber = "F0000071";
}
/// <summary>
/// 生产制造流程,辗环
/// </summary>
[Table("辗环")]
public class RollingRing
{
    public static readonly string TableCode = "Saesg17flbcod0mvbdha0kkk44";
    public RollingRing() { }
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //加工总量
    public static readonly string TotalProcessingQuantity = "F0000078";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //单重
    public static readonly string SingleWeight = "F0000040";
    //辗环工人组
    public static readonly string RingRollingWorkerGroup = "F0000032";
    //质检结论
    public static readonly string QualityInspectionConclusion = "F0000064";
    //设备名称
    public static readonly string EquipmentName = "F0000067";
    //本工序需求期
    public static readonly string CurrentOperationDemandPeriod = "F0000058";
    //当前工序
    public static readonly string CurrentOperation = "F0000056";
    //加工单位
    public static readonly string ProcessingUnit = "F0000049";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //任务名称
    public static readonly string TaskName = "F0000077";
    //确定轧制方式
    public static readonly string DetermineRollingMethod = "F0000036";
    //热加工信息
    public static readonly string HotProcessingInformation = "D001419Fc33fc9abe5f2451e83ce06a5edc1669f";
    //数据标题
    public static readonly string Name = "Name";
    //炉号
    public static readonly string HeatNumber = "F0000035";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //产品位置
    public static readonly string ProductLocation = "F0000053";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //Status
    public static readonly string Status = "Status";
    //数据代码
    public static readonly string DataCode = "F0000050";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000047";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000043";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000054";
    //发起异常
    public static readonly string InitiateException = "F0000048";
    //备注
    public static readonly string Remarks = "F0000063";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //车间位置
    public static readonly string WorkshopLocation = "F0000052";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //是否调整至其他工序
    public static readonly string AdjustToOtherOperation = "F0000060";
    //ID
    public static readonly string ID = "F0000044";
    //计划炉次编号
    public static readonly string PlannedHeatNumber = "F0000059";
    //双轧编号
    public static readonly string DoubleRollingNumber = "F0000068";
    //异常描述
    public static readonly string ExceptionDescription = "F0000062";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //工件号
    public static readonly string WorkpieceNumber = "F0000018";
    //测试用objectID
    public static readonly string ObjectidForTest = "F0000069";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //检验结果
    public static readonly string InspectionResult = "F0000045";
    //设备编号
    public static readonly string EquipmentNumber = "F0000066";
    //异常类别
    public static readonly string ExceptionCategory = "F0000055";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
}
/// <summary>
/// 生产制造流程,锻压
/// </summary>
[Table("锻压")]
public class Forge
{
    public static readonly string TableCode = "Sdoly16pnqd5z66wl60hc4y1u1";
    public Forge() { }
    //双轧编号
    public static readonly string DoubleRollingNumber = "F0000064";
    //热加工信息
    public static readonly string HotProcessingInformation = "D001419Fe6ad4c9956ed4788927c31123893dc9e";
    //设备名称
    public static readonly string EquipmentName = "F0000060";
    //当前工序
    public static readonly string CurrentOperation = "F0000053";
    //工件号
    public static readonly string WorkpieceNumber = "F0000018";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000040";
    //锻压班组
    public static readonly string ForgingTeam = "F0000031";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //备注
    public static readonly string Remarks = "F0000059";
    //数据代码
    public static readonly string DataCode = "F0000048";
    //数据标题
    public static readonly string Name = "Name";
    //异常类别
    public static readonly string ExceptionCategory = "F0000046";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //炉号
    public static readonly string HeatNumber = "F0000063";
    //单重
    public static readonly string SingleWeight = "F0000037";
    //产品位置
    public static readonly string ProductLocation = "F0000051";
    //轧制方式
    public static readonly string RollingMethod = "F0000032";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000044";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //完成总量
    public static readonly string TotalAmountCompleted = "F0000082";
    //Status
    public static readonly string Status = "Status";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //异常描述
    public static readonly string ExceptionDescription = "F0000058";
    //加工单位
    public static readonly string ProcessingUnit = "F0000047";
    //ID
    public static readonly string ID = "F0000041";
    //质检结论
    public static readonly string QualityInspectionConclusion = "F0000061";
    //计划炉次编号
    public static readonly string PlannedHeatNumber = "F0000056";
    //是否调整至其他工序
    public static readonly string AdjustToOtherOperations = "F0000049";
    //测试用objectID
    public static readonly string ObjectidForTest = "F0000065";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //任务名称
    public static readonly string TaskName = "F0000081";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000052";
    //检验结果
    public static readonly string InspectionResult = "F0000042";
    //车间位置
    public static readonly string WorkshopLocation = "F0000050";
    //本工序需求期
    public static readonly string ThisOperationDemandPeriod = "F0000055";
    //设备编号
    public static readonly string EquipmentNumber = "F0000007";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //发起异常
    public static readonly string InitiateException = "F0000045";
    //产品名称
    public static readonly string ProductName = "F0000002";
}
/// <summary>
/// 生产制造流程,锯切
/// </summary>
[Table("锯切")]
public class SawCut
{
    public static readonly string TableCode = "So3cw528p3w543tqpt12v28o31";
    public SawCut() { }
    //测试用objectID
    public static readonly string ObjectidForTest = "F0000080";
    //车间位置
    public static readonly string WorkshopLocation = "F0000007";
    //产品位置
    public static readonly string ProductLocation = "F0000065";
    //备注
    public static readonly string Remarks = "F0000072";
    //工件号
    public static readonly string WorkpieceNumber = "F0000018";
    //设备名称
    public static readonly string EquipmentName = "F0000075";
    //当前工序
    public static readonly string CurrentOperation = "F0000067";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //订单规格号
    public static readonly string OrderSpecificationNumber = "F0000016";
    //ID
    public static readonly string ID = "F0000030";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //测试用ID
    public static readonly string IDForTest = "F0000078";
    //异常类别
    public static readonly string ExceptionCategory = "F0000058";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //订单批次号
    public static readonly string OrderBatchNumber = "F0000014";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //轧制方式
    public static readonly string RollingMethod = "F0000031";
    //异常描述
    public static readonly string ExceptionDescription = "F0000071";
    //双轧编号
    public static readonly string DoubleRollingNumber = "F0000079";
    //计划炉次编号
    public static readonly string PlannedHeatNumber = "F0000070";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //加工单位
    public static readonly string ProcessingUnit = "F0000045";
    //是否调整至其他工序
    public static readonly string AdjustToOtherOperation = "F0000063";
    //节点名称
    public static readonly string NodeName = "F0000076";
    //质检结论
    public static readonly string QualityInspectionConclusion = "F0000077";
    //本工序需求期
    public static readonly string ThisOperationDemandPeriod = "F0000068";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //检验结果
    public static readonly string InspectionResult = "F0000043";
    //数据代码
    public static readonly string DataCode = "F0000061";
    //数据标题
    public static readonly string Name = "Name";
    //所有者
    public static readonly string Owner = "F0000028";
    //产品名称
    public static readonly string ProductName = "F0000002";
    //当前工步
    public static readonly string CurrentWorkStep = "F0000056";
    //转至工步
    public static readonly string TransferToWorkStep = "F0000073";
    //发起异常
    public static readonly string InitiateException = "F0000057";
    //Status
    public static readonly string Status = "Status";
    //是否取样
    public static readonly string SampleOrNot = "F0000036";
    //设备编号
    public static readonly string EquipmentNumber = "F0000066";
    //产品规格
    public static readonly string ProductSpecification = "F0000003";
    //订单批次规格号
    public static readonly string OrderBatchSpecificationNumber = "F0000040";
    //原材料号
    public static readonly string RawMaterialNumber = "F0000009";
    //单重
    public static readonly string SingleWeight = "F0000032";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //订单号
    public static readonly string OrderNumber = "F0000012";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
}
/// <summary>
/// 生产制造流程,异常工步记录表
/// </summary>
[Table("异常工步记录表")]
public class AbNormalWorkStep
{
    public static readonly string TableCode = "43239d1b3ebf4ab9b43457e95b2657a7";
    public AbNormalWorkStep() { }
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //工步来源
    public static readonly string StepSource = "workStepSource";
    //Status
    public static readonly string Status = "Status";
    //所属部门
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //修改时间
    public static readonly string ModifiedTime = "ModifiedTime";
    //数据标题
    public static readonly string Name = "Name";
    //ID
    public static readonly string ID = "ID";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //创建时间
    public static readonly string CreatedTime = "CreatedTime";
    //拥有者
    public static readonly string OwnerId = "OwnerId";
    //创建人
    public static readonly string CreatedBy = "CreatedBy";
    //处理耗时
    public static readonly string ProcessingTime = "F0000001";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //异常类别
    public static readonly string ExceptionCategory = "abNormalType";
    //异常描述
    public static readonly string ExceptionDescription = "abNormalDescibe";
    //工序来源
    public static readonly string OperationSource = "processSource";
}

