using System.Collections.Generic;
using System.Linq.Expressions;
using PoorMansGrid.Extensions;

namespace PoorMansGrid.FilterTypes
{
    public class DateFilter : FilterCondition
    {

        public DateFilter(string columnName, FilterModel model)
        {
            model.Filter = model.Filter.ToDateTime();
            model.FilterTo = model.FilterTo.ToDateTime();

            GenerateCondition(model, columnName);
        }

        private void GenerateCondition(FilterModel filterModel, string columnName)
        {

            
            AddValue(filterModel.Filter);

            switch (filterModel.Type)
            {
                case "equals":
                    Condition = $"{columnName} = @{0}";
                    break;
                case "notEquals":
                    Condition = $"{columnName} <> @{0}";
                    break;
                case "lessThan":
                    Condition = $"{columnName} < @{0}";
                    break;
                case "lessThanOrEqual":
                    Condition = $"{columnName} <= @{0}";
                    break;
                case "greaterThan":
                    Condition = $"{columnName} > @{0}";
                    break;
                case "greaterThanOrEqual":
                    Condition = $"{columnName} >= @{0}";
                    break;
                case "inRange":
                    AddValue(filterModel.FilterTo);
                    Condition = $"({columnName} >= @{0} AND {columnName} <= @{1})";
                    break;
            }
        }
    }
}