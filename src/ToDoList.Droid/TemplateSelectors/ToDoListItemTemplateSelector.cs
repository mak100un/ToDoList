using MvvmCross.DroidX.RecyclerView.ItemTemplates;
using ToDoList.Core.ViewModels.Items;

namespace ToDoList.Droid.TemplateSelectors;

public class ToDoListItemTemplateSelector : IMvxTemplateSelector
{
    public int ItemTemplateId { get; set; } = Resource.Layout.item_template;

    public int GetItemViewType(object item)
    {
        if (item is BaseToDoListItemViewModel baseToDoListItemViewModel)
            return (int)baseToDoListItemViewModel.ItemType;

        return -1;
    }

    public int GetItemLayoutId(int viewType)
        => viewType switch
        {
            0 => Resource.Layout.item_template,
            1 => Resource.Layout.loading_item,
            _ => ItemTemplateId,
        };
}
