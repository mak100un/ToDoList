using Android.Views;
using MvvmCross.Platforms.Android.Binding.Target;

namespace ToDoList.Droid.Bindings;

public class ViewBackgroundColorTargetBinding : MvxAndroidTargetBinding<View, int>
{
    public ViewBackgroundColorTargetBinding(View view)
        : base(view)
    {
    }

    protected override void SetValueImpl(View view, int resId) => view?.SetBackgroundResource(resId);
}
