using System;
using System.Globalization;
using MvvmCross.Converters;
using ToDoList.Core.Definitions.Enums;
using ToDoList.iOS.Styles;
using UIKit;

namespace ToDoList.iOS.Definitions.Converters;

public class StatusToTextColorConverter : MvxValueConverter<ToDoTaskStatus, UIColor>
{
    protected override UIColor Convert(ToDoTaskStatus value, Type targetType, object parameter, CultureInfo culture)
        => value switch
        {
            ToDoTaskStatus.Done => ColorPalette.DisabledTextColor,
            _ => ColorPalette.Primary,
        };
}
