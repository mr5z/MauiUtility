using System.Runtime.CompilerServices;

namespace MauiUtility.Helpers;

public static class BindableHelper
{
    private const string KnownPropertyPattern = "Property";

    public static BindableProperty CreateProperty<T>(
        T? defaultValue = default,
        BindingMode mode = BindingMode.TwoWay,
        BindableProperty.BindingPropertyChangedDelegate? propertyChanged = null,
        [CallerMemberName] string? propertyName = null)
    {
        propertyName = RemoveLastOccurrence(propertyName!, KnownPropertyPattern);
        return BindableProperty.Create(
            propertyName,
            typeof(T),
            typeof(BindableObject),
            defaultValue,
            mode,
            propertyChanged: propertyChanged);
    }

    public static BindablePropertyKey CreateReadonlyProperty<T>(
        T? defaultValue = default,
        BindingMode mode = BindingMode.TwoWay,
        BindableProperty.BindingPropertyChangedDelegate? propertyChanged = null,
        [CallerMemberName] string? propertyName = null)
    {
        propertyName = RemoveLastOccurrence(propertyName!, KnownPropertyPattern);
        return BindableProperty.CreateReadOnly(
            propertyName,
            typeof(T),
            typeof(BindableObject),
            defaultValue,
            mode,
            propertyChanged: propertyChanged);
    }

    public static BindableProperty CreateAttached<TProperty>(
        TProperty? defaultValue = default,
        BindingMode mode = BindingMode.TwoWay,
        BindableProperty.BindingPropertyChangedDelegate? propertyChanged = null,
        [CallerMemberName] string? propertyName = null)
    {
        propertyName = RemoveLastOccurrence(propertyName!, KnownPropertyPattern);
        return BindableProperty.CreateAttached(
            propertyName,
            typeof(TProperty),
            typeof(BindableObject),
            defaultValue,
            mode,
            propertyChanged: propertyChanged);
    }

    public static BindableProperty CreateEffect<TEffect, TProperty>(
        BindableProperty.BindingPropertyChangedDelegate? propertyChanged = null,
        [CallerMemberName] string? propertyName = null)
        where TEffect : Effect, new()
    {
        var autoManage = propertyChanged == null;
        var callback = autoManage ? (bindableObject, oldValue, newValue) =>
            {
                if (bindableObject is not View view)
                    return;

                UpdateInstalledEffects<TEffect, TProperty>(view, (TProperty)newValue);
            }
        : propertyChanged;

        return CreateAttached<TProperty>(propertyChanged: callback, propertyName: propertyName);
    }

    private static void UpdateInstalledEffects<TEffect, TProperty>(Element element, TProperty newValue)
        where TEffect : Effect, new()
    {
        var effect = element.Effects.OfType<TEffect>().FirstOrDefault();
        var isDefault = EqualityComparer<TProperty>.Default.Equals(newValue, default!);

        // add
        if (!isDefault && effect == null)
            element.Effects.Add(new TEffect());

        // remove
        if (isDefault && effect != null)
            element.Effects.Remove(effect);
    }

    private static string RemoveLastOccurrence(string source, string toFind)
    {
        var index = source.LastIndexOf(toFind, StringComparison.Ordinal);
        return index == -1 ? source : source[..index];
    }
}
