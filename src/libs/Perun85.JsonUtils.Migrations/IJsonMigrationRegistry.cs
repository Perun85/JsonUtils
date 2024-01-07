namespace Perun85.JsonUtils.Migrations;

internal interface IJsonMigrationRegistry
{
    void Register(IJsonMigration migration);

    List<IJsonMigration> GetOrderedMigrations(string documentId);
}
