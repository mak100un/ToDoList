using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using CoreFoundation;
using DynamicData.Binding;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using ReactiveUI;
using ToDoList.Core.ViewModels.Items;
using ToDoList.iOS.Cells;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.Sources;

public class ToDoListSource : MvxTableViewSource, INotifyPropertyChanged
{
    private readonly CompositeDisposable _compositeDisposable = new ();
    private readonly Action _scrollToTop;
    private IDisposable _disposable;

    public ToDoListSource(UITableView tableView, Action scrollToTop)
        : base(tableView)
    {
        _scrollToTop = scrollToTop;

        this.WhenAnyValue(vm => vm.Items)
            .WhereNotNull()
            .Subscribe(_ =>
            {
                _disposable?.Dispose();
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
                        var loaderView = TableView.DequeueReusableHeaderFooterView(nameof(LoaderView)) as LoaderView;
                        loaderView?.StartAnimating();
                        TableView.TableFooterView = loaderView;
                        break;
                    case false:
                        (TableView.TableFooterView as LoaderView)?.StopAnimating();
                        TableView.TableFooterView = null;
                        break;
                }
            })
            .DisposeWith(_compositeDisposable);
    }

    protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item) =>
        TableView.DequeueReusableCell(nameof(ToDoListItemCell), indexPath);

    public ObservableCollection<ToDoListItemViewModel> Items { get; set; }

    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public bool IsLoadingMore { get; set; }

    public override void Scrolled(UIScrollView scrollView)
    {
        if (scrollView is not UITableView tableView
            || tableView.IndexPathsForVisibleRows?.Any() != true
            || tableView.IndexPathsForVisibleRows.Max(index => index.Row) < LoadingOffset)
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
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
                DispatchQueue.MainQueue.DispatchAsync(() => _scrollToTop?.Invoke());
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
