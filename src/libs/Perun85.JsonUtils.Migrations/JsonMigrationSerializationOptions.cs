using System.Text.Json;
using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations;

/// <summary>
/// Migration serialization options.
/// </summary>
public sealed class JsonMigrationSerializationOptions
{
    /// <summary>
    /// Used in combination with <see cref="JsonDocumentOptions"/> when input JSON string is converted into <see cref="JsonNode"./>
    /// </summary>
    public JsonNodeOptions NodeOptions { get; }

    /// <summary>
    /// Used in combination with <see cref="JsonNodeOptions"/> when input JSON string is converted into <see cref="JsonNode"./>
    /// </summary>
    public JsonDocumentOptions DocumentOptions { get; }

    /// <summary>
    /// Used during final serialization of JsonNode to string.
    /// </summary>
    public JsonSerializerOptions? SerializerOptions { get; }

    /// <summary>
    /// Migration options constructor.
    /// </summary>
    /// <param name="nodeOptions">Options used in combination with <see cref="JsonDocumentOptions"/> when document content is converted from <see cref="string"/> to <see cref="JsonNode"/>.</param>
    /// <param name="documentOptions">Options used in combination with <see cref="JsonNodeOptions"/> when document content is converted from <see cref="string"/> to <see cref="JsonNode"/>.</param>
    /// <param name="serializerOptions">Options used during final serialziation when <see cref="JsonNode"/> is converted to <see cref="string"/>. </param>
    public JsonMigrationSerializationOptions(JsonNodeOptions nodeOptions = default, JsonDocumentOptions documentOptions = default, JsonSerializerOptions? serializerOptions = default)
    {
        NodeOptions = nodeOptions;
        DocumentOptions = documentOptions;
        SerializerOptions = serializerOptions;
    }
}
