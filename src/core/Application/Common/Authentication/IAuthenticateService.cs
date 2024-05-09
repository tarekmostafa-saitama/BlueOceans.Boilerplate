using Application.Requests.Authentication.Models;
using Shared.ServiceContracts;

namespace Application.Common.Authentication;

/// <summary>
///     Interface for authentication.
/// </summary>
public interface IAuthenticateService : IScopedService
{
    /// <summary>
    ///     Authenticates user.
    ///     Takes responsibilities to generate access and refresh token, save refresh token in database
    ///     and return instance of <see cref="AuthenticateResponse" /> class.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken">Instance of <see cref="CancellationToken" />.</param>
    Task<AuthenticateResponse> Authenticate(string userId, CancellationToken cancellationToken);
}