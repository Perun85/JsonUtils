namespace Perun85.JsonUtils.Migrations;

/// <summary>
/// Engine that applies registered migration to the document.
/// </summary>
public interface IJsonMigrationEngine
{
    /// <summary>
    /// Applies registered migration to the document.
    /// </summary>
    /// <param name="documentId">Document identifier.</param>
    /// <param name="currentDocumentContent">Contentn of the JSON document before migrations are applied.</param>
    /// <returns>JSON migration result <see cref="JsonMigrationResult"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="documentId"/> or <paramref name="currentDocumentContent"/> is null or empty string..
    /// </exception>
    /// <exception cref="NoApplicableMigrationsFoundException">
    /// Thrown when there are no migrations found where <see cref="JsonMigrationVersionInfo.Initial"/> matches current version of the document.
    /// This does not apply to the situation where document is already at version that is higher or equal to the one of highest registered migration. 
    /// In that case property <see cref="JsonMigrationResult.IsDocumentMigrated"/> will be set to <see cref="false"/> and unmodified document content and current version will be returned.
    /// </exception>
    /// <exception cref="ErrorApplyingMigrationException">
    /// Throw when the engine experiences an error during application of the migration.
    /// Exception caused by migration is placed inside the <see cref="Exception.InnerException"/> property.
    /// </exception>
    /// <exception cref="VersionPropertyNotFoundException">
    /// Throw when document does not contain property with information of its current version.
    /// </exception>
    public JsonMigrationResult ApplyMigrations(string documentId, string currentDocumentContent);
}