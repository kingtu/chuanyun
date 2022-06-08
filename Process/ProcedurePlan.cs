
using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using H3;
using System.Collections;
using H3.DataModel;


public class D001419Szlywopbivyrv1d64301ta5xv4 : H3.SmartForm.SmartFormController
{
    string activityCode = "工序计划表";
    H3.SmartForm.SmartFormResponseDataItem responseDataItem;  //用户提示信息
    string info = string.Empty;  //值班信息
    H3.DataModel.BizObject me;   //本表单对象
    string systemUserId = H3.Organization.User.SystemUserId; //系统用户Id
    string userName = ""; //当前用户

    // 生产计划,ABCD工序计划表
    //表代码
    string ABCDProcessPlan_TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";

    // 外协合同表-精车
    string ABCDProcessPlan_OutsourcingContractFormFinishTurning = "F0000087";

    // 钻孔班组长
    string ABCDProcessPlan_DrillTeamLeader = "F0000250";

    // 单件四面光配置
    string ABCDProcessPlan_SinglePieceFourSideLightConfiguration = "F0000224";

    // 精车班组长
    string ABCDProcessPlan_FinishingTeamLeader = "F0000249";

    // 热加工完成时间
    string ABCDProcessPlan_HotProcessingPlan = "F0000023";

    // 粗车班组长
    string ABCDProcessPlan_RoughingTeamLeader = "F0000248";

    // 精车班组
    string ABCDProcessPlan_FinishTeam = "FinishTeam";

    // 单件忽略理化结果
    string ABCDProcessPlan_SinglePieceIgnorePhysicochemicalResults = "F0000161";

    // 粗车班组
    string ABCDProcessPlan_RoughTeam = "RoughTeam";
    // 加工单位-粗车
    string ABCDProcessPlan_ProcessingUnitRoughTurning = "F0000078";

    // 外协合同表-钻孔
    string ABCDProcessPlan_OutsourcingContractFormDrilling = "F0000089";

    // 精车计划完成时间
    string ABCDProcessPlan_FinishTurningPlannedCompletionTime = "F0000098";

    // 加工单位-精车
    string ABCDProcessPlan_ProcessingUnitFinishTurning = "F0000082";

    // 粗车计划完成时间
    string ABCDProcessPlan_RoughTurningPlanCompletionTime = "F0000095";

    // 取样班组长
    string ABCDProcessPlan_SamplingTeamLeader = "F0000247";

    // ID
    string ABCDProcessPlan_ID = "F0000007";

    // 订单批次规格表
    string ABCDProcessPlan_OrderBatchSpecificationTable = "F0000017";

    // 取样计划完成时间
    string ABCDProcessPlan_CompletionTimeOfSamplingPlan = "F0000212";

    // 加工单位-钻孔
    string ABCDProcessPlan_ProcessingUnitDrilling = "F0000084";

    // 质量状态
    string ABCDProcessPlan_QualityStatus = "F0000114";

    // 冷加工科长
    string ABCDProcessPlan_ColdWorkingSectionChief = "F0000251";

    // 单件精整配置
    string ABCDProcessPlan_SinglePieceFinishingConfiguration = "F0000146";
    // 钻孔计划完成时间
    string ABCDProcessPlan_DrillingPlannedCompletionTime = "F0000102";

    // 钻孔班组
    string ABCDProcessPlan_DrillTeam = "DrillTeam";


    // 生产计划,派工表
    string Dispatchs_TableCode = "D001419c08bb982ac44481a9439076269a8f783";
   
    // 粗车计划完成时间
    string Dispatchs_RoughTurningPlanCompletionTime = "F0000004";
  
    // 冷加工科长
    string Dispatchs_ColdWorkingSectionChief = "F0000086";

    // ID
    string Dispatchs_ID = "F0000025";
   
    // 钻孔班组长
    string Dispatchs_DrillTeamLeader = "F0000090";
    
    // 精车班组长
    string Dispatchs_FinishingTeamLeader = "F0000089";
  
    // 精车计划完成时间
    string Dispatchs_FinishTurningPlanCompletionTime = "F0000005";
  
    // 取样计划完成时间
    string Dispatchs_SamplingPlanCompletionTime = "F0000003";
    // 粗车班组长
    string Dispatchs_RoughingTeamLeader = "F0000088";
   
    // 钻孔计划完成时间
    string Dispatchs_DrillingPlanCompletionTime = "F0000006";
    // 取样班组长
    string Dispatchs_SamplingTeamLeader = "F0000087";
   
    string Dispatchs_Objectid= "ObjectId";

    public D001419Szlywopbivyrv1d64301ta5xv4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        responseDataItem = new H3.SmartForm.SmartFormResponseDataItem();   //////////item命名可优化////////////
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        try
        {
            if (this.Request.IsCreateMode)
            {
                //初始化工艺与质量配置 -- Author:FuBin
                LoadingProcessAndQualityConfiguration();
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            responseDataItem.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }

        response.ReturnData.Add("responseDataItem", responseDataItem);    /////////key1命名需要优化////////////
        base.OnLoad(response);

        //--------------------------加载前后分割线-------------------------//

        try
        {
            if (!this.Request.IsCreateMode)
            {
                //加载后代码
            }
        }
        catch (Exception ex)
        {
            info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            responseDataItem.Value = string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        try
        {
            //提交
            if (actionName == "Submit")
            {
                //计划数不允许大于成品需求数、订单批次规格表不允许为空 -- Author:FuBin
                //CheckDemandCount(response);
                //同步派工表开始：TeamLeader班组长，PlanCompletionTime计划完成时间。
                string id = (string)postValue.Data[ABCDProcessPlan_ID]; //产品ID
                H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
                Tools.Filter.And(filter, Dispatchs_ID, H3.Data.ComparisonOperatorType.Equal, id);      //产品ID
                BizObject objDispatch = Tools.BizOperation.GetFirst(this.Engine, Dispatchs_TableCode, filter); //
                //派工表关联数据的ObjectId
                if (objDispatch != null)
                {
                    string disObjectId = (string)objDispatch[Dispatchs_Objectid];  //派工表Objectid 
                    //同步派工表
                    SyncDispath(postValue, disObjectId);
                }

            }
            if (postValue.Data.ContainsKey("cmd"))
            {
                string cmd = (string)postValue.Data["cmd"];
                if (cmd == "btnModify")
                {
                    BathModify(postValue, response);
                }
            }
            base.OnSubmit(actionName, postValue, response);
        }
        catch (Exception ex)
        {		//负责人信息
            string info = Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
            response.Message =
                string.Format("管理员已收到问题反馈，({0})信息专员正在修复中！({1})", info, ex.Message);
        }
    }



    //批量修改
    private void BathModify(H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (postValue.Data.ContainsKey("objectIdList"))
        {
            string idList = (string)postValue.Data["objectIdList"];      //所有选中的objectId
            string[] objectIdList = this.Deserialize<string[]>(idList) as string[];
            foreach (string objectId in objectIdList)
            {
                //修改其中的一个业务对象
                Modify(postValue, response, objectId);
            }
        }
    }




    //修改一个业务对象的可修改字段
    //objectId：被修改的业务对象的objectId。
    private void Modify(H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response, string objectId)
    {
        H3.DataModel.BizObject bizObject = Tools.BizOperation.Load(this.Engine, me.Schema.SchemaCode, objectId);
        //可修改组：粗车计划完成时间、加工单位-粗车、外协合同表-粗车、粗车班组、粗车班组长
        string[] modifyFields = {
            ABCDProcessPlan_RoughTurningPlanCompletionTime,
            ABCDProcessPlan_ProcessingUnitRoughTurning,
            ABCDProcessPlan_RoughTeam,
            ABCDProcessPlan_RoughingTeamLeader,
            ABCDProcessPlan_HotProcessingPlan
        };

        if (Modify(postValue, bizObject, modifyFields))
        {
            bizObject.Update();
        }
        //可修改组：精车计划完成时间、加工单位-精车、外协合同表-精车、精车班组、精车班组长
        string[] modifyFields2 = {
            ABCDProcessPlan_FinishTurningPlannedCompletionTime,
            ABCDProcessPlan_ProcessingUnitFinishTurning,
            ABCDProcessPlan_OutsourcingContractFormFinishTurning,
            ABCDProcessPlan_FinishTeam,
            ABCDProcessPlan_FinishingTeamLeader
        };
        if (Modify(postValue, bizObject, modifyFields2))
        {
            bizObject.Update();
        }
        //可修改组：钻孔计划完成时间、加工单位-钻孔、外协合同表-钻孔、钻孔班组、钻孔班组长
        string[] modifyFields3 = {
            ABCDProcessPlan_DrillingPlannedCompletionTime,
            ABCDProcessPlan_ProcessingUnitDrilling,
            ABCDProcessPlan_OutsourcingContractFormDrilling,
            ABCDProcessPlan_DrillTeam,
            ABCDProcessPlan_DrillTeamLeader
        };
        if (Modify(postValue, bizObject, modifyFields3))
        {
            bizObject.Update();
        }
        ////可修改组：取样计划完成时间、取样班组长
        string[] modifyFields4 = {
            ABCDProcessPlan_CompletionTimeOfSamplingPlan,
            ABCDProcessPlan_SamplingTeamLeader
        };
        if (Modify(postValue, bizObject, modifyFields4))
        {
            bizObject.Update();
        }

        //派工表关联数据的ObjectId
        string disObjectId = (string)bizObject["DispatchTable"];
        //开始同步派工表字段：TeamLeader班组长，PlanCompletionTime计划完成时间。
        SyncDispath(postValue, disObjectId);

    }


    //根据可修改组来修改组内字段
    //target：被修改的业务对象。modifyFields：需要修改的字段的数组。
    private bool Modify(H3.SmartForm.SmartFormPostValue postValue, H3.DataModel.BizObject target, string[] modifyFields)
    {
        bool canUpdate = false;
        foreach (string field in modifyFields)
        {
            object src = postValue.Data[field];
            if ((src != null) && !string.IsNullOrEmpty(src.ToString()))
            {
                target[field] = src;
                canUpdate = true;
            }
        }
        return canUpdate;
    }

    //同步各工序组长、需求期
    //target：需要同步的业务对象。dic：字段对应关系集合（字典）
    private bool SyncObject(H3.SmartForm.SmartFormPostValue postValue, BizObject target, System.Collections.Generic.Dictionary<string, string> dic)
    {
        bool canUpdate = false;
        if (dic != null)
        {
            foreach (string key in dic.Keys)
            {
                object value = postValue.Data[dic[key]];
                if ((value != null) && !string.IsNullOrEmpty(value.ToString()))
                {
                    target[key] = value;
                    canUpdate = true;
                }
            }
        }
        return canUpdate;
    }

    //建立字段同步关系后，开始同步派工表
    //disObjectId：派工表业务对象objectId
    private void SyncDispath(H3.SmartForm.SmartFormPostValue postValue, string disObjectId)
    {
        BizObject dispatch = Tools.BizOperation.Load(this.Engine, Dispatchs_TableCode, disObjectId);     //派工表业务对象
        if (dispatch == null) { return; }
        Dictionary<string, string> dic = new Dictionary<string, string>();     // 建立用于存储被修改字段的字典

        //TeamLeader：班组长。PlanCompletionTime：计划完成时间。
        dic.Add(Dispatchs_SamplingTeamLeader, ABCDProcessPlan_SamplingTeamLeader);
        dic.Add(Dispatchs_SamplingPlanCompletionTime, ABCDProcessPlan_CompletionTimeOfSamplingPlan);

        dic.Add(Dispatchs_RoughTurningPlanCompletionTime, ABCDProcessPlan_RoughTurningPlanCompletionTime);
        dic.Add(Dispatchs_RoughingTeamLeader, ABCDProcessPlan_RoughingTeamLeader);

        dic.Add(Dispatchs_FinishTurningPlanCompletionTime, ABCDProcessPlan_FinishTurningPlannedCompletionTime);
        dic.Add(Dispatchs_FinishingTeamLeader, ABCDProcessPlan_FinishingTeamLeader);

        dic.Add(Dispatchs_DrillingPlanCompletionTime, ABCDProcessPlan_DrillingPlannedCompletionTime);
        dic.Add(Dispatchs_DrillTeamLeader, ABCDProcessPlan_DrillTeamLeader);
        //冷加工科长 ColdWorkingSectionChief
        dic.Add(Dispatchs_ColdWorkingSectionChief, ABCDProcessPlan_ColdWorkingSectionChief);
        if (SyncObject(postValue, dispatch, dic))
        {
            dispatch.Update();
        }
    }

    //加载工艺与质量配置 -- Author:FuBin
    private void LoadingProcessAndQualityConfiguration()
    {   //后续优化：为空时执行，精整需要考虑优先级顺序决定是否允许配置

        //工艺配置表
        string mySql = string.Format("Select * from i_{0}", ProcessConfig.TableCode);
        DataTable table = this.Engine.Query.QueryTable(mySql, null);       /////////////jzForm命名可优化/////////////////

        //获取"全局精整配置"
        string finishing = table.Rows[0][ProcessConfig.GlobalFinishingConfiguration] + string.Empty;
        me[ABCDProcessPlan_SinglePieceFinishingConfiguration] = finishing; //初始化"单件精整配置"

        //获取"全局四面光配置"
        string lighting = table.Rows[0][ProcessConfig.GlobalFourSideLightConfiguration] + string.Empty;
        me[ABCDProcessPlan_SinglePieceFourSideLightConfiguration] = lighting; //初始化"四面光配置"

        //质量配置表
        string sql = string.Format("Select * from i_{0}", QAConfig.TableCode);
        table = this.Engine.Query.QueryTable(sql, null);     ////////////qcForm命名可优化/////////////////

        //获取"全局忽略理化结果"
        string assay = table.Rows[0][QAConfig.GlobalIgnorePhysicochemicalResults] + string.Empty;
        me[ABCDProcessPlan_SinglePieceIgnorePhysicochemicalResults] = assay; //初始化"单件忽略理化结果"

        me.Update();
    }

    //计划数不允许大于成品需求数 -- Author:FuBin
    protected void CheckDemandCount(H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (me[ABCDProcessPlan_OrderBatchSpecificationTable] + string.Empty != string.Empty)
        {
            //加载订单批次规格表
            string objectId = me[ABCDProcessPlan_OrderBatchSpecificationTable] + string.Empty;
            H3.DataModel.BizObject bizObjectABC = Tools.BizOperation.Load(this.Engine, OrderBatchSpecification.TableCode, objectId);

            //成品需求数
            int demandCount = int.Parse(bizObjectABC[OrderBatchSpecification.FinishedProductDemandQuantity].ToString());

            //查找订单批次规格表相同的《工序计划表》数据
            string mySql = string.Format("Select * From i_{0} Where {1} = '{2}' and {3} != '已报废'",
                ABCDProcessPlan_TableCode, ABCDProcessPlan_OrderBatchSpecificationTable, objectId, ABCDProcessPlan_QualityStatus);
            DataTable thisData = this.Engine.Query.QueryTable(mySql, null);
            if (thisData != null && thisData.Rows != null && thisData.Rows.Count > demandCount)
            {
                response.Errors.Add("抱歉,您添加的计划产品己达上限");
            }
        }
    }
}


