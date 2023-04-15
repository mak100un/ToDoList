using System;
using System.Linq;
using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Plugin.Visibility;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.ViewModels;
using ToDoList.iOS.Extensions;
using ToDoList.iOS.Styles;
using ToDoList.iOS.Views;
using UIKit;

namespace ToDoList.iOS.ViewControllers;

[MvxChildPresentation]
public class EditTaskViewController : BaseToolbarViewController<EditTaskViewModel>
{
    private UIStackView _titleStack;
    private UITextField _titleField;
    private UIStackView _descriptionStack;
    private UITextView _descriptionView;
    private UIScrollView _scrollView;
    private UIStackView _statusStack;
    private UISegmentedControl _statusSegmentedControl;
    private UIStackView _itemInfoStack;
    private UIStackView _createdAtInfoStack;
    private UILabel _createdAtInfoLabel;
    private UIStackView _updatedAtInfoStack;
    private UILabel _updatedAtInfoLabel;
    private UIButton _actionButton;
    private UIView _contentView;
    private bool _moreThan11;

    public override string Image => "Delete";

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

        _titleField.AttributedPlaceholder = new NSAttributedString(TextResources.TITLE_PLACEHOLDER, FontPalette.BodySize, ColorPalette.PlaceholderColor, UIColor.Clear);

        var titlePaddingView = new UIView(new CGRect(0, 0, 16, 0));
        _titleField.LeftView = titlePaddingView;
        _titleField.RightView = titlePaddingView;
        _titleField.LeftViewMode = UITextFieldViewMode.Always;
        _titleField.RightViewMode = UITextFieldViewMode.Always;
        _titleField.ShouldReturn = OnShouldReturn;

        // TODO MinimumOSVersion ?
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

        _contentView.Add(_statusStack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Fill,
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 6,
        });

        _statusStack.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.STATUS));

        var items = Enum.GetValues(typeof(ToDoTaskStatus))
            .Cast<ToDoTaskStatus>()
            .Select(status => status.ToString().ToUpper())
            .ToArray();

        _statusStack.AddArrangedSubview(_statusSegmentedControl = new UISegmentedControl(items));

        _contentView.Add(_itemInfoStack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Fill,
            Axis = UILayoutConstraintAxis.Vertical,
            Spacing = 4,
        });

        _itemInfoStack.AddArrangedSubview(_createdAtInfoStack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Fill,
            Axis = UILayoutConstraintAxis.Horizontal,
            Spacing = 4,
        });

        _createdAtInfoStack.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.CREATED_AT));

        _createdAtInfoStack.AddArrangedSubview(_createdAtInfoLabel = ViewPalette.CreateTitleLabel(textAlignment: UITextAlignment.Right));

        _itemInfoStack.AddArrangedSubview(_updatedAtInfoStack = new UIStackView
        {
            Alignment = UIStackViewAlignment.Fill,
            Axis = UILayoutConstraintAxis.Horizontal,
            Spacing = 4,
        });

        _updatedAtInfoStack.AddArrangedSubview(ViewPalette.CreateTitleLabel(TextResources.UPDATED_AT));

        _updatedAtInfoStack.AddArrangedSubview(_updatedAtInfoLabel = ViewPalette.CreateTitleLabel(textAlignment: UITextAlignment.Right));

        _contentView.Add(_actionButton = ViewPalette.CreateActionButton(TextResources.SAVE_CHANGES));

        _scrollView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        _contentView.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    protected override void LayoutView()
    {
        base.LayoutView();

        var safeAreaGuide = View.SafeAreaLayoutGuide;

        // TODO Why native constraints + fluent layout constraints ?
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            // _scrollView
            _scrollView.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
            _scrollView.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()),
            _scrollView.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
            _scrollView.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),
            _titleField.HeightAnchor.ConstraintEqualTo(44),

            _descriptionView.HeightAnchor.ConstraintEqualTo(200),

            _actionButton.TopAnchor.ConstraintGreaterThanOrEqualTo(_statusStack.BottomAnchor, 54),
        });

        View.AddConstraints(
            // _scrollView.AtBottomOfSafeArea(View),
            // _scrollView.AtTopOfSafeArea(View),
            // _scrollView.AtLeftOfSafeArea(View),
            // _scrollView.AtRightOfSafeArea(View),
            
            // _titleField.Height().EqualTo(44),
            
            // _descriptionView.Height().EqualTo(200),
            
            // _actionButton.Top().GreaterThanOrEqualTo().BottomOf(_statusStack).Plus(54),
            
        
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

            // _statusStack
            _statusStack.Below(_descriptionStack, 32),
            _statusStack.AtLeadingOf(_contentView, 20),
            _statusStack.AtTrailingOf(_contentView, 20),

            // _itemInfoStack
            _itemInfoStack.Below(_statusStack, 32),
            _itemInfoStack.AtLeadingOf(_contentView, 20),
            _itemInfoStack.AtTrailingOf(_contentView, 20),

            // _itemInfoStack
            _actionButton.AtLeadingOf(_contentView, 20),
            _actionButton.AtTrailingOf(_contentView, 20),
            _actionButton.AtBottomOf(_contentView, 54)
        );
    }

    protected override void BindView()
    {
        base.BindView();

        var set = this.CreateBindingSet<EditTaskViewController, EditTaskViewModel>();

        set
            .Bind(_titleField)
            .For(v => v.Text)
            .To(vm => vm.Title);

        set
            .Bind(_descriptionView)
            .For(v => v.Text)
            .To(vm => vm.Description);

        set
            .Bind(_createdAtInfoLabel)
            .For(v => v.Text)
            .To(vm => vm.CreatedAt);

        set
            .Bind(_updatedAtInfoLabel)
            .For(v => v.Text)
            .To(vm => vm.UpdatedAt);

        set
            .Bind(_updatedAtInfoStack)
            .For(v => v.BindVisibility())
            .To(vm => vm.UpdatedAtVisible)
            .WithConversion<MvxVisibilityValueConverter>();

        set
            .Bind(_actionButton)
            .For(v => v.BindTouchUpInside())
            .To(vm => vm.ActionCommand);

        set
            .Bind(_actionButton)
            .For(v => v.Enabled)
            .To(vm => vm.IsValid);

        set
            .Bind(_statusSegmentedControl)
            .For(v => v.SelectedSegment)
            .To(vm => vm.SelectedSegment);

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
