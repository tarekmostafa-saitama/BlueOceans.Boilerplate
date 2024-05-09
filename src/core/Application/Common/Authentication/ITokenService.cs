namespace Application.Common.Authentication;

/// <summary>
///     Interface for generating token.
/// </summary>
public interface ITokenService
{
    /// <summary>
    ///     Generates token based on user information.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="user"><see cref="User" /> instance.</param>
    /// <returns>Generated token.</returns>
    Task<string> Generate(string userId);
}