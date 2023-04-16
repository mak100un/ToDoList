using System;
using Android.Views;
using Google.Android.Material.Snackbar;
using MvvmCross.Platforms.Android;
using ToDoList.Droid.Listeners;
using ToDoList.Droid.Services.Interfaces;

namespace ToDoList.Droid.Services;

public class NativeDialogService : INativeDialogService
{
    private readonly IMvxAndroidCurrentTopActivity _topActivity;

    public NativeDialogService(IMvxAndroidCurrentTopActivity topActivity) => _topActivity = topActivity;

    public void ShowSnackbar(string labelText, View view, string actionText = null, Action action = null, Action onClose = null)
    {
        var snackbar = Snackbar.Make(_topActivity.Activity, view, labelText, BaseTransientBottomBar.LengthLong);

        if (action != null)
        {
            snackbar.SetAction(actionText, _ => action());
        }

        if (onClose != null)
        {
            snackbar.AddCallback(new BaseSnackbarCallback(e =>
            {
                if (e == Snackbar.Callback.DismissEventAction)
                {
                    return;
                }

                onClose();
            }));
        }

        snackbar.Show();
    }
}
