using System;
using System.Collections.Generic;
using System.Text;

namespace H3.Workflow.Messages
{
    public class CancelActivityMessage
    {
        private string workflowInstanceId;
        private string currentActivityCode;
        private bool v;

        public CancelActivityMessage(string workflowInstanceId, string currentActivityCode, bool v)
        {
            this.workflowInstanceId = workflowInstanceId;
            this.currentActivityCode = currentActivityCode;
            this.v = v;
        }
    }
}
