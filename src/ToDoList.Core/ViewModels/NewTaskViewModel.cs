using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.ViewModels.Base;

namespace ToDoList.Core.ViewModels
{
    public sealed class NewTaskViewModel : BasePageTitledActionViewModel
    {
        public NewTaskViewModel(
            IMapper mapper,
            ILogger<NewTaskViewModel> logger)
            : base(mapper, logger)
        {
            this.WhenAnyValue(vm => vm.Title,vm => vm.Description)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => OnPropertyChanged(nameof(IsValid)))
                .DisposeWith(CompositeDisposable);
        }

        public override string PageTitle => "New task";

        protected override void OnAfterMap(ToDoListItemDalModel toDoListItemDalModel) => toDoListItemDalModel.CreatedAt = DateTime.Now;
    }
}
