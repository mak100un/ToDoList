using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ReactiveUI;
using ToDoList.Core.Definitions.Converters;
using ToDoList.Core.ViewModels.Items;
using ToDoList.Droid.Definitions.Converters;

namespace ToDoList.Droid.ViewHolder;

public class ToDoListItemRecyclerViewHolder : MvxRecyclerViewHolder
{
    private readonly CompositeDisposable _compositeDisposable = new ();

    // TODO click - Adapter.ItemClick ; MvxRecyclerViewHolder.OnItemViewClick
    public ToDoListItemRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context, ICommand itemClick)
        : base(itemView, context)
    {
        var toDoListItemTitleView = itemView.FindViewById<TextView>(Resource.Id.todo_list_item_title_view);
        var toDoListItemStatusView = itemView.FindViewById<TextView>(Resource.Id.todo_list_item_status_view);
        var toDoListItemBackgroundLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.todo_list_item_background_layout);

        Observable.FromEventPattern<EventHandler, EventArgs>(
                h => toDoListItemBackgroundLayout.Click += h,
                h => toDoListItemBackgroundLayout.Click -= h)
            .Select(_ => DataContext)
            .InvokeCommand(itemClick);

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<ToDoListItemRecyclerViewHolder, ToDoListItemViewModel>();

            set.Bind(toDoListItemBackgroundLayout)
                .For("Background")
                .To(vm => vm.Status)
                .WithConversion<StatusToBackgroundConverter>();

            set.Bind(toDoListItemTitleView)
                .For(v => v.Text)
                .To(vm => vm.Title);

            set.Bind(toDoListItemTitleView)
                .For("TextColor")
                .To(vm => vm.Status)
                .WithConversion<StatusToTextColorConverter>();

            set.Bind(toDoListItemStatusView)
                .For(v => v.Text)
                .To(vm => vm.Status)
                .WithConversion<StatusToTextConverter>();

            set.Bind(toDoListItemStatusView)
                .For("TextColor")
                .To(vm => vm.Status)
                .WithConversion<StatusToTextColorConverter>();

            set.Bind(itemView.FindViewById<ImageView>(Resource.Id.todo_list_item_image_view))
                .For("DrawableResourceId")
                .To(vm => vm.Status)
                .WithConversion<StatusToImageConverter>();

            set.Apply();
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _compositeDisposable?.Clear();
        }

        base.Dispose(disposing);
    }
}
