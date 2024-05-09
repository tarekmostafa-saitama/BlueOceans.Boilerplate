using System.Collections.ObjectModel;

namespace Shared.Permissions;

public static class Actions
{
    public const string View = nameof(View);
    public const string Search = nameof(Search);
    public const string Create = nameof(Create);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    public const string Export = nameof(Export);
}

public static class Resources
{
    public const string Users = nameof(Users);
    public const string Roles = nameof(Roles);
}

public static class Permissions
{
    private static readonly Permission[] _all =
    {
        new("View Users", Actions.View, Resources.Users, true)
    };

    public static IReadOnlyList<Permission> All { get; } = new ReadOnlyCollection<Permission>(_all);

    public static IReadOnlyList<Permission> Admin { get; } =
        new ReadOnlyCollection<Permission>(_all.Where(p => p.IsAdmin).ToArray());
}

public record Permission(string Description, string Action, string Resource, bool IsAdmin = false)
{
    public string Name => NameFor(Action, Resource);

    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}