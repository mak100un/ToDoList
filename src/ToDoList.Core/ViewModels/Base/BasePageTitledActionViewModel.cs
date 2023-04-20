using System;
using System.ComponentModel.DataAnnotations;
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
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Core.ViewModels.Base;

public abstract class BasePageTitledActionViewModel : BasePageTitledResultViewModel<ToDoListItemViewModel, ToDoListItemViewModel>, IBaseActionViewModel
{
    private readonly IMapper _mapper;
    public IMvxAsyncCommand ActionCommand { get; }

    public CompositeDisposable CompositeDisposable { get; } = new ();

    public BasePageTitledActionViewModel(
        IMapper mapper,
        ILogger logger)
        : base(logger)
    {
        _mapper = mapper;

        ActionCommand = new MvxAsyncCommand(() =>
                RunSafeTaskAsync(() => NavigationService.Close(this, OnResultSet(_mapper.Map(this, CurrentToDoList,
                    o => o.AfterMap((_, d) => OnAfterMap(d.Item)))))),
            () => IsValid);
    }

    [Reactive]
    [Observe]
    [Required(ErrorMessage = "\"Title\" should not be empty")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "\"Title\" length must be between {2}-{1} characters")]
    public string Title { get; set; }

    [Reactive]
    public string TitleError { get; set; }

    [Reactive]
    [Observe]
    [StringLength(1000, ErrorMessage = "\"Title\" length must be less than {1} characters")]
    public string Description { get; set; }

    [Reactive]
    public string DescriptionError { get; set; }

    [AnyObserveChanged(ErrorMessage = "Any values should be changed.")]
    public ToDoListItemViewModel CurrentToDoList { get; set; }

    public bool IsValid => this.Validate();

    public override void Prepare(ToDoListItemViewModel parameter)
    {
        _mapper.Map(parameter, this);

        this.WhenAnyValue(vm => vm.Title)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Skip(1)
            .Do(_ => TitleError = null)
            .Throttle(TimeSpan.FromMilliseconds(350), RxApp.MainThreadScheduler)
            .Select(_ => TitleError = this.Validate(nameof(Title)))
            .WhereNotNull()
            .Throttle(TimeSpan.FromMilliseconds(TimeConstants.WaitOnErrorDisappearing), RxApp.MainThreadScheduler)
            .Subscribe(_ => TitleError = null)
            .DisposeWith(CompositeDisposable);

        this.WhenAnyValue(vm => vm.Description)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Skip(1)
            .Do(_ => DescriptionError = null)
            .Throttle(TimeSpan.FromMilliseconds(350), RxApp.MainThreadScheduler)
            .Select(_ => DescriptionError = this.Validate(nameof(Description)))
            .WhereNotNull()
            .Throttle(TimeSpan.FromMilliseconds(TimeConstants.WaitOnErrorDisappearing), RxApp.MainThreadScheduler)
            .Subscribe(_ => DescriptionError = null)
            .DisposeWith(CompositeDisposable);
    }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (viewFinishing)
        {
            CompositeDisposable.Clear();
        }
        base.ViewDestroy(viewFinishing);
    }

    protected abstract void OnAfterMap(ToDoListItemDalModel toDoListItemDalModel);

    protected virtual ToDoListItemViewModel OnResultSet(ToDoListItemViewModel item) => item;
}
