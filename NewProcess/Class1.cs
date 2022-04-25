using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Chuanyun.NewProcess
{
    class Class1
    {
        public H3.Request Request { get; private set; }
        public H3.IEngine Engine { get; private set; }

        string   GetParticipantsBy(string WorkflowId)
        {
            H3.Workflow.Instance.IToken tok = this.Request.Engine.WorkflowInstanceManager.GetWorkflowInstance(WorkflowId).GetLastToken();





            var t1 = tok.Activity; //：流程节点编码，string类型

            H3.Data.BoolValue  t2 = tok.Approval;//：是否同意，H3.Data.BoolValue类型：

            //  H3.Data.BoolValue.True 是同意，

            // H3.Data.BoolValue.False 是不同意，

            //  H3.Data.BoolValue.Unspecified 是未处理或节点被取消



            string t3 = tok.TokenId; //：流程步骤表（H_Token）ObjectId字段值，string类型

            string[] t4 = tok.Participants;//：流程审批人，string[] 类型，每一个数组元素对应一个审批人用户ID

            DateTime t5 = tok.CreatedTime;//：流程节点创建时间，DateTime类型

            DateTime t6 = tok.FinishedTime;//：流程节点结束事件，DateTime类型

            TimeSpan t7 = tok.UsedTime;//：当前节点从开始到结束耗时，TimeSpan类型
            string users="";
            if(tok .Participants .Length >0)
            {
                foreach( string p in tok.Participants)
                {
                    DataRow dr = GetRow("User", "ObjectId='" + p + "'");
                    users += dr["Name"] + ";" ;
                } 
            }

            return users;

        }

        private DataRow GetRow(string table, string where, string selector = "*")
        {
            string sql = "select " + selector + " from " + "H_" + table + (where == "" ? "" : " where " + where);

            DataTable dt = this.Engine.Query.QueryTable(sql, null);
            int Count = dt.Rows.Count;
            if (Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }
    }
}
