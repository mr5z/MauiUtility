using CrossUtility.Extensions;
using CrossUtility.Helpers;
using PropertyChanged;
using MauiUtility.Enums;

namespace MauiUtility.ViewModels;

/// <inheritdoc />
public class ModalViewModel(INavigationService navigationService) : ModalViewModel<ModalResult>(navigationService)
{
}

/// <summary>
/// ViewModel for modal pages.
/// </summary>
/// <param name="navigationService"></param>
/// <typeparam name="TReturnType"></typeparam>
public abstract class ModalViewModel<TReturnType>(INavigationService navigationService)
    : BaseViewModel(navigationService), IDismissableModal
{
    public override void Initialize(INavigationParameters parameters)
    {
        base.Initialize(parameters);
        TaskCompletion = parameters[nameof(TaskCompletion)] as TaskCompletionSource<TReturnType?>;
    }

    protected async Task DismissWithResult(TReturnType? result)
    {
        // TODO cast to object if null
        var parameter = result?.AsDictionary();
        var navParam = new NavigationParameters();
        if (parameter != null)
        {
            foreach (var entry in parameter)
            {
                navParam.Add(entry.Key, entry.Value);
            }
        }

        var navigationResult = await NavigationService.GoBackAsync(navParam);
        // TODO https://github.com/dansiegel/Prism.Plugin.Popups/issues/129
        if (navigationResult.Success)
        {
            SetResult(result);
        }
        else
        {
            Contract.NotNull(TaskCompletion);
            TaskCompletion!.SetException(navigationResult.Exception ?? new InvalidOperationException("Exception occurred during navigation."));
        }
    }

    protected void SetResult(TReturnType? result)
    {
        Contract.NotNull(TaskCompletion);
        TaskCompletion!.TrySetResult(result);
    }

    public virtual bool DismissOnBackgroundClick()
    {
        return false;
    }

    public virtual bool DismissOnBackButtonPress()
    {
        return false;
    }

    #region Properties

    [DoNotNotify]
    public TaskCompletionSource<TReturnType?>? TaskCompletion { get; private set; }

    #endregion
}
