using System;

namespace GridFilters
{
    public class FilterModel
    {
        public enum FilterTypes { }

        /// <summary>
        /// The type of the field we will be filtering
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// The type of filter we will be applying
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The value to compare the filter to (in binary filters it can be used as Filter From)
        /// </summary>
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