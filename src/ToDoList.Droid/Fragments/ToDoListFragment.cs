using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.Adapters;
using ToDoList.Droid.Listeners;
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

    protected override bool HasBackButton => false;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        MvxRecyclerView mvxRecyclerView = null;
        View emptyView = null;

        var view = base.OnCreateView(inflater, container, savedInstanceState);

        var stateContainer = view.FindViewById<StateContainer>(Resource.Id.state_container);

        stateContainer.States = new Dictionary<State, Func<View>>
        {
            [State.Default] = CreateRecyclerView,
            [State.NoData] = CreateEmptyView,
            [State.None] = default,
        };

        return view;

        View CreateRecyclerView()
        {
            return mvxRecyclerView ??= CreateInnerRecyclerView();

            MvxRecyclerView CreateInnerRecyclerView()
            {
                mvxRecyclerView = new MvxRecyclerView(Activity, null);
                var layoutManager = mvxRecyclerView.GetLayoutManager() as LinearLayoutManager;
                layoutManager.StackFromEnd = true;
                layoutManager.ReverseLayout = false;
                //mvxRecyclerView.SetAdapter(new ToDoListItemRecyclerAdapter());

                var scrollListener = new RecyclerPaginationListener(layoutManager);
                mvxRecyclerView.AddOnScrollListener(scrollListener);
                mvxRecyclerView.ItemTemplateId = Resource.Layout.item_template;
                mvxRecyclerView.ItemTemplateSelector = new ToDoListItemTemplateSelector();

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

                return mvxRecyclerView;
            }
        }

        View CreateEmptyView() => emptyView ??= this.BindingInflate(Resource.Layout.todo_list_empty_view, stateContainer, false);
    }
}
