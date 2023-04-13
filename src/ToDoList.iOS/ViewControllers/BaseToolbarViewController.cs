using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using ToDoList.Core.Definitions.Converters;
using ToDoList.Core.ViewModels.Interfaces;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

public abstract class BaseToolbarViewController<TViewModel> : BaseViewController<TViewModel>
    where TViewModel : class, IBaseToolbarViewModel
{
    public abstract string Image { get; }

    public sealed override void ViewDidLoad()
    {
        base.ViewDidLoad();

        NavigationItem.RightBarButtonItem = new UIBarButtonItem(UIImage.FromBundle(Image), UIBarButtonItemStyle.Plain, null);

        var set = this.CreateBindingSet<BaseToolbarViewController<TViewModel>, IBaseToolbarViewModel>();

        set.Bind(NavigationItem.RightBarButtonItem)
            .For(x => x.BindClicked())
            .To(vm => vm.ToolbarCommand);

        set
            .Bind(NavigationItem.RightBarButtonItem)
            .For(v => v.Hidden)
            .To(vm => vm.ToolbarItemVisible)
            .WithConversion(new InvertedValueConverter());

        set.Apply();
    }
}
