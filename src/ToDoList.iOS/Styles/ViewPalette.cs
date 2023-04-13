using UIKit;

namespace ToDoList.iOS.Styles;

public static class ViewPalette
{
    public static UILabel CreateTitleLabel(string text = null, UITextAlignment textAlignment = UITextAlignment.Left, UIFont font = null) =>
        new UILabel
        {
            TextColor = ColorPalette.Accent,
            Font = font ?? FontPalette.SecondarySize,
            Text = text,
            Lines = 1,
            TextAlignment = textAlignment,
        };

    public static UIButton CreateActionButton(string text)
    {
        using var buttonConfig = UIButtonConfiguration.FilledButtonConfiguration;
        buttonConfig.TitleAlignment = UIButtonConfigurationTitleAlignment.Center;
        buttonConfig.BaseForegroundColor = ColorPalette.Primary;
        buttonConfig.BaseBackgroundColor = ColorPalette.PrimaryButton;
        buttonConfig.ContentInsets = new NSDirectionalEdgeInsets(17, 0, 17, 0);
        var button = new UIButton
        {
            HorizontalAlignment = UIControlContentHorizontalAlignment.Fill,
            Configuration = buttonConfig,
            ClipsToBounds = true,
        };
        button.Font = UIFont.BoldSystemFontOfSize(20);
        button.SetTitle(text, UIControlState.Normal);
        button.Layer.CornerRadius = 4;
        return button;
    }
}
