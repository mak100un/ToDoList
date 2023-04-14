using System.Collections.Generic;
using Android.Views;

namespace ToDoList.Droid.Extensions;

public static class ViewGroupExtensions
{
    public static IEnumerable<View> GetAllChildViews(this ViewGroup viewGroup)
    {
        if (viewGroup.ChildCount <= 0)
        {
            return null;
        }

        var childs = new View[viewGroup.ChildCount];

        for (var i = 0; i < viewGroup.ChildCount; i++)
        {
            childs[i] = viewGroup.GetChildAt(i);
        }

        return childs;
    }

    public static TView GetChildAt<TView>(this ViewGroup viewGroup, int index)
        where TView : View
    {
        if (viewGroup.ChildCount <= index)
        {
            return null;
        }

        return viewGroup.GetChildAt(index) as TView;
    }
}
