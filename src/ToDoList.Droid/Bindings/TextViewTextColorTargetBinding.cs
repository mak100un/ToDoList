using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using ToDoList.Core.Definitions.Extensions;

namespace ToDoList.Droid.Bindings;

public class TextViewTextColorTargetBinding : MvxAndroidTargetBinding<TextView, int>
{
    public TextViewTextColorTargetBinding(TextView textView)
        : base(textView)
    {
    }

    protected override void SetValueImpl(TextView textView, int resId) => textView?.Context?.Then(context => textView.SetTextColor(context.Resources.GetColor(resId, context.Theme)));
}
