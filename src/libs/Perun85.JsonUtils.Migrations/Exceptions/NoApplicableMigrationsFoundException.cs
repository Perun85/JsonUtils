using JsonMigrator.Utils;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Perun85.JsonUtils.Migrations.Exceptions;

/// <summary>
/// Exception thrown when there is no applicable migration registered for that version of the JSON document.
/// </summary>

#if !NET8_0_OR_GREATER
[Serializable]
#endif
public sealed class NoApplicableMigrationsFoundException : Exception
{
    public NoApplicableMigrationsFoundException()
    {
    }

    public NoApplicableMigrationsFoundException(string message) : base(message)
    {
    }

    public NoApplicableMigrationsFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !NET8_0_OR_GREATER
    private NoApplicableMigrationsFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif

    internal static void Throw(string documentId, uint documentInitialVersion)
    {
        Arg.Guard.AgainstStringNullOrEmpty(documentId);
        throw new NoApplicableMigrationsFoundException($"Document '{documentId}' with version '{documentInitialVersion}' has no applicable migrations.");
    }
}
