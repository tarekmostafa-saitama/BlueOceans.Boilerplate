using Domain.Entities;
using Shared.ServiceContracts;

namespace Application.Repositories;

public interface IUnitOfWork : IDisposable, IScopedService
{
    public IRefreshTokenRepository RefreshTokensRepository { get; }
    public ITrailsRepository TrailsRepository { get; }
    Task<int> CommitAsync();
}