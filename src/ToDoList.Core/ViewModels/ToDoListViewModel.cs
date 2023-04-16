using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DynamicData.Binding;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using ReactiveUI.Fody.Helpers;
using Swordfish.NET.Collections.Auxiliary;
using ToDoList.Core.Definitions.Constants;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.Definitions.Interactions;
using ToDoList.Core.Repositories.Interfaces;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.ViewModels
{
    public class ToDoListViewModel : BasePageTitledViewModel
    {
        private const int PAGE_SIZE = 10;
        private const string REVERT_TASK_DELETION = "Do you want to revert task?";

        private readonly Lazy<IToDoListRepository> _toDoListRepository;
        private readonly IMapper _mapper;

        private readonly List<ToDoListItemViewModel> _deletionWaitList = new();

        public IMvxAsyncCommand<ToDoListItemViewModel> EditTaskCommand { get; }

        public IMvxAsyncCommand NewTaskCommand { get; }

        public IMvxAsyncCommand LoadMoreCommand { get; }

        public ToDoListViewModel(
            IMapper mapper,
            Lazy<IToDoListRepository> toDoListRepository,
            ILogger<ToDoListViewModel> logger)
            : base(logger)
        {
            _mapper = mapper;
            _toDoListRepository = toDoListRepository;

            EditTaskCommand = new MvxAsyncCommand<ToDoListItemViewModel>(toDoListItem => RunSafeTaskAsync(async () =>
            {
                if (await NavigationService.Navigate<EditTaskViewModel, ToDoListItemViewModel, ToDoListItemViewModel>(toDoListItem) is not { } deleteTaskResult)
                {
                    return;
                }

                _deletionWaitList.Add(deleteTaskResult);
                Items.TryRemove(deleteTaskResult);

                var request = new SnackbarInteraction
                {
                    Action = revert =>
                    {
                        _deletionWaitList.TryRemove(deleteTaskResult);
                        switch (revert)
                        {
                            case true:
                                var previousItem = Items.OrderByDescending(item => item.Item.CreatedAt)?.FirstOrDefault(item => item.Item.CreatedAt < deleteTaskResult.Item.CreatedAt);
                                if (previousItem == null)
                                {
                                    Items.Add(deleteTaskResult);
                                }
                                else
                                {
                                    Items.Insert(Items.IndexOf(previousItem), deleteTaskResult);
                                }
                                break;
                            default:
                                toDoListRepository.Value.Delete(toDoListItem.Item.Id);
                                break;
                        }
                    },
                    LabelText = REVERT_TASK_DELETION,
                    ActionText = MessageConstants.YES_MESSAGE,
                };

                DeleteInteraction.Raise(request);
            }), toDoListItem => Items.Contains(toDoListItem));

            NewTaskCommand = new MvxAsyncCommand(() => RunSafeTaskAsync(async () =>
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
                var newItems = _toDoListRepository.Value.GetItems(Items.Count + _deletionWaitList.Count, PAGE_SIZE)?.ToArray();

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
            }),() => IsLoadMoreEnabled && !IsLoadingMore);

            Items
                .ObserveCollectionChanges()
                .Subscribe(_ =>
                {
                    State = Items.Count > 0 ? State.Default : State.NoData;
                    LoadingOffset = Items.Count - 3;
                });
        }

        public MvxInteraction<SnackbarInteraction> DeleteInteraction { get; } = new ();

        [Reactive]
        public bool IsLoadMoreEnabled { get; private set; }

        [Reactive]
        public bool IsLoadingMore { get; private set; }

        [Reactive]
        public State State { get; private set; }

        [Reactive]
        public int LoadingOffset { get; private set; }

        public ObservableCollection<ToDoListItemViewModel> Items { get; } = new();

        public override async Task Initialize()
        {
            await base.Initialize();

            await RunSafeTaskAsync(async () =>
            {
                var newItems = _toDoListRepository.Value.GetItems(Items.Count + _deletionWaitList.Count, PAGE_SIZE)?.ToArray();

                var anyItems = newItems?.Length > 0;
                if (anyItems)
                {

                    Items.AddRange(_mapper.Map<IEnumerable<ToDoListItemViewModel>>(newItems));
                }

                IsLoadMoreEnabled = newItems?.Length >= PAGE_SIZE;
                State = anyItems ? State.Default : State.NoData;
            });
        }

        public override string PageTitle => "ToDo list";
    }
}
