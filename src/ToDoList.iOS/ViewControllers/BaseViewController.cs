using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

public abstract class BaseViewController<TViewModel> : MvxViewController<TViewModel>
    where TViewModel : class, IBaseViewModel
{
    private static UIView _bottomLine;

    public sealed override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View.BackgroundColor = ColorPalette.Primary;
        NavigationItem.BackButtonTitle = string.Empty;
        NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
        NavigationController.NavigationBar.BackgroundColor = ColorPalette.Primary;
        NavigationController.NavigationBar.TintColor = ColorPalette.Accent;

        NavigationController.NavigationBar.TitleTextAttributes = new UIStringAttributes
        {
            ForegroundColor = ColorPalette.Accent,
        };

        if (_bottomLine == null)
        {
            _bottomLine = new UIView
            {
                BackgroundColor = ColorPalette.PlaceholderColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            NavigationController.NavigationBar.Add(_bottomLine);


            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _bottomLine.TopAnchor.ConstraintGreaterThanOrEqualTo(NavigationController.NavigationBar.TopAnchor),
                _bottomLine.HeightAnchor.ConstraintEqualTo(0.5f),
            });

            NavigationController.NavigationBar.AddConstraints(
                // _bottomLine
                _bottomLine.AtLeadingOf(NavigationController.NavigationBar),
                _bottomLine.AtTrailingOf(NavigationController.NavigationBar),
                _bottomLine.AtBottomOf(NavigationController.NavigationBar)
            );
        }

        CreateView();
        LayoutView();
        BindView();
    }

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        NavigationItem.Title = ViewModel.PageTitle;
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
