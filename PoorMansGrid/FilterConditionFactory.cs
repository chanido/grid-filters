using PoorMansGrid.Extensions;
using PoorMansGrid.FilterTypes;

namespace PoorMansGrid
{
    public class FilterConditionFactory
    {

        public static FilterCondition Create(string colName, FilterModel model, TextSearchOption? poorMansGridOptions)
        {

            if (model.FieldType == "text")
                return new TextFilter(colName, model, poorMansGridOptions);

            if (model.FieldType == "number")
                return new NumberFilter(colName, model);

            return new DateFilter(colName, model);

        }
    }
}