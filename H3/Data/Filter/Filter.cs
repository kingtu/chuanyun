using System;
using System.Collections.Generic;
using System.Text;

namespace H3.Data.Filter
{
    public class Filter 
    {
        public ItemMatcher Matcher { get;  set; }
        public int FromRowNum { get;  set; }
        public int ToRowNum { get;  set; }

        public void AddSortBy(string v, object descending)
        {
            throw new NotImplementedException();
        }
    }
}
