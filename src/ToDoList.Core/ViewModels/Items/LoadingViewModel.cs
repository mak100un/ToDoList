using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Core.ViewModels.Items;

public class LoadingViewModel : BaseToDoListItemViewModel
{
    public override ToDoListItemType ItemType => ToDoListItemType.Loading;
}
