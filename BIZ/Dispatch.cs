using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using H3;

public class Dispatch
{
    DataRow thisIdDispatch;
    string ID;
    H3.IEngine Engine;
    public string[] Manager = null;
    public PlanData[] PlanInfos = null;


    /*
   *--Author:zlm
   * 取得派工信息并分配权限
   * @param: actionName 被点击按钮名称
   * @param: Engine 氚云
   * @paramp： ID  产品ID号
   * dataTeime:2021-11-11 
   */
    public Dispatch(H3.IEngine Engine, string ID)
    {
        this.Engine = Engine;
        this.ID = ID;
        inti();
    }
    private void inti()
    {
        string where = Dispatchs.ID + "='" + ID + "'";
        thisIdDispatch = GetRow(Dispatchs.TableCode, where);
    }
    /*
  *--Author:zlm
  * 根据工序名称获取派工顺序是否受限制
  * @param： processName 工序名称
  * return：  ture 不限制 false 限制
  * dataTeime: 2021-11-11 
  */

    private bool GetOrder(string processName)
    {
        if (thisIdDispatch == null) { return false; }
        object o = null;
        switch (processName)
        {
            case "锯切":
                o = thisIdDispatch[Dispatchs.SawingDoesNotLimitTheOrderOfDispatch];
                break;
            case "粗车":
                o = thisIdDispatch[Dispatchs.RoughTurningUnlimitedDispatchSequence];
                break;
            case "精车":
                o = thisIdDispatch[Dispatchs.FinishTurningUnlimitedDispatchSequence];
                break;
            case "钻孔":
                o = thisIdDispatch[Dispatchs.DrillingUnlimitedDispatchSequence];
                break;
            case "取样":
                o = thisIdDispatch[Dispatchs.SamplingUnlimitedDispatchSequence];
                break;
        }
        if (o == null || o == DBNull.Value) { return false; }
        return Convert.ToBoolean(o);
    }

    /*
     *--Author:zlm
     * 向上获取负责人，班组长->科长->生产计划员
     * @param: processName 工序名称
     * return: 
     * dataTeime:2021-11-10  
     */
    public string[] GetManagers(string processName)
    {
        string[] Result = null;
        //根据工序名称取得ABCD计划表内班组字段编码
        string TeamFiled = GetABCDTeamField(processName);
        H3.Data.Filter.Filter filter = new H3.Data.Filter.Filter();
        Tools.Filter.And(filter, ABCDProcessPlan.ID, H3.Data.ComparisonOperatorType.Equal, ID);
        H3.DataModel.BizObject abcdBizObject = Tools.BizOperation.GetFirst(Engine, ABCDProcessPlan.TableCode, filter);


        if (abcdBizObject != null)
        {

            string unitID = abcdBizObject[TeamFiled] + string.Empty;

            H3.Organization.Unit Team = null;

            if (!string.IsNullOrEmpty(unitID))
            {
                Team = this.Engine.Organization.GetUnit(unitID) as H3.Organization.Unit;
                if (Team != null) { Result = new string[] { Team.ManagerId }; }

            }

            else
            {
                unitID = abcdBizObject[ABCDProcessPlan.ColdProcessingDepartment] + string.Empty;
                if (!string.IsNullOrEmpty(unitID))
                {
                    Team = this.Engine.Organization.GetUnit(unitID);
                    if (Team != null) { Result = new string[] { Team.ManagerId }; }
                }
            }
            if (Result == null)
            {
                Result = Tools.Role.GetUsers(Engine, "生产计划员");

            }

        }

        return Result;
    }

    /*
       *--Author:zlm
       * 根据已经完成的工作量，选取阶段性加工者
       * @param: processName 工序名称
       * @param : completedTask 所有加工者以完成加工量记录 
       * return: 
       * dataTeime:2021-11-10  
       */
    private string[] GetStagePerson(string processName, Dictionary<string, double> completedTask)
    {   // 取得派工模式，有序还是无序
        bool order = GetOrder(processName);
        List<string> result = new List<string>();
        List<PlanData> Plans = new List<PlanData>();


        //取得派工表里对应子表的编码
        string tasks = GetPlanSubtableCode(processName);
        //派工表内对应子表的"姓名"编码
        string planNameCode = GetPlanPersonField(processName);
        //派工表内对应子表"加工量"编码
        string planMountCode = GetPlanMountField(processName);
        string planDeviceName = GetPlanDevice(processName);
        string PlanWorkTime = GetPlanMonHour(processName);

        string where = "ParentObjectId" + "='" + thisIdDispatch[Dispatchs.Objectid] + "' order by parentIndex";
        //取得派工表内对应的子表内的记录
        DataRowCollection personsTask = GetRows((string)tasks, where);

        if (personsTask == null) { return result.ToArray(); }

        //用计划内的每一个人于它自身以完成的加工量进行比对，选出合适的加工者
        foreach (DataRow item in personsTask)
        {

            double totalLoad = 0;
            string name = (string)item[planNameCode];//派工表内的姓名
            string planDevicename = (string)item[planDeviceName];//派工表内的设备名称
            var debugPlanmanhour = item[PlanWorkTime];
            double planManhour = Convert.ToDouble(item[PlanWorkTime]);//派工表内的设备工时；           


            double planload = (double)item[planMountCode];//派工表内的加工量
            //已经完成的任务
            if (completedTask != null && completedTask.ContainsKey(name))
            {   //已经完成的加工量
                totalLoad = completedTask[name];
                if (totalLoad > planload)
                {
                    throw new Exception("您填写的报工量大于给您的安排的加工量！");
                }
            }

            if (order)//无序
            {
                if (totalLoad < planload)
                {
                    result.Add(name);
                    Plans.Add(new PlanData(name, planDeviceName, planManhour));


                }

            }
            else//有序
            {   //以加工量小于计划加工量
                if (totalLoad < planload && result.Count == 0)
                {
                    result.Add(name);
                    Plans.Add(new PlanData(name, planDeviceName, planManhour));
                }
            }
        }

        PlanInfos = Plans.ToArray();

        return result.ToArray();
    }

    /*
      *--Author:
      * 根据给定条件返回指定表的数据
      * @param: table  表单Code
      * @param : where 过滤条件 
      * return: DataRow 只返回1行
      * dataTeime:2021-11-10  
      */
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
    /*
     *--Author:
     * 根据给定条件返回指定表的数据
     * @param: table  表单Code
     * @param : where 过滤条件 
     * return: DataRow 返回符合条件的所有记录
     * dataTeime:2021-11-10  
     */
    private DataRowCollection GetRows(string table, string where, string selector = "*")
    {
        string sql = "select " + selector + " from " + "i_" + table + (where == "" ? "" : " where " + where);
        DataTable dt = this.Engine.Query.QueryTable(sql, null);
        return dt.Rows;
    }

    /*
     *--Author:zlm
     * 根据工序名称对已经完成的任务的加工量进行分类汇总
     * @param: processName  工序名称
     * @param : processSubTableRows  工序表内的子表
     * return: dictionary <加工者，加工量>
     * dataTeime:2021-11-10  
     */
    private Dictionary<string, double> GetCompletedTask(string processName, H3.DataModel.BizObject[] processSubTableRows)
    {
        Dictionary<string, double> result = new Dictionary<string, double>();
        if (processSubTableRows != null)
        {
            foreach (H3.DataModel.BizObject item in processSubTableRows)
            {
                string processNameCode = GetProcessPersonField(processName);

                string name = item[processNameCode] + string.Empty;
                string loadCode = GetProcessMountField(processName);
                var loadValue = item[loadCode];
                //工序子表内一行的加工量
                double load = Convert.ToDouble(loadValue);
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


    /*
   *--Author:zlm
   * 获取派工表中指定的车间
   * @param: processName  工序名称 
   * return: 
   * dataTeime:2021-11-10  
   */
    public string[] GetPlanWorkShop(string processName)
    {

        string[] result = new string[] { null, null };
        if (thisIdDispatch == null) { return result; }
        switch (processName)
        {
            case "取样":
                result[0] = (string)thisIdDispatch[Dispatchs.SamplingWorkshopName].ToString();
                result[1] = (string)thisIdDispatch[Dispatchs.SamplingWorkshopLocation].ToString();
                break;
            case "粗车":
                result[0] = (string)thisIdDispatch[Dispatchs.RoughTurningWorkshopName].ToString();
                result[1] = (string)thisIdDispatch[Dispatchs.RoughTurningWorkshopLocation].ToString();
                break;
            case "精车":
                result[0] = (string)thisIdDispatch[Dispatchs.FinishTurningWorkshopName].ToString();
                result[1] = (string)thisIdDispatch[Dispatchs.FinishTurningWorkshopLocation].ToString();
                break;
            case "钻孔":
                result[0] = (string)thisIdDispatch[Dispatchs.DrillingWorkshopName].ToString();
                result[1] = (string)thisIdDispatch[Dispatchs.DrillingWorkshopLocation].ToString();
                break;
        }
        return result;
    }

    /*
     *--Author:zlm
     *  取得加工计划人员
     *  @param: processName  工序名称 
     *  @param: processSubTableRows  工序表内加工记录子表
     *  return: 
     *  dataTeime:2021-11-10  
     */
    public string[] GetPerson(string processName, H3.DataModel.BizObject[] processSubTableRows)
    {

        // 获取派工计划中所有加工者的以完成加工量
        Dictionary<string, double> task = GetCompletedTask(processName, processSubTableRows);
        // 根据以完成加工量，取合适的阶段加工者
        string[] person = GetStagePerson(processName, task);
        // 如果没工件加工计划，取得工段负责人
        Manager = GetManagers(processName);
        string[] productor = person.Length != 0 ? person : Manager;
        return productor;
    }


    //返回派工子表内的第一行数据的姓名
    public string GetFirstPerson(string processName)
    {
        List<string> g = new List<string>();
        string pTask = processName == "粗车" ? Dispatchs.RoughTurning :
            processName == "精车" ? Dispatchs.FinishTurning :
                processName == "钻孔" ? Dispatchs.Drilling : Dispatchs.Sampling;
        //"姓名"
        string tName = GetPlanPersonField(processName);
        //"加工量"
        string tMount = GetPlanMountField(processName);
        string w = DispatchRoughSubTable.Parentobjectid + "='" + thisIdDispatch[Dispatchs.Objectid] + "' order by parentIndex";

        DataRow personsTask = GetRow((string)pTask, w);

        string name = null;
        if (personsTask != null)
        {
            name = (string)personsTask[tName];
        }
        else
        {
            //string[] manager = GetManager(processName, currentWorkshop);
            //if(manager != null && manager.Length > 0) { name = manager[0]; }
        }
        return name;
    }

    /*
   *--Author:zlm
   *  取ABCD计划表内的班组字段编码
   *  @param: processName  工序名称    
   *  return: 
   *  dataTeime:2021-11-10  
   */
    private string GetABCDTeamField(string processName)
    {
        string result = processName == "粗车" ? ABCDProcessPlan.RoughTeam :
            processName == "取样" ? ABCDProcessPlan.RoughTeam :
                processName == "锯切" ? ABCDProcessPlan.SawcutTeam :
                    processName == "精车" ? ABCDProcessPlan.FinishTeam :
                        processName == "钻孔" ? ABCDProcessPlan.DrillTeam : null;

        return result;
    }


    /*
  *--Author:zlm
  * 取工序表内对应子表内的姓名字段
  *  @param: processName  工序名称    
  *  return: 
  *  dataTeime:2021-11-10  
  */
    private string GetProcessPersonField(string processName)
    {
        string tName = processName == "粗车" ? RoughSubTable.Processor :
            processName == "锯切" ? SawCutSubTable.Processor :
                processName == "精车" ? FinishSubTable.Processor :
                    processName == "钻孔" ? DrillSubTable.Processor :
                        processName == "取样" ? ProcessOfAppearanceSubtabulation.Processor :
                            processName == "粗车四面光" ? RoughFourLathe.Worker :
                                processName == "取样四面光" ? SamplingFourLathe.Worker :
                                    SamplingFourLathe.Worker;
        return tName;
    }


    /*
  *--Author:zlm
  * 取工序表内对应子表内的加工量字段
  *  @param: processName  工序名称    
  *  return: 
  *  dataTeime:2021-11-10  
  */
    private string GetProcessMountField(string processName)
    {
        string tMount = processName == "粗车" ? RoughSubTable.ProcessingQuantity :
            processName == "锯切" ? SawCutSubTable.ProcessingQuantity :
                processName == "精车" ? FinishSubTable.ProcessingQuantity :
                    processName == "钻孔" ? DrillSubTable.ProcessingQuantity :
                        processName == "取样" ? ProcessOfAppearanceSubtabulation.ProcessingQuantity :
                            processName == "粗车四面光" ? RoughFourLathe.WorkLoad :
                                processName == "取样四面光" ? SamplingFourLathe.WorkLoad :
                                    SamplingFourLathe.WorkLoad;
        return tMount;
    }

    /*
  *--Author:zlm
  * 取派工表内对应子表内的姓名字段
  *  @param: processName  工序名称    
  *  return: 
  *  dataTeime:2021-11-10  
  */
    private string GetPlanPersonField(string processName)
    {

        string tName = processName == "粗车" ? DispatchRoughSubTable.Name :
            processName == "锯切" ? DispatchSawCutSubTable.Name :
                processName == "精车" ? DispatchFinishSubTable.Name :
                    processName == "钻孔" ? DispatchDrillSubTable.Name :
                        processName == "取样" ? DispatchSamplingSubTable.Name :
                            processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.Name :
                                processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.Name :
                                    DispatchSamplingFourLatheSubTable.Name;
        return tName;
    }

    /*
 *--Author:zlm
 * 取派工表内对应子表内的加工量字段
 *  @param: processName  工序名称    
 *  return: 
 *  dataTeime:2021-11-10  
 */
    private string GetPlanMountField(string processName)
    {
        string tMount = processName == "粗车" ? DispatchRoughSubTable.ProcessingQuantity :
            processName == "锯切" ? DispatchSawCutSubTable.ProcessingQuantity :
                processName == "精车" ? DispatchFinishSubTable.ProcessingQuantity :
                    processName == "钻孔" ? DispatchDrillSubTable.ProcessingQuantity :
                        processName == "取样" ? DispatchSamplingSubTable.ProcessingQuantity :
                            processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.ProcessingQuantity :
                                processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.ProcessingQuantity :
                                    DispatchSamplingFourLatheSubTable.Name;
        return tMount;
    }
    /*
*--Author:zlm
* 取派工表内对应子表的编码
*  @param: processName  工序名称    
*  return: 
*  dataTeime:2021-11-10  
*/
    private string GetPlanSubtableCode(string processName)
    {
        string result = processName == "粗车" ? Dispatchs.RoughTurning :
            processName == "锯切" ? Dispatchs.Sawing :
                processName == "精车" ? Dispatchs.FinishTurning :
                    processName == "钻孔" ? Dispatchs.Drilling : Dispatchs.Sampling;
        return result;
    }

    /*
 *--Author:zlm
 * 取派工表内对应子表内的设备字段的编码
 *  @param: processName  工序名称    
 *  return: 
 *  dataTeime:2021-11-10  
 */
    private string GetPlanDevice(string processName)
    {
        string result = processName == "粗车" ? DispatchRoughSubTable.EquipmentName :
            processName == "锯切" ? DispatchSawCutSubTable.EquipmentName :
                processName == "精车" ? DispatchFinishSubTable.EquipmentName :
                    processName == "钻孔" ? DispatchDrillSubTable.EquipmentName :
                        processName == "取样" ? DispatchSamplingSubTable.EquipmentName :
                            processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.EquipmentName :
                                processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.EquipmentName :
                                    DispatchSamplingFourLatheSubTable.Name;
        return result;
    }

    /*
 *--Author:zlm
 * 取派工表内对应子表内的设备字段的编码
 *  @param: processName  工序名称    
 *  return: 
 *  dataTeime:2021-11-10  
 */
    private string GetPlanDeviceName(string processName)
    {
        string result = processName == "粗车" ? DispatchRoughSubTable.EquipmentName :
            processName == "锯切" ? DispatchSawCutSubTable.EquipmentName :
                processName == "精车" ? DispatchFinishSubTable.EquipmentName :
                    processName == "钻孔" ? DispatchDrillSubTable.EquipmentName :
                        processName == "取样" ? DispatchSamplingSubTable.EquipmentName :
                            processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.EquipmentName :
                                processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.EquipmentName :
                                    DispatchSamplingFourLatheSubTable.Name;
        return result;
    }
    /*
 *--Author:zlm
 * 取派工表内对应子表内的工时字段的编码
 *  @param: processName  工序名称    
 *  return: 
 *  dataTeime:2021-11-10  
 */
    private string GetPlanMonHour(string processName)
    {
        string result = processName == "粗车" ? DispatchRoughSubTable.ManHour :
            processName == "锯切" ? DispatchSawCutSubTable.ManHour :
                processName == "精车" ? DispatchFinishSubTable.ManHour :
                    processName == "钻孔" ? DispatchDrillSubTable.ManHour :
                        processName == "取样" ? DispatchSamplingSubTable.ManHour :
                            processName == "粗车四面光" ? DispatchRoughFourLatheSubTable.ManHour :
                                processName == "取样四面光" ? DispatchSamplingFourLatheSubTable.ManHour :
                                    DispatchSamplingFourLatheSubTable.Name;
        return result;
    }

    public struct PlanData
    {
        public string DeviceName;
        public string Name;
        public Double MonHour;

        public PlanData(string deviceName, string name, Double monHour)
        {
            DeviceName = deviceName;
            Name = name;
            MonHour = monHour;
        }

        public double WorkLoad { get; internal set; }
    }
}
