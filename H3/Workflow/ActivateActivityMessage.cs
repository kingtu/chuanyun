using System;
using System.Collections.Generic;
using System.Text;

namespace H3.Workflow.Messages
{
    class ActivateActivityMessage
    {
        private string workflowInstanceId;
        private string nextActivityCode;
        private object unspecifiedId;
        private string[] v1;
        private object p;
        private bool v2;
        private object adjust;

        public ActivateActivityMessage(string workflowInstanceId, string nextActivityCode, object unspecifiedId, string[] v1, object p, bool v2, object adjust)
        {
            this.workflowInstanceId = workflowInstanceId;
            this.nextActivityCode = nextActivityCode;
            this.unspecifiedId = unspecifiedId;
            this.v1 = v1;
            this.p = p;
            this.v2 = v2;
            this.adjust = adjust;
        }
    }
}
