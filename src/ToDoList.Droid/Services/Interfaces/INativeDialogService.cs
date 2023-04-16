using System;
using Android.Views;

namespace ToDoList.Droid.Services.Interfaces;

public interface INativeDialogService
{
    void ShowSnackbar(string labelText, View view, string actionText = null, Action action = null, Action onClose = null);
}
