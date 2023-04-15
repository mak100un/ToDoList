using System.Windows.Input;
using MvvmCross.Commands;

namespace ToDoList.Core.ViewModels.Interfaces;

public interface IBaseToolbarViewModel : IBaseViewModel
{
    IMvxAsyncCommand ToolbarCommand { get; }

    bool ToolbarItemVisible { get; }
}
