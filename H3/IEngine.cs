
using H3.Data;
using H3.Workflow.Instance;
using System;
using System.Data;

namespace H3
{
    public interface IEngine
    {
        BizObjectManager BizObjectManager { get; set; }
        WorkflowInstanceManager WorkflowInstanceManager { get; set; }
        Interactor Interactor { get; set; }
        WorkflowTemplateManager WorkflowTemplateManager { get; set; }
        Query Query { get; set; }
        IOrganization Organization { get; set; }
    }

  

    public class Query
    {
         public  DataTable QueryTable(string sql, object p)
        {
            throw new NotImplementedException();
        }

        internal DataTable QueryChildInstance(string instanceId, object unspecifiedId, WorkflowInstanceState running, BoolValue unspecified)
        {
            throw new NotImplementedException();
        }

        internal DataTable QueryWorkItemDisplayAndParticipant(string[] vs, WorkItemState unfinished)
        {
            throw new NotImplementedException();
        }
    }
}