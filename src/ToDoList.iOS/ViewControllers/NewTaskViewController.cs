using System.Drawing;
using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using ToDoList.Core.ViewModels;
using ToDoList.iOS.Extensions;
using ToDoList.iOS.Styles;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

[MvxChildPresentation]
public class NewTaskViewController : BaseViewController<NewTaskViewModel>
{
    private UIStackView _titleStack;
    private UITextField _titleField;
    private UIStackView _descriptionStack;
    private UITextView _descriptionView;
    private UIScrollView _scrollView;
    private UIButton _actionButton;
    private UIView _contentView;
    private bool _moreThan11;

    public override void ViewWillAppear(bool animated)
    {
        base.ViewWillAppear(animated);
        _scrollView?.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    protected override void CreateView()
    {
        base.CreateView();

        View.Add(_scrollView = new UIScrollView
        {
            TranslatesAutoresizingMaskIntoConstraints = false,
        });

        _scrollView.Add(_contentView = new UIView());

        _contentView.Add(_titleStack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Fill,
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 4,
        });

        _titleStack.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.TITLE));

        _titleStack.AddArrangedSubview(_titleField = new UITextField
        {
            TextColor = ColorPalette.Accent,
            Font = FontPalette.BodySize,
            BackgroundColor = ColorPalette.InputBackgroundButton,
            ClipsToBounds = true,
            ReturnKeyType = UIReturnKeyType.Done,
        });

        _titleField.AttributedPlaceholder =  new NSAttributedString(TextResources.TITLE_PLACEHOLDER, FontPalette.BodySize, ColorPalette.PlaceholderColor, UIColor.Clear);

        var titlePaddingView = new UIView(new CGRect(0, 0, 16, 0));
        _titleField.LeftView = titlePaddingView;
        _titleField.RightView = titlePaddingView;
        _titleField.LeftViewMode = UITextFieldViewMode.Always;
        _titleField.RightViewMode = UITextFieldViewMode.Always;
        _titleField.ShouldReturn = OnShouldReturn;

        if (_moreThan11 = UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
        {
            _titleField.Layer.CornerRadius = 4;
            _titleField.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
        }
        _titleField.Layer.MasksToBounds = true;

        _contentView.Add(_descriptionStack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Fill,
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 4,
        });

        _descriptionStack.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.DESCRIPTION));

        _descriptionStack.AddArrangedSubview(_descriptionView = new PlaceholderedTextView
        {
            TextColor = ColorPalette.Accent,
            Font = FontPalette.BodySize,
            Placeholder = TextResources.DESCRIPTION_PLACEHOLDER,
            BackgroundColor = ColorPalette.InputBackgroundButton,
            ClipsToBounds = true,
            ReturnKeyType = UIReturnKeyType.Done,
        });

        if (_moreThan11)
        {
            _descriptionView.Layer.CornerRadius = 4;
            _descriptionView.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
        }

        _descriptionView.Layer.MasksToBounds = true;

        _contentView.Add(_actionButton = ViewPalette.CreateActionButton(TextResources.CREATE));

        _scrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        _contentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    protected override void LayoutView()
    {
        base.LayoutView();

        var safeAreaGuide = View.SafeAreaLayoutGuide;

        NSLayoutConstraint.ActivateConstraints(new []
        {
            // _scrollView
            _scrollView.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
            _scrollView.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()),
            _scrollView.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
            _scrollView.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),
            _titleField.HeightAnchor.ConstraintEqualTo(44),

            _descriptionView.HeightAnchor.ConstraintEqualTo(200),

            _actionButton.TopAnchor.ConstraintGreaterThanOrEqualTo(_descriptionStack.BottomAnchor, 54),

            _actionButton.HeightAnchor.ConstraintEqualTo(55),
        });

        View.AddConstraints(
            // _contentView
            _contentView.AtTopOf(_scrollView),
            _contentView.AtLeadingOf(_scrollView),
            _contentView.AtTrailingOf(_scrollView),
            _contentView.AtBottomOf(_scrollView),

            _contentView.WithSameWidth(_scrollView),
            _contentView.WithSameHeight(_scrollView),

            // _titleStack
            _titleStack.AtTopOf(_contentView, 12),
            _titleStack.AtLeadingOf(_contentView, 20),
            _titleStack.AtTrailingOf(_contentView, 20),

            // _descriptionStack
            _descriptionStack.Below(_titleStack, 32),
            _descriptionStack.AtLeadingOf(_contentView, 20),
            _descriptionStack.AtTrailingOf(_contentView, 20),

            // _itemInfoStack
            _actionButton.AtLeadingOf(_contentView, 20),
            _actionButton.AtTrailingOf(_contentView, 20),
            _actionButton.AtBottomOf(_contentView, 54)
        );
    }

    protected override void BindView()
    {
        base.BindView();

        var set = this.CreateBindingSet<NewTaskViewController, NewTaskViewModel>();

        set
            .Bind(_titleField)
            .For(v => v.Text)
            .To(vm => vm.Title);

        set
            .Bind(_descriptionView)
            .For(v => v.Text)
            .To(vm => vm.Description);

        set
            .Bind(_actionButton)
            .For(v => v.BindTouchUpInside())
            .To(vm => vm.ActionCommand);

        set
            .Bind(_actionButton)
            .For(v => v.Enabled)
            .To(vm => vm.IsValid);

        set.Apply();
    }

    public override void ViewWillLayoutSubviews()
    {
        base.ViewWillLayoutSubviews();
        if (_moreThan11)
        {
            return;
        }

        _titleField.AddCornerRadius(UIRectCorner.TopLeft | UIRectCorner.TopRight, 4);
        _descriptionView.AddCornerRadius(UIRectCorner.TopLeft | UIRectCorner.TopRight, 4);
    }

    private static bool OnShouldReturn(UITextField textfield)
    {
        textfield?.ResignFirstResponder();
        return false;
    }
}
