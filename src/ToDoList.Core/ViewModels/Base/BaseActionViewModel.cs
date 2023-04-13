using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reactive.Disposables;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using ReactiveUI.Fody.Helpers;
using ToDoList.Core.Definitions.Attributes;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.ViewModels.Items;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ToDoList.Core.ViewModels.Base;

public abstract class BaseActionViewModel<TResult> : BaseResultViewModel<ToDoListItemViewModel, TResult>, IBaseActionViewModel
    where TResult : class
{
    private readonly IMapper _mapper;
    public IMvxAsyncCommand ActionCommand { get; }

    public CompositeDisposable CompositeDisposable { get; } = new ();

    public BaseActionViewModel(
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
    [Required]
    [Observe]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "\"Title\" length must be between {2}-{1} characters")]
    [RegularExpression("[a-zA-Z]+")]
    public string Title { get; set; }

    [Reactive]
    [Observe]
    [StringLength(1000, ErrorMessage = "\"Title\" length must be less than {1} characters")]
    public string Description { get; set; }

    [AnyObserveChanged(ErrorMessage = "Any values should be changed.")]
    public ToDoListItemViewModel CurrentToDoList { get; set; }

    public bool IsValid => Validate(this);

    public override void Prepare(ToDoListItemViewModel parameter) => _mapper.Map(parameter, this);

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (viewFinishing)
        {
            CompositeDisposable.Clear();
        }
        base.ViewDestroy(viewFinishing);
    }

    protected abstract void OnAfterMap(ToDoListItemDalModel toDoListItemDalModel);

    protected abstract TResult OnResultSet(ToDoListItemViewModel item);

    private static bool Validate(object @object)
    {
        var validationResult = new List<ValidationResult>();
        return Validator.TryValidateObject(
            @object, new ValidationContext(@object, null, null), validationResult, validateAllProperties: true);
    }
}
