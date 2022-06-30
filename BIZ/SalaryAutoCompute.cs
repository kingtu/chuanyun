
using System;
using System.Collections.Generic;
using System.Text;
using H3;
//修改建议：
//1.删除不必要的空行
//2.将短行的注释放变量定义之后，但是特别长的变量除外。
public class D0014197f6ba68a418040adac5265aa100e36c7 : H3.SmartForm.SmartFormController
{
    string activityCode = "工资计算自动化";
    string userName = ""; //当前用户
    H3.DataModel.BizObject me;
    public D0014197f6ba68a418040adac5265aa100e36c7(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        userName = this.Request.UserContext.User.FullName;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        base.OnSubmit(actionName, postValue, response);
    }

    protected override void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
    {
        try
        {
            //流程审批结束事件（先执行业务规则，在执行该方法）。
            if (newState == H3.Workflow.Instance.WorkflowInstanceState.Finished)
            {
                //执行业务代码逻辑
                string id = me[this.ID] + string.Empty;
                //粗车表objectID
                string objectID = me[this.ProcessObjectID] + string.Empty;
                //工序名称
                string process = me[this.ProcessName] + string.Empty;

                //工序计划表objectID
                string processPlanObjectid = me[this.ProcessPlanObjectid] + string.Empty;

                //更新机加记录表
                UpdateMacineRecord umd = new UpdateMacineRecord();
                //工资计算
                var slr = new Salary(this.Engine, id);

                string macineRecordID = me[this.MacineRecordObjectid] + string.Empty;
                if (string.IsNullOrEmpty(macineRecordID))
                {
                    //todo   

                    //更新机加记录表
                    umd.UpdateMacineRecordTable(this.Engine, process, objectID, processPlanObjectid, me);

                    //机加记录添加工时
                    umd.AddTechHoursToRecord(this.Engine, process, objectID, processPlanObjectid);

                }

                //todo
                //获取机加任务记录中的objectID
                macineRecordID = me[this.MacineRecordObjectid] + string.Empty;

                //工资计算
                slr.Save(process, macineRecordID);
            }
        }
        catch (Exception ex)
        {
            Tools.Log.ErrorLog(this.Engine, me, ex, activityCode, userName);
        }
        base.OnWorkflowInstanceStateChanged(oldState, newState);
    }

    //todo
   
    string ID = "ID"; //ID
    //工序名称
    string ProcessName = "ProcessName";
    //工序表objectId
    string ProcessObjectID = "F0000007";
    //工序计划表objectid
    string ProcessPlanObjectid = "F0000008";
    //机加任务记录objectid
    string MacineRecordObjectid = "F0000013";
    //当前工步
    string CurrentWorkStep = "F0000009";
    //是/否  开关
    string IsComputed = "IsComputed";
}

