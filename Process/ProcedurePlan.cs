
using H3;
using H3.DataModel;
using System;
using System.Data;
using System.Collections.Generic;


public class D001419Szlywopbivyrv1d64301ta5xv4 : H3.SmartForm.SmartFormController
{
    string activityCode = "工序计划表";
    H3.SmartForm.SmartFormResponseDataItem responseDataItem;  //用户提示信息
    string info = string.Empty;  //值班信息
    BizObject me;   //本表单对象   
    string userName = ""; //当前用户
    public D001419Szlywopbivyrv1d64301ta5xv4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        responseDataItem = new H3.SmartForm.SmartFormResponseDataItem();
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

        response.ReturnData.Add("responseDataItem", responseDataItem);
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
                string id = (string)postValue.Data[ID]; //产品ID
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
            RoughCuttingPlanCompletionTime,
            ProcessingUnitRoughCutting,
            RoughCuttingTeam,
            RoughCuttingTeamLeader,
            HeatTreatmentCompletionTime
        };

        if (Modify(postValue, bizObject, modifyFields))
        {
            bizObject.Update();
        }
        //可修改组：精车计划完成时间、加工单位-精车、外协合同表-精车、精车班组、精车班组长
        string[] modifyFields2 = {
            FinishingPlannedCompletionTime,
            ProcessingUnitFinishing,
            OutsourcingContractFormFinishing,
            FinishingTeam,
            FinishingTeamLeader
        };
        if (Modify(postValue, bizObject, modifyFields2))
        {
            bizObject.Update();
        }
        //可修改组：钻孔计划完成时间、加工单位-钻孔、外协合同表-钻孔、钻孔班组、钻孔班组长
        string[] modifyFields3 = {
            DrillingPlannedCompletionTime,
            ProcessingUnitDrilling,
            OutsourcingContractFormDrilling,
            DrillTeam,
            DrillTeamLeader
        };
        if (Modify(postValue, bizObject, modifyFields3))
        {
            bizObject.Update();
        }
        ////可修改组：取样计划完成时间、取样班组长
        string[] modifyFields4 = {
            CompletionTimeOfSamplingPlan,
            SamplingTeamLeader
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
        dic.Add(Dispatchs_SamplingTeamLeader, SamplingTeamLeader);
        dic.Add(Dispatchs_SamplingPlanCompletionTime, CompletionTimeOfSamplingPlan);

        dic.Add(Dispatchs_RoughCuttingPlanCompletionTime, RoughCuttingPlanCompletionTime);
        dic.Add(Dispatchs_RoughCutingTeamLeader, RoughCuttingTeamLeader);

        dic.Add(Dispatchs_FinishingPlanCompletionTime, FinishingPlannedCompletionTime);
        dic.Add(Dispatchs_FinishingTeamLeader, FinishingTeamLeader);

        dic.Add(Dispatchs_DrillingPlanCompletionTime, DrillingPlannedCompletionTime);
        dic.Add(Dispatchs_DrillingTeamLeader, DrillTeamLeader);
        //冷加工科长 ColdWorkingSectionChief
        dic.Add(Dispatchs_ColdWorkingSectionChief, ColdWorkingSectionChief);
        if (SyncObject(postValue, dispatch, dic))
        {
            dispatch.Update();
        }
    }

    //加载工艺与质量配置 -- Author:FuBin
    private void LoadingProcessAndQualityConfiguration()
    {   //后续优化：为空时执行，精整需要考虑优先级顺序决定是否允许配置

        //工艺配置表
        string mySql = string.Format("Select * from i_{0}", this.WorkmanshipConfig_TableCode);
        DataTable table = this.Engine.Query.QueryTable(mySql, null);

        //获取"全局精整配置"
        string finishing = table.Rows[0][this.WorkmanshipConfig_GlobalFinishingConfiguration] + string.Empty;
        me[SinglePieceFinishingConfiguration] = finishing; //初始化"单件精整配置"

        //获取"全局四面光配置"
        string lighting = table.Rows[0][this.WorkmanshipConfig_GlobalFourSideLightConfiguration] + string.Empty;
        me[SinglePieceFourSideLightConfiguration] = lighting; //初始化"四面光配置"

        //质量配置表
        string sql = string.Format("Select * from i_{0}", this.QAConfig_TableCode);
        table = this.Engine.Query.QueryTable(sql, null);

        //获取"全局忽略理化结果"
        string assay = table.Rows[0][this.QAConfig_GlobalIgnorePhysicochemicalResults] + string.Empty;
        me[SinglePieceIgnorePhysicochemicalResults] = assay; //初始化"单件忽略理化结果"

        me.Update();
    }

    //计划数不允许大于成品需求数 -- Author:FuBin
    protected void CheckDemandCount(H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (me[OrderBatchSpecificationTable] + string.Empty != string.Empty)
        {
            //加载订单批次规格表
            string objectId = me[OrderBatchSpecificationTable] + string.Empty;
            H3.DataModel.BizObject bizObjectABC = Tools.BizOperation.Load(this.Engine, this.OrderBatchSpecification_TableCode, objectId);

            //成品需求数
            int demandCount = int.Parse(bizObjectABC[this.OrderBatchSpecification_FinishedProductDemandQuantity].ToString());

            //查找订单批次规格表相同的《工序计划表》数据
            string mySql = string.Format("Select * From i_{0} Where {1} = '{2}' and {3} != '已报废'",
                TableCode, OrderBatchSpecificationTable, objectId, QualityStatus);
            DataTable thisData = this.Engine.Query.QueryTable(mySql, null);
            if (thisData != null && thisData.Rows != null && thisData.Rows.Count > demandCount)
            {
                response.Errors.Add("抱歉,您添加的计划产品己达上限");
            }
        }
    }

    string OrderBatchSpecification_TableCode = "D001419Sh8z1xnes2iju59dzn4ett4bb2";  //订单批次规格TableCode
    string OrderBatchSpecification_FinishedProductDemandQuantity = "F0000047";       //成品需求数

    string WorkmanshipConfig_TableCode = "D0014194755c7eecbe9410c84cf6640d9cb147b";   //工艺配置表TableCode
    string WorkmanshipConfig_GlobalFinishingConfiguration = "F0000001";               //全局精整配置
    string WorkmanshipConfig_GlobalFourSideLightConfiguration = "F0000011";           // 全局四面光配置

    string QAConfig_TableCode = "D0014198feb957936e040648d486b034af96597";        //质量配置表TableCode
    string QAConfig_GlobalIgnorePhysicochemicalResults = "F0000004";              //全局忽略理化结果

    // 生产计划,ABCD工序计划表
    //表代码
    string TableCode = "D001419Szlywopbivyrv1d64301ta5xv4";

    // 外协合同表-精车
    string OutsourcingContractFormFinishing = "F0000087";

    // 钻孔班组长
    string DrillTeamLeader = "F0000250";

    // 单件四面光配置
    string SinglePieceFourSideLightConfiguration = "F0000224";

    // 精车班组长
    string FinishingTeamLeader = "F0000249";

    // 热加工完成时间
    string HeatTreatmentCompletionTime = "F0000023";

    // 粗车班组长
    string RoughCuttingTeamLeader = "F0000248";

    // 精车班组
    string FinishingTeam = "FinishTeam";

    // 单件忽略理化结果
    string SinglePieceIgnorePhysicochemicalResults = "F0000161";

    // 粗车班组
    string RoughCuttingTeam = "RoughTeam";
    // 加工单位-粗车
    string ProcessingUnitRoughCutting = "F0000078";

    // 外协合同表-钻孔
    string OutsourcingContractFormDrilling = "F0000089";

    // 精车计划完成时间
    string FinishingPlannedCompletionTime = "F0000098";

    // 加工单位-精车
    string ProcessingUnitFinishing = "F0000082";

    // 粗车计划完成时间
    string RoughCuttingPlanCompletionTime = "F0000095";

    // 取样班组长
    string SamplingTeamLeader = "F0000247";

    // ID
    string ID = "F0000007";

    // 订单批次规格表
    string OrderBatchSpecificationTable = "F0000017";

    // 取样计划完成时间
    string CompletionTimeOfSamplingPlan = "F0000212";

    // 加工单位-钻孔
    string ProcessingUnitDrilling = "F0000084";

    // 质量状态
    string QualityStatus = "F0000114";

    // 冷加工科长
    string ColdWorkingSectionChief = "F0000251";

    // 单件精整配置
    string SinglePieceFinishingConfiguration = "F0000146";
    // 钻孔计划完成时间
    string DrillingPlannedCompletionTime = "F0000102";

    // 钻孔班组
    string DrillTeam = "DrillTeam";


    // 生产计划,派工表
    string Dispatchs_TableCode = "D001419c08bb982ac44481a9439076269a8f783";

    // 粗车计划完成时间
    string Dispatchs_RoughCuttingPlanCompletionTime = "F0000004";

    // 冷加工科长
    string Dispatchs_ColdWorkingSectionChief = "F0000086";

    // ID
    string Dispatchs_ID = "F0000025";

    // 钻孔班组长
    string Dispatchs_DrillingTeamLeader = "F0000090";

    // 精车班组长
    string Dispatchs_FinishingTeamLeader = "F0000089";

    // 精车计划完成时间
    string Dispatchs_FinishingPlanCompletionTime = "F0000005";

    // 取样计划完成时间
    string Dispatchs_SamplingPlanCompletionTime = "F0000003";
    // 粗车班组长
    string Dispatchs_RoughCutingTeamLeader = "F0000088";

    // 钻孔计划完成时间
    string Dispatchs_DrillingPlanCompletionTime = "F0000006";
    // 取样班组长
    string Dispatchs_SamplingTeamLeader = "F0000087";

    string Dispatchs_Objectid = "ObjectId";
}
