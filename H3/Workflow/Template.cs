using System;

namespace H3.Workflow.Template
{
    public class WorkflowTemplate
    {
        public object WorkflowVersion { get; set; }

        public Activity GetActivityByCode(string v)
        {
            throw new NotImplementedException();
        }
    }

    public class Activity
    {
        public string DisplayName { get; set; }
    }

}