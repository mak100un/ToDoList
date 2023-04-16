using System;

namespace ToDoList.Core.Definitions.Interactions;

public class SnackbarInteraction
{
    public string LabelText { get; set; }

    public string ActionText { get; set; }

    public Action<bool> Action  { get; set; }
}
