using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Validation
{
    // Validates that a value is one of the enum's declared members.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DefinedEnumAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            if (value is null) return ValidationResult.Success;
            var t = value.GetType();
            if (!t.IsEnum) return new ValidationResult($"{context.MemberName} must be an enum");
            return Enum.IsDefined(t, value)
                ? ValidationResult.Success
                : new ValidationResult($"{context.MemberName} has an ivalid value.");
        }
    }
}