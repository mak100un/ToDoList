using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using ToDoList.Droid.Extensions;

namespace ToDoList.Droid.Views
{
    public abstract class BaseActivity<TViewModel> : MvxActivity<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        private readonly Rect _currentFocusRect = new Rect();

        protected abstract int ActivityLayoutId { get; }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (ev.Action != MotionEventActions.Up
                || CurrentFocus is not EditText editText)
            {
                return base.DispatchTouchEvent(ev);
            }

            var result = base.DispatchTouchEvent(ev);

            if (editText != CurrentFocus)
            {
                return result;
            }

            var location = new int[2];
            editText.GetLocationOnScreen(location);
            _currentFocusRect.Left = location[0];
            _currentFocusRect.Top = location[1];
            _currentFocusRect.Right = location[0] + editText.Width;
            _currentFocusRect.Bottom = location[1] + editText.Height;

            if (_currentFocusRect.Contains((int)ev.GetX(), (int)ev.GetY()))
            {
                return result;
            }

            this.HideKeyboard(editText);
            editText.ClearFocus();

            return result;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(ActivityLayoutId);
        }
    }
}
