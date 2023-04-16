using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ToDoList.Core.Definitions.Attributes;
using ToDoList.Core.Definitions.Constants;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Repositories.Interfaces;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.ViewModels
{
    public sealed class EditTaskViewModel : BasePageTitledActionViewModel
    {
        private const string DELETE_TASK_MESSAGE = "Are you sure to delete this task?";

        private readonly Lazy<IToDoListRepository> _toDoListRepository;

        public IMvxAsyncCommand DeleteCommand { get; }

        public EditTaskViewModel(
            IMapper mapper,
            Lazy<IToDoListRepository> toDoListRepository,
            ILogger<EditTaskViewModel> logger)
            : base(mapper, logger)
        {
            _toDoListRepository = toDoListRepository;

            DeleteCommand = new MvxAsyncCommand(() =>
                RunSafeTaskAsync(async () =>
                {
                    if (!await DialogService.DisplayAlertAsync(null, DELETE_TASK_MESSAGE, MessageConstants.YES_MESSAGE, MessageConstants.NO_MESSAGE))
                    {
                        return;
                    }

                    await NavigationService.Close(this, CurrentToDoList);
                }));

             this.WhenAnyValue(vm => vm.Title,
                     vm => vm.Description,
                     vm => vm.Status)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => OnPropertyChanged(nameof(IsValid)))
                .DisposeWith(CompositeDisposable);
        }

        [Reactive]
        [Observe]
        public ToDoTaskStatus Status { get; set; }

        public int SelectedSegment
        {
            get => (int)Status;
            set => Status = (ToDoTaskStatus)value;
        }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }

        public bool UpdatedAtVisible => UpdatedAt != null;

        public override string PageTitle => "Edit task";

        protected override void OnAfterMap(ToDoListItemDalModel toDoListItemDalModel) => toDoListItemDalModel.UpdatedAt = DateTime.Now;

        protected override ToDoListItemViewModel OnResultSet(ToDoListItemViewModel item)
        {
            _toDoListRepository.Value.Update(CurrentToDoList.Item);

            // Return null cause nothing to do on ToDoListViewModel
            return null;
        }
    }
}
