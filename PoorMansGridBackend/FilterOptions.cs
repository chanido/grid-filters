using System.Collections.Generic;

namespace PoorMansGridBackend
{
    public class FilterOptions
    {
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public Dictionary<string, FilterModel> FilterModels { get; set; }
        public SortModel[] SortModel { get; set; }
    }
}
