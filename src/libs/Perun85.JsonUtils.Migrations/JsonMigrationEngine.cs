using JsonMigrator.Utils;
using Perun85.JsonUtils.Migrations.Exceptions;
using Perun85.JsonUtils.Migrations.Extensions;
using System.Text.Json.Nodes;

namespace Perun85.JsonUtils.Migrations;

internal sealed class JsonMigrationEngine : IJsonMigrationEngine
{
    private readonly string _versionPropertyName;
    private readonly IJsonMigrationRegistry _registry;
    private readonly JsonMigrationSerializationOptions _serializationOptions;

    internal JsonMigrationEngine(string versionPropertyName, IJsonMigrationRegistry registry, JsonMigrationSerializationOptions serializationOptions)
    {
        Arg.Guard.AgainstStringNullOrEmpty(versionPropertyName);
        Arg.Guard.AgainstNull(registry);

        _versionPropertyName = versionPropertyName;
        _serializationOptions = serializationOptions;
        _registry = registry;
    }

    JsonMigrationResult IJsonMigrationEngine.ApplyMigrations(string documentId, string currentDocumentContent)
    {
        Arg.Guard.AgainstStringNullOrEmpty(documentId);
        Arg.Guard.AgainstStringNullOrEmpty(currentDocumentContent);

        var documentJsonNode = JsonNode.Parse(currentDocumentContent, _serializationOptions.NodeOptions, _serializationOptions.DocumentOptions)!;
        var orderedMigrations = _registry.GetOrderedMigrations(documentId);

        var documentHasVersionProperty = documentJsonNode[_versionPropertyName] is not null;

        if (!documentHasVersionProperty)
            VersionPropertyNotFoundException.Throw(_versionPropertyName);

        var documentInitialVersion = documentJsonNode.GetDocumentVersion(_versionPropertyName);
        var isDocumentAlreadyAtHighestVersion = orderedMigrations[orderedMigrations.Count - 1].VersionInfo.Initial < documentInitialVersion;

        if (isDocumentAlreadyAtHighestVersion)
            return new JsonMigrationResult
            {
                DocumentContent = currentDocumentContent,
                CurrentDocumentVersion = documentInitialVersion,
                IsDocumentMigrated = false
            };

        if (!orderedMigrations.Exists(x => x.VersionInfo.Initial == documentInitialVersion))
            NoApplicableMigrationsFoundException.Throw(documentId, documentInitialVersion);

        var potentiallyApplicableMigrations = orderedMigrations.Where(x => x.VersionInfo.Initial >= documentInitialVersion).ToList();

        potentiallyApplicableMigrations.ForEach(x => documentJsonNode = ApplyMigration(x, _versionPropertyName, documentJsonNode, _serializationOptions));

        return new JsonMigrationResult
        {
            CurrentDocumentVersion = documentJsonNode.GetDocumentVersion(_versionPropertyName),
            DocumentContent = documentJsonNode.ToJsonString(_serializationOptions.SerializerOptions),
            IsDocumentMigrated = true
        };
    }

    private static JsonNode ApplyMigration(IJsonMigration migration, string versionPropertyName, JsonNode documentNode, JsonMigrationSerializationOptions serializationOptions)
    {
        var documentVersion = documentNode.GetDocumentVersion(versionPropertyName);

        if (documentVersion != migration.VersionInfo.Initial)
            return documentNode;
        
        try
        {
            migration.Apply(documentNode, serializationOptions);
            documentNode.SetDocumentVersion(versionPropertyName, migration.VersionInfo.Final);
        }
        catch (Exception ex)
        {
            ErrorApplyingMigrationException.Throw(migration.GetType().FullName!, ex);
        }
        
        return documentNode;
    }
}