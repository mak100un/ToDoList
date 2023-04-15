using System;
using Cirrious.FluentLayouts.Touch;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Cells;

public class LoaderCell : UITableViewCell
{
    private UIActivityIndicatorView _indicator;

    public LoaderCell(IntPtr intPtr)
        : base(intPtr)
    {
        InitCell();
    }

    private void InitCell()
    {
        _indicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.Medium)
        {
            Color = ColorPalette.Accent,
            HidesWhenStopped = false,
            Transform = CGAffineTransform.MakeScale(1.5f, 1.5f)
        };

        // TODO ContentView.AddSubview() ?
        Add(_indicator);

        StartAnimating();

        this.AddConstraints(
            // indicator
            _indicator.AtLeadingOf(this),
            _indicator.AtTrailingOf(this),
            _indicator.AtTopOf(this, 10),
            _indicator.AtBottomOf(this, 20)
        );

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    public void StartAnimating() => DispatchQueue.MainQueue.DispatchAsync(() => _indicator?.StartAnimating());

    protected override void Dispose(bool disposing)
    {
        _indicator?.Dispose();
        _indicator = null;
        base.Dispose(disposing);
    }
}
