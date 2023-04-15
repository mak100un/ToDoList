using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ToDoList.Core.Definitions.Converters.Genetics;

public class GenericValueConverter<TParam, TResult> : MvxValueConverter<TParam, TResult>
{
    private readonly Func<TParam, TResult> _converter;

    public GenericValueConverter(Func<TParam, TResult> converter)
    {
        _converter = converter;
    }

    protected override TResult Convert(TParam value, Type targetType, object parameter, CultureInfo culture)
        => _converter(value);

    public static explicit operator GenericValueConverter<TParam, TResult>(Func<TParam, TResult> converter) =>
        new(converter);
}
