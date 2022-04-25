using H3.Data;
using System;

namespace H3.Workflow.Instance
{

    internal interface IToken
    {
        string Activity { get; set; }
        string[] Participants { get; set; }
        BoolValue Approval { get; }
        string TokenId { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime FinishedTime { get; set; }
        TimeSpan UsedTime { get; set; }
    }

    public class WorkflowInstance
    {
        public Token[] Tokens;

        public string BizObjectId { get; internal set; }
        public bool IsUnfinished { get; internal set; }

        internal Token GetRunningToken(object activityCode)
        {
            throw new NotImplementedException();
        }

        internal IToken GetLastToken()
        {
            throw new NotImplementedException();
        }
    }



}