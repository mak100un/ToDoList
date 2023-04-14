using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;
using ToDoList.Core.Definitions.Extensions;

namespace ToDoList.Droid.Bindings;

public class ImageViewDrawableIdTargetBinding: MvxAndroidTargetBinding<ImageView, int>
{
    public ImageViewDrawableIdTargetBinding(ImageView imageView)
        : base(imageView)
    {
    }

    protected override void SetValueImpl(ImageView imageView, int resId) => imageView?.SetImageResource(resId);
}

