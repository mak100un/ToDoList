using System;
using System.Drawing;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace ToDoList.iOS.Services;

public class KeyboardInsetTracker : IDisposable
{
    private readonly UIScrollView _targetView;
    private readonly Action<UIEdgeInsets> _setInsetAction;
    private readonly Action<PointF> _setContentOffset;
    private CGRect _lastKeyboardRect;

    public KeyboardInsetTracker(
        UIScrollView targetView,
        Action<UIEdgeInsets> setInsetAction,
        Action<PointF> setContentOffset)
    {
        _targetView = targetView;
        _setInsetAction = setInsetAction;
        _setContentOffset = setContentOffset;
        KeyboardObserver.KeyboardWillShow += OnKeyboardWillShow;
        KeyboardObserver.KeyboardWillHide += OnKeyboardWillHide;
    }

    private void UpdateInsets()
    {
        if (_lastKeyboardRect.IsEmpty)
            return;

        UIView field = _targetView.FindFirstResponder();
        CGSize boundsSize = _targetView.Frame.Size;

        var rect = (RectangleF)_targetView.Superview.ConvertRectFromView(_lastKeyboardRect, null);
        var overlay = RectangleF.Intersect(rect, (RectangleF)_targetView.Frame);

        _setInsetAction(new UIEdgeInsets(0, 0, overlay.Height, 0));

        if (field is not UITextView)
        {
            return;
        }

        nfloat keyboardTop = boundsSize.Height - overlay.Height;
        CGPoint fieldPosition = field.ConvertPointToView(field.Frame.Location, _targetView.Superview);
        nfloat fieldBottom = fieldPosition.Y + field.Frame.Height;
        nfloat offset = fieldBottom - keyboardTop;
        if (offset > 0)
        {
            _setContentOffset(new PointF(0, (float)offset));
        }
    }

    private void OnKeyboardWillHide(object sender, UIKeyboardEventArgs args)
    {
        _setInsetAction(new UIEdgeInsets(0,0,0,0));
        _lastKeyboardRect = RectangleF.Empty;
    }

    private void OnKeyboardWillShow(object sender, UIKeyboardEventArgs args)
    {
        _lastKeyboardRect = args.FrameEnd;
        UpdateInsets();
    }

    public void Dispose()
    {
        KeyboardObserver.KeyboardWillShow -= OnKeyboardWillShow;
        KeyboardObserver.KeyboardWillHide -= OnKeyboardWillHide;
    }
}
