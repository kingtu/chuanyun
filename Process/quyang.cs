
using System;
using System.Collections.Generic;
using System.Text;
using H3;
using System.Data;
using H3.DataModel;
// "取样";
public class D001419Sgljz62e1rneytbqjckbe1vu25 : H3.SmartForm.SmartFormController
{
    BizObject me;                                   //本表单业务对象
    string activityCode;                            //当前节点
    H3.SmartForm.SmartFormResponseDataItem item;    //用户提示信息
    H3.SmartForm.SmartFormResponseDataItem userId;  //用户Id  
    public D001419Sgljz62e1rneytbqjckbe1vu25(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;                            //本表单业务对象
        activityCode = Request.ActivityCode;               //活动节点编码   
        item = new H3.SmartForm.SmartFormResponseDataItem();    //用户提示信息
        userId = new H3.SmartForm.SmartFormResponseDataItem();  //用户Id
        userId.Value = Request.UserContext.UserId;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (!Request.IsCreateMode)  //不是创建模式
            {
                UserId(response);//获取用户ID                
                InitSamplingSubProcess();//初始化取样理化               
                MachiningTime(); //统计机加工耗时               
                InitFlawDetectionForm(); //初始化探伤表
            }
        }
        catch (Exception ex)
        {
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode);
            item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
        response.ReturnData.Add("key1", item);
        base.OnLoad(response);
        ////--------------------------加载前后分割线-------------------------//

        //try
        //{
        //    if (!Request.IsCreateMode)
        //    {
        //        //加载后代码
        //    }
        //}
        //catch (Exception ex)
        //{
        //    info = Tools.Log.ErrorLog(Engine, me, ex, activityCode);
        //    item.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        //}
    }



    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            if (actionName == "Submit")
            {
                Workers(); //上下机加工审批权限人                
                MultistageProcessingLogic(activityCode);//多阶段加工流程逻辑                
                UnqualifiedSource();//赋值审批来源               
                // DipatchFlag();    // //派工开关              
            }            
            SMGSubmittedMinistry();//上下机时，赋值派工信息子表的任务状态          
            Authority.Approver(Request);  //审批人记录
            base.OnSubmit(actionName, postValue, response);         
            UpdateRecordForm(actionName, activityCode);   //多阶段加工新方案同步机加工任务记录
        }
        catch (Exception ex)
        {		
            string info = Tools.Log.ErrorLog(Engine, me, ex, activityCode);//负责人信息
            response.Message = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    /**
    * --Author: zzx
    * 赋值审批来源
    */
    protected void DipatchFlag()
    {
        string dipatchFlag = Request.BizObject[DispatchSwitch] + string.Empty; //派工开关
        if (dipatchFlag == "关")
        {
            if (activityCode == "Activity23")//判断是否为待上机节点
            {
                Request.BizObject[SamplingStageProcessingPerson] = (string)userId.Value; //加工节点的取样加工中人为上机权限人
            }
        }
    }
    /**
    * --Author: nkx
    * 赋值审批来源
    */
    protected void UnqualifiedSource()
    {
        string currentApprover = Request.UserContext.User.Name;                    //当前审批人
        string currentProcess = me[CurrentSection] + string.Empty;                 //当前工序
        string currentWorkStep = me[CurrentWorkStep] + string.Empty;               //当前工步
        if (me[InitiateAbnormal] + string.Empty == "是")                           //发起异常
        {
            string abnormal = "发起异常";
            string sourceOfApproval = currentApprover + "在" + currentProcess + "工序的" + currentWorkStep + "工步" + abnormal;
            me[SourceOfApproval] = sourceOfApproval;                                    //审批来源
            me.Update();
        }
    }

    /*
    *--Author:nkx
    * 上下机时，赋值派工信息子表的任务状态
    */
    protected void SMGSubmittedMinistry()
    {
        //节点为为待上机并且发起异常为为否
        if (activityCode == "Activity23" && me[InitiateAbnormal] + string.Empty == "否")
        {
            BizObject[] processSubs = me[SamplingMachiningSubtable_TableCode] as BizObject[];//工序表多阶段加工子表
            //判断多阶段加工子表有没有值
            if (processSubs != null && processSubs.Length > 0)
            {
                //工序表派工子表信息
                BizObject[] disProcessSubs = me[SamplingSamplingDispatchInformation_TableCode] as BizObject[];
                //判断工序表派工子表有没有值
                if (disProcessSubs != null && disProcessSubs.Length > 0)
                {
                    //循环多阶段加工子表信息
                    for (int i = 0; i < processSubs.Length; i++)
                    {
                        //判断多阶段加工子表的“派工任务”与工序表派工子表的“派工任务”是否一样
                        if (processSubs[i][SamplingMachiningSubtable_DispatchTask] + string.Empty == disProcessSubs[0][SamplingSamplingDispatchInformation_DispatchTask] + string.Empty)
                        {
                            //赋值工序表派工子表信息的“任务状态”
                            disProcessSubs[0][SamplingSamplingDispatchInformation_TaskStatus] = "加工中";
                        }
                    }
                }
            }
        }
        //节点为为 加工中 并且发起异常为为 否
        if (activityCode == "Activity104" && me[InitiateAbnormal] + string.Empty == "否")
        {
            //工序表派工子表信息
            BizObject[] disProcessSubs = me[SamplingSamplingDispatchInformation_TableCode] as BizObject[];
            //工序表多阶段加工子表
            BizObject[] processSubs = me[SamplingMachiningSubtable_TableCode] as BizObject[];
            //判断工序表多阶段加工子表有没有值
            if (processSubs != null && processSubs.Length > 0)
            {
                //判断工序表派工子表有没有值
                if (disProcessSubs != null && disProcessSubs.Length > 0)
                {
                    //循环多阶段加工子表信息
                    foreach (BizObject item in processSubs)
                    {
                        //判断多阶段加工子表的“加工量”与工序表派工子表的“加工量”是否一样
                        if (item[SamplingMachiningSubtable_ProcessingQuantity] + string.Empty == disProcessSubs[0][SamplingSamplingDispatchInformation_DispatchQuantity] + string.Empty)
                        {
                            //赋值工序表派工子表信息的“任务状态”
                            disProcessSubs[0][SamplingSamplingDispatchInformation_TaskStatus] = "已完成";
                        }
                    }
                }
            }
            //完成总量不等于1
            if (me[TotalAmountComplete] + string.Empty != "1")
            {
                //完成本取赋值空
                me[AccomplishNoumenonSampling] = null;
                me.Update();
            }
        }
    }

    /**
    * Auther： zzx
    *初始化取样理化 
    */
    protected void InitSamplingSubProcess()
    {
        //当前工序
        if (me[CurrentSection] + string.Empty == string.Empty) { me[CurrentSection] = "取样子流程"; }
        //初始化产品类别
        ProductCategoryUpdate();
        //获取多阶段加工子表
        BizObject[] thisLstArray = me[SamplingMachiningSubtable_TableCode] as BizObject[];
        //初始化任务名称
        me[TaskName] = me[TaskName] + string.Empty != string.Empty ? me[TaskName] + string.Empty : "1"; //初始化计数器
        if (thisLstArray == null)
        {
            //本表单纲目结构
            BizObjectSchema schema = Request.Engine.BizObjectManager.GetPublishedSchema(Request.SchemaCode);
            //初始化取样子表
            CreatSublist(me);
        }
        //完成总量
        if (me[TotalAmountComplete] + string.Empty == string.Empty)
        {
            me[TotalAmountComplete] = 0;
        }
        //获取当前流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(Request.WorkflowInstance.ParentInstanceId);
        var parentId = instance != null ? instance.BizObjectId : "";
        //获取父流程业务对象
        BizObject parentObj = Tools.BizOperation.Load(Request.Engine, RoughCast.TableCode, parentId);
        var parentInstanceId = parentObj.WorkflowInstanceId;
        H3.Workflow.Instance.WorkflowInstance instances = Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(parentInstanceId);
        //获取工序计划表数据
        BizObject planObject = LoadingConfig.GetPlanningData(Engine, instances);
        //读取《工序计划表》是否制样
        me[YesOrNoopenSamplePreparationProcess] = planObject != null ? planObject[ABCDProcessPlan_YesOrNoopenSamplePreparationProcess] + string.Empty : string.Empty;
        //更新本表
        me.Update();

    }

    /*
    *--Author:nkx
    * 上下机加工审批权限人筛选字段
    */
    private void Workers()
    {
        List<string> namestatus = new List<string>();
        //节点为 待上机
        if (activityCode == "Activity23")
        {
            //取样加工中人 为为 当前用户
            me[SamplingStageProcessingPerson] = Request.UserContext.UserId;

            namestatus.Add(Request.UserContext.UserId);

            //赋值当前审批人（文本类型）
            me[CurrentAuditors] = DispatchLogic.SyncParticipants(Request.Engine, namestatus);
        }
        //节点为 加工中
        if (activityCode == "Activity104")
        {
            //完成总量不等于1
            if (me[TotalAmountComplete] + string.Empty != "1")
            {
                //取样派工信息 不为空
                if (me[SamplingDispatchInformation] != null)
                {
                    //取样待上机人
                    string[] taskWorker = me[SamplingStageToBeStartedPerson] as string[];
                    foreach (string worke in taskWorker)
                    {
                        namestatus.Add(worke);
                    }
                    //赋值当前审批人（文本类型）
                    me[CurrentAuditors] = DispatchLogic.SyncParticipants(Request.Engine, namestatus);
                }
            }
        }
        //更新本表
        me.Update();
    }

    //把用户ID 写入集合 返回前端
    private void UserId(H3.SmartForm.LoadSmartFormResponse response)
    {
        H3.SmartForm.SmartFormResponseDataItem sd = new H3.SmartForm.SmartFormResponseDataItem();
        sd.Value = Request.UserContext.UserId;
        response.ReturnData.Add("UserId", sd);
    }

    /*
    *--Author:fubin
    * 产品类别为空时，查询产品参数表中的车加工类别
    */
    protected void ProductCategoryUpdate()
    {
        //产品类别更新
        if (me[ProductType] + string.Empty == string.Empty)
        {   //订单规格号
            string orderSpec = me[OrderSpecificationNumber] + string.Empty;
            //以订单规格号相同为条件，查询产品参数表中的车加工类别
            string mySql = string.Format("Select ObjectId,{0} From i_{1} Where {2} = '{3}'",
                ProductParameterTable_ProductMachiningCategory, ProductParameterTable_TableCode,
                ProductParameterTable_OrderSpecificationNumber, orderSpec);
            DataTable typeData = Engine.Query.QueryTable(mySql, null);
            if (typeData != null && typeData.Rows != null && typeData.Rows.Count > 0)
            {   //赋值产品参数表
                me[ProductParameterTable] = typeData.Rows[0][ProductParameterTable_Objectid] + string.Empty;
                //赋值车加工类别
                me[ProductType] = typeData.Rows[0][ProductParameterTable_ProductMachiningCategory] + string.Empty;
            }
            //更新本表
            me.Update();
        }
    }

    /*
    *--Author:fubin
    * 查询更新机加工耗时
    */
    protected void MachiningTime()
    {
        string bizid = me.ObjectId;
        //查询粗车加工中所有耗时
        string command = string.Format("Select b.bizobjectid,b.activitycode, sum(b.usedtime) as utime  From i_{0} " +
            " a left join H_WorkItem b on a.objectid = b.BizObjectId  where b.ActivityCode = 'Activity104' and b.BizObjectId = '{1}' " +
            " group by b.bizobjectid", TableCode, bizid);
        DataTable data = Engine.Query.QueryTable(command, null);
        //机加工耗时计算
        if (data != null && data.Rows != null && data.Rows.Count > 0)
        {
            if (data.Rows[0]["utime"] != null)
            {
                string utimestr = data.Rows[0]["utime"] + string.Empty;
                double utime = double.Parse(utimestr) / 10000000 / 60;  //转换时间单位为秒
                me[ActualProcessingTime] = utime;                       //实际加工耗时
            }
            //更新本表
            me.Update();
        }
    }

    /*
    *--Author:fubin
    * 初始化探伤表
    */
    protected void InitFlawDetectionForm()
    {   //探伤表
        string tsFormId = me[FlawDetectionTable] + string.Empty;
        //当前用户ID
        string userId = Request.UserContext.UserId;
        //活动节点
        string activityName = Request.ActivityCode;
        if (tsFormId == string.Empty)  //探伤表为空时，查询探伤表中ID相同的数据放入本表单中
        {
            BizObject tsForm = Tools.BizOperation.New(Engine, FlawDetectionTable_TableCode);              //新建一个探伤表业务对象
            tsForm.Status = BizObjectStatus.Effective;                                                         //生效
            tsForm[FlawDetectionTable_ID] = me[ID] + string.Empty;                                                          //ID
            tsForm[FlawDetectionTable_CurrentOperation] = "取样子流程";                                                      //赋值探伤表的当前工序
            tsForm[FlawDetectionTable_SampleProcess] = me.ObjectId;                                                         //赋值探伤表的取样子流程 为 本表单ObjectId
            List<BizObject> lstObject = new List<BizObject>();                                 //new子表数据集合
            BizObject lstArray = Tools.BizOperation.New(Engine, FlawDetectionSubTable_TableCode);         //new一个子表业务对象
            lstArray[FlawDetectionSubTable_Process] = "取样子流程";                                                          //赋值探伤子表的工序
            lstArray[FlawDetectionSubTable_WorkStep] = "";                                                                  //赋值探伤子表的工步
            lstObject.Add(lstArray);                                                                                        //将这个子表业务对象添加至子表数据集合中
            tsForm[FlawDetectionTable_FlawDetectionRecord] = lstObject.ToArray();                                           //子表数据赋值
            tsForm.Create();                                                                                                //更新
            me[FlawDetectionTable] = tsFormId = tsForm.ObjectId;                                                            //赋值探伤表
            me.Update();                                                                                                    //更新本表
        }
        if (tsFormId != string.Empty)                                                                                        //探伤表不为空时,写入工序信息
        {
            BizObject tsForm = Tools.BizOperation.Load(Engine, FlawDetectionTable_TableCode, tsFormId);   //加载探伤表
            BizObject[] lstArray = tsForm[FlawDetectionSubTable_TableCode] as BizObject[];        //获取子表数据
            tsForm[FlawDetectionTable_CurrentOperation] = "取样子流程";                                                      //赋值探伤表的当前工序
            tsForm[FlawDetectionTable_SampleProcess] = me.ObjectId;                                                         //赋值探伤表的取样子流程 为 本表单ObjectId
            if (lstArray[lstArray.Length - 1][FlawDetectionSubTable_ThisFlawDetectionResult] + string.Empty == string.Empty) //探伤结果
            {
                lstArray[lstArray.Length - 1][FlawDetectionSubTable_Process] = "取样子流程";                                 //赋值探伤子表的工序
                lstArray[lstArray.Length - 1][FlawDetectionSubTable_WorkStep] = me[CurrentWorkStep] + string.Empty;         //赋值探伤子表的工步
                lstArray[lstArray.Length - 1].Update();                                                                     //更新
            }
            tsForm.Update();                                                                                                //更新
        }
    }

    /*
    *--Author:fubin
    * 创建添加新的取样子表行数据
    * @param thisObj 本表单数据
    */
    protected void CreatSublist(BizObject thisObj)
    {
        //new一个取样多阶段加工子表业务对象
        BizObject ChildObj = Tools.BizOperation.New(Engine, SamplingMachiningSubtable_TableCode);
        ChildObj[SamplingMachiningSubtable_CountingTask] = thisObj[TaskName] + string.Empty == string.Empty ? "1" : thisObj[TaskName] + string.Empty; //任务名称
        //派工开关
        string dipatchFlag = Request.BizObject[DispatchSwitch] + string.Empty;
        if (dipatchFlag == "关")
        {
            //子表 任务名称
            ChildObj[SamplingMachiningSubtable_DispatchTask] = "默认派工任务";
        }
        //添加一行子表数据
        Tools.BizOperation.AddChildBizObject(Engine, thisObj, SamplingMachiningSubtable_TableCode, ChildObj);
    }

    /*
    *--Author:fubin
    * 多阶段加工流程逻辑
    * @param activityCode 流程节点编码
    */
    public void MultistageProcessingLogic(string activityCode)
    {
        //获取多阶段加工子表
        BizObjectSchema schema = Request.Engine.BizObjectManager.GetPublishedSchema(Request.SchemaCode);
        BizObject[] lstArray = me[SamplingMachiningSubtable_TableCode] as BizObject[];//获取取样多阶段加工子表

        {   //修正任务数-----任务名称
            me[TaskName] = lstArray != null ? lstArray.Length + string.Empty : "1";
            //获取任务数
            int taskNum = me[TaskName] + string.Empty != string.Empty ? int.Parse(me[TaskName] + string.Empty) - 1 : 0; //获取任务数

            if (activityCode == "Activity23") //取样上机
            {    //当前加工者
                lstArray[taskNum][SamplingMachiningSubtable_Processor] = Request.UserContext.UserId;
                //加工开始时间
                lstArray[taskNum][SamplingMachiningSubtable_StartTime] = System.DateTime.Now;
            }

            if (activityCode == "Activity104") //取样下机
            {
                //任务递增至取样下机
                if ((me[TotalAmountComplete] + string.Empty) != string.Empty && decimal.Parse(me[TotalAmountComplete] + string.Empty) < 1)
                {
                    //递增计数器，并更新
                    me[TaskName] = lstArray.Length + 1;

                    //创建添加新的子表行数据
                    CreatSublist(me);
                }

                if (lstArray[taskNum][SamplingMachiningSubtable_Processor] + string.Empty == string.Empty)
                {   //当前加工者
                    lstArray[taskNum][SamplingMachiningSubtable_Processor] = Request.UserContext.UserId;
                }
            }
        }
        //探伤表Id
        string objId = me[FlawDetectionTable] + string.Empty;
        //返回探伤结果
        if (activityCode == "Activity105" && objId != string.Empty)
        {
            BizObject tsForm = Tools.BizOperation.Load(Engine, FlawDetectionTable_TableCode, objId);
            //赋值探伤认定
            me[FlawDetectionIdentification] = tsForm[FlawDetectionTable_FlawDetectionIdentification] + string.Empty;
        }
    }

    //同步数据至“机加工任务记录”
    public void UpdateRecordForm(string actionName, string activityCode)
    {
        BizObjectSchema schema = Request.Engine.BizObjectManager.GetPublishedSchema(Request.SchemaCode);
        BizObject[] lstArray = me[SamplingMachiningSubtable_TableCode] as BizObject[];  //获取取样子表
        //任务计数器
        int taskNum = me[AccomplishNoumenonSampling] + string.Empty != "已取样" ? int.Parse(me[TaskName] + string.Empty) - 2 : int.Parse(me[TaskName] + string.Empty) - 1;
        if (actionName == "Submit" && activityCode == "Activity104" && lstArray != null) //四面光下机\取样下机
        {
            //当前任务记录
            BizObject currentTask = lstArray[taskNum];
            //设备工时系数表-子表
            BizObject[] subObj = null;
            //设备工时系数表
            BizObject mtObj = null;
            //当前加工者
            H3.Organization.User employee = Engine.Organization.GetUnit(currentTask[SamplingMachiningSubtable_Processor] + string.Empty) as H3.Organization.User;
            //总下屑量
            string totalxx = "";
            //本工序产品工时
            string productTime = "";
            //轧制方式
            string zzMode = me[RollingMode] + string.Empty;
            //产品类别
            string productType = me[ProductType] + string.Empty;
            //设备类型
            string deviceType = currentTask[SamplingMachiningSubtable_EquipmentType] + string.Empty;
            //设备工时系数
            string deviceParam = string.Empty;
            //产品类别
            if (productType != string.Empty)
            {
                //获取设备工时系数模块
                string command = string.Format("Select ObjectId From i_{0} Where {1} = '粗车' and {2} = '{3}'", DeviceWorkingHour_TableCode, DeviceWorkingHour_OperationName, DeviceWorkingHour_ProductMachiningCategory, productType);//产品类别
                DataTable data = Engine.Query.QueryTable(command, null);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    mtObj = BizObject.Load(H3.Organization.User.SystemUserId, Engine, DeviceWorkingHour_TableCode, data.Rows[0]["ObjectId"] + string.Empty, true);
                }
            }
            //设备工时系数表-子表
            subObj = mtObj != null ? mtObj[EquipmentTimeCoefficientSubtabulation_TableCode] as BizObject[] : null;
            //设备工时系数表子表 不为空
            if (subObj != null)
            {
                foreach (BizObject item in subObj)
                {
                    if (deviceType != string.Empty)
                    {
                        //按设备类型查找
                        if (item[EquipmentTimeCoefficientSubtabulation_EquipmentType] + string.Empty == deviceType)
                        {
                            if (zzMode == "单轧" && item[EquipmentTimeCoefficientSubtabulation_SingleRollingManHourCoefficient] != null)
                            {
                                //单轧工时系数
                                deviceParam = item[EquipmentTimeCoefficientSubtabulation_SingleRollingManHourCoefficient] + string.Empty;
                            }
                            else if (zzMode == "双轧" && item[EquipmentTimeCoefficientSubtabulation_DoubleRollingManHourCoefficient] != null)
                            {
                                //双轧工时系数
                                deviceParam = item[EquipmentTimeCoefficientSubtabulation_DoubleRollingManHourCoefficient] + string.Empty;
                            }
                        }
                    }
                    break;
                }
            }
            //产品参数表
            BizObject productObj = Tools.BizOperation.Load(Engine, ProductParameterTable_TableCode, me[ProductParameterTable] + string.Empty);
            //产品参数表 不为空
            if (productObj != null)
            {
                //产品参数表-单轧工时
                if (zzMode == "单轧" && productObj[ProductParameterTable_SingleRoughingMaNHour] != null)
                {
                    //根据本表单产品轧制方式从产品参数表中获取"单轧毛坯工时"
                    productTime = productObj[ProductParameterTable_SingleRoughingMaNHour] + string.Empty;
                    //单轧毛坯下屑
                    totalxx = productObj[ProductParameterTable_SingleRollingRoughTurningChip] + string.Empty;
                }
                //产品参数表-双轧工时
                if (zzMode == "双轧" && productObj[ProductParameterTable_DoubleRoughingManhour] != null)
                {
                    //根据本表单产品轧制方式从产品参数表中获取"双轧毛坯工时"
                    productTime = productObj[ProductParameterTable_DoubleRoughingManhour] + string.Empty;
                    //双轧毛坯下屑
                    totalxx = productObj[ProductParameterTable_DoubleRollingRoughingChip] + string.Empty;
                }
            }
            //新建机加工记录数据对象
            BizObject recordObj = Tools.BizOperation.New(Engine, MachiningTaskRecord_TableCode);
            recordObj.Status = BizObjectStatus.Effective;                                                                          //设置为生效状态
            recordObj[MachiningTaskRecord_OperationName] = "取样";                                                                              //工序
            recordObj[MachiningTaskRecord_ProductSpecification] = me[ProductSpecification] + string.Empty;                                      //产品规格
            recordObj[MachiningTaskRecord_ID] = me[ID] + string.Empty;                                                                          //工件ID
            recordObj[MachiningTaskRecord_WorkPieceNumber] = me[WorkpieceNumber] + string.Empty;                                                //工件号
            recordObj[MachiningTaskRecord_TaskName] = taskNum;                                                                                  //任务计数器
            recordObj[MachiningTaskRecord_SampleType] = me[SampleType] + string.Empty;                                                          //试样类型
            recordObj[MachiningTaskRecord_Processor] = currentTask[SamplingMachiningSubtable_Processor] + string.Empty;                         //加工者
            recordObj[MachiningTaskRecord_DepartmentName] = employee != null ? employee.DepartmentName : "";                                    //部门名称
            recordObj[MachiningTaskRecord_StartTime] = currentTask[SamplingMachiningSubtable_StartTime] + string.Empty;                         //加工开始时间
            recordObj[MachiningTaskRecord_DeviceName] = currentTask[SamplingMachiningSubtable_EquipmentName] + string.Empty;                    //设备名称
            recordObj[MachiningTaskRecord_DeviceNumber] = currentTask[SamplingMachiningSubtable_EquipmentNumber] + string.Empty;                //设备编号
            recordObj[MachiningTaskRecord_DeviceType] = currentTask[SamplingMachiningSubtable_EquipmentType] + string.Empty;                    //设备类型
            recordObj[MachiningTaskRecord_DeviceCoefficient] = deviceParam;                                                                     //设备工时系数
            recordObj[MachiningTaskRecord_RollingMode] = zzMode;                                                                                //轧制方式
            recordObj[MachiningTaskRecord_ProcessChipWeight] = totalxx;                                                                         //工艺下屑量
            recordObj[MachiningTaskRecord_WorkLoad] = currentTask[SamplingMachiningSubtable_ProcessingQuantity] + string.Empty;                 //任务加工量
            recordObj[MachiningTaskRecord_EndTime] = DateTime.Now;                                                                              //加工结束时间
            double pTime = productTime != string.Empty ? double.Parse(productTime) : 0;                                                         //本工序产品工时转换
            double dParam = deviceParam != string.Empty ? double.Parse(deviceParam) : 0;                                                        //设备工时系数转换
            double mScale = currentTask[SamplingMachiningSubtable_ProcessingQuantity] + string.Empty != string.Empty ? double.Parse(currentTask[SamplingMachiningSubtable_ProcessingQuantity] + string.Empty) : 0; //加工量转换
            recordObj[MachiningTaskRecord_ProcessManHour] = pTime;                                                                              //本工序产品工时
            recordObj[MachiningTaskRecord_UnitmanHour] = pTime * dParam;                                                                        //单件拟定工时
            recordObj[MachiningTaskRecord_TaskManHour] = pTime * dParam * mScale;                                                               //任务工时

            if (productObj != null)
            {
                recordObj[MachiningTaskRecord_ProductName] = productObj[ProductParameterTable_ProductName] + string.Empty;                                  //产品名称
                recordObj[MachiningTaskRecord_LatheProcessingCategory] = productObj[ProductParameterTable_ProductMachiningCategory] + string.Empty;         //产品类别
                recordObj[MachiningTaskRecord_DrillingProcessingCategory] = productObj[ProductParameterTable_ProductDrillingCategory] + string.Empty;       //产品小类
                recordObj[MachiningTaskRecord_OrderSpecifications] = productObj[ProductParameterTable_OrderSpecificationNumber] + string.Empty;             //订单规格号
                recordObj[MachiningTaskRecord_UnitWeightofFinish] = productObj[ProductParameterTable_FinishedProductUnitWeight] + string.Empty;             //成品单重
                recordObj[MachiningTaskRecord_OutsideDiameter] = productObj[ProductParameterTable_ACOuterDiameter] + string.Empty;                          //工件外径
                recordObj[MachiningTaskRecord_InsideDiameter] = productObj[ProductParameterTable_ACInnerDiameter] + string.Empty;                           //工件内径
                recordObj[MachiningTaskRecord_TotalHeight] = productObj[ProductParameterTable_ACTotalHeight] + string.Empty;                                //工件总高
                recordObj[MachiningTaskRecord_Thickness] = productObj[ProductParameterTable_ACSheetThickness] + string.Empty;                               //工件片厚
                recordObj[MachiningTaskRecord_HoleAmount] = productObj[ProductParameterTable_ACNumberOfHoles] + string.Empty;                               //工件孔数
                recordObj[MachiningTaskRecord_Aperture] = productObj[ProductParameterTable_ACHoleDiameter] + string.Empty;                                  //工件孔径
            }

            DateTime startTime = recordObj[MachiningTaskRecord_StartTime] + string.Empty != string.Empty ? Convert.ToDateTime(recordObj[MachiningTaskRecord_StartTime] + string.Empty) : DateTime.Now; //加工开始时间
            TimeSpan delayTime = DateTime.Now.Subtract(startTime);                                                                                          //与现在时间的差值
            recordObj[MachiningTaskRecord_ActualElapsedTime] = delayTime.TotalHours;                                                                        //实际耗时
            recordObj[MachiningTaskRecord_DataCode] = me[DataCode] + string.Empty;                                                                          //数据代码
            recordObj.Create();
            currentTask[SamplingMachiningSubtable_ProcessingRecord] = recordObj.ObjectId;                                                                   //当前任务加工记录
            currentTask.Update();
        }        
        if (lstArray != null && lstArray.Length > 0)
        {
            for (int i = taskNum - 1; i >= 0; i--)
            {                
                //循环加载机加工任务记录数据
                BizObject recordObj = Tools.BizOperation.Load(Engine, MachiningTaskRecord_TableCode, lstArray[i][SamplingMachiningSubtable_ProcessingRecord] + string.Empty);
                if (recordObj != null)
                {
                    //探伤结果
                    recordObj[MachiningTaskRecord_UltrasonicResults] = me[FlawDetectionIdentification] + string.Empty;
                    recordObj.Update();
                }
            }
        }
    }

    //取样
    string TableCode = "D001419Sgljz62e1rneytbqjckbe1vu25";                                            	                 //表ID
    string DispatchSwitch = "F0000221";                                                                	                 //派工开关
    string SamplingStageToBeStartedPerson = "F0000214";                                                                	 //取样待上机人
    string SamplingStageProcessingPerson = "F0000218";                                                                      //取样加工中人
    string CurrentAuditors = "CurrentAuditors";                                                        	                 //当前审批人
    string CurrentSection = "F0000071";                                                              	                     //当前工序
    string CurrentWorkStep = "F0000069";                                                               	                 //当前工步
    string InitiateAbnormal = "F0000060";                                                             	                     //发起异常
    string SourceOfApproval = "F0000222";                                                                	                 //审批来源
    string TotalAmountComplete = "F0000104";                                                          	                     //完成总量
    string SamplingDispatchInformation = "D0014199bf30a21df3f43f7b5cd8f0fda256d4c";                  	                     //取样派工信息
    string ProductType = "F0000088";                                                               	                     //产品类别
    string OrderSpecificationNumber = "F0000016";                                                      	                 //订单规格号
    string ProductParameterTable = "F0000103";                                                         	                 //产品参数表
    string ActualProcessingTime = "CountTime";                                                         	                 //实际加工耗时
    string FlawDetectionTable = "F0000173";                                                            	                 //探伤表
    string FlawDetectionIdentification = "F0000174";                                                   	                 //探伤认定
    string AccomplishNoumenonSampling = "F0000106";                                                                	     //完成本取
    string TaskName = "F0000082";                                                                      	                 //任务名称
    string RollingMode = "F0000039";                                                                 	                     //轧制方式
    string ProductSpecification = "F0000003";                                                          	                 //产品规格
    string ID = "F0000058";                                                                                  	             //ID
    string WorkpieceNumber = "F0000025";                                                               	                 //工件号
    string SampleType = "F0000119";                                                            	       	                 //试样类型
    string DataCode = "F0000064";                                                                      	                 //数据代码
    string YesOrNoopenSamplePreparationProcess = "F0000124";                                                                //是否制样

    //取样派工信息子表    
    string SamplingSamplingDispatchInformation_TableCode = "D0014199bf30a21df3f43f7b5cd8f0fda256d4c";                       //表ID
    string SamplingSamplingDispatchInformation_TaskStatus = "F0000204";                                                     //任务状态
    string SamplingSamplingDispatchInformation_DispatchTask = "F0000205";                                                   //任务状态
    string SamplingSamplingDispatchInformation_DispatchQuantity = "F0000162";                                               //派工量
    string SamplingSamplingDispatchInformation_SamplingWorkshopName = "F0000201";                                           //取样车间名称

    //取样机加工子表    
    string SamplingMachiningSubtable_TableCode = "D001419Fj7nrmbgha1j10v5zst0zg7hi1";                                       //表ID
    string SamplingMachiningSubtable_CountingTask = "F0000210";                                                             //任务计数
    string SamplingMachiningSubtable_DispatchTask = "F0000211";                                                             //任务名称
    string SamplingMachiningSubtable_Processor = "F0000157";                                                                //当前加工者
    string SamplingMachiningSubtable_EquipmentName = "F0000158";                                                            //设备名称
    string SamplingMachiningSubtable_EquipmentNumber = "F0000159";                                                          //设备编号
    string SamplingMachiningSubtable_EquipmentType = "F0000160";                                                            //设备类型
    string SamplingMachiningSubtable_StartTime = "F0000164";                                                                //开始时间
    string SamplingMachiningSubtable_ProcessingQuantity = "F0000162";                                                       //任务加工量
    string SamplingMachiningSubtable_ProcessingRecord = "F0000161";                                                         //当前任务加工记录

    //工序计划表
    string ABCDProcessPlan_YesOrNoopenSamplePreparationProcess = "F0000225";                                                //是否制样

    //产品参数表
    string ProductParameterTable_TableCode = "D0014196b62f7decd924e1e8713025dc6a39aa5";                                     //表ID
    string ProductParameterTable_ProductMachiningCategory = "F0000004";                                                     //产品车加工类别
    string ProductParameterTable_OrderSpecificationNumber = "F0000073";                                                     //订单规格号
    string ProductParameterTable_Objectid = "ObjectId";                                                                     //ObjectId
    string ProductParameterTable_SingleRoughingMaNHour = "F0000048";                                                        //单轧粗车工时
    string ProductParameterTable_SingleRollingRoughTurningChip = "F0000045";                                                //单轧粗车下屑
    string ProductParameterTable_DoubleRoughingManhour = "F0000049";                                                        //双轧粗车工时
    string ProductParameterTable_DoubleRollingRoughingChip = "F0000046";                                                    //双轧粗车下屑
    string ProductParameterTable_ProductName = "F0000067";                                                                  //产品名称
    string ProductParameterTable_ProductDrillingCategory = "F0000006";                                                      //产品小类
    string ProductParameterTable_FinishedProductUnitWeight = "F0000014";                                                    //成品单重
    string ProductParameterTable_ACOuterDiameter = "F0000082";                                                              //工件外径
    string ProductParameterTable_ACInnerDiameter = "F0000083";                                                              //工件内径
    string ProductParameterTable_ACTotalHeight = "F0000084";                                                                //工件总高
    string ProductParameterTable_ACSheetThickness = "F0000085";                                                             //工件片厚
    string ProductParameterTable_ACNumberOfHoles = "F0000086";                                                              //工件孔数
    string ProductParameterTable_ACHoleDiameter = "F0000087";                                                               //工件孔径

    //探伤表
    string FlawDetectionTable_TableCode = "D001419fdcaecf556264750ae2d5684b2a3706e";                                        //表ID
    string FlawDetectionTable_ID = "F0000001";                                                                              //ID
    string FlawDetectionTable_CurrentOperation = "F0000023";                                                                //当前工序
    string FlawDetectionTable_FlawDetectionIdentification = "F0000004";                                                     //探伤认定
    string FlawDetectionTable_SampleProcess = "F0000021";                                                                   //取样子流程
    string FlawDetectionTable_FlawDetectionRecord = "D001419F89050d4fc56d4bf7b41f343f2e3bd5a1";                             //探伤记录

    //探伤子表
    string FlawDetectionSubTable_TableCode = "D001419F89050d4fc56d4bf7b41f343f2e3bd5a1";                                    //表ID
    string FlawDetectionSubTable_Process = "F0000017";                                                                      //工序
    string FlawDetectionSubTable_WorkStep = "F0000018";                                                                     //工步
    string FlawDetectionSubTable_ThisFlawDetectionResult = "F0000002";                                                      //探伤结果

    //设备工时系数表
    string DeviceWorkingHour_TableCode = "D0014195ed7e837ecee4f97800877820d9a2f05";                                         //表ID
    string DeviceWorkingHour_OperationName = "F0000001";                                                                    //工序名称
    string DeviceWorkingHour_ProductMachiningCategory = "F0000002";                                                         //产品车加工类别

    //设备工时系数表子表
    string EquipmentTimeCoefficientSubtabulation_TableCode = "D001419Fbb7854d117af4bba8eff4de46d128f63";                    //表ID
    string EquipmentTimeCoefficientSubtabulation_EquipmentType = "F0000004";                                                //设备类型
    string EquipmentTimeCoefficientSubtabulation_SingleRollingManHourCoefficient = "F0000007";                              //单轧工时系数
    string EquipmentTimeCoefficientSubtabulation_DoubleRollingManHourCoefficient = "F0000008";                              //双轧工时系数

    //加工任务记录表
    string MachiningTaskRecord_TableCode = "D0014194963919529e44d60be759656d4a16b63";                                       //表ID
    string MachiningTaskRecord_OperationName = "F0000001";                                                                  //工序名称
    string MachiningTaskRecord_ProductSpecification = "F0000003";                                                           //产品规格
    string MachiningTaskRecord_ID = "ID";                                                                                   //ID
    string MachiningTaskRecord_WorkPieceNumber = "F0000040";                                                                //工件号
    string MachiningTaskRecord_TaskName = "F0000002";                                                                       //任务名称
    string MachiningTaskRecord_SampleType = "F0000032";                                                                     //试样类型
    string MachiningTaskRecord_Processor = "F0000011";                                                                      //加工者
    string MachiningTaskRecord_DepartmentName = "F0000030";                                                                 //部门名称
    string MachiningTaskRecord_StartTime = "StartTime";                                                                     //加工开始时间
    string MachiningTaskRecord_DeviceName = "F0000007";                                                                     //设备名称
    string MachiningTaskRecord_DeviceNumber = "F0000014";                                                                   //设备编号
    string MachiningTaskRecord_DeviceType = "F0000041";                                                                     //设备类型
    string MachiningTaskRecord_DeviceCoefficient = "F0000008";                                                              //设备系数
    string MachiningTaskRecord_RollingMode = "F0000024";                                                                    //轧制方式
    string MachiningTaskRecord_ProcessChipWeight = "F0000023";                                                              //工艺下屑重量
    string MachiningTaskRecord_WorkLoad = "F0000010";                                                                       //任务加工量
    string MachiningTaskRecord_EndTime = "EndTime";                                                                         //加工结束时间
    string MachiningTaskRecord_ProcessManHour = "F0000004";                                                                 //本工序产品工时
    string MachiningTaskRecord_UnitmanHour = "F0000005";                                                                    //单件拟定工时
    string MachiningTaskRecord_TaskManHour = "F0000006";                                                                    //任务工时
    string MachiningTaskRecord_ProductName = "F0000026";                                                                    //产品名称
    string MachiningTaskRecord_LatheProcessingCategory = "F0000027";                                                        //车加工类别
    string MachiningTaskRecord_DrillingProcessingCategory = "F0000025";                                                     //钻加工类别
    string MachiningTaskRecord_OrderSpecifications = "ProductNum";                                                          //订单规格号
    string MachiningTaskRecord_UnitWeightofFinish = "F0000017";                                                             //成品单重
    string MachiningTaskRecord_OutsideDiameter = "F0000016";                                                                //工件外径
    string MachiningTaskRecord_InsideDiameter = "F0000018";                                                                 //工件内径
    string MachiningTaskRecord_TotalHeight = "F0000020";                                                                    //工件总高
    string MachiningTaskRecord_Thickness = "F0000019";                                                                      //工件片厚
    string MachiningTaskRecord_HoleAmount = "F0000021";                                                                     //工件孔数
    string MachiningTaskRecord_Aperture = "F0000022";                                                                       //工件孔径
    string MachiningTaskRecord_ActualElapsedTime = "F0000013";                                                              //实际耗时
    string MachiningTaskRecord_DataCode = "F0000015";                                                                       //数据代码
    string MachiningTaskRecord_UltrasonicResults = "F0000029";                                                              //探伤结果
}
