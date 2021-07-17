using H3.Data.Filter;
using System;

namespace H3.DataModel
{
    public class BizObject
    {
        private IEngine engine;
        private BizObjectSchema tableSchema;
        private string userId;
        public  string  ObjectId;

        public object Status { get; internal set; }
        public string WorkflowInstanceId { get; internal set; }
        public H3.DataModel.BizObjectSchema Schema { get; internal set; }

        public BizObject(IEngine engine, BizObjectSchema tableSchema, string userId)
        {
            this.engine = engine;
            this.tableSchema = tableSchema;
            this.userId = userId;
        }

        internal static BizObject Load(string userId, IEngine engine, string v1, string bizObjectId, bool v2)
        {
            throw new NotImplementedException();
        }

        internal static BizObject[] GetList(IEngine engine, string userId, BizObjectSchema tableSchema, object globalAll, Filter filter)
        {
            throw new NotImplementedException();
        }
        //public string Item;
        public object this[string day]
        {
            get { return null; }
            set { }
        }

        internal void Update()
        {
            throw new NotImplementedException();
        }

        internal void Create()
        {
            throw new NotImplementedException();
        }

        internal void Remove()
        {
            throw new NotImplementedException();
        }
    }
}