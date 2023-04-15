using System;
using MvvmCross.Binding.BindingContext;
using ToDoList.Core.Definitions.Converters.Genetics;

namespace ToDoList.Core.Definitions.Extensions;

public static class MvxFluentBindingDescriptionExtensions
{
    public static MvxFluentBindingDescription<TTarget, TSource> WithGenericConversion<TTarget, TSource, TParam, TResult>(this MvxFluentBindingDescription<TTarget, TSource> bindingDescription, Func<TParam, TResult> converterFunc)
        where TTarget : class =>
        bindingDescription.WithConversion(new GenericValueConverter<TParam, TResult>(converterFunc));
}
