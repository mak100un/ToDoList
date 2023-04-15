using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using DynamicData.Binding;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ReactiveUI;
using ToDoList.Core.ViewModels.Items;
using ToDoList.Droid.ViewHolder;

namespace ToDoList.Droid.Adapters;

public class ToDoListItemRecyclerAdapter : MvxRecyclerAdapter, INotifyPropertyChanged
{
    private readonly CompositeDisposable _compositeDisposable = new ();
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
            })
            .DisposeWith(_compositeDisposable);

        this.WhenAnyValue(vm => vm.IsLoadingMore)
            .Skip(1)
            .Subscribe(_ =>
            {
                switch (IsLoadingMore)
                {
                    case true:
                        NotifyItemInserted(Items.Count);
                        break;
                    default: NotifyItemRemoved(Items.Count);
                        break;
                }
            })
            .DisposeWith(_compositeDisposable);
    }

    public ObservableCollection<ToDoListItemViewModel> Items { get; set; }

    public override int ItemCount => IsLoadingMore ? Items.Count + 1 : Items.Count;

    public bool IsLoadingMore { get; set; }

    public override int GetItemViewType(int position) =>
        (Items.Count == position) switch
        {
            false => Resource.Layout.loading_item,
            _ => Resource.Layout.todo_list_item_template,
        };

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
            _compositeDisposable?.Clear();
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
