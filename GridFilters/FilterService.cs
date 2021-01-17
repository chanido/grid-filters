using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace GridFilters
{
    public class FilterService : IFilterService
    {
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

            foreach (var filterModel in options.FilterModels)
            {
                string condition, tmp;
                var conditionValues = new List<object>();

                if (!string.IsNullOrWhiteSpace(filterModel.Value.LogicOperator))
                {
                    tmp = GetConditionFromModel(filterModel.Key, filterModel.Value.Condition1, conditionValues);
                    condition = tmp;

                    tmp = GetConditionFromModel(filterModel.Key, filterModel.Value.Condition2, conditionValues);
                    condition = $"{condition} {filterModel.Value.LogicOperator} {tmp}";
                }
                else
                {
                    condition = GetConditionFromModel(filterModel.Key, filterModel.Value, conditionValues);
                }

                query = conditionValues.Count == 0 ? query.Where(condition) : query.Where(condition, conditionValues.ToArray());
            }

            return query;
        }

        private IQueryable<T> ApplySort<T>(IQueryable<T> query, FilterOptions options)
        {
            if (options.SortModel == null) return query;

            foreach (var sortModel in options.SortModel)
                query = query.OrderBy($"{sortModel.ColId}{(sortModel.Sort.ToLower() == "desc" ? " descending" : string.Empty)}");

            return query;
        }

        private string GetConditionFromModel(string colName, FilterModel model, List<object> values)
        {
            var modelResult = "";

            switch (model.FilterType)
            {
                case "text":
                    switch (model.Type)
                    {
                        case "equals":
                            modelResult = $"{colName} = \"{model.Filter}\"";
                            break;
                        case "notEqual":
                            modelResult = $"{colName} != \"{model.Filter}\"";
                            break;
                        case "contains":
                            modelResult = $"{colName}.Contains(@{values.Count})";
                            values.Add(model.Filter);
                            break;
                        case "notContains":
                            modelResult = $"!{colName}.Contains(@{values.Count})";
                            values.Add(model.Filter);
                            break;
                        case "startsWith":
                            modelResult = $"{colName}.StartsWith(@{values.Count})";
                            values.Add(model.Filter);
                            break;
                        case "endsWith":
                            modelResult = $"!{colName}.StartsWith(@{values.Count})";
                            values.Add(model.Filter);
                            break;
                    }

                    break;
                case "number":
                    switch (model.Type)
                    {
                        case "equals":
                            modelResult = $"{colName} = {model.Filter}";
                            break;
                        case "notEqual":
                            modelResult = $"{colName} <> {model.Filter}";
                            break;
                        case "lessThan":
                            modelResult = $"{colName} < {model.Filter}";
                            break;
                        case "lessThanOrEqual":
                            modelResult = $"{colName} <= {model.Filter}";
                            break;
                        case "greaterThan":
                            modelResult = $"{colName} > {model.Filter}";
                            break;
                        case "greaterThanOrEqual":
                            modelResult = $"{colName} >= {model.Filter}";
                            break;
                        case "inRange":
                            modelResult = $"({colName} >= {model.Filter} AND {colName} <= {model.FilterTo})";
                            break;
                    }

                    break;
                case "date":
                    values.Add(model.DateFrom);

                    switch (model.Type)
                    {
                        case "equals":
                            modelResult = $"{colName} = @{values.Count - 1}";
                            break;
                        case "notEqual":
                            modelResult = $"{colName} <> @{values.Count - 1}";
                            break;
                        case "lessThan":
                            modelResult = $"{colName} < @{values.Count - 1}";
                            break;
                        case "lessThanOrEqual":
                            modelResult = $"{colName} <= @{values.Count - 1}";
                            break;
                        case "greaterThan":
                            modelResult = $"{colName} > @{values.Count - 1}";
                            break;
                        case "greaterThanOrEqual":
                            modelResult = $"{colName} >= @{values.Count - 1}";
                            break;
                        case "inRange":
                            values.Add(model.DateTo);
                            modelResult =
                                $"({colName} >= @{values.Count - 2} AND {colName} <= @{values.Count - 1})";
                            break;
                    }

                    break;
            }

            return modelResult;
        }
    }
}
