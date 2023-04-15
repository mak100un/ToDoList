using Android.Views;
using AndroidX.RecyclerView.Widget;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels.Items;
using ToDoList.Droid.ViewHolder;

namespace ToDoList.Droid.Adapters;

public class ToDoListItemRecyclerAdapter : MvxRecyclerAdapter
{
    public ToDoListItemRecyclerAdapter(IMvxAndroidBindingContext bindingContext)
        : base(bindingContext)
    {
    }

    public override int GetItemViewType(int position)
    {
        var itemAtPosition = GetItem(position);

        // TODO MvxTemplateSelector
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
            Resource.Layout.todo_list_item_template => new ToDoListItemRecyclerViewHolder(view, itemBindingContext, ItemClick)
            {
                Id = viewType
            },
            Resource.Layout.loading_item => new LoadingRecyclerViewHolder(view, itemBindingContext)
            {
                Id = viewType
            },
        };
    }
}
