namespace Application.Requests.Authentication.Models;

public class RefreshRequest
{
    /// <summary>
    ///     The refresh token.
    /// </summary>
    public string RefreshToken { get; set; }
}