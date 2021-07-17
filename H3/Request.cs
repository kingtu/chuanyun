using System;
using System.Collections.Generic;
using System.Text;
using H3.DataModel;
using H3.Workflow;

namespace H3
{
    public class Request
    {
        public BizObject BizObject;

        public string BizObjectId { get; set; }
        public string ActivityCode { get; set; }
        public ActivityTemplate ActivityTemplate { get; set; }
        public UserContext UserContext { get; set; }
        public bool IsCreateMode { get; set; }
        public string ParticipantId { get; set; }
        public string SchemaCode { get; set; }
        public H3.Workflow.Workflow WorkflowInstance { get; set; }
        public WorkItem WorkItem { get; set; }
        public H3.IEngine Engine { get; internal set; }
    }
}
