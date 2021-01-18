namespace PoorMansGrid.FilterTypes
{
    public class NumberFilter : FilterCondition
    {

        public NumberFilter(string columnName, FilterModel model)
        {
            GenerateCondition(model, columnName);
        }

        private void GenerateCondition(FilterModel filterModel, string columnName)
        {
            
            switch (filterModel.Type)
            {
                case "equals":
                    Condition = $"{columnName} = {filterModel.Filter}";
                    break;
                case "notEquals":
                    Condition = $"{columnName} <> {filterModel.Filter}";
                    break;
                case "lessThan":
                    Condition = $"{columnName} < {filterModel.Filter}";
                    break;
                case "lessThanOrEqual":
                    Condition = $"{columnName} <= {filterModel.Filter}";
                    break;
                case "greaterThan":
                    Condition = $"{columnName} > {filterModel.Filter}";
                    break;
                case "greaterThanOrEqual":
                    Condition = $"{columnName} >= {filterModel.Filter}";
                    break;
                case "inRange":
                    Condition = $"({columnName} >= {filterModel.Filter} AND {columnName} <= {filterModel.FilterTo})";
                    break;
            }
        }
    }
}