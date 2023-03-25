using H3.Workflow;
using H3.Workflow.Instance;
using H3.Workflow.Messages;
using System;

namespace H3
{
    public class WorkflowInstanceManager
    {
        public WorkflowInstance GetWorkflowInstance(string workflowInstanceId)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(ActivateActivityMessage activiteMessage)
        {
            throw new NotImplementedException();
        }
        public void SendMessage(H3.Workflow.Messages.CancelActivityMessage cancelActivityMessage)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(AdjustParticipantMessage adjustMessage)
        {
            throw new NotImplementedException();
        }
    }
}