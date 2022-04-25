using H3.Workflow.Instance;
using System;
using System.Collections.Generic;
using System.Text;

namespace H3.Workflow
{
    public class Workflow
    {
        public Token[] Tokens { get;  set; }
        public bool IsUnfinished { get; internal set; }
        public string ParentInstanceId { get; internal set; }
        public string InstanceId { get; internal set; }

        public Token GetRunningToken(string activityCode)
        {
            throw new NotImplementedException();
        }
    }
}
