using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using H3;
using H3.DataModel;

public class D001419Siizvpn3x17wj6jj3pifsmbic3 : H3.SmartForm.SmartFormController
{
    string activityCode;        //当前节点
    string userName = "";       //当前用户
    BizObject me;  //本表单数据
    string info = string.Empty; //值班信息
    Dictionary<string, bool> boolConfig;//布尔值字典
    H3.Workflow.Instance.WorkflowInstance instance;//本表单流程实例
    H3.SmartForm.SmartFormResponseDataItem message;//用户提示信息
    public D001419Siizvpn3x17wj6jj3pifsmbic3(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        activityCode = Request.ActivityCode;//当前节点
        me = Request.BizObject;             //本表单数据
        //获取本流程的实例
        instance = Engine.WorkflowInstanceManager.GetWorkflowInstance(me.WorkflowInstanceId);
        //转换工艺配置为布尔值
        boolConfig = new Dictionary<string, bool>();
        boolConfig.Add("是", true);
        boolConfig.Add("否", false);
        message = new H3.SmartForm.SmartFormResponseDataItem(); //用户提示信息
        userName = Request.UserContext.User.FullName;      //当前用户
    }
    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!Request.IsCreateMode && instance.IsUnfinished)  //当前流程未结束
            {

                ClearTheTransitionStepsAndSectionOfTheParentProcess();//清空父流程的转至工步与转至工序
                ClearTheGoToWorkStepInformation(); //清空转至工步信息
                BizObject planObject = LoadingConfig.GetPlanningData(Engine, instance);//根据当前工件流程追溯工序计划数据
                LoadQualityConfiguration(planObject, boolConfig);//加载质量配置数据
                InitializeTheFormControlInformation();//初始化表单控件信息
                Hashtable workSteps = ProgressManagement.HeatTreatmentProgress(Engine, TableCode, CurrentWorkStep);//同步数据至实时制造情况
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[CurrentWorkStep] = workSteps[me.ObjectId];  //mark：z注释
                }
            }

        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        response.ReturnData.Add("message", message);
        base.OnLoad(response);
        //--------------------------加载前后分割线-------------------------//
        //try
        //{
        //    if (!Request.IsCreateMode)
        //    {
        //        //加载后代码
        //    }
        //}
        //catch (Exception ex)
        //{
        //    info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
        //    message.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        //}
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            if (actionName == "Submit")
            {
                //校验异常信息是否与数据库保持一致               
                if (ExceptionIsChanged()) { response.Message = "异常数据有更新，请刷新页面！"; return; }
                Authority.Approver(Request);
                BizObject planObject = LoadingConfig.GetPlanningData(Engine, instance);//根据当前工件流程追溯工序计划数据
                //加载工艺配置数据
                LoadingProcessConfiguration(planObject, boolConfig);
                //赋值审批来源
                AssignmentApprovalSource();
                base.OnSubmit(actionName, postValue, response);
                //更新炉次实际信息
                UpdateTheActualFurnaceInformation();
                //异常工步
                AbnormalWorkingStep();

            }
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment()
    {
        if (activityCode == "Activity127")
        {
            //获取当前流程业务对象
            BizObject current = BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                               Request.SchemaCode, Request.BizObjectId, false);
            current[InitiateAbnormal] = "否";        //发起异常
            current[TargetStep] = null;              //转至工步
            current[AbnormalCategory] = null;        //异常类别
            current[AbnormalDescription] = null;     //异常描述
            current[AbnormalRepresentative] = null;  //异常代表
            current[processAdjustmentRange] = "否";  //是否调整至其他工序
            current[AssociatedWithOtherAbnormalWorkpieces] = null;//关联其它异常工件
            current[QualityApprovalList] = null;       //质量审批单
            current[DemandApprovalForm] = null;        //需求审批单
            current[CirculationApprovalSheet] = null;  //流转审批单
            current[OtherApprovalDocuments] = null;    //其它审批单
            current[SourceOfApproval] = null;          //审批来源
            current.Update();
        }
    }

    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void AssignmentApprovalSource()
    {
        //发起异常  是
        if (me[InitiateAbnormal] + string.Empty == "是" && activityCode != "Activity127")
        {
            string abnormal = "发起异常";
            string sourceOfApproval = Request.UserContext.User.Name + 
                                      "在" + me[CurrentSection] + 
                                      "工序的" + me[CurrentWorkStep] + "工步" + abnormal;
            me[SourceOfApproval] = sourceOfApproval;  //审批来源
            me.Update();
        }
    }

    /**
    * --Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTheTransitionStepsAndSectionOfTheParentProcess()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance =
            Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(
                Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        BizObject current = BizObject.Load(H3.Organization.User.SystemUserId,
                                           Engine, instance.SchemaCode, instance.BizObjectId, false);
        current[ProcessFlow_TargetSection] = null; //转至工序
        current[ProcessFlow_TargetStep] = null;    //转至工步
        current.Update();
    }

    /**
    * --Author: zzx
    * 检查发起异常控件是否被其它异常代表更改
    */
    protected bool ExceptionIsChanged()
    {
        //表单中发起异常
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty;
        if (AnExceptionWasRaisedInTheForm == "是")
        {
            return false;
        }
        //获取当前流程业务对象
        BizObject thisObj = BizObject.Load(H3.Organization.User.SystemUserId,
                                           Engine, Request.SchemaCode, Request.BizObjectId, false);
        //数据库中发起异常的值
        string AnExceptionIsGeneratedInTheDatabase = thisObj[InitiateAbnormal] + string.Empty;
        return AnExceptionWasRaisedInTheForm != AnExceptionIsGeneratedInTheDatabase;

    }

    /**
    * --Author: zzx
    * 初始化表单控件信息
    */
    public void InitializeTheFormControlInformation()
    {
        me[CurrentSection] = "热处理";//当前工序      
        me.Update();
    }
    /*
    * --Author: zzx
    * 清空转至工步信息
    */
    public void ClearTheGoToWorkStepInformation()
    {
        //正常节点 转至工步清空
        if (activityCode != "Activity127")
        {
            me[TargetStep] = null;//转至工步
        }
    }

    /**
    * --Author: zzx
    * 更新炉次信息
    */
    protected void UpdateTheActualFurnaceInformation()
    {
        //炉次编号
        var strFurnaceTimeNumber = Request.BizObject[ConfirmedHeatNumber].ToString();
        var id = Request.BizObject[ID].ToString(); //ID
        //热处理表中查询相同炉次编号和相同热处理炉号的数据
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, ABCDProcessPlan_ID, H3.Data.ComparisonOperatorType.Equal, id); //ID编号
        BizObject[] bizObjects = Tools.BizOperation.GetList(Request.Engine,
                                                            ABCDProcessPlan_TableCode, filter);
        if (bizObjects != null)
        {
            foreach (BizObject bizObject in bizObjects)
            {
                BizObject currentBizObject = Tools.BizOperation.Load(Request.Engine,
                                                                     ABCDProcessPlan_TableCode,
                                                                     bizObject.ObjectId); //加载符合条件数据

                currentBizObject[ABCDProcessPlan_ActualNumberOfFurnace] = strFurnaceTimeNumber;   //赋值实际炉次编号              
                currentBizObject.Update();
            }
        }
    }

    /**
    * --Author: zzx
    * 关于发起异常之后各个节点进行的操作  异常工步
    */
    protected void AbnormalWorkingStep()
    {
        //表单中发起异常
        string AnExceptionWasRaisedInTheForm = me[InitiateAbnormal] + string.Empty;
        if (AnExceptionWasRaisedInTheForm != "是") { return; }
        //关联其它异常工件
        string[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            BizObject otherIdObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                  ScheduleManagement_TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = otherIdObj[ScheduleManagement_OperationTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = otherIdObj[ScheduleManagement_CurrentPreviousOperationTableSchemacode] + string.Empty;
            //加载当前工序表中的业务对象
            BizObject sectionObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine,
                                                  currentSchemaCode, otherExceptionId, false);
            //传递异常信息
            foreach (PropertySchema activex in sectionObj.Schema.Properties)
            {
                if (activex.DisplayName.Contains("发起异常"))
                {
                    sectionObj[activex.Name] = "是";
                }

                if (activex.DisplayName.Contains("异常类别"))
                {
                    sectionObj[activex.Name] = me[AbnormalCategory] + string.Empty;
                }

                if (activex.DisplayName.Contains("异常代表"))
                {
                    sectionObj[activex.Name] = me[ID];
                }
            }
            sectionObj.Update();
        }

        if (activityCode != "Activity127") //工步节点
        {
            Request.BizObject[Owner] = Request.UserContext.UserId; //设置异常权限
        }
        //确认调整后转至工步清空和发起异常赋值“否”
        AfterConfirmingTheAdjustmentGoToWorkStepClearingAndInitiateExceptionAssignment();
    }

    /*
    *--Author:fubin
    * 加载质量配置数据     LoadQualityConfiguration
    * @param planObj 工序计划数据
    * @param boolConfig 布尔值字典
    */
    protected void LoadQualityConfiguration(BizObject planObj, Dictionary<string, bool> boolConfig)
    {
        //读取装炉前检验在《质量配置表》中的优先层级
        string heatTreatment = LoadingConfig.GetQualityConfigForm(
            Engine, QAConfig.PriorityLevelInspectionBeforeCharging);
        //读取《质量配置表》装炉前检验配置
        string globalHeatTreatment = LoadingConfig.GetQualityConfigForm(
            Engine, QAConfig.GlobalInspectionBeforeCharging);
        //读取《工序计划表》装炉前检验配置
        string planHeatTreatment = planObj != null ?
                                   planObj[ABCDProcessPlan_InspectionBeforeSinglePieceFurnaceLoading] + string.Empty :
                                   string.Empty;
        //优先层级
        switch (heatTreatment)
        {
            case "配置表":
                me[CheckBeforeLoading] = boolConfig[globalHeatTreatment];  //全局装炉前检验
                break;
            case "计划表":
                if (planHeatTreatment != string.Empty)
                {   //计划装炉前检验
                    me[CheckBeforeLoading] = boolConfig[planHeatTreatment] + string.Empty;
                }
                else
                {   //全局装炉前检验
                    me[CheckBeforeLoading] = boolConfig[globalHeatTreatment] + string.Empty;
                }
                break;
        }
        //装炉前检验
        if ((bool)me[CheckBeforeLoading] == false)
        {
            me[InspectionBeforeCharging] = "合格"; //热处理质量检验结果
        }
    }

    /*
    *--Author:fubin
    * 加载工艺配置数据
    * @param planObj 工序计划数据
    * @param boolConfig 布尔值字典
    */
    protected void LoadingProcessConfiguration(BizObject planObj, Dictionary<string, bool> boolConfig)
    {
        //获取工艺配置精整优先级层级
        string finishing = LoadingConfig.GetWorkmanshipForm(Engine, ProcessConfig.PriorityLevelFinishing);
        //读取《配置表》精整配置
        string globalFinishing = LoadingConfig.GetWorkmanshipForm(Engine, ProcessConfig.GlobalFinishingConfiguration);
        //加载对应订单规格表的数据
        BizObject productObj = LoadingConfig.GetProductData(Engine, planObj);
        //读取《产品表》精整配置
        string productFinishing = productObj != null ?
                                  productObj[OrderSpecification_ProductFinishingConfiguration] + string.Empty :
                                  string.Empty;
        //读取《计划表》精整设置
        string planFinishing = planObj != null ?
                               planObj[ABCDProcessPlan_SinglePieceFinishingConfiguration] + string.Empty :
                               string.Empty;
        //获取工艺配置回火优先层级
        string tempering = LoadingConfig.GetWorkmanshipForm(Engine, ProcessConfig.PriorityLevelTempering);
        //读取《配置表》回火配置
        string globalTempering = LoadingConfig.GetWorkmanshipForm(Engine, ProcessConfig.GlobalTemperingConfiguration);
        //读取《计划表》回火设置
        string planTempering = planObj != null ?
                               planObj[ABCDProcessPlan_SinglePieceTemperingConfiguration] + string.Empty :
                               string.Empty;
        //精整优先级层级
        switch (finishing)
        {
            case "配置表":
                //全局精整配置
                me[Refinement] = boolConfig[globalFinishing] + string.Empty;
                break;
            case "产品表":
                if (productFinishing != string.Empty)
                {
                    me[Refinement] = boolConfig[productFinishing] + string.Empty;//产品精整配置
                }
                else
                {
                    me[Refinement] = boolConfig[globalFinishing] + string.Empty;//全局精整配置
                }
                break;
            case "计划表":
                if (planFinishing != string.Empty)
                {
                    me[Refinement] = boolConfig[planFinishing] + string.Empty;//计划精整配置
                }
                else
                {
                    if (productFinishing != string.Empty)
                    {
                        me[Refinement] = boolConfig[productFinishing] + string.Empty; //产品精整配置
                    }
                    else
                    {
                        me[Refinement] = boolConfig[globalFinishing] + string.Empty;//全局精整配置                        
                    }
                }
                break;
        }
        //回火优先层级
        switch (tempering)
        {
            case "配置表":
                me[Tempering] = boolConfig[globalTempering] + string.Empty; //全局回火配置
                break;
            case "计划表":
                if (planTempering != string.Empty)
                {
                    me[Tempering] = boolConfig[planTempering] + string.Empty;  //计划回火配置
                }
                else
                {
                    me[Tempering] = boolConfig[globalTempering] + string.Empty; //全局回火配置
                }
                break;
        }
        //读取《工序计划表》计划本取
        me[NoumenonSampling] = planObj != null ?
                               planObj[ABCDProcessPlan_NoumenonSampling] + string.Empty :
                               string.Empty;
    }

    //热处理
    string TableCode = "D001419Siizvpn3x17wj6jj3pifsmbic3"; //表ID
    string ID = "F0000038";                 //ID
    string Objectid = "ObjectId";           //热处理ObjectId
    string Owner = "OwnerId";               //拥有者
    string CurrentSection = "F0000050";     //当前工序
    string CurrentWorkStep = "F0000048";    //当前工步
    string TargetStep = "F0000039";         //转至工步
    string ConfirmedHeatNumber = "F0000068";//确认炉次编号    
    string CheckBeforeLoading = "F0000074";      //装炉前检验
    string InspectionBeforeCharging = "F0000072";//热处理质量检验结果
    string Refinement = "F0000073";              //(是/否)精整
    string Tempering = "F0000087";               //(是/否)回火
    string NoumenonSampling = "F0000056";        //计划本取

    string InitiateAbnormal = "F0000040";         //发起异常
    string AbnormalCategory = "F0000049";         //异常类别
    string AbnormalDescription = "F0000058";      //异常描述
    string AbnormalRepresentative = "F0000084";   //异常代表
    string processAdjustmentRange = "F0000045";   //是否跨工序调整流程
    string QualityApprovalList = "F0000200";      //质量审批单
    string DemandApprovalForm = "F0000201";       //需求审批单
    string CirculationApprovalSheet = "F0000202"; //流转审批单
    string OtherApprovalDocuments = "F0000203";   //其它审批单
    string SourceOfApproval = "F0000204";         //审批来源
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199"; //关联其它异常工件

    //工艺流程表
    string ProcessFlow_TargetSection = "F0000056"; //转至工序
    string ProcessFlow_TargetStep = "F0000057";    //转至工步

    //工序计划表
    string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";         //表ID
    string ABCDProcessPlan_ID = "F0000007";                     //ID
    string ABCDProcessPlan_NoumenonSampling = "F0000141";       //计划本取
    string ABCDProcessPlan_ActualNumberOfFurnace = "F0000217";  //实际炉次编号
    string ABCDProcessPlan_SinglePieceFinishingConfiguration = "F0000146";          //单件精整配置
    string ABCDProcessPlan_SinglePieceTemperingConfiguration = "F0000216";          //单件回火配置
    string ABCDProcessPlan_InspectionBeforeSinglePieceFurnaceLoading = "F0000148";  //单件装炉前检验

    //进度管理
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd"; //表ID
    string ScheduleManagement_OperationTableDataID = "F0000070";                     //工序表数据ID
    string ScheduleManagement_CurrentPreviousOperationTableSchemacode = "F0000071";  //工序表SchemaCode

    //A-C订单规格表    
    string OrderSpecification_ProductFinishingConfiguration = "F0000130"; //产品精整配置
}

//internal class QAConfig
//{
//    public static string PriorityLevelInspectionBeforeCharging { get; internal set; }
//    public static string GlobalInspectionBeforeCharging { get; internal set; }
//}