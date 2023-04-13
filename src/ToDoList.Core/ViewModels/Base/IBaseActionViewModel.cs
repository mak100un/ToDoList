using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.ViewModels.Base;

public interface IBaseActionViewModel
{
    string Title { get; set; }

    string Description { get; set; }

    ToDoListItemViewModel CurrentToDoList { get; set; }
}
