using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using DynamicData.Binding;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ReactiveUI;
using Swordfish.NET.Collections;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels.Items;
using ToDoList.Droid.ViewHolder;

namespace ToDoList.Droid.Adapters;

public class ToDoListItemRecyclerAdapter : MvxRecyclerAdapter, INotifyPropertyChanged
{
    private readonly Action _scrollToTop;
    private IDisposable _disposable;

    public ToDoListItemRecyclerAdapter(IMvxAndroidBindingContext bindingContext, Action scrollToTop)
        : base(bindingContext)
    {
        _scrollToTop = scrollToTop;
        this.WhenAnyValue(vm => vm.Items)
            .WhereNotNull()
            .Subscribe(_ =>
            {
                _disposable?.Dispose();
                _disposable = null;
                _disposable = Items
                    ?.ObserveCollectionChanges()
                    .Subscribe(args => OnItemsChanged(args.EventArgs));
            });
    }

    public ConcurrentObservableCollection<BaseToDoListItemViewModel> Items { get; set; }

    public override int GetItemViewType(int position)
    {
        var itemAtPosition = GetItem(position);

        return (itemAtPosition as BaseToDoListItemViewModel)?.ItemType switch
        {
            ToDoListItemType.Task => Resource.Layout.todo_list_item_template,
            ToDoListItemType.Loading => Resource.Layout.loading_item,
        };
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        var itemBindingContext = new MvxAndroidBindingContext(parent.Context, BindingContext.LayoutInflaterHolder);
        var view = InflateViewForHolder(parent, viewType, itemBindingContext);
        return viewType switch
        {
            Resource.Layout.todo_list_item_template => new ToDoListItemRecyclerViewHolder(view, itemBindingContext)
            {
                Id = viewType
            },
            Resource.Layout.loading_item => new LoadingRecyclerViewHolder(view, itemBindingContext)
            {
                Id = viewType
            },
        };
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _disposable?.Dispose();
            _disposable = null;
        }
        base.Dispose(disposing);
    }

    private void OnItemsChanged(NotifyCollectionChangedEventArgs argsEventArgs)
    {
        switch (argsEventArgs.NewStartingIndex == 0)
        {
            case true:
                _scrollToTop?.Invoke();
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
