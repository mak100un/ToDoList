using System;
using CoreGraphics;
using UIKit;

namespace ToDoList.iOS.Views;

public class DynamicFooterTableView : UITableView
{
    public DynamicFooterTableView(CGRect frame, UITableViewStyle style)
    : base(frame, style)
    {

    }

    public override void LayoutSubviews()
    {
        base.LayoutSubviews();

        if (TableFooterView == null)
        {
            return;
        }

        UIView footerView = TableFooterView;

        nfloat width = Bounds.Size.Width;
        CGSize size = footerView.SystemLayoutSizeFittingSize(new CGSize(width: width, height: UILayoutFittingCompressedSize.Height));
        if (footerView.Frame.Size.Height == size.Height)
        {
            return;
        }

        CGSize footerSize = footerView.Frame.Size;
        footerSize.Height = size.Height;

        CGRect footerFrame = footerView.Frame;
        footerFrame.Size = footerSize;

        footerView.Frame = footerFrame;
        TableFooterView = footerView;
    }
}
