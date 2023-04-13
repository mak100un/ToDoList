using MvvmCross.ViewModels;

namespace ToDoList.Core.ViewModels.Interfaces;

public interface IBaseViewModel : IMvxViewModel
{
    public string PageTitle { get; }
}
