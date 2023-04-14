using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using AndroidX.Core.Content;

namespace ToDoList.Droid.Extensions;

public static class ContextExtensions
{
    public static Drawable GetDrawableFromAttribute(this Context context, int attributeId)
    {
        if (context?.Theme == null)
        {
            return null;
        }

        var typedValue = new TypedValue();
        context.Theme.ResolveAttribute(attributeId, typedValue, true);
        return ContextCompat.GetDrawable(context, typedValue.ResourceId);
    }
}
