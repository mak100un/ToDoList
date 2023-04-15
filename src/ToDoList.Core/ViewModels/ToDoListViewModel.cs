using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using MvvmCross;
using MvvmCross.Commands;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Swordfish.NET.Collections;
using Swordfish.NET.Collections.Auxiliary;
using ToDoList.Core.Definitions;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.Definitions.Models;
using ToDoList.Core.Repositories.Interfaces;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.ViewModels
{
    public class ToDoListViewModel : BaseViewModel, IBaseToolbarViewModel
    {
        private const int PAGE_SIZE = 10;

        private readonly Lazy<IToDoListRepository> _toDoListRepository;
        private readonly IMapper _mapper;

        private bool _isLoadingMore;

        public IMvxAsyncCommand<ToDoListItemViewModel> EditTaskCommand { get; }

        public IMvxAsyncCommand ToolbarCommand { get; }

        public IMvxAsyncCommand LoadMoreCommand { get; }

        public ToDoListViewModel(
            IMapper mapper,
            ILogger<ToDoListViewModel> logger)
            : base(logger)
        {
            _mapper = mapper;

            // TODO DI ?
            _toDoListRepository = new Lazy<IToDoListRepository>(Mvx.IoCProvider.Resolve<IToDoListRepository>);

            EditTaskCommand = new MvxAsyncCommand<ToDoListItemViewModel>(toDoListItem => RunSafeTaskAsync(async () =>
            {
                if (await NavigationService.Navigate<EditTaskViewModel, ToDoListItemViewModel, EditTaskResult>(toDoListItem) is not { } editTaskResult)
                {
                    return;
                }

                switch (editTaskResult.EditType)
                {
                    case EditTaskType.Update:
                        _toDoListRepository.Value.Update(editTaskResult.Task.Item);
                        break;
                    case EditTaskType.Delete:
                        _toDoListRepository.Value.Delete(editTaskResult.Task.Item.Id);
                        Items.Remove(editTaskResult.Task);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }), toDoListItem => toDoListItem.ItemType == ToDoListItemType.Task
                                && Items.Contains(toDoListItem));

            ToolbarCommand = new MvxAsyncCommand(() => RunSafeTaskAsync(async () =>
            {
                if (await NavigationService.Navigate<NewTaskViewModel, ToDoListItemViewModel, ToDoListItemViewModel>(new ToDoListItemViewModel()) is not { } createdItem)
                {
                    return;
                }

                _toDoListRepository.Value.Add(createdItem.Item);
                Items.Insert(0, createdItem);
            }));

            LoadMoreCommand = new MvxAsyncCommand(() => RunSafeTaskAsync(async () =>
                {
                    IsLoadingMore = true;

                    // Simulate loading
                    await Task.Delay(2500);
                    var newItems = _toDoListRepository.Value.GetItems(ToDoListItemsCount, PAGE_SIZE)?.ToArray();

                    if (newItems?.Length > 0)
                    {
                        Items.AddRange(_mapper.Map<IEnumerable<ToDoListItemViewModel>>(newItems));
                    }

                    IsLoadMoreEnabled = newItems?.Length >= PAGE_SIZE;
                    IsLoadingMore = false;
                },
                ex =>
                {
                    IsLoadMoreEnabled = false;
                    IsLoadingMore = false;
                }), () => IsLoadMoreEnabled && !IsLoadingMore);

            Items
                .ObserveCollectionChanges()
                .Subscribe(_ =>
                {
                    LoadingOffset = Items.Count - (_isLoadingMore ? 3 : 2);
                    State = Items.Count > 0 ? State.Default : State.NoData;
                    OnPropertyChanged(nameof(ToolbarItemVisible));
                });
        }

        [Reactive]
        public bool IsLoadMoreEnabled { get; set; }

        public bool ToolbarItemVisible => State == State.Default;

        public bool IsLoadingMore
        {
            get => _isLoadingMore;
            set
            {
                if (_isLoadingMore == value)
                {
                    return;
                }

                switch (value)
                {
                    case true:
                        // TODO How to keep only task items in Items collection, but keep loader at the bottom?
                        Items.Add(new LoadingViewModel());
                        break;
                    default:
                        Items.TryRemove(Items.FirstOrDefault(item => item.ItemType == ToDoListItemType.Loading));
                        break;
                }

                _isLoadingMore = value;

                this.RaisePropertyChanged();
            }
        }

        [Reactive]
        public int LoadingOffset { get; private set; }

        [Reactive]
        public State State { get; private set; }

        private int ToDoListItemsCount => _isLoadingMore ? Items.Count - 1 : Items.Count;

        // TODO Why concurrent?
        public ConcurrentObservableCollection<BaseToDoListItemViewModel> Items { get; } = new ();

        public override async Task Initialize()
        {
            await base.Initialize();

            // TODO What if I don't want to wait for Initialize() ?
            await RunSafeTaskAsync(async () =>
            {
                var newItems = _toDoListRepository.Value.GetItems(ToDoListItemsCount, PAGE_SIZE)?.ToArray();

                var anyItems = newItems?.Length > 0;

                if (anyItems)
                {
                    Items.AddRange(_mapper.Map<IEnumerable<ToDoListItemViewModel>>(newItems));
                }

                IsLoadMoreEnabled = newItems?.Length >= PAGE_SIZE;
                State = anyItems ? State.Default : State.NoData;
                OnPropertyChanged(nameof(ToolbarItemVisible));
            });
        }

        public override string PageTitle => "ToDo list";
    }
}
