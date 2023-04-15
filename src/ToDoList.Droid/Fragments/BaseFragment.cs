using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.Graphics.Drawable;
using Google.Android.Material.AppBar;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Droid.Extensions;

namespace ToDoList.Droid.Fragments;

public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel>
    where TViewModel : BasePageTitledViewModel
{
    private MaterialToolbar _toolbar;
    protected abstract int ResourceId { get; }

    protected abstract bool HasBackButton { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);
        var view = this.BindingInflate(ResourceId, container, false);

        if (!HasBackButton)
        {
            return view;
        }

        (_toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar)).NavigationClick += OnNavigationClick;

        Activity?.Then(activity =>
        {
            var icon = activity.GetDrawableFromAttribute(Resource.Attribute.homeAsUpIndicator);
            DrawableCompat.SetTint(icon, Color.Black);
            _toolbar.NavigationIcon = icon;
        });

        return view;
    }

    private void OnNavigationClick(object sender, Toolbar.NavigationClickEventArgs e) => Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(ViewModel);

    protected override void Dispose(bool disposing)
    {
        if (disposing
            && _toolbar != null)
        {
            _toolbar.NavigationClick -= OnNavigationClick;
        }
        base.Dispose(disposing);
    }
}
