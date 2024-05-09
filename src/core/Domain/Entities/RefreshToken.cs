using Domain.Common;

namespace Domain.Entities;

public class RefreshToken : Entity<int>
{
    /// <summary>
    ///     Gets or sets user primary key.
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    ///     Gets or sets refresh token.
    /// </summary>
    public string Token { get; set; }
}