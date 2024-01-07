namespace Perun85.JsonUtils.Migrations;

/// <summary>
/// Migration result.
/// </summary>
public sealed record JsonMigrationResult
{
    /// <summary>
    /// Content of the document after migrations are applied.
    /// </summary>
    public string? DocumentContent { get; init; }

    /// <summary>
    /// Version of the document after all registered migrations are updated.
    /// </summary>
    public uint CurrentDocumentVersion { get; init; }

    /// <summary>
    /// Flag that indicates were migrations applied on the document content.
    /// </summary>
    /// <remarks>
    /// In case the value is set to false, document was not migrated because it is already on the highest possible version.
    /// </remarks>
    public bool IsDocumentMigrated { get; init; }
}
