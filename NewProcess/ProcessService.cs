
using System;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;
using H3.Workflow.Instance;
using System.Data;
using H3.Workflow;
using H3.Workflow.Template;


public class ProcessService
{
    private BizObject me;

    private H3.IEngine Engine;

    private string InstanceId;

    BizObject processFlowObj; //《工艺流程》表记录。


    private WorkflowInstance TopInstance;

    private int MaxLevel = 3 + 1;  //暂定为4个 因为要从1开始设置，所以+1 

    private int CurrentSyncLevel;


    public ProcessService(H3.IEngine Engine, string InstanceId)
    {
        this.Engine = Engine;
        this.InstanceId = InstanceId;
        this.TopInstance = GetTopFlowInstance(InstanceId);
        if (TopInstance.SchemaCode != ProcessFlow_TableCode)
        {
            throw new ApplicationException(Const.LocalMessageTableErrorChekConst.ParamsError);
        }
        //《工艺流程表》表记录
        processFlowObj = Tools.BizOperation.Load(Engine, TopInstance.SchemaCode, TopInstance.BizObjectId);
        //《生产进度管理》表记录。
        me = Tools.BizOperation.Load(Engine, progressCode, processFlowObj[Progress].ToString());
    }


    public void SyncFlowProgress(string worker, string team, string device)
    {
        ClearSurplusLevle();
        CurrentSyncLevel = 1;
        SyncFlowProgress(TopInstance);
        SyncProgressPostion(worker, team, device);
        me.Update();
    }
    public void SyncProgressPostion(string worker, string team, string device)
    {

        Postion postion = new Postion(Engine, Postion.ToString());
        Worker = worker;
        Team = team;
        Device = device;

        WorkShip = postion.WorkShip;
        Zone = postion.Zone;
    }

    //通过控件显示名称获取控件编码（字段名）
    public string GetFieldNameBy(string SchemaCode, string DisplayName)
    {
        BizObjectSchema bs = Engine.BizObjectManager.GetPublishedSchema(SchemaCode);
        foreach (PropertySchema p in bs.Properties)
        {
            if (p.DisplayName == DisplayName) { return p.Name; }
        }
        return null;
    }
    //获取本流程所有活动节点名字
    public string GetRunningActivitysName(string InstanceId, string schemaCode) //, ref List < int > tokenList
    {
        string names = "";
        H3.Workflow.Template.WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(schemaCode);
        string sql = "SELECT Activity,TokenId FROM H_Token where ParentObjectId = '" + InstanceId + "' and State=0"; // and TokenId = '" + next + "'";     
        DataTable dtToken = Engine.Query.QueryTable(sql, null); // Engine.Query.GetInstanceTokens(InstanceId);     
        foreach (DataRow row in dtToken.Rows)
        {
            string activityCode = row["Activity"] + string.Empty;
            H3.Workflow.Template.Activity a = wfTemp.GetActivityByCode(activityCode);
            names = a.DisplayName;
            //tokenList.Add(Convert.ToInt32(row["TokenId"] + string.Empty));
        }
        return names;
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
        string activityName = GetRunningActivitysName(instance.InstanceId, instance.SchemaCode);// GetLastActivityName(instance);
        FreshProgressLevelValue(CurrentSyncLevel, activityName);

        BizObject processCurrentFlow = Tools.BizOperation.Load(Engine, instance.SchemaCode, instance.BizObjectId);
        //GetFieldNameBy(instance.SchemaCode, "当前流程节点");
        try
        {
            processCurrentFlow["CurrentNode"] = activityName;
            processCurrentFlow.Update();
        }
        catch (Exception e)
        {
            string message = processCurrentFlow.Schema.DisplayName + "缺少CurrentNode";
            ExceptionCompoent.Log(Engine, message);

        }

        CurrentSyncLevel += 1;
        IToken token = instance.GetLastToken();
        DataTable childs = Engine.Query.QueryChildInstance(instance.InstanceId, token.TokenId,
            WorkflowInstanceState.Unspecified, H3.Data.BoolValue.Unspecified);
        //childs数量大于0说明是子流程，需要递归处理
        if (childs.Rows.Count > 0)
        {
            BizObject obj = Tools.BizOperation.Load(Engine, (string)childs.Rows[0]["SchemaCode"], (string)childs.Rows[0]["BizObjectId"]);
            SyncFlowProgress(Engine.WorkflowInstanceManager.GetWorkflowInstance(obj.WorkflowInstanceId));
        }
    }
    private WorkflowInstance GetTopFlowInstance(string instanceId)
    {
        WorkflowInstance instance = Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId);
        if (instance == null)
        {
            throw new ApplicationException(Const.LocalMessageTableErrorChekConst.ParamsError);
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
        int level = 2;
        while (level < MaxLevel)
        {
            FreshProgressLevelValue(level, string.Empty);
            level++;
        }
    }


    public void FreshProgressLevelValue(int level, string activityName)
    {
        switch (level)
        {
            case 1:
                Step1 = activityName;
                // new SingleDemandManageService(Engine).UpdateCurrentProductionOperation(processFlowObj.ObjectId, activityName);
                break;
            case 2:
                Step2 = activityName;
                break;
            case 3:
                Step3 = activityName;
                break;
            default:
                break;
        }
    }

    public string GetLastActivityName(WorkflowInstance instance)
    {
        IToken token = instance.GetLastToken();
        if (TokenState.Running == token.State)
        {
            WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(instance.SchemaCode);
            Activity activity = wfTemp.GetActivityByCode(token.Activity);
            return activity.DisplayName;
        }
        return string.Empty;
    }

    //《生产进度管理》表Code
    string progressCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd"; //Sq0biizim9l50i2rl6kgbpo3u4
    string step1 = "Step1", step2 = "Step2", step3 = "Step3";
    string postionField = "F0000083";
    string workShip = "F0000073";//车间
    string progressZone = "F0000074";//区域
    string progressWorker = "F0000005";//工人
    string progressTeam = "F0000006";//班组
    string progressDevice = "F0000007";//设备

    public object WorkShip
    {
        get { return me[workShip]; }
        set { me[workShip] = value; }
    }
    public object Zone
    {
        get { return me[progressZone]; }
        set { me[progressZone] = value; }
    }

    public object Worker
    {
        get { return me[progressWorker]; }
        set { me[progressWorker] = value; }
    }

    public object Team
    {
        get { return me[progressTeam]; }
        set
        {
            me[progressTeam] = value;
        }
    }
    public object Device
    {
        get { return me[progressDevice]; }
        set
        {
            me[progressDevice] = value;
        }
    }

    public object Postion
    {
        get { return me[postionField]; }
        set { me[postionField] = value; }

    }
    public object Step1
    {
        get { return me[step1]; }
        set { me[step1] = value; }
    }
    public object Step2
    {
        get { return me[step2]; }
        set { me[step2] = value; }
    }
    public object Step3
    {
        get { return me[step3]; }
        set { me[step3] = value; }
    }

    //《工艺流程》表TableCode
    public static string ProcessFlow_TableCode = "D001419Sq0biizim9l50i2rl6kgbpo3u4";
    public static string Progress = "Progress";//《工艺流程》表中的关联表《生产进度管理》的控件名

}
public class Postion
{
    private H3.IEngine Engine;
    private BizObject me;


    public Postion(H3.IEngine Engine, string bizObjectId)
    {
        this.Engine = Engine;
        me = Tools.BizOperation.Load(Engine, TableCode, bizObjectId);
    }
    public object WorkShip
    {
        get { return me[postionWorkShip]; }
        set { me[postionWorkShip] = value; }
    }
    public object Zone
    {
        get { return me[postionZone]; }
        set { me[postionZone] = value; }
    }
    public void Update()
    {
        me.Update();
    }
    string TableCode = "D001419Sapkihcly8tgus2a7d6rnngp94";//Sapkihcly8tgus2a7d6rnngp94
    string postionWorkShip = "F0000011";
    string postionZone = "F0000012";
}