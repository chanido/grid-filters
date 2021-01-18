﻿using PoorMansGrid.Extensions;
using PoorMansGrid.FilterConditions;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace PoorMansGrid
{
    public class FilterService : IFilterService
    {
        public TextSearchOption Options { get; }

        public FilterService(TextSearchOption options = TextSearchOption.Default)
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
                var condition = FilterConditionFactory.Create(fieldName, filterModel, Options);

                query = condition.AddFilterToQuery(query);
            }

            return query;
        }

        private IQueryable<T> ApplySort<T>(IQueryable<T> query, FilterOptions options)
        {
            if (options.SortModel == null) return query;

            foreach (var sortModel in options.SortModel)
                query = query.OrderBy($"{sortModel.ColId}{(sortModel.Sort?.ToLower() == "desc" ? " descending" : string.Empty)}");

            return query;
        }
    }
}
