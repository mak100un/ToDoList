using System;

namespace ToDoList.iOS.Services.Interfaces;

public interface INativeDialogService
{
    void ShowSnackbar(string labelText, string actionText = null, Action action = null, Action onClose = null);
}
