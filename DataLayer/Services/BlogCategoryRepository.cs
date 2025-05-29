using Core.Base.EF;
using Core.EntityFramework.Models;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
namespace DataLayer.Services;
public interface IBlogCategoryRepository : IRepository<BlogCategory>
{
}
public class BlogCategoryRepository : Repository<BlogCategory>, IBlogCategoryRepository
{
    private readonly IQueryable<BlogCategory> _queryable;

    public BlogCategoryRepository(ApplicationDbContext context) : base(context)
    {
        _queryable = DbContext.Set<BlogCategory>();
    }
}