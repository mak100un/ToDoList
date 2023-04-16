using System;
using UIKit;

namespace ToDoList.iOS.Services;

public static class KeyboardObserver
{
    public static event EventHandler<UIKeyboardEventArgs> KeyboardWillHide;
    public static event EventHandler<UIKeyboardEventArgs> KeyboardWillShow;

    static KeyboardObserver()
    {
        UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShown);
        UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHidden);
    }

    private static void OnKeyboardHidden(object sender, UIKeyboardEventArgs args) => KeyboardWillHide?.Invoke(sender, args);

    private static void OnKeyboardShown(object sender, UIKeyboardEventArgs args) => KeyboardWillShow?.Invoke(sender, args);
}
