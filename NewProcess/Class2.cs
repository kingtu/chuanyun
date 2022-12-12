using H3.Workflow.Instance;
using H3.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Chuanyun.NewProcess
{
    internal class Class2
    {
        void GetDefaultWorkflow(H3.IEngine Engine, string ProcessFlow_TableCode, string me_ProcessFlowForm)
        {
            string instanceId = string.Empty;          //查询表单的流程实例Id
            string tabelCode = string.Empty;          //进行工序表单编码
            string bizObjectId = string.Empty;        //进行数据的objectid
            bool flag = true;
            //初始流程实例Id为工艺流程表的流程实例Id
            H3.DataModel.BizObject flowBizObj =
                Tools.BizOperation.Load(Engine, ProcessFlow_TableCode, me_ProcessFlowForm + string.Empty);
            instanceId = flowBizObj.WorkflowInstanceId;
            //获取当前流程实例对象
            H3.Workflow.Instance.WorkflowInstance instanc = Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId);
            tabelCode = instanc.SchemaCode;

            while (flag)
            {
                //正在进行中的节点
                H3.Workflow.Instance.IToken tok = Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId).GetLastToken();

                // bool flags = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId).IsActivityRunning(tok.Activity);
                // if(!flags)
                // {
                //     int[] toks = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId).GetPreTokens(tok.TokenId);
                //     var tokt = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(instanceId).GetToken(toks[0]);
                // }

                //流程模版
                H3.Workflow.Template.WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(tabelCode);
                //子流程模版或节点
                SubInstanceActivity subWfTemp = wfTemp.GetActivityByCode(tok.Activity) as SubInstanceActivity; ;

                if (subWfTemp == null || subWfTemp.ActivityType + string.Empty == "Approve")//节点
                {
                    // bizObjectId = instanc.BizObjectId + string.Empty;
                    flag = false;
                }
                if (subWfTemp != null && subWfTemp.ActivityType + string.Empty == "SubInstance")//子流程
                {
                    string id = flowBizObj["F0000006"] + string.Empty;
                    // string id = "200-1-1-61";
                    tabelCode = subWfTemp.SchemaCode;
                    string sql = "select ObjectId from i_" + tabelCode + " where status = 2 and name = '" + id + "'";
                    System.Data.DataTable data = Engine.Query.QueryTable(sql, null);
                    if (data != null && data.Rows != null && data.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow row in data.Rows)
                        {
                            bizObjectId = row["ObjectId"] + string.Empty;
                        }
                    }
                    H3.DataModel.BizObject subBizObj = Tools.BizOperation.Load(Engine, tabelCode, bizObjectId);
                    instanceId = subBizObj.WorkflowInstanceId;
                }
            }


            //定义返回前端数据
            Dictionary<string, object> resData = new Dictionary<string, object>();
            if (tabelCode != null && bizObjectId != null)
            {
                resData.Add("tabelCode", tabelCode);
                resData.Add("bizObjectId", bizObjectId);
            }
            //将数据返回前端
            //response.ReturnData = resData;
        }

        //获取最后一个活动节点的显示名称
        public static string GetLastActivityName3(H3.IEngine Engine, string schemaCode, string instanceId)
        {
            string mysql = "SELECT NextTokenId FROM H_WorkflowInstance where ObjectId = '" + instanceId + "'";
            System.Data.DataTable dtInstance = Engine.Query.QueryTable(mysql, null);
            if (dtInstance == null || dtInstance.Rows == null || dtInstance.Rows.Count == 0) { return null; }
            int next = Convert.ToInt32(dtInstance.Rows[0]["NextTokenId"]) - 1;
            string sql = "SELECT Activity FROM H_Token where ParentObjectId = '" + instanceId + "' and TokenId = '" + next + "'";
            System.Data.DataTable dtToken = Engine.Query.QueryTable(sql, null);
            H3.Workflow.Template.WorkflowTemplate wfTemp = Engine.WorkflowTemplateManager.GetDefaultWorkflow(schemaCode);
            //H3.Workflow.Template.SubInstanceActivity subWfTemp = wfTemp.GetActivityByCode(dtToken.Rows[0]["Activity"] + string.Empty) as SubInstanceActivity;
            H3.Workflow.Template.Activity a = wfTemp.GetActivityByCode(dtToken.Rows[0]["Activity"] + string.Empty);
            return a.DisplayName;
        }

        public void f(H3.IEngine Engine, string instanceId, string tabelCode, string bizObjectId)
        {
            // H3.Workflow.Instance.WorkflowInstance wfInstance = null;
            DataTable subprocessData = null;                     //查询子流程的数据结果
                                                                 //string instanceId = string.Empty;                    //查询表单的流程实例Id
                                                                 //string tabelCode = string.Empty;                     //进行工序表单编码
                                                                 //string bizObjectId = string.Empty;                   //进行数据的objectid
            bool isFirst = true;                                 //第一次进入循环
            H3.DataModel.BizObjectSchema aSchema = null;
            //DataTable currentData = null;

            //获取流程实例对象
            //赋值tabelCode
            while ((subprocessData != null && subprocessData.Rows != null && subprocessData.Rows.Count > 0) || isFirst)
            {
                //加载最后一个流程节点信息GetLastToken
                //获取流程模版GetDefaultWorkflow
                //获取子流程模版GetActivityByCode
                //查询流程实例的首个活动子流程
                isFirst = false;
                subprocessData = Engine.Query.QueryChildInstance(instanceId, H3.Workflow.Instance.Token.UnspecifiedId,
                    H3.Workflow.Instance.WorkflowInstanceState.Running, H3.Data.BoolValue.Unspecified);
                if (subprocessData != null && subprocessData.Rows != null && subprocessData.Rows.Count > 0)
                {
                    //currentData = subprocessData;
                    instanceId = subprocessData.Rows[0]["ObjectId"] + string.Empty;       //将活动子流程的流程实例Id作为最新查询的流程实例Id
                    tabelCode = subprocessData.Rows[0]["SchemaCode"] + string.Empty;      //记录查询到的表单编码
                    bizObjectId = subprocessData.Rows[0]["BizObjectId"] + string.Empty;   //记录查询到的数据Id
                    aSchema = Engine.BizObjectManager.GetPublishedSchema(tabelCode);
                    BizObject currentObj = H3.DataModel.BizObject.Load(H3.Organization.User.AnonymousUserId,
                                                                                    Engine, tabelCode, bizObjectId, true);
                    currentObj["F0000003"] += aSchema.DisplayName;
                    currentObj.Update();
                }
                else
                {
                    H3.DataModel.BizObject subObj = H3.DataModel.BizObject.Load(H3.Organization.User.AnonymousUserId, Engine, tabelCode, bizObjectId, true);

                    //DataTable activeinfo = null;
                    //查询当前活动节点信息
                    DataTable activeinfo = Engine.Query.QueryWorkItemDisplayAndParticipant(new string[] { instanceId }, WorkItemState.Unfinished);
                    List<string> l = new List<string>();

                    if (activeinfo != null && activeinfo.Rows != null && activeinfo.Rows.Count > 0)
                    {
                        foreach (DataRow row in activeinfo.Rows)
                        {
                            subObj["F0000003"] += row["activitydisplayname"] + string.Empty;
                        }
                    }
                    subObj.Update();

                }
                //加载业务对象
                //赋值instanceId
                //具体详情请看文档     https://sxshgroup.yuque.com/dfs01y/xg1fm0/rcniyg
            }



        }
    }

}

