using System.Collections.Generic;
using Android.OS;
using Android.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.TemplateSelectors;
using ToDoList.Droid.Widgets;

namespace ToDoList.Droid.Fragments;

[MvxFragmentPresentation(
    IsCacheableFragment = true,
    ActivityHostViewModelType = typeof(MainViewModel),
    FragmentContentId = Resource.Id.content_frame,
    AddToBackStack = false)]
public class ToDoListFragment : BaseFragment<ToDoListViewModel>
{
    protected override int ResourceId => Resource.Layout.todo_list_layout;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var view = base.OnCreateView(inflater, container, savedInstanceState);

        var stateContainer = view.FindViewById<StateContainer>(Resource.Id.state_container);

        var emptyView = this.BindingInflate(Resource.Layout.todo_list_empty_view, stateContainer, false);

        var mvxRecyclerView = new MvxRecyclerView(Activity, null);
        mvxRecyclerView.ItemTemplateId = Resource.Layout.item_template;
        mvxRecyclerView.ItemTemplateSelector = new ToDoListItemTemplateSelector();

        stateContainer.States = new Dictionary<State, View>
        {
            [State.Default] = mvxRecyclerView,
            [State.NoData] = emptyView,
            [State.None] = default,
        };

        var set = this.CreateBindingSet<ToDoListFragment, ToDoListViewModel>();

        set
            .Bind(mvxRecyclerView)
            .For(v => v.ItemsSource)
            .To(vm => vm.Items);

        set
            .Bind(mvxRecyclerView)
            .For(v => v.ItemClick)
            .To(vm => vm.EditTaskCommand);

        set.Apply();

        return view;
    }
}
