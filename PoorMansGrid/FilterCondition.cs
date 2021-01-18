using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace PoorMansGrid
{
    public class FilterCondition
    {
        public string Condition { get; set; }
        public List<object> Values { get; set; }


        public FilterCondition()
        {
            Values = new List<object>();
        }

        internal void AddValue(object filter) => Values.Add(filter);

        internal IQueryable<T> AddFilterToQuery<T>(IQueryable<T> query) =>
            Values.Count == 0 ? query.Where(Condition) : query.Where(Condition, Values.ToArray());

    }
}
