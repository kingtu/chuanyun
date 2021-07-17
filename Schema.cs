
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using H3;
using H3.DataModel;

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
        //add by zzx
        if (Operator == "!=") { return H3.Data.ComparisonOperatorType.NotEqual; }
        if (Operator == ">") { return H3.Data.ComparisonOperatorType.Above; }
        if (Operator == "<") { return H3.Data.ComparisonOperatorType.Below; }
        if (Operator == ">=") { return H3.Data.ComparisonOperatorType.NotBelow; }
        if (Operator == "<=") { return H3.Data.ComparisonOperatorType.NotAbove; }
        if (Operator == "in") { return H3.Data.ComparisonOperatorType.In; }
        return H3.Data.ComparisonOperatorType.Equal;
    }
}
public class Schema
{
    //#Region  属性和初始化
    public const string RegisterTable = "i_D001419bfcf04a7402c410593ba4cec28953075";//RegisterTable
    public const string PID = "ParentObjectId";
    public const string RID = "ObjectId";
    //public const string WorkflowInstanceID = "workflowinstanceid";   
    private static Dictionary<string, string> Codes = new Dictionary<string, string> { };
    public static string[] SysColumns = {
        "objectid", "name", "createdby", "ownerid", "ownerdeptid", "createdtime",
        "modifiedby", "modifiedtime", "workflowinstanceid", "status", "parentobjectid",
        "创建人", "拥有者", "所属部门", "创建时间", "修改时间", "数据标题"
    };

    private string appID = "D001419";
    private string tableID;
    private string tableName;

    public string UserId;
    public H3.IEngine Engine = null;
    public H3.DataModel.BizObjectSchema TableSchema;
    public H3.Data.Filter.Filter Filter = new H3.Data.Filter.Filter();
    public Dictionary<string, string> Columns = new Dictionary<string, string>();
    public Dictionary<string, string> SubTables = new Dictionary<string, string>();

    public H3.DataModel.BizObject CurrentRow;
    public H3.SmartForm.SmartFormPostValue CurrentPostValue;
    public H3.SmartForm.ListViewPostValue CurrentPostValue2;



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
    //IsAllName为真时
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
        this.tableName = tableName;
        this.tableID = Code(tableName, Engine);
        this.CurrentPostValue = postData;
        Init();
    }
    public Schema(H3.IEngine Engine, H3.SmartForm.ListViewPostValue postData, string tableName)
    {
        this.Engine = Engine;
        this.tableName = tableName;
        this.tableID = Code(tableName, Engine);
        this.CurrentPostValue2 = postData;
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
    public Schema(H3.IEngine Engine, H3.DataModel.BizObject bo)
    {
        TableSchema = bo.Schema;
        Init(false);
    }
    private void Init(bool bo = true)
    {
        if (bo) { TableSchema = Get(); }
        foreach (PropertySchema p in TableSchema.Properties)
        {
            if (Columns.ContainsKey(p.DisplayName))
            {
                throw new Exception("存在相同的字段：" + p.DisplayName);
            }
            Columns.Add(p.DisplayName, p.Name);
            if (p.Name.StartsWith(appID)) { SubTables.Add(p.DisplayName, p.Name); }

        }
    }

    public static string Code(string tableName, H3.IEngine engine)
    {
        if (Codes.ContainsKey(tableName)) return Codes[tableName];
        if (engine == null) return "";
        string sql = string.Format("select tableID from " + RegisterTable + "  where tableName ='{0}'", tableName); //从RegisterTable取TableID
        DataTable dt = engine.Query.QueryTable(sql, null);
        int Count = dt.Rows.Count;
        if (Count > 0)
        {
            var r = (string)dt.Rows[0][0];
            Codes.Add(tableName, r);
            return r;
        }
        else
        {
            return "";
        }
    }
    //#EndRegion
    public bool CheckColumnName(string columnName)
    {
        if (!Columns.ContainsKey(columnName))
        {
            throw new Exception("表【" + this.tableName + "】不存在字段：" + columnName);
        }
        return true;
    }
    public string Cell(string columnName)
    {
        CheckColumnName(columnName);
        if (CurrentRow == null || CurrentRow[Columns[columnName]] == null) { return null; }
        return (string)CurrentRow[Columns[columnName]];

    }
    public object CellAny(string columnName)
    {
        CheckColumnName(columnName);
        if (CurrentRow == null || CurrentRow[Columns[columnName]] == null) { return null; }
        return CurrentRow[Columns[columnName]];
    }
    public void Cell(string columnName, string value)
    {
        CheckColumnName(columnName);
        if (Columns.ContainsKey(columnName)) { CurrentRow[Columns[columnName]] = value; }
        return;
    }
    public void Cell(string columnName, int value)
    {
        CheckColumnName(columnName);
        if (Columns.ContainsKey(columnName)) { CurrentRow[Columns[columnName]] = value; }
        return;
    }
    public void CellAny(string columnName, object value)
    {
        CheckColumnName(columnName);
        if (Columns.ContainsKey(columnName)) { CurrentRow[Columns[columnName]] = value; }
        return;
    }
    public object this[string columnName]
    {
        get { return CellAny(columnName); }
        set { CellAny(columnName, value); }
    }
    public object CellAny(string columnName, H3.DataModel.BizObject row)
    {
        CheckColumnName(columnName);
        return row[Columns[columnName]];
    }

    public void CellAny(string columnName, object value, H3.DataModel.BizObject row)
    {
        CheckColumnName(columnName);
        row[Columns[columnName]] = value;
        return;
    }

    public string PostValue(string columnName)
    {
        CheckColumnName(columnName);
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
    public object PostValueAny(string columnName)
    {
        CheckColumnName(columnName);
        if (CurrentPostValue == null && !(CurrentPostValue2 == null))
        {
            return CurrentPostValue2.Data[Columns[columnName]];
        }
        if (CurrentPostValue.Data.ContainsKey(Columns[columnName]))
        {
            return CurrentPostValue.Data[Columns[columnName]];
        }
        return null;

    }
    //H3.SmartForm.SmartFormPostValue CurrentPostData;
    public string PostValue(string columnName, H3.SmartForm.SmartFormPostValue postValue)
    {
        CheckColumnName(columnName);
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
    //该方法已经过期,即将去除
    [Obsolete("Will be removed in next version. Please use ‘And’ method instead.")]
    public Schema AndFilter(string columnName, string Operator, string value)
    {
        //构建过滤器
        if (Filter.Matcher == null)
        {
            Filter.Matcher = new H3.Data.Filter.And();
        }
        H3.Data.Filter.And matcher = (H3.Data.Filter.And)Filter.Matcher;//构造And匹配器
        CheckColumnName(columnName);
        var filed = Columns[columnName];
        matcher.Add(new H3.Data.Filter.ItemMatcher(filed, OperatorType.Get(Operator), value));
        Filter.Matcher = matcher;
        return this;
    }
    //该方法已经过期,即将去除
    [Obsolete("Will be removed in next version. Please use ‘Or’ method instead.")]
    public Schema OrFilter(string columnName, string Operator, string value)
    {
        //构建过滤器
        if (Filter.Matcher == null)
        {
            Filter.Matcher = new H3.Data.Filter.Or();
        }
        H3.Data.Filter.Or matcher = (H3.Data.Filter.Or)Filter.Matcher;//构造Or匹配器
        CheckColumnName(columnName);
        var filed = Columns[columnName];
        matcher.Add(new H3.Data.Filter.ItemMatcher(filed, OperatorType.Get(Operator), value));
        Filter.Matcher = matcher;
        return this;
    }
    public Schema And(string columnName, string Operator, string value)
    {
        CheckColumnName(columnName);
        var filed = Columns[columnName];
        H3.Data.Filter.And matcher = new H3.Data.Filter.And();//构造And匹配器        
        matcher.Add(new H3.Data.Filter.ItemMatcher(filed, OperatorType.Get(Operator), value));
        if (Filter.Matcher != null)
        {
            matcher.Add(Filter.Matcher);
        }
        Filter.Matcher = matcher;
        return this;
    }
    public Schema Or(string columnName, string Operator, string value)
    {
        CheckColumnName(columnName);
        var filed = Columns[columnName];
        H3.Data.Filter.Or matcher = new H3.Data.Filter.Or();//构造Or匹配器        
        matcher.Add(new H3.Data.Filter.ItemMatcher(filed, OperatorType.Get(Operator), value));
        if (Filter.Matcher != null)
        {
            matcher.Add(Filter.Matcher);
        }
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
            this.Or("ObjectId", "=", obj);
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
    public Schema GetSubSchema(string tableName)
    {
        CheckColumnName(tableName);
        return new Schema(Engine, Columns[tableName], true);
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
            if (Schema.IsSysColumn(p) || srcSchema.SubTables.ContainsKey(p) || !this.Columns.ContainsKey(p))
            {
                continue;
            }
            CellAny(p, srcSchema.CellAny(p));
        }
    }
    public void Copy(Schema srcSchema, Dictionary<string, string> mapping)
    {
        foreach (string p in mapping.Keys)
        {
            if (Schema.IsSysColumn(p) || srcSchema.SubTables.ContainsKey(mapping[p]))
            {
                continue;
            }
            CellAny(p, srcSchema.CellAny(mapping[p]));
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
            var v = PostValueAny(p);
            if (v == null || v == "")
            {
                continue;
            }
            CellAny(p, v);
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
    public void AdjustCurrentParticipant(string[] participant)
    {
        string activityCode = null;
        H3.Workflow.Instance.IToken tok = Engine.WorkflowInstanceManager.GetWorkflowInstance(CurrentRow.WorkflowInstanceId).GetLastToken();
        activityCode = tok.Activity;
        /*        
        tok.Activity：流程节点编码，string类型        
        tok.Approval：是否同意，H3.Data.BoolValue类型：
           H3.Data.BoolValue.True 是同意，        
           H3.Data.BoolValue.False 是不同意，        
           H3.Data.BoolValue.Unspecified 是未处理或节点被取消      
                
        tok.TokenId：流程步骤表（H_Token）ObjectId字段值，string类型        
        tok.Participants：流程审批人，string[]类型，每一个数组元素对应一个审批人用户ID        
        tok.CreatedTime：流程节点创建时间，DateTime类型        
        tok.FinishedTime：流程节点结束事件，DateTime类型        
        tok.UsedTime：当前节点从开始到结束耗时，TimeSpan类型           
        */
        H3.Workflow.Messages.AdjustParticipantMessage AdjustMessage = new H3.Workflow.Messages.AdjustParticipantMessage(CurrentRow.WorkflowInstanceId, activityCode, participant);
        Engine.WorkflowInstanceManager.SendMessage(AdjustMessage);
    }

    public static bool IsSysColumn(string columnName)
    {
        return Array.IndexOf(SysColumns, columnName.ToLower()) >= 0;
    }

}