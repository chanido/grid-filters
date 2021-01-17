using System.Linq;

namespace GridFilters
{
    public interface IFilterService
    {
        FilteredResult<T> Filter<T>(IQueryable<T> query, FilterOptions options);
    }
}