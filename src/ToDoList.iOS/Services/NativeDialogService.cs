using System;
using MaterialComponents;
using ToDoList.iOS.Services.Interfaces;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Services;

public class NativeDialogService : INativeDialogService
{
    public void ShowSnackbar(string labelText, string actionText = null, Action action = null, Action onClose = null)
    {
        var message = new SnackbarMessage();
        message.Text = labelText;
        message.CompletionHandler = new (result =>
        {
            if (result)
            {
                return;
            }
            onClose?.Invoke();
        });

        if (action != null)
        {
            var messageAction = new SnackbarMessageAction();
            var actionHandler = new SnackbarMessageActionHandler(() => action());
            messageAction.Handler = actionHandler;
            messageAction.Title = actionText;
            message.ButtonTextColor = ColorPalette.SnackbarButtonColor;
            message.Action = messageAction;
        }

        SnackbarManager.DefaultManager.ShowMessage(message);
    }
}
