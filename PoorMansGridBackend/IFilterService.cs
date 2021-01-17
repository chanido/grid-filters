using System.Linq;

namespace PoorMansGridBackend
{
    public interface IFilterService
    {
        FilteredResult<T> Filter<T>(IQueryable<T> query, FilterOptions options);
    }
}