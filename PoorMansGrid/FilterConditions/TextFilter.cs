using PoorMansGrid.Extensions;
using System;

namespace PoorMansGrid.FilterConditions
{
    public class TextFilter : FilterCondition
    {
        public TextFilter(string columnName, FilterModel model, TextSearchOption options)
        {
            GenerateTextCondition(model, columnName, options);
        }

        private void GenerateTextCondition(FilterModel model, string columnName, TextSearchOption? options)
        {
            var insensitiveCased = options == TextSearchOption.ForceCaseInsensitive ? ".ToLower()" : string.Empty;

            if (model.Filter != null) Values.Add(model.Filter);

            Condition = model.Type switch
            {
                "equals" => $@"{columnName}{insensitiveCased} = ""{model.Filter}""{insensitiveCased}",
                "notEquals" => $@"{columnName}{insensitiveCased} != ""{model.Filter}""{insensitiveCased}",
                "contains" => $"{columnName}{insensitiveCased}.Contains(@0{insensitiveCased})",
                "notContains" => $"!{columnName}{insensitiveCased}.Contains(@0{insensitiveCased})",
                "startsWith" => $"{columnName}{insensitiveCased}.StartsWith(@0{insensitiveCased})",
                "notStartsWith" => $"!{columnName}{insensitiveCased}.StartsWith(@0{insensitiveCased})",
                "endsWith" => $"{columnName}{insensitiveCased}.EndsWith(@0{insensitiveCased})",
                "notEndsWith" => $"!{columnName}{insensitiveCased}.EndsWith(@0{insensitiveCased})",
                _ => throw new ArgumentOutOfRangeException($"A filter of type {model.Type} cannot be applied to a Text filter.")
            };
        }
    }
}