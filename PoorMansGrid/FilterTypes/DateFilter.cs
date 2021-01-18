namespace PoorMansGrid.FilterTypes
{
    public class DateFilter : FilterCondition
    {

        public DateFilter(string columnName, FilterModel model)
        {
            GenerateCondition(model, columnName);
        }

        private void GenerateCondition(FilterModel filterModel, string columnName)
        {

            AddValue(filterModel.Filter);

            switch (filterModel.Type)
            {
                case "equals":
                    Condition = $"{columnName} = @{Values.Count - 1}";
                    break;
                case "notEquals":
                    Condition = $"{columnName} <> @{Values.Count - 1}";
                    break;
                case "lessThan":
                    Condition = $"{columnName} < @{Values.Count - 1}";
                    break;
                case "lessThanOrEqual":
                    Condition = $"{columnName} <= @{Values.Count - 1}";
                    break;
                case "greaterThan":
                    Condition = $"{columnName} > @{Values.Count - 1}";
                    break;
                case "greaterThanOrEqual":
                    Condition = $"{columnName} >= @{Values.Count - 1}";
                    break;
                case "inRange":
                    AddValue(filterModel.FilterTo);
                    Condition =
                        $"({columnName} >= @{Values.Count - 2} AND {columnName} <= @{Values.Count - 1})";
                    break;
            }
        }
    }
}