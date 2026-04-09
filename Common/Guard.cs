namespace ProjectMaui.Common;

public class Guard
{
    internal static string NotNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty.", paramName);
        return value.Trim();
    }
    internal static DateTime NotDefault(DateTime value, string paramName)
    {
        if (value == DateTime.MinValue)
            throw new ArgumentException("Date must be a valid, non-default date.", paramName);
        return value;
    }
    internal static int AtLeast(int value, int min, string paramName)
    {
        if (value < min)
            throw new ArgumentOutOfRangeException(paramName, $"Value must be at least {min}.");
        return value;
    }
    internal static decimal NotNegative(decimal value, string paramName)
    {
        if (value < 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        return value;
    }
    internal static decimal Positive(decimal value, string paramName)
    {
        if (value <= 0m)
            throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.");
        return value;
    }
}