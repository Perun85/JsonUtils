#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace Perun85.JsonUtils.Migrations.Exceptions;

/// <summary>
/// Exception throw when document does not contain property with information of its current version.
/// </summary>
#if !NET8_0_OR_GREATER
[Serializable]
#endif
public sealed class VersionPropertyNotFoundException : Exception
{
    public VersionPropertyNotFoundException()
    {
    }

    public VersionPropertyNotFoundException(string message) : base(message)
    {
    }

    public VersionPropertyNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    #if !NET8_0_OR_GREATER
    private VersionPropertyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
    #endif
}
