using Application.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    private readonly Lazy<ITrailsRepository> _trailsRepository;
    private readonly Lazy<IRefreshTokenRepository> _refreshTokenRepository;
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        _trailsRepository = new Lazy<ITrailsRepository>(() => new TrailsRepository(_context));
        _refreshTokenRepository = new Lazy<IRefreshTokenRepository>(() => new RefreshTokenRepository(_context));
    }



    public IRefreshTokenRepository RefreshTokensRepository => _refreshTokenRepository.Value;
    public ITrailsRepository TrailsRepository => _trailsRepository.Value;
    
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}