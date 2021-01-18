using PoorMansGrid.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace PoorMansGrid
{
    public class FilterService : IFilterService
    {
        public PoorMansGridOptions? Options { get; }

        public FilterService(PoorMansGridOptions? options = null)
        {
            Options = options;
        }

        public FilteredResult<T> Filter<T>(IQueryable<T> query, FilterOptions options)
        {
            query = ApplyFilters(query, options);

            query = ApplySort(query, options);

            var count = query.Count();

            var pageSize = options.EndRow - options.StartRow < 1 ? 20 : options.EndRow - options.StartRow;

            var result = query
                .Skip(options.StartRow)
                .Take(pageSize);

            return new FilteredResult<T>
            {
                Items = result.ToList(),
                TotalItems = count
            };
        }

        private IQueryable<T> ApplyFilters<T>(IQueryable<T> query, FilterOptions options)
        {
            if (options.FilterModels == null) return query;

            //The day we want to allow more different logic operators between filters we can do it like this
            //condition = $"{leftSide} {filterModel.LogicOperator} {rightSide}";

            foreach (var (fieldName, filterModel) in options.FilterModels)
            {
                ParseFilterValues(filterModel);

                var condition = GetConditionFromModel(fieldName, filterModel);

                query = condition.AddFilterToQuery(query);
            }

            return query;
        }

        private void ParseFilterValues(FilterModel filterModel)
        {
            if (filterModel.FieldType == "date")
            {
                filterModel.Filter = filterModel.Filter.ToDateTime();
                filterModel.FilterTo = filterModel.FilterTo.ToDateTime();
            }
        }

        private IQueryable<T> ApplySort<T>(IQueryable<T> query, FilterOptions options)
        {
            if (options.SortModel == null) return query;

            foreach (var sortModel in options.SortModel)
                query = query.OrderBy($"{sortModel.ColId}{(sortModel.Sort?.ToLower() == "desc" ? " descending" : string.Empty)}");

            return query;
        }

        private FilterCondition GetConditionFromModel(string colName, FilterModel model)
        {
            var filterCondition = new FilterCondition();

            switch (model.FieldType)
            {
                case "text": return new TextFilter(colName, model, Options);

                case "number":
                    switch (model.Type)
                    {
                        case "equals":
                            filterCondition.Condition = $"{colName} = {model.Filter}";
                            break;
                        case "notEquals":
                            filterCondition.Condition = $"{colName} <> {model.Filter}";
                            break;
                        case "lessThan":
                            filterCondition.Condition = $"{colName} < {model.Filter}";
                            break;
                        case "lessThanOrEqual":
                            filterCondition.Condition = $"{colName} <= {model.Filter}";
                            break;
                        case "greaterThan":
                            filterCondition.Condition = $"{colName} > {model.Filter}";
                            break;
                        case "greaterThanOrEqual":
                            filterCondition.Condition = $"{colName} >= {model.Filter}";
                            break;
                        case "inRange":
                            filterCondition.Condition = $"({colName} >= {model.Filter} AND {colName} <= {model.FilterTo})";
                            break;
                    }
                    break;

                case "date":
                    filterCondition.AddValue(model.Filter);

                    switch (model.Type)
                    {
                        case "equals":
                            filterCondition.Condition = $"{colName} = @{filterCondition.Values.Count - 1}";
                            break;
                        case "notEquals":
                            filterCondition.Condition = $"{colName} <> @{filterCondition.Values.Count - 1}";
                            break;
                        case "lessThan":
                            filterCondition.Condition = $"{colName} < @{filterCondition.Values.Count - 1}";
                            break;
                        case "lessThanOrEqual":
                            filterCondition.Condition = $"{colName} <= @{filterCondition.Values.Count - 1}";
                            break;
                        case "greaterThan":
                            filterCondition.Condition = $"{colName} > @{filterCondition.Values.Count - 1}";
                            break;
                        case "greaterThanOrEqual":
                            filterCondition.Condition = $"{colName} >= @{filterCondition.Values.Count - 1}";
                            break;
                        case "inRange":
                            filterCondition.AddValue(model.FilterTo);
                            filterCondition.Condition =
                                $"({colName} >= @{filterCondition.Values.Count - 2} AND {colName} <= @{filterCondition.Values.Count - 1})";
                            break;
                    }
                    break;
            }

            return filterCondition;
        }
    }
}
