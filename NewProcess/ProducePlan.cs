
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using static H3.SmartForm;

public class D001419Szlywopbivyrv1d64301ta5xv4_ListViewController : H3.SmartForm.ListViewController
{
    string info = "";
    string activityCode = "领料生产";
    private IEngine Engine;

    public ListViewRequest Request { get;  set; }

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
            if (actionName == "wuliao")
            {
                //批量开启制造流程
                BatchStartProcess(postValue, response);
            }
            if (actionName == "Dispath") //|| actionCode == "Dispath"
            {
                //批量派工
                BatchModifyDispath(postValue, response);
            }
        }
        catch (Exception ex)
        {
            //负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, null, ex, activityCode);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        base.OnSubmit(actionName, postValue, response);
    }

    /*
     * -- Author:fubin
     * 批量开启制造流程
     * @param postValue 前端接口
     * @param response 表单响应
     */
    public void BatchStartProcess(H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        string[] objectIds = postValue.Data["ObjectIds"] as string[];
        if (objectIds.Length == 0)
        {
            return;
        }
        foreach (string objectId in objectIds)
        {
            H3.DataModel.BizObject flowObject = Tools.BizOperation.New(this.Engine, ProcessFlow.TableCode);
            H3.DataModel.BizObject dispatchObj = Tools.BizOperation.New(this.Engine, Dispatchs.TableCode);
            H3.DataModel.BizObject processObj = Tools.BizOperation.New(this.Engine, RealTimeDynamicProduction.TableCode);
            H3.DataModel.BizObject bizObject = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            H3.DataModel.BizObject acObject = Tools.BizOperation.Load(this.Engine, OrderSpecification.TableCode, bizObject[ABCDProcessPlan.OrderSpecificationTable] + string.Empty);
            if (bizObject[ABCDProcessPlan.OpenProcess] + string.Empty == "已开启") { continue; }
            if (bizObject[ABCDProcessPlan.PlannedRollingMethod] + string.Empty == "双轧")
            {   //双轧工件号校验
                if (bizObject[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty == string.Empty)
                {
                    response.Message = "注意，双轧工件，{双轧工件号}不能为空!";
                    return;
                }
                //工件号转数值
                int workpieceNumber = Convert.ToInt32(bizObject[ABCDProcessPlan.WorkpieceNumber] + string.Empty);
                //双轧工件号转数值
                int doubleTieWorkpieceNumber = Convert.ToInt32(bizObject[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty);

                //工件号大于双轧工件号
                if (workpieceNumber > doubleTieWorkpieceNumber)
                {
                    //双扎工艺分割前工件值为是
                    flowObject[ProcessFlow.DoubleTieTheWorkpieceBeforeSegmentation] = "是";
                }
                else
                {
                    //双扎工艺分割前工件值为否
                    flowObject[ProcessFlow.DoubleTieTheWorkpieceBeforeSegmentation] = "否";
                }
            }
            //订单号
            flowObject[ProcessFlow.OrderNumber] = bizObject[ABCDProcessPlan.OrderNumber] + string.Empty;
            dispatchObj[Dispatchs.OrderNumber] = bizObject[ABCDProcessPlan.OrderNumber] + string.Empty;
            //规格号
            //dispatchObj[Dispatchs.SpecificationNumber] = bizObject[ABCDProcessPlan.SpecificationNumber] + string.Empty;
            //订单批次号
            flowObject[ProcessFlow.OrderBatchNumber] = bizObject[ABCDProcessPlan.OrderBatchNumber] + string.Empty;
            //订单规格号
            string orderSpec = bizObject[ABCDProcessPlan.OrderSpecificationNumber] + string.Empty;
            flowObject[ProcessFlow.OrderSpecificationNumber] = orderSpec;
            //订单批次规格号
            flowObject[ProcessFlow.OrderBatchSpecificationNumber] = bizObject[ABCDProcessPlan.OrderBatchSpecificationNumber] + string.Empty;
            //工件号
            flowObject[ProcessFlow.WorkpieceNumber] = bizObject[ABCDProcessPlan.WorkpieceNumber] + string.Empty;

            dispatchObj[Dispatchs.WorkpieceNumber] = bizObject[ABCDProcessPlan.WorkpieceNumber] + string.Empty;
            //ID
            flowObject[ProcessFlow.ID] = bizObject[ABCDProcessPlan.ID] + string.Empty;
            dispatchObj[Dispatchs.ID] = bizObject[ABCDProcessPlan.ID] + string.Empty;
            processObj[RealTimeDynamicProduction.ID] = bizObject[ABCDProcessPlan.ID] + string.Empty;
            //产品名称
            flowObject[ProcessFlow.ProductName] = bizObject[ABCDProcessPlan.ProductName] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter.ProductMachiningCategory, ProductParameter.TableCode,
                ProductParameter.OrderSpecificationNumber, orderSpec);
            DataTable parameterData = this.Engine.Query.QueryTable(mySql, null);
            if (parameterData != null && parameterData.Rows != null && parameterData.Rows.Count > 0)
            {
                dispatchObj["ParameterForm"] = parameterData.Rows[0]["ObjectId"] + string.Empty;
                //产品参数表
                // flowObject["F0000206"] = parameterData.Rows[0]["ObjectId"] + string.Empty;
                // //车加工类别
                // flowObject["F0000207"] = parameterData.Rows[0][ProductParameter.ProductMachiningCategory] + string.Empty;
                // //钻加工类别
                //  flowObject["F0000208"] = parameterData.Rows[0]["ObjectId"] + string.Empty;
            }
            else
            {
                response.Message = "请确认产品参数表，是否有本产品数据！";
                continue;
            }
            if (acObject != null && (acObject[OrderSpecification.ProductCategory] + string.Empty) != string.Empty)
            {
                //赋值产品种类
                flowObject[ProcessFlow.ProductType] = acObject[OrderSpecification.ProductCategory] + string.Empty;
            }
            else
            {
                response.Message = "请确认订单规格表，是否有本产品数据，产品种类不能为空！";
                continue;
            }
            //成品单重
            flowObject[ProcessFlow.FinishedProductUnitWeight] = bizObject[ABCDProcessPlan.FinishedProductUnitWeight] + string.Empty;
            //规格参数
            flowObject[ProcessFlow.SpecificationParameters] = bizObject[ABCDProcessPlan.SpecificationParameters] + string.Empty;
            //数据代码
            flowObject[ProcessFlow.DataCode] = bizObject[ABCDProcessPlan.DataCode] + string.Empty;
            //派工表-数据代码
            dispatchObj["F0000070"] = bizObject[ABCDProcessPlan.DataCode] + string.Empty;
            //进度管理-数据代码
            processObj["F0000014"] = bizObject[ABCDProcessPlan.DataCode] + string.Empty;
            //部门代码
            flowObject["F0000154"] = bizObject["F0000236"] + string.Empty;
            //派工表-部门代码
            dispatchObj["F0000071"] = bizObject["F0000236"] + string.Empty;
            //进度管理-部门代码
            processObj["F0000076"] = bizObject["F0000236"] + string.Empty;
            //版本号
            flowObject["F0000153"] = bizObject["F0000235"] + string.Empty;
            //派工表-版本号
            dispatchObj["F0000091"] = bizObject["F0000235"] + string.Empty;
            //进度管理-版本号
            processObj["F0000077"] = bizObject["F0000235"] + string.Empty;
            //部门多选
            flowObject["F0000152"] = bizObject["F0000234"];
            //派工表-部门多选
            dispatchObj["F0000072"] = bizObject["F0000234"];
            //进度管理-部门多选
            processObj["F0000078"] = bizObject["F0000234"] + string.Empty;
            //dispatchObj[Dispatchs.DataCode] = bizObject[ABCDProcessPlan.DataCode] + string.Empty;
            processObj[RealTimeDynamicProduction.DataCode] = bizObject[ABCDProcessPlan.DataCode] + string.Empty;
            //再生品ID
            flowObject[ProcessFlow.RecycledProductID] = bizObject[ABCDProcessPlan.RecycledProductID] + string.Empty;
            if (bizObject[ABCDProcessPlan.RecycledProductID] + string.Empty != string.Empty)
            {
                flowObject[ProcessFlow.CurrentOperation] = "再生库";
            }
            //质量状态
            flowObject[ProcessFlow.QualityStatus] = "正常";
            //再生工序
            flowObject[ProcessFlow.RegenerationProcess] = bizObject[ABCDProcessPlan.RegenerationProcess] + string.Empty;
            //计划轧制方式
            flowObject[ProcessFlow.PlannedRollingMethod] = bizObject[ABCDProcessPlan.PlannedRollingMethod] + string.Empty;
            //确认轧制方式
            flowObject[ProcessFlow.DeterminedRollingMethod] = bizObject[ABCDProcessPlan.PlannedRollingMethod] + string.Empty;

            //计划本取
            flowObject[ProcessFlow.PlannedTake] = bizObject[ABCDProcessPlan.PlanThisOptionTakes] + string.Empty;
            //炉次计划
            //flowObject[ProcessFlow.HeatPlan] = bizObject[ABCDProcessPlan.HeatCountPlan] + string.Empty;
            //计划炉次编号
            flowObject[ProcessFlow.PlannedHeatNumber] = bizObject[ABCDProcessPlan.PlannedFurnaceNumber] + string.Empty;
            //计划热处理炉号
            flowObject[ProcessFlow.PlannedHeatTreatmentHeatNumber] = bizObject[ABCDProcessPlan.HeatTreatmentFurnaceNumber] + string.Empty;

            //取样计划完成日期
            dispatchObj[Dispatchs.SamplingPlanCompletionTime] = bizObject[ABCDProcessPlan.CompletionTimeOfSamplingPlan] + string.Empty;
            //热加工计划期
            processObj[RealTimeDynamicProduction.HotProcessingPlanPeriod] = bizObject[ABCDProcessPlan.HotProcessingPlan] + string.Empty;
            //冷加工计划期
            processObj[RealTimeDynamicProduction.ColdProcessingPlanPeriod] = bizObject[ABCDProcessPlan.ColdProcessingPlan] + string.Empty;
            //市场需求达成进度
            processObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "未投产";
            //订单批次表
            processObj[RealTimeDynamicProduction.OrderBatchTable] = bizObject[ABCDProcessPlan.OrderBatchTable] + string.Empty;
            //订单规格表
            processObj[RealTimeDynamicProduction.OrderSpecificationTable] = bizObject[ABCDProcessPlan.OrderSpecificationTable] + string.Empty;
            //派工-订单规格表
            dispatchObj["F0000055"] = bizObject[ABCDProcessPlan.OrderSpecificationTable] + string.Empty;
            //订单批次规格表
            processObj[RealTimeDynamicProduction.OrderBatchSpecificationTable] = bizObject[ABCDProcessPlan.OrderBatchSpecificationTable] + string.Empty;
            //质量状态
            processObj[RealTimeDynamicProduction.QualityStatus] = "正常";
            //锯切加工单位
            flowObject[ProcessFlow.SawingProcessingUnit] = bizObject[ABCDProcessPlan.ProcessingUnitSawing] + string.Empty;
            //本工序需求期-锯切
            flowObject[ProcessFlow.DemandPeriodOfThisProcessSawing] = bizObject[ABCDProcessPlan.DemandPeriodOfThisProcedureSawing] + string.Empty;

            //锻压加工单位
            flowObject[ProcessFlow.ForgingProcessingUnit] = bizObject[ABCDProcessPlan.ProcessingUnitForging] + string.Empty;
            //本工序需求期-锻压
            flowObject[ProcessFlow.DemandPeriodOfThisProcedureForging] = bizObject[ABCDProcessPlan.DemandPeriodOfThisOperationForging] + string.Empty;

            //辗环加工单位
            flowObject[ProcessFlow.RingRollingProcessingUnit] = bizObject[ABCDProcessPlan.ProcessingUnitRingRolling] + string.Empty;
            //本工序需求期-辗环
            flowObject[ProcessFlow.DemandPeriodOfThisProcedureRingRolling] = bizObject[ABCDProcessPlan.DemandPeriodOfThisOperationRingRolling] + string.Empty;

            //热处理加工单位
            flowObject[ProcessFlow.HeatTreatmentProcessingUnit] = bizObject[ABCDProcessPlan.ProcessingUnitHeatTreatment] + string.Empty;
            //本工序需求期-热处理
            flowObject[ProcessFlow.DemandPeriodOfThisProcedureHeatTreatment] = bizObject[ABCDProcessPlan.DemandPeriodOfThisOperationHeatTreatment] + string.Empty;

            //毛坯加工单位
            flowObject[ProcessFlow.BlankProcessingUnit] = bizObject[ABCDProcessPlan.ProcessingUnitBlank] + string.Empty;
            //本工序需求期-毛坯
            flowObject[ProcessFlow.DemandPeriodOfThisOperationBlank] = bizObject[ABCDProcessPlan.DemandPeriodOfThisOperationBlank] + string.Empty;

            //粗车加工单位
            flowObject[ProcessFlow.RoughTurningUnit] = bizObject[ABCDProcessPlan.ProcessingUnitRoughTurning] + string.Empty;
            //本工序需求期-粗车
            flowObject[ProcessFlow.DemandPeriodOfThisOperationRoughTurning] = bizObject[ABCDProcessPlan.RoughTurningPlanCompletionTime] + string.Empty;
            dispatchObj[Dispatchs.RoughTurningPlanCompletionTime] = bizObject[ABCDProcessPlan.RoughTurningPlanCompletionTime] + string.Empty;

            //精车加工单位
            flowObject[ProcessFlow.FinishTurningUnit] = bizObject[ABCDProcessPlan.ProcessingUnitFinishTurning] + string.Empty;

            //本工序需求期-精车
            flowObject[ProcessFlow.DemandPeriodOfThisProcessFinishTurning] = bizObject[ABCDProcessPlan.FinishTurningPlannedCompletionTime] + string.Empty;
            dispatchObj[Dispatchs.FinishTurningPlanCompletionTime] = bizObject[ABCDProcessPlan.FinishTurningPlannedCompletionTime] + string.Empty;

            //钻孔加工单位
            flowObject[ProcessFlow.DrillingUnit] = bizObject[ABCDProcessPlan.ProcessingUnitDrilling] + string.Empty;
            //本工序需求期-钻孔
            flowObject[ProcessFlow.DemandPeriodOfThisProcessDrilling] = bizObject[ABCDProcessPlan.DrillingPlannedCompletionTime] + string.Empty;
            dispatchObj[Dispatchs.DrillingPlanCompletionTime] = bizObject[ABCDProcessPlan.DrillingPlannedCompletionTime] + string.Empty;

            //工序计划表
            flowObject[ProcessFlow.OperationSchedule] = bizObject[ABCDProcessPlan.ObjectId] + string.Empty;
            processObj[RealTimeDynamicProduction.OperationPlanTable] = bizObject[ABCDProcessPlan.ObjectId] + string.Empty;

            //双轧关联表单
            flowObject[ProcessFlow.DoubleTieAssociatedForm] = bizObject[ABCDProcessPlan.DoubleTieAssociatedForm] + string.Empty;
            //双轧工件号
            flowObject[ProcessFlow.DoubleTieWorkpieceNumber] = bizObject[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty;
            //数量-切割前
            flowObject[ProcessFlow.QuantityBeforeCutting] = bizObject[ABCDProcessPlan.QuantityBeforeCutting] + string.Empty;


            //原料库
            flowObject[ProcessFlow.RawMaterialWarehouse] = bizObject[ABCDProcessPlan.RawMaterialStorage] + string.Empty;
            //原材料类型
            flowObject[ProcessFlow.RawMaterialType] = bizObject[ABCDProcessPlan.RawMaterialType] + string.Empty;
            //原材料编号
            flowObject[ProcessFlow.RawMaterialNumber] = bizObject[ABCDProcessPlan.RawMaterialNumber] + string.Empty;

            // if(string.IsNullOrEmpty(bizObject["shuj"] + string.Empty))
            // {
            //     //流程参与部门
            //     flowObject[ProcessFlow.TheCirculationDepartment] = bizObject[ABCDProcessPlan.ProcessParticipatingDepartments];
            // }
            // else
            // {
            //     //开发测试部门
            //     flowObject[ProcessFlow.TheCirculationDepartment] = bizObject[ABCDProcessPlan.DevelopmentTestDepartment];
            // }

            //完成总量
            flowObject[ProcessFlow.ToCompleteTheTotalAmount] = 0;
            //"派工表"赋值工艺流程数据Id
            dispatchObj["FlowInfo"] = flowObject.ObjectId;
            //派工表赋值无派工加工审批权限人
            dispatchObj["F0000085"] = bizObject["F0000252"]; //总计划员
            dispatchObj["F0000086"] = bizObject["F0000251"]; //冷加工科长
            dispatchObj["F0000087"] = bizObject["F0000247"]; //取样班组长
            dispatchObj["F0000088"] = bizObject["F0000248"]; //粗车班组长
            dispatchObj["F0000089"] = bizObject["F0000249"]; //精车班组长
            dispatchObj["F0000090"] = bizObject["F0000250"]; //钻孔班组长

            //工艺流程表赋值无派工加工审批权限人
            flowObject["F0000194"] = bizObject["F0000252"]; //总计划员
            flowObject["F0000193"] = bizObject["F0000251"]; //冷加工科长
            flowObject["F0000195"] = bizObject["F0000247"]; //取样班组长
            flowObject["F0000196"] = bizObject["F0000248"]; //粗车班组长
            flowObject["F0000197"] = bizObject["F0000249"]; //精车班组长
            flowObject["F0000198"] = bizObject["F0000250"]; //钻孔班组长

            processObj.Status = H3.DataModel.BizObjectStatus.Effective;
            dispatchObj.Status = H3.DataModel.BizObjectStatus.Effective;
            //工艺流程表数据
            flowObject.Create();
            //派工数据
            dispatchObj.Create();
            //"进度管理"赋值工艺流程数据Id
            processObj[RealTimeDynamicProduction.ProcessFlowTable] = flowObject.ObjectId;
            //进度管理数据
            processObj.Create();
            //"工艺流程"赋值派工信息Id
            flowObject[ProcessFlow.DispatchingInformation] = dispatchObj.ObjectId;
            //"工艺流程"赋值进度管理信息
            flowObject["Progress"] = processObj.ObjectId;
            //工艺流程数据
            flowObject.Update();
            //开启流程
            Tools.WorkFlow.StartWorkflow(this.Engine, flowObject, this.Request.UserContext.UserId, true);
            //开启状态
            bizObject[ABCDProcessPlan.OpenProcess] = "已开启";
            //工艺流程表
            bizObject[ABCDProcessPlan.ProcessFlowTable] = flowObject[ProcessFlow.ObjectId];
            //更新本表单

            bizObject.Update();
            response.Message = "开启成功！";
        }
    }
    public void BatchModifyDispath(H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response)
    {
        string[] objectIds = postValue.Data["ObjectIds"] as string[];
        if (objectIds.Length == 0)
        {
            return;
        }
        foreach (string objectId in objectIds)
        {

            return;
        }
    }
}
