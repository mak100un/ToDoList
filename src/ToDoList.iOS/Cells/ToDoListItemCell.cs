using System;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using ToDoList.Core.Definitions.Enums;
using ToDoList.Core.Definitions.Extensions;
using ToDoList.Core.ViewModels.Items;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Cells;

public class ToDoListItemCell : MvxTableViewCell
{
    public ToDoListItemCell(IntPtr intPtr)
        : base(intPtr)
    {
        InitCell();
    }

    private void InitCell()
    {
        Func<ToDoTaskStatus, UIColor> statusToBackgroundColorConverter = StatusToBackgroundColorConverter;
        Func<ToDoTaskStatus, UIColor> statusToTextColorConverter = StatusToTextColorConverter;
        Func<ToDoTaskStatus, UIImage> statusToImageConverter = StatusToImageConverter;

        var titleLabel = new UILabel
        {
            TextAlignment = UITextAlignment.Left,
            TextColor = ColorPalette.Primary,
            Font = FontPalette.TitleSize,
            Lines = 1,
            LineBreakMode = UILineBreakMode.TailTruncation
        };

        var statusLabel = new UILabel
        {
            TextAlignment = UITextAlignment.Left,
            TextColor = ColorPalette.Primary,
            Font = FontPalette.SecondarySize,
            Lines = 1,
        };

        var statusImage = new UIImageView
        {
            ContentMode = UIViewContentMode.ScaleAspectFit,
        };

        var container = new UIView
        {
            BackgroundColor =  ColorPalette.PrimaryButton,
            ClipsToBounds = true,
        };
        container.Layer.CornerRadius = 8;

        container.Add(statusLabel);
        container.Add(statusImage);
        container.Add(titleLabel);

        Add(container);

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
            statusLabel.Leading().GreaterThanOrEqualTo(12).TrailingOf(titleLabel),

            // statusLabel
            statusImage.AtTopOf(container, 12),
            statusImage.AtBottomOf(container, 12),
            statusImage.AtTrailingOf(container, 20),
            statusImage.Leading().GreaterThanOrEqualTo(8).TrailingOf(statusLabel)
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
                .WithGenericConversion((ToDoTaskStatus value) => value.ToString().ToUpper());

            set.Bind(statusImage)
                .For(x => x.Image)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToImageConverter);

            set.Bind(titleLabel)
                .For(x => x.TextColor)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToTextColorConverter);

            set.Bind(statusLabel)
                .For(x => x.TextColor)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToTextColorConverter);

            set.Bind(container)
                .For(x => x.BackgroundColor)
                .To(vm => vm.Status)
                .WithGenericConversion(statusToBackgroundColorConverter);

            set.Apply();
        });

        this.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
        container.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
    }

    private static UIColor StatusToBackgroundColorConverter(ToDoTaskStatus value)
        => value switch
        {
            ToDoTaskStatus.Done => ColorPalette.DisabledBackgroundColor,
            _ => ColorPalette.PrimaryButton,
        };

    private static UIColor StatusToTextColorConverter(ToDoTaskStatus value)
        => value switch
        {
            ToDoTaskStatus.Done => ColorPalette.DisabledTextColor,
            _ => ColorPalette.Primary,
        };

    private static UIImage StatusToImageConverter(ToDoTaskStatus value)
        => value switch
        {
            ToDoTaskStatus.Done => UIImage.FromBundle(nameof(ToDoTaskStatus.Done)),
            ToDoTaskStatus.InProgress => UIImage.FromBundle(nameof(ToDoTaskStatus.InProgress)),
            ToDoTaskStatus.ToDo => UIImage.FromBundle(nameof(ToDoTaskStatus.ToDo)),
        };
}
