using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
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

    public static void HideKeyboard(this Context context, View view) => (context.GetSystemService(Context.InputMethodService) as InputMethodManager)?.HideSoftInputFromWindow(view.WindowToken, HideSoftInputFlags.None);
}
