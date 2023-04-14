using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using ToDoList.Core.ViewModels.Base;

namespace ToDoList.Droid.Fragments;

public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel>
    where TViewModel : BaseViewModel
{
    protected abstract int ResourceId { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);
        var view = this.BindingInflate(ResourceId, container, false);

        var toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);

        var set = this.CreateBindingSet<BaseFragment<TViewModel>, TViewModel>();

        /*set
            .Bind(toolbar)
            .For(v => v.BindClick())
            .To(vm => vm.SelectedSegment);*/

        set.Apply();

        return view;
    }

    public override void OnStart()
    {
        base.OnStart();

        if (Activity?.ActionBar is not { } actionBar)
        {
            return;
        }
    }
}
