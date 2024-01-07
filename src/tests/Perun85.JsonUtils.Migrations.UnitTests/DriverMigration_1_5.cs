using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.UnitTests;

internal sealed class DriverMigration_1_5 : IJsonMigration
{
    public string DocumentId => Constants.Documents.Driver.Id;

    public JsonMigrationVersionInfo VersionInfo => new (1, 5);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        // Does nothing.
    }
}
