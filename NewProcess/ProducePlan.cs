
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using static H3.SmartForm;
using H3.DataModel;

public class D001419Szlywopbivyrv1d64301ta5xv4_ListViewController : H3.SmartForm.ListViewController
{
    string info = "";
    string activityCode = "领料生产";
    private IEngine Engine;

    public ListViewRequest Request { get; set; }

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
            if (objPlan[ABCDProcessPlan.OpenProcess] + string.Empty == "已开启") { continue; }
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
                if (objPlan[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty == string.Empty)
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
                objProcessB[RealTimeDynamicProduction.ProcessFlowTable] = objFlowB.ObjectId;
                objProcessB.Update();

                //开启流程
                Tools.WorkFlow.StartWorkflow(this.Engine, objFlowB, this.Request.UserContext.UserId, true);
                //开启状态
                objPlanB[ABCDProcessPlan.OpenProcess] = "已开启";
                //工序计划-工艺流程表
                objPlanB[ABCDProcessPlan.ProcessFlowTable] = objFlowB.ObjectId;
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
            objProcess[RealTimeDynamicProduction.ProcessFlowTable] = objFlow.ObjectId;
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
            objPlan[ABCDProcessPlan.OpenProcess] = "已开启";
            //工序计划
            objPlan[ABCDProcessPlan.ProcessFlowTable] = objFlow.ObjectId;
            //更新本表单
            objPlan.Update();
            //加载工艺流程数据  
            BizObject objFlowDisPatch = Tools.BizOperation.Load(this.Engine, ProcessFlow.TableCode, objFlow.ObjectId);
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
        BizObject dispatchObj = Tools.BizOperation.New(this.Engine, Dispatchs.TableCode);   //派工表
        dispatchObj[Dispatchs.OrderNumber] = objPlan[ABCDProcessPlan.OrderNumber] + string.Empty;
        dispatchObj[Dispatchs.WorkpieceNumber] = objPlan[ABCDProcessPlan.WorkpieceNumber] + string.Empty;
        dispatchObj[Dispatchs.ID] = objPlan[ABCDProcessPlan.ID] + string.Empty;

        //派工表-数据代码
        dispatchObj["F0000070"] = objPlan[ABCDProcessPlan.DataCode] + string.Empty;
        //派工表-部门代码
        dispatchObj["F0000071"] = objPlan["F0000236"] + string.Empty;
        //派工表-版本号
        dispatchObj["F0000091"] = objPlan["F0000235"] + string.Empty;
        //派工表-部门多选
        dispatchObj["F0000072"] = objPlan["F0000234"];

        //取样计划完成日期
        dispatchObj[Dispatchs.SamplingPlanCompletionTime] = objPlan[ABCDProcessPlan.CompletionTimeOfSamplingPlan] + string.Empty;
        //派工-订单规格表
        dispatchObj["F0000055"] = objPlan[ABCDProcessPlan.OrderSpecificationTable] + string.Empty;

        dispatchObj[Dispatchs.RoughTurningPlanCompletionTime] = objPlan[ABCDProcessPlan.RoughTurningPlanCompletionTime] + string.Empty;
        dispatchObj[Dispatchs.FinishTurningPlanCompletionTime] = objPlan[ABCDProcessPlan.FinishTurningPlannedCompletionTime] + string.Empty;
        dispatchObj[Dispatchs.DrillingPlanCompletionTime] = objPlan[ABCDProcessPlan.DrillingPlannedCompletionTime] + string.Empty;


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
        BizObject dSampling = Tools.BizOperation.New(this.Engine, DispatchSamplingSubTable.TableCode);
        //派工任务名
        dSampling[DispatchSamplingSubTable.TaskName] = "取样任务";
        //派工人员
        dSampling[DispatchSamplingSubTable.Name] = new object[1] { samplingTeamLeader != "" ? samplingTeamLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dSampling[DispatchSamplingSubTable.ProcessingQuantity] = 1.0;
        //取样派工总量
        dispatchObj["F0000098"] = 1.0;
        //取样（派工）子表控件
        dispatchObj[DispatchSamplingSubTable.TableCode] = new BizObject[1] { dSampling };


        //派工粗车子表业务对象
        BizObject dRough = Tools.BizOperation.New(this.Engine, DispatchRoughSubTable.TableCode);
        //派工任务名
        dRough[DispatchRoughSubTable.TaskName] = "粗车任务";
        //派工人员
        dRough[DispatchRoughSubTable.Name] = new object[1] { roughingShiftLeader != "" ? roughingShiftLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };    
        //派工量
        dRough[DispatchRoughSubTable.ProcessingQuantity] = 1.0;  
        //粗车派工总量
        dispatchObj["F0000099"] = 1.0;
        //粗车（派工）子表控件           
        dispatchObj[DispatchRoughSubTable.TableCode] = new BizObject[1] { dRough };

        //派工精车子表业务对象
        BizObject dFinish = Tools.BizOperation.New(this.Engine, DispatchFinishSubTable.TableCode);
        //派工任务名
        dFinish[DispatchFinishSubTable.TaskName] = "精车任务";
        //派工人员
        dFinish[DispatchFinishSubTable.Name] = new object[1] { finishingShiftLeader != "" ? finishingShiftLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dFinish[DispatchFinishSubTable.ProcessingQuantity] = 1.0;
        //精车派工总量
        dispatchObj["F0000100"] = 1.0;
        //精车（派工）子表控件
        dispatchObj[DispatchFinishSubTable.TableCode] = new BizObject[1] { dFinish };      

        //派工钻孔子表   
        BizObject dDrill = Tools.BizOperation.New(this.Engine, DispatchDrillSubTable.TableCode);
        //派工任务名
        dDrill[DispatchDrillSubTable.TaskName] = "钻孔任务";
        //派工人员
        dDrill[DispatchDrillSubTable.Name] = new object[1] { drillingTeamLeader != "" ? drillingTeamLeader : coldWorkingSectionChief != "" ? coldWorkingSectionChief : chiefPlanner };
        //派工量
        dDrill[DispatchDrillSubTable.ProcessingQuantity] = 1.0;
        //钻孔派工总量
        dispatchObj["F0000101"] = 1.0;
        //钻孔（派工）子表控件
        dispatchObj[DispatchDrillSubTable.TableCode] = new BizObject[1] { dDrill };

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
        BizObject objFlow = Tools.BizOperation.New(this.Engine, ProcessFlow.TableCode);

        if (objPlan[ABCDProcessPlan.PlannedRollingMethod] + string.Empty == "双轧")
        {
            //工件号转数值
            string workpieceNumber = objPlan[ABCDProcessPlan.WorkpieceNumber] + string.Empty;
            //双轧工件号转数值
            string doubleTieWorkpieceNumber = objPlan[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty;

            //工件号大于双轧工件号
            if (string.Compare(workpieceNumber, doubleTieWorkpieceNumber, true) > 0)
            {
                //双扎工艺分割前工件值为是
                objFlow[ProcessFlow.DoubleTieTheWorkpieceBeforeSegmentation] = "是";
            }
            else
            {
                //双扎工艺分割前工件值为否
                objFlow[ProcessFlow.DoubleTieTheWorkpieceBeforeSegmentation] = "否";
            }
        }



        //订单号
        objFlow[ProcessFlow.OrderNumber] = objPlan[ABCDProcessPlan.OrderNumber] + string.Empty;
        //订单批次号
        objFlow[ProcessFlow.OrderBatchNumber] = objPlan[ABCDProcessPlan.OrderBatchNumber] + string.Empty;
        //订单规格号
        string orderSpec = objPlan[ABCDProcessPlan.OrderSpecificationNumber] + string.Empty;
        objFlow[ProcessFlow.OrderSpecificationNumber] = orderSpec;
        //订单批次规格号
        objFlow[ProcessFlow.OrderBatchSpecificationNumber] = objPlan[ABCDProcessPlan.OrderBatchSpecificationNumber] + string.Empty;
        //工件号
        objFlow[ProcessFlow.WorkpieceNumber] = objPlan[ABCDProcessPlan.WorkpieceNumber] + string.Empty;
        //ID
        objFlow[ProcessFlow.ID] = objPlan[ABCDProcessPlan.ID] + string.Empty;
        //产品名称
        objFlow[ProcessFlow.ProductName] = objPlan[ABCDProcessPlan.ProductName] + string.Empty;
        //产品种类
        objFlow[ProcessFlow.ProductType] = objPlan["ProductType"];

        //成品单重
        objFlow[ProcessFlow.FinishedProductUnitWeight] = objPlan[ABCDProcessPlan.FinishedProductUnitWeight] + string.Empty;
        //规格参数
        objFlow[ProcessFlow.SpecificationParameters] = objPlan[ABCDProcessPlan.SpecificationParameters] + string.Empty;

        //数据代码
        objFlow[ProcessFlow.DataCode] = objPlan[ABCDProcessPlan.DataCode] + string.Empty;

        //部门代码
        objFlow["F0000154"] = objPlan["F0000236"] + string.Empty;

        //版本号
        objFlow["F0000153"] = objPlan["F0000235"] + string.Empty;

        //部门多选
        objFlow["F0000152"] = objPlan["F0000234"];

        //再生品ID
        objFlow[ProcessFlow.RecycledProductID] = objPlan[ABCDProcessPlan.RecycledProductID] + string.Empty;
        if (objPlan[ABCDProcessPlan.RecycledProductID] + string.Empty != string.Empty)
        {
            objFlow[ProcessFlow.CurrentOperation] = "再生库";
        }
        //质量状态
        objFlow[ProcessFlow.QualityStatus] = "正常";
        //再生工序
        objFlow[ProcessFlow.RegenerationProcess] = objPlan[ABCDProcessPlan.RegenerationProcess] + string.Empty;
        //计划轧制方式
        objFlow[ProcessFlow.PlannedRollingMethod] = objPlan[ABCDProcessPlan.PlannedRollingMethod] + string.Empty;

        //计划本取
        objFlow[ProcessFlow.PlannedTake] = objPlan[ABCDProcessPlan.PlanThisOptionTakes] + string.Empty;

        //计划热处理炉号
        objFlow[ProcessFlow.PlannedHeatTreatmentHeatNumber] = objPlan[ABCDProcessPlan.HeatTreatmentFurnaceNumber] + string.Empty;

        //锯切加工单位
        objFlow[ProcessFlow.SawingProcessingUnit] = objPlan[ABCDProcessPlan.ProcessingUnitSawing] + string.Empty;
        //本工序需求期-锯切
        objFlow[ProcessFlow.DemandPeriodOfThisProcessSawing] = objPlan[ABCDProcessPlan.DemandPeriodOfThisProcedureSawing] + string.Empty;

        //锻压加工单位
        objFlow[ProcessFlow.ForgingProcessingUnit] = objPlan[ABCDProcessPlan.ProcessingUnitForging] + string.Empty;
        //本工序需求期-锻压
        objFlow[ProcessFlow.DemandPeriodOfThisProcedureForging] = objPlan[ABCDProcessPlan.DemandPeriodOfThisOperationForging] + string.Empty;

        //辗环加工单位
        objFlow[ProcessFlow.RingRollingProcessingUnit] = objPlan[ABCDProcessPlan.ProcessingUnitRingRolling] + string.Empty;
        //本工序需求期-辗环
        objFlow[ProcessFlow.DemandPeriodOfThisProcedureRingRolling] = objPlan[ABCDProcessPlan.DemandPeriodOfThisOperationRingRolling] + string.Empty;

        //热处理加工单位
        objFlow[ProcessFlow.HeatTreatmentProcessingUnit] = objPlan[ABCDProcessPlan.ProcessingUnitHeatTreatment] + string.Empty;
        //本工序需求期-热处理
        objFlow[ProcessFlow.DemandPeriodOfThisProcedureHeatTreatment] = objPlan[ABCDProcessPlan.DemandPeriodOfThisOperationHeatTreatment] + string.Empty;

        //毛坯加工单位
        objFlow[ProcessFlow.BlankProcessingUnit] = objPlan[ABCDProcessPlan.ProcessingUnitBlank] + string.Empty;
        //本工序需求期-毛坯
        objFlow[ProcessFlow.DemandPeriodOfThisOperationBlank] = objPlan[ABCDProcessPlan.DemandPeriodOfThisOperationBlank] + string.Empty;

        //粗车加工单位
        objFlow[ProcessFlow.RoughTurningUnit] = objPlan[ABCDProcessPlan.ProcessingUnitRoughTurning] + string.Empty;
        //本工序需求期-粗车
        objFlow[ProcessFlow.DemandPeriodOfThisOperationRoughTurning] = objPlan[ABCDProcessPlan.RoughTurningPlanCompletionTime] + string.Empty;


        //精车加工单位
        objFlow[ProcessFlow.FinishTurningUnit] = objPlan[ABCDProcessPlan.ProcessingUnitFinishTurning] + string.Empty;

        //本工序需求期-精车
        objFlow[ProcessFlow.DemandPeriodOfThisProcessFinishTurning] = objPlan[ABCDProcessPlan.FinishTurningPlannedCompletionTime] + string.Empty;


        //钻孔加工单位
        objFlow[ProcessFlow.DrillingUnit] = objPlan[ABCDProcessPlan.ProcessingUnitDrilling] + string.Empty;
        //本工序需求期-钻孔
        objFlow[ProcessFlow.DemandPeriodOfThisProcessDrilling] = objPlan[ABCDProcessPlan.DrillingPlannedCompletionTime] + string.Empty;


        //工序计划表
        objFlow[ProcessFlow.OperationSchedule] = objPlan[ABCDProcessPlan.ObjectId] + string.Empty;

        //双轧关联表单
        objFlow[ProcessFlow.DoubleTieAssociatedForm] = objPlan[ABCDProcessPlan.DoubleTieAssociatedForm] + string.Empty;
        //双轧工件号
        objFlow[ProcessFlow.DoubleTieWorkpieceNumber] = objPlan[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty;
        //数量-切割前
        objFlow[ProcessFlow.QuantityBeforeCutting] = objPlan[ABCDProcessPlan.QuantityBeforeCutting] + string.Empty;


        //原料库
        objFlow[ProcessFlow.RawMaterialWarehouse] = objPlan[ABCDProcessPlan.RawMaterialStorage] + string.Empty;
        //原材料类型
        objFlow[ProcessFlow.RawMaterialType] = objPlan[ABCDProcessPlan.RawMaterialType] + string.Empty;
        //原材料编号
        objFlow[ProcessFlow.RawMaterialNumber] = objPlan[ABCDProcessPlan.RawMaterialNumber] + string.Empty;

        //工艺流程表赋值无派工加工审批权限人
        objFlow["F0000194"] = objPlan["F0000252"]; //总计划员
        objFlow["F0000193"] = objPlan["F0000251"]; //冷加工科长
        objFlow["F0000195"] = objPlan["F0000247"]; //取样班组长
        objFlow["F0000196"] = objPlan["F0000248"]; //粗车班组长
        objFlow["F0000197"] = objPlan["F0000249"]; //精车班组长
        objFlow["F0000198"] = objPlan["F0000250"]; //钻孔班组长

        //"工艺流程"的派工信息字段取派工表ObjectId的值
        objFlow[ProcessFlow.DispatchingInformation] = objPlan["DispatchTable"];

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
        BizObject objProcess = Tools.BizOperation.New(this.Engine, RealTimeDynamicProduction.TableCode);
        //进度管理-产品ID
        objProcess[RealTimeDynamicProduction.ID] = objPlan[ABCDProcessPlan.ID] + string.Empty;
        //进度管理-数据代码
        objProcess["F0000014"] = objPlan[ABCDProcessPlan.DataCode] + string.Empty;
        //进度管理-部门代码
        objProcess["F0000076"] = objPlan["F0000236"] + string.Empty;
        //进度管理-版本号
        objProcess["F0000077"] = objPlan["F0000235"] + string.Empty;
        //进度管理-部门多选
        objProcess["F0000078"] = objPlan["F0000234"];

        objProcess[RealTimeDynamicProduction.DataCode] = objPlan[ABCDProcessPlan.DataCode] + string.Empty;

        //热加工计划期
        objProcess[RealTimeDynamicProduction.HotProcessingPlanPeriod] = objPlan[ABCDProcessPlan.HotProcessingPlan] + string.Empty;
        //冷加工计划期
        objProcess[RealTimeDynamicProduction.ColdProcessingPlanPeriod] = objPlan[ABCDProcessPlan.ColdProcessingPlan] + string.Empty;
        //市场需求达成进度
        objProcess[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "未投产";
        //订单批次表
        objProcess[RealTimeDynamicProduction.OrderBatchTable] = objPlan[ABCDProcessPlan.OrderBatchTable] + string.Empty;
        //订单规格表
        objProcess[RealTimeDynamicProduction.OrderSpecificationTable] = objPlan[ABCDProcessPlan.OrderSpecificationTable] + string.Empty;

        //订单批次规格表
        objProcess[RealTimeDynamicProduction.OrderBatchSpecificationTable] = objPlan[ABCDProcessPlan.OrderBatchSpecificationTable] + string.Empty;
        //质量状态
        objProcess[RealTimeDynamicProduction.QualityStatus] = "正常";

        objProcess[RealTimeDynamicProduction.OperationPlanTable] = objPlan[ABCDProcessPlan.ObjectId] + string.Empty;

        objProcess.Status = BizObjectStatus.Effective;

        //进度管理数据
        objProcess.Create();
        return objProcess;
    }


}
