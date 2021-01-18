using System.Linq;

namespace PoorMansGrid
{
    public interface IFilterService
    {
        FilteredResult<T> Filter<T>(IQueryable<T> query, FilterOptions options);
    }
}