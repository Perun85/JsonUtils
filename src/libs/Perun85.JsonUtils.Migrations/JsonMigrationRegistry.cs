using JsonMigrator.Utils;
using Perun85.JsonUtils.Migrations.Exceptions;

namespace Perun85.JsonUtils.Migrations;

internal sealed class JsonMigrationRegistry : IJsonMigrationRegistry
{
    private readonly List<IJsonMigration> _migrations = new();

    void IJsonMigrationRegistry.Register(IJsonMigration migration)
    {
        Arg.Guard.AgainstNull(migration);

        var orderedMigrations = (this as IJsonMigrationRegistry).GetOrderedMigrations(migration.DocumentId);

        if (orderedMigrations.Exists(registeredMigration => registeredMigration.VersionInfo.RangeOverlaps(migration.VersionInfo)))
            MigrationRangeOverlappingException.Throw();
        
        _migrations.Add(migration);
    }

    List<IJsonMigration> IJsonMigrationRegistry.GetOrderedMigrations(string documentId)
    {
        Arg.Guard.AgainstStringNullOrEmpty(documentId);

        return _migrations
            .Where(m => m.DocumentId.Equals(documentId, StringComparison.Ordinal))
            .OrderBy(m => m.VersionInfo.Initial).ToList();
    }
}