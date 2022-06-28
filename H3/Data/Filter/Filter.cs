using System;
using System.Collections.Generic;
using System.Text;

namespace H3.Data.Filter
{
    public class Filter 
    {
        public ItemMatcher Matcher { get; internal set; }
        public int FromRowNum { get; internal set; }
        public int ToRowNum { get; internal set; }

        internal void AddSortBy(string v, object descending)
        {
            throw new NotImplementedException();
        }
    }
}
