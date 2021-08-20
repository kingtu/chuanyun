using H3.DataModel;
using H3.Workflow;
using System.Collections.Generic;

namespace H3
{
    public class SmartForm
    {
        public class SmartFormPostValue
        {
            public Dictionary<string,string> Data { get;  set; }
        }

        public class ListViewPostValue
        {
            public Dictionary<string, string> Data { get; set; }
        }
        public class SubmitSmartFormResponse
        {
            internal Errors Errors;

            public string Message { get; internal set; }
        }

        public class LoadSmartFormResponse
        {
            internal Errors Errors;
        }

        public class SmartFormController
        {
            public SmartFormRequest Request ;
            public IEngine Engine;

            public SmartFormController(SmartFormRequest request)
            {
                this.Request = request;
            }
            protected virtual  void OnLoad(H3.SmartForm.LoadSmartFormResponse response)
            {

            }
            protected virtual void OnSubmit(string actionName, H3.SmartForm.SmartFormPostValue postValue, H3.SmartForm.SubmitSmartFormResponse response)
            {

            }
        }

        public class SmartFormRequest
        {
            public string ActivityCode { get;  set; }
            //public Workflow.Instance.WorkflowInstance WorkflowInstance { get; internal set; }
            public BizObject BizObject;

            public string BizObjectId { get; set; }
           // public string ActivityCode { get; set; }
            public ActivityTemplate ActivityTemplate { get; set; }
            public UserContext UserContext { get; set; }
            public bool IsCreateMode { get; set; }
            public string ParticipantId { get; set; }
            public string SchemaCode { get; set; }
            public H3.Workflow.Workflow WorkflowInstance { get; set; }
            public WorkItem WorkItem { get; set; }
            public H3.IEngine Engine { get;  set; }
        }
    }
    public class SmartFormController
    {
    }
}