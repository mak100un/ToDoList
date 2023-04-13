using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using ToDoList.Core.Definitions.Converters;
using ToDoList.Core.ViewModels.Items;
using ToDoList.iOS.Definitions.Converters;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Cells;

public class ToDoListItemCell : UITableViewCell, IMvxBindingContextOwner
{
    public ToDoListItemCell(IntPtr intPtr)
        : base(intPtr)
    {
        this.CreateBindingContext();
        InitCell();
    }

    public IMvxBindingContext BindingContext { get; set; }

    [MvxSetToNullAfterBinding]
    public object DataContext
    {
        get => BindingContext?.DataContext;
        set
        {
            if (BindingContext == null)
            {
                return;
            }

            BindingContext.DataContext = value;
        }
    }

    private void InitCell()
    {
        var titleLabel = new UILabel
        {
            TextAlignment = UITextAlignment.Left,
            TextColor = ColorPalette.Primary,
            Font = FontPalette.TitleSize,
            TranslatesAutoresizingMaskIntoConstraints = false,
            Lines = 1,
            LineBreakMode = UILineBreakMode.TailTruncation
        };

        var statusLabel = new UILabel
        {
            TextAlignment = UITextAlignment.Left,
            TextColor = ColorPalette.Primary,
            Font = FontPalette.SecondarySize,
            TranslatesAutoresizingMaskIntoConstraints = false,
            Lines = 1,
        };

        var statusImage = new UIImageView
        {
            ContentMode = UIViewContentMode.ScaleAspectFit,
            TranslatesAutoresizingMaskIntoConstraints = false,
        };

        var container = new UIView
        {
            BackgroundColor =  ColorPalette.PrimaryButton,
            ClipsToBounds = true,
        };
        container.Layer.CornerRadius = 8;

        container.Add(titleLabel);
        container.Add(statusLabel);
        container.Add(statusImage);

        Add(container);

        NSLayoutConstraint.ActivateConstraints(new[]
        {
            statusLabel.LeadingAnchor.ConstraintGreaterThanOrEqualTo(titleLabel.TrailingAnchor, 12),
            statusImage.LeadingAnchor.ConstraintGreaterThanOrEqualTo(statusLabel.TrailingAnchor, 8),
        });

        this.AddConstraints(
            // container
            container.AtLeadingOf(this, 20),
            container.AtTrailingOf(this, 20),
            container.AtTopOf(this, 10),
            container.AtBottomOf(this, 10),

            // titleLabel
            titleLabel.AtLeadingOf(container, 20),
            titleLabel.AtTopOf(container, 12),
            titleLabel.AtBottomOf(container, 12),

            // statusLabel
            statusLabel.AtTopOf(container, 12),
            statusLabel.AtBottomOf(container, 12),

            // statusLabel
            statusImage.AtTopOf(container, 12),
            statusImage.AtBottomOf(container, 12),
            statusImage.AtTrailingOf(container, 20)
        );

        this.DelayBind(() =>
        {
            var set = this.CreateBindingSet<ToDoListItemCell, ToDoListItemViewModel>();

            set.Bind(titleLabel)
                .For(x => x.Text)
                .To(vm => vm.Title);

            set.Bind(statusLabel)
                .For(x => x.Text)
                .To(vm => vm.Status)
                .WithConversion(new StatusToTextConverter());

            set.Bind(statusImage)
                .For(x => x.Image)
                .To(vm => vm.Status)
                .WithConversion(new StatusToImageConverter());

            set.Bind(titleLabel)
                .For(x => x.TextColor)
                .To(vm => vm.Status)
                .WithConversion(new StatusToTextColorConverter());

            set.Bind(statusLabel)
                .For(x => x.TextColor)
                .To(vm => vm.Status)
                .WithConversion(new StatusToTextColorConverter());

            set.Bind(container)
                .For(x => x.BackgroundColor)
                .To(vm => vm.Status)
                .WithConversion(new StatusToBackgroundColorConverter());

            set.Apply();
        });

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }
}
