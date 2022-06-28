using H3.DataModel;
using System;

public class D001419Szlywopbivyrv1d64301ta5xv4_ListViewController : H3.SmartForm.ListViewController
{
    string activityCode = "领料生产";
    string userName = ""; //当前用户

    public D001419Szlywopbivyrv1d64301ta5xv4_ListViewController(H3.SmartForm.ListViewRequest request) : base(request)
    {
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {
        try
        {
            base.OnLoad(response);
            //去除派工表、工艺流程表的重复记录
            ClearingDuplicateData();
        }
        catch (Exception ex)
        {
            Tools.Log.ErrorLog(this.Engine, null, ex, activityCode, userName);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        try
        {

            if (actionName == "Dispatch")
            {
                //批量生成机加派工单
                BatchModifyDispatch(postValue, response);
            }
            if (actionName == "wuliao")
            {
                //批量开启制造流程
                BatchStartProcess(postValue, response);
            }
        }
        catch (Exception ex)
        {
            //负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, null, ex, activityCode);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        base.OnSubmit(actionName, postValue, response);
    }

    /// <summary>
    /// 批量开启制造流程
    /// </summary>
    /// <param name="postValue">前端接口</param>
    /// <param name="response">表单响应</param>
    public void BatchStartProcess(H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        string[] objectIds = postValue.Data["ObjectIds"] as string[];
        //无勾选记录时方法返回
        if (objectIds.Length == 0) { return; }
        bool hasError = false;
        foreach (string objectId in objectIds)
        {
            BizObject objPlan = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            if (objPlan[OpenProcess] + string.Empty == "已开启") { continue; }
            if (objPlan[DispatchTable] + string.Empty == "")
            {
                response.Message = "未生成派工单！";
                hasError = true;
                continue;
            }

            //双轧工件号校验
            BizObject objPlanB = null;
            BizObject objProcessB = null;
            BizObject objFlowB = null;
            if (objPlan["F0000152"] + string.Empty == "双轧")
            {
                if (objPlan[DoubleTieWorkpieceNumber] + string.Empty == string.Empty)
                {
                    response.Message = "注意，双轧工件，{双轧工件号}不能为空!";
                    hasError = true;
                    continue;
                }

                //双轧关联表单
                string objectIdB = (string)objPlan["F0000226"];
                objPlanB = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectIdB);
                objProcessB = CreateProcessObject(objPlanB);
                objFlowB = CreateFlowObject(objPlanB);

                //"工艺流程"取值 进度管理数据Id
                objFlowB["Progress"] = objProcessB.ObjectId;
                //工艺流程数据
                objFlowB.Update();

                //"进度管理”取值 工艺流程数据Id
                objProcessB[ScheduleManagement_ProcessFlowTable] = objFlowB.ObjectId;
                objProcessB.Update();

                //开启流程
                Tools.WorkFlow.StartWorkflow(this.Engine, objFlowB, this.Request.UserContext.UserId, true);
                //开启状态
                objPlanB[OpenProcess] = "已开启";
                //工序计划-工艺流程表
                objPlanB[ProcessFlowTable] = objFlowB.ObjectId;
                //更新计划表单
                objPlanB.Update();
            }

            BizObject objProcess = CreateProcessObject(objPlan);
            BizObject objFlow = CreateFlowObject(objPlan);

            //"工艺流程"取值 进度管理数据Id
            objFlow["Progress"] = objProcess.ObjectId;
            //工艺流程数据
            objFlow.Update();

            //"进度管理”取值 工艺流程数据Id
            objProcess[ScheduleManagement_ProcessFlowTable] = objFlow.ObjectId;
            objProcess.Update();

            //双轧工件进度管理建立相互关联关系
            if (objPlan["F0000152"] + string.Empty == "双轧")
            {
                objProcess["F0000079"] = objProcessB.ObjectId;
                objProcessB["F0000079"] = objProcess.ObjectId;
                objProcess.Update();
                objProcessB.Update();
            }

            //开启流程
            Tools.WorkFlow.StartWorkflow(this.Engine, objFlow, this.Request.UserContext.UserId, true);
            //开启状态
            objPlan[OpenProcess] = "已开启";
            //工序计划
            objPlan[ProcessFlowTable] = objFlow.ObjectId;
            //更新本表单
            objPlan.Update();
            //加载工艺流程数据  
            BizObject objFlowDisPatch = Tools.BizOperation.Load(this.Engine, ProcessFlow_TableCode, objFlow.ObjectId);
            //判定工艺流程表中的派工开关
            string dipatchFlag = objFlowDisPatch["F0000230"] + string.Empty;
            if (dipatchFlag == "开")
            {
                //关闭派工功能多选控件
                objFlowDisPatch["F0000231"] = "";
            }
            else
            {
                //关闭派工功能多选控件
                objFlowDisPatch["F0000231"] = objFlowDisPatch["F0000152"];
            }
            objFlowDisPatch.Update();
        }

        if (!hasError) { response.Message = "开启成功！"; }
    }

    /// <summary>
    /// 批量生成机加派工表
    /// </summary>
    /// <param name="postValue">前端接口</param>
    /// <param name="response">表单响应</param>
    public void BatchModifyDispatch(H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        string[] objectIds = postValue.Data["ObjectIds"] as string[];
        //无勾选记录时方法返回
        if (objectIds.Length == 0) { return; }

        foreach (string objectId in objectIds)
        {
            BizObject objPlan = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            if (objPlan[DispatchTable] + string.Empty != "") { continue; }
            BizObject dispatchObj = CreateDispatchObject(objPlan, objectId);
            objPlan[DispatchTable] = dispatchObj.ObjectId;
            objPlan.Update();
            response.Message = "生成派工单成功！";
        }
    }

    /// <summary>
    /// 创建“派工表”对象
    /// </summary>
    /// <param name="objPlan">计划表业务对象</param>
    /// <param name="objectId">计划表业务对象</param>
    /// <returns></returns>
    private BizObject CreateDispatchObject(BizObject objPlan, string objectId)
    {
        BizObject dispatchObj = Tools.BizOperation.New(this.Engine, Dispatchs_TableCode);   //派工表
        dispatchObj[Dispatchs_OrderNumber] = objPlan[OrderNumber] + string.Empty;
        dispatchObj[Dispatchs_WorkpieceNumber] = objPlan[WorkpieceNumber] + string.Empty;
        dispatchObj[Dispatchs_ID] = objPlan[ID] + string.Empty;

        //派工表-数据代码
        dispatchObj["F0000070"] = objPlan[DataCode] + string.Empty;
        //派工表-部门代码
        dispatchObj["F0000071"] = objPlan["F0000236"] + string.Empty;
        //派工表-版本号
        dispatchObj["F0000091"] = objPlan["F0000235"] + string.Empty;
        //派工表-部门多选
        dispatchObj["F0000072"] = objPlan["F0000234"];

        //取样计划完成日期
        dispatchObj[Dispatchs_SamplingPlanCompletionTime] = objPlan[CompletionTimeOfSamplingPlan] + string.Empty;
        //派工-订单规格表
        dispatchObj["F0000055"] = objPlan[OrderSpecificationTable] + string.Empty;

        dispatchObj[Dispatchs_RoughCuttingPlanCompletionTime] = objPlan[RoughCuttingPlanCompletionTime] + string.Empty;
        dispatchObj[Dispatchs_FinishingPlanCompletionTime] = objPlan[FinishingPlannedCompletionTime] + string.Empty;
        dispatchObj[Dispatchs_DrillingPlanCompletionTime] = objPlan[DrillingPlannedCompletionTime] + string.Empty;


        string chiefPlanner = (string)objPlan["F0000252"]; //总计划员   
        string coldWorkingSectionChief = (string)objPlan["F0000251"]; //冷加工科长 
        string samplingTeamLeader = (string)objPlan["F0000247"]; //取样班组长 
        string roughingShiftLeader = (string)objPlan["F0000248"]; //粗车班组长
        string finishingShiftLeader = (string)objPlan["F0000249"]; //精车班组长
        string drillingTeamLeader = (string)objPlan["F0000250"]; //钻孔班组长

        //派工表取值无派工加工审批权限人
        dispatchObj["F0000085"] = chiefPlanner;
        dispatchObj["F0000086"] = coldWorkingSectionChief;
        dispatchObj["F0000087"] = samplingTeamLeader;
        dispatchObj["F0000088"] = roughingShiftLeader;
        dispatchObj["F0000089"] = finishingShiftLeader;
        dispatchObj["F0000090"] = drillingTeamLeader;


        //派工取样子表业务对象
        BizObject dSampling = Tools.BizOperation.New(this.Engine, DispatchSamplingSubTable_TableCode);
        //派工任务名
        dSampling[DispatchSamplingSubTable_TaskName] = "取样任务";
        //派工人员
        dSampling[DispatchSamplingSubTable_Name] = new object[1] { samplingTeamLeader != "" ? samplingTeamLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dSampling[DispatchSamplingSubTable_ProcessingQuantity] = 1.0;
        //取样派工总量
        dispatchObj["F0000098"] = 1.0;
        //取样（派工）子表控件
        dispatchObj[DispatchSamplingSubTable_TableCode] = new BizObject[1] { dSampling };


        //派工粗车子表业务对象
        BizObject dRough = Tools.BizOperation.New(this.Engine, DispatchRoughSubTable_TableCode);
        //派工任务名
        dRough[DispatchRoughSubTable_TaskName] = "粗车任务";
        //派工人员
        dRough[DispatchRoughSubTable_Name] = new object[1] { roughingShiftLeader != "" ? roughingShiftLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dRough[DispatchRoughSubTable_ProcessingQuantity] = 1.0;
        //粗车派工总量
        dispatchObj["F0000099"] = 1.0;
        //粗车（派工）子表控件           
        dispatchObj[DispatchRoughSubTable_TableCode] = new BizObject[1] { dRough };

        //派工精车子表业务对象
        BizObject dFinish = Tools.BizOperation.New(this.Engine, DispatchFinishSubTable_TableCode);
        //派工任务名
        dFinish[DispatchFinishSubTable_TaskName] = "精车任务";
        //派工人员
        dFinish[DispatchFinishSubTable_Name] = new object[1] { finishingShiftLeader != "" ? finishingShiftLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dFinish[DispatchFinishSubTable_ProcessingQuantity] = 1.0;
        //精车派工总量
        dispatchObj["F0000100"] = 1.0;
        //精车（派工）子表控件
        dispatchObj[DispatchFinishSubTable_TableCode] = new BizObject[1] { dFinish };

        //派工钻孔子表   
        BizObject dDrill = Tools.BizOperation.New(this.Engine, DispatchDrillingSubTable_TableCode);
        //派工任务名
        dDrill[DispatchDrillingSubTable_TaskName] = "钻孔任务";
        //派工人员
        dDrill[DispatchDrillingSubTable_Name] = new object[1] { drillingTeamLeader != "" ? drillingTeamLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dDrill[DispatchDrillingSubTable_ProcessingQuantity] = 1.0;
        //钻孔派工总量
        dispatchObj["F0000101"] = 1.0;
        //钻孔（派工）子表控件
        dispatchObj[DispatchDrillingSubTable_TableCode] = new BizObject[1] { dDrill };

        //“派工表”取值“工序计划表”的数据ID
        dispatchObj["PlanTable"] = objectId;
        dispatchObj.Status = BizObjectStatus.Effective;
        //派工数据
        dispatchObj.Create();
        return dispatchObj;

    }

    /// <summary>
    /// 创建“工艺流程”对象
    /// </summary>
    /// <param name="objPlan">提供值的工序计划表</param> 
    /// <returns></returns>
    private BizObject CreateFlowObject(BizObject objPlan)
    {
        BizObject objFlow = Tools.BizOperation.New(this.Engine, ProcessFlow_TableCode);

        if (objPlan[PlannedRollingMode] + string.Empty == "双轧")
        {
            //工件号转数值
            string workpieceNumber = objPlan[WorkpieceNumber] + string.Empty;
            //双轧工件号转数值
            string doubleTieWorkpieceNumber = objPlan[DoubleTieWorkpieceNumber] + string.Empty;

            //工件号大于双轧工件号
            if (string.Compare(workpieceNumber, doubleTieWorkpieceNumber, true) > 0)
            {
                //双扎工艺分割前工件值为是
                objFlow[ProcessFlow_DoubleTieTheWorkpieceBeforeSegmentation] = "是";
            }
            else
            {
                //双扎工艺分割前工件值为否
                objFlow[ProcessFlow_DoubleTieTheWorkpieceBeforeSegmentation] = "否";
            }
        }



        //订单号
        objFlow[ProcessFlow_OrderNumber] = objPlan[OrderNumber] + string.Empty;
        //订单批次号
        objFlow[ProcessFlow_OrderBatchNumber] = objPlan[OrderBatchNumber] + string.Empty;
        //订单规格号
        string orderSpec = objPlan[OrderSpecificationNumber] + string.Empty;
        objFlow[ProcessFlow_OrderSpecificationNumber] = orderSpec;
        //订单批次规格号
        objFlow[ProcessFlow_OrderBatchSpecificationNumber] = objPlan[OrderBatchSpecificationNumber] + string.Empty;
        //工件号
        objFlow[ProcessFlow_WorkpieceNumber] = objPlan[WorkpieceNumber] + string.Empty;
        //ID
        objFlow[ProcessFlow_ID] = objPlan[ID] + string.Empty;
        //产品名称
        objFlow[ProcessFlow_ProductName] = objPlan[ProductName] + string.Empty;
        //产品种类
        objFlow[ProcessFlow_ProductType] = objPlan["ProductType"];

        //成品单重
        objFlow[ProcessFlow_FinishedProductUnitWeight] = objPlan[FinishedProductUnitWeight] + string.Empty;
        //规格参数
        objFlow[ProcessFlow_SpecificationParameters] = objPlan[SpecificationParameters] + string.Empty;

        //数据代码
        objFlow[ProcessFlow_DataCode] = objPlan[DataCode] + string.Empty;

        //部门代码
        objFlow["F0000154"] = objPlan["F0000236"] + string.Empty;

        //版本号
        objFlow["F0000153"] = objPlan["F0000235"] + string.Empty;

        //部门多选
        objFlow["F0000152"] = objPlan["F0000234"];

        //再生品ID
        objFlow[ProcessFlow_RecycledProductID] = objPlan[RecycledProductID] + string.Empty;
        if (objPlan[RecycledProductID] + string.Empty != string.Empty)
        {
            objFlow[ProcessFlow_CurrentSection] = "再生库";
        }
        //质量状态
        objFlow[ProcessFlow_QualityStatus] = "正常";
        //再生工序
        objFlow[ProcessFlow_RecycledSection] = objPlan[RecycledSection] + string.Empty;
        //计划轧制方式
        objFlow[ProcessFlow_PlannedRollingMode] = objPlan[PlannedRollingMode] + string.Empty;

        //计划本取
        objFlow[ProcessFlow_PlannedTake] = objPlan[PlanThisOptionTakes] + string.Empty;

        //计划热处理炉号
        objFlow[ProcessFlow_PlannedHeatTreatmentHeatNumber] = objPlan[HeatTreatmentFurnaceNumber] + string.Empty;

        //锯切加工单位
        objFlow[ProcessFlow_SawCutProcessingUnit] = objPlan[ProcessingUnitSawCut] + string.Empty;
        //本工序需求期-锯切
        objFlow[ProcessFlow_DemandPeriodOfThisSectionSawCut] = objPlan[DemandPeriodOfThisSectionSawCut] + string.Empty;

        //锻压加工单位
        objFlow[ProcessFlow_ForgingProcessingUnit] = objPlan[ProcessingUnitForging] + string.Empty;
        //本工序需求期-锻压
        objFlow[ProcessFlow_DemandPeriodOfThisProcedureForging] = objPlan[DemandPeriodOfThisSectionForge] + string.Empty;

        //辗环加工单位
        objFlow[ProcessFlow_RingRollingProcessingUnit] = objPlan[ProcessingUnitRingRolling] + string.Empty;
        //本工序需求期-辗环
        objFlow[ProcessFlow_DemandPeriodOfThisSectionRingRolling] = objPlan[DemandPeriodOfThisSectionRingRolling] + string.Empty;

        //热处理加工单位
        objFlow[ProcessFlow_HeatTreatmentProcessingUnit] = objPlan[ProcessingUnitHeatTreatment] + string.Empty;
        //本工序需求期-热处理
        objFlow[ProcessFlow_DemandPeriodOfThisSectionHeatTreatment] = objPlan[DemandPeriodOfThisSectionHeatTreatment] + string.Empty;

        //毛坯加工单位
        objFlow[ProcessFlow_BlankProcessingUnit] = objPlan[ProcessingUnitRoughCast] + string.Empty;
        //本工序需求期-毛坯
        objFlow[ProcessFlow_DemandPeriodOfThisSectionRoughCast] = objPlan[DemandPeriodOfThisSectionRoughCast] + string.Empty;

        //粗车加工单位
        objFlow[ProcessFlow_RoughCuttingUnit] = objPlan[ProcessingUnitRoughCutting] + string.Empty;
        //本工序需求期-粗车
        objFlow[ProcessFlow_DemandPeriodOfThisSectionRoughCutting] = objPlan[RoughCuttingPlanCompletionTime] + string.Empty;


        //精车加工单位
        objFlow[ProcessFlow_FinishingUnit] = objPlan[ProcessingUnitFinishing] + string.Empty;

        //本工序需求期-精车
        objFlow[ProcessFlow_DemandPeriodOfThisSectionFinishing] = objPlan[FinishingPlannedCompletionTime] + string.Empty;


        //钻孔加工单位
        objFlow[ProcessFlow_DrillingUnit] = objPlan[ProcessingUnitDrilling] + string.Empty;
        //本工序需求期-钻孔
        objFlow[ProcessFlow_DemandPeriodOfThisSectionDrilling] = objPlan[DrillingPlannedCompletionTime] + string.Empty;


        //工序计划表
        objFlow[ProcessFlow_SectionSchedule] = objPlan[ObjectId] + string.Empty;

        //双轧关联表单
        objFlow[ProcessFlow_DoubleTieAssociatedForm] = objPlan[DoubleTieAssociatedForm] + string.Empty;
        //双轧工件号
        objFlow[ProcessFlow_DoubleTieWorkpieceNumber] = objPlan[DoubleTieWorkpieceNumber] + string.Empty;
        //数量-切割前
        objFlow[ProcessFlow_QuantityBeforeCutting] = objPlan[QuantityBeforeCutting] + string.Empty;


        //原料库
        objFlow[ProcessFlow_RawMaterialWarehouse] = objPlan[RawMaterialStorage] + string.Empty;
        //原材料类型
        objFlow[ProcessFlow_RawMaterialType] = objPlan[RawMaterialType] + string.Empty;
        //原材料编号
        objFlow[ProcessFlow_RawMaterialNumber] = objPlan[RawMaterialNumber] + string.Empty;

        //工艺流程表赋值无派工加工审批权限人
        objFlow["F0000194"] = objPlan["F0000252"]; //总计划员
        objFlow["F0000193"] = objPlan["F0000251"]; //冷加工科长
        objFlow["F0000195"] = objPlan["F0000247"]; //取样班组长
        objFlow["F0000196"] = objPlan["F0000248"]; //粗车班组长
        objFlow["F0000197"] = objPlan["F0000249"]; //精车班组长
        objFlow["F0000198"] = objPlan["F0000250"]; //钻孔班组长

        //"工艺流程"的派工信息字段取派工表ObjectId的值
        objFlow[ProcessFlow_DispatchingInformation] = objPlan[DispatchTable];

        //工艺流程表数据
        objFlow.Create();
        return objFlow;

    }
    /// <summary>
    /// 创建“实时生产动态”对象
    /// </summary>
    /// <param name="objPlan">提供值的工序计划表</param>
    /// <returns></returns>
    private BizObject CreateProcessObject(BizObject objPlan)
    {
        BizObject objProcess = Tools.BizOperation.New(this.Engine, ScheduleManagement_TableCode);
        //进度管理-产品ID
        objProcess[ScheduleManagement_ID] = objPlan[ID] + string.Empty;
        //进度管理-数据代码
        objProcess["F0000014"] = objPlan[DataCode] + string.Empty;
        //进度管理-部门代码
        objProcess["F0000076"] = objPlan["F0000236"] + string.Empty;
        //进度管理-版本号
        objProcess["F0000077"] = objPlan["F0000235"] + string.Empty;
        //进度管理-部门多选
        objProcess["F0000078"] = objPlan["F0000234"];

        objProcess[ScheduleManagement_DataCode] = objPlan[DataCode] + string.Empty;

        //热加工计划期
        objProcess[ScheduleManagement_HotProcessingPlanPeriod] = objPlan[HotProcessingPlan] + string.Empty;
        //冷加工计划期
        objProcess[ScheduleManagement_ColdProcessingPlanPeriod] = objPlan[ColdProcessingPlan] + string.Empty;
        //市场需求达成进度
        objProcess[ScheduleManagement_MarketDemandAchievementProgress] = "未投产";
        //订单批次表
        objProcess[ScheduleManagement_OrderBatchTable] = objPlan[OrderBatchTable] + string.Empty;
        //订单规格表
        objProcess[ScheduleManagement_OrderSpecificationTable] = objPlan[OrderSpecificationTable] + string.Empty;

        //订单批次规格表
        objProcess[ScheduleManagement_OrderBatchSpecificationTable] = objPlan[OrderBatchSpecificationTable] + string.Empty;
        //质量状态
        objProcess[ScheduleManagement_QualityStatus] = "正常";

        objProcess[ScheduleManagement_ProcessPlanTable] = objPlan[ObjectId] + string.Empty;

        objProcess.Status = BizObjectStatus.Effective;

        //进度管理数据
        objProcess.Create();
        return objProcess;
    }

    /*
     *Author:fubin
     *去除派工表、工艺流程表的重复记录
     */
    private void ClearingDuplicateData()
    {
        //构建过滤器
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        //过去一天的日期
        DateTime today = System.DateTime.Today;
        //["ModifiedTime"]-[修改时间]-A   筛选昨天与今天修改过的订单
        Tools.Filter.Or(filter, "ModifiedTime", H3.Data.ComparisonOperatorType.Above, today);
        //'选取前（二十）条记录'
        filter.FromRowNum = 0;
        filter.ToRowNum = 20;
        //以ModifiedTime逆向排序
        filter.AddSortBy("ModifiedTime", H3.Data.Filter.SortDirection.Descending);
        //获取业务对象集合
        H3.DataModel.BizObject[] bizObjects = Tools.BizOperation.GetList(this.Engine, this.Request.SchemaCode, filter);
        if (bizObjects != null)
        {
            foreach (H3.DataModel.BizObject item in bizObjects)
            {
                string dispatchObjectId = item[DispatchTable] + string.Empty;     //派工表记录ObjectId
                string processObjectId = item[ProcessFlowTable] + string.Empty;   //工艺流程记录ObjectId
                string startFlag = item[OpenProcess] + string.Empty;
                if (string.IsNullOrEmpty(dispatchObjectId) || string.IsNullOrEmpty(processObjectId) || startFlag == "未开启") //开启状态
                {
                    return;
                }

                //进度管理 ObjectId
                //加载工艺流程数据  
                BizObject objFlowObj = Tools.BizOperation.Load(this.Engine, ProcessFlow_TableCode, processObjectId);
                string scheduleManagementId = objFlowObj[ScheduleManagement_ProcessFlowTable] + string.Empty;
                string idNumber = item[ID] + string.Empty;                        //工件ID

                //查询所有工件ID与本工件ID相同的派工表记录
                H3.Data.Filter.Filter dispatchFilter = new H3.Data.Filter.Filter();
                Tools.Filter.And(dispatchFilter, Dispatchs_ID, H3.Data.ComparisonOperatorType.Equal, idNumber);
                H3.DataModel.BizObject[] DispatchObjects = Tools.BizOperation.GetList(this.Engine, Dispatchs_TableCode, dispatchFilter);
                if (DispatchObjects != null)
                {
                    for (int i = 0; i < DispatchObjects.Length; i++)
                    {   //如果与本记录记录的派工表记录相同则保留该记录，否则删除该记录
                        if (DispatchObjects[i].ObjectId == dispatchObjectId) { continue; }
                        DispatchObjects[i].Remove();
                    }
                }

                //查询所有工件ID与本工件ID相同的工艺流程表记录
                H3.Data.Filter.Filter processFilter = new H3.Data.Filter.Filter();
                Tools.Filter.And(processFilter, ProcessFlow_ID, H3.Data.ComparisonOperatorType.Equal, idNumber);
                H3.DataModel.BizObject[] ProcessFlowObjects = Tools.BizOperation.GetList(this.Engine, ProcessFlow_TableCode, processFilter);
                if (ProcessFlowObjects != null)
                {
                    for (int i = 0; i < ProcessFlowObjects.Length; i++)
                    {   //如果与本记录记录的工艺流程表记录相同则保留该记录，否则删除该记录
                        if (ProcessFlowObjects[i].ObjectId == processObjectId) { continue; }
                        ProcessFlowObjects[i].Remove();
                    }
                }
                //查询所有工件ID与本工件ID相同的进度管理表记录
                H3.Data.Filter.Filter scheduleManagementFilter = new H3.Data.Filter.Filter();
                Tools.Filter.And(scheduleManagementFilter, ScheduleManagement_ID, H3.Data.ComparisonOperatorType.Equal, idNumber);
                H3.DataModel.BizObject[] scheduleManagementObjects = Tools.BizOperation.GetList(this.Engine, ScheduleManagement_TableCode, scheduleManagementFilter);
                if (scheduleManagementObjects != null)
                {
                    for (int i = 0; i < scheduleManagementObjects.Length; i++)
                    {   //如果与本记录记录的工艺流程表记录相同则保留该记录，否则删除该记录
                        if (scheduleManagementObjects[i].ObjectId == processObjectId) { continue; }
                        scheduleManagementObjects[i].Remove();
                    }
                }
            }
        }
    }

    // 生产计划,ABCD工序计划表
    string TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";
    string OrderBatchTable = "F0000144"; // 订单批次表    
    string HeatTreatmentFurnaceNumber = "F0000140";// 计划热处理炉号    
    string ProcessingUnitForging = "F0000062";// 加工单位-锻压 
    string HotProcessingPlan = "F0000023";   // 热加工完成时间    
    string DoubleTieAssociatedForm = "F0000226";// 双轧关联表单   
    string QuantityBeforeCutting = "F0000228"; // 数量-切割前   
    string RawMaterialNumber = "F0000221"; // 原材料编号    
    string OpenProcess = "F0000020";// 开启流程    
    string WorkpieceNumber = "F0000054";// 工件号    
    string ProcessingUnitSawCut = "F0000058";// 加工单位-锯切    
    string PlanThisOptionTakes = "F0000141";// 计划本取    
    string DoubleTieWorkpieceNumber = "F0000227";// 双轧工件号   
    string ProcessingUnitRoughCutting = "F0000078"; // 加工单位-粗车    
    string FinishingPlannedCompletionTime = "F0000098";// 精车计划完成时间   
    string OrderBatchSpecificationNumber = "F0000006"; // 订单批次规格号    
    string ProcessFlowTable = "F0000190";// 工艺流程表   
    string DispatchTable = "DispatchTable"; //机加派工表  
    string ProcessingUnitFinishing = "F0000082";  // 加工单位-精车    
    string PlannedRollingMode = "F0000152";// 计划轧制方式   
    string RoughCuttingPlanCompletionTime = "F0000095"; // 粗车计划完成时间   
    string OrderSpecificationNumber = "F0000004"; // 订单规格号   
    string DataCode = "shuj"; // 数据代码   
    string DemandPeriodOfThisSectionRingRolling = "F0000067"; // 本工序需求期-辗环   
    string ProcessingUnitHeatTreatment = "F0000070"; // 加工单位-热处理   
    string ProductName = "F0000025"; // 产品名称 
    string RawMaterialStorage = "F0000219";   // 原料库    
    string ID = "F0000007";// ID  
    string OrderSpecificationTable = "F0000145";  // 订单规格表   
    string FinishedProductUnitWeight = "F0000031"; // 成品单重   
    string OrderBatchSpecificationTable = "F0000017"; // 订单批次规格表    
    string CompletionTimeOfSamplingPlan = "F0000212";// 取样计划完成时间    
    string DemandPeriodOfThisSectionSawCut = "F0000059";// 本工序需求期-锯切  
    string DemandPeriodOfThisSectionForge = "F0000063";  // 本工序需求期-锻压  
    string OrderNumber = "F0000001";  // 订单号    
    string ProcessingUnitDrilling = "F0000084";// 加工单位-钻孔   
    string RecycledSection = "F0000142"; // 再生工序   
    string ColdProcessingPlan = "F0000024"; // 冷加工完成时间   
    string DemandPeriodOfThisSectionHeatTreatment = "F0000071"; // 本工序需求期-热处理    
    string OrderBatchNumber = "F0000003";// 订单批次号  
    string DrillingPlannedCompletionTime = "F0000102";  // 钻孔计划完成时间   
    string ProcessingUnitRingRolling = "F0000066"; // 加工单位-辗环   
    string RecycledProductID = "F0000137"; // 再生品ID    
    string SpecificationParameters = "F0000028";// 规格参数   
    string DemandPeriodOfThisSectionRoughCast = "F0000075"; // 本工序需求期-毛坯  
    string ProcessingUnitRoughCast = "F0000074";  // 加工单位-毛坯   
    string RawMaterialType = "F0000220"; // 原材料类型
    string ObjectId = "ObjectId";

    // 生产计划,派工表
    string Dispatchs_TableCode = "D001419c08bb982ac44481a9439076269a8f783";   
    string Dispatchs_RoughCuttingPlanCompletionTime = "F0000004"; // 粗车计划完成时间   
    string Dispatchs_ID = "F0000025"; // ID    
    string Dispatchs_OrderNumber = "F0000038";// 订单号    
    string Dispatchs_FinishingPlanCompletionTime = "F0000005";// 精车计划完成时间   
    string Dispatchs_SamplingPlanCompletionTime = "F0000003"; // 取样计划完成时间  
    string Dispatchs_WorkpieceNumber = "F0000039";  // 工件号   
    string Dispatchs_DrillingPlanCompletionTime = "F0000006"; // 钻孔计划完成时间

    // 生产计划,取样派工子表
    string DispatchSamplingSubTable_TableCode = "D001419Fc9380612ad364043a33702a36bf5fde9";    
    string DispatchSamplingSubTable_Name = "F0000036";// 派工人员  
    string DispatchSamplingSubTable_TaskName = "F0000059";  // 派工任务
    string DispatchSamplingSubTable_ProcessingQuantity = "F0000019";    // 派工量

    // 生产制造流程,工艺流程表
    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";   
    string ProcessFlow_PlannedTake = "F0000118"; // 计划本取    
    string ProcessFlow_DoubleTieAssociatedForm = "F0000143";// 双轧关联表单   
    string ProcessFlow_OrderBatchNumber = "F0000002"; // 订单批次号   
    string ProcessFlow_SpecificationParameters = "F0000151"; // 产品规格   
    string ProcessFlow_DemandPeriodOfThisProcedureForging = "F0000073"; // 本工序需求期-锻压  
    string ProcessFlow_QualityStatus = "F0000065";  // 质量状态    
    string ProcessFlow_DemandPeriodOfThisSectionRoughCast = "F0000082";// 本工序需求期-毛坯   
    string ProcessFlow_RawMaterialType = "F0000062"; // 原材料类型  
    string ProcessFlow_PlannedHeatTreatmentHeatNumber = "F0000124";  // 计划热处理炉号  
    string ProcessFlow_DemandPeriodOfThisSectionRingRolling = "F0000076";  // 本工序需求期-辗环    
    string ProcessFlow_DemandPeriodOfThisSectionSawCut = "F0000068";// 本工序需求期-锯切    
    string ProcessFlow_FinishingUnit = "F0000040";// 精车加工单位   
    string ProcessFlow_ID = "F0000006"; // ID  
    string ProcessFlow_DrillingUnit = "F0000041";  // 钻孔加工单位   
    string ProcessFlow_OrderBatchSpecificationNumber = "F0000004"; // 订单批次规格号   
    string ProcessFlow_OrderNumber = "F0000001"; // 订单号    
    string ProcessFlow_SawCutProcessingUnit = "F0000024";// 锯切加工单位   
    string ProcessFlow_RawMaterialNumber = "F0000058"; // 原材料编号   
    string ProcessFlow_DataCode = "F0000069"; // 数据代码   
    string ProcessFlow_RecycledProductID = "F0000060"; // 再生品ID   
    string ProcessFlow_PlannedRollingMode = "F0000112"; // 计划轧制方式    
    string ProcessFlow_ScheduleManagementInformation = "Progress";// 进度管理信息    
    string ProcessFlow_DemandPeriodOfThisSectionFinishing = "F0000088";// 本工序需求期-精车    
    string ProcessFlow_RingRollingProcessingUnit = "F0000028";// 辗环加工单位   
    string ProcessFlow_DoubleTieWorkpieceNumber = "F0000144"; // 双轧工件号   
    string ProcessFlow_OrderSpecificationNumber = "F0000003"; // 订单规格号   
    string ProcessFlow_FinishedProductUnitWeight = "F0000017"; // 成品单重    
    string ProcessFlow_RoughCuttingUnit = "F0000039";// 粗车加工单位   
    string ProcessFlow_WorkpieceNumber = "F0000005"; // 工件号    
    string ProcessFlow_DemandPeriodOfThisSectionDrilling = "F0000097";// 本工序需求期-钻孔
   
    string ProcessFlow_BlankProcessingUnit = "F0000030"; // 毛坯加工单位    
    string ProcessFlow_ForgingProcessingUnit = "F0000027";// 锻压加工单位   
    string ProcessFlow_SectionSchedule = "F0000126"; // 工序计划表  
    string ProcessFlow_RawMaterialWarehouse = "F0000119";  // 原料库   
    string ProcessFlow_RecycledSection = "F0000121"; // 再生工序    
    string ProcessFlow_DispatchingInformation = "F0000148";// 派工信息    
    string ProcessFlow_HeatTreatmentProcessingUnit = "F0000029";// 热处理加工单位    
    string ProcessFlow_DoubleTieTheWorkpieceBeforeSegmentation = "F0000150";// 双扎分割前工件  
    string ProcessFlow_DemandPeriodOfThisSectionRoughCutting = "F0000085";  // 本工序需求期-粗车   
    string ProcessFlow_DepartmentCode = "F0000154"; // 部门代码   
    string ProcessFlow_CurrentSection = "F0000018"; // 当前工序   
    string ProcessFlow_ProductType = "F0000023"; // 产品种类   
    string ProcessFlow_QuantityBeforeCutting = "F0000145"; // 数量-切割前  
    string ProcessFlow_ProductName = "F0000007";  // 产品名称    
    string ProcessFlow_DemandPeriodOfThisSectionHeatTreatment = "F0000079";// 本工序需求期-热处理    
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    
    // 生产数据分析,实时生产动态----生产进度管理    
    string ScheduleManagement_CurrentSchemaCode = "F0000071";// 当前工序    
    string ScheduleManagement_CurrentObjectId = "F0000070";//当前记录Id    
    string ScheduleManagement_MarketDemandAchievementProgress = "F0000020";// 市场需求达成进度    
    string ScheduleManagement_DataCode = "F0000014";// 数据代码    
    string ScheduleManagement_OrderBatchSpecificationTable = "F0000069";// 订单批次规格表    
    string ScheduleManagement_OrderSpecificationTable = "F0000018";// 订单规格表   
    string ScheduleManagement_ID = "F0000001"; // ID   
    string ScheduleManagement_QualityStatus = "F0000010"; // 质量状态   
    string ScheduleManagement_ProcessFlowTable = "F0000075"; // 工艺流程表  
    string ScheduleManagement_ProcessPlanTable = "F0000019";  // 工序计划表    
    string ScheduleManagement_OrderBatchTable = "F0000026";// 订单批次表  
    string ScheduleManagement_ColdProcessingPlanPeriod = "F0000013";  // 冷加工计划期    
    string ScheduleManagement_HotProcessingPlanPeriod = "F0000012";// 热加工计划期

    // 生产计划,粗车派工子表
    string DispatchRoughSubTable_TableCode = "D001419Ffb3f2e583e31421e8aaa5a085bbada58";  
    string DispatchRoughSubTable_TaskName = "F0000058";  // 派工任务   
    string DispatchRoughSubTable_Name = "F0000037"; // 派工人员   
    string DispatchRoughSubTable_ProcessingQuantity = "F0000020"; // 派工量

    // 生产计划,精车派工子表
    string DispatchFinishSubTable_TableCode = "D001419F4a23f2f26a01428f952a593da3d99fe5";    
    string DispatchFinishSubTable_ProcessingQuantity = "F0000021";// 派工量 
    string DispatchFinishSubTable_Name = "F0000014";   // 派工人员   
    string DispatchFinishSubTable_TaskName = "F0000060"; // 派工任务

    // 生产计划,钻孔派工子表
    string DispatchDrillingSubTable_TableCode = "D001419F5ccfa7d5acad41bf98c640057f2570ae";   
    string DispatchDrillingSubTable_TaskName = "F0000061"; // 派工任务   
    string DispatchDrillingSubTable_Name = "F0000015"; // 派工人员   
    string DispatchDrillingSubTable_ProcessingQuantity = "F0000022"; // 派工量

}
