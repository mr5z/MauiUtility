namespace MauiUtility.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresRoleAttribute(string roles) : Attribute
{
    public string Roles { get; } = roles;
}
