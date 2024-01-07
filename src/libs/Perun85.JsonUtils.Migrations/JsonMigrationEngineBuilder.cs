using JsonMigrator.Utils;

namespace Perun85.JsonUtils.Migrations;

/// <summary>
/// Utility for building instances of <see cref="IJsonMigrationEngine"/> instance.
/// </summary>
public sealed class JsonMigrationEngineBuilder
{
    private readonly IJsonMigrationRegistry _registry = new JsonMigrationRegistry();
    private string _versionPropertyName = JsonMigratorConstants.DefaultVersionPropertyName;
    private JsonMigrationSerializationOptions _serializationOptions = new();

    /// <summary>
    /// When used allows library user to override default version property name.
    /// </summary>
    /// <param name="versionPropertyName">Version property name.</param>
    /// <returns>The <see cref="JsonMigrationEngineBuilder"/> instance.</returns>
    public JsonMigrationEngineBuilder WithVersionPropertyName(string versionPropertyName)
    {
        Arg.Guard.AgainstStringNullOrEmpty(versionPropertyName);
        _versionPropertyName = versionPropertyName;
        return this;
    }

    /// <summary>
    /// Registers migration with the engine.
    /// </summary>
    /// <param name="migration">The migration to be registered.</param>
    /// <returns>The <see cref="JsonMigrationEngineBuilder"/> instance.</returns>
    public JsonMigrationEngineBuilder WithMigration(IJsonMigration migration)
    {
        Arg.Guard.AgainstNull(migration);
        _registry.Register(migration);
        return this;
    }

    /// <summary>
    /// Allows registration of specific serialization options that will be used instead of the default ones.
    /// </summary>
    /// <param name="serializationOptions">Custom serialization options.</param>
    /// <returns>The <see cref="JsonMigrationEngineBuilder"/> instance.</returns>
    public JsonMigrationEngineBuilder WithSerializationOptions(JsonMigrationSerializationOptions serializationOptions)
    {
        Arg.Guard.AgainstNull(serializationOptions);
        _serializationOptions = serializationOptions;
        return this;
    }

    /// <summary>
    /// Builds the <see cref="IJsonMigrationEngine"/> instance.
    /// </summary>
    /// <returns><see cref="IJsonMigrationEngine"/> instance.</returns>
    public IJsonMigrationEngine Build() => new JsonMigrationEngine(_versionPropertyName, _registry, _serializationOptions);
}
