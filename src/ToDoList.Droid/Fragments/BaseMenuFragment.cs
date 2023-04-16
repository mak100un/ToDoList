using System.Windows.Input;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using Google.Android.Material.AppBar;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.ViewModels.Base;

namespace ToDoList.Droid.Fragments;

public abstract class BaseMenuFragment<TViewModel> : BaseFragment<TViewModel>
    where TViewModel : BasePageTitledViewModel
{
    private MaterialToolbar _toolbar;

    protected abstract int MenuResourceId { get; }
    protected abstract ICommand MenuCommand { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        OnMenuItemClickUnsubscribe();
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        (_toolbar = view.FindViewById<MaterialToolbar>(Resource.Id.toolbar)).MenuItemClick += OnMenuItemClick;

        _toolbar.InflateMenu(MenuResourceId);

        return view;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            OnMenuItemClickUnsubscribe();
        }
        base.Dispose(disposing);
    }

    private void OnMenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e) => MenuCommand?.Execute(null);

    private void OnMenuItemClickUnsubscribe() => _toolbar?.Then(toolbar => toolbar.MenuItemClick -= OnMenuItemClick);
}
