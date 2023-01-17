namespace MicroservicesCrudManager.Core.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class AuthorizeService : Attribute
{
    public string Roles { get; }

    public AuthorizeService(string roles)
    {
        Roles = roles;
    }
}