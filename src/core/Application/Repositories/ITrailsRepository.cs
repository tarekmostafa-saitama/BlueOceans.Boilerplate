using Domain.Entities;
using Shared.ServiceContracts;

namespace Application.Repositories;

public interface ITrailsRepository : IRepository<Trail>, IScopedService
{
}