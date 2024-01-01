using MauiUtility.ViewModels;
using Mopups.Pages;

namespace MauiUtility.Pages.Modals;

public class BaseModal : PopupPage
{
    protected override bool OnBackButtonPressed()
    {
        var result = base.OnBackButtonPressed();
        if (BindingContext is IDismissableModal viewModel)
            return viewModel.DismissOnBackButtonPress();
        return result;
    }

    protected override bool OnBackgroundClicked()
    {
        var result = base.OnBackgroundClicked();
        if (BindingContext is IDismissableModal viewModel)
            return viewModel.DismissOnBackgroundClick();
        return result;
    }
}
