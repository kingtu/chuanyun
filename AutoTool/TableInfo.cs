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
/// ������Э����,��Э��ͬ
/// </summary>
[Table("��Э��ͬ")]
public class OutsourcingContract
{
    public static readonly string TableCode = "39df0a783f8d4124bb9ee8119e4e1147";
    public OutsourcingContract() { }
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //���칤��
    public static readonly string ForgingProcess = "F0000007";
    //�׷�
    public static readonly string PartyA = "F0000002";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //��Ʒ�б�
    public static readonly string ProductList = "D001419Faebf741cae044316a15f4db3db5ebc18";
    //Status
    public static readonly string Status = "Status";
    //���ݱ���
    public static readonly string Name = "Name";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ҷ�
    public static readonly string PartyB = "F0000004";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //���ݴ���
    public static readonly string DataCode = "F0000008";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��ͬ���
    public static readonly string ContractNumber = "F0000003";
    //ǩ��ʱ��
    public static readonly string SigningTime = "F0000005";
}
/// <summary>
/// ������Э����,��Э����
/// </summary>
[Table("��Э����")]
public class Outsource
{
    public static readonly string TableCode = "Spznzknfgeoyf3x14nmw70r7h2";
    public Outsource() { }
    //��ƽ
    public static readonly string Unevenness = "F0000024";
    //��Э����1
    public static readonly string OutsourcingProcedure1 = "F0000089";
    //��Э��ͬ2
    public static readonly string OutsourcingContract2 = "F0000087";
    //��Բ
    public static readonly string Ellipse = "F0000021";
    //��Э��ͬ1
    public static readonly string OutsourcingContract1 = "F0000086";
    //���ݱ���
    public static readonly string Name = "Name";
    //��Э��ͬ3
    public static readonly string OutsourcingContract3 = "F0000088";
    //��ǰ����
    public static readonly string CurrentStep = "F0000078";
    //��������
    public static readonly string SendingOperation = "F0000005";
    //�������̱�
    public static readonly string ProcessFlowTable = "F0000048";
    //Status
    public static readonly string Status = "Status";
    //������
    public static readonly string WorkpieceNumber = "F0000004";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000010";
    //�ھ�
    public static readonly string InnerDiameter = "F0000019";
    //������λ
    public static readonly string SendingCompany = "F0000006";
    //������
    public static readonly string InspectionResults = "F0000025";
    //��Э����3
    public static readonly string OutsourcingProcedure3 = "F0000091";
    //���չ���
    public static readonly string ReceivingOperation = "F0000009";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000003";
    //ID
    public static readonly string ID = "F0000011";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000079";
    //�ܸ�
    public static readonly string TotalHeight = "F0000023";
    //��Э״̬
    public static readonly string OutsourcingStatus = "F0000028";
    //����
    public static readonly string Title = "F0000017";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�����������ݷ���
    public static readonly string OtherLogisticsDataClassifications = "F0000092";
    //Ƭ��
    public static readonly string SheetThickness = "F0000022";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��Э����2
    public static readonly string OutsourcingProcedure2 = "F0000090";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���ھ�
    public static readonly string NozzleInnerDiameter = "F0000020";
    //���ݴ���
    public static readonly string DataCode = "F0000077";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000002";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�⾶
    public static readonly string OuterDiameter = "F0000018";
    //��Ӧ��
    public static readonly string Supplier = "F0000013";
    //���յ�λ
    public static readonly string ReceivingCompany = "F0000008";
    //���
    public static readonly string Review = "F0000027";
    //����Ա
    public static readonly string Inspector = "F0000026";
    //������
    public static readonly string OrderNumber = "F0000001";
}
/// <summary>
/// �г�����,A���۶�����
/// </summary>
[Table("A���۶�����")]
public class Order
{
    public static readonly string TableCode = "Shla6mrsjywq2pl57mjs5x80y4";
    public Order() { }
    //��Ͳ��̬
    public static readonly string TowerDynamics = "F0000046";
    //������ַ
    public static readonly string ThirdAddress = "F0000069";
    //�����ؿ����
    public static readonly string OrderPaymentCollection = "F0000047";
    //�ͻ����
    public static readonly string CustomerNumber = "F0000061";
    //��Ŀ���
    public static readonly string ProjectNumber = "F0000062";
    //��Ͳ����
    public static readonly string TowerSurvey = "F0000054";
    //��Ŀ����
    public static readonly string ProjectSurvey = "F0000031";
    //�����ͻ�
    public static readonly string HostCustomer = "F0000005";
    //��Ŀ����
    public static readonly string ProjectName = "F0000020";
    //Ĭ����ϵ��
    public static readonly string DefaultContact = "F0000052";
    //Status
    public static readonly string Status = "Status";
    //�����淶
    public static readonly string TechnicalSpecifications = "F0000050";
    //�ֹ�˾����
    public static readonly string BranchName = "F0000073";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ͻ�����
    public static readonly string CustomerRating = "F0000042";
    //�ڶ����
    public static readonly string SecondPartner = "F0000006";
    //������
    public static readonly string DemandFulfillment = "F0000064";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�����ܶ�λ
    public static readonly string TotalOrderTonnage = "F0000057";
    //�ڶ���ַ
    public static readonly string SecondAddress = "F0000068";
    //�������
    public static readonly string ProductionComplete = "F0000065";
    //�����������
    public static readonly string OrderFulfillment = "F0000010";
    //�������
    public static readonly string ShipmentCompletion = "F0000036";
    //ִ������
    public static readonly string ExecutionDate = "F0000018";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //Ĭ�ϵ�ַ
    public static readonly string DefaultAddress = "F0000063";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //���鱸ע��ʷ
    public static readonly string ProposalRemarksHistory = "F0000070";
    //���ݴ���
    public static readonly string DataCode = "F0000041";
    //��Ӧѹ��
    public static readonly string SupplyPressure = "F0000060";
    //��ŵ��
    public static readonly string Promisor = "F0000072";
    //�����ֵ���
    public static readonly string OrderTonUnitPrice = "F0000058";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�����ܼ�
    public static readonly string TotalOrderPrice = "F0000056";
    //��ͬ���
    public static readonly string ContractNumber = "F0000048";
    //������
    public static readonly string OrderNumber = "F0000028";
    //���������
    public static readonly string PaymentAndDeliveryTerms = "F0000059";
    //ǩ������
    public static readonly string SigningDate = "F0000017";
    //ս������
    public static readonly string StrategicSignificance = "F0000022";
    //��������
    public static readonly string OrderSets = "F0000055";
    //��Ŀ����
    public static readonly string ProjectManager = "F0000049";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�����ͻ���
    public static readonly string AssociatedCustomerTable = "F0000032";
    //������
    public static readonly string NumberOfOrders = "F0000043";
    //��Ŀ��̬
    public static readonly string ProjectDynamics = "F0000045";
    //�ܹ�˾����
    public static readonly string HeadOfficeName = "F0000033";
    //���ݱ���
    public static readonly string Name = "Name";
    //��ŵ��ע��ʷ
    public static readonly string CommitmentRemarksHistory = "F0000071";
    //��������
    public static readonly string OrderRating = "F0000044";
    //Ĭ�ϵ绰
    public static readonly string DefaultTelephone = "F0000053";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
}
/// <summary>
/// �г�����,AB�������α�
/// </summary>
[Table("AB�������α�")]
public class OrderBatch
{
    public static readonly string TableCode = "Sv3ey3zxy6sufw6mrqg0p3rv76";
    public OrderBatch() { }
    //��ͬ��������
    public static readonly string NumberOfContractDemandSets = "F0000059";
    //Ĭ�Ϸ��˵�ַ
    public static readonly string DefaultShippingAddress = "F0000072";
    //��Ʒ��������
    public static readonly string NumberOfFinishedProductDemandSets = "F0000058";
    //Status
    public static readonly string Status = "Status";
    //�����������
    public static readonly string OrderPaymentStatus = "F0000054";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //������
    public static readonly string OrderNumber = "F0000001";
    //Ĭ�ϵ绰
    public static readonly string DefaultTelephone = "F0000084";
    //���齻��
    public static readonly string ProposedDeliveryDate = "F0000079";
    //������ǰ����
    public static readonly string ProductionAdvanceDays = "F0000055";
    //Э������
    public static readonly string NegotiatedSets = "F0000066";
    //��ŵ����
    public static readonly string CommittedDeliveryDate = "F0000081";
    //�ڲ���ע
    public static readonly string InProductionRemarks = "F0000067";
    //��ͬ����
    public static readonly string ContractDeliveryDate = "F0000048";
    //��������
    public static readonly string ShippingDate = "F0000023";
    //����ʡ��
    public static readonly string ShippingProvince = "F0000085";
    //��������
    public static readonly string ShippingDetails = "F0000076";
    //������������
    public static readonly string ProductionDelayDays = "F0000038";
    //��Ͳ��̬
    public static readonly string TowerDynamics = "F0000063";
    //�ͻ���������
    public static readonly string CustomerPaymentReputation = "F0000050";
    //���ݴ���
    public static readonly string DataCode = "F0000042";
    //��ŵ����
    public static readonly string CommittedQuantity = "F0000082";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //׼ʱ�깤
    public static readonly string OntimeCompletion = "F0000041";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�������
    public static readonly string ShippingCompletion = "F0000024";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��ŵ��ע
    public static readonly string CommitmentRemarks = "F0000078";
    //��Ʒ������
    public static readonly string FinishedProductDemandPeriod = "F0000016";
    //��Ŀ��̬
    public static readonly string ProjectDynamics = "F0000062";
    //��ͬ����
    public static readonly string ContractBatch = "F0000071";
    //Э�̽���
    public static readonly string NegotiatedDeliveryDate = "F0000049";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ͻ����
    public static readonly string CustomerNumber = "F0000069";
    //��������
    public static readonly string ProposalQuantity = "F0000080";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //���˱�ע
    public static readonly string ShippingRemarks = "F0000068";
    //��Ŀ���
    public static readonly string ProjectNumber = "F0000070";
    //Ĭ����ϵ��
    public static readonly string DefaultContact = "F0000083";
    //�ܹ�˾����
    public static readonly string HeadOfficeName = "F0000026";
    //���ݱ���
    public static readonly string Name = "Name";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //Э�̱���
    public static readonly string NegotiationNotes = "F0000047";
    //��������
    public static readonly string ShippingRegion = "F0000022";
    //�����������
    public static readonly string ProductionCompletionDate = "FinishDate";
    //�������
    public static readonly string ProductionProductionCompletion = "F0000074";
    //��������
    public static readonly string OrderRating = "F0000051";
    //���鱸ע
    public static readonly string ProposalRemarks = "F0000077";
    //���κ�
    public static readonly string BatchNumber = "F0000002";
    //���۶���
    public static readonly string SalesOrder = "F0000009";
    //������
    public static readonly string DemandFulfillment = "F0000073";
    //�ͻ�����
    public static readonly string CustomerRating = "F0000036";
    //��Ʒ������ʱ
    public static readonly string TimeconsumingOfFinishedProducts = "F0000043";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //��Ŀ����
    public static readonly string ProjectName = "F0000020";
    //��ͬ�������
    public static readonly string NumberOfContractDemandPieces = "F0000064";
    //�������̵�
    public static readonly string ProductionTrafficLight = "F0000013";
}
/// <summary>
/// �г�����,AC��������
/// </summary>
[Table("AC��������")]
public class OrderSpecification
{
    public static readonly string TableCode = "Skniz33124ryujrhb4hry7md21";
    public OrderSpecification() { }
    //��Ʒ��������
    public static readonly string ProductFinishingConfiguration = "F0000130";
    //�������������
    public static readonly string SpecificationDemandQuantity = "F0000095";
    //�ܹ�˾����
    public static readonly string HeadOfficeName = "F0000101";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�ڲ���ע
    public static readonly string InProductionRemarks = "F0000114";
    //��ӹ����
    public static readonly string DrillingCategory = "F0000006";
    //�ܸ�
    public static readonly string TotalHeight = "F0000107";
    //�ܷ�˫��
    public static readonly string WhetherDoubleRollingCanBeCarriedOut = "F0000128";
    //�����ߴ�
    public static readonly string SampleSize = "F0000134";
    //��ӹ�������
    public static readonly string DrillingCategoryCode = "F0000007";
    //���ӹ�������
    public static readonly string MachiningCategoryCode = "F0000005";
    //��Ʒ����
    public static readonly string ProductCategory = "F0000118";
    //�����淶
    public static readonly string TechnicalSpecification = "F0000072";
    //ȫ�־���
    public static readonly string GlobalFinishing = "F0000132";
    //�⾶�ı�
    public static readonly string OuterDiameterText = "F0000113";
    //Ƭ��
    public static readonly string SheetThickness = "F0000108";
    //�ֳ��Ƿ������
    public static readonly string WhetherRoughTurningIsSmoothOnAllSides = "F0000136";
    //��ͬ����
    public static readonly string ContractWeight = "F0000100";
    //����ڿ�����
    public static readonly string TotalWeightOfSpecificationInStock = "F0000115";
    //ͼֽ
    public static readonly string Drawing = "F0000117";
    //��Ŀ���
    public static readonly string ItemNumber = "F0000103";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�����ڿ������ı�
    public static readonly string TotalWeightOfOrderInStockText = "F0000112";
    //���δ���������ı�
    public static readonly string SpecificationNotMentionedDemandQuantityText = "F0000093";
    //�������ñ�
    public static readonly string ProcessConfigurationTable = "F0000131";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //�⾶
    public static readonly string OuterDiameter = "F0000105";
    //�ͻ����
    public static readonly string CustomerNumber = "F0000102";
    //��ͬ��Ʒ����
    public static readonly string UnitWeightOfContractFinishedProduct = "F0000104";
    //������
    public static readonly string OrderDocNo = "ProductCode";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //��Ʒ����
    public static readonly string ProductName = "F0000067";
    //�������ı�
    public static readonly string OrderQuantityText = "F0000092";
    //���ݱ���
    public static readonly string Name = "Name";
    //�׾�
    public static readonly string HoleDiameter = "F0000110";
    //���ӹ����
    public static readonly string MachiningCategory = "F0000004";
    //����
    public static readonly string SpecificationNumber = "F0000066";
    //�ھ�
    public static readonly string InnerDiameter = "F0000106";
    //����
    public static readonly string Material = "F0000068";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000076";
    //�����淶2
    public static readonly string TechnicalSpecification2 = "F0000116";
    //���ݴ���
    public static readonly string DataCode = "F0000098";
    //IconId
    public static readonly string Iconid = "IconId";
    //���ϵ���
    public static readonly string BlankingUnitWeight = "F0000015";
    //��Ʒ����
    public static readonly string UnitWeightOfFinishedProduct = "F0000014";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���۶���
    public static readonly string SalesOrder = "F0000089";
    //��Ʒ�ϻ�ǰ����
    public static readonly string MutualInspectionBeforeProductOnMachine = "F0000135";
    //�����ڿ�����
    public static readonly string TotalWeightOfOrderInStock = "F0000111";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //���δ��������
    public static readonly string SpecificationDemandQuantityNotMentioned = "F0000087";
    //Status
    public static readonly string Status = "Status";
    //���߽ʿ�
    public static readonly string ScribingTwistBuckle = "F0000133";
    //�¿�ͼ
    public static readonly string GrooveDiagram = "F0000071";
    //����
    public static readonly string NumberOfHoles = "F0000109";
    //��ͬ��
    public static readonly string ContractQuantity = "F0000077";
    //����ͬ���ı�
    public static readonly string SpecificationContractQuantityText = "F0000099";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
}
/// <summary>
/// �г�����,ABC�������ι���
/// </summary>
[Table("ABC�������ι���")]
public class OrderBatchSpecification
{
    public static readonly string TableCode = "Sh8z1xnes2iju59dzn4ett4bb2";
    public OrderBatchSpecification() { }
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //��������
    public static readonly string OrderSpecificationTable = "F0000019";
    //��Ʒ���ι��ABC
    public static readonly string ProductBatchSpecificationAbc = "F0000066";
    //������������
    public static readonly string DemandQuantityAfterThisBatch = "F0000062";
    //���ë����
    public static readonly string ChangeBlankQuantity = "F0000115";
    //��ɳ�Ʒ��
    public static readonly string NumberOfFinishedProductsReached = "F0000122";
    //�������
    public static readonly string ProductionCompletion = "F0000125";
    //����
    public static readonly string SpecificationNumber = "F0000021";
    //�����Ʒ��ʶ
    public static readonly string ChangeFinishedProductIdentification = "F0000136";
    //�ڿ��Ʒ����
    public static readonly string WeightOfFinishedProductsInStock = "F0000045";
    //������������
    public static readonly string ProductionDifferenceWeight = "F0000093";
    //���δ��������
    public static readonly string DemandQuantityNotMentionedInTheSpecification = "F0000103";
    //��Ʒ������Сʱ
    public static readonly string FinishedProductDeliveryHours = "F0000129";
    //�ڿ��ȼӹ���
    public static readonly string HotWorkInStock = "F0000111";
    //��Ʒ����
    public static readonly string ProductName = "F0000022";
    //��Ʒ�����������
    public static readonly string DemandDifferenceWeightOfFinishedProducts = "F0000132";
    //���κ�
    public static readonly string BatchNumber = "F0000020";
    //������������
    public static readonly string ProductionDelayDays = "DelayedDays";
    //������
    public static readonly string DemandFulfillment = "F0000124";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000026";
    //��Ʒ�����ķ�����
    public static readonly string FinishedProductDeliveryMinutes = "F0000130";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //���ǰ��Ʒ������
    public static readonly string FinishedProductDemandBeforeChange = "F0000106";
    //��������������
    public static readonly string DemandQuantityAfterThisBatchWeightCalculation = "F0000099";
    //��ͬ��Ʒ����
    public static readonly string ContractFinishedProductUnitWeight = "F0000046";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //��Ŀ���
    public static readonly string ProjectNumber = "F0000101";
    //��Ʒ������ʱ
    public static readonly string FinishedProductDeliveryTime = "F0000076";
    //��Ʒ����������
    public static readonly string FinishedProductDeliverySeconds = "F0000134";
    //�������α�
    public static readonly string OrderBatchTable = "F0000012";
    //��Ʒ���AC
    public static readonly string ProductNumberAc = "F0000058";
    //��ӹ��ƻ�
    public static readonly string ColdProcessingPlan = "F0000139";
    //�ۼƳ�Ʒ�������
    public static readonly string CumulativeFinishedProductWarehousingWeight = "F0000092";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //�ڿ���ӹ���
    public static readonly string ColdWorkInStock = "F0000112";
    //��Ʒ������
    public static readonly string FinishedProductDemandPeriod = "F0000027";
    //��Ʒ�������
    public static readonly string DemandDifferenceOfFinishedProducts = "F0000131";
    //�ۼƷ�������
    public static readonly string CumulativeShipmentWeight = "F0000091";
    //�ۼƳ�Ʒ��
    public static readonly string CumulativeFinishedProductQuantity = "F0000071";
    //������������
    public static readonly string ProductionDelaySeconds = "F0000127";
    //��������
    public static readonly string NumberOfPiecesCount = "F0000135";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //���ݱ���
    public static readonly string Name = "Name";
    //�������Ʒ��
    public static readonly string NumberOfFinishedProductsOutsideTheDemand = "F0000121";
    //�����Ʒ��ע
    public static readonly string RemarksOfFinishedProductsChanged = "F0000120";
    //�ڲ�����
    public static readonly string ProductionDetails = "F0000098";
    //�����������
    public static readonly string ProductionCompletionDate = "FinishDate";
    //��Ʒ����AB
    public static readonly string ProductBatchAB = "F0000067";
    //���ݴ���
    public static readonly string DataCode = "F0000079";
    //���δ����������
    public static readonly string DemandWeightNotMentionedInTheSpecification = "F0000104";
    //�ڿ��Ʒ��
    public static readonly string NumberOfFinishedProductsInStock = "F0000064";
    //��������Сʱ
    public static readonly string ProductionDelayHours = "F0000133";
    //��������
    public static readonly string ShipmentDate = "F0000077";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //������ǰ����
    public static readonly string ProductionAdvanceDays = "F0000097";
    //���������������
    public static readonly string DemandWeightRaisedInTheSpecification = "F0000105";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //Status
    public static readonly string Status = "Status";
    //���ë����ע
    public static readonly string ChangeBlankRemarks = "F0000116";
    //���ë����ʶ
    public static readonly string ChangeBlankIdentification = "F0000137";
    //������
    public static readonly string OrderNumber = "F0000017";
    //�ۼƷ�����
    public static readonly string CumulativeShipmentNumber = "F0000063";
    //�ۼ�ë����
    public static readonly string CumulativeBlankQuantity = "F0000108";
    //�������
    public static readonly string ShipmentCompletion = "F0000123";
    //�ȼӹ��ƻ�
    public static readonly string HotProcessingPlan = "F0000138";
    //���ë����
    public static readonly string AchievedBlankQuantity = "F0000117";
    //�������ڷ���
    public static readonly string ProductionDelayMinutes = "F0000126";
    //��������
    public static readonly string ProductionDifference = "F0000065";
    //�������������
    public static readonly string DemandQuantityRaisedInTheSpecification = "F0000102";
    //��Ʒ������
    public static readonly string FinishedProductDemandQuantity = "F0000047";
    //׼ʱ�깤
    public static readonly string OntimeCompletion = "F0000069";
    //�����Ʒ��
    public static readonly string ChangeNumberOfFinishedProducts = "F0000119";
    //��Ʒ��������
    public static readonly string FinishedProductDemandWeight = "F0000081";
}
/// <summary>
/// �г�����,�ͻ���
/// </summary>
[Table("�ͻ���")]
public class Customers
{
    public static readonly string TableCode = "cace8d77e69a4113b425d16711ee46e9";
    public Customers() { }
    //���ݴ���
    public static readonly string DataCode = "F0000020";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�ܹ�˾����
    public static readonly string HeadOfficeName = "F0000003";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //ƽ������ʱ��
    public static readonly string AveragePaymentDuration = "F0000016";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //Status
    public static readonly string Status = "Status";
    //��ϵ��
    public static readonly string ContactPerson = "F0000024";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //����Ƿ���
    public static readonly string TotalAmountOwed = "F0000023";
    //�ͻ�ȫ��
    public static readonly string FullNameOfCustomer = "F0000028";
    //�ͻ���ַ
    public static readonly string CustomerAddress = "F0000006";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ͻ�����
    public static readonly string CustomerName = "F0000029";
    //�ͻ�����
    public static readonly string CustomerReputation = "F0000022";
    //�绰
    public static readonly string Telephone = "F0000009";
    //��Ӫ��ҵ
    public static readonly string MainIndustry = "F0000010";
    //�ͻ���չǱ��
    public static readonly string CustomerDevelopmentPotential = "F0000026";
    //�ͻ����
    public static readonly string CustomerCategory = "F0000027";
    //��Ʊ��Ϣ
    public static readonly string InvoiceInformation = "F0000025";
    //����ͻ�Ƿ����
    public static readonly string AllowableAmountOwedByCustomer = "F0000015";
    //�ͻ�����
    public static readonly string CustomerRating = "F0000004";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //��Ӫ��Ʒ
    public static readonly string MainProduct = "F0000011";
    //���ݱ���
    public static readonly string Name = "Name";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�ͻ����
    public static readonly string CustomerNumber = "SeqNo";
}
/// <summary>
/// ������������,��Ʒ��
/// </summary>
[Table("��Ʒ��")]
public class FinishedStore
{
    public static readonly string TableCode = "Sazlj5e6epn2ek3eiukcbzt321";
    public FinishedStore() { }
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //����
    public static readonly string UnitWeight = "F0000008";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //����ʱ��
    public static readonly string IssueTime = "F0000014";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000015";
    //����λ��
    public static readonly string WorkshopLocation = "F0000019";
    //��ǰ����
    public static readonly string CurrentStep = "F0000021";
    //ID
    public static readonly string ID = "F0000016";
    //���ʱ��
    public static readonly string CompletionTime = "F0000009";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000002";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000006";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //���ݴ���
    public static readonly string DataCode = "F0000018";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000020";
    //��Ʒ����
    public static readonly string ProductName = "F0000005";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000003";
    //������
    public static readonly string OrderNumber = "F0000001";
    //���ݱ���
    public static readonly string Name = "Name";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000022";
    //������
    public static readonly string PieceNumber = "F0000004";
    //���ʱ��
    public static readonly string ReceiptTime = "F0000013";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //Status
    public static readonly string Status = "Status";
}
/// <summary>
/// ������������,�˹���������
/// </summary>
[Table("�˹���������")]
public class ManualAdjustProcess
{
    public static readonly string TableCode = "Se0zvmpq4f9zpi894bxzf5sz35";
    public ManualAdjustProcess() { }
    //ID
    public static readonly string ID = "F0000006";
    //��ǰ����
    public static readonly string CurrentJobStep = "F0000029";
    //�Ƿ�ȡ��
    public static readonly string SamplingOrNot = "F0000019";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000004";
    //����
    public static readonly string Worker = "F0000030";
    //��Ʒ����
    public static readonly string ProductName = "F0000007";
    //������Դ
    public static readonly string OtherSources = "F0000023";
    //������
    public static readonly string OrderNumber = "F0000001";
    //ԭ���Ϻ�
    public static readonly string RawMaterialNumber = "F0000013";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //������
    public static readonly string WorkpieceNumber = "F0000005";
    //���ݱ���
    public static readonly string Name = "Name";
    //���ݷ���
    public static readonly string DataClassification = "F0000024";
    //ת������
    public static readonly string TransferToWorkStep = "F0000022";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000027";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //Status
    public static readonly string Status = "Status";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //����
    public static readonly string UnitWeight = "F0000017";
    //����λ��
    public static readonly string WorkshopLocation = "F0000026";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000014";
    //ת������
    public static readonly string TransferToOperation = "F0000020";
    //���ݴ���
    public static readonly string DataCode = "F0000028";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //���κ�
    public static readonly string BatchNumber = "F0000002";
    //����
    public static readonly string SpecificationNumber = "F0000003";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
}
/// <summary>
/// �����ƻ�,ABCD����ƻ���
/// </summary>
[Table("ABCD����ƻ���")]
public class ABCDProcessPlan
{
    public static readonly string TableCode = "Szlywopbivyrv1d64301ta5xv4";
    public ABCDProcessPlan() { }
    //�ܷ�˫��
    public static readonly string DoubleRollingPossible = "F0000153";
    //Status
    public static readonly string Status = "Status";
    //�ֳ�
    public static readonly string RoughTurning = "D001419e4ec5c3c47594922975c8553366c47d0";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //ȡ��
    public static readonly string Sampling = "D001419Fef5c946a47b04101889b43bf290ada42";
    //�������α�
    public static readonly string OrderBatchTable = "F0000144";
    //����ת�˳�������
    public static readonly string NameOfFinishTurningTransferWorkshop = "F0000195";
    //��Э��ͬ����
    public static readonly string OutsourcingContractFormFinishTurning = "F0000087";
    //��Ʒ������
    public static readonly string FinishedProductDemandPeriod = "F0000021";
    //��Э��ͬ���ѹ
    public static readonly string OutsourcingContractTableForgingPressing = "F0000065";
    //¯�μƻ�
    public static readonly string HeatCountPlan = "F0000138";
    //�ȴ���¯��
    public static readonly string HeatTreatmentFurnaceNumber = "F0000140";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�ӹ���λ��ѹ
    public static readonly string ProcessingUnitForging = "F0000062";
    //����
    public static readonly string FinishTurning = "D001419e351edfd0ae44d3e960e3f1c14991f82";
    //ȫ��װ¯ǰ����
    public static readonly string GlobalInspectionBeforeFurnaceLoading = "F0000191";
    //�ȼӹ��ƻ�
    public static readonly string HotProcessingPlan = "F0000023";
    //��Э��ͬ���ȴ���
    public static readonly string OutsourcingContractTableHeatTreatment = "F0000073";
    //ȫ������
    public static readonly string AllUtilization = "Finish";
    //����װ¯ǰ����
    public static readonly string InspectionBeforeSinglePieceFurnaceLoading = "F0000148";
    //��ǰ�û�
    public static readonly string CurrentUser = "F0000143";
    //�������ñ�
    public static readonly string ProcessConfigurationTable = "F0000158";
    //�������������
    public static readonly string SinglePieceIgnorePhysicochemicalResults = "F0000161";
    //���κ�
    public static readonly string BatchNumber = "F0000125";
    //��������
    public static readonly string OpenProcess = "F0000020";
    //������
    public static readonly string WorkpieceNumber = "F0000054";
    //�ӹ���λ����
    public static readonly string ProcessingUnitSawing = "F0000058";
    //�ƻ���ȡ
    public static readonly string PlanThisOptionTakes = "F0000141";
    //�ӹ���λ�ֳ�
    public static readonly string ProcessingUnitRoughTurning = "F0000078";
    //��ӹ�����
    public static readonly string ColdProcessingDepartment = "F0000135";
    //��Ʒ�ϻ�ǰ����
    public static readonly string MutualInspectionBeforeProductMachineOperation = "F0000189";
    //���
    public static readonly string Drilling = "D001419f342384a36db4bd2ac53ffbc5b86d8b4";
    //�������ñ�
    public static readonly string QualityConfigurationTable = "F0000173";
    //��Э��ͬ�����
    public static readonly string OutsourcingContractFormDrilling = "F0000089";
    //�����ƻ����ʱ��
    public static readonly string FinishTurningPlannedCompletionTime = "F0000098";
    //ȡ��ת�˳���λ��
    public static readonly string LocationOfSamplingTransferWorkshop = "F0000214";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000006";
    //�������̱�
    public static readonly string ProcessFlowTable = "F0000190";
    //�ӹ���λ����
    public static readonly string ProcessingUnitFinishTurning = "F0000082";
    //�ȼӹ�����
    public static readonly string HotProcessingDepartment = "F0000134";
    //�ƻ����Ʒ�ʽ
    public static readonly string PlannedRollingMethod = "F0000152";
    //�ֳ��ƻ����ʱ��
    public static readonly string RoughTurningPlanCompletionTime = "F0000095";
    //�����ϻ�ǰ����
    public static readonly string MutualInspectionBeforeSinglePieceMachineOperation = "F0000175";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000004";
    //���ݴ���
    public static readonly string DataCode = "shuj";
    //������������շ��
    public static readonly string DemandPeriodOfThisOperationRingRolling = "F0000067";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //����ת�˳���λ��
    public static readonly string LocationOfFinishTurningTransferWorkshop = "F0000196";
    //�ӹ���λ�ȴ���
    public static readonly string ProcessingUnitHeatTreatment = "F0000070";
    //��Ʒ����
    public static readonly string ProductName = "F0000025";
    //���ݱ���
    public static readonly string Name = "Name";
    //�ƻ�¯�α��
    public static readonly string PlannedFurnaceNumber = "F0000139";
    //ID
    public static readonly string ID = "F0000007";
    //��Э��ͬ�����
    public static readonly string OutsourcingContractTableSawing = "F0000061";
    //��������
    public static readonly string OrderSpecificationTable = "F0000145";
    //��Ʒ����
    public static readonly string FinishedProductUnitWeight = "F0000031";
    //�������ι���
    public static readonly string OrderBatchSpecificationTable = "F0000017";
    //ԭ���ϱ��
    public static readonly string RawMaterialNumber = "F0000192";
    //ȫ�ֺ��������
    public static readonly string GlobalIgnorePhysicochemicalResults = "F0000174";
    //��Э��ͬ��ֳ�
    public static readonly string OutsourcingContractTableRoughTurning = "F0000081";
    //�ֳ�ת�˳�������
    public static readonly string RoughTurningTransferWorkshopName = "F0000193";
    //ȡ���ƻ����ʱ��
    public static readonly string CompletionTimeOfSamplingPlan = "F0000212";
    //�����������ھ���
    public static readonly string DemandPeriodOfThisProcedureSawing = "F0000059";
    //�������
    public static readonly string RegenerationWarehouseTable = "F0000018";
    //�����������ڶ�ѹ
    public static readonly string DemandPeriodOfThisOperationForging = "F0000063";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //������
    public static readonly string OrderNumber = "F0000001";
    //�ӹ���λ���
    public static readonly string ProcessingUnitDrilling = "F0000084";
    //ȫ���ϻ�ǰ����
    public static readonly string GlobalMutualInspectionBeforeMachineOperation = "F0000185";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //����
    public static readonly string SpecificationNumber = "F0000093";
    //��������
    public static readonly string RegenerationProcess = "F0000142";
    //��ӹ��ƻ�
    public static readonly string ColdProcessingPlan = "F0000024";
    //����״̬
    public static readonly string QualityStatus = "F0000114";
    //ȫ�־�������
    public static readonly string GlobalFinishingConfiguration = "F0000160";
    //ȡ��ת�˳�������
    public static readonly string NameOfSamplingTransferWorkshop = "F0000213";
    //�������������ȴ���
    public static readonly string DemandPeriodOfThisOperationHeatTreatment = "F0000071";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000003";
    //������������
    public static readonly string SinglePieceFinishingConfiguration = "F0000146";
    //��׼ƻ����ʱ��
    public static readonly string DrillingPlannedCompletionTime = "F0000102";
    //�ӹ���λշ��
    public static readonly string ProcessingUnitRingRolling = "F0000066";
    //���ת�˳�������
    public static readonly string NameOfDrillingTransferWorkshop = "F0000197";
    //��Э��ͬ��շ��
    public static readonly string OutsourcingContractTableRingRolling = "F0000069";
    //�ֳ�ת�˳���λ��
    public static readonly string RoughTurningLocationOfTransferWorkshop = "F0000194";
    //����ƷID
    public static readonly string RecycledProductID = "F0000137";
    //��Э��ͬ��ë��
    public static readonly string OutsourcingContractTableBlank = "F0000077";
    //��Ʒ��������
    public static readonly string ProductFinishingConfiguration = "F0000157";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //������
    public static readonly string SpecificationParameters = "F0000028";
    //������������ë��
    public static readonly string DemandPeriodOfThisOperationBlank = "F0000075";
    //�ӹ���λë��
    public static readonly string ProcessingUnitBlank = "F0000074";
    //���ת�˳���λ��
    public static readonly string LocationOfDrillingTransferWorkshop = "F0000198";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //��Ʒ������
    public static readonly string FinishedProductDemandQuantity = "F0000039";
}
/// <summary>
/// ABCD�ƻ�,ABCD����ӱ�
/// </summary>
[Table("ABCD����ӱ�")]
public class ABCDDrillSubTable
{
    public static readonly string TableCode = "f342384a36db4bd2ac53ffbc5b86d8b4";
    public ABCDDrillSubTable() { }
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�豸���
    public static readonly string DeviceNum = "F0000217";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //�ӹ���
    public static readonly string WorkLoad = "F0000219";
    //�豸����
    public static readonly string DeviceName = "F0000216";
    //����
    public static readonly string FullName = "F0000215";
    //���ݱ���
    public static readonly string Name = "Name";
    //��ʱ
    public static readonly string WorkingHours = "F0000220";
}
/// <summary>
/// ABCD�ƻ�,ABCD�����ӱ�
/// </summary>
[Table("ABCD�����ӱ�")]
public class ABCDFinishSubTable
{
    public static readonly string TableCode = "e351edfd0ae44d3e960e3f1c14991f82";
    public ABCDFinishSubTable() { }
    //����
    public static readonly string FullName = "F0000215";
    //�豸���
    public static readonly string DeviceNum = "F0000217";
    //��ʱ
    public static readonly string WorkingHours = "F0000220";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���ݱ���
    public static readonly string Name = "Name";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //�ӹ���
    public static readonly string WorkLoad = "F0000219";
    //�豸����
    public static readonly string DeviceName = "F0000216";
}
/// <summary>
/// ABCD�ƻ�,ABCD�ֳ��ӱ�
/// </summary>
[Table("ABCD�ֳ��ӱ�")]
public class ABCDRoughSubTable
{
    public static readonly string TableCode = "e4ec5c3c47594922975c8553366c47d0";
    public ABCDRoughSubTable() { }
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�豸����
    public static readonly string DeviceName = "F0000216";
    //���ݱ���
    public static readonly string Name = "Name";
    //��ʱ
    public static readonly string WorkingHours = "F0000220";
    //����
    public static readonly string FullName = "F0000215";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //�豸���
    public static readonly string DeviceNum = "F0000217";
    //�ӹ���
    public static readonly string WorkLoad = "F0000219";
}
/// <summary>
/// ABCD����ƻ���,ABCDȡ���ӱ�
/// </summary>
[Table("ABCDȡ���ӱ�")]
public class ABCDSimpleSubTable
{
    public static readonly string TableCode = "Fef5c946a47b04101889b43bf290ada42";
    public ABCDSimpleSubTable() { }
    //��ʱ
    public static readonly string WorkingHours = "F0000220";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���ݱ���
    public static readonly string Name = "Name";
    //�ӹ���
    public static readonly string WorkLoad = "F0000219";
    //�豸���
    public static readonly string DeviceNum = "F0000217";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //�豸����
    public static readonly string DeviceName = "F0000216";
    //����
    public static readonly string FullName = "F0000215";
}
/// <summary>
/// ������������,ȡ��������ӱ�
/// </summary>
[Table("ȡ��������ӱ�")]
public class SamplingFourLathe
{
    public static readonly string TableCode = "a955856dedeb4b27b86c6424f525bbeb";
    public SamplingFourLathe() { }
    //�ӹ���
    public static readonly string WorkLoad = "F0000162";
    //�ӹ���¼
    public static readonly string ProcessRecord = "F0000161";
    //�����Ѷȵ���
    public static readonly string Adjustment = "F0000184";
    //�豸���
    public static readonly string DeviceNum = "F0000159";
    //�ӹ���
    public static readonly string Worker = "F0000157";
    //�豸����
    public static readonly string DeviceType = "F0000160";
    //����ʱ��
    public static readonly string EndTime = "F0000187";
    //�豸ѡ��
    public static readonly string DeviceSelect = "F0000163";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //��ʼʱ��
    public static readonly string StartTime = "F0000164";
    //�Ƿ�̽��
    public static readonly string IsUt = "F0000188";
    //�豸����
    public static readonly string DeviceName = "F0000158";
    //��������
    public static readonly string TaskName = "F0000166";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //���ݱ���
    public static readonly string Name = "Name";
}
/// <summary>
/// ������������,�ֳ�������ӱ�
/// </summary>
[Table("�ֳ�������ӱ�")]
public class RoughFourLathe
{
    public static readonly string TableCode = "9e58919544424654bcc75ef1dc953be6";
    public RoughFourLathe() { }
    //�豸����
    public static readonly string DeviceType = "F0000160";
    //�豸���
    public static readonly string DeviceNum = "F0000159";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�����Ѷȵ���
    public static readonly string Adjustment = "F0000181";
    //�Ƿ�̽��
    public static readonly string IsUt = "F0000168";
    //��ʼʱ��
    public static readonly string StartTime = "F0000164";
    //���ݱ���
    public static readonly string Name = "Name";
    //�ӹ���
    public static readonly string Worker = "F0000157";
    //��������
    public static readonly string TaskName = "F0000166";
    //����ʱ��
    public static readonly string EndTime = "F0000185";
    //�豸ѡ��
    public static readonly string DeviceSelect = "F0000163";
    //�ӹ���
    public static readonly string WorkLoad = "F0000162";
    //ParentObjectId
    public static readonly string ParentObjectId = "ParentObjectId";
    //�豸����
    public static readonly string DeviceName = "F0000158";
    //�ӹ���¼
    public static readonly string ProcessRecord = "F0000161";
}
/// <summary>
/// ������������,ȡ��������
/// </summary>
[Table("ȡ��������")]
public class SamplingSubProcess
{
    public static readonly string TableCode = "Sgljz62e1rneytbqjckbe1vu25";
    public SamplingSubProcess() { }
    //��������
    public static readonly string RepairType = "F0000127";
    //��ǰ����
    public static readonly string CurrentProcess = "F0000071";
    //��������
    public static readonly string TaskName = "F0000082";
    //����
    public static readonly string Worker = "F0000135";
    //ת��λ��
    public static readonly string TransferLocation = "F0000134";
    //�����
    public static readonly string FourSideLight = "D001419a955856dedeb4b27b86c6424f525bbeb";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000039";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000057";
    //�Ƿ��������������
    public static readonly string WhetherToAdjustToOtherProcesses = "F0000066";
    //Status
    public static readonly string Status = "Status";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //������������
    public static readonly string DemandPeriodOfThisProcess = "F0000073";
    //��������ϸ�
    public static readonly string UnqualifiedPhysicalAndChemicalResults = "F0000170";
    //���ݴ���
    public static readonly string DataCode = "F0000064";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //������
    public static readonly string PhysicalAndChemicalData = "D001419F74390f3bba284177a2924f383ae069eb";
    //����̽��
    public static readonly string InitiateFlawDetection = "F0000116";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //̽���϶�
    public static readonly string FlawDetectionIdentification = "F0000174";
    //������Ƿ�̽��
    public static readonly string FourSideLightFlawDetection = "F0000183";
    //������
    public static readonly string OrderNumber = "F0000012";
    //�ƻ���ȡ
    public static readonly string PlanBookRetrieval = "F0000077";
    //ȷ�ϱ�ȡ
    public static readonly string ConfirmationBookRetrieval = "F0000040";
    //���鳤
    public static readonly string TeamLeader = "F0000128";
    //¯�μƻ�
    public static readonly string HeatPlan = "F0000075";
    //̽�˱�
    public static readonly string FlawDetectionTable = "F0000173";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000069";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "qualityResult";
    //ȷ��¯�α��
    public static readonly string ConfirmationFurnaceNumber = "F0000074";
    //̽�˽��
    public static readonly string FlawDetectionResults = "F0000105";
    //��������
    public static readonly string OrderSpecificationTable = "F0000102";
    //�Ƿ�̽��
    public static readonly string FlawDetection = "F0000123";
    //ȷ���ȴ���¯��
    public static readonly string ConfirmationHeatTreatmentFurnaceno = "F0000076";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //�Ƿ�����������
    public static readonly string YesOrNoopenSamplePreparationProcess = "F0000124";
    //�����쳣
    public static readonly string InitiateException = "F0000060";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000061";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ID
    public static readonly string ID = "F0000058";
    //ת�˳���
    public static readonly string TransferWorkshop = "F0000133";
    //������
    public static readonly string WorkpieceNumber = "F0000025";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //��ע
    public static readonly string Remarks = "F0000080";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000070";
    //����
    public static readonly string UnitWeight = "F0000045";
    //ʵ�ʼӹ���ʱ
    public static readonly string ActualProcessingTime = "CountTime";
    //��ɱ�ȡ
    public static readonly string CompletionCost = "F0000106";
    //�������
    public static readonly string TotalAmountCompleted = "F0000104";
    //ȡ��
    public static readonly string Sampling = "D001419Fj7nrmbgha1j10v5zst0zg7hi1";
    //���ݱ���
    public static readonly string Name = "Name";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //��ǰ��ȡ�ӹ���
    public static readonly string CurrentProcessor = "F0000129";
    //��Ʒ������
    public static readonly string ProductParameterTable = "F0000103";
    //������
    public static readonly string Owner = "F0000126";
    //�ش�������
    public static readonly string ReprocessingType = "F0000140";
    //�����
    public static readonly string PhysicalAndChemicalResults = "F0000122";
    //��ǰ����
    public static readonly string CurrentWorkshop = "F0000067";
    //ת������
    public static readonly string TransferToWorkStep = "F0000072";
    //������
    public static readonly string InspectionResults = "F0000041";
    //��ǰλ��
    public static readonly string CurrentLocation = "F0000068";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000079";
    //��������
    public static readonly string SampleType = "F0000119";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //��Ʒ���
    public static readonly string ProductCategory = "F0000088";
}
/// <summary>
/// �����ƻ�,�ɹ���
/// </summary>
[Table("�ɹ���")]
public class Dispatchs
{
    public static readonly string TableCode = "c08bb982ac44481a9439076269a8f783";
    public Dispatchs() { }
    //�ֳ��ƻ����ʱ��
    public static readonly string RoughTurningPlanCompletionTime = "F0000004";
    //�ֳ���������
    public static readonly string RoughTurningWorkshopName = "F0000028";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ID
    public static readonly string ID = "F0000025";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //�����������ɹ�˳��
    public static readonly string FinishTurningUnlimitedDispatchSequence = "F0000043";
    //�ֳ�����⳵��λ��
    public static readonly string RoughFourSideLightWorkshopLocation = "F0000082";
    //�ֳ������ƻ����ʱ��
    public static readonly string RoughFourSideLightPlannedCompletionTime = "F0000075";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //ȡ�������
    public static readonly string samplingTetrahedralLight = "D001419Fbb3556b399a44f998b82f9aa74624afd";
    //�ֳ�����⳵������
    public static readonly string RoughTurningTetrahedralLightWorkshopName = "F0000081";
    //ȡ������ⲻ�����ɹ�˳��
    public static readonly string SamplingTetrahedralLightUnlimitedDispatchSequence = "F0000074";
    //��׳���λ��
    public static readonly string DrillingWorkshopLocation = "F0000033";
    //ȡ����������
    public static readonly string SamplingWorkshopName = "F0000026";
    //��׳�������
    public static readonly string DrillingWorkshopName = "F0000032";
    //����
    public static readonly string SpecificationNumber = "F0000040";
    //ȡ���������ɹ�˳��
    public static readonly string SamplingUnlimitedDispatchSequence = "F0000041";
    //ȡ������⳵��λ��
    public static readonly string SamplingFourSideLightWorkshopLocation = "F0000080";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�ֳ�
    public static readonly string RoughTurning = "D001419Ffb3f2e583e31421e8aaa5a085bbada58";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ֳ������
    public static readonly string RoughTurningTetrahedralLight = "D001419F694a0d18773d4a329ad4e145ccee2bb7";
    //ȡ�������ƻ����ʱ��
    public static readonly string SamplingTetrahedralLightPlanCompletionTime = "F0000073";
    //���ݱ���
    public static readonly string Name = "Name";
    //��������λ��
    public static readonly string FinishTurningWorkshopLocation = "F0000031";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�ֳ�����λ��
    public static readonly string RoughTurningWorkshopLocation = "F0000029";
    //������
    public static readonly string orderNumber = "F0000038";
    //ȡ��
    public static readonly string Sampling = "D001419Fc9380612ad364043a33702a36bf5fde9";
    //Status
    public static readonly string Status = "Status";
    //ȡ������⳵������
    public static readonly string SamplingTetrahedralLightWorkshopNameweighing = "F0000079";
    //�ֳ��������ɹ�˳��
    public static readonly string RoughTurningUnlimitedDispatchSequence = "F0000042";
    //����
    public static readonly string FinishTurning = "D001419F4a23f2f26a01428f952a593da3d99fe5";
    //�ֳ�����ⲻ�����ɹ�˳��
    public static readonly string RoughFourSideLightUnlimitedDispatchSequence = "F0000076";
    //ȡ������λ��
    public static readonly string SamplingWorkshopLocation = "F0000027";
    //��ײ������ɹ�˳��
    public static readonly string DrillingUnlimitedDispatchSequence = "F0000044";
    //�����ƻ����ʱ��
    public static readonly string FinishTurningPlanCompletionTime = "F0000005";
    //������������
    public static readonly string FinishTurningWorkshopName = "F0000030";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���
    public static readonly string Drilling = "D001419F5ccfa7d5acad41bf98c640057f2570ae";
    //ȡ���ƻ����ʱ��
    public static readonly string SamplingPlanCompletionTime = "F0000003";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //������
    public static readonly string WorkpieceNumber = "F0000039";
    //��׼ƻ����ʱ��
    public static readonly string DrillingPlanCompletionTime = "F0000006";
}
/// <summary>
/// ������������,����
/// </summary>
[Table("����")]
public class Repair
{
    public static readonly string TableCode = "Sz2y3t1gjtgld76g5dcl3edj24";
    public Repair() { }
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //ת������
    public static readonly string TransferToProcess = "F0000006";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //���ݴ���
    public static readonly string DataCode = "F0000024";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //Status
    public static readonly string Status = "Status";
    //����
    public static readonly string Worker = "F0000017";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ת������
    public static readonly string TransferToWorkStep = "F0000031";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000020";
    //������Դ
    public static readonly string OperationSource = "F0000035";
    //����λ��
    public static readonly string WorkshopLocation = "F0000022";
    //���ݱ���
    public static readonly string Name = "Name";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000023";
    //�����
    public static readonly string PhysicalAndChemicalResults = "F0000032";
    //¯�α��
    public static readonly string HeatNumber = "F0000027";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //¯�μƻ�
    public static readonly string HeatPlan = "F0000026";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //�������
    public static readonly string PostInspectionProcessing = "F0000034";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�ȴ���¯��
    public static readonly string HeatTreatmentFurnaceNumber = "F0000028";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "F0000036";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //ID
    public static readonly string ID = "F0000021";
    //�ӱ�
    public static readonly string SubTable = "D001419F24790e6009c14d8a9b7e1473ad7d8db7";
    //������
    public static readonly string WorkpieceNumber = "F0000001";
    //������
    public static readonly string OrderNumber = "F0000012";
    //ʹ���豸
    public static readonly string EquipmentUsed = "F0000009";
    //��������
    public static readonly string RepairType = "F0000025";
    //������
    public static readonly string InspectionResults = "F0000033";
}
/// <summary>
/// �����ƻ�,�����������λ���
/// </summary>
[Table("�����������λ���")]
public class WorkshopManager
{
    public static readonly string TableCode = "19b80510f0e24d8695f5e80f8c485fa8";
    public WorkshopManager() { }
    //�����Ա��ɫ
    public static readonly string DrillingPersonnelAngleColor = "F0000005";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //����ɹ���ɫ
    public static readonly string DrillingDispatchingRole = "F0000003";
    //���ݱ���
    public static readonly string Name = "Name";
    //������Ա��ɫ
    public static readonly string FineTurningPersonnelRole = "F0000007";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //����
    public static readonly string Workshop = "F0000004";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //Status
    public static readonly string Status = "Status";
    //�ֳ���Ա��ɫ
    public static readonly string RoughTurningPersonnelRole = "F0000006";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�����ɹ���ɫ
    public static readonly string FineTurningDispatchingRole = "F0000002";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //������Ա��ɫ
    public static readonly string InspectorRole = "F0000008";
    //ȡ���ɹ���ɫ
    public static readonly string SamplingDispatchingRole = "F0000009";
    //�ֳ��ɹ���ɫ
    public static readonly string RoughTurningDispatchingRole = "F0000001";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
}

/// <summary>
/// ���ӹ���Ч,����Ч��
/// </summary>
[Table("����Ч��")]
public class TaskPerformance
{
    public static readonly string TableCode = "22a4f64f7fd74aed89a85e018fca456d";
    public TaskPerformance() { }
    //��������
    public static readonly string OrderSpecificationNumber = "ProductNum";
    //��ʱ����
    public static readonly string WorkhourlyWage = "F0000008";
    //������м����
    public static readonly string ProcessChipWeight = "F0000011";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ܹ�ʱ
    public static readonly string TotalManHours = "F0000006";
    //��������
    public static readonly string OperationName = "F0000003";
    //������
    public static readonly string InspectionResult = "F0000015";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�������
    public static readonly string ToolReplenishmentAmount = "F0000013";
    //�豸����
    public static readonly string EquipmentName = "F0000002";
    //�����ⶨ��ʱ
    public static readonly string PlannedManHoursForASinglePiece = "F0000005";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ID
    public static readonly string ID = "F0000016";
    //�ܹ�����
    public static readonly string TotalWorkload = "gongzuoliang";
    //��������
    public static readonly string TaskName = "F0000004";
    //��������
    public static readonly string DepartmentName = "F0000018";
    //�豸����
    public static readonly string EquipmentType = "F0000020";
    //�ӹ�����
    public static readonly string ProcessingQuantity = "F0000010";
    //Status
    public static readonly string Status = "Status";
    //����м��
    public static readonly string TotalScrap = "F0000012";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string WorkpieceNumber = "F0000014";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //����
    public static readonly string WorkPrice = "F0000009";
    //�������
    public static readonly string TaskCategory = "F0000017";
    //�ӹ���Ա
    public static readonly string Processor = "F0000019";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���ݱ���
    public static readonly string Name = "Name";
}
/// <summary>
/// ���ӹ���Ч,�ӹ������¼
/// </summary>
[Table("�ӹ������¼")]
public class MachiningTaskRecord
{
    public static readonly string TableCode = "4963919529e44d60be759656d4a16b63";
    public MachiningTaskRecord() { }
    //�ܸ�
    public static readonly string TotalHeight = "F0000020";
    //���ӹ����
    public static readonly string DrillingProcessingCategory = "F0000025";
    //�»�ʱ��
    public static readonly string EndTime = "EndTime";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��������
    public static readonly string TaskName = "F0000002";
    //�������Ʒ��ʱ
    public static readonly string ProcessManHour = "F0000004";
    //�ھ�
    public static readonly string InsideDiameter = "F0000018";
    //����
    public static readonly string HoleAmount = "F0000021";
    //Status
    public static readonly string Status = "Status";
    //������
    public static readonly string WorkPieceNumber = "F0000040";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //����ʱ
    public static readonly string TaskManHour = "F0000006";
    //ʵ��Ƭ��
    public static readonly string ActualThickness = "F0000036";
    //��������
    public static readonly string OrderSpecifications = "ProductNum";
    //ʵ���ܸ�
    public static readonly string ActualTotalHeight = "F0000035";
    //ʵ�ʿ׾�
    public static readonly string ActualAperture = "F0000039";
    //�豸����
    public static readonly string DeviceName = "F0000007";
    //��Ʒ����
    public static readonly string ProductName = "F0000026";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�������
    public static readonly string TaskType = "F0000031";
    //ʵ�ʺ�ʱ
    public static readonly string ElapsedTime = "F0000013";
    //̽�˽��
    public static readonly string UltrasonicResults = "F0000029";
    //�׾�
    public static readonly string Aperture = "F0000022";
    //ID
    public static readonly string ID = "ID";
    //�ӹ�����
    public static readonly string WorkLoad = "F0000010";
    //ʵ���ھ�
    public static readonly string ActualInsideDiameter = "F0000034";
    //��������
    public static readonly string IsCumpute = "F0000028";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //��ӹ����
    public static readonly string LatheProcessingCategory = "F0000027";
    //��¼��ʶ
    public static readonly string RecordGuid = "F0000054";
    //�ӹ��Ѷ�
    public static readonly string ProcessDifficulty = "F0000043";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ʵ���⾶
    public static readonly string ActualOutsideDiameter = "F0000033";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //������
    public static readonly string InspectionResults = "F0000009";
    //��������
    public static readonly string DepartmentName = "F0000030";
    //��ע
    public static readonly string Remarks = "F0000042";
    //������м����
    public static readonly string CraftScrapWeight = "F0000023";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�����ⶨ��ʱ
    public static readonly string UnitmanHour = "F0000005";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //�ӹ���Ա
    public static readonly string Worker = "F0000011";
    //��������
    public static readonly string SampleType = "F0000032";
    //��������
    public static readonly string ProcessName = "F0000001";
    //�ϻ�ʱ��
    public static readonly string StartTime = "StartTime";
    //��Ʒ����
    public static readonly string UnitWeightofFinish = "F0000017";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000024";
    //�豸����
    public static readonly string DeviceType = "F0000041";
    //ʵ�ʿ���
    public static readonly string ActualHoleCount = "F0000038";
    //�����Ѷȵ���
    public static readonly string ApplyAdjust = "F0000044";
    //�豸ϵ��
    public static readonly string DeviceCoefficient = "F0000008";
    //ʵ�ʵ���
    public static readonly string Actualunitweight = "F0000037";
    //�⾶
    public static readonly string OutsideDiameter = "F0000016";
    //Ƭ��
    public static readonly string Thickness = "F0000019";
    //�豸���
    public static readonly string DeviceNumber = "F0000014";
}
/// <summary>
/// �����ƻ�,������
/// </summary>
[Table("������")]
public class ReviveWarehouse
{
    public static readonly string TableCode = "Sfb3zsjf4iglhv1sjs995pmho4";
    public ReviveWarehouse() { }
    //�����ͺ�
    public static readonly string MaterialModel = "F0000020";
    //���Ϲ���
    public static readonly string ScrappingProcedure = "F0000005";
    //��������ȴ���
    public static readonly string InspectionInspectionConclusionHeatTreatment = "F0000023";
    //ID
    public static readonly string ID = "F0000018";
    //����ԭ��
    public static readonly string RegenerationReason = "F0000007";
    //����ԭ��
    public static readonly string ScrappingReason = "F0000032";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000017";
    //���ݴ���
    public static readonly string DataCode = "F0000031";
    //Status
    public static readonly string Status = "Status";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000003";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000002";
    //���ݱ���
    public static readonly string Name = "Name";
    //�������շ��
    public static readonly string InspectionConclusionRingRolling = "F0000022";
    //������
    public static readonly string WorkpieceNumber = "F0000004";
    //�������ë��
    public static readonly string InspectionConclusionBlank = "F0000024";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�����쳣��
    public static readonly string QualityExceptionForm = "F0000016";
    //������۶�ѹ
    public static readonly string InspectionConclusionForging = "F0000021";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //������۴ֳ�
    public static readonly string InspectionConclusionRoughTurning = "F0000025";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string Used = "IsUsed";
    //������۾���
    public static readonly string InspectionConclusionFineTurning = "F0000027";
    //����������
    public static readonly string InspectionConclusionDrilling = "F0000029";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //������۾���
    public static readonly string InspectionConclusionSawing = "F0000019";
    //������
    public static readonly string OrderNumber = "F0000001";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
}
/// <summary>
/// �����豸����,�豸����
/// </summary>
[Table("�豸����")]
public class DeviceArchives
{
    public static readonly string TableCode = "Sq0biizim9l50i2rl6kgbpo3u4";
    public DeviceArchives() { }
    //�ȴ���ת�����˹���������
    public static readonly string HeatTreatmentTransferredToManualAdjustmentOperation = "F0000105";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�����������ھ���
    public static readonly string DemandPeriodOfThisProcessFinishTurning = "F0000088";
    //�ֳ��ӹ���λ
    public static readonly string RoughTurningUnit = "F0000039";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000003";
    //��ɱ�ȡ
    public static readonly string CompletedTake = "F0000120";
    //��Ʒ����
    public static readonly string ProductType = "F0000023";
    //������
    public static readonly string WorkpieceNumber = "F0000005";
    //�ƻ��ȴ���¯��
    public static readonly string PlannedHeatTreatmentHeatNumber = "F0000124";
    //ȷ�����Ʒ�ʽ
    public static readonly string DeterminedRollingMethod = "F0000111";
    //ȷ��¯�μƻ�
    public static readonly string ConfirmedHeatPlan = "F0000125";
    //¯�μƻ�
    public static readonly string HeatPlan = "F0000115";
    //ת������
    public static readonly string TransferToStep = "F0000057";
    //ת������
    public static readonly string TransferToOperation = "F0000056";
    //�ֳ�����ת�����˹���������
    public static readonly string RoughTurningOperationTransferredToManualAdjustmentOperation = "F0000107";
    //����ƻ���
    public static readonly string OperationSchedule = "F0000126";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000002";
    //�˹������������ݷ���
    public static readonly string ManuallyAdjustOperationDataClassification = "F0000110";
    //����ת�����˹���������
    public static readonly string FinishTurningTransferredToManualAdjustmentProcess = "F0000108";
    //������
    public static readonly string OrderNumber = "F0000001";
    //ԭ���ϱ��
    public static readonly string RawMaterialNumber = "F0000058";
    //շ��ת�����˹���������
    public static readonly string RingRollingTransferredToManualAdjustmentProcedure = "F0000104";
    //�ʼ���۾���
    public static readonly string QualityinspectionConclusionSawing = "F0000026";
    //�����Ѽӹ���
    public static readonly string FinishTurningProcessedQuantity = "F0000134";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ȷ���ȴ���¯��
    public static readonly string ConfirmedHeatTreatmentHeatNumber = "F0000116";
    //շ����Э��ͬ��
    public static readonly string RingRollingOutsourcingContractForm = "F0000075";
    //��׼ӹ���λ
    public static readonly string DrillingUnit = "F0000041";
    //������Э��ͬ��
    public static readonly string SawingOutsourcingContractForm = "F0000070";
    //�����������ڴֳ�
    public static readonly string DemandPeriodOfThisOperationRoughTurning = "F0000085";
    //��׹���ת�����˹���������
    public static readonly string DrillingProcessTransferredToManualAdjustmentProcess = "F0000109";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //��ѹ��Э��ͬ��
    public static readonly string ForgingOutsourcingContractForm = "F0000072";
    //�ֳ��Ѽӹ���
    public static readonly string RoughTurningProcessedQuantity = "F0000133";
    //�ֳ���Э��ͬ��
    public static readonly string RoughTurningOutsourcingContractTable = "F0000084";
    //˫�����
    public static readonly string DoubleRollingNumber = "F0000127";
    //�ƻ����Ʒ�ʽ
    public static readonly string PlannedRollingMethod = "F0000112";
    //��ѹ�ӹ���λ
    public static readonly string ForgingProcessingUnit = "F0000027";
    //�ȴ�����������
    public static readonly string HeatTreatmentUnitbusinessType = "F0000122";
    //ԭ��������
    public static readonly string RawMaterialType = "F0000062";
    //�ʼ���۶�ѹ
    public static readonly string QualityInspectionConclusionForging = "F0000035";
    //�ʼ���۾���
    public static readonly string QualityInspectionConclusionFinishTurning = "F0000045";
    //��Ʒ����
    public static readonly string FinishedProductUnitWeight = "F0000017";
    //�����Э��ͬ��
    public static readonly string DrillingOutsourcingContractTable = "F0000096";
    //ë����Э��ͬ��
    public static readonly string BlankOutsourcingContractTable = "F0000081";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //������Э��ͬ��
    public static readonly string FinishTurningOutsourcingContractTable = "F0000087";
    //���ݴ���
    public static readonly string DataCode = "F0000069";
    //�ƻ�¯�α��
    public static readonly string PlannedHeatNumber = "F0000113";
    //��Ʒ����
    public static readonly string ProductName = "F0000007";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��ѹת�����˹���������
    public static readonly string ForgingTransferredToManualAdjustmentProcedure = "F0000103";
    //�ʼ�������
    public static readonly string QualityInspectionConclusionDrilling = "F0000046";
    //����״̬
    public static readonly string QualityStatus = "F0000065";
    //ȷ��¯�α��
    public static readonly string ConfirmedHeatNumber = "F0000114";
    //շ���ӹ���λ
    public static readonly string RingRollingProcessingUnit = "F0000028";
    //�Ƿ���������
    public static readonly string IgnorePhysicalAndChemicalResults = "F0000123";
    //�ֳ�����̽��
    public static readonly string RoughTurningInitiatedFlawDetection = "F0000131";
    //��������
    public static readonly string regenerationProcess = "F0000121";
    //�ȴ���ӹ���λ
    public static readonly string HeatTreatmentProcessingUnit = "F0000029";
    //˫���и�
    public static readonly string DoubleRollingCutting = "F0000132";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000004";
    //������������ë��
    public static readonly string DemandPeriodOfThisOperationblank = "F0000082";
    //�ƻ���ȡ
    public static readonly string PlannedTake = "F0000118";
    //Status
    public static readonly string Status = "Status";
    //��������
    public static readonly string RepairType = "F0000130";
    //�ʼ���۴ֳ�
    public static readonly string QualityInspectionconclusionRoughTurning = "F0000047";
    //�����������ڶ�ѹ
    public static readonly string DemandPeriodOfThisProcedureForging = "F0000073";
    //ȷ����ȡ
    public static readonly string DeterminedTake = "F0000019";
    //���мӹ���λ
    public static readonly string SawingProcessingUnit = "F0000024";
    //�����������ھ���
    public static readonly string DemandPeriodOfThisProcessSawing = "F0000068";
    //�ʼ���۷���
    public static readonly string QualityInspectionConclusionReturnrepair = "F0000129";
    //�ȴ�����Э��ͬ��
    public static readonly string HeatTreatmentOutsourcingContractForm = "F0000078";
    //ID
    public static readonly string ID = "F0000006";
    //�ʼ����շ��
    public static readonly string QualityInspectionConclusionRingRolling = "F0000036";
    //�ʼ�����ȴ���
    public static readonly string QualityInspectionConclusionHeatTreatment = "F0000037";
    //���������������
    public static readonly string DemandPeriodOfThisProcessdrilling = "F0000097";
    //������������շ��
    public static readonly string DemandPeriodOfThisProcedureRingRolling = "F0000076";
    //ë������ת�����˹���������
    public static readonly string BlankOperationTransferredToManualAdjustmentOperation = "F0000106";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000018";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //��һ����
    public static readonly string PreviousStep = "F0000128";
    //�����ӹ���λ
    public static readonly string FinishTurningUnit = "F0000040";
    //�ʼ����ë��
    public static readonly string QualityInspectionConclusionblank = "F0000038";
    //�������������ȴ���
    public static readonly string DemandPeriodOfThisProcedureHeatTreatment = "F0000079";
    //����ƷID
    public static readonly string RecycledProductid = "F0000060";
    //����ת�����˹���������
    public static readonly string SawingTransferredToManualAdjustmentProcess = "F0000102";
    //���ݱ���
    public static readonly string Name = "Name";
    //ë���ӹ���λ
    public static readonly string BlankProcessingUnit = "F0000030";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ԭ�Ͽ�
    public static readonly string RawMaterialWarehouse = "F0000119";
}
/// <summary>
/// ������������,�������̱�
/// </summary>
[Table("�������̱�")]
public class ProcessFlow
{
    public static readonly string TableCode = "Sq0biizim9l50i2rl6kgbpo3u4";
    public ProcessFlow() { }
    //�ƻ���ȡ
    public static readonly string PlannedTake = "F0000118";
    //ȷ�����Ʒ�ʽ
    public static readonly string DeterminedRollingMethod = "F0000111";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000002";
    //�ֳ��Ѽӹ���
    public static readonly string RoughTurningProcessedQuantity = "F0000133";
    //ȷ��¯�μƻ�
    public static readonly string ConfirmedHeatPlan = "F0000125";
    //˫�����
    public static readonly string DoubleRollingNumber = "F0000127";
    //�����������ڶ�ѹ
    public static readonly string DemandPeriodOfThisProcedureForging = "F0000073";
    //����״̬
    public static readonly string QualityStatus = "F0000065";
    //������������ë��
    public static readonly string DemandPeriodOfThisOperationBlank = "F0000082";
    //ԭ��������
    public static readonly string RawMaterialType = "F0000062";
    //շ��ת�����˹���������
    public static readonly string RingRollingTransferredToManualAdjustmentProcedure = "F0000104";
    //�ʼ����շ��
    public static readonly string QualityInspectionConclusionRingRolling = "F0000036";
    //ë������ת�����˹���������
    public static readonly string BlankOperationTransferredToManualAdjustmentOperation = "F0000106";
    //�ƻ��ȴ���¯��
    public static readonly string PlannedHeatTreatmentHeatNumber = "F0000124";
    //������������շ��
    public static readonly string DemandPeriodOfThisProcedureRingRolling = "F0000076";
    //�����������ھ���
    public static readonly string DemandPeriodOfThisProcessSawing = "F0000068";
    //¯�μƻ�
    public static readonly string HeatPlan = "F0000115";
    //�ƻ�¯�α��
    public static readonly string PlannedHeatNumber = "F0000113";
    //�ȴ���ת�����˹���������
    public static readonly string HeatTreatmentTransferredToManualAdjustmentOperation = "F0000105";
    //�����ӹ���λ
    public static readonly string FinishTurningUnit = "F0000040";
    //�ʼ����ë��
    public static readonly string QualityInspectionConclusionBlank = "F0000038";
    //����ת�����˹���������
    public static readonly string FinishTurningTransferredToManualAdjustmentProcess = "F0000108";
    //ID
    public static readonly string ID = "F0000006";
    //�ֳ�����ת�����˹���������
    public static readonly string RoughTurningOperationTransferredToManualAdjustmentOperation = "F0000107";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //��׼ӹ���λ
    public static readonly string DrillingUnit = "F0000041";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000004";
    //������
    public static readonly string OrderNumber = "F0000001";
    //��ɱ�ȡ
    public static readonly string CompletedTake = "F0000120";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�ʼ�����ȴ���
    public static readonly string QualityInspectionConclusionHeatTreatment = "F0000037";
    //���мӹ���λ
    public static readonly string SawingProcessingUnit = "F0000024";
    //ԭ���ϱ��
    public static readonly string RawMaterialNumber = "F0000058";
    //���ݴ���
    public static readonly string DataCode = "F0000069";
    //Status
    public static readonly string Status = "Status";
    //����ƷID
    public static readonly string RecycledProductID = "F0000060";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //ȷ��¯�α��
    public static readonly string ConfirmedHeatNumber = "F0000114";
    //ȷ����ȡ
    public static readonly string DeterminedTake = "F0000019";
    //��������
    public static readonly string RepairType = "F0000130";
    //�ƻ����Ʒ�ʽ
    public static readonly string PlannedRollingMethod = "F0000112";
    //�����������ھ���
    public static readonly string DemandPeriodOfThisProcessFinishTurning = "F0000088";
    //շ���ӹ���λ
    public static readonly string RingRollingProcessingUnit = "F0000028";
    //���ݱ���
    public static readonly string Name = "Name";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //�ȴ�����������
    public static readonly string HeatTreatmentUnitBusinessType = "F0000122";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000003";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //��Ʒ����
    public static readonly string FinishedProductUnitWeight = "F0000017";
    //�ֳ��ӹ���λ
    public static readonly string RoughTurningUnit = "F0000039";
    //�ȴ�����Э��ͬ��
    public static readonly string HeatTreatmentOutsourcingContractForm = "F0000078";
    //�ֳ���Э��ͬ��
    public static readonly string RoughTurningOutsourcingContractTable = "F0000084";
    //������
    public static readonly string WorkpieceNumber = "F0000005";
    //���������������
    public static readonly string DemandPeriodOfThisProcessDrilling = "F0000097";
    //շ����Э��ͬ��
    public static readonly string RingRollingOutsourcingContractForm = "F0000075";
    //��ѹת�����˹���������
    public static readonly string ForgingTransferredToManualAdjustmentProcedure = "F0000103";
    //˫���и�
    public static readonly string DoubleRollingCutting = "F0000132";
    //�ʼ���۶�ѹ
    public static readonly string QualityInspectionConclusionForging = "F0000035";
    //ë���ӹ���λ
    public static readonly string BlankProcessingUnit = "F0000030";
    //��ѹ�ӹ���λ
    public static readonly string ForgingProcessingUnit = "F0000027";
    //������Э��ͬ��
    public static readonly string SawingOutsourcingContractForm = "F0000070";
    //�ʼ���۾���
    public static readonly string QualityInspectionConclusionFinishTurning = "F0000045";
    //ת������
    public static readonly string TransferToOperation = "F0000056";
    //ת������
    public static readonly string TransferToStep = "F0000057";
    //�ʼ���۾���
    public static readonly string QualityInspectionConclusionSawing = "F0000026";
    //����ƻ���
    public static readonly string OperationSchedule = "F0000126";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //ԭ�Ͽ�
    public static readonly string RawMaterialWarehouse = "F0000119";
    //�˹������������ݷ���
    public static readonly string ManuallyAdjustOperationDataClassification = "F0000110";
    //�Ƿ���������
    public static readonly string IgnorePhysicalAndChemicalResults = "F0000123";
    //��������
    public static readonly string RegenerationProcess = "F0000121";
    //�����Э��ͬ��
    public static readonly string DrillingOutsourcingContractTable = "F0000096";
    //�ȴ���ӹ���λ
    public static readonly string HeatTreatmentProcessingUnit = "F0000029";
    //������Э��ͬ��
    public static readonly string FinishTurningOutsourcingContractTable = "F0000087";
    //��һ����
    public static readonly string PreviousStep = "F0000128";
    //�ʼ���۷���
    public static readonly string QualityInspectionConclusionReturnRepair = "F0000129";
    //�ֳ�����̽��
    public static readonly string RoughTurningInitiatedFlawDetection = "F0000131";
    //�����������ڴֳ�
    public static readonly string DemandPeriodOfThisOperationRoughTurning = "F0000085";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000018";
    //��Ʒ����
    public static readonly string ProductType = "F0000023";
    //��׹���ת�����˹���������
    public static readonly string DrillingProcessTransferredToManualAdjustmentProcess = "F0000109";
    //�����Ѽӹ���
    public static readonly string FinishTurningProcessedQuantity = "F0000134";
    //����ת�����˹���������
    public static readonly string SawingTransferredToManualAdjustmentProcess = "F0000102";
    //�ʼ�������
    public static readonly string QualityInspectionConclusionDrilling = "F0000046";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //��ѹ��Э��ͬ��
    public static readonly string ForgingOutsourcingContractForm = "F0000072";
    //��Ʒ����
    public static readonly string ProductName = "F0000007";
    //�������������ȴ���
    public static readonly string DemandPeriodOfThisProcedureHeatTreatment = "F0000079";
    //ë����Э��ͬ��
    public static readonly string BlankOutsourcingContractTable = "F0000081";
    //ȷ���ȴ���¯��
    public static readonly string ConfirmedHeatTreatmentHeatNumber = "F0000116";
    //�ʼ���۴ֳ�
    public static readonly string QualityInspectionConclusionRoughTurning = "F0000047";
}
/// <summary>
/// ������������,��������
/// </summary>
[Table("��������")]
public class QualityObjection
{
    public static readonly string TableCode = "Sw5x3oekte0p227mti924vmf21";
    public QualityObjection() { }
    //����ƻ���
    public static readonly string OperationPlan = "F0000030";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string OrderNumber = "F0000004";
    //��������
    public static readonly string InspectionType = "F0000009";
    //��������
    public static readonly string InspectionQuantity = "F0000010";
    //�ϸ���%
    public static readonly string QualifiedRate = "F0000013";

    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //������
    public static readonly string WorkpieceNumber = "F0000007";
    //����շ��
    public static readonly string AssociatedRingRolling = "F0000022";
    //��Ʒ��Դ
    public static readonly string ProductSource = "F0000028";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ID
    public static readonly string ID = "F0000024";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000005";
    //���ϸ�����
    public static readonly string UnqualifiedQuantity = "F0000011";
    //��������
    public static readonly string AssociatedSawing = "F0000020";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000006";
    //��ע
    public static readonly string Remarks = "F0000015";
    //������ѹ
    public static readonly string AssociatedForging = "F0000021";
    //�����쳣
    public static readonly string AssociatedAbnormality = "F0000027";
    //�����ȴ���
    public static readonly string AssociatedHeatTreatment = "F0000023";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ת������
    public static readonly string TransferToWorkSequence = "F0000016";
    //���ڹ���
    public static readonly string Operation = "F0000025";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000026";
    //����Ա
    public static readonly string Inspector = "F0000003";
    //ת������
    public static readonly string GoToWorkStep = "F0000017";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //���ݱ���
    public static readonly string Name = "Name";
    //�������
    public static readonly string InspectionConclusion = "F0000018";
    //����
    public static readonly string Date = "F0000002";
    //Status
    public static readonly string Status = "Status";
    //�ϸ�����
    public static readonly string QualifiedQuantity = "F0000012";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //���ݴ���
    public static readonly string DataCode = "F0000029";
}
/// <summary>
/// ������������,��Ʒ������
/// </summary>
[Table("��Ʒ������")]
public class ProductParameter
{
    public static readonly string TableCode = "6b62f7decd924e1e8713025dc6a39aa5";
    public ProductParameter() { }
    //AC����
    public static readonly string ACNumberOfHoles = "F0000086";
    //�����ֳ���м
    public static readonly string SingleRollingRoughTurningChip = "F0000045";
    //��׾�5�����棩
    public static readonly string SideHoleDiameter5 = "F0000063";
    //��ͬ��Ʒ����
    public static readonly string ContractFinishedProductUnitWeight = "F0000088";
    //�����4�����棩
    public static readonly string NumberOfSideHoles4 = "F0000061";
    //�����1��ƽ�棩
    public static readonly string SideSoleSepth1 = "F0000053";
    //��Ʒ���ӹ�������
    public static readonly string ProductMachiningCategoryCode = "F0000005";
    //�����淶
    public static readonly string TechnicalSpecification = "F0000072";
    //�¿�ͼ
    public static readonly string GrooveDiagram = "F0000071";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //����
    public static readonly string NumberOfHoles = "F0000080";
    //�׾�
    public static readonly string HoleDiameter = "F0000081";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //����
    public static readonly string Material = "F0000068";
    //���ϵ���
    public static readonly string BlankingUnitWeight = "F0000015";
    //����
    public static readonly string SpecificationNumber = "F0000066";
    //�����м
    public static readonly string DrillingChip = "F0000074";
    //��Ʒ���ӹ����
    public static readonly string ProductMachiningCategory = "F0000004";
    //˫���ֳ���м
    public static readonly string DoubleRollingRoughingChip = "F0000046";
    //�����5�����棩
    public static readonly string NumberOfSideHoles5 = "F0000064";
    //��׾�2��ƽ�棩
    public static readonly string SideHoleDiameter2 = "F0000054";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //�����2��ƽ�棩
    public static readonly string SideHoleDepth2 = "F0000056";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //AC�⾶
    public static readonly string ACOuterDiameter = "F0000082";
    //AC�׾�
    public static readonly string ACHoleDiameter = "F0000087";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //������м
    public static readonly string FinishingChip = "F0000047";
    //��Ʒ����
    public static readonly string ProductName = "F0000067";
    //��׾�3��ƽ�棩
    public static readonly string SideHoleDiameter3 = "F0000057";
    //Status
    public static readonly string Status = "Status";
    //�ھ�
    public static readonly string InnerDiameter = "F0000077";
    //�����3��ƽ�棩
    public static readonly string SideHoleNumber3 = "F0000058";
    //�����ֳ���ʱ
    public static readonly string SingleRoughingMaNHour = "F0000048";
    //Ƭ��
    public static readonly string SliceThickness = "F0000079";
    //�������ֳ���ʱ
    public static readonly string FoursidesRoughingManHour = "F0000050";
    //�����5�����棩
    public static readonly string SideHoleDepth5 = "F0000065";
    //��׹�ʱ
    public static readonly string DrillingManHour = "F0000052";
    //�⾶
    public static readonly string OuterDiameter = "F0000076";
    //�����2��ƽ�棩
    public static readonly string SideHoleNumber2 = "F0000055";
    //˫���ֳ���ʱ
    public static readonly string DoubleRoughingManhour = "F0000049";
    //�����3��ƽ�棩
    public static readonly string SideHoleDepth3 = "F0000059";
    //��Ʒ����
    public static readonly string FinishedProductUnitWeight = "F0000014";
    //��׾�4�����棩
    public static readonly string SideHoleDiameter4 = "F0000060";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000073";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //AC�ܸ�
    public static readonly string ACTotalHeight = "F0000084";
    //��Ʒ��ӹ�������
    public static readonly string ProductDrillingCategoryCode = "F0000007";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //��Ʒ��ӹ����
    public static readonly string ProductDrillingCategory = "F0000006";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ͼƬ
    public static readonly string Picture = "MainDrawing";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //������
    public static readonly string OrderNumber = "ProductCode";
    //�����4�����棩
    public static readonly string SIdeHoleDepth4 = "F0000062";
    //������ʱ
    public static readonly string FinishingManHour = "F0000051";
    //�ܸ�
    public static readonly string TotalHeight = "F0000078";
    //AC�ھ�
    public static readonly string ACInnerDiameter = "F0000083";
    //ACƬ��
    public static readonly string ACSheetThickness = "F0000085";
}
/// <summary>
/// �������չ���,�豸��ʱϵ����
/// </summary>
[Table("�豸��ʱϵ����")]
public class DeviceWorkingHour
{
    public static readonly string TableCode = "5ed7e837ecee4f97800877820d9a2f05";
    public DeviceWorkingHour() { }
    //��Ʒ���ӹ����
    public static readonly string ProductMachiningCategory = "F0000002";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��Ʒ��ӹ����
    public static readonly string ProductDrillingCategory = "F0000003";
    //���ݱ���
    public static readonly string Name = "Name";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //��������
    public static readonly string OperationName = "F0000001";
    //�ӱ�
    public static readonly string SubTable = "D001419Fbb7854d117af4bba8eff4de46d128f63";
    //Status
    public static readonly string Status = "Status";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //�����ı�
    public static readonly string SingleLineText = "F0000009";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
}
/// <summary>
/// �������չ���,�����
/// </summary>
[Table("�����")]
public class ProcessTable
{
    public static readonly string TableCode = "9016d53506b44f7d95ebbab5a05faf50";
    public ProcessTable() { }
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //������
    public static readonly string OperationNumber = "F0000001";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //Status
    public static readonly string Status = "Status";
    //��������
    public static readonly string OperationName = "F0000002";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //���ݱ���
    public static readonly string Name = "Name";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
}
/// <summary>
/// �������չ���,��ӹ����
/// </summary>
[Table("��ӹ����")]
public class DrillMachiningType
{
    public static readonly string TableCode = "31e1fc7e25d8417dbe2f54a5bf6218bf";
    public DrillMachiningType() { }
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //С�����
    public static readonly string SubclassCode = "F0000001";
    //Status
    public static readonly string Status = "Status";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //С������
    public static readonly string SubclassName = "F0000002";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //���ݱ���
    public static readonly string Name = "Name";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
}
/// <summary>
/// �������չ���,���ӹ����
/// </summary>
[Table("���ӹ����")]
public class LatheMachiningType
{
    public static readonly string TableCode = "50a743c942da4709821d273780730402";
    public LatheMachiningType() { }
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //���ݱ���
    public static readonly string Name = "Name";
    //Status
    public static readonly string Status = "Status";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�������
    public static readonly string CategoryName = "F0000002";
    //������
    public static readonly string CategoryCode = "F0000001";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
}
/// <summary>
/// ������������,���
/// </summary>
[Table("���")]
public class Drill
{
    public static readonly string TableCode = "Sugyf7m5q744eyhe45o26haop4";
    public Drill() { }
    //��ע
    public static readonly string Remarks = "F0000101";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //ʵ���⾶
    public static readonly string ActualOuterDiameter = "F0000091";
    //��������
    public static readonly string RepairType = "F0000119";
    //��Ʒ������
    public static readonly string ProductParameterTable = "F0000090";
    //����
    public static readonly string Worker = "F0000060";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000056";
    //̽�˱�
    public static readonly string FlawDetectionTable = "F0000167";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "F0000111";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000054";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //������
    public static readonly string WorkpieceNumber = "F0000001";
    //��ǰλ��
    public static readonly string CurrentLocation = "F0000053";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000049";
    //ʵ���ھ�
    public static readonly string ActualInnerDiameter = "F0000092";
    //ʵ���ܸ�
    public static readonly string ActualTotalHeight = "F0000093";
    //�����쳣
    public static readonly string InitiateException = "F0000045";
    //ת������
    public static readonly string GoToWorkStep = "F0000046";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000100";
    //��ӹ����
    public static readonly string DrillingCategory = "F0000103";
    //�������
    public static readonly string TotalAmountCompleted = "F0000073";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //������
    public static readonly string InspectionResults = "F0000020";
    //��ǰ����
    public static readonly string CurrentWorkshop = "F0000052";
    //ID
    public static readonly string ID = "F0000029";
    //���ӹ����
    public static readonly string MachiningCategory = "F0000107";
    //ʵ�ʿ׾�
    public static readonly string ActualHoleDiameter = "F0000096";
    //�Ƿ��߽ʿ�
    public static readonly string ScribingAndWringing = "F0000022";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //���ݱ���
    public static readonly string Name = "Name";
    //ʵ�ʼӹ���ʱ
    public static readonly string ActualProcessingTime = "CountTime";
    //�Ƿ��������������
    public static readonly string WhetherToAdjustToOtherOperation = "F0000051";
    //��ʱ
    public static readonly string ManHour = "F0000058";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //������
    public static readonly string OrderNumber = "F0000012";
    //��������
    public static readonly string TaskName = "F0000166";
    //ʵ��Ƭ��
    public static readonly string ActualSheetThickness = "F0000094";
    //ת�˳���
    public static readonly string TransferWorkshop = "F0000117";
    //ʵ�ʿ���
    public static readonly string ActualNumberOfHoles = "F0000095";
    //�ϻ�������
    public static readonly string ResultsOfMutualInspectionOnMachine = "F0000109";
    //Status
    public static readonly string Status = "Status";
    //������������
    public static readonly string DemandPeriodOfThisOperation = "F0000071";
    //��������
    public static readonly string OrderSpecificationTable = "F0000098";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //����
    public static readonly string Team = "F0000070";
    //ʵ�ʵ���
    public static readonly string ActualUnitWeight = "F0000097";
    //���ݴ���
    public static readonly string DataCode = "F0000048";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000042";
    //����
    public static readonly string UnitWeight = "F0000089";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000055";
    //���ӹ���Ϣ
    public static readonly string MachiningInformation = "D001419F790f3a6b004e4988abe9511380792293";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //�ϻ�ǰ�������
    public static readonly string MutualInspectionBeforeMachineDrilling = "F0000108";
    //ת��λ��
    public static readonly string TransferTransportationLocation = "F0000118";
}
/// <summary>
/// ������������,����
/// </summary>
[Table("����")]
public class Finishing
{
    public static readonly string TableCode = "Sqy2b1uy8h8cahh17u9kn0jk10";
    public Finishing() { }
    //��ǰλ��
    public static readonly string CurrentLocation = "F0000067";
    //�����쳣
    public static readonly string InitiateException = "F0000059";
    //ת��λ��
    public static readonly string TransferLocation = "F0000128";
    //ʵ�ʼӹ���ʱ
    public static readonly string ActualProcessingTime = "CountTime";
    //��Ʒ���2
    public static readonly string ProductCategory2 = "F0000124";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //������
    public static readonly string MutualInspector = "F0000173";
    //�������
    public static readonly string TotalAmountCompleted = "F0000086";
    //����
    public static readonly string Worker = "F0000073";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ʵ�ʵ���
    public static readonly string ActualUnitWeight = "F0000109";
    //ʵ���ܸ�
    public static readonly string ActualTotalHeight = "F0000107";
    //��������
    public static readonly string OrderSpecificationTable = "F0000110";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //ʵ���ھ�
    public static readonly string ActualInnerDiameter = "F0000106";
    //ʵ��Ƭ��
    public static readonly string ActualFilmThickness = "F0000108";
    //ת������
    public static readonly string TransferToWorkStep = "F0000117";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000052";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "F0000120";
    //����̽��
    public static readonly string InitiateFlawDetection = "F0000168";
    //���ݱ���
    public static readonly string Name = "Name";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000115";
    //��ǰ����
    public static readonly string CurrentWorkshop = "F0000066";
    //��Ʒ������
    public static readonly string ProductParameterTable = "F0000104";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string MutualInspectionResults = "F0000174";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //�Ƿ��������������
    public static readonly string YesnoAdjustToOtherOperation = "F0000065";
    //ID
    public static readonly string ID = "F0000053";
    //��������
    public static readonly string TaskName = "F0000118";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //���ݴ���
    public static readonly string DataCode = "F0000062";
    //������
    public static readonly string InspectionResults = "F0000018";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000069";
    //��������
    public static readonly string RepairType = "F0000121";
    //�ش�������
    public static readonly string ReprocessingType = "F0000129";
    //����
    public static readonly string Team = "F0000083";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //��Ʒ���
    public static readonly string ProductCategory = "F0000111";
    //�Ѽӹ���
    public static readonly string ProcessedQuantity = "F0000170";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000063";
    //������
    public static readonly string OrderNumber = "F0000012";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //������������
    public static readonly string ThisoperationDemandPeriod = "F0000084";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000055";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //������
    public static readonly string WorkpieceNumber = "F0000001";
    //���ӹ���Ϣ
    public static readonly string MachiningInformation = "D001419Fd25eb8064b424ed9855ced1923841f1c";
    //ת�˳���
    public static readonly string TransferWorkshop = "F0000127";
    //��ע
    public static readonly string Remarks = "F0000116";
    //̽���϶�
    public static readonly string FlawDetectionIdentification = "F0000138";
    //�ϻ�ǰ����
    public static readonly string MutualInspectionBeforeMachineOperation = "F0000172";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000068";
    //̽�˱�
    public static readonly string FlawDetectionTable = "F0000167";
    //Status
    public static readonly string Status = "Status";
    //ʵ���⾶
    public static readonly string ActualOuterDiameter = "F0000105";
}
/// <summary>
/// ������������,�ֳ�
/// </summary>
[Table("�ֳ�")]
public class Roughing
{
    public static readonly string TableCode = "Szzswrfsp91x3heen4dykgwus0";
    public Roughing() { }
    //ת�˳���
    public static readonly string TransportWorkshop = "F0000154";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��ǰλ��
    public static readonly string CurrentPosition = "F0000081";
    //ID
    public static readonly string ID = "F0000067";
    //��ע
    public static readonly string Remarks = "F0000141";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�����쳣
    public static readonly string ApplyException = "F0000075";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ʵ�ʵ���
    public static readonly string ActualUintWeight = "F0000115";
    //������
    public static readonly string OrderNumber = "F0000012";
    //��������
    public static readonly string TaskName = "F0000133";
    //������
    public static readonly string WorkpieceNumber = "F0000033";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��ǰ����
    public static readonly string CurrentProcess = "F0000083";
    //������
    public static readonly string MutualInspectionResult = "F0000183";
    //ʵ���ھ�
    public static readonly string ActualInsideDiameter = "F0000112";
    //ת��λ��
    public static readonly string TransportPlace = "F0000155";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000140";
    //��Ʒ������
    public static readonly string ParameterList = "F0000116";
    //�Ƿ������
    public static readonly string IsFourScale = "F0000186";
    //��ɱ�ȡ
    public static readonly string FinishSampling = "F0000134";
    //�ʼ����
    public static readonly string CheckoutConclusion = "F0000142";
    //�ش�������
    public static readonly string ReprocessingType = "F0000156";
    //�쳣���
    public static readonly string ExceptionType = "F0000070";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //������������
    public static readonly string ProcessNecessityPeriod = "F0000088";
    //������
    public static readonly string MutualInspection = "F0000182";
    //�Ѽӹ���
    public static readonly string Manufactured = "F0000169";
    //Message
    public static readonly string Message = "Message";
    //ʵ�ʼӹ���ʱ
    public static readonly string ActualTimeConsuming = "CountTime";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //ʵ���ܸ�
    public static readonly string ActualTotalHeight = "F0000113";
    //�����
    public static readonly string FourScale = "D0014199e58919544424654bcc75ef1dc953be6";
    //�������
    public static readonly string TotalManufactured = "F0000090";
    //��ǰ����
    public static readonly string CurrentProcessStep = "F0000082";
    //�ӹ��Ѷ�
    public static readonly string ProcessingDifficulty = "F0000105";
    //�ּӹ�
    public static readonly string RoughMachining = "D001419F8cbba24c57a74ad99bd809ab8e262996";
    //����̽��
    public static readonly string ApplyUltrasonic = "F0000139";
    //��������
    public static readonly string RepairType = "F0000149";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //��Ʒ���
    public static readonly string Specification = "F0000003";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //��Ʒ���
    public static readonly string ProductType = "F0000121";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //ʵ���⾶
    public static readonly string ActualOutsideDiameter = "F0000111";
    //������
    public static readonly string InspectionResult = "F0000023";
    //����
    public static readonly string Worker = "F0000084";
    //��ǰ����
    public static readonly string CurrentWorkshop = "F0000080";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000122";
    //Status
    public static readonly string Status = "Status";
    //ʵ��Ƭ��
    public static readonly string ActualThickness = "F0000114";
}
/// <summary>
/// ������������,ë��
/// </summary>
[Table("ë��")]
public class RoughCast
{
    public static readonly string TableCode = "Sgx7flbvwu9r0u3hail6512uq4";
    public RoughCast() { }
    //��ʱ
    public static readonly string ManHour = "F0000139";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000079";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //̽�˽��
    public static readonly string FlawDetectionResults = "F0000105";
    //ID
    public static readonly string ID = "F0000058";
    //�豸����
    public static readonly string EquipmentType = "F0000137";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000070";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //��Ʒ������
    public static readonly string ProductParameterTable = "F0000103";
    //ʹ���豸
    public static readonly string EquipmentUsed = "F0000136";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�ش�������
    public static readonly string ReprocessingType = "F0000140";
    //Status
    public static readonly string Status = "Status";
    //��ǰλ��
    public static readonly string CurrentLocation = "F0000068";
    //������������
    public static readonly string DemandPeriodOfThisProcedure = "F0000073";
    //������
    public static readonly string Owner = "F0000126";
    //��ǰ����
    public static readonly string CurrentWorkshop = "F0000067";
    //�����
    public static readonly string PhysicalAndChemicalResults = "F0000122";
    //ȷ��¯�α��
    public static readonly string ConfirmationHeatNumber = "F0000074";
    //�����쳣
    public static readonly string InitiateException = "F0000060";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000061";
    //ʵ�ʼӹ���ʱ
    public static readonly string ActualProcessingTime = "CountTime";
    //��������
    public static readonly string OrderSpecificationTable = "F0000102";
    //�������
    public static readonly string TotalCompletion = "F0000104";
    //�豸���
    public static readonly string EquipmentNumber = "F0000138";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //��������
    public static readonly string RepairType = "F0000127";
    //��ע
    public static readonly string Remarks = "F0000080";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "qualityResult";
    //������
    public static readonly string InspectionResults = "F0000041";
    //ת������
    public static readonly string TransferToWorkStep = "F0000072";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000069";
    //���鳤
    public static readonly string TeamLeader = "F0000128";
    //����
    public static readonly string Worker = "F0000135";
    //����
    public static readonly string UnitWeight = "F0000045";
    //˫���и�
    public static readonly string DoubleRollingCutting = "F0000173";
    //���ݱ���
    public static readonly string Name = "Name";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //��ǰ��ȡ�ӹ���
    public static readonly string CurrentProcessor = "F0000129";
    //��ǰ����
    public static readonly string CurrentProcedure = "F0000071";
    //ȷ���ȴ���¯��
    public static readonly string ConfirmationheatTreatmentFurnaceNumber = "F0000076";
    //ת�˳���
    public static readonly string TransferWorkshop = "F0000133";
    //��ɱ�ȡ
    public static readonly string CostOfCompletion = "F0000106";
    //������
    public static readonly string OrderNumber = "F0000012";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //�����¼
    public static readonly string InspectionRecord = "D001419Fbae2fac51c2f4957aaa45430960bfda8";
    //��Ʒ���
    public static readonly string ProductCategory = "F0000088";
    //ȷ�ϱ�ȡ
    public static readonly string ConfirmationBookRetrieval = "F0000040";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000057";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //���ݴ���
    public static readonly string DataCode = "F0000064";
    //������
    public static readonly string WorkpieceNumber = "F0000025";
    //ת��λ��
    public static readonly string TransferLocation = "F0000134";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //�Ƿ�����������
    public static readonly string WhetherToStartTheSamplePreparationProcess = "F0000124";
    //�Ƿ�����������ת
    public static readonly string WhetherToIgnorePhysicalAndChemicalResultsresultFlow = "advanceTransfer";
    //��������
    public static readonly string SampleType = "F0000119";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000039";
    //�Ƿ��������������
    public static readonly string WhetherToAdjustToOtherProcesses = "F0000066";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //��������ϸ�
    public static readonly string UnqualifiedPhysicalAndChemicalResults = "F0000170";
    //�ƻ���ȡ
    public static readonly string PlanBookRetrieval = "F0000077";
    //¯�μƻ�
    public static readonly string HeatPlan = "F0000075";
}
/// <summary>
/// ������������,�ȴ���
/// </summary>
[Table("�ȴ���")]
public class HeatTreatment
{
    public static readonly string TableCode = "Siizvpn3x17wj6jj3pifsmbic3";
    public HeatTreatment() { }
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ȷ���ȴ���¯��
    public static readonly string ConfirmedHeatTreatmentHeatNumber = "F0000069";
    //������objectID
    public static readonly string DataCode = "F0000080";
    //�ƻ���ȡ
    public static readonly string PlanEntry = "F0000056";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000041";
    //�쳣���
    public static readonly string IssueStartException = "F0000049";
    //�ʼ����
    public static readonly string InspectionBeforeCharging = "F0000072";
    //������
    public static readonly string OrderNumber = "F0000012";
    //����λ��
    public static readonly string WorkshopLocation = "F0000046";
    //ȷ��¯�α��
    public static readonly string ConfirmedHeatNumber = "F0000068";
    //������
    public static readonly string Owner = "F0000077";
    //����ԭ��
    public static readonly string ScrapReason = "F0000064";
    //¯�μƻ�
    public static readonly string HeatPlan = "F0000052";
    //������������
    public static readonly string DemandPeriodOfThisOperation = "F0000051";
    //����
    public static readonly string Worker = "F0000028";
    //��ע
    public static readonly string ExceptionDescription = "F0000059";
    //������
    public static readonly string InspectionResult = "F0000067";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //����ȷ�ϱ�ȡ
    public static readonly string CheckAndConfirmTheCopy = "F0000061";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //װ¯ǰ����
    public static readonly string CheckBeforeLoading = "F0000074";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000031";
    //�ƻ�¯�α��
    public static readonly string PlannedHeatNumber = "F0000054";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�ƻ��ȴ���¯��
    public static readonly string PlannedHeatTreatmentHeatNumber = "F0000055";
    //���ݱ���
    public static readonly string Name = "Name";
    //����������ǰ
    public static readonly string InspectionResultBeforeTreatment = "F0000075";
    //������2
    public static readonly string InspectionResult2 = "F0000065";
    //�Ƿ��������������
    public static readonly string Remarks = "F0000045";
    //Status
    public static readonly string Status = "Status";
    //ת������
    public static readonly string WhetherToAdjustToOtherOperation = "F0000039";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //����
    public static readonly string SingleWeight = "F0000035";
    //������
    public static readonly string WorkpieceNumber = "F0000018";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000037";
    //˫�����
    public static readonly string DoubleRollingNumber = "F0000079";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000047";
    //������1
    public static readonly string InspectionResult1 = "F0000042";
    //װ¯ȷ�ϱ�ȡ
    public static readonly string ChargingConfirmationCopy = "F0000062";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000050";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000048";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //��������
    public static readonly string RepairType = "F0000078";
    //ID
    public static readonly string ID = "F0000038";
    //��������1
    public static readonly string RepairType1 = "F0000060";
    //��������2
    public static readonly string RepairType2 = "F0000066";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //��������
    public static readonly string TaskName = "F0000081";
    //�����쳣
    public static readonly string QualityInspectionConclusion = "F0000040";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�쳣����
    public static readonly string ExceptionCategory = "F0000058";
    //���ݴ���
    public static readonly string GoToWorkStep = "F0000044";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //(��/��)����
    public static readonly string IsFinishing = "F0000073";

    //¯��λ�ú�
    public static readonly string HeatInternalPositionNumber = "F0000071";
}
/// <summary>
/// ������������,շ��
/// </summary>
[Table("շ��")]
public class RollingRing
{
    public static readonly string TableCode = "Saesg17flbcod0mvbdha0kkk44";
    public RollingRing() { }
    //������
    public static readonly string OrderNumber = "F0000012";
    //�ӹ�����
    public static readonly string TotalProcessingQuantity = "F0000078";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //����
    public static readonly string SingleWeight = "F0000040";
    //շ��������
    public static readonly string RingRollingWorkerGroup = "F0000032";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "F0000064";
    //�豸����
    public static readonly string EquipmentName = "F0000067";
    //������������
    public static readonly string CurrentOperationDemandPeriod = "F0000058";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000056";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000049";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��������
    public static readonly string TaskName = "F0000077";
    //ȷ�����Ʒ�ʽ
    public static readonly string DetermineRollingMethod = "F0000036";
    //�ȼӹ���Ϣ
    public static readonly string HotProcessingInformation = "D001419Fc33fc9abe5f2451e83ce06a5edc1669f";
    //���ݱ���
    public static readonly string Name = "Name";
    //¯��
    public static readonly string HeatNumber = "F0000035";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000053";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //Status
    public static readonly string Status = "Status";
    //���ݴ���
    public static readonly string DataCode = "F0000050";
    //ת������
    public static readonly string TransferToWorkStep = "F0000047";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000043";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000054";
    //�����쳣
    public static readonly string InitiateException = "F0000048";
    //��ע
    public static readonly string Remarks = "F0000063";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //����λ��
    public static readonly string WorkshopLocation = "F0000052";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�Ƿ��������������
    public static readonly string AdjustToOtherOperation = "F0000060";
    //ID
    public static readonly string ID = "F0000044";
    //�ƻ�¯�α��
    public static readonly string PlannedHeatNumber = "F0000059";
    //˫�����
    public static readonly string DoubleRollingNumber = "F0000068";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000062";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //������
    public static readonly string WorkpieceNumber = "F0000018";
    //������objectID
    public static readonly string ObjectidForTest = "F0000069";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //������
    public static readonly string InspectionResult = "F0000045";
    //�豸���
    public static readonly string EquipmentNumber = "F0000066";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000055";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
}
/// <summary>
/// ������������,��ѹ
/// </summary>
[Table("��ѹ")]
public class Forge
{
    public static readonly string TableCode = "Sdoly16pnqd5z66wl60hc4y1u1";
    public Forge() { }
    //˫�����
    public static readonly string DoubleRollingNumber = "F0000064";
    //�ȼӹ���Ϣ
    public static readonly string HotProcessingInformation = "D001419Fe6ad4c9956ed4788927c31123893dc9e";
    //�豸����
    public static readonly string EquipmentName = "F0000060";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000053";
    //������
    public static readonly string WorkpieceNumber = "F0000018";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000040";
    //��ѹ����
    public static readonly string ForgingTeam = "F0000031";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��ע
    public static readonly string Remarks = "F0000059";
    //���ݴ���
    public static readonly string DataCode = "F0000048";
    //���ݱ���
    public static readonly string Name = "Name";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000046";
    //������
    public static readonly string OrderNumber = "F0000012";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //¯��
    public static readonly string HeatNumber = "F0000063";
    //����
    public static readonly string SingleWeight = "F0000037";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000051";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000032";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //ת������
    public static readonly string TransferToWorkStep = "F0000044";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //�������
    public static readonly string TotalAmountCompleted = "F0000082";
    //Status
    public static readonly string Status = "Status";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000058";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000047";
    //ID
    public static readonly string ID = "F0000041";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "F0000061";
    //�ƻ�¯�α��
    public static readonly string PlannedHeatNumber = "F0000056";
    //�Ƿ��������������
    public static readonly string AdjustToOtherOperations = "F0000049";
    //������objectID
    public static readonly string ObjectidForTest = "F0000065";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //��������
    public static readonly string TaskName = "F0000081";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000052";
    //������
    public static readonly string InspectionResult = "F0000042";
    //����λ��
    public static readonly string WorkshopLocation = "F0000050";
    //������������
    public static readonly string ThisOperationDemandPeriod = "F0000055";
    //�豸���
    public static readonly string EquipmentNumber = "F0000007";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //�����쳣
    public static readonly string InitiateException = "F0000045";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
}
/// <summary>
/// ������������,����
/// </summary>
[Table("����")]
public class SawCut
{
    public static readonly string TableCode = "So3cw528p3w543tqpt12v28o31";
    public SawCut() { }
    //������objectID
    public static readonly string ObjectidForTest = "F0000080";
    //����λ��
    public static readonly string WorkshopLocation = "F0000007";
    //��Ʒλ��
    public static readonly string ProductLocation = "F0000065";
    //��ע
    public static readonly string Remarks = "F0000072";
    //������
    public static readonly string WorkpieceNumber = "F0000018";
    //�豸����
    public static readonly string EquipmentName = "F0000075";
    //��ǰ����
    public static readonly string CurrentOperation = "F0000067";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //��������
    public static readonly string OrderSpecificationNumber = "F0000016";
    //ID
    public static readonly string ID = "F0000030";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //������ID
    public static readonly string IDForTest = "F0000078";
    //�쳣���
    public static readonly string ExceptionCategory = "F0000058";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�������κ�
    public static readonly string OrderBatchNumber = "F0000014";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //���Ʒ�ʽ
    public static readonly string RollingMethod = "F0000031";
    //�쳣����
    public static readonly string ExceptionDescription = "F0000071";
    //˫�����
    public static readonly string DoubleRollingNumber = "F0000079";
    //�ƻ�¯�α��
    public static readonly string PlannedHeatNumber = "F0000070";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //�ӹ���λ
    public static readonly string ProcessingUnit = "F0000045";
    //�Ƿ��������������
    public static readonly string AdjustToOtherOperation = "F0000063";
    //�ڵ�����
    public static readonly string NodeName = "F0000076";
    //�ʼ����
    public static readonly string QualityInspectionConclusion = "F0000077";
    //������������
    public static readonly string ThisOperationDemandPeriod = "F0000068";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //������
    public static readonly string InspectionResult = "F0000043";
    //���ݴ���
    public static readonly string DataCode = "F0000061";
    //���ݱ���
    public static readonly string Name = "Name";
    //������
    public static readonly string Owner = "F0000028";
    //��Ʒ����
    public static readonly string ProductName = "F0000002";
    //��ǰ����
    public static readonly string CurrentWorkStep = "F0000056";
    //ת������
    public static readonly string TransferToWorkStep = "F0000073";
    //�����쳣
    public static readonly string InitiateException = "F0000057";
    //Status
    public static readonly string Status = "Status";
    //�Ƿ�ȡ��
    public static readonly string SampleOrNot = "F0000036";
    //�豸���
    public static readonly string EquipmentNumber = "F0000066";
    //��Ʒ���
    public static readonly string ProductSpecification = "F0000003";
    //�������ι���
    public static readonly string OrderBatchSpecificationNumber = "F0000040";
    //ԭ���Ϻ�
    public static readonly string RawMaterialNumber = "F0000009";
    //����
    public static readonly string SingleWeight = "F0000032";
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������
    public static readonly string OrderNumber = "F0000012";
    //������
    public static readonly string CreatedBy = "CreatedBy";
}
/// <summary>
/// ������������,�쳣������¼��
/// </summary>
[Table("�쳣������¼��")]
public class AbNormalWorkStep
{
    public static readonly string TableCode = "43239d1b3ebf4ab9b43457e95b2657a7";
    public AbNormalWorkStep() { }
    //ModifiedBy
    public static readonly string ModifiedBy = "ModifiedBy";
    //������Դ
    public static readonly string StepSource = "workStepSource";
    //Status
    public static readonly string Status = "Status";
    //��������
    public static readonly string OwnerDeptId = "OwnerDeptId";
    //�޸�ʱ��
    public static readonly string ModifiedTime = "ModifiedTime";
    //���ݱ���
    public static readonly string Name = "Name";
    //ID
    public static readonly string ID = "ID";
    //ObjectId
    public static readonly string ObjectId = "ObjectId";
    //����ʱ��
    public static readonly string CreatedTime = "CreatedTime";
    //ӵ����
    public static readonly string OwnerId = "OwnerId";
    //������
    public static readonly string CreatedBy = "CreatedBy";
    //�����ʱ
    public static readonly string ProcessingTime = "F0000001";
    //WorkflowInstanceId
    public static readonly string WorkflowInstanceId = "WorkflowInstanceId";
    //�쳣���
    public static readonly string ExceptionCategory = "abNormalType";
    //�쳣����
    public static readonly string ExceptionDescription = "abNormalDescibe";
    //������Դ
    public static readonly string OperationSource = "processSource";
}

