using Domain.Entities;
using Shared.ServiceContracts;

namespace Application.Repositories;

public interface IRefreshTokenRepository : IRepository<RefreshToken>, IScopedService
{
}