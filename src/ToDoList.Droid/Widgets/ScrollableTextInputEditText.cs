using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Google.Android.Material.TextField;

namespace ToDoList.Droid.Widgets;

public class ScrollableTextInputEditText : TextInputEditText
{
    protected ScrollableTextInputEditText(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
        InitScroll();
    }

    public ScrollableTextInputEditText(Context context)
        : base(context)
    {
        InitScroll();
    }

    public ScrollableTextInputEditText(Context context, IAttributeSet attrs)
        : base(context, attrs)
    {
        InitScroll();
    }

    public ScrollableTextInputEditText(Context context, IAttributeSet attrs, int defStyleAttr)
        : base(context, attrs, defStyleAttr)
    {
        InitScroll();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            SetOnTouchListener(null);
        }

        base.Dispose(disposing);
    }

    private void InitScroll() => SetOnTouchListener(new EditorTouchListener());

    // TODO Change scrollable property is not enough?
    private class EditorTouchListener : Java.Lang.Object, IOnTouchListener
    {
        /// <inheritdoc/>
        public bool OnTouch(Android.Views.View view, MotionEvent e)
        {
            if (!view.IsFocused)
            {
                return false;
            }

            view.Parent?.RequestDisallowInterceptTouchEvent(true);

            if ((e.Action & e.ActionMasked) == MotionEventActions.Up)
            {
                view.Parent?.RequestDisallowInterceptTouchEvent(false);
            }

            return false;
        }
    }
}
