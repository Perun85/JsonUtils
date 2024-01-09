#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Perun85.JsonUtils.Migrations.Exceptions;

/// <summary>
/// Exception thrown when registering migration who's range is overlapping with the already registered one.
/// </summary>
#if !NET8_0_OR_GREATER
[Serializable]
#endif
public sealed class MigrationRangeOverlappingException : Exception
{
    public MigrationRangeOverlappingException()
    {
    }

    public MigrationRangeOverlappingException(string message) : base(message)
    {
    }

    public MigrationRangeOverlappingException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !NET8_0_OR_GREATER
    private MigrationRangeOverlappingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif

    internal static void Throw() => throw new MigrationRangeOverlappingException("Range overlaps with already registered migration.");
}
