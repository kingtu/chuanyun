using H3.Data.Filter;
using System;
using System.Collections.Generic;

namespace H3.DataModel
{
    public class BizObject
    {
        private IEngine engine;
        private BizObjectSchema tableSchema;
        private string userId;
        public  string  ObjectId;

        private Dictionary <string ,object > inner =new Dictionary <string ,object > ();
        //private Loginer DataProxy;

        public object Status { get;  set; }
        public string WorkflowInstanceId { get;  set; }
        public H3.DataModel.BizObjectSchema Schema { get;  set; }
        public H3.DataModel.BizObjectState State { get;  set; }
        

        public BizObject(IEngine engine, BizObjectSchema tableSchema, string userId)
        {
            this.engine = engine;
            this.tableSchema = tableSchema;
            this.userId = userId;
        }

        public static BizObject Load(string userId, IEngine engine, string v1, string bizObjectId, bool v2)
        {
            throw new NotImplementedException();
        }

        public static BizObject[] GetList(IEngine engine, string userId, BizObjectSchema tableSchema, object globalAll, Filter filter)
        {
            throw new NotImplementedException();
        }
        //public string Item;
        public object this[string itemName]
        {
            get { return inner[itemName]; }
            set { inner[itemName] = value; }
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}