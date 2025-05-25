using Core.Base.EF;
using Core.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public interface IBlogRepository : IRepository<Blog>
{
}
public class BlogRepository : Repository<Blog>, IBlogRepository
{
	private readonly IQueryable<Blog> _queryable;

	public BlogRepository(ApplicationDbContext context) : base(context)
	{
		_queryable = DbContext.Set<Blog>();
	}
}