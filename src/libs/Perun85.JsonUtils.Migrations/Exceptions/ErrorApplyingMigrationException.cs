using JsonMigrator.Utils;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Perun85.JsonUtils.Migrations.Exceptions;

/// <summary>
/// Exception throw when the engine experiences an error during application of the migration.
/// Exception caused by migration is placed inside the <see cref="Exception.InnerException"/> property.
/// </summary>
#if !NET8_0_OR_GREATER
[Serializable]
#endif
public sealed class ErrorApplyingMigrationException : Exception
{
    public ErrorApplyingMigrationException()
    {
    }

    public ErrorApplyingMigrationException(string message) : base(message)
    {
    }

    public ErrorApplyingMigrationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !NET8_0_OR_GREATER
    private ErrorApplyingMigrationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif

    internal static void Throw(string migrationName, Exception innerException)
    {
        Arg.Guard.AgainstStringNullOrEmpty(migrationName);
        Arg.Guard.AgainstNull(innerException);

        throw new ErrorApplyingMigrationException($"Failed to apply migration '{migrationName}'.", innerException);
    }
}
