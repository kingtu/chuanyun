
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using H3;

public class Dispatch
{
    DataRow me;
    string ID;
    H3.IEngine Engine;

    //public Dispatch(H3.IEngine Engine)
    //{
    //    this.Engine = Engine;
    //}
    /// <summary>
    /// 获取派工信息
    /// </summary>
    /// <param name="Engine">H3.IEngine</param>
    /// <param name="ID">产品ID</param>
    public Dispatch(H3.IEngine Engine, string ID)
    {
        this.Engine = Engine;
        this.ID = ID;
        inti();
    }
    private void inti()
    {
        //me = new Schema(Engine, "派工表");
        //me.And("ID", "=", ID).GetFirst(true);
        string w = Dispatchs.ID + "='" + ID + "'";
        me = GetRow(Dispatchs.TableCode, w);
    }
    /// <summary>
    /// 根据工序名称获取派工顺序是否受限制。
    /// </summary>
    /// <param name="processName"></param>
    /// <returns>true：不限制；false：限制</returns>
    public bool GetOrder(string processName)
    {
        object o = null;
        switch (processName)
        {
            case "粗车":
                o = me[Dispatchs.RoughTurningUnlimitedDispatchSequence];
                break;
            case "精车":
                o = me[Dispatchs.FinishTurningUnlimitedDispatchSequence];
                break;
            case "钻孔":
                o = me[Dispatchs.DrillingUnlimitedDispatchSequence];
                break;
            case "取样":
                o = me[Dispatchs.SamplingUnlimitedDispatchSequence];
                break;
        }
        if (o == null) { return false; }
        //var n=NameOf(o);
        return (bool)o;


    }

    /// <summary>
    /// 获取车间管理人员
    /// </summary>
    /// <param name="processName">工序名称</param>
    /// <param name="position">车间位置</param>
    /// <returns></returns>
    public string[] GetManager(string processName, string position)
    {
        //var scmMng = new Schema(Engine, "车间管理人员");
        //scmMng.And("车间", "=", position).GetFirst(true);

        string w = WorkshopManager.Workshop + "='" + position + "'";
        DataRow scmMng = GetRow(WorkshopManager.TableCode, w);
        var f = processName == "粗车" ? WorkshopManager.RoughTurningDispatchingRole :
                processName == "精车" ? WorkshopManager.FineTurningDispatchingRole :
                processName == "钻孔" ? WorkshopManager.DrillingDispatchingRole : WorkshopManager.SamplingDispatchingRole;
        string ro = (string)scmMng[f];
        var manager = Tools.Role.GetUsers(Engine, ro);
        return manager;
    }

    public string[] GetStagePerson(string processName, Dictionary<string, double> task)
    {
        bool order = GetOrder(processName);
        List<string> g = new List<string>();
        string pTask = processName == "粗车" ? Dispatchs.RoughTurning :
                      processName == "精车" ? Dispatchs.FinishTurning :
                      processName == "钻孔" ? Dispatchs.Drilling : Dispatchs.Sampling;
        //"姓名"
        string tName = GetPersonField(processName);
        //"加工量"
        string tMount = GetMountField(processName);
        string w = DispatchRoughSubTable.Parentobjectid + "='" + me[Dispatchs.Objectid] + "'";
        DataRowCollection personsTask = GetRows((string)me[pTask], w);
        if (personsTask == null) { return g.ToArray(); }
        //Schema s = me.GetSubSchema(processName);

        foreach (DataRow p in personsTask)
        {

            double totalLoad = 0;
            string name = (string)p[tName];
            double planload = (double)p[tMount];

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


    public Dictionary<string, double> GetCompletedTask(string processName, H3.DataModel.BizObject[] processSubTableRows)
    {
        //H3.DataModel.BizObject[] list = main[ProcessTableName] as H3.DataModel.BizObject[];
        //Schema s = main.GetSubSchema(ProcessTableName);
        Dictionary<string, double> result = new Dictionary<string, double>();
        if (processSubTableRows != null)
        {
            foreach (H3.DataModel.BizObject r in processSubTableRows)
            {
                //s.CurrentRow = r;
                string name = r[GetPersonField2(processName)] + string.Empty;
                var t = r[GetMountField2(processName)];
                double load = Convert.ToDouble(t);
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

    /// <summary>
    /// 获取派工表中指定的车间
    /// </summary>
    /// <param name="processName">工序名称</param>
    /// <returns></returns>
    public string[] GetWorkShop(string processName)
    {
        string[] o = new string[] { null, null };
        switch (processName)
        {
            case "取样":
                o[0] = (string)me[Dispatchs.SamplingWorkshopName];
                o[1] = (string)me[Dispatchs.SamplingWorkshopLocation];
                break;
            case "粗车":
                o[0] = (string)me[Dispatchs.RoughTurningWorkshopName];
                o[1] = (string)me[Dispatchs.RoughTurningWorkshopLocation];
                break;
            case "精车":
                o[0] = (string)me[Dispatchs.FinishTurningWorkshopName];
                o[1] = (string)me[Dispatchs.FinishTurningWorkshopLocation];
                break;
            case "钻孔":
                o[0] = (string)me[Dispatchs.DrillingWorkshopName];
                o[1] = (string)me[Dispatchs.DrillingWorkshopLocation];
                break;
        }
        //if(o == null) { return null; }
        return o;
    }
    /// <summary>
    /// 获取派工表中指定的人员
    /// </summary>
    /// <param name="processName">工序名称</param>
    /// <param name="currentWorkshop">当前车间</param>
    /// <param name="processSubTableRows">单前工序表任务子表行</param>
    /// <returns></returns>
    public string[] GetPerson(string processName, string currentWorkshop, H3.DataModel.BizObject[] processSubTableRows)
    {
        Dictionary<string, double> task = GetCompletedTask(processName, processSubTableRows);
        string[] person = GetStagePerson(processName, task);
        string[] manager = GetManager(processName, currentWorkshop);
        string[] productor = person.Length != 0 ? person : manager;
        //target["工人"] = productor;
        //target.Update(false);
        return productor;
    }

    private DataRow GetRow(string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "i_" + table + (where == "" ? "" : " where " + where);

        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        int Count = dt.Rows.Count;
        if (Count > 0)
        {
            return dt.Rows[0];
        }
        else
        {
            return null;
        }
    }
    private DataRowCollection GetRows(string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "i_" + table + (where == "" ? "" : " where " + where);
        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        return dt.Rows;
    }


    private string GetPersonField(string processName)
    {

        string tName = processName == "粗车" ? DispatchRoughSubTable.Name :
                       processName == "精车" ? DispatchFinishSubTable.Name :
                       processName == "钻孔" ? DispatchDrillSubTable.Name :
                       processName == "取样" ? DispatchSamplingSubTable.Name :
                       processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.Name :
                       processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.Name :
                       DispatchSamplingFourLatheSubTable.Name;
        return tName;
    }
    private string GetMountField(string processName)
    {
        string tMount = processName == "粗车" ? DispatchRoughSubTable.ProcessingQuantity :
                        processName == "精车" ? DispatchFinishSubTable.ProcessingQuantity :
                        processName == "钻孔" ? DispatchDrillSubTable.ProcessingQuantity :
                        processName == "取样" ? DispatchSamplingSubTable.ProcessingQuantity :
                        processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.ProcessingQuantity :
                        processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.ProcessingQuantity :
                        DispatchSamplingFourLatheSubTable.Name;
        return tMount;
    }

    private string GetPersonField2(string processName)
    {

        string tName = processName == "粗车" ? RoughSubTable.Processor :
                       processName == "精车" ? FinishSubTable.Processor :
                       processName == "钻孔" ? DrillSubTable.Processor :
                       processName == "取样" ? ProcessOfAppearanceSubtabulation.Processor :                       
                       ProcessOfAppearanceSubtabulation.Processor;
        return tName;
    }
    private string GetMountField2(string processName)
    {
        string tMount = processName == "粗车" ? RoughSubTable.ProcessingQuantity :
                        processName == "精车" ? FinishSubTable.ProcessingQuantity :
                        processName == "钻孔" ? DrillSubTable.ProcessingQuantity :
                        processName == "取样" ? ProcessOfAppearanceSubtabulation.ProcessingQuantity :
                        ProcessOfAppearanceSubtabulation.Processor;
        return tMount;
    }

}
