using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class Dispatch
{
    Schema me;
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
        me = new Schema(Engine, "派工表");
        me.ClearFilter()
            .And("ID", "=", ID)
            .GetFirst(true);
    }

    public bool GetOrder(string processName)
    {
        object o = null;
        switch (processName)
        {
            case "粗车":
                o = me["粗车不限制派工顺序"];
                break;
            case "精车":
                o = me["精车不限制派工顺序"];
                break;
            case "钻孔":
                o = me["钻孔不限制派工顺序"];
                break;
            case "取样":
                o = me["取样不限制派工顺序"];
                break;
        }
        if (o == null) { return false; }
        //var n=NameOf(o);
        return (bool)o;


    }
    public string[] GetWorkShop(string processName)
    {
        string[] o = new string[] { null, null };
        switch (processName)
        {
            case "取样":
                o[0] = (string)me["取样车间名称"];
                o[1] = (string)me["取样车间位置"];
                break;
            case "粗车":
                o[0] = (string)me["粗车车间名称"];
                o[1] = (string)me["粗车车间位置"];
                break;
            case "精车":
                o[0] = (string)me["精车车间名称"];
                o[1] = (string)me["精车车间位置"];
                break;
            case "钻孔":
                o[0] = (string)me["钻孔车间名称"];
                o[1] = (string)me["钻孔车间位置"];
                break;
        }
        //if(o == null) { return null; }
        return o;
    }

    public string[] GetManager(string processName, string position)
    {
        var scmMng = new Schema(Engine, "车间管理人员");
        scmMng.And("车间", "=", position).GetFirst(true);
        var f = processName == "粗车" ? "粗车派工角色" : processName == "精车" ? "精车派工角色" : processName == "钻孔" ? "钻孔派工角色" : "取样派工角色";
        var ro = scmMng.Cell(f);
        var manager = Role.GetUsers(ro, Engine);
        return manager;
    }

    public string[] GetStagePerson(string processName, Dictionary<string, double> task)
    {
        bool order = GetOrder(processName);
        List<string> g = new List<string>();

        H3.DataModel.BizObject[] persons = (H3.DataModel.BizObject[])me[processName];
        if (persons == null) { return g.ToArray(); }
        Schema s = me.GetSubSchema(processName);
        foreach (H3.DataModel.BizObject p in persons)
        {
            s.CurrentRow = p;
            double totalLoad = 0;
            string name = (string)s["姓名"];
            double planload = (double)s["加工量"];

            if (task != null && task.ContainsKey(name))
            {
                totalLoad = task[name];
                if (totalLoad > planload)
                {
                    throw new Exception("您填写的报工量大于给您的安排的加工量！");
                }
            }

            if (order)//无序
            {
                if (totalLoad < planload)
                {
                    g.Add(name);
                }

            }
            else//有序
            {
                if (totalLoad < planload && g.Count == 0)
                {
                    g.Add(name);
                }
            }
        }

        return g.ToArray();
    }

    public Dictionary<string, double> GetCompletedTask(Schema main, string ProcessTableName)
    {
        H3.DataModel.BizObject[] list = main[ProcessTableName] as H3.DataModel.BizObject[];
        Schema s = main.GetSubSchema(ProcessTableName);
        Dictionary<string, double> result = new Dictionary<string, double>();
        if (list != null)
        {
            foreach (H3.DataModel.BizObject r in list)
            {
                s.CurrentRow = r;
                string name = s["加工者"] + string.Empty;
                var t = s["加工量"];
                double load = Convert.ToDouble(t);
                //if(ProcessTableName == "取样") { load = load / 2; }
                if (string.IsNullOrEmpty(name)) { continue; }
                if (!result.ContainsKey(name))
                {
                    result.Add(name, load);
                }
                else
                {
                    result[name] += load;
                }
            }
        }
        return result;
    }

    ///填充派工所派车间
    public void FillWorkShop(Schema target, string 工序名称)
    {

        var ws = GetWorkShop(工序名称);
        target["转运车间"] = ws[0];
        target["转运位置"] = ws[1];
        target.Update(false);
    }
    ///填充派工所派人员
    public string[] FillPerson(Schema target, string 工序名称, string SubTableName = "机加工信息")
    {
        Dictionary<string, double> task = GetCompletedTask(target, SubTableName);
        string[] person = GetStagePerson(工序名称, task);
        string[] manager = GetManager(工序名称, Convert.ToString(target["当前车间"]));
        string[] productor = person.Length != 0 ? person : manager;
        target["工人"] = productor;
        target.Update(false);
        return productor;
    }
}