using MauiUtility.Extensions.Navigations;

namespace MauiUtility.Exceptions;

public class NavigationValidationException(ViewModelMetadata pageInfo, string? message = null) : Exception(message)
{
    public ViewModelMetadata PageInfo { get; } = pageInfo;
}
