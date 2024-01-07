using Perun85.JsonUtils.Migrations.Extensions;
using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations.IntegrationTests;

internal sealed class Migration_0_1 : IJsonMigration
{
    public string DocumentId => Constants.Document.Id;

    public JsonMigrationVersionInfo VersionInfo => new JsonMigrationVersionInfo(0, 1);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        // Change property value
        documentJsonNode[Constants.Document.Properties.Description.Name] = Constants.Document.Properties.Description.Value;
        documentJsonNode[Constants.Document.Properties.Active.Name] = Constants.Document.Properties.Active.Value;

        // Preserve values to create new property value
        var firstName = documentJsonNode[Constants.Document.Properties.FirstName.Name]?.GetValue<string>();
        var lastName = documentJsonNode[Constants.Document.Properties.LastName.Name]?.GetValue<string>();

        // Remove properties
        documentJsonNode.Remove(Constants.Document.Properties.FirstName.Name);
        documentJsonNode.Remove(Constants.Document.Properties.LastName.Name);
        

        // Create new property
        documentJsonNode[Constants.Document.Properties.FullName.Name] = $"{firstName} {lastName}";

        // Create new complex property
        documentJsonNode[Constants.Document.Properties.Address.Name] = JsonNode.Parse(Constants.Document.Properties.Address.Value,
            serializationOptions.NodeOptions, serializationOptions.DocumentOptions);
    }
}
