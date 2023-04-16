using System;
using Google.Android.Material.Snackbar;
using Object = Java.Lang.Object;

namespace ToDoList.Droid.Listeners;

public class BaseSnackbarCallback : BaseTransientBottomBar.BaseCallback
{
    private readonly Action<int> _onDismissed;
    private readonly Action _onShown;

    public BaseSnackbarCallback(Action<int> onDismissed = null, Action onShown = null)
    {
        _onDismissed = onDismissed;
        _onShown = onShown;
    }

    public override void OnDismissed(Object transientBottomBar, int e)
    {
        base.OnDismissed(transientBottomBar, e);
        _onDismissed?.Invoke(e);
    }

    public override void OnShown(Object transientBottomBar)
    {
        base.OnShown(transientBottomBar);
        _onShown?.Invoke();
    }
}
