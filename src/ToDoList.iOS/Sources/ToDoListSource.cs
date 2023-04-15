using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData.Binding;
using Foundation;
using ReactiveUI;
using ToDoList.Core.ViewModels.Items;
using ToDoList.iOS.Cells;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.Sources;

public class ToDoListSource : UITableViewSource, INotifyPropertyChanged
{
    private readonly CompositeDisposable _compositeDisposable = new ();
    private IDisposable _disposable;

    public ToDoListSource(UITableView tableView)
    {
        TableView = tableView;
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

    public UITableView TableView { get; }

    public ObservableCollection<ToDoListItemViewModel> Items { get; set; }

    public ICommand LoadMoreCommand { get; set; }

    public ICommand EditTaskCommand { get; set; }

    public int LoadingOffset { get; set; }

    public bool IsLoadingMore { get; set; }

    public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
    {
        tableView.DeselectRow(indexPath, false);
        EditTaskCommand?.Execute(Items?.ElementAtOrDefault(indexPath.Row));
    }

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

    public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
    {
        var item = Items?.ElementAtOrDefault(indexPath.Row);

        if (item == null)
        {
            return null;
        }

        UITableViewCell cell = tableView.DequeueReusableCell(nameof(ToDoListItemCell), indexPath);

        return cell switch
        {
            ToDoListItemCell itemCell => GetItemCell(itemCell),
            _ => cell,
        };

        ToDoListItemCell GetItemCell(ToDoListItemCell itemCell)
        {
            itemCell.DataContext = item;
            return itemCell;
        }
    }

    public override nint RowsInSection(UITableView tableview, nint section) => Items?.Count ?? 0;

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

    private void OnItemsChanged(NotifyCollectionChangedEventArgs e)
    {
		switch (e.Action)
		{
			case NotifyCollectionChangedAction.Add:
				if (e.NewStartingIndex == -1)
					goto case NotifyCollectionChangedAction.Reset;

				InsertRows(e.NewStartingIndex, e.NewItems.Count);

				break;

			case NotifyCollectionChangedAction.Remove:
				if (e.OldStartingIndex == -1)
					goto case NotifyCollectionChangedAction.Reset;

				DeleteRows(e.OldStartingIndex, e.OldItems.Count);

				break;

			case NotifyCollectionChangedAction.Move:
				if (e.OldStartingIndex == -1 || e.NewStartingIndex == -1)
					goto case NotifyCollectionChangedAction.Reset;

				MoveRows(e.NewStartingIndex, e.OldStartingIndex, e.OldItems.Count);

				break;

			case NotifyCollectionChangedAction.Replace:
				if (e.OldStartingIndex == -1)
					goto case NotifyCollectionChangedAction.Reset;

				ReloadRows(e.OldStartingIndex, e.OldItems.Count);

				break;

			case NotifyCollectionChangedAction.Reset:
				ReloadData();
				return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ReloadData() => TableView.ReloadData();

    private void ReloadRows(int oldStartingIndex, int oldItemsCount)
    {
        TableView.BeginUpdates();
        TableView.ReloadRows(GetPaths(oldStartingIndex, oldItemsCount), UITableViewRowAnimation.Automatic);
        TableView.EndUpdates();
    }

    private void InsertRows(int newStartingIndex, int newItemsCount)
    {
        TableView.BeginUpdates();
        TableView.InsertRows(GetPaths(newStartingIndex, newItemsCount), UITableViewRowAnimation.Automatic);
        TableView.EndUpdates();
    }

    private void DeleteRows(int oldStartingIndex, int oldItemsCount)
    {
        TableView.BeginUpdates();
        TableView.DeleteRows(GetPaths(oldStartingIndex, oldItemsCount), UITableViewRowAnimation.Automatic);
        TableView.EndUpdates();
    }

    private void MoveRows(int newStartingIndex, int oldStartingIndex, int oldItemsCount)
    {
        TableView.BeginUpdates();
        for (var i = 0; i < oldItemsCount; i++)
        {
            var oldIndex = oldStartingIndex;
            var newIndex = newStartingIndex;

            if (newStartingIndex < oldStartingIndex)
            {
                oldIndex += i;
                newIndex += i;
            }

            TableView.MoveRow(NSIndexPath.FromRowSection(oldIndex, 0), NSIndexPath.FromRowSection(newIndex, 0));
        }
        TableView.EndUpdates();
    }


    private static NSIndexPath[] GetPaths(int index, int count)
    {
        var paths = new NSIndexPath[count];

        for (var i = 0; i < paths.Length; i++)
        {
            paths[i] = NSIndexPath.FromRowSection(index + i, 0);
        }

        return paths;
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
