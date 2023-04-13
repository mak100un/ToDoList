using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.Definitions.Models;

public class EditTaskResult
{
    public EditTaskResult(ToDoListItemViewModel task, EditTaskType editType)
    {
        Task = task;
        EditType = editType;
    }

    public EditTaskType EditType { get; }

    public ToDoListItemViewModel Task { get; }


}
