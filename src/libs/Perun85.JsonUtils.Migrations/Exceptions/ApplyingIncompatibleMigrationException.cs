#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Perun85.JsonUtils.Migrations.Exceptions;

/// <summary>
/// Exception thrown when engine tries to apply migration on the document with incompatible version.
/// Appearance of this exception indicates there is a bug in the engine.
/// </summary>

#if !NET8_0_OR_GREATER
[Serializable]
#endif
public sealed class ApplyingIncompatibleMigrationException : Exception
{
    public ApplyingIncompatibleMigrationException()
    {
    }

    public ApplyingIncompatibleMigrationException(string message) : base(message)
    {
    }

    public ApplyingIncompatibleMigrationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !NET8_0_OR_GREATER
    private ApplyingIncompatibleMigrationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
}
