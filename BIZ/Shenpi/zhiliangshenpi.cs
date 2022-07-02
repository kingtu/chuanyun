using System;
public class D001419c587c429cb0e457fb094bd05641d07e4 : H3.SmartForm.SmartFormController
{    //本表单数据
    H3.DataModel.BizObject me;   
    public D001419c587c429cb0e457fb094bd05641d07e4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = Request.BizObject;      
    }
    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!Request.IsCreateMode)
        {
            if (Request.ActivityCode == "Activity3")
            {
                if (me[HowManyTimesToEnterTheNode] + string.Empty == string.Empty)
                {
                    //如果"关联机加质量处理"控件没有值，则取父流程，否则取父流程的父流程
                    if (me[AssociatedMachineQualityVolumeHandling] + string.Empty == string.Empty)
                    {
                        Component(); 
                    }
                    else
                    {
                        OtherComponent();
                    }
                }
            }           
            ObjWorkflow(); //把ObjectId返回给父流程
        }
        base.OnLoad(response);
    }
    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (me[HowManyTimesToEnterTheNode] + string.Empty == string.Empty)
        {
            me[HowManyTimesToEnterTheNode] = 1;
        }
        base.OnSubmit(actionName, postValue, response);
    }

    public void Component()
    {   //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, instance.SchemaCode, instance.BizObjectId, false);
        if (current[AssociatedWithOtherAbnormalWorkpieces] + string.Empty != string.Empty)//“F0000199”是“关联其他异常工件”控件的控件编码
        {
            string[] bizObjectIDArray = current[AssociatedWithOtherAbnormalWorkpieces] as string[];
            string abc = "";
            foreach (string bizObjectID in bizObjectIDArray)
            {
                //加载其他异常ID 的业务对象
                H3.DataModel.BizObject currentt = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, ScheduleManagement_TableCode, bizObjectID, false);
                abc += (currentt[ScheduleManagement_ID] + ",");
            }
            //赋值本表单“其他异常关联控件表单”控件值
            me[OtherExceptionRelatedControlForm] = abc;
        }
    }
    public void OtherComponent()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, instance.SchemaCode, instance.BizObjectId, false);
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instances = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(current.WorkflowInstanceId);
        //获取父流程的父流程的实例对象
        H3.Workflow.Instance.WorkflowInstance instancess = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(instances.ParentInstanceId);
        //获取父流程的父流程的业务对象
        H3.DataModel.BizObject currents = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, instancess.SchemaCode, instancess.BizObjectId, false);
        if (currents[AssociatedWithOtherAbnormalWorkpieces] + string.Empty != string.Empty)//“F0000199”是“关联其他异常工件”控件的控件编码
        {
            string[] bizObjectIDArray = currents[AssociatedWithOtherAbnormalWorkpieces] as string[];
            string abc = "";
            foreach (string bizObjectID in bizObjectIDArray)
            {
                //加载其他异常ID 的业务对象
                H3.DataModel.BizObject currentt = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, ScheduleManagement_TableCode, bizObjectID, false);
                abc += (currentt[ScheduleManagement_ID] + string.Empty + ",");
            }
            //赋值本表单“其他异常关联控件表单”控件值
            me[OtherExceptionRelatedControlForm] = abc;
        }
    }
    //把ObjectId返回给父流程
    protected void ObjWorkflow()
    {
        //获取父流程实例对象
        H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
        //获取父流程业务对象
        H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, instance.SchemaCode, instance.BizObjectId, false);
        string WorkflowDisplayName = instance.WorkflowDisplayName;
        switch (WorkflowDisplayName)
        {
            case "锯切":
                current[SawCut_QualityApprovalList] = me[ObjectId] + string.Empty;
                break;
            case "锻压":
                current[Forge_QualityApprovalList] = me[ObjectId] + string.Empty;
                break;
            case "辗环":
                current[RollingRing_QualityApprovalList] = me[ObjectId] + string.Empty;
                break;
            case "热处理":
                current[HeatTreatment_QualityApprovalList] = me[ObjectId] + string.Empty;
                break;
            case "机加质量问题保证":
                current[MechanicalQualityAssurance_QualityApprovalList] = me[ObjectId] + string.Empty;
                break;
        }
        current.Update();
    }

    //质量审批单
    string HowManyTimesToEnterTheNode = "F0000024";                //第几次进入节点
    string AssociatedMachineQualityVolumeHandling = "F0000015";    //关联机加质量处理
    string OtherExceptionRelatedControlForm = "F0000006";         //被关联的其他异常工件ID
    string ObjectId = "ObjectId";              //ObjectId
    //工序表的“可以被关联的其他异常工件”
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";
    //生产进度管理
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";
    string ScheduleManagement_ID = "F0000001";     //任务状态
    //工序表及质量保证
    string SawCut_QualityApprovalList = "F0000202";         //锯切_质量审批单
    string Forge_QualityApprovalList = "F0000201";           //锻压_质量审批单
    string RollingRing_QualityApprovalList = "F0000200";      //辗环_质量审批单
    string HeatTreatment_QualityApprovalList = "F0000200";    //热处理_质量审批单
    string MechanicalQualityAssurance_QualityApprovalList = "F0000022";   //质量保证_质量审批单
}
