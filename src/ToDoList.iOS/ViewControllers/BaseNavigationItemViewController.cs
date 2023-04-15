using System;
using System.Linq.Expressions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using ToDoList.Core.ViewModels.Interfaces;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

public abstract class BaseNavigationItemViewController<TViewModel> : BaseViewController<TViewModel>
    where TViewModel : class, IBasePageTitledViewModel
{
    protected abstract string Image { get; }

    protected abstract Expression<Func<TViewModel, object>> NavigationItemCommandExtractor { get; }

    public sealed override void ViewDidLoad()
    {
        base.ViewDidLoad();

        NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIImage.FromBundle(Image), UIBarButtonItemStyle.Plain, null);

        var set = this.CreateBindingSet<BaseNavigationItemViewController<TViewModel>, TViewModel>();

        set.Bind(NavigationItem.RightBarButtonItem)
            .For(x => x.BindClicked())
            .To(NavigationItemCommandExtractor);

        SetNavigationItemVisibility(set);

        set.Apply();
    }

    protected virtual void SetNavigationItemVisibility(MvxFluentBindingDescriptionSet<BaseNavigationItemViewController<TViewModel>, TViewModel> bindingSet)
    {
    }
}
