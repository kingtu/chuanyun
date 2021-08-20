using System;
using System.Collections.Generic;
using System.Text;

public class Register
{
    static string RegisterTable = "D001419bfcf04a7402c410593ba4cec28953075";
    static string usageTable = "8bf85ce3493444e2a7feff571236c4b0";

    public static void SetColumns(H3.IEngine Engine)
    {
        Schema r = new Schema(Engine, RegisterTable, true);
        //r.And("类名", "!=", "");
        Schema s = r.GetSubSchema("列结构");

        var list = r.GetList();
        if (list == null) { return; }
        foreach (var l in list)
        {
            r.CurrentRow = l;
            Register.SetColumns(Engine, s, r.Cell(Schema.RID), r.Cell("表名"));
            r.Update(true);
        }

    }
    public static void SetColumns(H3.IEngine Engine, Schema s, string currentRowID, string tableName)
    {
        Schema t = new Schema(Engine, tableName);
        var cs = t.Columns;
        foreach (var key in cs.Keys)
        {
            var existRow = s.ClearFilter().And(Schema.PID, "=", currentRowID).And("编码", "=", cs[key]).GetFirst(true);
            if (existRow == null) { s.GetNew(); }
            s["编码"] = cs[key];
            if (string.IsNullOrEmpty(s["名称"] + string.Empty)) { s["名称"] = key; }
            s["显示名称"] = key;
            s["是子表"] = t.SubTables == null ? false : t.SubTables.ContainsKey(key);
            s[Schema.PID] = currentRowID;
            if (existRow == null)
            { s.Create(); }
            else
            { s.Update(); }
        }
    }

    public static int SumUsage(H3.IEngine Engine, string[] idList)
    {
        Schema reg = new Schema(Engine, RegisterTable, true);
        Schema s = reg.GetSubSchema("列结构");

        Schema usage = new Schema(Engine, usageTable);

        int sum = 0;
        foreach (string id in idList)
        {
            var row = reg.GetRow(id);
            var slist = s.ClearFilter().And(Schema.PID, "=", reg[Schema.RID]).GetList();
            foreach (var l in slist)
            {
                s.CurrentRow = l;
                if (Schema.IsSysColumn(s.Cell("名称"))) { continue; }
                var uRow = usage.ClearFilter().And("名称", "=", s.Cell("名称")).GetFirst(true);
                if (uRow == null)
                {
                    usage.GetNew();
                    usage["名称"] = s.Cell("名称");
                    usage["次数"] = 1;
                    usage.Create();
                }
                else
                {
                    usage["次数"] = Convert.ToInt32(usage["次数"]) + 1;
                    usage.Update();
                }
            }
        }
        return sum;
    }

    public static string CreateCode(Schema reg)
    {
        Schema s = reg.GetSubSchema("列结构");
        var fields = s.ClearFilter().And(Schema.PID, "=", reg[Schema.RID]).GetList();
        if (fields == null) { return ""; }
        string appName = reg.Cell("应用名称");
        string tableName = reg.Cell("表名");
        string tableCode = reg.Cell("表ID");
        string className = reg.Cell("类名");
        string inherit = reg.Cell("继承");
        StringBuilder model = new StringBuilder("");


        //model.Append("namespace " + param.NameSpace + (param.NameSpace.IsNullOrEmpty() ? "" : ".") + param.CNSC.Model + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1) + "\r\n");
        //model.Append("{\r\n");
        //model.Append("\t[Serializable]\r\n");
        model.Append("\t/// <summary>\r\n");
        model.Append("\t/// " + appName + "," + tableName + "\r\n");
        model.Append("\t/// </summary>\r\n");
        model.Append("\t[Table(\"" + tableName + "\")]\r\n");
        model.Append("\tpublic class " + (string.IsNullOrEmpty(inherit) ? className : className + ":" + inherit) + "\r\n");
        model.Append("\t{\r\n");
        model.Append("\t\tH3.DataModel.BizObject inner;\r\n");
        model.Append("\t\tpublic static Dictionary<string, string> Columns = new Dictionary<string, string>();\r\n");
        model.Append("\t\tpublic static string TableCode=\"" + tableCode + "\";\r\n");
        model.Append("\t\tstatic " + className + "(){Init();}\r\n");
        model.Append("\t\tpublic " + className + "(H3.DataModel.BizObject bizObject)\r\n");
        model.Append("\t\t{\r\n");
        model.Append("\t\t\tinner = bizObject;\r\n");
        model.Append("\t\t\tInit();\r\n");
        model.Append("\t\t}\r\n");


        model.Append("\t\tstatic void Init()\r\n");
        model.Append("\t\t{\r\n");
        foreach (var field in fields)
        {
            s.CurrentRow = field;
            string fieldName = s.Cell("编码");
            string PropName = s.Cell("名称");
            string DisplayName = s.Cell("显示名称");
            model.Append("\t\t\t\tColumns.Add(\"" + DisplayName +"\",\"" + fieldName + "\")\r\n");
        }
        model.Append("\t\t}\r\n");
        

        foreach (var field in fields)
        {
            s.CurrentRow = field;

            string fieldName = s.Cell("编码");
            string PropName = s.Cell("名称");
            string DisplayName = s.Cell("显示名称");
            string Note = "";
            string DotNetType = "Object";
            string LPropName = PropName.ToUpper();
            model.Append("\t\tprivate string _" + PropName + "=\"" + fieldName + "\";\r\n");

            model.Append("\t\t/// <summary>\r\n");
            model.Append("\t\t/// " + DisplayName + "\r\n");
            model.Append("\t\t/// </summary>\r\n");

            //if (field.IsPrimaryKey)
            //{
            //    model.Append("\t\t[Key]\r\n");
            //}
            //if (!field.IsNull)
            //{
            //    model.Append("\t\t[Required(ErrorMessage = \"" + (field.Note.IsNullOrEmpty() ? field.Name : field.Note) + "不能为空\")]\r\n");
            //}

            model.Append("\t\t[Column(\"" + fieldName + "\")]\r\n");
            //model.Append("\t\t[DisplayName(\"" + (string.IsNullOrEmpty(PropName) ? PropName : DisplayName) + "\")]\r\n");
            model.Append("\t\tpublic " + DotNetType + " " + PropName + "\r\n");
            model.Append("\t\t{\r\n");
            model.Append("\t\t\tget { return " + "inner[_" + PropName + "];}\r\n");
            model.Append("\t\t\tset { inner[_" + PropName + "]=value;}\r\n");
            model.Append("\t\t}\r\n");

        }
        string Updates = @"
        public void Update(bool Effective = true)
        {
            if(Effective)
            {
                inner.Status = H3.DataModel.BizObjectStatus.Effective;
            }
            inner.Update();
        } ";

        model.Append(Updates + "\r\n");
        string removeS = @"
        public void Remove()
        {
            if(inner != null)
            {
                inner.Remove();
            }
        } ";
        model.Append(removeS + "\r\n");

        model.Append("\t}\r\n");
        return model.ToString();
    }
    public static string CreateCode(H3.IEngine Engine, string interfaceName)
    {
        Schema usage = new Schema(Engine, usageTable);
        var fields = usage.ClearFilter().GetList();//And("次数", "=", sum)
        StringBuilder model = new StringBuilder("");

        model.Append("\tpublic interface " + interfaceName + "\r\n");
        model.Append("\t{\r\n");

        foreach (var field in fields)
        {
            usage.CurrentRow = field;
            string PropName = usage.Cell("名称");
            string DotNetType = "Object";

            model.Append("\t\t" + DotNetType + " " + PropName + " { get; set; }\r\n");

        }
        model.Append("\t}\r\n");
        return model.ToString();

    }
    public static string CreateMap(H3.IEngine Engine)
    {
        Schema reg = new Schema(Engine, RegisterTable,true );
        var fields = reg.ClearFilter().And("表名", "!=", "").And("类名", "!=", "").GetList();
        StringBuilder model = new StringBuilder("");

        model.Append("\tpublic static Dictionary < string, string > GetColumns(string tableName)\r\n");
        model.Append("\t{\r\n");
        //if (tableName == "") { return Finishing.Columns; }
        foreach (var field in fields)
        {
            reg.CurrentRow = field;
            string tableName = reg.Cell("表名");
            string className = reg.Cell("类名");

            model.Append("\t\tif (tableName == \""+ tableName +  "\") { return " + className + ".Columns; }\r\n");

        }
        model.Append("\t\treturn null;\r\n");
        model.Append("\t}\r\n");
        return model.ToString();

    }
}

public class Fields
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public string Type { get; set; }
    /// <summary>
    /// 长度
    /// </summary>
    public int Length { get; set; }
    /// <summary>
    /// 是否可为空
    /// </summary>
    public bool IsNull { get; set; }
    /// <summary>
    /// 是否是主键
    /// </summary>
    public bool IsPrimaryKey { get; set; }
    /// <summary>
    /// 是否是自增列
    /// </summary>
    public bool IsIdentity { get; set; }
    /// <summary>
    /// 对应的.net类型
    /// </summary>
    public string DotNetType { get; set; }
    /// <summary>
    /// 对应的.net SQL类型
    /// </summary>
    public string DotNetSqlType { get; set; }
    /// <summary>
    /// 默认值 
    /// </summary>
    public string Default { get; set; }
    /// <summary>
    /// 备注说明
    /// </summary>
    public string Note { get; set; }
}


public class ClassName
{
    H3.DataModel.BizObject inner;

    public ClassName() { }

    ClassName(H3.DataModel.BizObject bizObject)
    {
        inner = bizObject;
    }
    //Dictionary<string, string> Columns = new Dictionary<string, string>();
   
    //void Init()
    //{
    //    Columns.Add(DisplayName, "FieldName");
    //}

    public const string _PropName = "FieldName";
    public object PropName
    {
        get { return inner[_PropName]; }
        set { inner[_PropName] = value; }
    }

    public void Create()
    { inner.Create(); }

}

public class ColumnAtrrbute:Attribute 
{ 
     public string Name { get; set; }
     public ColumnAtrrbute(string ColumnName) {
        Name = ColumnName;
    }
}

