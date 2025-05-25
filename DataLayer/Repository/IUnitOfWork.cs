using Core.Base.EF;
using DataLayer.Services;
using System.Threading;

namespace DataLayer.Repository;
public interface IUnitOfWork : IDisposable
{
	IBlogCategoryRepository BlogCategoryRepository { get; }
	IBlogRepository BlogRepository { get; }
	IOrderRepository OrderRepository { get; }
	IOtpRepository OtpRepository { get; }
	ITokenBlacklistRepository TokenBlacklistRepository { get; }
	IUserRepo UserRepository { get; }
	IUserRoleRepo UserRoleRepository { get; }
	IRoleRepo RoleRepository { get; }
	ICityRepository CityRepository { get; }
	IOptionRepository OptionRepository { get; }
	IPlanRepository PlanRepository { get; }
	IProvinceRepository ProvinceRepository { get; }
	ITicketRepository TicketRepository { get; }
	ITicketReplyRepository TicketReplyRepository { get; }
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
		OrderRepository = new OrderRepository(_context);
		OtpRepository = new OtpRepository(_context);
		TokenBlacklistRepository = new TokenBlacklistRepo(_context);
		UserRepository = new UserRepo(_context);
		UserRoleRepository = new UserRoleRepo(_context);
		RoleRepository = new RoleRepo(_context);
		CityRepository = new CityRepository(_context);
		OptionRepository = new OptionRepository(_context);
		PlanRepository = new PlanRepository(_context);
		ProvinceRepository = new ProvinceRepository(_context);
		TicketRepository = new TicketRepository(_context);
		TicketReplyRepository = new TicketReplyRepository(_context);
	}

	public IBlogCategoryRepository BlogCategoryRepository { get; }
	public IBlogRepository BlogRepository { get; }
	public IOrderRepository OrderRepository { get; }
	public IOtpRepository OtpRepository { get; }
	public ITokenBlacklistRepository TokenBlacklistRepository { get; }
	public IUserRepo UserRepository { get; }
	public IUserRoleRepo UserRoleRepository { get; }
	public IRoleRepo RoleRepository { get; }
	public ICityRepository CityRepository { get; set; }
	public IOptionRepository OptionRepository { get; set; }
	public IPlanRepository PlanRepository { get; set; }
	public IProvinceRepository ProvinceRepository { get; set; }
	public ITicketRepository TicketRepository { get; set; }
	public ITicketReplyRepository TicketReplyRepository { get; set; }
	public async Task<bool> CommitAsync(CancellationToken cancellationToken) 
		=> await _context.SaveChangesAsync(cancellationToken) > 0;

	// dispose and add to garbage collector
	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}
}