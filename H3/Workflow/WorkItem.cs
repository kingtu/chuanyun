﻿namespace H3.Workflow
{
    public class WorkItem
    {
        public static ActionEventType ActionEventType { get; set; }
        public static AccessMethod AccessMethod { get; set; }
        public static WorkItemState WorkItemState { get; set; }

        public string Participant { get; set; }
        public string ActivityDisplayName { get; internal set; }
    }


}


public enum WorkItemState
{
    Unfinished
        
}
