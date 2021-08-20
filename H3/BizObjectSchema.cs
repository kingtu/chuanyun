using System;
using System.Collections.Generic;
using System.Text;

namespace H3.DataModel
{
    public class BizObjectSchema
    {
        public IEnumerable<PropertySchema> Properties { get; internal set; }
        public string  SchemaCode { get; internal set; }

        internal BizObjectSchema GetChildSchema(string v)
        {
            throw new NotImplementedException();
        }
    }
}
