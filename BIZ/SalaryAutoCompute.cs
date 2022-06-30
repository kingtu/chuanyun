using System;
public class D0014197f6ba68a418040adac5265aa100e36c7 : H3.SmartForm.SmartFormController
{
    H3.DataModel.BizObject me;
    public D0014197f6ba68a418040adac5265aa100e36c7(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
    }
    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }
    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }
    //流程审批结束事件（先执行业务规则，在执行该方法）。
    protected override void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
    {
        try
        {
            if (newState == H3.Workflow.Instance.WorkflowInstanceState.Finished)
            {
                //执行业务代码逻辑
                string id = me[this.ID] + string.Empty;
                string objectID = me[this.ProcessObjectID] + string.Empty; //粗车表objectID                
                string process = me[this.ProcessName] + string.Empty;//工序名称                
                string processPlanObjectid = me[this.ProcessPlanObjectid] + string.Empty;//工序计划表objectID
                //更新机加记录表
                UpdateMacineRecord umd = new UpdateMacineRecord();
                var slr = new Salary(this.Engine, id); //工资计算
                string macineRecordID = me[this.MacineRecordObjectid] + string.Empty;
                if (string.IsNullOrEmpty(macineRecordID))
                {   //更新机加记录表
                    umd.UpdateMacineRecordTable(this.Engine, process, objectID, processPlanObjectid, me);
                    //机加记录添加工时
                    umd.AddTechHoursToRecord(this.Engine, process, objectID, processPlanObjectid);
                }
                //获取机加任务记录中的objectID
                macineRecordID = me[this.MacineRecordObjectid] + string.Empty;
                slr.Save(process, macineRecordID);  //工资计算
            }
        }
        catch (Exception ex)
        {
            Tools.Log.ErrorLog(this.Engine, me, ex, "工资计算自动化", this.Request.UserContext.User.FullName);
        }
        base.OnWorkflowInstanceStateChanged(oldState, newState);
    }
    string ID = "ID"; //ID   
    string ProcessName = "ProcessName"; //工序名称   
    string ProcessObjectID = "F0000007"; //工序表objectId   
    string ProcessPlanObjectid = "F0000008"; //工序计划表objectid   
    string MacineRecordObjectid = "F0000013"; //机加任务记录objectid      
}