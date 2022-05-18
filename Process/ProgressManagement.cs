using H3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

public class ProgressManagement
{
    //锯切进度同步逻辑
    public static Hashtable SawCutProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询锯切工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {	                            //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity135", "外协流程");
                            activityCodes.Add("Activity80", "质量异常");
                            activityCodes.Add("Activity81", "设备异常");
                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "1")
                        {
                            if ("合格,利用".Contains(item[SawCut.QualityInspectionConclusion] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "已投产";
                            }
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "1z.锯切/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.DemandPeriodOfThisProcedureSawing] + string.Empty != string.Empty)
                        {
                            //本工序需求期-锯切
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.DemandPeriodOfThisProcedureSawing] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-锯切与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //锯切-延期天数
                                targetObj[RealTimeDynamicProduction.SawingDelayDays] = Math.Abs(days);
                                //锯切-提前天数
                                targetObj[RealTimeDynamicProduction.SawingLeadDays] = "0";
                                //锯切-准时完工
                                targetObj[RealTimeDynamicProduction.SawingOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //锯切-延期天数 
                                targetObj[RealTimeDynamicProduction.SawingDelayDays] = "0";
                                //锯切-提前天数
                                targetObj[RealTimeDynamicProduction.SawingLeadDays] = days;
                                //锯切-准时完工
                                targetObj[RealTimeDynamicProduction.SawingOnTimeCompletion] = "1";
                            }
                        }

                        //锯切-完成日期
                        targetObj[RealTimeDynamicProduction.SawingCompletionDate] = instance.FinishTime.ToString();
                    }



                    if (instance.IsUnfinished)
                    {

                        string[] activties = { "a.待锯切", "b.锯切中", "c.确认调整意见", "d.审批确认", "e.质量异常", "f.外协流程" };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] =
                                    "1" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "1y.锯切/" + item[currentWorkStep];
                            }
                        }


                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[SawCut.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[SawCut.WorkshopLocation] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[SawCut.ProductLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[SawCut.WorkshopLocation] + " " + item[SawCut.ProductLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "锯切";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[SawCut.EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "1";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }

                
                H3.DataModel.BizObject another = GetAnotherProcess(engine , (string)targetObj[SawCut.OperationSchedule]);
                SyncSawCutProgress(another, targetObj);
            }
        }
        return workSteps;
    }
    
    private static void SyncSawCutProgress(H3.DataModel.BizObject target, H3.DataModel.BizObject current)
    {
        if (target == null) { return; }
        List<string> mpaping = new List<string>
        {
             //市场需求达成进度
            RealTimeDynamicProduction.MarketDemandAchievementProgress,
            RealTimeDynamicProduction.CurrentWorkStep, //当前工步
            //热加工需求达成进度
            RealTimeDynamicProduction.HotProcessingDemandAchievementProgress,          
            //锯切-延期天数 
            RealTimeDynamicProduction.SawingDelayDays,
            //锯切-提前天数
            RealTimeDynamicProduction.SawingLeadDays,
            //锯切-准时完工
            RealTimeDynamicProduction.SawingOnTimeCompletion,
            //锯切-完成日期
            RealTimeDynamicProduction.SawingCompletionDate,
            //当前工步
            RealTimeDynamicProduction.CurrentWorkStep,
            //热加工需求达成进度
            RealTimeDynamicProduction.HotProcessingDemandAchievementProgress,
            //车间位置
            RealTimeDynamicProduction.Workshop,
            //区域位置
            RealTimeDynamicProduction.Area,
            //产品位置
            RealTimeDynamicProduction.ProductLocation,
            //当前加工者
            RealTimeDynamicProduction.Worker,
            //当前工序
            RealTimeDynamicProduction.CurrentOperation,
            //更新日期
            RealTimeDynamicProduction.UpdateDate,
            //当前工序编号
            RealTimeDynamicProduction.OperationNumber,
            //当前工序表ObjectId
            "F0000070",
            //当前工序表schemaCode
            "F0000071"
         };
        //填充数据空值
        Tools.BizOperation.FillNullActivex(target);
        Copy(target, current, mpaping);
    }



    //锻压进度同步逻辑
    public static Hashtable ForgetProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询锻压工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {	                            //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity180", "外协流程");
                            activityCodes.Add("Activity134", "质量异常");
                            activityCodes.Add("Activity184", "其它异常");
                            activityCodes.Add("Activity177", "质量处理审批");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "2")
                        {
                            targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "已投产";
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "2z.锻压/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.DemandPeriodOfThisOperationForging] + string.Empty != string.Empty)
                        {
                            //本工序需求期-锻压
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.DemandPeriodOfThisOperationForging] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-锻压与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //锻压-延期天数
                                targetObj[RealTimeDynamicProduction.ForgingDelayDays] = Math.Abs(days);
                                //锻压-提前天数
                                targetObj[RealTimeDynamicProduction.ForgingLeadDays] = "0";
                                //锻压-准时完工
                                targetObj[RealTimeDynamicProduction.ForgingOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //锻压-延期天数 
                                targetObj[RealTimeDynamicProduction.ForgingDelayDays] = "0";
                                //锻压-提前天数
                                targetObj[RealTimeDynamicProduction.ForgingLeadDays] = days;
                                //锻压-准时完工
                                targetObj[RealTimeDynamicProduction.ForgingOnTimeCompletion] = "1";
                            }
                        }

                        //锻压-完成日期
                        targetObj[RealTimeDynamicProduction.ForgingCompletionDate] = instance.FinishTime.ToString();
                    }



                    if (instance.IsUnfinished)
                    {

                        string[] activties = {
                            "a.待转运", "b.待装炉", "c.待升温", "d.升温中", "e.待锻压", "f.锻压中", "g.确认调整意见",
                            "h.审批确认", "i.质量处理审批", "k.质量异议", "g.其它异常", "l.外协管理"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] =
                                    "2" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "2y.锻压/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[Forge.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[Forge.WorkshopLocation] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[Forge.ProductLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[Forge.WorkshopLocation] + " " + item[Forge.ProductLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "锻压";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "2";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
                H3.DataModel.BizObject another = GetAnotherProcess(engine, (string)targetObj[Forge .OperationSchedule ]);
                SyncForgetProgress(another, targetObj);
            }
        }
        return workSteps;
    }
    public static void SyncForgetProgress(H3.DataModel.BizObject target, H3.DataModel.BizObject current)
    {
        if (target == null) { return; }
        List<string> mpaping = new List<string>
        {       
        //市场需求达成进度
        RealTimeDynamicProduction.MarketDemandAchievementProgress ,
        //当前工步
        RealTimeDynamicProduction.CurrentWorkStep ,
        //热加工需求达成进度
        RealTimeDynamicProduction.HotProcessingDemandAchievementProgress ,

        //锻压-延期天数
        RealTimeDynamicProduction.ForgingDelayDays ,
        //锻压-提前天数
        RealTimeDynamicProduction.ForgingLeadDays ,
        //锻压-准时完工
        RealTimeDynamicProduction.ForgingOnTimeCompletion ,
        //锻压-完成日期
        RealTimeDynamicProduction.ForgingCompletionDate ,
        //当前工步
        RealTimeDynamicProduction.CurrentWorkStep ,
        //车间位置
        RealTimeDynamicProduction.Workshop ,
        //区域
        RealTimeDynamicProduction.Area , 
        //产品位置
        RealTimeDynamicProduction.ProductLocation , 
       
        //当前加工者
        RealTimeDynamicProduction.Worker ,
        //当前工序
        RealTimeDynamicProduction.CurrentOperation , 
        //更新日期
        RealTimeDynamicProduction.UpdateDate , 
        //当前工序编号
        RealTimeDynamicProduction.OperationNumber , 
        //当前工序表ObjectId
        "F0000070" , 
        //当前工序表schemaCode
        "F0000071"
        };

        //填充数据空值
        Tools.BizOperation.FillNullActivex(target);
        Copy(target, current, mpaping);
    }



    //辗环进度同步逻辑
    public static Hashtable RollingRingProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询辗环工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity158", "外协流程");
                            activityCodes.Add("Activity118", "质量异常");
                            activityCodes.Add("Activity162", "其它异常");
                            activityCodes.Add("Activity156", "质量处理审批");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "3")
                        {
                            targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "热加工完成";
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "2z.辗环/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.DemandPeriodOfThisOperationRingRolling] + string.Empty != string.Empty)
                        {
                            //本工序需求期-辗环
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.DemandPeriodOfThisOperationRingRolling] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-辗环与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //辗环-延期天数
                                targetObj[RealTimeDynamicProduction.RingRollingDelayDays] = Math.Abs(days);
                                //辗环-提前天数
                                targetObj[RealTimeDynamicProduction.RingRollingAdvanceDays] = "0";
                                //辗环-准时完工
                                targetObj[RealTimeDynamicProduction.RingRollingOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //辗环-延期天数 
                                targetObj[RealTimeDynamicProduction.RingRollingDelayDays] = "0";
                                //辗环-提前天数
                                targetObj[RealTimeDynamicProduction.RingRollingAdvanceDays] = days;
                                //辗环-准时完工
                                targetObj[RealTimeDynamicProduction.RingRollingOnTimeCompletion] = "1";
                            }
                        }

                        //辗环-完成日期
                        targetObj[RealTimeDynamicProduction.RingRollingCompletionDate] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = { "a.升温中", "b.待上机", "c.辗环中", "d.冷却中", "e.确认调整意见", "f.审批确认", "i.质量处理审批", "k.质量异议", "g.其它异常", "l.外协管理" };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] =
                                    "3" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "3y.辗环/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[RollingRing.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[RollingRing.WorkshopLocation] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[RollingRing.ProductLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[RollingRing.WorkshopLocation] + " " + item[RollingRing.ProductLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "辗环";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "3";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
                H3.DataModel.BizObject another = GetAnotherProcess(engine, (string)targetObj[RollingRing.OperationSchedule]);
                SyncRollingRingProgress(another, targetObj);
            }
        }
        return workSteps;
    }
    public static void SyncRollingRingProgress(H3.DataModel.BizObject target, H3.DataModel.BizObject current)
    {
        if (target == null) { return; }
        List<string> mpaping = new List<string>
        {
        //市场需求达成进度
        RealTimeDynamicProduction.MarketDemandAchievementProgress ,
        //当前工步
        RealTimeDynamicProduction.CurrentWorkStep ,
        //热加工需求达成进度
        RealTimeDynamicProduction.HotProcessingDemandAchievementProgress ,
        //辗环-延期天数
        RealTimeDynamicProduction.RingRollingDelayDays ,
        //辗环-提前天数
        RealTimeDynamicProduction.RingRollingAdvanceDays ,
        //辗环-准时完工
        RealTimeDynamicProduction.RingRollingOnTimeCompletion ,

        //辗环-完成日期
        RealTimeDynamicProduction.RingRollingCompletionDate ,

        //车间位置
        RealTimeDynamicProduction.Workshop ,
        //区域位置
        RealTimeDynamicProduction.Area ,
        //产品位置
        RealTimeDynamicProduction.ProductLocation ,

        //当前加工者
        RealTimeDynamicProduction.Worker ,
        //当前工序
        RealTimeDynamicProduction.CurrentOperation ,
        //更新日期
        RealTimeDynamicProduction.UpdateDate ,
        //当前工序编号
        RealTimeDynamicProduction.OperationNumber ,
        //当前工序表ObjectId
        "F0000070" ,
        //当前工序表schemaCode
        "F0000071"
         };
        //填充数据空值
        Tools.BizOperation.FillNullActivex(target);
        Copy(target, current, mpaping);
    }



    //热处理进度同步逻辑
    public static Hashtable HeatTreatmentProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询热处理工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity201", "外协流程");
                            activityCodes.Add("Activity132", "质量异常");
                            activityCodes.Add("Activity217", "其它异常");
                            activityCodes.Add("Activity156", "质量处理审批");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "4")
                        {
                            //市场需求达成进度
                            targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "热加工完成";
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "4z.热处理/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.DemandPeriodOfThisOperationHeatTreatment] + string.Empty != string.Empty)
                        {
                            //本工序需求期-热处理
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.DemandPeriodOfThisOperationHeatTreatment] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-热处理与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //热处理-延期天数
                                targetObj[RealTimeDynamicProduction.HeatTreatmentDelayDays] = Math.Abs(days);
                                //热处理-提前天数
                                targetObj[RealTimeDynamicProduction.HeatTreatmentAdvanceDays] = "0";
                                //热处理-准时完工
                                targetObj[RealTimeDynamicProduction.HeatTreatmentOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //热处理-延期天数 
                                targetObj[RealTimeDynamicProduction.HeatTreatmentDelayDays] = "0";
                                //热处理-提前天数
                                targetObj[RealTimeDynamicProduction.HeatTreatmentAdvanceDays] = days;
                                //热处理-准时完工
                                targetObj[RealTimeDynamicProduction.HeatTreatmentOnTimeCompletion] = "1";
                            }
                        }

                        //热处理-完成日期
                        targetObj[RealTimeDynamicProduction.HeatTreatmentCompletionDate] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = {
                            "a.待转运", "b.装炉前待检验", "c.待装炉", "d.待升温", "e.处理中", "f.待出炉",
                            "g.待精整", "h.冷却中", "i.回火待装炉", "j.回火中", "k.回火冷却中", "l.确认调整意见", "m.审批确认",
                            "i.质量处理审批", "k.质量异议", "g.其它异常", "l.外协管理"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] =
                                    "4" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "4y.热处理/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[HeatTreatment.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[HeatTreatment.WorkshopLocation] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[HeatTreatment.ProductLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[HeatTreatment.WorkshopLocation] + " " + item[HeatTreatment.ProductLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "热处理";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "4";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }

                H3.DataModel.BizObject another = GetAnotherProcess(engine, (string)targetObj[HeatTreatment.OperationSchedule]);
                SyncHeatTreatmentProgress(another, targetObj);
            }
        }
        return workSteps;
    }
    public static void SyncHeatTreatmentProgress(H3.DataModel.BizObject target, H3.DataModel.BizObject current)
    {
        if (target == null) { return; }
        List<string> mpaping = new List<string>
        {
        //市场需求达成进度
        RealTimeDynamicProduction.MarketDemandAchievementProgress,
        //当前工步
        RealTimeDynamicProduction.CurrentWorkStep,
        //热加工需求达成进度
        RealTimeDynamicProduction.HotProcessingDemandAchievementProgress,
        //热处理-延期天数
        RealTimeDynamicProduction.HeatTreatmentDelayDays,
        //热处理-提前天数
        RealTimeDynamicProduction.HeatTreatmentAdvanceDays,
        //热处理-准时完工
        RealTimeDynamicProduction.HeatTreatmentOnTimeCompletion,
        //热处理-完成日期
        RealTimeDynamicProduction.HeatTreatmentCompletionDate,
       
        //车间位置
        RealTimeDynamicProduction.Workshop,
        //区域位置
        RealTimeDynamicProduction.Area,
        //产品位置
        RealTimeDynamicProduction.ProductLocation,
        //当前加工者
        RealTimeDynamicProduction.Worker,
        //当前工序
        RealTimeDynamicProduction.CurrentOperation,        
        //更新日期
        RealTimeDynamicProduction.UpdateDate,
        //当前工序编号
        RealTimeDynamicProduction.OperationNumber,
        //当前工序表ObjectId
        "F0000070",
        //当前工序表schemaCode
        "F0000071"
        };
        //填充数据空值
        Tools.BizOperation.FillNullActivex(target);
        Copy(target, current, mpaping);
    }



    //毛坯进度同步逻辑
    public static Hashtable RoughCastProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询毛坯工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity282", "外协流程");
                            activityCodes.Add("Activity279", "理化质量评审");
                            activityCodes.Add("Activity153", "机加质量处理");
                            activityCodes.Add("Activity260", "取样子流程");
                            activityCodes.Add("Activity96", "质量异常");
                            activityCodes.Add("Activity97", "需求异常");
                            activityCodes.Add("Activity297", "其它异常");
                            activityCodes.Add("Activity299", "流转异常");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "5")
                        {
                            //市场需求达成进度
                            targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "毛坯完成";
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "5z.毛坯/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.DemandPeriodOfThisOperationBlank] + string.Empty != string.Empty)
                        {
                            //本工序需求期-毛坯
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.DemandPeriodOfThisOperationBlank] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-毛坯与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //毛坯-延期天数
                                targetObj[RealTimeDynamicProduction.BlankDelayDays] = Math.Abs(days);
                                //毛坯-提前天数
                                targetObj[RealTimeDynamicProduction.BlankAdvanceDays] = "0";
                                //毛坯-准时完工
                                targetObj[RealTimeDynamicProduction.BlankOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //毛坯-延期天数 
                                targetObj[RealTimeDynamicProduction.BlankDelayDays] = "0";
                                //毛坯-提前天数
                                targetObj[RealTimeDynamicProduction.BlankAdvanceDays] = days;
                                //毛坯-准时完工
                                targetObj[RealTimeDynamicProduction.BlankOnTimeCompletion] = "1";
                            }
                        }

                        //毛坯-完成日期
                        targetObj[RealTimeDynamicProduction.BlankCompletionDate] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = {
                            "a.待转运", "b.待切割", "c.待检验", "d.忽略理化结果", "e.待理化结果", "f.确认调整意见",
                            "g.审批确认", "h.外协流程", "i.机加质量处理", "j.取样子流程", "k.理化质量评审", "l.质量异常", "m.其它异常",
                            "i.需求异常", "k.流转异常"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] =
                                    "5" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "5y.毛坯/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[RoughCast.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[RoughCast.CurrentWorkshop] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[RoughCast.CurrentLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[RoughCast.CurrentWorkshop] + " " + item[RoughCast.CurrentLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "毛坯";
                        //市场需求达成进度
                        targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "热加工完成";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "5";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
                H3.DataModel.BizObject another = GetAnotherProcess(engine, (string)targetObj[RoughCast .OperationSchedule]);
                SyncRoughCastProgress(another, targetObj);
            }
        }
        return workSteps;
    }

    public static void SyncRoughCastProgress(H3.DataModel.BizObject target, H3.DataModel.BizObject current)
    {
        if (target == null) { return; }

        List<string> mpaping = new List<string>
        {

        //市场需求达成进度
        RealTimeDynamicProduction.MarketDemandAchievementProgress,
        //当前工步
        RealTimeDynamicProduction.CurrentWorkStep,
        //热加工需求达成进度
        RealTimeDynamicProduction.HotProcessingDemandAchievementProgress,
        //毛坯-延期天数
        RealTimeDynamicProduction.BlankDelayDays,
        //毛坯-提前天数
        RealTimeDynamicProduction.BlankAdvanceDays,
        //毛坯-准时完工
        RealTimeDynamicProduction.BlankOnTimeCompletion,
        //毛坯-完成日期
        RealTimeDynamicProduction.BlankCompletionDate,

        //车间位置
        RealTimeDynamicProduction.Workshop,
        //区域位置
        RealTimeDynamicProduction.Area,
        //产品位置
        RealTimeDynamicProduction.ProductLocation,
        //当前加工者
        RealTimeDynamicProduction.Worker,              
        //更新日期
        RealTimeDynamicProduction.UpdateDate,
        //当前工序编号
        RealTimeDynamicProduction.OperationNumber,
        //当前工序表ObjectId
        "F0000070",
        //当前工序表schemaCode
        "F0000071"
             };
        //填充数据空值
        Tools.BizOperation.FillNullActivex(target);
        Copy(target, current, mpaping);
    }



    //粗车进度同步逻辑
    public static Hashtable RoughingProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询粗车工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                            //item["CurrentAuditors"] = activeinfo.Rows[0]["Participant"] + string.Empty; 
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity165", "外协流程");
                            activityCodes.Add("Activity96", "质量异常");
                            activityCodes.Add("Activity97", "需求异常");
                            activityCodes.Add("Activity190", "其它异常");
                            activityCodes.Add("Activity193", "流转异常");
                            activityCodes.Add("Activity179", "机加质量处理");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }

                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                    //CurrentAuditors.SetAuditors(engine,item.WorkflowInstanceId,item.Schema.SchemaCode,item.ObjectId);
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "6")
                        {
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "6z.粗车/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.RoughTurningPlanCompletionTime] + string.Empty != string.Empty)
                        {
                            //本工序需求期-粗车
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.RoughTurningPlanCompletionTime] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-粗车与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //粗车-延期天数
                                targetObj[RealTimeDynamicProduction.RoughTurningDelayDays] = Math.Abs(days);
                                //粗车-提前天数
                                targetObj[RealTimeDynamicProduction.RoughTurningAdvanceDays] = "0";
                                //粗车-准时完工
                                targetObj[RealTimeDynamicProduction.RoughTurningOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //粗车-延期天数 
                                targetObj[RealTimeDynamicProduction.RoughTurningDelayDays] = "0";
                                //粗车-提前天数
                                targetObj[RealTimeDynamicProduction.RoughTurningAdvanceDays] = days;
                                //粗车-准时完工
                                targetObj[RealTimeDynamicProduction.RoughTurningOnTimeCompletion] = "1";
                            }
                        }

                        //粗车-完成日期
                        targetObj[RealTimeDynamicProduction.RoughTurningCompletionDatePeriod] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = {
                            "a.待转运", "b.待四面光", "c.四面光中", "d.待上机", "e.粗车中", "f.待探伤",
                            "g.待检验", "h.确认调整意见", "i.审批确认", "j.外协流程", "k.机加质量处理", "l.质量异常", "m.其它异常",
                            "i.需求异常", "k.流转异常"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] =
                                    "6" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "6y.粗车/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[Roughing.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[Roughing.CurrentWorkshop] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[Roughing.CurrentLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[Roughing.CurrentWorkshop] + " " + item[Roughing.CurrentLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "粗车";
                        //市场需求达成进度
                        targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "毛坯完成";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "6";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
            }
        }
        return workSteps;
    }


    //精车进度同步逻辑
    public static Hashtable FinishingProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询精车工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity107", "外协流程");
                            activityCodes.Add("Activity88", "机加质量处理");
                            activityCodes.Add("Activity59", "质量异常");
                            activityCodes.Add("Activity60", "需求异常");
                            activityCodes.Add("Activity126", "其它异常");
                            activityCodes.Add("Activity129", "流转异常");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                    //CurrentAuditors.SetAuditors(engine,item.WorkflowInstanceId,item.Schema.SchemaCode,item.ObjectId);
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "7")
                        {
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "7z.精车/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.FinishTurningPlannedCompletionTime] + string.Empty != string.Empty)
                        {
                            //本工序需求期-精车
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.FinishTurningPlannedCompletionTime] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-精车与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //精车-延期天数
                                targetObj[RealTimeDynamicProduction.FinishTurningDelayDays] = Math.Abs(days);
                                //精车-提前天数
                                targetObj[RealTimeDynamicProduction.FinishTurningAdvanceDays] = "0";
                                //精车-准时完工
                                targetObj[RealTimeDynamicProduction.FinishTurningOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //精车-延期天数 
                                targetObj[RealTimeDynamicProduction.FinishTurningDelayDays] = "0";
                                //精车-提前天数
                                targetObj[RealTimeDynamicProduction.FinishTurningAdvanceDays] = days;
                                //精车-准时完工
                                targetObj[RealTimeDynamicProduction.FinishTurningOnTimeCompletion] = "1";
                            }
                        }

                        //精车-完成日期
                        targetObj[RealTimeDynamicProduction.FinishTurningCompletionDate] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = {
                            "a.待转运", "b.待上机", "c.精车中", "d.待探伤", "e.待检验", "f.待探伤",
                            "g.待检验", "h.确认调整意见", "i.审批确认", "j.外协流程", "k.机加质量处理", "l.质量异常", "m.其它异常",
                            "i.需求异常", "k.流转异常"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] =
                                    "7" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "7y.精车/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[Finishing.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[Finishing.CurrentWorkshop] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[Finishing.CurrentLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[Finishing.CurrentWorkshop] + " " + item[Finishing.CurrentLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "精车";
                        //市场需求达成进度
                        targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "毛坯完成";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "7";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
            }
        }
        return workSteps;
    }
    //钻孔进度同步逻辑
    public static Hashtable DrillProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询钻孔工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity144", "外协流程");
                            activityCodes.Add("Activity140", "机加质量处理");
                            activityCodes.Add("Activity91", "质量异常");
                            activityCodes.Add("Activity93", "需求异常");
                            activityCodes.Add("Activity153", "其它异常");
                            activityCodes.Add("Activity156", "流转异常");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach (string key in activityCodes.Keys)
                            {
                                if (key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "8")
                        {
                            //市场需求达成进度
                            targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "冷加工完成";
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "8z.钻孔/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.DrillingPlannedCompletionTime] + string.Empty != string.Empty)
                        {
                            //本工序需求期-钻孔
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.DrillingPlannedCompletionTime] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-钻孔与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //钻孔-延期天数
                                targetObj[RealTimeDynamicProduction.DrillingDelayDays] = Math.Abs(days);
                                //钻孔-提前天数
                                targetObj[RealTimeDynamicProduction.DrillingAdvanceDays] = "0";
                                //钻孔-准时完工
                                targetObj[RealTimeDynamicProduction.DrillingOnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //钻孔-延期天数 
                                targetObj[RealTimeDynamicProduction.DrillingDelayDays] = "0";
                                //钻孔-提前天数
                                targetObj[RealTimeDynamicProduction.DrillingAdvanceDays] = days;
                                //钻孔-准时完工
                                targetObj[RealTimeDynamicProduction.DrillingOnTimeCompletion] = "1";
                            }
                        }

                        //钻孔-完成日期
                        targetObj[RealTimeDynamicProduction.DrillingCompletionDate] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = {
                            "a.待转运", "b.待上机", "c.钻孔中", "d.待毛刺", "e.待检验", "f.待检验",
                            "g.待划线绞扣", "h.绞扣检验", "i.确认调整意见", "j.审批确认", "k.外协流程", "l.机加质量处理",
                            "m.质量异常", "n.需求异常", "o.其它异常", "p.流转异常"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] =
                                    "8" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "8y.钻孔/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[Drill.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[Drill.CurrentWorkshop] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[Drill.CurrentLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[Drill.CurrentWorkshop] + " " + item[Drill.CurrentLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "钻孔";
                        //市场需求达成进度
                        targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "毛坯完成";
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "8";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
            }
        }
        return workSteps;
    }
    //成品进度同步逻辑
    public static Hashtable FinishedStoreProgress(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询成品工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                        else
                        {
                            /*
                            Hashtable activityCodes = new Hashtable();
                            activityCodes.Add("Activity144", "外协流程");

                            //当前Token
                            H3.Workflow.Instance.IToken tok = engine.WorkflowInstanceManager.GetWorkflowInstance(instance.InstanceId).GetLastToken();

                            foreach(string key in activityCodes.Keys)
                            {
                                if(key == tok.Activity)
                                {
                                    item[currentWorkStep] = activityCodes[tok.Activity] + string.Empty;
                                }
                            }
                            */
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    //工序计划数据Id
                    string planObjId = targetObj[RealTimeDynamicProduction.OperationPlanTable] + string.Empty;
                    //工序计划数据对象
                    H3.DataModel.BizObject planObj = planObjId != string.Empty ? Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, planObjId) : null;

                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "9")
                        {
                            //市场需求达成进度
                            targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "发运完成";
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "9z.成品/结束";
                        }

                        //工序需求期不为空值时，计算指标
                        if (planObj != null && planObj[ABCDProcessPlan.FinishedProductDemandPeriod] + string.Empty != string.Empty)
                        {
                            //本工序需求期-成品
                            DateTime planDate = Convert.ToDateTime(planObj[ABCDProcessPlan.FinishedProductDemandPeriod] + string.Empty);
                            //生产完成日期
                            DateTime finishDate = Convert.ToDateTime(instance.FinishTime.ToString());
                            //本工序需求期-成品与生产完成日期的差值
                            TimeSpan delayTime = planDate.Subtract(finishDate);
                            //换算为天数
                            double days = delayTime.TotalDays;

                            if (days < 0)
                            {   //成品-延期天数
                                targetObj[RealTimeDynamicProduction.ProductionDelayDays] = Math.Abs(days);
                                //成品-提前天数
                                targetObj[RealTimeDynamicProduction.ProductionLeadDays] = "0";
                                //成品-准时完工
                                targetObj[RealTimeDynamicProduction.OnTimeCompletion] = "0";
                            }

                            if (days >= 0)
                            {
                                //成品-延期天数 
                                targetObj[RealTimeDynamicProduction.ProductionDelayDays] = "0";
                                //成品-提前天数
                                targetObj[RealTimeDynamicProduction.ProductionLeadDays] = days;
                                //成品-准时完工
                                targetObj[RealTimeDynamicProduction.OnTimeCompletion] = "1";
                            }
                        }

                        //成品-完成日期
                        targetObj[RealTimeDynamicProduction.ProductionCompletionDate] = instance.FinishTime.ToString();
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = {
                            "a.待终检", "b.待转运", "c.待指令", "d.待包装", "e.待发运", "f.审批确认"
                        };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] =
                                    "9" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.ColdProcessingDemandCompletionSchedule] = "9y.成品/" + item[currentWorkStep];
                            }
                        }

                        {
                            //厂区位置
                            //targetObj[RealTimeDynamicProduction.PlantArea] = item[FinishedStore.FactoryLocation] + string.Empty;
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[FinishedStore.WorkshopLocation] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[FinishedStore.ProductLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[FinishedStore.WorkshopLocation] + " " + item[FinishedStore.ProductLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "成品";
                        //市场需求达成进度
                        targetObj[RealTimeDynamicProduction.MarketDemandAchievementProgress] = "冷加工完成";
                        //生产完成日期
                        targetObj[RealTimeDynamicProduction.ProductionCompletionDate] = item[FinishedStore.CreationTime] + string.Empty;
                        //冷加工完成日期
                        targetObj[RealTimeDynamicProduction.ColdWorkingCompletionDate] = item[FinishedStore.CreationTime] + string.Empty;
                        //加工设备  注释原因：因改为多阶段子表记录，导致获取失效
                        //targetObj[RealTimeDynamicProduction.ProcessingEquipment] = item[EquipmentNumber] + string.Empty;
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "9";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                        //填充数据空值
                        Tools.BizOperation.FillNullActivex(targetObj);
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
            }
        }
        return workSteps;
    }
    //调度进度同步逻辑
    public static Hashtable ManualRegulationProcess(H3.IEngine engine, string tableCode, string currentWorkStep)
    {
        //查询人工调整工序今天修改和新建的数据
        H3.DataModel.BizObject[] originObj = ProgressManagement.GetObjects(engine, tableCode);
        //当前工步集合
        Hashtable workSteps = new Hashtable();

        if (originObj != null)
        {    //遍历今天修改和新建的数据
            foreach (H3.DataModel.BizObject item in originObj)
            {
                //获取工序流程实例
                H3.Workflow.Instance.WorkflowInstance instance = engine.WorkflowInstanceManager.GetWorkflowInstance(item.WorkflowInstanceId);
                //获取进度管理数据对象
                H3.DataModel.BizObject targetObj = Tools.BizOperation.Load(engine, RealTimeDynamicProduction.TableCode, item["Progress"] + string.Empty);

                DataTable activeinfo = null;

                if (instance != null)
                {
                    if (instance.RunningActivties != null && instance.RunningActivties.Length > 0)
                    {                               //查询当前活动节点信息
                        activeinfo = engine.Query.QueryWorkItemDisplayAndParticipant
                            (new string[] { instance.InstanceId }, WorkItemState.Unfinished);
                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            item[currentWorkStep] = activeinfo.Rows[0]["activitydisplayname"] + string.Empty;
                        }
                    }

                    if (instance.IsFinished)
                    {         //本工序当前工步
                        item[currentWorkStep] = "结束";
                    }

                    workSteps.Add(item.ObjectId, item[currentWorkStep] + string.Empty);

                    item.Update();
                }

                if (targetObj != null)
                {
                    if (instance.IsFinished)
                    {
                        if (targetObj[RealTimeDynamicProduction.OperationNumber] + string.Empty == "0")
                        {
                            //当前工步
                            targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "z.结束";
                            targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "0z.人工调整/结束";
                        }
                    }

                    if (instance.IsUnfinished)
                    {
                        string[] activties = { "a.发起节点", "b.物流组" };

                        foreach (string one in activties)
                        {   //当前工步、热加工需求达成进度
                            if (one.Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = one;
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] =
                                    "0" + one.Remove(2) + targetObj[RealTimeDynamicProduction.CurrentOperation] + "/" + item[currentWorkStep];
                            }

                            if (!(targetObj[RealTimeDynamicProduction.CurrentWorkStep] + string.Empty).Contains(item[currentWorkStep] + string.Empty))
                            {
                                targetObj[RealTimeDynamicProduction.CurrentWorkStep] = "y." + item[currentWorkStep];
                                targetObj[RealTimeDynamicProduction.HotProcessingDemandAchievementProgress] = "0y.人工调整/" + item[currentWorkStep];
                            }
                        }

                        {
                            //车间位置
                            targetObj[RealTimeDynamicProduction.Workshop] = item[ManualAdjustProcess.WorkshopLocation] + string.Empty;
                            //区域位置
                            targetObj[RealTimeDynamicProduction.Area] = item[ManualAdjustProcess.ProductLocation] + string.Empty;
                            //产品位置
                            targetObj[RealTimeDynamicProduction.ProductLocation] = item[ManualAdjustProcess.WorkshopLocation] + " " + item[ManualAdjustProcess.WorkshopLocation];
                        }

                        if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                        {
                            //清空工人记录
                            targetObj[RealTimeDynamicProduction.Worker] = string.Empty;
                            //重新获取工人
                            foreach (System.Data.DataRow workItem in activeinfo.Rows)
                            {
                                H3.Organization.User user = engine.Organization.GetUnit(workItem["Participant"] + string.Empty) as H3.Organization.User;
                                //获取当前加工者
                                targetObj[RealTimeDynamicProduction.Worker] += user != null ? user.Name + ";" : string.Empty;
                            }
                        }
                        //当前工序
                        targetObj[RealTimeDynamicProduction.CurrentOperation] = "人工调整";
                        //更新日期
                        targetObj[RealTimeDynamicProduction.UpdateDate] = DateTime.Now;
                        //当前工序编号
                        targetObj[RealTimeDynamicProduction.OperationNumber] = "0";
                        //当前工序表ObjectId
                        targetObj["F0000070"] = item.ObjectId;
                        //当前工序表schemaCode
                        targetObj["F0000071"] = item.Schema.SchemaCode;
                    }
                    //更新产品实时情况表单
                    targetObj.Update();
                }
            }
        }
        return workSteps;
    }

    //数据筛选
    public static H3.DataModel.BizObject[] GetObjects(H3.IEngine iengine, string sChemaCode)
    {
        //构建过滤器
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        //过去一天的日期
        DateTime today = System.DateTime.Today;
        //["ModifiedTime"]-[修改时间]-A   筛选昨天与今天修改过的订单
        Tools.Filter.Or(filter, "ModifiedTime", H3.Data.ComparisonOperatorType.Above, today);
        //获取业务对象集合
        H3.DataModel.BizObject[] bizObject = Tools.BizOperation.GetList(iengine, sChemaCode, filter);

        return bizObject;
    }

    private static void Copy(H3.DataModel.BizObject target, H3.DataModel.BizObject source, List<string> mapping)
    {
        foreach (var map in mapping)
        {
            target[map] = source[map];
        }
        target.Update();//更新产品实时情况表单
    }
    /// <summary>
    /// 由计划表找出Another的进度
    /// </summary>
    /// <param name="objPlan">工序计划表</param>
    /// <returns></returns>
    private static H3.DataModel.BizObject GetAnotherProcess(H3.IEngine engine, string  selfPlanObjectId)
    {
        //工序计划表
        H3.DataModel.BizObject objPlan = Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, selfPlanObjectId);
        if (objPlan == null) return null;
        //F0000226:工序计划表中的双轧关联表单字段名
        H3.DataModel.BizObject objAnotherPlan = Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, objPlan["F0000226"] + string.Empty);
        if (objAnotherPlan == null) return null;
        H3.DataModel.BizObject objAnotherProcess = Tools.BizOperation.Load(engine, ABCDProcessPlan.TableCode, objAnotherPlan["Progress"] + string.Empty);
        return objAnotherProcess;
    }
}
