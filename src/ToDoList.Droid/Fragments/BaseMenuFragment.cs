using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.AppBar;
using ToDoList.Core.ViewModels.Base;
using ToDoList.Core.ViewModels.Interfaces;

namespace ToDoList.Droid.Fragments;

public abstract class BaseMenuFragment<TViewModel> : BaseFragment<TViewModel>
    where TViewModel : BaseViewModel, IBaseToolbarViewModel
{
    private MaterialToolbar _toolbar;

    protected abstract int MenuResourceId { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        (_toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar)).MenuItemClick += OnMenuItemClick;

        _toolbar.InflateMenu(MenuResourceId);

        return view;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing
            && _toolbar != null)
        {
            _toolbar.MenuItemClick -= OnMenuItemClick;
        }
        base.Dispose(disposing);
    }

    private void OnMenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e) => ViewModel.ToolbarCommand?.Execute(null);
}
