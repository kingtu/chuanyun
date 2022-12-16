
using System;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;

public class ThansferSalaryService
{
    H3.IEngine Engine;
    BizObject taSalary;
    H3.SmartForm.SmartFormRequest request;

    public ThansferSalaryService(H3.IEngine Engine, BizObject bizObject)
    {
        this.Engine = Engine;
        this.taSalary = bizObject;
    }
    public ThansferSalaryService(H3.IEngine Engine, H3.SmartForm.SmartFormRequest request)
    {
        this.Engine = Engine;
        this.request = request;
        this.taSalary = request.BizObject;
    }

    public void SyncSalary()
    {
        string TSalaryTableObjectId = "F0000001";//转运记录ObjectId
        BizObject b = Tools.BizOperation.Load(this.Engine, TransferRecord_TableCode, (string)taSalary[TSalaryTableObjectId]);//物流转运（记录）
        string _Id = (string)b[TransferRecord_Id];
        string _OrderSpecificationNumber = (string)b[TransferRecord_OrderSpecificationNumber];
        string _WorkpieceNumber = (string)b[TransferRecord_WorkpieceNumber];

        string planID = "PlanTable";//工序计划表ID       
        BizObject plan = Tools.BizOperation.Load(this.Engine, ABCDProcessPlan_TableCode, (string)b[planID]);//工序计划表（记录）
        if (plan == null)
        {
            Exception ex = new Exception("工序计划表（记录）数据未准备--" + _Id);
            //Tools.Log.ErrorLog(this.Engine, me, ex, "转运工资自动计算", this.Request.UserContext.User.FullName);
            return;
        }

        string _PlannedRollingMethod = (string)plan[ABCDProcessPlan_PlannedRollingMethod]; //计划轧制方式


        string orderSpecField = "F0000145";//订单规格表ID
        BizObject orderSpec = Tools.BizOperation.Load(this.Engine, OrderSpecification_TableCode, (string)plan[orderSpecField]);//订单规格表（记录）
        if (orderSpec == null)
        {
            Exception ex = new Exception("订单规格表（记录）数据未准备--" + _Id);
            //Tools.Log.ErrorLog(this.Engine, me, ex, "转运工资自动计算", this.Request.UserContext.User.FullName);
            return;
        }

        double _UnitWeightOfBlanking = 0;
        if (orderSpec[OrderSpecification_UnitWeightOfBlanking] != null)
        {
            _UnitWeightOfBlanking = (double)orderSpec[OrderSpecification_UnitWeightOfBlanking];
        }
        double _DoubleRollingBlankingSingleWeight = 0;
        if (orderSpec[OrderSpecification_DoubleRollingBlankingSingleWeight] != null)
        {
            _DoubleRollingBlankingSingleWeight = (double)orderSpec[OrderSpecification_DoubleRollingBlankingSingleWeight];
        }
        _UnitWeightOfBlanking = _PlannedRollingMethod == "单轧" ? _UnitWeightOfBlanking : _DoubleRollingBlankingSingleWeight;
        if (_UnitWeightOfBlanking == 0)
        {
            Exception ex = new Exception("订单规格表（记录）数据未准备--缺少下料重量：" + _Id);
            //Tools.Log.ErrorLog(this.Engine, me, ex, "转运工资自动计算", this.Request.UserContext.User.FullName);
            return;
        }

        string flowID = "F0000190";//工艺流程表ID
        BizObject flow = Tools.BizOperation.Load(this.Engine, ProcessFlow_TableCode, (string)plan[flowID]);//工艺流程表（记录）
        string _DoubleRollingCutting = flow[ProcessFlow_DoubleRollingCutting] + string.Empty;//双轧切割状态


        double _UnitPrice = 2;
        double actor = _PlannedRollingMethod == "双轧" && (_DoubleRollingCutting == "" || _DoubleRollingCutting == "未切割") ? 2 : 1;
        double _TransshipmentSalary = _UnitPrice * _UnitWeightOfBlanking / 1000 * actor;
        string _Transshiper = request.UserContext.User.UnitId; //  this.Request.UserContext.User.UnitId;        

        CreateSalary(
            _Transshiper,
            1,
            _TransshipmentSalary,
            _UnitPrice,
            _OrderSpecificationNumber,
            _WorkpieceNumber,
            _Id,
            b["CreatedTime"],
            b[TransferRecord_DepartmentCode],
            b[TransferRecord_State],
            taSalary[TSalaryTableObjectId],
            // b[TransferRecord_DataCode],
            b[TransferRecord_VersionNumber]);

    }
    private void CreateSalary(object Transshiper, object Quantity, object Amount, object Wages,
        object OrderSpecificationNumber, object WorkpieceNumber, object Id,
        object OccurrenceDate, object DepartmentName,
        object TaskName, object TransferTable,
        object VersionNumber)
    {
        BizObject TSalary = Tools.BizOperation.New(this.Engine, TSalaryTable_TableCode);
        TSalary[TSalaryTable_Transshiper] = Transshiper;
        TSalary[TSalaryTable_Quantity] = Quantity;
        TSalary[TSalaryTable_Amount] = Amount;
        TSalary[TSalaryTable_Wages] = Wages;

        TSalary[TSalaryTable_OrderSpecificationNumber] = OrderSpecificationNumber;
        TSalary[TSalaryTable_WorkpieceNumber] = WorkpieceNumber;
        TSalary[TSalaryTable_Id] = Id;
        TSalary[TSalaryTable_OrderSpecificationNumber] = OrderSpecificationNumber;

        TSalary[TSalaryTable_OccurrenceDate] = OccurrenceDate;
        TSalary[TSalaryTable_DepartmentName] = DepartmentName;

        TSalary[TSalaryTable_TaskName] = TaskName;
        TSalary[TSalaryTable_TransferTable] = TransferTable;

        // TSalary[TSalaryTable_DataCode] = DataCode;
        TSalary[TSalaryTable_VersionNumber] = VersionNumber;
        TSalary.Create();
    }
    // 市场需求,A-C订单规格表
    string OrderSpecification_TableCode = "D001419Skniz33124ryujrhb4hry7md21";
    string OrderSpecification_OrderSpecificationNumber = "F0000076";// 订单规格号
    string OrderSpecification_UnitWeightOfBlanking = "F0000015"; // 下料单重 
    string OrderSpecification_DoubleRollingBlankingSingleWeight = "F0000143"; // 双轧下料单重

    // 生产计划,ABCD工序计划表
    string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";
    string ABCDProcessPlan_ID = "F0000007";  // ID
    string ABCDProcessPlan_PlannedRollingMethod = "F0000152"; // 计划轧制方式

    // 生产制造流程,工艺流程表
    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";
    string ProcessFlow_DoubleRollingCutting = "F0000132";// 双轧切割状态
    //转运工资表
    string TSalaryTable_TableCode = "D001419Sacxlj9bmt1o1ka813wfbr7366";
    string TSalaryTable_VersionNumber = "F0000022"; // 版本号        
    string TSalaryTable_Amount = "F0000013";// 金额       
    string TSalaryTable_Quantity = "F0000010"; // 数量       
    string TSalaryTable_Wages = "F0000009"; // 工价       
    string TSalaryTable_OrderSpecificationNumber = "ProductNum"; // 订单规格号       
    string TSalaryTable_Transshiper = "F0000019"; // 转运人员       
    string TSalaryTable_WorkpieceNumber = "F0000014"; // 工件号       
    string TSalaryTable_Id = "F0000016"; // ID 
    string TSalaryTable_TaskName = "TaskName";        // 转运任务
    string TSalaryTable_TransferTable = "TransferTable"; //物流转运表记录

    string TSalaryTable_OccurrenceDate = "F0000025";  // 发生日期     
    string TSalaryTable_DepartmentName = "F0000018";  // 部门名称 
    string TSalaryTable_DepartmentCode = "F0000021";  // 部门代码       
    string TSalaryTable_DataCode = "F0000024";        // 数据代码

    //物流转运记录表
    string TransferRecord_TableCode = "D001419Son0vyw9n413fhgqud7zeeocz2";
    string TransferRecord_OrderSpecificationNumber = "F0000016";    // 订单规格号 
    string TransferRecord_WorkpieceNumber = "F0000025";             // 工件号
    string TransferRecord_Id = "F0000058";                          // ID
    string TransferRecord_UnitPrice = "F0000263";// 单价
    string TransferRecord_Amount = "F0000264";   // 金额
    string TransferRecord_Transshiper = "F0000273"; // 转运人     
    string TransferRecord_State = "F0000275"; //转运状态

    string TransferRecord_VersionNumber = "F0000196";   // 版本号
    string TransferRecord_DataCode = "F0000064";        // 数据代码
    string TransferRecord_DepartmentCode = "F0000200";  // 部门代码
    string TransferRecord_WorkshopLocation = "F0000067";// 车间位置
}