using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace PoorMansGrid.FilterTypes
{
    public abstract class FilterCondition
    {
        protected string Condition { get; set; }
        protected List<object> Values { get; set; }


        protected FilterCondition()
        {
            Values = new List<object>();
        }

        protected void AddValue(object filter) => Values.Add(filter);

        internal IQueryable<T> AddFilterToQuery<T>(IQueryable<T> query) =>
            Values.Count == 0 ? query.Where(Condition) : query.Where(Condition, Values.ToArray());

    }
}
