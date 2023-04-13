using Microsoft.Extensions.Logging;
using ToDoList.Core.ViewModels.Base;

namespace ToDoList.Core.ViewModels.Extra
{
    public sealed class MainViewModel : BaseViewModel
    {
        public MainViewModel(ILogger<MainViewModel> logger)
            : base(logger)
        {
        }

        public override string PageTitle => null;
    }
}
