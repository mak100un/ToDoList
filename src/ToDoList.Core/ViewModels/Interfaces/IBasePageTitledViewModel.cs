using MvvmCross.ViewModels;

namespace ToDoList.Core.ViewModels.Interfaces;

public interface IBasePageTitledViewModel : IMvxViewModel
{
    public string PageTitle { get; }
}
