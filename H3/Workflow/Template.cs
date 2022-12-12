using System;

namespace H3.Workflow
{
    internal class Template
    {
        internal class WorkflowTemplate
        {
            public object WorkflowVersion { get; internal set; }

            internal Activity GetActivityByCode(string v)
            {
                throw new NotImplementedException();
            }
        }

        internal class Activity
        {
            public string DisplayName { get; internal set; }
        }
    }
}