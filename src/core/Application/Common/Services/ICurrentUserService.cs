using Shared.ServiceContracts;

namespace Application.Common.Services;

public interface ICurrentUserService : IScopedService
{
    string Id { get; }
}