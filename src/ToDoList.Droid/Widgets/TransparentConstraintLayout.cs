using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.ConstraintLayout.Widget;

namespace ToDoList.Droid.Widgets;

// TODO Purpose ?
public class TransparentConstraintLayout : ConstraintLayout
{
    protected TransparentConstraintLayout(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public TransparentConstraintLayout(Context context)
        : base(context)
    {
    }

    public TransparentConstraintLayout(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
    }

    public TransparentConstraintLayout(Context context, IAttributeSet attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr)
    {
    }

    public TransparentConstraintLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
        : base(context, attrs, defStyleAttr, defStyleRes)
    {
    }

    public override bool OnTouchEvent(MotionEvent e) => false;
}
