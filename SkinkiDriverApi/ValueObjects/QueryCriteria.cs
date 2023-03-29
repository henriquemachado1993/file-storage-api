using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SkinkiDriverApi.ValueObjects
{
    public class QueryCriteria<T>
    {
        public QueryCriteria()
        {
            Navigation = new List<string>();
        }

        public QueryCriteria(Page pager = null)
        {
            Navigation = new List<string>();
            if (pager != null)
            {
                this.Limit = pager.Limit;
                this.Offset = pager.Offset;
            }
        }

        public Expression<Func<T, bool>> Expression { get; set; }
        public List<string> Navigation { get; set; }
        public bool IgnoreNavigation { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public bool Ascending { get; set; }       
        public int Limit { get; set; }
        public int Offset { get; set; }    
    }
}
