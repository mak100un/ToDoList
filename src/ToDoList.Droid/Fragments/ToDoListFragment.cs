using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.Adapters;
using ToDoList.Droid.Listeners;
using ToDoList.Droid.Widgets;

namespace ToDoList.Droid.Fragments;

[MvxFragmentPresentation(
    IsCacheableFragment = true,
    ActivityHostViewModelType = typeof(MainViewModel),
    FragmentContentId = Resource.Id.content_frame,
    AddToBackStack = true,
    PopBackStackImmediateFlag = MvxPopBackStack.Inclusive)]
public class ToDoListFragment : BaseFragment<ToDoListViewModel>
{
    protected override int ResourceId => Resource.Layout.todo_list_layout;

    protected override bool HasBackButton => false;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View itemsView = null;
        View emptyView = null;

        var view = base.OnCreateView(inflater, container, savedInstanceState);

        var stateContainer = view.FindViewById<StateContainer>(Resource.Id.state_container);

        stateContainer.States = new Dictionary<State, Func<View>>
        {
            [State.Default] = CreateItemsView,
            [State.NoData] = CreateEmptyView,
            [State.None] = default,
        };

        return view;

        View CreateItemsView()
        {
            return itemsView ??= CreateInnerItemsView();

            View CreateInnerItemsView()
            {
                itemsView = LayoutInflater.Inflate(Resource.Layout.to_do_list_items_view, stateContainer, false);
                var mvxRecyclerView = itemsView.FindViewById<MvxRecyclerView>(Resource.Id.items_recycler);
                mvxRecyclerView.SetAdapter(new ToDoListItemRecyclerAdapter(BindingContext as IMvxAndroidBindingContext));

                var addButton = itemsView.FindViewById<FloatingActionButton>(Resource.Id.add_button);

                var scrollListener = new RecyclerPaginationListener(mvxRecyclerView.GetLayoutManager() as LinearLayoutManager, OnRecyclerScroll);
                mvxRecyclerView.AddOnScrollListener(scrollListener);

                var set = this.CreateBindingSet<ToDoListFragment, ToDoListViewModel>();

                set
                    .Bind(mvxRecyclerView)
                    .For(v => v.ItemsSource)
                    .To(vm => vm.Items);

                set
                    .Bind(mvxRecyclerView)
                    .For(v => v.ItemClick)
                    .To(vm => vm.EditTaskCommand);

                set.Bind(scrollListener)
                    .For(x => x.LoadMoreCommand)
                    .To(vm => vm.LoadMoreCommand);

                set.Bind(scrollListener)
                    .For(x => x.LoadingOffset)
                    .To(vm => vm.LoadingOffset);

                set
                    .Bind(addButton)
                    .For(v => v.BindClick())
                    .To(vm => vm.ToolbarCommand);

                set.Apply();

                return itemsView;

                void OnRecyclerScroll(float dy)
                {
                    switch (dy)
                    {
                        case > 0
                        when addButton.IsShown:
                            addButton.Hide();
                            break;
                        case < 0
                             when !addButton.IsShown:
                            addButton.Show();
                            break;
                    }
                }
            }
        }

        View CreateEmptyView() => emptyView ??= this.BindingInflate(Resource.Layout.todo_list_empty_view, stateContainer, false);
    }
}
