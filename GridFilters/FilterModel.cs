using System;

namespace GridFilters
{
    public class FilterModel
    {
        public string FilterType { get; set; }
        public string Type { get; set; }
        public object Filter { get; set; }
        public object FilterTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string LogicOperator { get; set; }
        public FilterModel Condition1 { get; set; }
        public FilterModel Condition2 { get; set; }

        public FilterModel() { }
    }
}