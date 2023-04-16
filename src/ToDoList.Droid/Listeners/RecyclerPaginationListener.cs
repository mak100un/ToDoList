using System;
using System.ComponentModel;
using System.Windows.Input;
using AndroidX.RecyclerView.Widget;

namespace ToDoList.Droid.Listeners;

public class RecyclerPaginationListener: RecyclerView.OnScrollListener, INotifyPropertyChanged
{
    private readonly LinearLayoutManager _layoutManager;
    private readonly Action<float> _onRecyclerScroll;

    public RecyclerPaginationListener(LinearLayoutManager layoutManager, Action<float> onRecyclerScroll)
    {
        _layoutManager = layoutManager;
        _onRecyclerScroll = onRecyclerScroll;
    }

    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public bool IsLoadingMore { get; set; }

    public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
    {
        base.OnScrolled(recyclerView, dx, dy);
        _onRecyclerScroll?.Invoke(dy);

        if (_layoutManager.FindLastVisibleItemPosition() < (IsLoadingMore ? LoadingOffset - 1 : LoadingOffset))
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}
