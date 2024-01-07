using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.UnitTests;

internal sealed class VehicleMigration_0_1 : IJsonMigration
{
    public string DocumentId => Constants.Documents.Vehicle.Id;

    public JsonMigrationVersionInfo VersionInfo => new (0, 1);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        // Does nothing.
    }
}
