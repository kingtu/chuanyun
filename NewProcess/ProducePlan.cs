
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
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
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
        bool hasError=false;
        foreach (string objectId in objectIds)
        {
            H3.DataModel.BizObject objPlan = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            if (objPlan[ABCDProcessPlan.OpenProcess] + string.Empty == "已开启") { continue; }
            if (objPlan["DispatchTable"] + string.Empty == "")
            {
                response.Message = "未生成派工单！";
                hasError = true;
                continue;
            } 
           
            //双轧工件号校验
            if (objPlan[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty == string.Empty)
            {
                response.Message = "注意，双轧工件，{双轧工件号}不能为空!";
                hasError = true;
                continue;
            }
            else
            {
                //双轧关联表单
                string objectIdB = (string)objPlan["F0000226"];
                H3.DataModel.BizObject objPlanB = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectIdB);
                H3.DataModel.BizObject objProcessB = CreateProcessObject(objPlanB);
                H3.DataModel.BizObject objFlowB = CreateFlowObject(objPlanB);

                //"工艺流程"取值 进度管理数据Id
                objFlowB["Progress"] = objProcessB.ObjectId;
                //工艺流程数据
                objFlowB.Update();

                //"进度管理”取值 工艺流程数据Id
                objProcessB[RealTimeDynamicProduction.ProcessFlowTable] = objFlowB.ObjectId;
                objProcessB.Update();

                objPlanB[ABCDProcessPlan.ProcessFlowTable] = objFlowB.ObjectId;
                objPlanB.Update();

            }

            H3.DataModel.BizObject objProcess = CreateProcessObject(objPlan );            
            H3.DataModel.BizObject objFlow = CreateFlowObject(objPlan);

            //"工艺流程"取值 进度管理数据Id
            objFlow["Progress"] = objProcess.ObjectId;            
            objFlow.Update();

            //"进度管理”取值 工艺流程数据Id
            objProcess[RealTimeDynamicProduction.ProcessFlowTable] = objFlow.ObjectId;
            objProcess.Update();

            //开启流程
            Tools.WorkFlow.StartWorkflow(this.Engine, objFlow, this.Request.UserContext.UserId, true);

            //开启状态
            objPlan[ABCDProcessPlan.OpenProcess] = "已开启";
            //"工序计划"取值 "工艺流程"数据Id
            objPlan[ABCDProcessPlan.ProcessFlowTable] = objFlow.ObjectId;
            //更新本表单
            objPlan.Update();
        }

        if ( !hasError) { response.Message = "开启成功！"; }
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
            H3.DataModel.BizObject objPlan = Tools.BizOperation.Load(this.Engine, this.Request.SchemaCode, objectId);
            if (objPlan["DispatchTable"] + string.Empty != "") { continue; }
            H3.DataModel.BizObject dispatchObj = CreateDispatchObject(objPlan, objectId);
            objPlan["DispatchTable"] = dispatchObj.ObjectId;
            objPlan.Update();
            response.Message = "生成派工单成功！";
        }
    }
   
    /// <summary>
    /// 创建“派工表”对象
    /// </summary>
    /// <param name="objPlan"></param>
    /// <param name="objectId"></param>
    /// <returns></returns>
    private H3.DataModel.BizObject CreateDispatchObject(H3.DataModel.BizObject objPlan, string objectId)
    {
        H3.DataModel.BizObject dispatchObj = Tools.BizOperation.New(this.Engine, Dispatchs.TableCode);
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

        
        string z = (string)objPlan["F0000252"]; //总计划员
        string l = (string)objPlan["F0000251"]; //冷加工科长
        string qy = (string)objPlan["F0000247"]; //取样班组长
        string cc = (string)objPlan["F0000248"]; //粗车班组长
        string jc = (string)objPlan["F0000249"]; //精车班组长
        string zk = (string)objPlan["F0000250"]; //钻孔班组长

        //派工表取值无派工加工审批权限人
        dispatchObj["F0000085"] = z;
        dispatchObj["F0000086"] = l;
        dispatchObj["F0000087"] = qy;
        dispatchObj["F0000088"] = cc;
        dispatchObj["F0000089"] = jc;
        dispatchObj["F0000090"] = zk;


        //派工取样子表
        BizObject bs = Tools.BizOperation.New(this.Engine, DispatchSamplingSubTable.TableCode);
        bs[DispatchSamplingSubTable.TaskName] = "取样任务";
        bs[DispatchSamplingSubTable.Name] = new object[1] { qy != "" ? qy : l != "" ? l : z };
        bs[DispatchSamplingSubTable.ProcessingQuantity] = 1.0;
        dispatchObj["F0000098"] = 1.0;
        dispatchObj[DispatchSamplingSubTable.TableCode] = new BizObject[1] { bs };


        //派工粗车子表
        BizObject br = Tools.BizOperation.New(this.Engine, DispatchRoughSubTable.TableCode);
        br[DispatchRoughSubTable.TaskName] = "粗车任务";
        br[DispatchRoughSubTable.Name] = new object[1] { cc != "" ? cc : l != "" ? l : z };
        br[DispatchRoughSubTable.ProcessingQuantity] = 1.0;
        dispatchObj["F0000099"] = 1.0;
        dispatchObj[DispatchRoughSubTable.TableCode] = new BizObject[1] { br };

        //派工精车子表
        BizObject bf = Tools.BizOperation.New(this.Engine, DispatchFinishSubTable.TableCode);
        bf[DispatchFinishSubTable.TaskName] = "精车任务";
        bf[DispatchFinishSubTable.Name] = new object[1] { jc != "" ? jc : l != "" ? l : z };
        bf[DispatchFinishSubTable.ProcessingQuantity] = 1.0;
        dispatchObj["F0000100"] = 1.0;
        dispatchObj[DispatchFinishSubTable.TableCode] = new BizObject[1] { bf };

        //派工钻孔子表
        BizObject bd = Tools.BizOperation.New(this.Engine, DispatchDrillSubTable.TableCode);
        bd[DispatchDrillSubTable.TaskName] = "钻孔任务";
        bd[DispatchDrillSubTable.Name] = new object[1] { zk != "" ? zk : l != "" ? l : z };
        bd[DispatchDrillSubTable.ProcessingQuantity] = 1.0;
        dispatchObj["F0000101"] = 1.0;
        dispatchObj[DispatchDrillSubTable.TableCode] = new BizObject[1] { bd };

        //“派工表”取值“工序计划表”的数据ID
        dispatchObj["PlanTable"] = objectId;
        dispatchObj.Status = H3.DataModel.BizObjectStatus.Effective;
        //派工数据
        dispatchObj.Create();
        return dispatchObj;

    }
    /// <summary>
    /// 创建“工艺流程”对象
    /// </summary>
    /// <param name="objPlan"></param>
    /// <param name="ProductType"></param>
    /// <returns></returns>
    private H3.DataModel.BizObject CreateFlowObject(H3.DataModel.BizObject objPlan)
    {
        H3.DataModel.BizObject objFlow = Tools.BizOperation.New(this.Engine, ProcessFlow.TableCode);

        if (objPlan[ABCDProcessPlan.PlannedRollingMethod] + string.Empty == "双轧")
        {
            //工件号转数值
            int workpieceNumber = Convert.ToInt32(objPlan[ABCDProcessPlan.WorkpieceNumber] + string.Empty);
            //双轧工件号转数值
            int doubleTieWorkpieceNumber = Convert.ToInt32(objPlan[ABCDProcessPlan.DoubleTieWorkpieceNumber] + string.Empty);

            //工件号大于双轧工件号
            if (workpieceNumber > doubleTieWorkpieceNumber)
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
        objFlow[ProcessFlow.ProductType] =objPlan["ProductType"];

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
        //计划炉次编号
        objFlow[ProcessFlow.PlannedHeatNumber] = objPlan[ABCDProcessPlan.PlannedFurnaceNumber] + string.Empty;
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
    /// <param name="objPlan"></param>
    /// <returns></returns>
    private H3.DataModel.BizObject CreateProcessObject(H3.DataModel.BizObject objPlan)
    {
         H3.DataModel.BizObject objProcess = Tools.BizOperation.New(this.Engine, RealTimeDynamicProduction.TableCode);
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

        objProcess.Status = H3.DataModel.BizObjectStatus.Effective;

        //进度管理数据
        objProcess.Create();
        return objProcess;
    }

}
