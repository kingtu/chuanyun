
using System;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;

public class D001419Son0vyw9n413fhgqud7zeeocz2 : H3.SmartForm.SmartFormController
{
    H3.DataModel.BizObject me;
    string userName = ""; //当前用户





    public D001419Son0vyw9n413fhgqud7zeeocz2(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        // 派工开关
        string dipatchFlag = this.Request.BizObject["F0000245"] + string.Empty;
        if (dipatchFlag == "开")
        {
            //DispatchLogics();
        }
        if (actionName == "Activity305")
        {
            SyncSalary();
        }

        base.OnSubmit(actionName, postValue, response);
    }
    private void SyncSalary()
    {
        BizObject b = Request.BizObject;
        if (b[TransferRecord_Transshiper1] + string .Empty != "")
        {
            CreateSalary(b[TransferRecord_Transshiper1], 
                         b[TransferRecord_AllocationProportion1],
                         b[TransferRecord_TransshipmentSalary1],
                         b[TransferRecord_UnitPrice],
                         b[TransferRecord_OrderSpecificationNumber],
                         b[TransferRecord_WorkpieceNumber],
                         b[TransferRecord_Id],
                         b["CreationTime"],
                         b[TransferRecord_DepartmentCode],
                         b[TransferRecord_DataCode],
                         b[TransferRecord_VersionNumber]);
        }
        if (b[TransferRecord_Transshiper2] + string.Empty != "")
        {
            CreateSalary(b[TransferRecord_Transshiper2],
                         b[TransferRecord_AllocationProportion2],
                         b[TransferRecord_TransshipmentSalary2],
                         b[TransferRecord_UnitPrice],
                         b[TransferRecord_OrderSpecificationNumber],
                         b[TransferRecord_WorkpieceNumber],
                         b[TransferRecord_Id],
                         b["CreationTime"],
                         b[TransferRecord_DepartmentCode],
                         b[TransferRecord_DataCode],
                         b[TransferRecord_VersionNumber]);
        }
        if (b[TransferRecord_Transshiper3] + string.Empty != "")
        {
            CreateSalary(b[TransferRecord_Transshiper3],
                         b[TransferRecord_AllocationProportion3],
                         b[TransferRecord_TransshipmentSalary3],
                         b[TransferRecord_UnitPrice],
                         b[TransferRecord_OrderSpecificationNumber],
                         b[TransferRecord_WorkpieceNumber],
                         b[TransferRecord_Id],
                         b["CreationTime"],
                         b[TransferRecord_DepartmentCode],
                         b[TransferRecord_DataCode],
                         b[TransferRecord_VersionNumber] );
        }
        if (b[TransferRecord_Transshiper4] + string.Empty != "")
        {
            CreateSalary(b[TransferRecord_Transshiper4],
                         b[TransferRecord_AllocationProportion4],
                         b[TransferRecord_TransshipmentSalary4],
                         b[TransferRecord_UnitPrice],
                         b[TransferRecord_OrderSpecificationNumber],
                         b[TransferRecord_WorkpieceNumber],
                         b[TransferRecord_Id],
                         b["CreationTime"],
                         b[TransferRecord_DepartmentCode],
                         b[TransferRecord_DataCode],
                         b[TransferRecord_VersionNumber]);
        }
    }
    private void CreateSalary(object Transshiper, object Quantity, object Amount, object Wages,
                              object OrderSpecificationNumber, object WorkpieceNumber, object Id, 
                              object OccurrenceDate, object DepartmentName, object DataCode,
                              object VersionNumber)
    {
        BizObject TSalary = Tools.BizOperation.New(this.Engine, TSalaryTable_TableCode);
        TSalary[TSalaryTable_Transshiper] = Transshiper;      
        TSalary[TSalaryTable_Quantity] = Quantity;
        TSalary[TSalaryTable_Amount] = Amount;
        TSalary[TSalaryTable_Wages] = Wages;

        TSalary[TSalaryTable_OrderSpecificationNumber] = OrderSpecificationNumber;
        TSalary[TSalaryTable_WorkpieceNumber] = WorkpieceNumber;
        TSalary[TSalaryTable_Id] =Id;
        TSalary[TSalaryTable_OrderSpecificationNumber] = OrderSpecificationNumber;

        TSalary[TSalaryTable_OccurrenceDate] = OccurrenceDate;
        TSalary[TSalaryTable_DepartmentName] = DepartmentName;

        TSalary[TSalaryTable_DataCode] = DataCode;
        TSalary[TSalaryTable_VersionNumber] = VersionNumber;
        TSalary.Create();


    }

    ///**
    //* --Author: nkx
    //* 派工子表信息
    //* 
    //*/
    //public void DispatchLogics()
    //{
    //    if ((bool)me[TransferRecord.WorkshopLocationChange])
    //    {
    //        //父流程实例ID
    //        string parentInstanceId = this.Request.WorkflowInstance.ParentInstanceId;
    //        //获取父流程实例对象
    //        H3.Workflow.Instance.WorkflowInstance parentInstance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(parentInstanceId);
    //        string WorkflowDisplayName = parentInstance.WorkflowDisplayName;
    //        switch (WorkflowDisplayName)
    //        {
    //            case "毛坯-取样理化":
    //                //取样派工信息
    //                DispatchLogicsSampling();
    //                break;
    //            case "粗车":
    //                //粗车派工信息
    //                DispatchLogicsRoughing();
    //                break;
    //            case "精车":
    //                //精车派工信息
    //                DispatchLogicsFinishing();
    //                break;
    //            case "钻孔":
    //                //钻孔派工信息
    //                DispatchLogicsDrill();
    //                break;
    //        }
    //    }
    //}

    ///**
    //* --Author: nkx
    //* 转运重新读取取样派工信息
    //*/
    //public void DispatchLogicsSampling()
    //{
    //    //转运取样
    //    H3.DataModel.BizObject processObj = me;
    //    //派工计划
    //    string dispatchId = processObj[TransferRecord.DispatchingPlan] + string.Empty;
    //    //派工表
    //    H3.DataModel.BizObject dispatchObj = Tools.BizOperation.Load(this.Engine, Dispatchs.TableCode, dispatchId);
    //    //派工表子表信息
    //    H3.DataModel.BizObject[] dispatchSubs = dispatchObj[DispatchSamplingSubTable.TableCode] as H3.DataModel.BizObject[];
    //    //转运取样派工信息子表信息
    //    H3.DataModel.BizObject[] disProcessSubs = processObj[TransferRecordSamplingAssignmentInformation.TableCode] as H3.DataModel.BizObject[];

    //    List<string> workers = new List<string>();
    //    List<string> namestatus = new List<string>();
    //    bool flagss = true;

    //    //判断转运取样派工信息表子表有没有值，有值则清空子表
    //    if (disProcessSubs != null && disProcessSubs.Length > 0)
    //    {
    //        for (int i = (disProcessSubs.Length - 1); i >= 0; i--)
    //        {
    //            //判断子表中“任务状态”控件中是否有值
    //            if (disProcessSubs[i][TransferRecordSamplingAssignmentInformation.TaskStatus] + string.Empty == string.Empty)
    //            {
    //                //清空子表最后一行
    //                Tools.BizOperation.DeleteChildBizObject(this.Engine, processObj, TransferRecordSamplingAssignmentInformation.TableCode);
    //            }
    //            else
    //            {
    //                if (disProcessSubs[i][TransferRecordSamplingAssignmentInformation.TaskStatus] + string.Empty != "已完成")
    //                {
    //                    string[] taskWorkers = dispatchSubs[i][DispatchSamplingSubTable.Name] as string[];
    //                    foreach (string worker in taskWorkers)
    //                    {
    //                        workers.Add(worker);
    //                    }
    //                    flagss = false;
    //                }
    //                if (disProcessSubs[i][TransferRecordSamplingAssignmentInformation.TaskStatus] + string.Empty == "已完成")
    //                {
    //                    //已完成时把任务名称添加进namestatus中
    //                    namestatus.Add(disProcessSubs[i][TransferRecordSamplingAssignmentInformation.DispatchTask] + string.Empty);
    //                }
    //            }
    //        }
    //    }
    //    //再次赋值转运取样派工信息表子表信息
    //    disProcessSubs = processObj[TransferRecordSamplingAssignmentInformation.TableCode] as H3.DataModel.BizObject[];

    //    //判断派工表子表有没有值
    //    if (dispatchSubs != null && dispatchSubs.Length > 0 && flagss)
    //    {
    //        processObj[TransferRecord.SamplingRestrictionDispatchSequence] = (bool)dispatchObj[Dispatchs.SamplingUnlimitedDispatchSequence];
    //        for (int i = 0; i < dispatchSubs.Length; i++)
    //        {
    //            bool flag = false;
    //            //判断转运取样派工信息表子表有没有值
    //            if (disProcessSubs != null && disProcessSubs.Length > 0)
    //            {
    //                for (int j = 0; j < disProcessSubs.Length; j++)
    //                {
    //                    //对比转运取样派工信息子表与派工表的“派工任务”是否一致
    //                    if (disProcessSubs[j][TransferRecordSamplingAssignmentInformation.DispatchTask] + string.Empty == dispatchSubs[i][DispatchSamplingSubTable.TaskName] + string.Empty)
    //                    {
    //                        flag = true;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (flag == true)
    //            {
    //                continue;
    //            }
    //            //转运取样派工信息派工子表对象
    //            H3.DataModel.BizObject disProcessSubsObj = Tools.BizOperation.New(this.Engine, TransferRecordSamplingAssignmentInformation.TableCode);
    //            //派工任务
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.DispatchTask] = dispatchSubs[i][DispatchSamplingSubTable.TaskName] + string.Empty;
    //            //派工人员
    //            string[] taskWorkers = dispatchSubs[i][DispatchSamplingSubTable.Name] as string[];
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.Dispatcher] = taskWorkers;
    //            //设备名称
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.EquipmentName] = dispatchSubs[i][DispatchSamplingSubTable.EquipmentName] + string.Empty;
    //            //派工量
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.DispatchQuantity] = dispatchSubs[i][DispatchSamplingSubTable.ProcessingQuantity] + string.Empty;
    //            //完成量
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.CompletedQuantity] = dispatchSubs[i][DispatchSamplingSubTable.ProcessedQuantity] + string.Empty;
    //            //工时
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.SamplingManHour] = dispatchSubs[i][DispatchSamplingSubTable.ManHour] + string.Empty;
    //            //下屑量
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.ChipQuantity] = dispatchSubs[i][DispatchSamplingSubTable.TheAmountOfScrap] + string.Empty;
    //            //设备编号
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.EquipmentNumber] = dispatchSubs[i][DispatchSamplingSubTable.EquipmentNumber] + string.Empty;
    //            //设备类型
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.EquipmentType] = dispatchSubs[i][DispatchSamplingSubTable.EquipmentType] + string.Empty;
    //            //车间名称
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.SamplingWorkshopName] = dispatchSubs[i][DispatchSamplingSubTable.NameSamplingWorkshop] + string.Empty;
    //            //车间位置
    //            disProcessSubsObj[TransferRecordSamplingAssignmentInformation.SamplingWorkshopLocation] = dispatchSubs[i][DispatchSamplingSubTable.LocationSamplingWorkshop] + string.Empty;
    //            disProcessSubsObj.Status = H3.DataModel.BizObjectStatus.Effective;
    //            //转运取样派工信息派工子表
    //            disProcessSubsObj.Update();
    //            //添加新的转运取样派工信息派工子表对象
    //            Tools.BizOperation.AddChildBizObject(this.Engine, processObj, TransferRecordSamplingAssignmentInformation.TableCode, disProcessSubsObj);
    //            //是否限制派工顺序
    //            if ((bool)dispatchObj[Dispatchs.SamplingUnlimitedDispatchSequence])
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    if (workers.Count == 0)
    //    {
    //        workers = DispatchLogic.CalculateWorkerd("取样", namestatus, dispatchObj);
    //    }
    //    //工人
    //    processObj[TransferRecord.SamplingWorker] = workers.ToArray();

    //    //判断派工表的取样班组长有无值
    //    if (dispatchObj[Dispatchs.SamplingTeamLeader] + string.Empty == string.Empty)
    //    {
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //取样班组长 = 总计划员
    //            processObj[TransferRecord.SamplingTeamLeader] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //取样班组长 = 冷加工科长
    //            processObj[TransferRecord.SamplingTeamLeader] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }
    //    else
    //    {
    //        //取样班组长
    //        processObj[TransferRecord.SamplingTeamLeader] = dispatchObj[Dispatchs.SamplingTeamLeader] + string.Empty;
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }

    //    //阶段待上机人（筛选）
    //    processObj["F0000243"] = DispatchLogic.SyncParticipants(this.Engine, workers);

    //    processObj.Update();
    //}

    ///**
    //* --Author: nkx
    //* 转运重新读取粗车派工信息
    //*/
    //public void DispatchLogicsRoughing()
    //{
    //    //转运粗车
    //    H3.DataModel.BizObject processObj = me;
    //    //派工计划
    //    string dispatchId = processObj[TransferRecord.DispatchingPlan] + string.Empty;
    //    //派工表
    //    H3.DataModel.BizObject dispatchObj = Tools.BizOperation.Load(this.Engine, Dispatchs.TableCode, dispatchId);
    //    //派工表子表信息
    //    H3.DataModel.BizObject[] dispatchSubs = dispatchObj[DispatchRoughSubTable.TableCode] as H3.DataModel.BizObject[];
    //    //转运粗车派工信息子表信息
    //    H3.DataModel.BizObject[] disProcessSubs = processObj[TransferRecordSubtableRoughDispatchInformation.TableCode] as H3.DataModel.BizObject[];

    //    List<string> workers = new List<string>();
    //    List<string> namestatus = new List<string>();
    //    bool flagss = true;

    //    //判断转运粗车派工信息表子表有没有值，有值则清空子表
    //    if (disProcessSubs != null && disProcessSubs.Length > 0)
    //    {
    //        for (int i = (disProcessSubs.Length - 1); i >= 0; i--)
    //        {
    //            //判断子表中“任务状态”控件中是否有值
    //            if (disProcessSubs[i][TransferRecordSubtableRoughDispatchInformation.TaskStatus] + string.Empty == string.Empty)
    //            {
    //                //清空子表最后一行
    //                Tools.BizOperation.DeleteChildBizObject(this.Engine, processObj, TransferRecordSubtableRoughDispatchInformation.TableCode);
    //            }
    //            else
    //            {
    //                if (disProcessSubs[i][TransferRecordSubtableRoughDispatchInformation.TaskStatus] + string.Empty != "已完成")
    //                {
    //                    string[] taskWorkers = dispatchSubs[i][DispatchRoughSubTable.Name] as string[];
    //                    foreach (string worker in taskWorkers)
    //                    {
    //                        workers.Add(worker);
    //                    }
    //                    flagss = false;
    //                }
    //                if (disProcessSubs[i][TransferRecordSubtableRoughDispatchInformation.TaskStatus] + string.Empty == "已完成")
    //                {
    //                    //已完成时把任务名称添加进namestatus中
    //                    namestatus.Add(disProcessSubs[i][TransferRecordSubtableRoughDispatchInformation.DispatchTask] + string.Empty);
    //                }
    //            }
    //        }
    //    }
    //    //再次赋值转运粗车派工信息表子表信息
    //    disProcessSubs = processObj[TransferRecordSubtableRoughDispatchInformation.TableCode] as H3.DataModel.BizObject[];

    //    //判断派工表子表有没有值
    //    if (dispatchSubs != null && dispatchSubs.Length > 0 && flagss)
    //    {
    //        processObj[TransferRecord.RoughCarRestrictedDispatchingSequence] = (bool)dispatchObj[Dispatchs.RoughTurningUnlimitedDispatchSequence];
    //        for (int i = 0; i < dispatchSubs.Length; i++)
    //        {
    //            bool flag = false;
    //            //判断转运粗车派工信息表子表有没有值
    //            if (disProcessSubs != null && disProcessSubs.Length > 0)
    //            {
    //                for (int j = 0; j < disProcessSubs.Length; j++)
    //                {
    //                    //对比转运粗车派工信息子表与派工表的“派工任务”是否一致
    //                    if (disProcessSubs[j][TransferRecordSubtableRoughDispatchInformation.DispatchTask] + string.Empty == dispatchSubs[i][DispatchRoughSubTable.TaskName] + string.Empty)
    //                    {
    //                        flag = true;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (flag == true)
    //            {
    //                continue;
    //            }
    //            //转运粗车派工信息派工子表对象
    //            H3.DataModel.BizObject disProcessSubsObj = Tools.BizOperation.New(this.Engine, TransferRecordSubtableRoughDispatchInformation.TableCode);
    //            //派工任务
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.DispatchTask] = dispatchSubs[i][DispatchRoughSubTable.TaskName] + string.Empty;
    //            //派工人员
    //            string[] taskWorkers = dispatchSubs[i][DispatchRoughSubTable.Name] as string[];
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.Dispatcher] = taskWorkers;
    //            //设备名称
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.EquipmentName] = dispatchSubs[i][DispatchRoughSubTable.EquipmentName] + string.Empty;
    //            //派工量
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.DispatchQuantity] = dispatchSubs[i][DispatchRoughSubTable.ProcessingQuantity] + string.Empty;
    //            //完成量
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.CompletionQuantity] = dispatchSubs[i][DispatchRoughSubTable.ProcessedQuantity] + string.Empty;
    //            //工时
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.RoughTurningManHour] = dispatchSubs[i][DispatchRoughSubTable.ManHour] + string.Empty;
    //            //下屑量
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.ChipRemovalQuantity] = dispatchSubs[i][DispatchRoughSubTable.TheAmountOfScrap] + string.Empty;
    //            //设备编号
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.EquipmentNumber] = dispatchSubs[i][DispatchRoughSubTable.EquipmentNumber] + string.Empty;
    //            //设备类型
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.EquipmentType] = dispatchSubs[i][DispatchRoughSubTable.EquipmentType] + string.Empty;
    //            //车间名称
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.RoughTurningWorkshopName] = dispatchSubs[i][DispatchRoughSubTable.NameRoughingWorkshop] + string.Empty;
    //            //车间位置
    //            disProcessSubsObj[TransferRecordSubtableRoughDispatchInformation.RoughTurningWorkshopLocation] = dispatchSubs[i][DispatchRoughSubTable.LocationRoughingWorkshop] + string.Empty;
    //            disProcessSubsObj.Status = H3.DataModel.BizObjectStatus.Effective;
    //            //转运粗车派工信息派工子表
    //            disProcessSubsObj.Update();
    //            //添加新的转运粗车派工信息派工子表对象
    //            Tools.BizOperation.AddChildBizObject(this.Engine, processObj, TransferRecordSubtableRoughDispatchInformation.TableCode, disProcessSubsObj);
    //            //是否限制派工顺序
    //            if ((bool)dispatchObj[Dispatchs.RoughTurningUnlimitedDispatchSequence])
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    if (workers.Count == 0)
    //    {
    //        workers = DispatchLogic.CalculateWorkerd("粗车", namestatus, dispatchObj);
    //    }
    //    //工人
    //    processObj[TransferRecord.RoughingWorker] = workers.ToArray();

    //    //判断派工表的粗车班组长有无值
    //    if (dispatchObj[Dispatchs.RoughingTeamLeader] + string.Empty == string.Empty)
    //    {
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //粗车班组长 = 总计划员
    //            processObj[TransferRecord.RoughingTeamLeader] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //粗车班组长 = 冷加工科长
    //            processObj[TransferRecord.RoughingTeamLeader] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }
    //    else
    //    {
    //        //粗车班组长
    //        processObj[TransferRecord.RoughingTeamLeader] = dispatchObj[Dispatchs.RoughingTeamLeader] + string.Empty;
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }

    //    //阶段待上机人（筛选）
    //    processObj["F0000244"] = DispatchLogic.SyncParticipants(this.Engine, workers);

    //    processObj.Update();
    //}

    ///**
    //* --Author: nkx
    //* 转运重新读取精车派工信息
    //*/
    //public void DispatchLogicsFinishing()
    //{
    //    //转运精车
    //    H3.DataModel.BizObject processObj = me;
    //    //派工计划
    //    string dispatchId = processObj[TransferRecord.DispatchingPlan] + string.Empty;
    //    //派工表
    //    H3.DataModel.BizObject dispatchObj = Tools.BizOperation.Load(this.Engine, Dispatchs.TableCode, dispatchId);
    //    //派工表子表信息
    //    H3.DataModel.BizObject[] dispatchSubs = dispatchObj[DispatchFinishSubTable.TableCode] as H3.DataModel.BizObject[];
    //    //转运精车派工信息子表信息
    //    H3.DataModel.BizObject[] disProcessSubs = processObj[TransferRecordPrecisionVehicleDispatchInformation.TableCode] as H3.DataModel.BizObject[];

    //    List<string> workers = new List<string>();
    //    List<string> namestatus = new List<string>();
    //    bool flagss = true;

    //    //判断转运精车派工信息表子表有没有值，有值则清空子表
    //    if (disProcessSubs != null && disProcessSubs.Length > 0)
    //    {
    //        for (int i = (disProcessSubs.Length - 1); i >= 0; i--)
    //        {
    //            //判断子表中“任务状态”控件中是否有值
    //            if (disProcessSubs[i][TransferRecordPrecisionVehicleDispatchInformation.TaskStatus] + string.Empty == string.Empty)
    //            {
    //                //清空子表最后一行
    //                Tools.BizOperation.DeleteChildBizObject(this.Engine, processObj, TransferRecordPrecisionVehicleDispatchInformation.TableCode);
    //            }
    //            else
    //            {
    //                if (disProcessSubs[i][TransferRecordPrecisionVehicleDispatchInformation.TaskStatus] + string.Empty != "已完成")
    //                {
    //                    string[] taskWorkers = dispatchSubs[i][DispatchFinishSubTable.Name] as string[];
    //                    foreach (string worker in taskWorkers)
    //                    {
    //                        workers.Add(worker);
    //                    }
    //                    flagss = false;
    //                }
    //                if (disProcessSubs[i][TransferRecordPrecisionVehicleDispatchInformation.TaskStatus] + string.Empty == "已完成")
    //                {
    //                    //已完成时把任务名称添加进namestatus中
    //                    namestatus.Add(disProcessSubs[i][TransferRecordPrecisionVehicleDispatchInformation.DispatchTask] + string.Empty);
    //                }
    //            }
    //        }
    //    }
    //    //再次赋值转运精车派工信息表子表信息
    //    disProcessSubs = processObj[TransferRecordPrecisionVehicleDispatchInformation.TableCode] as H3.DataModel.BizObject[];

    //    //判断派工表子表有没有值
    //    if (dispatchSubs != null && dispatchSubs.Length > 0 && flagss)
    //    {
    //        processObj[TransferRecord.FineCarRestrictedDispatchingSequence] = (bool)dispatchObj[Dispatchs.FinishTurningUnlimitedDispatchSequence];
    //        for (int i = 0; i < dispatchSubs.Length; i++)
    //        {
    //            bool flag = false;
    //            //判断转运精车派工信息表子表有没有值
    //            if (disProcessSubs != null && disProcessSubs.Length > 0)
    //            {
    //                for (int j = 0; j < disProcessSubs.Length; j++)
    //                {
    //                    //对比转运精车派工信息子表与派工表的“派工任务”是否一致
    //                    if (disProcessSubs[j][TransferRecordPrecisionVehicleDispatchInformation.DispatchTask] + string.Empty == dispatchSubs[i][DispatchFinishSubTable.TaskName] + string.Empty)
    //                    {
    //                        flag = true;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (flag == true)
    //            {
    //                continue;
    //            }
    //            //转运精车派工信息派工子表对象
    //            H3.DataModel.BizObject disProcessSubsObj = Tools.BizOperation.New(this.Engine, TransferRecordPrecisionVehicleDispatchInformation.TableCode);
    //            //派工任务
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.DispatchTask] = dispatchSubs[i][DispatchFinishSubTable.TaskName] + string.Empty;
    //            //派工人员
    //            string[] taskWorkers = dispatchSubs[i][DispatchFinishSubTable.Name] as string[];
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.Dispatcher] = taskWorkers;
    //            //设备名称
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.EquipmentName] = dispatchSubs[i][DispatchFinishSubTable.EquipmentName] + string.Empty;
    //            //派工量
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.DispatchQuantity] = dispatchSubs[i][DispatchFinishSubTable.ProcessingQuantity] + string.Empty;
    //            //完成量
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.CompletionQuantity] = dispatchSubs[i][DispatchFinishSubTable.ProcessedQuantity] + string.Empty;
    //            //工时
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.FinishingManHour] = dispatchSubs[i][DispatchFinishSubTable.ManHour] + string.Empty;
    //            //下屑量
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.ChipRemovalQuantity] = dispatchSubs[i][DispatchFinishSubTable.TheAmountOfScrap] + string.Empty;
    //            //设备编号
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.EquipmentNumber] = dispatchSubs[i][DispatchFinishSubTable.EquipmentNumber] + string.Empty;
    //            //设备类型
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.EquipmentType] = dispatchSubs[i][DispatchFinishSubTable.EquipmentType] + string.Empty;
    //            //车间名称
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.FinishingWorkshopName] = dispatchSubs[i][DispatchFinishSubTable.NameFinishingWorkshop] + string.Empty;
    //            //车间位置
    //            disProcessSubsObj[TransferRecordPrecisionVehicleDispatchInformation.FinishingWorkshopLocation] = dispatchSubs[i][DispatchFinishSubTable.LocationFinishingWorkshop] + string.Empty;
    //            disProcessSubsObj.Status = H3.DataModel.BizObjectStatus.Effective;
    //            //转运精车派工信息派工子表
    //            disProcessSubsObj.Update();
    //            //添加新的转运精车派工信息派工子表对象
    //            Tools.BizOperation.AddChildBizObject(this.Engine, processObj, TransferRecordPrecisionVehicleDispatchInformation.TableCode, disProcessSubsObj);
    //            //是否限制派工顺序
    //            if ((bool)dispatchObj[Dispatchs.FinishTurningUnlimitedDispatchSequence])
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    if (workers.Count == 0)
    //    {
    //        workers = DispatchLogic.CalculateWorkerd("精车", namestatus, dispatchObj);
    //    }
    //    //工人
    //    processObj[TransferRecord.FinishingWorker] = workers.ToArray();

    //    //判断派工表的精车班组长有无值
    //    if (dispatchObj[Dispatchs.FinishingTeamLeader] + string.Empty == string.Empty)
    //    {
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //精车班组长 = 总计划员
    //            processObj[TransferRecord.FinishingTeamLeader] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //精车班组长 = 冷加工科长
    //            processObj[TransferRecord.FinishingTeamLeader] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }
    //    else
    //    {
    //        //精车班组长
    //        processObj[TransferRecord.FinishingTeamLeader] = dispatchObj[Dispatchs.FinishingTeamLeader] + string.Empty;
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }

    //    //阶段待上机人（筛选）
    //    processObj["F0000242"] = DispatchLogic.SyncParticipants(this.Engine, workers);

    //    processObj.Update();
    //}

    ///**
    //* --Author: nkx
    //* 转运重新读取钻孔派工信息
    //*/
    //public void DispatchLogicsDrill()
    //{
    //    //转运钻孔
    //    H3.DataModel.BizObject processObj = me;
    //    //派工计划
    //    string dispatchId = processObj[TransferRecord.DispatchingPlan] + string.Empty;
    //    //派工表
    //    H3.DataModel.BizObject dispatchObj = Tools.BizOperation.Load(this.Engine, Dispatchs.TableCode, dispatchId);
    //    //派工表子表信息
    //    H3.DataModel.BizObject[] dispatchSubs = dispatchObj[DispatchDrillSubTable.TableCode] as H3.DataModel.BizObject[];
    //    //转运钻孔派工信息子表信息
    //    H3.DataModel.BizObject[] disProcessSubs = processObj[TransferRecordDrillingAssignmentInformation.TableCode] as H3.DataModel.BizObject[];

    //    List<string> workers = new List<string>();
    //    List<string> namestatus = new List<string>();
    //    bool flagss = true;

    //    //判断转运钻孔派工信息表子表有没有值，有值则清空子表
    //    if (disProcessSubs != null && disProcessSubs.Length > 0)
    //    {
    //        for (int i = (disProcessSubs.Length - 1); i >= 0; i--)
    //        {
    //            //判断子表中“任务状态”控件中是否有值
    //            if (disProcessSubs[i][TransferRecordDrillingAssignmentInformation.TaskStatus] + string.Empty == string.Empty)
    //            {
    //                //清空子表最后一行
    //                Tools.BizOperation.DeleteChildBizObject(this.Engine, processObj, TransferRecordDrillingAssignmentInformation.TableCode);
    //            }
    //            else
    //            {
    //                if (disProcessSubs[i][TransferRecordDrillingAssignmentInformation.TaskStatus] + string.Empty != "已完成")
    //                {
    //                    string[] taskWorkers = dispatchSubs[i][DispatchDrillSubTable.Name] as string[];
    //                    foreach (string worker in taskWorkers)
    //                    {
    //                        workers.Add(worker);
    //                    }
    //                    flagss = false;
    //                }
    //                if (disProcessSubs[i][TransferRecordDrillingAssignmentInformation.TaskStatus] + string.Empty == "已完成")
    //                {
    //                    //已完成时把任务名称添加进namestatus中
    //                    namestatus.Add(disProcessSubs[i][TransferRecordDrillingAssignmentInformation.DispatchTask] + string.Empty);
    //                }
    //            }
    //        }
    //    }
    //    //再次赋值转运钻孔派工信息表子表信息
    //    disProcessSubs = processObj[TransferRecordDrillingAssignmentInformation.TableCode] as H3.DataModel.BizObject[];

    //    //判断派工表子表有没有值
    //    if (dispatchSubs != null && dispatchSubs.Length > 0 && flagss)
    //    {
    //        processObj[TransferRecord.DrillingRestrictedDispatchingSequence] = (bool)dispatchObj[Dispatchs.DrillingUnlimitedDispatchSequence];
    //        for (int i = 0; i < dispatchSubs.Length; i++)
    //        {
    //            bool flag = false;
    //            //判断转运钻孔派工信息表子表有没有值
    //            if (disProcessSubs != null && disProcessSubs.Length > 0)
    //            {
    //                for (int j = 0; j < disProcessSubs.Length; j++)
    //                {
    //                    //对比转运钻孔派工信息子表与派工表的“派工任务”是否一致
    //                    if (disProcessSubs[j][TransferRecordDrillingAssignmentInformation.DispatchTask] + string.Empty == dispatchSubs[i][DispatchDrillSubTable.TaskName] + string.Empty)
    //                    {
    //                        flag = true;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (flag == true)
    //            {
    //                continue;
    //            }
    //            //转运钻孔派工信息派工子表对象
    //            H3.DataModel.BizObject disProcessSubsObj = Tools.BizOperation.New(this.Engine, TransferRecordDrillingAssignmentInformation.TableCode);
    //            //派工任务
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.DispatchTask] = dispatchSubs[i][DispatchDrillSubTable.TaskName] + string.Empty;
    //            //派工人员
    //            string[] taskWorkers = dispatchSubs[i][DispatchDrillSubTable.Name] as string[];
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.Dispatcher] = taskWorkers;
    //            //设备名称
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.EquipmentName] = dispatchSubs[i][DispatchDrillSubTable.EquipmentName] + string.Empty;
    //            //派工量
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.DispatchQuantity] = dispatchSubs[i][DispatchDrillSubTable.ProcessingQuantity] + string.Empty;
    //            //完成量
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.CompletionQuantity] = dispatchSubs[i][DispatchDrillSubTable.ProcessedQuantity] + string.Empty;
    //            //工时
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.DrillingManHour] = dispatchSubs[i][DispatchDrillSubTable.ManHour] + string.Empty;
    //            //下屑量
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.ChipRemovalQuantity] = dispatchSubs[i][DispatchDrillSubTable.TheAmountOfScrap] + string.Empty;
    //            //设备编号
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.EquipmentNumber] = dispatchSubs[i][DispatchDrillSubTable.EquipmentNumber] + string.Empty;
    //            //设备类型
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.EquipmentType] = dispatchSubs[i][DispatchDrillSubTable.EquipmentType] + string.Empty;
    //            //车间名称
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.DrillingWorkshopName] = dispatchSubs[i][DispatchDrillSubTable.NameDrillWorkshop] + string.Empty;
    //            //车间位置
    //            disProcessSubsObj[TransferRecordDrillingAssignmentInformation.DrillingWorkshopLocation] = dispatchSubs[i][DispatchDrillSubTable.LocationDrillWorkshop] + string.Empty;
    //            disProcessSubsObj.Status = H3.DataModel.BizObjectStatus.Effective;
    //            //转运钻孔派工信息派工子表
    //            disProcessSubsObj.Update();
    //            //添加新的转运钻孔派工信息派工子表对象
    //            Tools.BizOperation.AddChildBizObject(this.Engine, processObj, TransferRecordDrillingAssignmentInformation.TableCode, disProcessSubsObj);
    //            //是否限制派工顺序
    //            if ((bool)dispatchObj[Dispatchs.DrillingUnlimitedDispatchSequence])
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    if (workers.Count == 0)
    //    {
    //        workers = DispatchLogic.CalculateWorkerd("钻孔", namestatus, dispatchObj);
    //    }

    //    //工人
    //    processObj[TransferRecord.DrillWorker] = workers.ToArray();

    //    //判断派工表的钻孔班组长有无值
    //    if (dispatchObj[Dispatchs.DrillTeamLeader] + string.Empty == string.Empty)
    //    {
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //钻孔班组长 = 总计划员
    //            processObj[TransferRecord.DrillTeamLeader] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //钻孔班组长 = 冷加工科长
    //            processObj[TransferRecord.DrillTeamLeader] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }
    //    else
    //    {
    //        //钻孔班组长
    //        processObj[TransferRecord.DrillTeamLeader] = dispatchObj[Dispatchs.DrillTeamLeader] + string.Empty;
    //        //判断派工表的冷加工科长有无值
    //        if (dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty == string.Empty)
    //        {
    //            //冷加工科长 = 总计划员
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.TotalPlanner] + string.Empty;
    //        }
    //        else
    //        {
    //            //冷加工科长 = 冷加工科长
    //            processObj[TransferRecord.ColdWorkingSectionChief] = dispatchObj[Dispatchs.ColdWorkingSectionChief] + string.Empty;
    //        }
    //    }

    //    //阶段待上机人（筛选）
    //    processObj["F0000241"] = DispatchLogic.SyncParticipants(this.Engine, workers);

    //    processObj.Update();
    //}




    // 生产制造流程,转运记录

    // 转运工资表


    string TSalaryTable_TableCode = "D001419Sacxlj9bmt1o1ka813wfbr7366";       
    string TSalaryTable_VersionNumber = "F0000022"; // 版本号        
    string TSalaryTable_Amount = "F0000013";// 金额       
    string TSalaryTable_Quantity = "F0000010"; // 数量       
    string TSalaryTable_Wages = "F0000009"; // 工价       
    string TSalaryTable_OrderSpecificationNumber = "ProductNum"; // 订单规格号       
    string TSalaryTable_Transshiper = "F0000019"; // 转运人员       
    string TSalaryTable_WorkpieceNumber = "F0000014"; // 工件号       
    string TSalaryTable_Id = "F0000016"; // ID      
    string TSalaryTable_OccurrenceDate = "F0000025";  // 发生日期     
    string TSalaryTable_DepartmentName = "F0000018";   // 部门名称 
    string TSalaryTable_DepartmentCode = "F0000021";  // 部门代码       
    string TSalaryTable_DataCode = "F0000024"; // 数据代码

    //转运记录表
    string TransferRecord_TableCode = "D001419Son0vyw9n413fhgqud7zeeocz2";  
       string TransferRecord_OrderSpecificationNumber = "F0000016"; // 订单规格号 
    string TransferRecord_WorkpieceNumber = "F0000025";             // 工件号
    string TransferRecord_Id = "F0000058";                          // ID
                                         
    string TransferRecord_UnitPrice = "F0000263";// 单价
    string TransferRecord_Amount = "F0000264";   // 金额

    string TransferRecord_Transshiper1 = "F0000246"; // 转运人1   
    string TransferRecord_Transshiper2 = "F0000247"; // 转运人2
    string TransferRecord_Transshiper3 = "F0000248"; // 转运人3
    string TransferRecord_Transshiper4 = "F0000249"; // 转运人4
    
    string TransferRecord_AllocationProportion1 = "F0000265";// 分配比例1 
    string TransferRecord_AllocationProportion2 = "F0000266";// 分配比例2     
    string TransferRecord_AllocationProportion3 = "F0000267";// 分配比例3
    string TransferRecord_AllocationProportion4 = "F0000268";// 分配比例4
    
    string TransferRecord_TransshipmentSalary1 = "F0000258";  // 转运工资1  
    string TransferRecord_TransshipmentSalary2 = "F0000259";  // 转运工资2 
    string TransferRecord_TransshipmentSalary3 = "F0000260";  // 转运工资3   
    string TransferRecord_TransshipmentSalary4 = "F0000261";  // 转运工资4 

    string TransferRecord_VersionNumber = "F0000196";   // 版本号
    string TransferRecord_DataCode = "F0000064";        // 数据代码
    string TransferRecord_DepartmentCode = "F0000200";  // 部门代码
    string TransferRecord_WorkshopLocation = "F0000067";// 车间位置
 
}