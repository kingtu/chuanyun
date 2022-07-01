
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D001419Skk7vgg7h6fxn1n4cb9imiydu4 : H3.SmartForm.SmartFormController
{
    //本表单数据
    H3.DataModel.BizObject me;
    //当前节点
    string activityCode;
    public D001419Skk7vgg7h6fxn1n4cb9imiydu4(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        activityCode = this.Request.ActivityCode;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {
            if (activityCode == "Activity3")
            {
                if (me[HowManyTimesToEnterTheNode] + string.Empty == string.Empty)
                {
                    H3.Workflow.Instance.WorkflowInstance instance = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(this.Request.WorkflowInstance.ParentInstanceId);
                    H3.DataModel.BizObject current = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, instance.SchemaCode, instance.BizObjectId, false);
                    if (current[AssociatedWithOtherAbnormalWorkpieces] + string.Empty != string.Empty)
                    {
                        String[] bizObjectIDArray = current[AssociatedWithOtherAbnormalWorkpieces] as string[];
                        string abc = "";
                        foreach (string bizObjectID in bizObjectIDArray)
                        {
                            //加载其他异常ID 的业务对象
                            H3.DataModel.BizObject currentt = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, ScheduleManagement_TableCode, bizObjectID, false);
                            abc += (currentt[ScheduleManagement_ID] + string.Empty + ",");
                        }
                        me[OtherExceptionRelatedControlForm] = abc;
                    }
                }
            }
        }
        ObjWorkflow();
        base.OnLoad(response);
    }

    protected override void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
    {
        if (me[HowManyTimesToEnterTheNode] + string.Empty == string.Empty)
        {
            me[HowManyTimesToEnterTheNode] = 1;
        }
        if (me[AbnormalRepresentative] + string.Empty != string.Empty)
        {
            me[MarketDemandOpinion] = "再生库";
        }
        me.Update();
        base.OnSubmit(actionName, postValue, response);
    }

    //最后一个节点提交后把把ObjectId返回给父流程
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
                current[SawCut_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "锻压":
                current[Forge_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "辗环":
                current[RollingRing_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "热处理":
                current[HeatTreatment_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "毛坯":
                current[RoughCast_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "粗车":
                current[RoughCutting_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "精车":
                current[Finishing_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "钻孔":
                current[Drilling_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
        }
        current.Update();
    }


    //需求审批单
    string HowManyTimesToEnterTheNode = "F0000024";             //第几次进入节点
    string OtherExceptionRelatedControlForm = "F0000006";      //被关联的其他异常工件ID
    string ObjectId = "ObjectId";      //ObjectId
    string MarketDemandOpinion = "F0000034";                    //市场需求意见
    string AbnormalRepresentative = "F0000015";                  //异常代表
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199"; //工序表的“可以被关联的其他异常工件”
    //实时生产动态
    string ScheduleManagement_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";            //可以被关联的其他异常工件
    string ScheduleManagement_ID = "F0000001";          //任务状态
    //工序表及质量保证
    string SawCut_RequestExceptionApprovalForm = "F0000203";                  //锯切_需求审批单
    string Forge_RequestExceptionApprovalForm = "F0000202";                   //锻压_需求审批单
    string RollingRing_RequestExceptionApprovalForm = "F0000201";             //辗环_需求审批单
    string HeatTreatment_RequestExceptionApprovalForm = "F0000201";     //热处理_需求审批单
    string RoughCast_RequestExceptionApprovalForm = "F0000226";                //毛坯_需求审批单
    string RoughCutting_RequestExceptionApprovalForm = "F0000263";             //粗车_需求审批单
    string Finishing_RequestExceptionApprovalForm = "F0000257";          //精车_需求审批单
    string Drilling_RequestExceptionApprovalForm = "F0000235";          //钻孔_需求审批单
}