using CrossUtility.Helpers;
using MauiUtility.Enums;
using MauiUtility.Services;
using MauiUtility.ViewModels;
using static MauiUtility.Extensions.Common;

namespace MauiUtility.Extensions;

public static class PopupNavigationExtension
{
    public static Task<ModalResult> PushModal<TModalPopup>(
        this IPopupPageNavigation navigationService, 
        INavigationParameters? parameters = null)
        where TModalPopup : ModalViewModel
    {
        return PushModal<TModalPopup, ModalResult>(navigationService, parameters);
    }
    
    public static async Task<TReturnType?> PushModal<TModalPopup, TReturnType>(
        this IPopupPageNavigation popupNavigation,
        INavigationParameters? parameters = null)
        where TModalPopup : ModalViewModel<TReturnType>
    {
        parameters ??= new NavigationParameters();
        var completion = new TaskCompletionSource<TReturnType>();
        parameters.Add(nameof(ModalViewModel.TaskCompletion), completion);

        var page = ToPageName<TModalPopup>();
        var navResult = await popupNavigation.NavigateAsync(page, parameters);
        
        Contract.ThrowOn(navResult.Exception);

        return await completion.Task;
    }
}