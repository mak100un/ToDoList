using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ToDoList.Core.Definitions.Attributes;
using ToDoList.Core.Definitions.Constants;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Models;
using ToDoList.Core.Services.Interfaces;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.ViewModels
{
    public sealed class EditTaskViewModel : BaseActionViewModel<EditTaskResult>, IBaseToolbarViewModel
    {
        private const string DELETE_TASK_MESSAGE = "Are you sure to delete this task?";

        private readonly Lazy<IDialogService> _dialogService;
        public IMvxAsyncCommand ToolbarCommand { get; }

        public EditTaskViewModel(
            IMapper mapper,
            ILogger<EditTaskViewModel> logger)
            : base(mapper, logger)
        {
            _dialogService = new Lazy<IDialogService>(Mvx.IoCProvider.Resolve<IDialogService>);

            ToolbarCommand = new MvxAsyncCommand(() =>
                    RunSafeTaskAsync(async () =>
                    {
                        if (!await _dialogService.Value.DisplayAlertAsync(null, DELETE_TASK_MESSAGE, MessageConstants.YES_MESSAGE, MessageConstants.NO_MESSAGE))
                        {
                            return;
                        }

                        await NavigationService.Close(this, new(CurrentToDoList, EditTaskType.Delete));
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

        public bool ToolbarItemVisible => true;

        public override string PageTitle => "Edit task";

        protected override void OnAfterMap(ToDoListItemDalModel toDoListItemDalModel) => toDoListItemDalModel.UpdatedAt = DateTime.Now;
        protected override EditTaskResult OnResultSet(ToDoListItemViewModel item) => new (item, EditTaskType.Update);
    }
}
