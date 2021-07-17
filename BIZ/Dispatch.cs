using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class Dispatch
{
    Schema scm;
    string ID;
    H3.IEngine Engine;

    public Dispatch(H3.IEngine Engine)
    {
        this.Engine = Engine;
    }
    public Dispatch(H3.IEngine Engine, string ID)
    {
        this.Engine = Engine;
        this.ID = ID;
        inti();
    }
    private void inti()
    {
        scm = new Schema(Engine, (H3.SmartForm.SmartFormPostValue)null, "工序计划表");
        scm.ClearFilter()
            .And("ID", "=", ID)
            .GetFirst(true);
    }

    public bool GetOrder(string processName)
    {
        object o = null;
        switch (processName)
        {
            case "粗车":
                o = scm.CellAny("粗车人员有序派工");
                break;
            case "精车":
                o = scm.CellAny("精车人员有序派工");
                break;
            case "钻孔":
                o = scm.CellAny("钻孔人员有序派工");
                break;
        }
        if (o == null) { return false; }
        return (bool)o;
    }
    public string[] GetPerson(string processName)
    {
        string[] p = null;
        switch (processName)
        {
            case "粗车":
                p = (string[])scm.CellAny("粗车人员");
                break;
            case "精车":
                p = (string[])scm.CellAny("精车人员");
                break;
            case "钻孔":
                p = (string[])scm.CellAny("钻孔人员");
                break;
        }
        return p;
    }
    public string[] GetManager(string processName, string position)
    {
        var scmMng = new Schema(Engine, (H3.SmartForm.SmartFormPostValue)null, "车间管理人员");
        scmMng.And("车间", "=", position)
            .GetFirst(true);
        var f = processName == "粗车" ? "粗车派工角色" : processName == "精车" ? "精车派工角色" : "钻孔派工角色";
        var ro = scmMng.Cell(f);
        var manager = Role.GetUsers(ro, Engine);
        return manager;
    }
}