using Shared.ServiceContracts;

namespace Application.Common.Authentication;

/// <summary>
///     Interface for validating refresh token.
/// </summary>
public interface IRefreshTokenValidator : IScopedService
{
    /// <summary>
    ///     Validates refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token.</param>
    /// <returns>True if token is valid,otherwise false.</returns>
    bool Validate(string refreshToken);
}