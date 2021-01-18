using PoorMansGrid.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;
using PoorMansGrid.FilterTypes;

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

            if (model.FieldType == "text")
                return new TextFilter(colName, model, Options);

            if (model.FieldType == "number")
                return new NumberFilter(colName, model);

            return new DateFilter(colName, model);

        }
    }
}
