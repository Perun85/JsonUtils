using System.Runtime.CompilerServices;

namespace JsonMigrator.Utils;

internal static class Arg
{
    internal static class Guard
    {
        internal static void AgainstStringNullOrEmpty(string? value, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("Null or empty string.", argumentName);
        }

        internal static void AgainstNull(object? value, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
        {
            if (value is null) throw new ArgumentNullException(argumentName!);
        }

        internal static void AgainstGreaterOrEqualValue(uint value, uint other, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
        {
            if (value >= other) throw new ArgumentException($"Value should not be greater than or equal to '{other}'.");
        }
    }
}
