using PropertyChanged;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace MauiUtility.ViewModels;

public class BaseViewModel(INavigationService navigationService) : BindableBase, INavigationAware, IInitialize, IInitializeAsync
{
    [DoNotNotify]
    protected INavigationService NavigationService { get; } = navigationService;

    protected virtual void OnPageLoaded(INavigationParameters parameters)
    {

    }

    public virtual void Initialize(INavigationParameters parameters)
    {

    }

    public virtual Task InitializeAsync(INavigationParameters parameters)
    {
        return Task.CompletedTask;
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {

    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        var mode = parameters.GetNavigationMode();

        if (mode == NavigationMode.New)
            OnPageLoaded(parameters);
    }
}
