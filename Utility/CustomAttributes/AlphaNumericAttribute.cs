using System.ComponentModel.DataAnnotations;

namespace FlightControl.Utility.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class AlphaNumericAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is string stringValue && ValidationHelper.IsAlphaNumeric(stringValue);
        }
    }
}