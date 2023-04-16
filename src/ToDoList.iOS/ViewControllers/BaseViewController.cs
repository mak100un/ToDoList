using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Views;
using ToDoList.Core.ViewModels.Interfaces;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

public abstract class BaseViewController<TViewModel> : MvxViewController<TViewModel>
    where TViewModel : class, IBasePageTitledViewModel
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        View.BackgroundColor = ColorPalette.Primary;
        NavigationItem.BackButtonTitle = string.Empty;
        NavigationItem.Title = ViewModel.PageTitle;

        CreateView();
        View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        LayoutView();
        BindView();
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
