using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ToDoList.Core.Definitions.Attributes;

// TODO What's the purpose of this?
public class AnyObserveChangedAttribute : ValidationAttribute
{
    private static readonly Type _observeAttributeType = typeof(ObserveAttribute);

    protected override ValidationResult IsValid
    (
        object value,
        ValidationContext validationContext
    )
    {
        if (validationContext?.ObjectInstance == null
            || value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (GetObserveProperties(validationContext.ObjectInstance) is not { Length: > 0 } validationObserveProps
            || GetObserveProperties(value) is not { Length: > 0 } valueObserveProps)
        {
            throw new ArgumentException($"{value} doesn`t have properties with {_observeAttributeType}.");
        }

        if (validationObserveProps.Join(valueObserveProps,
                validationObserveProp => validationObserveProp.Name,
                valueObserveProp => valueObserveProp.Name,
                (validationObserveProp, valueObserveProp) =>
                    new
                    {
                        ValidationObserveProp = validationObserveProp,
                        ValueObserveProp = valueObserveProp
                    }).ToArray()
            is not { Length: > 0 } propertiesJoin)
        {
            throw new ArgumentException($"No properties with same name in {value} and {validationContext.ObjectInstance}.");
        }

        return propertiesJoin.Any(properties =>
            properties.ValidationObserveProp.GetValue(validationContext.ObjectInstance)?.Equals(properties.ValueObserveProp.GetValue(value)) == false)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage);
    }

    private static PropertyInfo[] GetObserveProperties(object value) => value
        ?.GetType()
        .GetProperties()
        .Where(property => property.CustomAttributes.Any(attribute => attribute.AttributeType == _observeAttributeType))
        .ToArray();
}
