using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.IntegrationTests;

internal sealed class MigrationWithException : IJsonMigration
{
    public string DocumentId => Constants.Document.Id;

    public JsonMigrationVersionInfo VersionInfo => new (0, 1);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        throw new NotImplementedException();
    }
}
