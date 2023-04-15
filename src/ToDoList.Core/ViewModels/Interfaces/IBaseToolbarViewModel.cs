using System.Windows.Input;
using MvvmCross.Commands;

namespace ToDoList.Core.ViewModels.Interfaces;

// TODO What if we want another toolbar command ?
public interface IBaseToolbarViewModel : IBaseViewModel
{
    IMvxAsyncCommand ToolbarCommand { get; }

    bool ToolbarItemVisible { get; }
}
