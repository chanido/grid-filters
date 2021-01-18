using PoorMansGrid.Extensions;

namespace PoorMansGrid
{
    public class TextFilter : FilterCondition
    {
        public string ColumnName { get; set; }
        public FilterModel Model { get; set; }
        public PoorMansGridOptions? Options { get; set; }


        public TextFilter(string columnName, FilterModel model, PoorMansGridOptions? options)
        {
            ColumnName = columnName;
            Model = model;
            Options = options;

            GenerateTextCondition();
        }

        private void GenerateTextCondition()
        {
            var insensitiveCased = Options == PoorMansGridOptions.ForceCaseInsensitive ? ".ToLower()" : string.Empty;

            switch (Model.Type)
            {
                case "equals":
                    Condition = $"{ColumnName}{insensitiveCased} = \"{Model.Filter}\"{insensitiveCased}";
                    break;
                case "notEquals":
                    Condition = $"{ColumnName}{insensitiveCased} != \"{Model.Filter}\"{insensitiveCased}";
                    break;
                case "contains":
                    Condition = $"{ColumnName}{insensitiveCased}.Contains(@{Values.Count}{insensitiveCased})";
                    AddValue(Model.Filter);
                    break;
                case "notContains":
                    Condition = $"!{ColumnName}{insensitiveCased}.Contains(@{Values.Count}{insensitiveCased})";
                    AddValue(Model.Filter);
                    break;
                case "startsWith":
                    Condition = $"{ColumnName}{insensitiveCased}.StartsWith(@{Values.Count}{insensitiveCased})";
                    AddValue(Model.Filter);
                    break;
                case "notStartsWith":
                    Condition = $"!{ColumnName}{insensitiveCased}.StartsWith(@{Values.Count}{insensitiveCased})";
                    AddValue(Model.Filter);
                    break;
                case "endsWith":
                    Condition = $"{ColumnName}{insensitiveCased}.EndsWith(@{Values.Count}{insensitiveCased})";
                    AddValue(Model.Filter);
                    break;
                case "notEndsWith":
                    Condition = $"!{ColumnName}{insensitiveCased}.EndsWith(@{Values.Count}{insensitiveCased})";
                    AddValue(Model.Filter);
                    break;
            }
        }
    }
}