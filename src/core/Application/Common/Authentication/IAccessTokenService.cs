using Shared.ServiceContracts;

namespace Application.Common.Authentication;

/// <summary>
///     Interface for generating access token.
/// </summary>
public interface IAccessTokenService : ITokenService, IScopedService
{
}