﻿
using System;
using System.Collections.Generic;
using System.Text;
using H3;

public class D001419Sk5o69536e6w6pev17ghk3u7y6 : H3.SmartForm.SmartFormController
{
    //本表单数据
    H3.DataModel.BizObject me;
    //当前节点
    string activityCode;
    public D001419Sk5o69536e6w6pev17ghk3u7y6(H3.SmartForm.SmartFormRequest request) : base(request)
    {
        me = this.Request.BizObject;
        activityCode = this.Request.ActivityCode;
    }

    protected override void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
    {
        if (!this.Request.IsCreateMode)
        {
            //工序名称
            if (me[ProcessName] + string.Empty != "取样子流程")
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
                                H3.DataModel.BizObject currentt = H3.DataModel.BizObject.Load(H3.Organization.User.SystemUserId, this.Engine, RealTimeDynamicProduction_TableCode, bizObjectID, false);
                                abc += (currentt[RealTimeDynamicProduction_ID] + string.Empty + ",");
                            }
                            me[OtherExceptionRelatedControlForm] = abc;
                        }
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
                current[Roughing_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "精车":
                current[Finishing_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
            case "钻孔":
                current[Drill_RequestExceptionApprovalForm] = me[ObjectId] + string.Empty;
                break;
        }
        current.Update();
    }

    //其它审批单
    string HowManyTimesToEnterTheNode = "F0000024";                                                                            //第几次进入节点
    string OtherExceptionRelatedControlForm = "F0000006";                                                                      //被关联的其他异常工件ID
    string ObjectId = "ObjectId";                                                                                              //ObjectId
                                                                                                                               // 工序名称
    string ProcessName = "F0000004";
    //工序表的“可以被关联的其他异常工件”
    string AssociatedWithOtherAbnormalWorkpieces = "F0000199";

    //实时生产动态
    string RealTimeDynamicProduction_TableCode = "D0014197b0d6db6d8d44c0a9f472411b6e754bd";                                    //可以被关联的其他异常工件
    string RealTimeDynamicProduction_ID = "F0000001";                                                                          //任务状态

    //工序表及质量保证
    string SawCut_RequestExceptionApprovalForm = "F0000205";                                                                   //锯切_其它审批单
    string Forge_RequestExceptionApprovalForm = "F0000204";                                                                    //锻压_其它审批单
    string RollingRing_RequestExceptionApprovalForm = "F0000203";                                                              //辗环_其它审批单
    string HeatTreatment_RequestExceptionApprovalForm = "F0000203";                                                            //热处理_其它审批单
    string RoughCast_RequestExceptionApprovalForm = "F0000228";                                                                //毛坯_其它审批单
    string Roughing_RequestExceptionApprovalForm = "F0000265";                                                                 //粗车_其它审批单
    string Finishing_RequestExceptionApprovalForm = "F0000259";                                                                //精车_其它审批单
    string Drill_RequestExceptionApprovalForm = "F0000237";                                                                    //钻孔_其它审批单
}

