using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Views;

public class LoaderView : UITableViewHeaderFooterView
{
    private UIActivityIndicatorView _indicator;

    public LoaderView(IntPtr intPtr)
    : base(intPtr)
    {
        _indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Medium)
        {
            Color = ColorPalette.Accent,
            HidesWhenStopped = false,
            Transform = CGAffineTransform.MakeScale(1.5f, 1.5f)
        };
        Add(_indicator);

        this.AddConstraints(

            // indicator
            _indicator.AtLeadingOf(this),
            _indicator.AtTrailingOf(this),
            _indicator.AtTopOf(this, 10),
            _indicator.AtBottomOf(this, 20)
        );

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    public void StartAnimating() => _indicator?.StartAnimating();

    public void StopAnimating() => _indicator?.StopAnimating();

    protected override void Dispose(bool disposing)
    {
        _indicator?.Dispose();
        _indicator = null;
        base.Dispose(disposing);
    }
}
