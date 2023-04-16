using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.Definitions.Interactions;
using ToDoList.Core.ViewModels;
using ToDoList.Core.ViewModels.Extra;
using ToDoList.Droid.Adapters;
using ToDoList.Droid.Listeners;
using ToDoList.Droid.Services.Interfaces;
using ToDoList.Droid.Widgets;

namespace ToDoList.Droid.Fragments;

[MvxFragmentPresentation(
    IsCacheableFragment = true,
    ActivityHostViewModelType = typeof(MainViewModel),
    FragmentContentId = Resource.Id.content_frame,
    AddToBackStack = true)]
public class ToDoListFragment : BaseFragment<ToDoListViewModel>
{
    private IMvxInteraction<SnackbarInteraction> _deleteInteraction;

    public IMvxInteraction<SnackbarInteraction> DeleteInteraction
    {
        get => _deleteInteraction;
        set
        {
            _deleteInteraction?.Then(deletionInteraction => deletionInteraction.Requested -= OnDeleteInteractionRequested);
            _deleteInteraction = value;
            _deleteInteraction.Requested += OnDeleteInteractionRequested;
        }
    }

    protected override int ResourceId => Resource.Layout.todo_list_layout;

    protected override bool HasBackButton => false;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
         View itemsView = null;
         View emptyView = null;

        View view = base.OnCreateView(inflater, container, savedInstanceState);

        StateContainer stateContainer = view.FindViewById<StateContainer>(Resource.Id.state_container);

        stateContainer.States = new Dictionary<State, Func<View>>
        {
            [State.Default] = CreateItemsView,
            [State.NoData] = CreateEmptyView,
            [State.None] = default,
        };

        var set = this.CreateBindingSet<ToDoListFragment, ToDoListViewModel>();

        set.Bind(this)
            .For(view => view.DeleteInteraction)
            .To(viewModel => viewModel.DeleteInteraction)
            .OneWay();

        set.Apply();

        return view;

        View CreateItemsView()
        {
            return itemsView ??= CreateInnerItemsView();

            View CreateInnerItemsView()
            {
                itemsView = LayoutInflater.Inflate(Resource.Layout.to_do_list_items_view, stateContainer, false);
                var mvxRecyclerView = itemsView.FindViewById<MvxRecyclerView>(Resource.Id.items_recycler);
                var adapter = new ToDoListItemRecyclerAdapter(BindingContext as IMvxAndroidBindingContext, () => mvxRecyclerView?.SmoothScrollToPosition(0));
                mvxRecyclerView.SetAdapter(adapter);

                var addButton = itemsView.FindViewById<FloatingActionButton>(Resource.Id.add_button);

                var scrollListener = new RecyclerPaginationListener(mvxRecyclerView.GetLayoutManager() as LinearLayoutManager, OnRecyclerScroll);
                mvxRecyclerView.AddOnScrollListener(scrollListener);

                var set = this.CreateBindingSet<ToDoListFragment, ToDoListViewModel>();

                set
                    .Bind(adapter)
                    .For(v => v.Items)
                    .To(vm => vm.Items);

                set
                    .Bind(adapter)
                    .For(v => v.IsLoadingMore)
                    .To(vm => vm.IsLoadingMore);

                set
                    .Bind(mvxRecyclerView)
                    .For(v => v.ItemsSource)
                    .To(vm => vm.Items);

                set
                    .Bind(mvxRecyclerView)
                    .For(v => v.ItemClick)
                    .To(vm => vm.EditTaskCommand);

                set
                    .Bind(mvxRecyclerView)
                    .For(v => v.ItemLongClick)
                    .To(vm => vm.EditTaskCommand);

                set.Bind(scrollListener)
                    .For(x => x.LoadMoreCommand)
                    .To(vm => vm.LoadMoreCommand);

                set.Bind(scrollListener)
                    .For(x => x.LoadingOffset)
                    .To(vm => vm.LoadingOffset);

                set
                    .Bind(scrollListener)
                    .For(v => v.IsLoadingMore)
                    .To(vm => vm.IsLoadingMore);

                set
                    .Bind(addButton)
                    .For(v => v.BindClick())
                    .To(vm => vm.NewTaskCommand);

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

    private void OnDeleteInteractionRequested(object sender, MvxValueEventArgs<SnackbarInteraction> e) => Mvx.IoCProvider.Resolve<INativeDialogService>().ShowSnackbar(e.Value.LabelText, View, e.Value.ActionText, () => e.Value.Action(true), () => e.Value.Action(false));
}
