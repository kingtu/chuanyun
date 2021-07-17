namespace H3.Workflow.Messages
{
    internal class AdjustParticipantMessage
    {
        public AdjustParticipantMessage(string workflowInstanceId, string activityCode, string[] participant)
        {
            WorkflowInstanceId = workflowInstanceId;
            ActivityCode = activityCode;
            Participant = participant;
        }

        public string WorkflowInstanceId { get; }
        public string ActivityCode { get; }
        public string[] Participant { get; }
    }
}