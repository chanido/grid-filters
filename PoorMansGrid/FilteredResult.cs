using System.Collections.Generic;

namespace PoorMansGrid
{
    public class FilteredResult<T>
    {
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}