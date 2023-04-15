using MvvmCross.ViewModels;

namespace ToDoList.Core.ViewModels.Interfaces;

public interface IBaseViewModel : IMvxViewModel
{
    // TODO What if page don't have title ?
    public string PageTitle { get; }
}
