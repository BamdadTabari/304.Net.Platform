using Core.Base.EF;
using Core.ExtensionAndSort;
using Core.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataLayer.Repository;
public interface IRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<bool> ExistsAsync();
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    int Count();
    int Count(Expression<Func<TEntity, bool>> predicate);
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    Task<TEntity> AddAsyncReturnId(TEntity entity);

    Task<TEntity?> FindSingle(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    List<TEntity> FindList(params string[]? includes);

    Task<TEntity?> FindSingleAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    Task<TEntity?> FindFirstAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes);
    List<TEntity> FindListAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes);

    Task<PaginatedList<TEntity>> GetPagedResultAsync(DefaultPaginationFilter filter, Expression<Func<TEntity, bool>>? predicate, params string[]? includes);
}

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
{
    protected readonly DbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    #region Queries

    public async Task<bool> ExistsAsync()
    {
        return await DbContext.Set<TEntity>().AnyAsync();
    }

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    #endregion

    #region Sync Commands

    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().AddRange(entities);
    }

    public void Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().UpdateRange(entities);
    }

    #endregion

    #region Async Commands

    public async Task AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities);
    }

    public async Task<TEntity> AddAsyncReturnId(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public int Count()
    {
        return DbContext.Set<TEntity>().Count();
    }

    public int Count(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().Count(predicate);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DbContext.Set<TEntity>().AnyAsync(predicate);
    }

    private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, string[]? includes)
    {
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query;
    }

    public async Task<TEntity?> FindSingle(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return query.Where(predicate).ToList();
    }

    public List<TEntity> FindList(params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        return query.ToList();
    }

    public async Task<TEntity?> FindSingleAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>().AsNoTracking(), includes);
        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> FindFirstAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>().AsNoTracking(), includes);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public List<TEntity> FindListAsNoTracking(Expression<Func<TEntity, bool>> predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>().AsNoTracking(), includes);
        return query.Where(predicate).ToList();
    }

    public async Task<PaginatedList<TEntity>> GetPagedResultAsync(DefaultPaginationFilter filter, Expression<Func<TEntity, bool>>? predicate, params string[]? includes)
    {
        var query = ApplyIncludes(DbContext.Set<TEntity>(), includes);
        if (predicate != null)
            query = query.Where(predicate);

        if (typeof(IBaseEntity).IsAssignableFrom(typeof(TEntity)))
        {
            var filterableQuery = query.Cast<IBaseEntity>();
            filterableQuery = filterableQuery.ApplyFilter(filter);
            filterableQuery = filterableQuery.ApplySort(filter.SortBy);
            query = filterableQuery.Cast<TEntity>();
        }
        else
        {
            query = query.OrderByDescending(x => EF.Property<object>(x, "Id"));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginatedList<TEntity>(items, totalCount, filter.Page, filter.PageSize);
    }

    #endregion
}