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

        public string BizObjectId { get;  set; }
        public bool IsUnfinished { get;  set; }
        public Activtie[] RunningActivties { get;  set; }
        public string InstanceId { get;  set; }
        public bool IsFinished { get;  set; }
        public object FinishTime { get;  set; }

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