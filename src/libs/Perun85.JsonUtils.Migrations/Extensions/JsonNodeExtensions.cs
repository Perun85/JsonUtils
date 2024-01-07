using JsonMigrator.Utils;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.Extensions;

public static class JsonNodeExtensions
{
    internal static uint GetDocumentVersion(this JsonNode node, string versionPropertyName)
    {
        Arg.Guard.AgainstNull(node);
        Arg.Guard.AgainstStringNullOrEmpty(versionPropertyName);

        var version = node[versionPropertyName]!.AsValue();
        return version.GetValue<uint>();
    }

    internal static void SetDocumentVersion(this JsonNode node, string versionPropertyName, uint version)
    {
        Arg.Guard.AgainstNull(node);
        Arg.Guard.AgainstStringNullOrEmpty(versionPropertyName);

        node[versionPropertyName] = version;
    }

    internal static string ToJsonString(this JsonNode node, JsonSerializerOptions? serializerOptions)
    {
        Arg.Guard.AgainstNull(node);
        return serializerOptions is null ? node.ToJsonString() : node.ToJsonString(serializerOptions);
    }

    internal static JsonNode? ParseToJsonNode(this string documentContent, JsonSerializerOptions? serializerOptions)
    {
        Arg.Guard.AgainstStringNullOrEmpty(documentContent);
        return serializerOptions is null ? JsonNode.Parse(documentContent) : JsonSerializer.SerializeToNode(documentContent, serializerOptions);
    }

    public static void Remove(this JsonNode node, string propertyName)
    {
        Arg.Guard.AgainstNull(node);
        Arg.Guard.AgainstStringNullOrEmpty(propertyName);

        var jsonObject = node.AsObject();
        jsonObject.Remove(propertyName);
    }
}
