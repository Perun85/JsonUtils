using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.IntegrationTests;

internal sealed class Migration_1_2 : IJsonMigration
{
    public string DocumentId => Constants.Document.Id;

    public JsonMigrationVersionInfo VersionInfo => new(1, 2);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        documentJsonNode[Constants.Document.Properties.FullName.Name] = Constants.Document.Properties.FullName.ValueVersion2;
    }
}
