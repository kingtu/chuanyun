﻿using H3.Workflow;
using H3.Workflow.Messages;
using System;

namespace H3
{
    public class WorkflowInstanceManager
    {
        internal Instance.WorkflowInstance GetWorkflowInstance(string workflowInstanceId)
        {
            throw new NotImplementedException();
        }

        internal void SendMessage(ActivateActivityMessage activiteMessage)
        {
            throw new NotImplementedException();
        }
        internal void SendMessage(H3.Workflow.Messages.CancelActivityMessage cancelActivityMessage)
        {
            throw new NotImplementedException();
        }
    }
}