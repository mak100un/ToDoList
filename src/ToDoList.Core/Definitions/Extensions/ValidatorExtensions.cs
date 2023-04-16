using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ToDoList.Core.Definitions.Extensions;

public static class ValidatorExtensions
{
    private static readonly Type _validationAttributeType = typeof(ValidationAttribute);

    public static bool Validate(this object @object)
    {
        var validationResult = new List<ValidationResult>();
        return Validator.TryValidateObject(
            @object, new ValidationContext(@object, null, null), validationResult, validateAllProperties: true);
    }

    public static bool Validate(this object @object, out List<ValidationResult> validationResult)
    {
        validationResult = new List<ValidationResult>();
        return Validator.TryValidateObject(
            @object, new ValidationContext(@object, null, null), validationResult, validateAllProperties: true);
    }

    public static string Validate<TTarget>(this TTarget target, string targetPropName)
        where TTarget : class
    {
        PropertyInfo targetProp = typeof(TTarget).GetProperty(targetPropName);
        var targetPropAttributes = targetProp?.GetCustomAttributes(_validationAttributeType, inherit: true) as ValidationAttribute[];

        if (targetPropAttributes?.Any() != true)
        {
            return null;
        }

        var validationContext = new ValidationContext(target);
        var propValue = targetProp.GetValue(target);

        return
            (from targetPropAttribute
                in targetPropAttributes
                select targetPropAttribute.GetValidationResult(propValue, validationContext)
                into validationResult
                where validationResult != ValidationResult.Success
                select validationResult?.ErrorMessage)
            .FirstOrDefault();
    }
}
