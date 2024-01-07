using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations;

/// <summary>
/// Interface that represents JSON migration.
/// </summary>
public interface IJsonMigration
{
    /// <summary>
    /// Document identifier used for grouping of migrations.
    /// </summary>
    string DocumentId { get; }

    /// <summary>
    /// Migration version information.
    /// </summary>
    JsonMigrationVersionInfo VersionInfo { get; }

    /// <summary>
    /// Upgrades document to version specified by <see cref="JsonMigrationVersionInfo.Final"/>.
    /// </summary>
    /// <param name="documentJsonNode"><see cref="JsonNode"/> Object that represents version of JSON document defined by <see cref="JsonMigrationVersionInfo.Initial"/>.</param>
    /// <param name="serializationOptions">Serialization options used during migration process.</param>
    void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions);
}
