using System.Linq;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.iOS.Styles;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

public abstract class BaseViewController<TViewModel> : MvxViewController<TViewModel>
    where TViewModel : class, IBaseViewModel
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View.BackgroundColor = ColorPalette.Primary;
        NavigationItem.BackButtonTitle = string.Empty;
        NavigationItem.Title = ViewModel.PageTitle;

        if (NavigationController?.NavigationBar?.Subviews?.Any(subview => subview is NavigationBarUnderLine) == false)
        {
            NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
            NavigationController.NavigationBar.BackgroundColor = ColorPalette.Primary;
            NavigationController.NavigationBar.TintColor = ColorPalette.Accent;
            NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes
            {
                ForegroundColor = ColorPalette.Accent,
            };

            var bottomLine = new NavigationBarUnderLine
            {
                BackgroundColor = ColorPalette.NavigationBarUnderLineColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            NavigationController.NavigationBar.Add(bottomLine);

            NSLayoutConstraint.ActivateConstraints(new[]
            {
                bottomLine.TopAnchor.ConstraintGreaterThanOrEqualTo(NavigationController.NavigationBar.TopAnchor),
                bottomLine.HeightAnchor.ConstraintEqualTo(0.5f),
            });

            NavigationController.NavigationBar.AddConstraints(
                // _bottomLine
                bottomLine.AtLeadingOf(NavigationController.NavigationBar),
                bottomLine.AtTrailingOf(NavigationController.NavigationBar),
                bottomLine.AtBottomOf(NavigationController.NavigationBar)
            );
        }

        CreateView();
        LayoutView();
        BindView();
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        // TODO Why here
        View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    protected virtual void CreateView()
    {
    }

    protected virtual void LayoutView()
    {
    }

    protected virtual void BindView()
    {
    }
}
