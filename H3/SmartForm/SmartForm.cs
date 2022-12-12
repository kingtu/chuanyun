using H3.DataModel;
using H3.Workflow;
using System;
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
            public Dictionary<string, object> Data { get; set; }
        }
        public class SubmitSmartFormResponse
        {
            internal Errors Errors;

            public string Message { get; internal set; }
            public bool ClosePage { get; internal set; }
            public Dictionary<string, object> ReturnData { get; internal set; }
        }

        public class LoadSmartFormResponse
        {
            internal Errors Errors;

            public Dictionary <string ,object> ReturnData { get; internal set; }
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
            public T Deserialize<T  >(string idList)
            {
                return default (T);
            }
            protected virtual void OnWorkflowInstanceStateChanged(H3.Workflow.Instance.WorkflowInstanceState oldState, H3.Workflow.Instance.WorkflowInstanceState newState)
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

        public class ListViewController
        {
            public H3.IEngine Engine { get; set; }

            public ListViewRequest Request;

            public ListViewController(ListViewRequest request)
            {
                this.Request = request;
            }

            protected virtual  void OnLoad(H3.SmartForm.LoadListViewResponse response) { }
            protected virtual void OnSubmit(string actionName, H3.SmartForm.ListViewPostValue postValue, H3.SmartForm.SubmitListViewResponse response) { }
        }

        public class ListViewRequest
        {
            public string SchemaCode { get; internal set; }
            public UserContext UserContext { get; internal set; }
            public H3.IEngine Engine { get; internal set; }
        }

        public class LoadListViewResponse
        {
            public string SchemaCode { get; internal set; }
        }

        public class SubmitListViewResponse
        {
            public string Message { get; internal set; }
        }

        public class RestApiController
        {
            public RestApiController(H3.SmartForm.RestApiRequest request) { }
            public Request Request;
            protected virtual  void OnInvoke(string actionName, H3.SmartForm.RestApiResponse response) { }
        }

        public class RestApiRequest
        {
        }

        public class RestApiResponse
        {
            public Dictionary <string ,object > ReturnData { get;  set; }
        }

        internal class SmartFormResponseDataItem
        {
            public SmartFormResponseDataItem()
            {
            }

            public string Value { get; internal set; }
        }
    }
    public class SmartFormController
    {
    }
}