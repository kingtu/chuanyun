
using H3;
using H3.DataModel;
using System;
using static H3.SmartForm;

public class D001419Szlywopbivyrv1d64301ta5xv4_ListViewController : H3.SmartForm.ListViewController
{
    
    string activityCode = "领料生产";   
    // 生产计划,ABCD工序计划表
    string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";

    // 订单批次表
    string ABCDProcessPlan_OrderBatchTable = "F0000144";

    // 计划热处理炉号
    string ABCDProcessPlan_HeatTreatmentFurnaceNumber = "F0000140";
    // 加工单位-锻压
    string ABCDProcessPlan_ProcessingUnitForging = "F0000062";

    // 热加工完成时间
    string ABCDProcessPlan_HotProcessingPlan = "F0000023";
    // 双轧关联表单
    string ABCDProcessPlan_DoubleTieAssociatedForm = "F0000226";

    // 数量-切割前
    string ABCDProcessPlan_QuantityBeforeCutting = "F0000228";

    // 原材料编号
    string ABCDProcessPlan_RawMaterialNumber = "F0000221";

    // 开启流程
    string ABCDProcessPlan_OpenProcess = "F0000020";
    // 工件号
    string ABCDProcessPlan_WorkpieceNumber = "F0000054";
    // 加工单位-锯切
    string ABCDProcessPlan_ProcessingUnitSawCut = "F0000058";
    // 计划本取
    string ABCDProcessPlan_PlanThisOptionTakes = "F0000141";
    // 双轧工件号
    string ABCDProcessPlan_DoubleTieWorkpieceNumber = "F0000227";

    // 加工单位-粗车
    string ABCDProcessPlan_ProcessingUnitRoughCutting = "F0000078";

    // 精车计划完成时间
    string ABCDProcessPlan_FinishingPlannedCompletionTime = "F0000098";
    // 订单批次规格号
    string ABCDProcessPlan_OrderBatchSpecificationNumber = "F0000006";
    // 工艺流程表
    string ABCDProcessPlan_ProcessFlowTable = "F0000190";
    // 加工单位-精车
    string ABCDProcessPlan_ProcessingUnitFinishing = "F0000082";

    // 计划轧制方式
    string ABCDProcessPlan_PlannedRollingMode = "F0000152";
    // 粗车计划完成时间
    string ABCDProcessPlan_RoughCuttingPlanCompletionTime = "F0000095";

    // 订单规格号
    string ABCDProcessPlan_OrderSpecificationNumber = "F0000004";
    // 数据代码
    string ABCDProcessPlan_DataCode = "shuj";
    // 本工序需求期-辗环
    string ABCDProcessPlan_DemandPeriodOfThisSectionRingRolling = "F0000067";

    // 加工单位-热处理
    string ABCDProcessPlan_ProcessingUnitHeatTreatment = "F0000070";

    // 产品名称
    string ABCDProcessPlan_ProductName = "F0000025";
    // 原料库
    string ABCDProcessPlan_RawMaterialStorage = "F0000219";

    // ID
    string ABCDProcessPlan_ID = "F0000007";

    // 订单规格表
    string ABCDProcessPlan_OrderSpecificationTable = "F0000145";
    // 成品单重
    string ABCDProcessPlan_FinishedProductUnitWeight = "F0000031";

    // 订单批次规格表
    string ABCDProcessPlan_OrderBatchSpecificationTable = "F0000017";

    // 取样计划完成时间
    string ABCDProcessPlan_CompletionTimeOfSamplingPlan = "F0000212";
    // 本工序需求期-锯切
    string ABCDProcessPlan_DemandPeriodOfThisSectionSawCut = "F0000059";

    // 本工序需求期-锻压
    string ABCDProcessPlan_DemandPeriodOfThisSectionForge = "F0000063";
    // 订单号
    string ABCDProcessPlan_OrderNumber = "F0000001";
    // 加工单位-钻孔
    string ABCDProcessPlan_ProcessingUnitDrilling = "F0000084";

    // 再生工序
    string ABCDProcessPlan_RecycledSection = "F0000142";
    // 冷加工完成时间
    string ABCDProcessPlan_ColdProcessingPlan = "F0000024";

    // 本工序需求期-热处理
    string ABCDProcessPlan_DemandPeriodOfThisSectionHeatTreatment = "F0000071";
    // 订单批次号
    string ABCDProcessPlan_OrderBatchNumber = "F0000003";

    // 钻孔计划完成时间
    string ABCDProcessPlan_DrillingPlannedCompletionTime = "F0000102";

    // 加工单位-辗环
    string ABCDProcessPlan_ProcessingUnitRingRolling = "F0000066";

    // 再生品ID
    string ABCDProcessPlan_RecycledProductID = "F0000137";

    // 规格参数
    string ABCDProcessPlan_SpecificationParameters = "F0000028";
    // 本工序需求期-毛坯
    string ABCDProcessPlan_DemandPeriodOfThisSectionRoughCast = "F0000075";
    // 加工单位-毛坯
    string ABCDProcessPlan_ProcessingUnitRoughCast = "F0000074";
    // 原材料类型
    string ABCDProcessPlan_RawMaterialType = "F0000220";

    string ABCDProcessPlan_ObjectId = "ObjectId";


    // 生产计划,派工表
    string Dispatchs_TableCode = "D001419c08bb982ac44481a9439076269a8f783";
    // 粗车计划完成时间
    string Dispatchs_RoughCuttingPlanCompletionTime = "F0000004";
    // ID
    string Dispatchs_ID = "F0000025";

    // 订单号
    string Dispatchs_OrderNumber = "F0000038";

    // 精车计划完成时间
    string Dispatchs_FinishingPlanCompletionTime = "F0000005";

    // 取样计划完成时间
    string Dispatchs_SamplingPlanCompletionTime = "F0000003";

    // 工件号
    string Dispatchs_WorkpieceNumber = "F0000039";
    // 钻孔计划完成时间
    string Dispatchs_DrillingPlanCompletionTime = "F0000006";


    // 生产计划,取样派工子表
    string DispatchSamplingSubTable_TableCode = "D001419Fc9380612ad364043a33702a36bf5fde9";
    // 派工人员
    string DispatchSamplingSubTable_Name = "F0000036";
    // 派工任务
    string DispatchSamplingSubTable_TaskName = "F0000059";
    // 派工量
    string DispatchSamplingSubTable_ProcessingQuantity = "F0000019";

    // 生产制造流程,工艺流程表
    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";
    // 计划本取
    string ProcessFlow_PlannedTake = "F0000118";
    // 双轧关联表单
    string ProcessFlow_DoubleTieAssociatedForm = "F0000143";

    // 订单批次号
    string ProcessFlow_OrderBatchNumber = "F0000002";

    // 产品规格
    string ProcessFlow_SpecificationParameters = "F0000151";
    // 本工序需求期-锻压
    string ProcessFlow_DemandPeriodOfThisProcedureForging = "F0000073";
    // 质量状态
    string ProcessFlow_QualityStatus = "F0000065";
    // 本工序需求期-毛坯
    string ProcessFlow_DemandPeriodOfThisSectionRoughCast = "F0000082";
    // 原材料类型
    string ProcessFlow_RawMaterialType = "F0000062";

    // 计划热处理炉号
    string ProcessFlow_PlannedHeatTreatmentHeatNumber = "F0000124";
    // 本工序需求期-辗环
    string ProcessFlow_DemandPeriodOfThisSectionRingRolling = "F0000076";
    // 本工序需求期-锯切
    string ProcessFlow_DemandPeriodOfThisSectionSawCut = "F0000068";

    // 精车加工单位
    string ProcessFlow_FinishingUnit = "F0000040";

    // ID
    string ProcessFlow_ID = "F0000006";
    // 钻孔加工单位
    string ProcessFlow_DrillingUnit = "F0000041";
    // 订单批次规格号
    string ProcessFlow_OrderBatchSpecificationNumber = "F0000004";
    // 订单号
    string ProcessFlow_OrderNumber = "F0000001";

    // 锯切加工单位
    string ProcessFlow_SawCutProcessingUnit = "F0000024";

    // 原材料编号
    string ProcessFlow_RawMaterialNumber = "F0000058";
    // 数据代码
    string ProcessFlow_DataCode = "F0000069";

    // 再生品ID
    string ProcessFlow_RecycledProductID = "F0000060";

    // 计划轧制方式
    string ProcessFlow_PlannedRollingMode = "F0000112";
    // 进度管理信息
    string ProcessFlow_ScheduleManagementInformation = "Progress";
    // 本工序需求期-精车
    string ProcessFlow_DemandPeriodOfThisSectionFinishing = "F0000088";   
    // 辗环加工单位
    string ProcessFlow_RingRollingProcessingUnit = "F0000028";
   
    // 双轧工件号
    string ProcessFlow_DoubleTieWorkpieceNumber = "F0000144";
   
    // 订单规格号
    string ProcessFlow_OrderSpecificationNumber = "F0000003";
    // 成品单重
    string ProcessFlow_FinishedProductUnitWeight = "F0000017";
  
    // 粗车加工单位
    string ProcessFlow_RoughCuttingUnit = "F0000039";
  
    // 工件号
    string ProcessFlow_WorkpieceNumber = "F0000005";
    // 本工序需求期-钻孔
    string ProcessFlow_DemandPeriodOfThisSectionDrilling = "F0000097";
  
    // 毛坯加工单位
    string ProcessFlow_BlankProcessingUnit = "F0000030";
    // 锻压加工单位
    string ProcessFlow_ForgingProcessingUnit = "F0000027";  
    // 工序计划表
    string ProcessFlow_SectionSchedule = "F0000126";    
    // 原料库
    string ProcessFlow_RawMaterialWarehouse = "F0000119";
   
    // 再生工序
    string ProcessFlow_RecycledSection = "F0000121";
    // 派工信息
    string ProcessFlow_DispatchingInformation = "F0000148";
   
    // 热处理加工单位
    string ProcessFlow_HeatTreatmentProcessingUnit = "F0000029";
   
    // 双扎分割前工件
    string ProcessFlow_DoubleTieTheWorkpieceBeforeSegmentation = "F0000150";
    // 本工序需求期-粗车
    string ProcessFlow_DemandPeriodOfThisSectionRoughCutting = "F0000085";
    // 部门代码
    string ProcessFlow_DepartmentCode = "F0000154";
    // 当前工序
    string ProcessFlow_CurrentSection = "F0000018";
    // 产品种类
    string ProcessFlow_ProductType = "F0000023";
  
    // 数量-切割前
    string ProcessFlow_QuantityBeforeCutting = "F0000145";   
    // 产品名称
    string ProcessFlow_ProductName = "F0000007";
    // 本工序需求期-热处理
    string ProcessFlow_DemandPeriodOfThisSectionHeatTreatment = "F0000079";
   

    // 生产数据分析,实时生产动态----生产进度管理
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";   
    // 市场需求达成进度
    string ScheduleManagement_MarketDemandAchievementProgress = "F0000020";   
    // 数据代码
    string ScheduleManagement_DataCode = "F0000014";   
    // 订单批次规格表
    string ScheduleManagement_OrderBatchSpecificationTable = "F0000069";
    
    // 订单规格表
    string ScheduleManagement_OrderSpecificationTable = "F0000018";
   
    // ID
    string ScheduleManagement_ID = "F0000001";
    // 质量状态
    string ScheduleManagement_QualityStatus = "F0000010";
   
    // 工艺流程表
    string ScheduleManagement_ProcessFlowTable = "F0000075";
    
    // 工序计划表
    string ScheduleManagement_ProcessPlanTable = "F0000019";
    // 订单批次表
    string ScheduleManagement_OrderBatchTable = "F0000026";    
    // 冷加工计划期
    string ScheduleManagement_ColdProcessingPlanPeriod = "F0000013";
       // 热加工计划期
    string ScheduleManagement_HotProcessingPlanPeriod = "F0000012";
   


    // 生产计划,粗车派工子表
    string DispatchRoughSubTable_TableCode = "D001419Ffb3f2e583e31421e8aaa5a085bbada58";
    // 派工任务
    string DispatchRoughSubTable_TaskName = "F0000058";
   
    // 派工人员
    string DispatchRoughSubTable_Name = "F0000037";
    // 粗车工时
 
    // 派工量
    string DispatchRoughSubTable_ProcessingQuantity = "F0000020";

    // 生产计划,精车派工子表
    string DispatchFinishSubTable_TableCode = "D001419F4a23f2f26a01428f952a593da3d99fe5";
    // 派工量
    string DispatchFinishSubTable_ProcessingQuantity = "F0000021";
    
    // 派工人员
    string DispatchFinishSubTable_Name = "F0000014";
    // 派工任务
    string DispatchFinishSubTable_TaskName = "F0000060";

    // 生产计划,钻孔派工子表
    string DispatchDrillingSubTable_TableCode = "D001419F5ccfa7d5acad41bf98c640057f2570ae";
   
    // 派工任务
    string DispatchDrillingSubTable_TaskName = "F0000061";
    // 派工人员
    string DispatchDrillingSubTable_Name = "F0000015";
   
    // 派工量
    string DispatchDrillingSubTable_ProcessingQuantity = "F0000022";
   
    public D001419Szlywopbivyrv1d64301ta5xv4_ListViewController(H3.SmartForm.ListViewRequest request) : base(request)
    {
    }

    protected override void OnLoad(H3.SmartForm.LoadListViewResponse response)
    {
        base.OnLoad(response);
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
        if (objectIds.Length == 0)
        {
            return;
        }
        bool hasError = false;
        foreach (string objectId in objectIds)
        {
            BizObject objPlan = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            if (objPlan[ABCDProcessPlan_OpenProcess] + string.Empty == "已开启") { continue; }
            if (objPlan["DispatchTable"] + string.Empty == "")
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
                if (objPlan[ABCDProcessPlan_DoubleTieWorkpieceNumber] + string.Empty == string.Empty)
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
                objPlanB[ABCDProcessPlan_OpenProcess] = "已开启";
                //工序计划-工艺流程表
                objPlanB[ABCDProcessPlan_ProcessFlowTable] = objFlowB.ObjectId;
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
            objPlan[ABCDProcessPlan_OpenProcess] = "已开启";
            //工序计划
            objPlan[ABCDProcessPlan_ProcessFlowTable] = objFlow.ObjectId;
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
        if (objectIds.Length == 0)
        {
            return;
        }

        foreach (string objectId in objectIds)
        {
            BizObject objPlan = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            if (objPlan["DispatchTable"] + string.Empty != "") { continue; }
            BizObject dispatchObj = CreateDispatchObject(objPlan, objectId);
            objPlan["DispatchTable"] = dispatchObj.ObjectId;
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
        dispatchObj[Dispatchs_OrderNumber] = objPlan[ABCDProcessPlan_OrderNumber] + string.Empty;
        dispatchObj[Dispatchs_WorkpieceNumber] = objPlan[ABCDProcessPlan_WorkpieceNumber] + string.Empty;
        dispatchObj[Dispatchs_ID] = objPlan[ABCDProcessPlan_ID] + string.Empty;

        //派工表-数据代码
        dispatchObj["F0000070"] = objPlan[ABCDProcessPlan_DataCode] + string.Empty;
        //派工表-部门代码
        dispatchObj["F0000071"] = objPlan["F0000236"] + string.Empty;
        //派工表-版本号
        dispatchObj["F0000091"] = objPlan["F0000235"] + string.Empty;
        //派工表-部门多选
        dispatchObj["F0000072"] = objPlan["F0000234"];

        //取样计划完成日期
        dispatchObj[Dispatchs_SamplingPlanCompletionTime] = objPlan[ABCDProcessPlan_CompletionTimeOfSamplingPlan] + string.Empty;
        //派工-订单规格表
        dispatchObj["F0000055"] = objPlan[ABCDProcessPlan_OrderSpecificationTable] + string.Empty;

        dispatchObj[Dispatchs_RoughCuttingPlanCompletionTime] = objPlan[ABCDProcessPlan_RoughCuttingPlanCompletionTime] + string.Empty;
        dispatchObj[Dispatchs_FinishingPlanCompletionTime] = objPlan[ABCDProcessPlan_FinishingPlannedCompletionTime] + string.Empty;
        dispatchObj[Dispatchs_DrillingPlanCompletionTime] = objPlan[ABCDProcessPlan_DrillingPlannedCompletionTime] + string.Empty;


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

        if (objPlan[ABCDProcessPlan_PlannedRollingMode] + string.Empty == "双轧")
        {
            //工件号转数值
            string workpieceNumber = objPlan[ABCDProcessPlan_WorkpieceNumber] + string.Empty;
            //双轧工件号转数值
            string doubleTieWorkpieceNumber = objPlan[ABCDProcessPlan_DoubleTieWorkpieceNumber] + string.Empty;

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
        objFlow[ProcessFlow_OrderNumber] = objPlan[ABCDProcessPlan_OrderNumber] + string.Empty;
        //订单批次号
        objFlow[ProcessFlow_OrderBatchNumber] = objPlan[ABCDProcessPlan_OrderBatchNumber] + string.Empty;
        //订单规格号
        string orderSpec = objPlan[ABCDProcessPlan_OrderSpecificationNumber] + string.Empty;
        objFlow[ProcessFlow_OrderSpecificationNumber] = orderSpec;
        //订单批次规格号
        objFlow[ProcessFlow_OrderBatchSpecificationNumber] = objPlan[ABCDProcessPlan_OrderBatchSpecificationNumber] + string.Empty;
        //工件号
        objFlow[ProcessFlow_WorkpieceNumber] = objPlan[ABCDProcessPlan_WorkpieceNumber] + string.Empty;
        //ID
        objFlow[ProcessFlow_ID] = objPlan[ABCDProcessPlan_ID] + string.Empty;
        //产品名称
        objFlow[ProcessFlow_ProductName] = objPlan[ABCDProcessPlan_ProductName] + string.Empty;
        //产品种类
        objFlow[ProcessFlow_ProductType] = objPlan["ProductType"];

        //成品单重
        objFlow[ProcessFlow_FinishedProductUnitWeight] = objPlan[ABCDProcessPlan_FinishedProductUnitWeight] + string.Empty;
        //规格参数
        objFlow[ProcessFlow_SpecificationParameters] = objPlan[ABCDProcessPlan_SpecificationParameters] + string.Empty;

        //数据代码
        objFlow[ProcessFlow_DataCode] = objPlan[ABCDProcessPlan_DataCode] + string.Empty;

        //部门代码
        objFlow["F0000154"] = objPlan["F0000236"] + string.Empty;

        //版本号
        objFlow["F0000153"] = objPlan["F0000235"] + string.Empty;

        //部门多选
        objFlow["F0000152"] = objPlan["F0000234"];

        //再生品ID
        objFlow[ProcessFlow_RecycledProductID] = objPlan[ABCDProcessPlan_RecycledProductID] + string.Empty;
        if (objPlan[ABCDProcessPlan_RecycledProductID] + string.Empty != string.Empty)
        {
            objFlow[ProcessFlow_CurrentSection] = "再生库";
        }
        //质量状态
        objFlow[ProcessFlow_QualityStatus] = "正常";
        //再生工序
        objFlow[ProcessFlow_RecycledSection] = objPlan[ABCDProcessPlan_RecycledSection] + string.Empty;
        //计划轧制方式
        objFlow[ProcessFlow_PlannedRollingMode] = objPlan[ABCDProcessPlan_PlannedRollingMode] + string.Empty;

        //计划本取
        objFlow[ProcessFlow_PlannedTake] = objPlan[ABCDProcessPlan_PlanThisOptionTakes] + string.Empty;

        //计划热处理炉号
        objFlow[ProcessFlow_PlannedHeatTreatmentHeatNumber] = objPlan[ABCDProcessPlan_HeatTreatmentFurnaceNumber] + string.Empty;

        //锯切加工单位
        objFlow[ProcessFlow_SawCutProcessingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitSawCut] + string.Empty;
        //本工序需求期-锯切
        objFlow[ProcessFlow_DemandPeriodOfThisSectionSawCut] = objPlan[ABCDProcessPlan_DemandPeriodOfThisSectionSawCut] + string.Empty;

        //锻压加工单位
        objFlow[ProcessFlow_ForgingProcessingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitForging] + string.Empty;
        //本工序需求期-锻压
        objFlow[ProcessFlow_DemandPeriodOfThisProcedureForging] = objPlan[ABCDProcessPlan_DemandPeriodOfThisSectionForge] + string.Empty;

        //辗环加工单位
        objFlow[ProcessFlow_RingRollingProcessingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitRingRolling] + string.Empty;
        //本工序需求期-辗环
        objFlow[ProcessFlow_DemandPeriodOfThisSectionRingRolling] = objPlan[ABCDProcessPlan_DemandPeriodOfThisSectionRingRolling] + string.Empty;

        //热处理加工单位
        objFlow[ProcessFlow_HeatTreatmentProcessingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitHeatTreatment] + string.Empty;
        //本工序需求期-热处理
        objFlow[ProcessFlow_DemandPeriodOfThisSectionHeatTreatment] = objPlan[ABCDProcessPlan_DemandPeriodOfThisSectionHeatTreatment] + string.Empty;

        //毛坯加工单位
        objFlow[ProcessFlow_BlankProcessingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitRoughCast] + string.Empty;
        //本工序需求期-毛坯
        objFlow[ProcessFlow_DemandPeriodOfThisSectionRoughCast] = objPlan[ABCDProcessPlan_DemandPeriodOfThisSectionRoughCast] + string.Empty;

        //粗车加工单位
        objFlow[ProcessFlow_RoughCuttingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitRoughCutting] + string.Empty;
        //本工序需求期-粗车
        objFlow[ProcessFlow_DemandPeriodOfThisSectionRoughCutting] = objPlan[ABCDProcessPlan_RoughCuttingPlanCompletionTime] + string.Empty;


        //精车加工单位
        objFlow[ProcessFlow_FinishingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitFinishing] + string.Empty;

        //本工序需求期-精车
        objFlow[ProcessFlow_DemandPeriodOfThisSectionFinishing] = objPlan[ABCDProcessPlan_FinishingPlannedCompletionTime] + string.Empty;


        //钻孔加工单位
        objFlow[ProcessFlow_DrillingUnit] = objPlan[ABCDProcessPlan_ProcessingUnitDrilling] + string.Empty;
        //本工序需求期-钻孔
        objFlow[ProcessFlow_DemandPeriodOfThisSectionDrilling] = objPlan[ABCDProcessPlan_DrillingPlannedCompletionTime] + string.Empty;


        //工序计划表
        objFlow[ProcessFlow_SectionSchedule] = objPlan[ABCDProcessPlan_ObjectId] + string.Empty;

        //双轧关联表单
        objFlow[ProcessFlow_DoubleTieAssociatedForm] = objPlan[ABCDProcessPlan_DoubleTieAssociatedForm] + string.Empty;
        //双轧工件号
        objFlow[ProcessFlow_DoubleTieWorkpieceNumber] = objPlan[ABCDProcessPlan_DoubleTieWorkpieceNumber] + string.Empty;
        //数量-切割前
        objFlow[ProcessFlow_QuantityBeforeCutting] = objPlan[ABCDProcessPlan_QuantityBeforeCutting] + string.Empty;


        //原料库
        objFlow[ProcessFlow_RawMaterialWarehouse] = objPlan[ABCDProcessPlan_RawMaterialStorage] + string.Empty;
        //原材料类型
        objFlow[ProcessFlow_RawMaterialType] = objPlan[ABCDProcessPlan_RawMaterialType] + string.Empty;
        //原材料编号
        objFlow[ProcessFlow_RawMaterialNumber] = objPlan[ABCDProcessPlan_RawMaterialNumber] + string.Empty;

        //工艺流程表赋值无派工加工审批权限人
        objFlow["F0000194"] = objPlan["F0000252"]; //总计划员
        objFlow["F0000193"] = objPlan["F0000251"]; //冷加工科长
        objFlow["F0000195"] = objPlan["F0000247"]; //取样班组长
        objFlow["F0000196"] = objPlan["F0000248"]; //粗车班组长
        objFlow["F0000197"] = objPlan["F0000249"]; //精车班组长
        objFlow["F0000198"] = objPlan["F0000250"]; //钻孔班组长

        //"工艺流程"的派工信息字段取派工表ObjectId的值
        objFlow[ProcessFlow_DispatchingInformation] = objPlan["DispatchTable"];

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
        objProcess[ScheduleManagement_ID] = objPlan[ABCDProcessPlan_ID] + string.Empty;
        //进度管理-数据代码
        objProcess["F0000014"] = objPlan[ABCDProcessPlan_DataCode] + string.Empty;
        //进度管理-部门代码
        objProcess["F0000076"] = objPlan["F0000236"] + string.Empty;
        //进度管理-版本号
        objProcess["F0000077"] = objPlan["F0000235"] + string.Empty;
        //进度管理-部门多选
        objProcess["F0000078"] = objPlan["F0000234"];

        objProcess[ScheduleManagement_DataCode] = objPlan[ABCDProcessPlan_DataCode] + string.Empty;

        //热加工计划期
        objProcess[ScheduleManagement_HotProcessingPlanPeriod] = objPlan[ABCDProcessPlan_HotProcessingPlan] + string.Empty;
        //冷加工计划期
        objProcess[ScheduleManagement_ColdProcessingPlanPeriod] = objPlan[ABCDProcessPlan_ColdProcessingPlan] + string.Empty;
        //市场需求达成进度
        objProcess[ScheduleManagement_MarketDemandAchievementProgress] = "未投产";
        //订单批次表
        objProcess[ScheduleManagement_OrderBatchTable] = objPlan[ABCDProcessPlan_OrderBatchTable] + string.Empty;
        //订单规格表
        objProcess[ScheduleManagement_OrderSpecificationTable] = objPlan[ABCDProcessPlan_OrderSpecificationTable] + string.Empty;

        //订单批次规格表
        objProcess[ScheduleManagement_OrderBatchSpecificationTable] = objPlan[ABCDProcessPlan_OrderBatchSpecificationTable] + string.Empty;
        //质量状态
        objProcess[ScheduleManagement_QualityStatus] = "正常";

        objProcess[ScheduleManagement_ProcessPlanTable] = objPlan[ABCDProcessPlan_ObjectId] + string.Empty;

        objProcess.Status = BizObjectStatus.Effective;

        //进度管理数据
        objProcess.Create();
        return objProcess;
    }


}
