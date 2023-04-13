using System;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using ReactiveUI;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Views;

public class PlaceholderedTextView : UITextView
{
    private UILabel _placeholderLabel;
    private IDisposable _disposable;

    public PlaceholderedTextView()
    {
        _placeholderLabel = new UILabel
        {
            TextColor = ColorPalette.PlaceholderColor,
            Font = FontPalette.BodySize,
            Lines = 1,
            Enabled = false,
            UserInteractionEnabled = false,
            TranslatesAutoresizingMaskIntoConstraints = false,
        };

        Add(_placeholderLabel);

        var edgeInsets = TextContainerInset = new UIEdgeInsets(11, 16, 11, 16);
        TextContainer.LineFragmentPadding = 0;

        this.AddConstraints(
            _placeholderLabel.AtLeadingOf(this, edgeInsets.Left),
            _placeholderLabel.AtTrailingOf(this, edgeInsets.Right),
            _placeholderLabel.AtTopOf(this, edgeInsets.Top),
            _placeholderLabel.AtBottomOf(this, edgeInsets.Bottom)
        );

        TranslatesAutoresizingMaskIntoConstraints = false;

        var keyboardWidth = UIScreen.MainScreen.Bounds.Width;
        var accessoryView = new UIToolbar(new CGRect(0, 0, keyboardWidth, 44))
        {
            BarStyle = UIBarStyle.Default,
            Translucent = true
        };

        var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
        var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => ResignFirstResponder());
        accessoryView.SetItems(new[] { spacer, doneButton }, false);
        InputAccessoryView = accessoryView;

        _disposable = this.WhenAnyValue(tV => tV.Text)
            .Subscribe(_ => _placeholderLabel.Hidden = !string.IsNullOrEmpty(Text));
    }

    public override string Text
    {
        get => base.Text;
        set
        {
            base.Text = value;
            _placeholderLabel.Hidden = !string.IsNullOrEmpty(value);
        }
    }

    public string Placeholder
    {
        set
        {
            if (_placeholderLabel == null)
            {
                return;
            }

            _placeholderLabel.Text = value;
        }
    }



    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _placeholderLabel?.Dispose();
            _placeholderLabel = null;
            _disposable?.Dispose();
            _disposable = null;
        }
        base.Dispose(disposing);
    }
}
