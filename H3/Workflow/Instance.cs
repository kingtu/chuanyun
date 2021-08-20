using System;

namespace H3.Workflow
{
    public  class Instance
    {
        public static Token Token { get; internal set; }

        internal interface IToken
        {
            string Activity { get; set; }
        }

        public class WorkflowInstance
        {
            public Token[] Tokens;

            public string BizObjectId { get; internal set; }

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
}