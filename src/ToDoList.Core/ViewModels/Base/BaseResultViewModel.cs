using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.ViewModels;

namespace ToDoList.Core.ViewModels.Base;

public abstract class BaseResultViewModel<TParameter, TResult> : BaseViewModelResult<TResult>, IMvxViewModel<TParameter, TResult>
    where TParameter : notnull
    where TResult : notnull
{
    protected BaseResultViewModel(ILogger logger)
        : base(logger)
    {
    }

    public abstract void Prepare(TParameter parameter);
}

public abstract class BaseViewModelResult<TResult> : BaseViewModel, IMvxViewModelResult<TResult>
    where TResult : notnull
{
    protected BaseViewModelResult(ILogger logger)
        : base(logger)
    {
    }

    public TaskCompletionSource<object> CloseCompletionSource { get; set; }

    public override void ViewDestroy(bool viewFinishing = true)
    {
        if (viewFinishing && CloseCompletionSource != null &&
            !CloseCompletionSource.Task.IsCompleted &&
            !CloseCompletionSource.Task.IsFaulted)
        {
            CloseCompletionSource.TrySetCanceled();
        }

        base.ViewDestroy(viewFinishing);
    }
}
