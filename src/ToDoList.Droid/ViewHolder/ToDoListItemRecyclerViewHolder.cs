using System;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.ViewModels.Items;
using ToDoList.Droid.Definitions.Constants;

namespace ToDoList.Droid.ViewHolder;

public class ToDoListItemRecyclerViewHolder : MvxRecyclerViewHolder
{
    public ToDoListItemRecyclerViewHolder(View itemView, IMvxAndroidBindingContext context)
        : base(itemView, context)
    {
        Func<ToDoTaskStatus, int> statusToTextColorConverter = StatusToTextColorConverter;
        Func<ToDoTaskStatus, int> statusToDrawableResourceIdConverter = StatusToDrawableResourceIdConverter;
        Func<ToDoTaskStatus, int> statusToBackgroundConverter = StatusToBackgroundConverter;

        var toDoListItemTitleView = itemView.FindViewById<TextView>(Resource.Id.todo_list_item_title_view);
        var toDoListItemStatusView = itemView.FindViewById<TextView>(Resource.Id.todo_list_item_status_view);

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<ToDoListItemRecyclerViewHolder, ToDoListItemViewModel>();

            set.Bind(itemView.FindViewById<ConstraintLayout>(Resource.Id.todo_list_item_background_layout))
                .For(BindingConstants.VIEW_BACKGROUND)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToBackgroundConverter);

            set.Bind(toDoListItemTitleView)
                .For(v => v.Text)
                .To(vm => vm.Title);

            set.Bind(toDoListItemTitleView)
                .For(BindingConstants.TEXTVIEW_TEXT_COLOR)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToTextColorConverter);

            set.Bind(toDoListItemStatusView)
                .For(v => v.Text)
                .To(vm => vm.Status)
                .WithGenericConversion((ToDoTaskStatus value) => value.ToString().ToUpper());

            set.Bind(toDoListItemStatusView)
                .For(BindingConstants.TEXTVIEW_TEXT_COLOR)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToTextColorConverter);

            set.Bind(itemView.FindViewById<ImageView>(Resource.Id.todo_list_item_image_view))
                .For(BindingConstants.IMAGEVIEW_DRAWABLE_RESOURCE_ID)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToDrawableResourceIdConverter);

            set.Apply();
        });
    }

    private static int StatusToTextColorConverter(ToDoTaskStatus value) => value switch
    {
        ToDoTaskStatus.Done => Resource.Color.disabled_text_color,
        _ => Resource.Color.primaryColor,
    };

    private static int StatusToDrawableResourceIdConverter(ToDoTaskStatus value) => value switch
    {
        ToDoTaskStatus.Done => Resource.Drawable.done,
        ToDoTaskStatus.InProgress => Resource.Drawable.inprogress,
        ToDoTaskStatus.ToDo => Resource.Drawable.todo,
    };

    private static int StatusToBackgroundConverter(ToDoTaskStatus value) => value switch
    {
        ToDoTaskStatus.Done => Resource.Drawable.todo_item_disabled_background,
        _ => Resource.Drawable.todo_item_primary_background,
    };
}
