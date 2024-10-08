namespace FlightControl.Utility
{
    public static class ValidationHelper
    {
        public static bool IsAlphaNumeric(string stringToValidate)
        {
            return stringToValidate.All(c => char.IsLetterOrDigit(c));
        }
    }
}