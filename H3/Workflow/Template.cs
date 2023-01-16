using System;

namespace H3.Workflow.Template
{     
        public class WorkflowTemplate
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