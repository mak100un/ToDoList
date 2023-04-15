using MvvmCross.Commands;
using ReactiveUI.Fody.Helpers;
using ToDoList.Core.Definitions.Attributes;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Core.ViewModels.Items;

public class ToDoListItemViewModel : BaseToDoListItemViewModel
{
    // TODO If we have Item, do we need to have properties?
    [Reactive]
    [Observe]
    public string Title { get; set; }

    [Reactive]
    [Observe]
    public string Description { get; set; }

    [Reactive]
    [Observe]
    public ToDoTaskStatus Status { get; set; }

    public ToDoListItemDalModel Item { get; set; } = new ();

    public override ToDoListItemType ItemType => ToDoListItemType.Task;
}
