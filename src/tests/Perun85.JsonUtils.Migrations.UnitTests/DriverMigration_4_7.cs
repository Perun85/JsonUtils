using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.UnitTests;

internal sealed class DriverMigration_4_7 : IJsonMigration
{
    public string DocumentId => Constants.Documents.Driver.Id;

    public JsonMigrationVersionInfo VersionInfo => new(4 , 7);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        // Does nothing.
    }
}
