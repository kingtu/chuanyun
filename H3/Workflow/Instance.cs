﻿//using Chuanyun;
using H3.Data;
using System;

namespace H3.Workflow.Instance
{

    public interface IToken
    {
        string Activity { get; set; }
        string[] Participants { get; set; }
        BoolValue Approval { get; }
        string TokenId { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime FinishedTime { get; set; }
        TimeSpan UsedTime { get; set; }
        TokenState State { get; set; }
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
        public string SchemaCode { get;  set; }
        public string WorkflowDisplayName { get;  set; }
        public string ParentInstanceId { get;  set; }
        public string ObjectId { get;  set; }

        public Token GetRunningToken(object activityCode)
        {
            throw new NotImplementedException();
        }

        public IToken GetLastToken()
        {
            throw new NotImplementedException();
        }
    }



}