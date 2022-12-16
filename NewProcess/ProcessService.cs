using Chuanyun;
using H3.DataModel;
using H3.Workflow.Instance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

public class ProcessService
{
    private H3.IEngine engine;

    private string schemaCode;

    private H3.IEngine Engine;

    private string InstanceId;

    BizObject mngObj;


    private WorkflowInstance TopInstance;

    private int MaxLevel = 4 + 1;  //暂定为4个 因为要从1开始设置，所以+1

    private int CurrentSyncLevel = 1;

    // public ProcessService(H3.IEngine engine)
    // {
    //     this.engine = engine;
    // }

    public ProcessService(H3.IEngine Engine, string InstanceId)
    {
        this.Engine = Engine;
        this.InstanceId = InstanceId;
        this.TopInstance = GetTopFlowInstance(InstanceId);
        BizObject processFlowObj = Tools.BizOperation.Load(engine, TopInstance.SchemaCode, TopInstance.ObjectId);
        string progressCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
        mngObj = Tools.BizOperation.Load(engine, progressCode, processFlowObj["Progress"].ToString());
    }
    public void SyncFlowProgress()
    {
        ClearSurplusLevle();
        CurrentSyncLevel = 1;
        SyncFlowProgress(TopInstance);
    }

    //更新本流程所有当前节点，obj为起始业务对象。
    public void UpdateAllCurrentNode(BizObject obj)
    {
        BizObject root = GetRootObjectBy(obj.WorkflowInstanceId);
        UpdateAllCurrentNodes(root);
    }
    //更新本流程所有当前节点，root为顶层业务对象。
    public void UpdateAllCurrentNodes(BizObject root)
    {
        string nodeFieldName = "本流程当前节点";
        string currentNodeFieldName = GetFieldNameBy(root.Schema.SchemaCode, nodeFieldName);
        List<int> tokenList = new List<int>();
        root[currentNodeFieldName] = GetRunningActivitysName(root.WorkflowInstanceId, root.Schema.SchemaCode, ref tokenList);
        root.Update();

        List<BizObject> ojblist = GetChildrenNodes(root, tokenList);
        foreach (BizObject r in ojblist)
        {
            UpdateAllCurrentNodes(r);
        }
    }
    //获取所有子流程对应的业务对象。
    public BizObject[] GetChildrenNodes(BizObject root)
    {
        //查询子流程实例
        System.Data.DataTable table = Engine.Query.QueryChildInstance(root.WorkflowInstanceId, Token.UnspecifiedId,
            WorkflowInstanceState.Running, H3.Data.BoolValue.Unspecified);
        BizObject[] objList = new BizObject[table.Rows.Count];

        for (int i = 0; i < table.Rows.Count; i++)
        {
            System.Data.DataRow row = table.Rows[i];
            string schemaCode = row["SchemaCode"] + string.Empty;
            string bizObjectId = row["BizObjectId"] + string.Empty;
            objList[i] = Tools.BizOperation.Load(Engine, schemaCode, bizObjectId);
        }
        return objList;
    }
    public List<BizObject> GetChildrenNodes(BizObject root, List<int> tokenList)
    {
        List<BizObject> bList = new List<BizObject>();
        if (tokenList == null) { return bList; }
        foreach (int id in tokenList)
        {
            //查询子流程实例
            System.Data.DataTable table = Engine.Query.QueryChildInstance(root.WorkflowInstanceId, id,
                WorkflowInstanceState.Unspecified, H3.Data.BoolValue.Unspecified);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                System.Data.DataRow row = table.Rows[i];
                string schemaCode = row["SchemaCode"] + string.Empty;
                string bizObjectId = row["BizObjectId"] + string.Empty;
                BizObject b = Tools.BizOperation.Load(Engine, schemaCode, bizObjectId);
                bList.Add(b);
            }
        }
        return bList;
    }

    //获取业务对象对应的最顶层业务对象（工艺流程表对象）。
    public BizObject GetRootObjectBy(string InstanceId)
    {
        WorkflowInstance instanc = Engine.WorkflowInstanceManager.GetWorkflowInstance(InstanceId);
        while (true)
        {
            WorkflowInstance tempInstanc = Engine.WorkflowInstanceManager.GetWorkflowInstance(instanc.ParentInstanceId);
            if (tempInstanc == null) { break; }
            instanc = tempInstanc;
        }

        return Tools.BizOperation.Load(Engine, instanc.SchemaCode, instanc.BizObjectId);// BizOperation.
    }
    //通过控件显示名称获取控件编码（字段名）
    public string GetFieldNameBy(string SchemaCode, string DisplayName)
    {
        H3.DataModel.BizObjectSchema bs = Engine.BizObjectManager.GetPublishedSchema(SchemaCode);
        foreach (PropertySchema p in bs.Properties)
        {
            if (p.DisplayName == DisplayName) { return p.Name; }
        }
        return null;
    }
    //获取本流程所有活动节点名字
    public string GetRunningActivitysName(string InstanceId, string schemaCode, ref List<int> tokenList)
    {
        string s = "";
        H3.Workflow.Template.WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(schemaCode);
        string sql = "SELECT Activity,TokenId FROM H_Token where ParentObjectId = '" + InstanceId + "' and State=0"; // and TokenId = '" + next + "'";
        //System.Data.DataTable dtToken = Engine.Query.QueryTable(sql, null);
        DataTable dt = Engine.Query.QueryTable(sql, null); // Engine.Query.GetInstanceTokens(InstanceId);      

        //DataRow[] rows = dt.Select("State=0");
        foreach (DataRow row in dt.Rows)
        {
            string activityCode = row["Activity"] + string.Empty;
            H3.Workflow.Template.Activity a = wfTemp.GetActivityByCode(activityCode);
            s += a.DisplayName;
            tokenList.Add(Convert.ToInt32(row["TokenId"] + string.Empty));
        }
        return s;
    }

    // private void FinshFlowProgress() {
    //     while(CurrentSyncLevel < MaxLevel)
    //     {
    //         string value = CurrentSyncLevel == 1 ? "完成" : string.Empty;
    //         FreshProgressLevelValue(CurrentSyncLevel, value);
    //         CurrentSyncLevel++;
    //     }
    // }

    private void SyncFlowProgress(WorkflowInstance instance)
    {
        string activityName = GetLastActivityName(instance);
        FreshProgressLevelValue(CurrentSyncLevel, activityName);

        BizObject processFlowObj = Tools.BizOperation.Load(engine, instance.SchemaCode, instance.ObjectId);
        string fieldName = GetFieldNameBy(instance.SchemaCode, "当前流程节点");
        processFlowObj[fieldName] = activityName;
        processFlowObj.Update();

        CurrentSyncLevel += 1;
        IToken token = instance.GetLastToken();
        System.Data.DataTable childs = Engine.Query.QueryChildInstance(instance.InstanceId, token.TokenId,
            WorkflowInstanceState.Unspecified, H3.Data.BoolValue.Unspecified);
        //childs数量大于0说明是子流程，需要递归处理
        if (childs.Rows.Count > 0)
        {
            H3.DataModel.BizObject obj = Tools.BizOperation.Load(Engine, (string)childs.Rows[0]["SchemaCode"], (string)childs.Rows[0]["BizObjectId"]);
            SyncFlowProgress(Engine.WorkflowInstanceManager.GetWorkflowInstance(obj.WorkflowInstanceId));
        }
    }
    private WorkflowInstance GetTopFlowInstance(string instanceId)
    {
        WorkflowInstance instance = Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId);
        if (instance == null)
        {
            throw new ApplicationException("未获取到流程实例 instanceId:" + instanceId);
        }
        string parentInstanceId = instance.ParentInstanceId;
        if (parentInstanceId == null || parentInstanceId == string.Empty)
        {
            return instance;
        }
        return GetTopFlowInstance(parentInstanceId);
    }

    private void ClearSurplusLevle()
    {
        int level = 0;
        while (level < MaxLevel)
        {
            FreshProgressLevelValue(level, string.Empty);
            level++;
        }
    }


    public void FreshProgressLevelValue(int level, string activityName)
    {
        // string field = level == 1 ? "Step1" : level == 2 ? "Step2" : level == 3 ? "Step3" : "";
        // if(field != "") { mngObj[field] = activityName; }

        switch (level)
        {
            case 1:
                mngObj["Step1"] = activityName;
                break;
            case 2:
                mngObj["Step2"] = activityName;
                break;
            case 3:
                mngObj["Step3"] = activityName;
                break;
            default:
                break;
        }
    }

    public string GetLastActivityName(string instanceId)
    {
        return GetLastActivityName(Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId));
    }
    public string GetLastActivityName(WorkflowInstance instance)
    {
        IToken token = instance.GetLastToken();
        if (TokenState.Running == token.State)
        {
            H3.Workflow.Template.WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(instance.SchemaCode);
            H3.Workflow.Template.Activity activity = wfTemp.GetActivityByCode(token.Activity);
            return activity.DisplayName;
        }
        return string.Empty;
    }


    // public ProcessService(H3.IEngine engine, string bizObjectId)
    // {
    //     this.engine = engine;
    //     me = Tools.BizOperation.Load(engine, schemaCode, bizObjectId);
    // }
    public void UpdateCurrentSection(string productId, string sectionName, string workflowInstanceId)
    {
        UpdateDemandCurrentSection(productId, sectionName);
        // UpdateProcessFlowCurrentSection(productId,sectionName);        
    }
    //《单件需求管理》表的TableCode
    public static string TableCode = "D001419a802bbdd6c71465bbb6b12bd5633722f";
    public static string DemandSectionName = "F0000004";//F0000004 当前生产工序
    public static string DemandProductIdFiledName = "F0000001";//单件需求ID F0000001

    //更新《单件需求管理》的当前生产工序
    public void UpdateDemandCurrentSection(string productId, string sectionName)
    {
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, DemandProductIdFiledName, H3.Data.ComparisonOperatorType.Equal, productId);
        //获取单件需求管理对象
        BizObject demandManageObj = Tools.BizOperation.GetFirst(engine, TableCode, filter);
        if (demandManageObj == null) return;
        demandManageObj[DemandSectionName] = sectionName;
        demandManageObj.Update();
    }


    // //《工艺流程》表的TableCode
    // public static string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";
    // public static string ProcessFlow_SectionName = "F0000018";//当前生产工序
    // public static string ProcessFlow_ProductIdFiledName = "F0000006";//ID

    // //更新《工艺流程》的当前生产工序
    // public void UpdateProcessFlowCurrentSection(string productId, string sectionName) {
    //     //获取单件需求管理对象
    //     H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
    //     Tools.Filter.And(filter, ProcessFlow_ProductIdFiledName, H3.Data.ComparisonOperatorType.Equal, productId);

    //     BizObject processFlowObj = Tools.BizOperation.GetFirst(engine, ProcessFlow_TableCode, filter);
    //     if(processFlowObj == null) return;
    //     processFlowObj[ProcessFlow_SectionName] = sectionName;
    //     processFlowObj.Update();
    // }

}
