using ReactiveUI;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Core.ViewModels.Items;

public abstract class BaseToDoListItemViewModel : ReactiveObject
{
    public abstract ToDoListItemType ItemType { get; }
}
