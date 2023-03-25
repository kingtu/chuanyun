using System;
using System.Collections.Generic;

namespace H3
{
    public class BizBus
    {
        public static AccessPointType AccessPointType { get; set; }

        public BizBus.InvokeResult InvokeApi(string systemUserId, object thirdConnection, string v1, string v2, string v3, Dictionary<string, string> headers, Dictionary<string, string> querys, Dictionary<string, object> data, BizStructureSchema structureSchema)
        {
            throw new NotImplementedException();
        }

        public class BizStructure
        {
            object obj;
            public object this[string  o]
            {
                get { return obj; }
                set { obj = value; }
            }
            //public object  Item(System.String itemName) 
            //{ return null;}
        }

        public class BizStructureSchema
        {
            public void Add(BizBus.ItemSchema itemSchema)
            {
                throw new NotImplementedException();
            }
        }

        public class ItemSchema
        {
            private string successful;
            private string v;
            private object @bool;
            private object value;

            public ItemSchema(string successful, string v, object @bool, object value)
            {
                this.successful = successful;
                this.v = v;
                this.@bool = @bool;
                this.value = value;
            }
        }

        public class InvokeResult
        {
            public BizStructure Data { get; set; }
        }
    }
}