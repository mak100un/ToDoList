using Android.Views;
using MvvmCross.Platforms.Android.Binding.Target;

namespace ToDoList.Droid.Bindings;

public class ViewBackgroundTargetBinding : MvxAndroidTargetBinding<View, int>
{
    public ViewBackgroundTargetBinding(View view)
        : base(view)
    {
    }

    protected override void SetValueImpl(View view, int resId) => view?.SetBackgroundResource(resId);
}
