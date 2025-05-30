using Core.Base.EF;
using Core.Pagination;

namespace Core.ExtensionAndSort;
public static class FilterExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, DefaultPaginationFilter filter)
        where T : class, IBaseEntity
    {
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var keyword = filter.SearchTerm.ToLower().Trim();
            query = query.Where(x =>
                (x.name != null && x.name.ToLower().Contains(keyword)) ||
                (x.slug != null && x.slug.ToLower().Contains(keyword)));
        }

        return query;
    }
}