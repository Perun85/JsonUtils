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
            throw new VersionPropertyNotFoundException($"Version property '{_versionPropertyName}' not found.");

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
            throw new NoApplicableMigrationsFoundException($"Document '{documentId}' with version '{documentInitialVersion}' has no applicable migrations.");

        foreach (var migration in orderedMigrations)
            documentJsonNode = ApplyMigrationIfVersionsMatch(migration, _versionPropertyName, documentJsonNode!, _serializationOptions);

        return new JsonMigrationResult
        {
            CurrentDocumentVersion = documentJsonNode.GetDocumentVersion(_versionPropertyName),
            DocumentContent = documentJsonNode.ToJsonString(_serializationOptions.SerializerOptions),
            IsDocumentMigrated = true
        };
    }

    private static JsonNode ApplyMigrationIfVersionsMatch(IJsonMigration migration, string versionPropertyName, JsonNode documentNode, JsonMigrationSerializationOptions serializationOptions)
    {
        var documentVersion = documentNode.GetDocumentVersion(versionPropertyName);

        if (documentVersion != migration.VersionInfo.Initial)
            throw new ApplyingIncompatibleMigrationException($"Attempting to apply migration '{migration.VersionInfo.Initial}' on the document with incompatible version '{documentVersion}'.");
        
        try
        {
            migration.Apply(documentNode, serializationOptions);
            documentNode.SetDocumentVersion(versionPropertyName, migration.VersionInfo.Final);
            return documentNode;
        }
        catch (Exception ex)
        {
            throw new ErrorApplyingMigrationException($"Failed to apply migration '{migration.GetType().FullName}'.", ex);
        }
    }
}