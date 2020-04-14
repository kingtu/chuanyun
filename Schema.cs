
using System;
using System.Collections.Generic;
using System.Text;
using H3;
using H3.DataModel;

public class Schema : ISchema
{
    //#Region  属性和初始化
    public H3.IEngine Engine = null;
    private string appID = "D001419";
    private string tableID;
    public H3.DataModel.BizObjectSchema TableSchema;
    public string UserId;
    public H3.Data.Filter.Filter Filter = new H3.Data.Filter.Filter();
    public Dictionary<string, string> Columns = new Dictionary<string, string>();
    public Dictionary<string, string> SubTables = new Dictionary<string, string>();

    public static string PID = "ParentObjectId";
    public static string RID = "ObjectId";

    public H3.DataModel.BizObject CurrentRow;
    public H3.SmartForm.SmartFormPostValue CurrentPostValue;
    public H3.SmartForm.ListViewPostValue CurrentPostValue2;

    public static string[] SysColumns = {
        "objectid", "name", "createdby", "ownerid", "ownerdeptid", "createdtime",
        "modifiedby", "modifiedtime", "workflowinstanceid", "status", "parentobjectid",
        "创建人", "拥有者", "所属部门", "创建时间", "修改时间", "数据标题"
    };
    public static Dictionary<string, string> Codes = new Dictionary<string, string> {
    { "客户表", "a591077834db41ef80b149a5f3bc26d5"},
    { "A销售订单", "870afcb18d9f457b9214693574e084b1"},
    { "A-C工艺要求", "a92c3799f0f04e96925633745b875ec1"},
    { "A-B生产订单", "ec318333645544f2b2606b71fb3f1d75"},
    { "A-B-C生产任务", "c6d04ca47af148a886539db711a1eef6"},
    { "A-B-C-D产品计划表", "3a1066eb19cf4ab79b0ac8fb1b14554c"},
    { "锯切", "ba65c69eb46c4156b0eb25930bf50b93"},
    { "锻压", "91f7d70310e94b1482010c6c7bfb3bed"},
    { "辗环", "2d3e0018f778406386964c6580e29ddb"},
    { "热处理", "ad33a9451cb6493aa479b645e799d9a8"},
    { "毛坯", "ae3db5afbfae43c395a407e490871be3"},
    { "粗车", "c7965aa8ba594bb9af82e380e6ae4eaf"},
    { "精车", "cda5ef72fb694a7b8fcd52d6849b98e2"},
    { "钻孔", "e17ff7e2aa2f42edb7b88fc9132f1936"},
    { "粗焊", "4851abf7cf294285a4977a02c2810459"},
    { "精焊", "1be4a53701a84da598d12944bc84cc39"},
    { "成品库", "7071c4b0c8444f10a226a71514b1c0cc"},
    { "再生库", "a3b71d50b275427eacc82bfa12fb48f2"},
    { "其他物流任务", "ecb579eec8704a128b92fed6c8ae2c70"},
    { "冷加工设备表", "7beb1c7d16a54b2eb58331a2c807fa39"},
    { "车间设备", "f57558a43c944e73b2f1a6b1bfc32b94"},
    { "产品类别", "50a743c942da4709821d273780730402"},
    { "产品小类", "31e1fc7e25d8417dbe2f54a5bf6218bf"},
    { "工序表", "9016d53506b44f7d95ebbab5a05faf50"},
    { "工作中心", "34249153f48b41699801e38a93e62449"},
    { "设备工时系数表", "5ed7e837ecee4f97800877820d9a2f05"},
    { "产品参数表", "6b62f7decd924e1e8713025dc6a39aa5"},
    { "红绿灯", "ef522660ca8742599ddc0d54c02d3f2a"},
    { "锯切修改表", "JuQie"}
};
    public static Dictionary<string, string> ProcessState = new Dictionary<string, string> {
{ "01", "待上机"},
{ "02", "待下机"},
{ "03", "待检验"},
{ "04", "待转运"},
{ "05", "加工中"},
{ "06", "待装炉"},
{ "07", "加热中"},
{ "08", "待出炉"},
{ "09", "待出炉"},
{ "10", "冷却中"},
{ "11", "待上机"},
{ "12", "待下机"},
{ "13", "待检验"},
{ "14", "待转运"},
{ "15", "加工中"},
{ "16", "待装炉"},
{ "17", "加热中"},
{ "18", "待出炉"},
{ "19", "待出炉"},
{ "20", "冷却中"},
{ "21", "冷却中"},
{ "22", "待下机"},
{ "23", "待检验"},
{ "24", "待转运"},
{ "25", "加工中"},
{ "26", "待装炉"},
{ "27", "加热中"},
{ "28", "待出炉"},
{ "29", "待出炉"},
{ "30", "冷却中"}
};

    public Schema(H3.IEngine Engine, string tableID)
    {
        this.Engine = Engine;
        this.tableID = tableID;
        Init();
    }
    public Schema(H3.IEngine Engine, string tableID, bool IsAllName)
    {
        this.Engine = Engine;
        this.tableID = tableID;
        if (IsAllName) { appID = ""; }
        Init();
    }
    //标准时创建的表单
    public Schema(H3.IEngine Engine, string tableID, H3.SmartForm.SmartFormPostValue postData, bool IsAllName)
    {
        this.Engine = Engine;
        this.CurrentPostValue = postData;
        this.tableID = tableID;
        if (IsAllName) { appID = ""; }
        Init();
    }
    //H3.SmartForm.ListViewPostValue postValue

    public Schema(H3.IEngine Engine, string tableID, H3.SmartForm.ListViewPostValue postData, bool IsAllName)
    {
        this.Engine = Engine;
        this.CurrentPostValue2 = postData;
        this.tableID = tableID;
        if (IsAllName) { appID = ""; }
        Init();
    }

    public Schema(H3.IEngine Engine, string tableID, H3.SmartForm.SmartFormPostValue postData)
    {
        this.Engine = Engine;
        this.tableID = tableID;
        this.CurrentPostValue = postData;
        Init();
    }
    public Schema(H3.IEngine Engine, H3.SmartForm.SmartFormPostValue postData, string tableName)
    {
        this.Engine = Engine;
        this.tableID = Code(tableName);
        this.CurrentPostValue = postData;
        Init();
    }

    public Schema(H3.IEngine Engine, string appID, string tableID)
    {
        this.Engine = Engine;
        this.appID = appID;
        this.tableID = tableID;
        Init();
    }
    public Schema(H3.IEngine Engine, string appID, string tableID, H3.SmartForm.SmartFormPostValue postData)
    {
        this.Engine = Engine;
        this.appID = appID;
        this.tableID = tableID;
        this.CurrentPostValue = postData;
        Init();
    }
    public Schema(H3.IEngine Engine, string appID, string tableID, string UserId)
    {
        this.Engine = Engine;
        this.appID = appID;
        this.tableID = tableID;
        this.UserId = UserId;
        Init();
    }
    public Schema(H3.IEngine Engine, string appID, string tableID, string UserId, H3.SmartForm.SmartFormPostValue postData)
    {
        this.Engine = Engine;
        this.appID = appID;
        this.tableID = tableID;
        this.UserId = UserId;
        this.CurrentPostValue = postData;
        Init();
    }
    private void Init()
    {
        TableSchema = Get();
        foreach (PropertySchema p in TableSchema.Properties)
        {
            Columns.Add(p.DisplayName, p.Name);
            if (p.Name.StartsWith(appID)) { SubTables.Add(p.DisplayName, p.Name); }

        }
    }
    public static string Code(string tableName)
    {
        if (Codes.ContainsKey(tableName)) { return Codes[tableName]; }
        return "";
    }
    //#EndRegion
    public string Cell(string columnName)
    {
        return (string)CurrentRow[Columns[columnName]];
    }
    public object CellAny(string columnName)
    {
        return CurrentRow[Columns[columnName]];
    }
    public void Cell(string columnName, string value)
    {
        if (Columns.ContainsKey(columnName)) { CurrentRow[Columns[columnName]] = value; }
        return;
    }

    public string Cell(string columnName, H3.DataModel.BizObject row)
    {
        return (string)row[Columns[columnName]];
    }

    public void Cell(string columnName, string value, H3.DataModel.BizObject row)
    {
        row[Columns[columnName]] = value;
        return;
    }

    public string PostValue(string columnName)
    {
        if (CurrentPostValue == null && !(CurrentPostValue2 == null))
        {
            return (string)CurrentPostValue2.Data[Columns[columnName]];
        }
        if (CurrentPostValue.Data.ContainsKey(Columns[columnName]))
        {
            return (string)CurrentPostValue.Data[Columns[columnName]];
        }
        return null;

    }
    //H3.SmartForm.SmartFormPostValue CurrentPostData;
    public string PostValue(string columnName, H3.SmartForm.SmartFormPostValue postValue)
    {
        if (postValue.Data.ContainsKey(Columns[columnName]))
        {
            return (string)postValue.Data[Columns[columnName]];
        }
        return null;
    }
    public H3.DataModel.BizObjectSchema Get()
    {
        return Engine.BizObjectManager.GetPublishedSchema(appID + tableID);
    }

    public H3.DataModel.BizObjectSchema Get(H3.IEngine Engine, string appID, string tableID)
    {
        return Engine.BizObjectManager.GetPublishedSchema(appID + tableID);
    }

    public Schema ClearFilter()
    {
        Filter = new H3.Data.Filter.Filter();
        return this;
    }

    public Schema AndFilter(string filed, string Operator, string value)
    {
        //构建过滤器
        if (Filter.Matcher == null)
        {
            Filter.Matcher = new H3.Data.Filter.And();
        }
        H3.Data.Filter.And matcher = (H3.Data.Filter.And)Filter.Matcher;//构造And匹配器
        matcher.Add(new H3.Data.Filter.ItemMatcher(filed, OperatorType.Get(Operator), value));
        Filter.Matcher = matcher;
        return this;
    }

    public Schema OrFilter(string filed, string Operator, string value)
    {
        //构建过滤器
        if (Filter.Matcher == null)
        {
            Filter.Matcher = new H3.Data.Filter.Or();
        }
        H3.Data.Filter.Or matcher = (H3.Data.Filter.Or)Filter.Matcher;//构造Or匹配器
        matcher.Add(new H3.Data.Filter.ItemMatcher(filed, OperatorType.Get(Operator), value));
        Filter.Matcher = matcher;
        return this;
    }

    public H3.DataModel.BizObject[] GetList()
    {
        H3.DataModel.BizObject[] Objects = H3.DataModel.BizObject.GetList(Engine, UserId, TableSchema, H3.DataModel.GetListScopeType.GlobalAll, Filter); //查询返回的结果对象
        return Objects;
    }
    public H3.DataModel.BizObject GetFirst()
    {
        H3.DataModel.BizObject[] Objects = H3.DataModel.BizObject.GetList(Engine, UserId, TableSchema, H3.DataModel.GetListScopeType.GlobalAll, Filter); //查询返回的结果对象
        if (Objects != null && Objects.GetLength(0) > 0) { return Objects[0]; }
        return null;
    }
    //true   设置当前行  同时移动指针
    public H3.DataModel.BizObject GetFirst(bool setCurrentRow)
    {
        H3.DataModel.BizObject[] Objects = H3.DataModel.BizObject.GetList(Engine, UserId, TableSchema, H3.DataModel.GetListScopeType.GlobalAll, Filter); //查询返回的结果对象
        if (Objects != null && Objects.GetLength(0) > 0)
        {
            if (setCurrentRow) { CurrentRow = Objects[0]; }
            return Objects[0];
        }
        if (setCurrentRow) { CurrentRow = null; }
        return null;
    }

    public H3.DataModel.BizObject[] GetRows(string[] objs)
    {
        this.ClearFilter();
        foreach (string obj in objs)
        {
            this.OrFilter("ObjectId", "=", obj);
        }
        return GetList();

    }
    public H3.DataModel.BizObject GetRow(string BizObjectId)
    {  //Load对象（schemaCode为表单编码，this.Request.BizObjectId为当前表单objectid）
        H3.DataModel.BizObject obj = H3.DataModel.BizObject.Load(UserId, Engine, appID + tableID, BizObjectId, false); //查询返回的结果对象
                                                                                                                       //obj.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
        CurrentRow = obj;
        return obj;
    }
    public void Update()
    {
        CurrentRow.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
        CurrentRow.Update();
    }
    //更新数据  数据是否生效  
    //参数是是TURE的时候  生效状态
    //参数是是FALSE的时候  不生效状态
    public void Update(bool Effective)
    {
        if (Effective)
        {
            CurrentRow.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
        }
        CurrentRow.Update();
    }
    public void GetNew()
    {
        H3.DataModel.BizObject newOne = new H3.DataModel.BizObject(Engine, TableSchema, UserId);
        CurrentRow = newOne;
    }

    public void Create()
    {
        CurrentRow.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
        CurrentRow.Create();
    }
    //新建数据  数据是否生效  
    //参数是是TURE的时候  生效状态
    //参数是是FALSE的时候  不生效状态
    public void Create(bool Effective)
    {
        if (Effective)
        {
            CurrentRow.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
        }
        CurrentRow.Create();
    }
    public void Remove()
    {
        //CurrentRow.Status = H3.DataModel.BizObjectStatus.Effective; // 将对象状态设为生效
        if (CurrentRow != null)
        {
            CurrentRow.Remove();
        }
    }

    public void Copy(Schema srcSchema)
    {
        foreach (string p in srcSchema.Columns.Keys)
        {
            if (Schema.IsSysColumn(p) || srcSchema.SubTables.ContainsKey(p))
            {
                continue;
            }
            Cell(p, srcSchema.Cell(p));
        }
    }
    public void CopyPostValue()
    {
        foreach (string p in Columns.Keys)
        {
            if (Schema.IsSysColumn(p) || SubTables.ContainsKey(p))
            {
                continue;
            }
            var v = PostValue(p);
            if (v == null || v == "")
            {
                continue;
            }
            Cell(p, v);
        }
    }

    public bool RunActivity(string currentActivityCode, string nextActivityCode)
    {
        //获取新流程ID    
        if (string.IsNullOrEmpty(CurrentRow.WorkflowInstanceId))
        {
            CurrentRow.WorkflowInstanceId = System.Guid.NewGuid().ToString();
        }
        CurrentRow.Update();  //存储记录。

        H3.Workflow.Messages.CancelActivityMessage cancelMessage = new H3.Workflow.Messages.CancelActivityMessage(CurrentRow.WorkflowInstanceId, currentActivityCode, false);
        var r = Engine;
        var m = r.WorkflowInstanceManager;
        Engine.WorkflowInstanceManager.SendMessage(cancelMessage);

        H3.Workflow.Messages.ActivateActivityMessage activiteMessage = new H3.Workflow.Messages.ActivateActivityMessage(CurrentRow.WorkflowInstanceId,
            nextActivityCode,
            H3.Workflow.Instance.Token.UnspecifiedId,
            new string[] { }, null, false,
            H3.Workflow.WorkItem.ActionEventType.Adjust);
        //参数对应描述：流程实例ID，活动节点编码，令牌ID，参与者，前驱令牌，是否检测入口条件，激活类型
        Engine.WorkflowInstanceManager.SendMessage(activiteMessage);//1.不会取消正在运行的节点。2.进行中的流程才能激活调整。
        return true;
    }

    public bool StartNewActivity()
    {
        //获取新流程ID  
        if (string.IsNullOrEmpty(CurrentRow.WorkflowInstanceId))
        {
            CurrentRow.WorkflowInstanceId = System.Guid.NewGuid().ToString();
        }
        CurrentRow.Update();  //存储记录。

        H3.Workflow.Instance.WorkflowInstance wfInstance = Engine.WorkflowInstanceManager.GetWorkflowInstance(CurrentRow.WorkflowInstanceId);
        if (wfInstance == null)
        {
            //启动流程
            string workItemID = string.Empty;
            string errorMsg = string.Empty;
            H3.Workflow.Template.WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(CurrentRow.Schema.SchemaCode);
            Engine.Interactor.OriginateInstance(UserId,
                CurrentRow.Schema.SchemaCode,
                wfTemp.WorkflowVersion,
                CurrentRow.ObjectId,
                CurrentRow.WorkflowInstanceId,
                H3.Workflow.WorkItem.AccessMethod.Web,
                true, string.Empty, true,
                out workItemID, out errorMsg);
        }//第七个参数 false/true 为是否提交流程操作
        return true;
    }


    public static bool IsSysColumn(string columnName)
    {
        return Array.IndexOf(SysColumns, columnName.ToLower()) >= 0;
    }

}

public  class PropertySchema
{
    public string Name { get;  set; }
    public string DisplayName { get;  set; }
}

public class OperatorType
{
    private string Operator;
    public OperatorType() { }

    public static H3.Data.ComparisonOperatorType Get(string Operator)
    {
        if (Operator == "=") { return H3.Data.ComparisonOperatorType.Equal; }
        if (Operator == ">") { return H3.Data.ComparisonOperatorType.Above; }
        if (Operator == "<") { return H3.Data.ComparisonOperatorType.Below; }
        if (Operator == ">=") { return H3.Data.ComparisonOperatorType.NotBelow; }
        if (Operator == "<=") { return H3.Data.ComparisonOperatorType.NotAbove; }
        if (Operator == "in") { return H3.Data.ComparisonOperatorType.In; }

        return H3.Data.ComparisonOperatorType.Equal;
    }
}


