using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.UnitTests;

internal sealed class DriverMigration_2_3 : IJsonMigration
{
    public string DocumentId => Constants.Documents.Driver.Id;

    public JsonMigrationVersionInfo VersionInfo => new(2, 3);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        // Does nothing.
    }
}
