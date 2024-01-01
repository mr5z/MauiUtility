namespace MauiUtility.ViewModels;

public interface IDismissableModal
{
    bool DismissOnBackgroundClick();
    bool DismissOnBackButtonPress();
}
