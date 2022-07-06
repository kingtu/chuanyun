
using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using H3.DataModel;
using H3;

public class D001419Sgx7flbvwu9r0u3hail6512uq4 : H3.SmartForm.SmartFormController
{
    BizObject me;//本表单数据  
    string activityCode;//当前节点   
    H3.Workflow.Instance.WorkflowInstance instance = null;//实例对象
    BizObject parentObj = null;//业务对象
    H3.SmartForm.SmartFormResponseDataItem item;  //用户提示信息   
    string userName = ""; //当前用户
    public D001419Sgx7flbvwu9r0u3hail6512uq4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        activityCode = Request.ActivityCode;//当前节点
        instance = Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);//父流程实例对象
        parentObj = Tools.BizOperation.Load(Request.Engine, ProcessFlow_TableCode, instance.BizObjectId);//父流程业务对象
        me = Request.BizObject;//本表单业务对象
        item = new H3.SmartForm.SmartFormResponseDataItem();//用户提示信息
        userName = Request.UserContext.User.FullName;//当前用户
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {           
            if (!Request.IsCreateMode) //不是创建模式
            {               
                ClearTargetSectionStep(); //清空父流程的转至工步与转至工序                
                ClearTargetStep();//清空转至工步信息               
                InitTableComponent(); //初始化控件               
                ProductCategoryUpdate(); //产品类别更新                
                BizObject planObj = LoadingConfig.GetPlanningData(Engine, Request.WorkflowInstance);//获取工序计划表数据
                //流程处于未完成状态
                if (Request.WorkflowInstance.IsUnfinished)
                {
                    //读取《工序计划表》计划本取
                    me[PlanNoumenonSampling] = planObj != null ? planObj[ABCDProcessPlan_PlanNoumenonSampling] + string.Empty : string.Empty;
                }
                //同步数据至实时制造情况
                Hashtable workSteps = ProgressManagement.RoughCastProgress(Engine, TableCode, CurrentWorkStep);
                //进度管理ID不为空
                if (workSteps[me.ObjectId] + string.Empty != string.Empty)
                {
                    me[CurrentWorkStep] = workSteps[me.ObjectId];
                }
            }
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);//错误信息反馈
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        response.ReturnData.Add("key1", item);
        base.OnLoad(response);
        //try
        //{
        //    //不是创建模式
        //    if (!Request.IsCreateMode)
        //    {
        //        //加载后代码
        //    }
        //}
        //catch (Exception ex)
        //{
        //    //错误信息反馈
        //    info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);
        //    item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        //}
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {   if (actionName == "Submit")
            {
                //校验异常信息是否与数据库保持一致                
                if (AbnormalIsChanged()) { response.Message = "异常数据有更新，请刷新页面！"; return; }
                Authority.Approver(Request);              
                string dipatchFlag = Request.BizObject[DispatchSwitch] + string.Empty;  //派工开关               
                if (dipatchFlag == "开")
                {  
                    DispatchLogic.DispatchLogicSamp(Engine, me, activityCode);       //毛坯取样工序的派工信息赋值
                    DispatchLogic.DispatchLogicRoughCasts(Engine, me, activityCode); //毛坯的审批确认节点的取样派工信息赋值                   
                    DispatchLogic.DispatchLogicRough(Engine, me, activityCode);      //毛坯粗车工序的派工信息赋值
                }                
                BizObject planObj = LoadingConfig.GetPlanningData(Engine, Request.WorkflowInstance);//获取工序计划表数据               
                LoadingQAConfiguration(planObj); //加载质量配置数据               
                UnqualifiedSource(); //赋值审批来源               
                WhetherTransshipment(); //计算下一工序是否转运
                base.OnSubmit(actionName, postValue, response);              
                AbnormalStep();  //异常工步                
                if (activityCode == "Activity36")//待切割
                {
                    string strRollingMode = Request.BizObject[RollingMode].ToString();//轧制方式
                    if (string.Compare(strRollingMode, "双轧") == 0) //双轧同步校验
                    {
                        DoubleRollingCheck(Request.BizObject, Request.Engine, TableCode, response); //双轧校验
                    }
                }
            }
        }
        catch (Exception ex)
        {		
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode, userName);//负责人信息
            response.Message =string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: nkx
    * 确认调整后转至工步清空和发起异常赋值“否”
    */
    protected void DeleteTransferToWorkStep()
    {
        if (activityCode == "Activity100")
        {
            //获取当前流程业务对象
            BizObject currentFlowObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
            currentFlowObj[InitiateAbnormal] = "否";                             //发起异常
            currentFlowObj[TargetStep] = null;                                   //转至工步
            currentFlowObj[AbnormalCategory] = null;                             //异常类别
            currentFlowObj[AbnormalDescription] = null;                          //异常描述
            currentFlowObj[AbnormalRepresentative] = null;                       //异常代表
            currentFlowObj[processAdjustmentRange] = "否";                       //是否调整至其他工序
            currentFlowObj[AssociatedWithOtherAbnormalWorkpieces] = null;        //关联其它异常工件
            currentFlowObj[MachineQualityAssurance] = null;                      //机加质量处理
            currentFlowObj[DemandApprovalForm] = null;                           //需求审批单
            currentFlowObj[CirculationApprovalSheet] = null;                     //流转审批单
            currentFlowObj[OtherApprovalDocuments] = null;                       //其它审批单
            currentFlowObj[SourceOfApproval] = null;                             //审批来源
            currentFlowObj.Update();                                             //更新业务对象
        }
    }

    /*
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = Request.UserContext.User.Name;                             //当前审批人
        string currentProcess = me[CurrentSection] + string.Empty;                          //当前工序
        string currentWorkStep = me[CurrentWorkStep] + string.Empty;                        //当前工步
        string results  ;
        if (me[InitiateAbnormal] + string.Empty == "是" && activityCode != "Activity100")   //发起异常
        {
            results = "发起异常";            
            me[SourceOfApproval] = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results;                                            //审批来源
            me.Update();                                                                        //更新业务对象
        }
        if ( me[InitiateAbnormal] + string.Empty == "否" &&  me[InspectionResults] + string.Empty == "不合格" && activityCode == "Activity61" )              //检验结果
        {
            results = "自检结果不合格";           
            me[SourceOfApproval] = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results; ;                                            //审批来源
            me.Update();                                                                        //更新业务对象
        }
        if (me[InitiateAbnormal] + string.Empty == "否" && me[PhysicochemicalResults] + string.Empty == "不合格" && activityCode == "Activity262" )        //理化结果
        {
            results = "理化结果不合格";            
            me[SourceOfApproval] = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + results;                                          //审批来源
            me.Update();                                                                        //更新业务对象
        }
    }

    /*
    * --Author: nkx
    * 计算下一工序是否转运
    */
    protected void WhetherTransshipment()
    {
        //节点名称是否为 待尺寸检验或者确认异常后去向
        if (activityCode == "Activity61" || activityCode == "Activity100")
        {
            bool flags = true;
            if (activityCode == "Activity100")
            {
                string transferToWorkStep = me[TargetStep] + string.Empty;//转至工步               
                if (transferToWorkStep.Contains("取样")) //包含“取样”
                {
                    me[WhetherTheSampleIsTransported] = "是";//取样是否需要转运
                    me.Update();
                }
                else
                {
                    flags = false;
                }
            }
            //物流开关
            if (me[LogisticsSwitch] + string.Empty == "开")
            {
                if (flags)
                {
                    //工序表取样派工子表信息
                    BizObject[] disProcessSubs = me[RoughCastSamplingDispatchingInformation_TableCode] as BizObject[];
                    //判断工序表取样派工子表有没有值
                    if (disProcessSubs != null && disProcessSubs.Length > 0)
                    {
                        //循环工序表取样派工子表信息
                        for (int i = 0; i < disProcessSubs.Length; i++)
                        {
                            //任务状态等于“已完成”
                            if (disProcessSubs[i][RoughCastSamplingDispatchingInformation_TaskStatus] + string.Empty != "已完成")
                            {
                                //判断子表信息的车间位置与现在工件所在的车间位置是否一致
                                if (disProcessSubs[i][RoughCastSamplingDispatchingInformation_SamplingWorkshopName] + string.Empty != me[WorkshopLocation] + string.Empty)
                                {
                                    flags = false;
                                    break;
                                }
                            }
                        }
                        if (flags)
                        {
                            me[WhetherTheSampleIsTransported] = "否";      //取样是否需要转运
                        }
                    }
                }
            }
            me.Update();
        }
        //节点名称是否为 待理化结果或忽略理化结果
        if (activityCode == "Activity214" || activityCode == "Activity262")
        {
            bool flags = true;
            if (me[LogisticsSwitch] + string.Empty == "开")  //物流开关
            {
                //工序表粗车派工子表信息
                BizObject[] disProcessSubs = me[RoughCastRoughCuttingDispatchingInformation_TableCode] as BizObject[];
                //判断工序表粗车派工子表有没有值
                if (disProcessSubs != null && disProcessSubs.Length > 0)
                {
                    //循环工序表粗车派工子表信息
                    for (int i = 0; i < disProcessSubs.Length; i++)
                    {
                        //任务状态等于已完成
                        if (disProcessSubs[i][RoughCastRoughCuttingDispatchingInformation_TaskStatus] + string.Empty != "已完成")
                        {
                            //判断子表信息的车间位置与现在工件所在的车间位置是否一致，不一致则给转至工步控件赋值“待四面光”（跳过待转运）
                            if (disProcessSubs[i][RoughCastRoughCuttingDispatchingInformation_RoughTurningWorkshopName] + string.Empty != me[WorkshopLocation] + string.Empty)
                            {
                                flags = false;
                                break;
                            }
                        }
                    }
                    if (flags)
                    {
                        me[WhetherTheRoughIsTransported] = "否";              //粗车是否需要转运
                    }
                }
                me.Update();
            }
           
            if (me[LogisticsSwitch] + string.Empty == "关") //物流开关
            {
                if (me[ConfirmNoumenonSampling] + string.Empty == "是") //确认本取
                {
                    me[WhetherTheRoughIsTransported] = "否";             //粗车是否需要转运
                }
                else  
                {
                    me[WhetherTheRoughIsTransported] = "是";              //粗车是否需要转运
                }
            }
            me.Update();
        }
    }

    /*
    * Author: nkx
    * 清空父流程的转至工步与转至工序
    */
    protected void ClearTargetSectionStep()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance parentInstance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        BizObject parentInstanceObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, parentInstance.SchemaCode, parentInstance.BizObjectId, false);
        parentInstanceObj[ProcessFlow_TargetSection] = null;                //转至工序
        parentInstanceObj[ProcessFlow_TargetStep] = null;                   //转至工步
        parentInstanceObj.Update();
    }

    /**
    * Author: zzx
    * 初始化控件
    */
    public void InitTableComponent()
    {
        //初始化当前工序
        if (me[CurrentSection] + string.Empty == string.Empty) { me[CurrentSection] = "毛坯"; }        
        me.Update();//更新本表单
        //赋值-工艺流程表-双轧切割状态
        if (parentObj[ProcessFlow_DoubleRollingCutting] + string.Empty == string.Empty && me[RollingMode] + string.Empty == "双轧")
        {
            parentObj[ProcessFlow_DoubleRollingCutting] = "未切割";
            parentObj.Update();
        }
    }

    /*
    * --Author: zzx
    * 清空转至工步信息。
    * 
    */
    public void ClearTargetStep()
    {             //正常节点 转至工步复位
        if (activityCode != "Activity100") { me[TargetStep] = null; } 
    }

    /*
    * Author: zzx
    * 双轧校验
    */
    public void DoubleRollingCheck(BizObject currentBizobject, H3.IEngine engine, string tableCode, H3.SmartForm.SubmitSmartFormResponse response)
    {
        var flag = true;//是否弹框
        //双轧关联表单   双轧另一件产品在工序计划表中的objectId
        string strABCDProcessPlanObjectId = currentBizobject[DoubleRollingAssociatedForm] + string.Empty;
        //加载工序计划表数据
        BizObject currentObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, ABCDProcessPlan_TableCode, strABCDProcessPlanObjectId, false);
        if (currentObj == null)
        {            
            response.Errors.Add("双轧关联表单为空");//弹出报错窗口
            return;
        }
        //工序计划表中的  关联工艺流程表中的objectId
        string strProcessFlowTableId = currentObj[ABCDProcessPlan_ProcessFlowTable] + string.Empty;
        string strID = currentObj[ABCDProcessPlan_ID] + string.Empty;  //双轧另一件ID
        //加载工艺流程表中的业务对象
        BizObject currentProcessFlowObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, ProcessFlow_TableCode, strProcessFlowTableId, false);
        if (currentProcessFlowObj == null)
        {
            response.Errors.Add("工序计划表中的工艺流程表为空");//弹出报错窗口
            return;
        }
       
        string strObjectId = currentProcessFlowObj[ProcessFlow_ObjectId] + string.Empty; //获取objectID
        //查询流程工作项表InstanceId
        string SelSql = "select * from H_WorkItem  where BizObjectId = '" + strObjectId + "'";
        DataTable SelDt = engine.Query.QueryTable(SelSql, null);
        if (SelDt != null && SelDt.Rows.Count > 0)
        {
            string instanceId =(string) SelDt.Rows[0]["InstanceId"];
            if (!string.IsNullOrEmpty(instanceId))
            {
                //查询流程步骤表
                string sqlStep = "select * from H_Token  where ParentObjectId = '" + instanceId + "' ";
                DataTable sqlStepDt = engine.Query.QueryTable(sqlStep, null);
                if (sqlStepDt != null && sqlStepDt.Rows.Count > 0)
                {
                    //循环数据行 是否经过毛坯节点
                    foreach (DataRow itemStep in sqlStepDt.Rows)
                    {
                        string currentActivityCode = itemStep["Activity"] + string.Empty;
                        //
                        if (currentActivityCode == "Activity23")
                        {
                            flag = false;
                            break;
                        }
                    }
                }
            }
        }
        //双轧的另一件产品没有到工艺流程表中的等待节点
        if (flag == true)
        {
            response.Errors.Add("请把" + strID + "这件产品提交到毛坯工序再进行提交");//弹出报错窗口
        }
    }

    /**
    * Author: zzx
    * 发起异常控件是否被其它异常代表更改
    */
    protected bool AbnormalIsChanged()
    {
        string strInitiateAbnormal = me[InitiateAbnormal] + string.Empty;//表单中发起异常
        if (strInitiateAbnormal == "是") return false;
        BizObject thisObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, Request.SchemaCode, Request.BizObjectId, false);
        string sqlInitiateException = thisObj[InitiateAbnormal] + string.Empty; //数据库中发起异常的值
        return strInitiateAbnormal != sqlInitiateException;       
    }

    /**
    * Author: zzx
    * 关于发起异常之后各个节点进行的操作
    */
    protected void AbnormalStep()
    {
        string strInitiateAbnormal = me[InitiateAbnormal] + string.Empty; //发起异常
        if (strInitiateAbnormal != "是") { return; }
        //关联其它异常工件
        string[] bizObjectIDArray = me[AssociatedWithOtherAbnormalWorkpieces] as string[];
        //遍历其他ID
        foreach (string bizObjectID in bizObjectIDArray)
        {
            //加载其他异常ID 的业务对象
            BizObject currentObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, RealTimeDynamicProduction.TableCode, bizObjectID, false);
            //实时生产动态 - 工序表数据ID
            string otherExceptionId = currentObj[RealTimeDynamicProduction.OperationTableDataID] + string.Empty;
            //实时生产动态 - 工序表SchemaCode
            string currentSchemaCode = currentObj[RealTimeDynamicProduction.CurrentPreviousOperationTableSchemacode] + string.Empty;
            //加载工序表中的业务对象
            BizObject otherObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, currentSchemaCode, otherExceptionId, false);
            //父流程实例ID
            string parentInstanceId = Request.WorkflowInstance.ParentInstanceId;
            ///获取父流程实例对象
            H3.Workflow.Instance.WorkflowInstance parentInstance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(parentInstanceId);

            //传递异常信息
            foreach (PropertySchema activex in otherObj.Schema.Properties)
            {
                if (activex.DisplayName.Contains("发起异常"))
                {
                    otherObj[activex.Name] = "是";
                }
                else if (activex.DisplayName.Contains("异常类别"))
                {
                    otherObj[activex.Name] = me[AbnormalCategory] + string.Empty;
                }
                else if(activex.DisplayName.Contains("异常代表"))
                {
                    otherObj[activex.Name] = me[ID];
                }
            }
            otherObj.Update();
        }
        var strActivityCode = Request.ActivityCode; //当前节点       
        if (strActivityCode != "Activity100" && strActivityCode != "Activity101") //工步节点
        {
            Request.BizObject[Owner] = Request.UserContext.UserId;//设置异常权限
        }
        DeleteTransferToWorkStep();
    }

    /*
    *--Author:fubin
    * 加载质量配置数据
    * @param planObj 工序计划数据
    */
    protected void LoadingQAConfiguration(BizObject planObj)
    {
        //读取《质量配置表》是否忽略理化结果顺序
        string crossCheck = LoadingConfig.GetQualityConfigForm(Engine, QAConfig.PriorityLevelphysicochemical);
        //读取《质量配置表》全局忽略理化结果配置
        string globalCrossCheck = LoadingConfig.GetQualityConfigForm(Engine, QAConfig.GlobalIgnorePhysicochemicalResults);
        //读取《工序计划表》单件忽略理化结果配置
        string planCrossCheck = planObj != null ? planObj[ABCDProcessPlan_SinglePieceIgnorePhysicochemicalResults] + string.Empty : string.Empty;

        switch (crossCheck)
        {
            case "配置表":
                //全局忽略理化结果
                me[WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = globalCrossCheck;
                break;
            case "计划表":
                if (planCrossCheck != string.Empty)
                {   //单件忽略理化结果
                    me[WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = planCrossCheck;
                }
                else
                {   //全局忽略理化结果
                    me[WhetherToIgnorePhysicalAndChemicalResultsresultFlow] = globalCrossCheck;
                }
                break;
        }
        //加载《订单规格表》数据
        BizObject productObj = LoadingConfig.GetProductData(Engine, planObj);          
        me[SampleType] = productObj[OrderSpecification_SampleSize]; //赋值试样尺寸
    }

    /*
    *--Author:fubin
    * 产品类别为空时，查询产品参数表中的车加工类别                        //////////////////////////不确定有没有用//////////
    */
    protected void ProductCategoryUpdate()
    {
        if (me[ProductType] + string.Empty == string.Empty)
        {  
            string orderSpec = me[OrderSpecificationNumber] + string.Empty; //订单规格号
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameter_ProductMachiningCategory, ProductParameter_TableCode,
                ProductParameter_OrderSpecificationNumber, orderSpec);
            DataTable typeData = Engine.Query.QueryTable(mySql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[ProductParameterTable] = typeData.Rows[0][ProductParameter_Objectid] + string.Empty;               
                me[ProductType] = typeData.Rows[0][ProductParameter_ProductMachiningCategory] + string.Empty; //赋值车加工类别
            }
        }
    }

    //毛坯
    string TableCode = "D001419Sgx7flbvwu9r0u3hail6512uq4";                                                                       //表ID
    string DispatchSwitch = "F0000235";                                                                                           //派工开关
    string InitiateAbnormal = "F0000060";                                                                                         //发起异常
    string TargetStep = "F0000072";                                                                                               //转至工步
    string AbnormalCategory = "F0000070";                                                                                         //异常类别
    string AbnormalDescription = "F0000079";                                                                                      //异常描述
    string AbnormalRepresentative = "F0000176";                                                                                   //异常代表
    string processAdjustmentRange = "F0000066";                                                                                   //是否跨工序调整流程
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";                                                                    //关联其它异常工件
    string MachineQualityAssurance = "F0000225";	                                                                               //机加质量保证
    string DemandApprovalForm = "F0000226";                                                                                       //需求审批单
    string CirculationApprovalSheet = "F0000227";                                                                                 //流转审批单
    string OtherApprovalDocuments = "F0000228";                                                                                   //其它审批单
    string SourceOfApproval = "F0000229";                                                                                         //审批来源
    string CurrentSection = "F0000071";                                                                                           //当前工序
    string CurrentWorkStep = "F0000069";                                                                                          //当前工步
    string InspectionResults = "F0000041";                                                                                        //检验结果
    string PhysicochemicalResults = "F0000122";                                                                                   //理化结果
    string WorkshopLocation = "F0000067";                                                                                         //车间位置
    string LogisticsSwitch = "F0000237";                                                                                          //物流开关
    string RollingMode = "F0000039";                                                                                              //轧制方式
    string DoubleRollingAssociatedForm = "F0000182";                                                                              //双轧关联表单
    string ID = "F0000058";                                                                                                       //ID
    string Owner = "OwnerId";                                                                                                     //拥有者
    string WhetherToIgnorePhysicalAndChemicalResultsresultFlow = "advanceTransfer";                                               //是否忽略理化结果流转
    string SampleType = "F0000119";                                                                                               //试样类型
    string ProductType = "F0000088";                                                                                              //产品类别
    string OrderSpecificationNumber = "F0000016";                                                                                 //订单规格号
    string ProductParameterTable = "F0000103";                                                                                    //产品参数表
    string PlanNoumenonSampling = "F0000077";                                                                                     //计划本取
    string WhetherTheSampleIsTransported = "F0000223";                                                                            //取样是否需要转运
    string WhetherTheRoughIsTransported = "F0000224";                                                                             //粗车是否需要转运
    string ConfirmNoumenonSampling = "F0000040";                                                                                  //确认本取
    //取样派工信息子表    
    string RoughCastSamplingDispatchingInformation_TableCode = "D0014194a44f0404f864f0dbfbe630064922ac9";                         //表ID
    string RoughCastSamplingDispatchingInformation_TaskStatus = "F0000213";                                                       //任务状态
    string RoughCastSamplingDispatchingInformation_SamplingWorkshopName = "F0000205";                                             //取样车间名称
    //粗车派工信息子表    
    string RoughCastRoughCuttingDispatchingInformation_TableCode = "D001419F550f0900f370420cb3346c763f61538f";                    //表ID
    string RoughCastRoughCuttingDispatchingInformation_TaskStatus = "F0000214";                                                   //任务状态
    string RoughCastRoughCuttingDispatchingInformation_RoughTurningWorkshopName = "F0000205";                                     //粗车车间名称
    //工艺流程表
    string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";                                                           //表ID
    string ProcessFlow_TargetSection = "F0000056";                                                                                //转至工序
    string ProcessFlow_TargetStep = "F0000057";                                                                                   //转至工步
    string ProcessFlow_DoubleRollingCutting = "F0000132";                                                                         //双轧切割状态
    string ProcessFlow_ObjectId = "ObjectId";                                                                                     //ObjectId
    //工序计划表
    string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";                                                       //表ID
    string ABCDProcessPlan_PlanNoumenonSampling = "F0000141";                                                                     //计划本取
    string ABCDProcessPlan_ProcessFlowTable = "F0000190";                                                                         //工艺流程表中的objectId
    string ABCDProcessPlan_ID = "F0000007";                                                                                       //ID
    string ABCDProcessPlan_SinglePieceIgnorePhysicochemicalResults = "F0000161";                                                  //单件忽略理化结果
    //A-C订单规格表    
    string OrderSpecification_SampleSize = "F0000134";                                                                            //试样尺寸
    //产品参数表
    string ProductParameter_TableCode = "D0014196b62f7decd924e1e8713025dc6a39aa5";                                                //表ID
    string ProductParameter_ProductMachiningCategory = "F0000004";                                                                //产品车加工类别
    string ProductParameter_OrderSpecificationNumber = "F0000073";                                                                //订单规格号
    string ProductParameter_Objectid = "ObjectId";                                                                                //ObjectId
}

