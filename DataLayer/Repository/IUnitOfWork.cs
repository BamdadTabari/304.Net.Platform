using Core.Base.EF;
using DataLayer.Services;

namespace DataLayer.Repository;
public interface IUnitOfWork : IDisposable
{
    IBlogCategoryRepository BlogCategoryRepository { get; }
    IBlogRepository BlogRepository { get; }
    Task<bool> CommitAsync(CancellationToken cancellationToken);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        BlogCategoryRepository = new BlogCategoryRepository(_context);
        BlogRepository = new BlogRepository(_context);
    }

    public IBlogCategoryRepository BlogCategoryRepository { get; }
    public IBlogRepository BlogRepository { get; }
    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken) > 0;

    // dispose and add to garbage collector
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}