using Shared.ServiceContracts;

namespace Application.Common.Authentication;

/// <summary>
///     Interface for generating refresh token.
/// </summary>
public interface IRefreshTokenService : ITokenService, IScopedService
{
}