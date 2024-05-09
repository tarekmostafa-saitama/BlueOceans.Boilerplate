namespace Shared.Contracts;

public interface ISoftDelete
{
    DateTimeOffset? DeletedOn { get; set; }
    string DeletedBy { get; set; }
}