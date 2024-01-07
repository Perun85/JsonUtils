# Perun85.JsonUtils

## Perun85.JsonUtils.Migrations

## Description
Library built on top of the System.Text.Json that allows you to upgrade JSON object schemas using database like migrations.

## Basics 

### Components
- **IJsonMigration** -> Interface that migration class has to implement
- **IJsonMigrationEngine** -> abstraction that applies registered migrations

### Concepts
- **Versioning** -> In order for this library to be used, your JSON objects must have **uint** property that represents its version. By default **IMigrationEngine** will look for property with the name **version**. If your version property has different name, it can be specified during the engine instance creation.

- **Document ID** -> Single instance of **IMigrationEngine** can be used to apply migrations on multiple document types. It is mandatory in the migration class to specify the ID of the document on whom that migration can be applied. E.g. if your application has two JSON object types that represent students and professors, you would use ID **'Student'** for all migrations that are going to be applied on the JSON objects that represent student, and ID **'Professor'** on ones for professors. ID can be any string appart from empty.


### Usage examples

#### Add NuGet package
```powershell
dotnet add Perun85.JsonUtils.Migrations
```

#### Basic
In this example no custom version property nor serialization options are used. Serialization process is using the defaults defined by System.Text.Json.

```javascript
// Version 1
{
  "firstName": "John",
  "lastName": "Smith",
  "version": 1
}
```
```javascript
// Version 2
{
  "fullName": "John Smith",
  "version": 2
}
```

```csharp
public sealed class PersonMigration_1_2 : IJsonMigration 
{
    public string DocumentId => "Person";

    public JsonMigrationVersionInfo VersionInfo => new JsonMigrationVersionInfo(1, 2);

    public void Apply(JsonNode documentJsonNode, JsonMigrationSerializationOptions serializationOptions)
    {
        var firstName = documentJsonNode["firstName"]?.GetValue<string>();
        var lastName = documentJsonNode["lastName"]?.GetValue<string>();

        // Remove properties (extension method defined in Perun85.JsonUtils.Migrations.Extensions)
        documentJsonNode.Remove("firstName");
        documentJsonNode.Remove("lastName");
        
        // Create new property
        documentJsonNode["fullName"] = $"{firstName} {lastName}";
    }
}

var engine = new JsonMigrationEngineBuilder()
     .WithMigration(new PersonMigration_1_2())
     .Build();

var migrationResult = engine.ApplyMigrations("Person", personJsonV1);

// Flag that indicates were migrations applied on the document content.
migrationResult.IsDocumentMigrated

// Content of the document after migrations are applied.
migrationResult.DocumentContent

// Version of the document after all registered migrations are updated.
migrationResult.CurrentDocumentVersion
```

#### Custom version property
In case that the document already has defined version property, specify it in the engine definition. 
(This will override version property name for all document types registered in the engine)

```javascript
// Version 1
{
  "firstName": "John",
  "lastName": "Smith",
  "schemaVersion": 1
}
```
```javascript
// Version 2
{
  "fullName": "John Smith",
  "schemaVersion": 2
}
```

```csharp
var engine = new JsonMigrationEngineBuilder()
     .WithVersionPropertyName("schemaVersion")
     .WithMigration(new PersonMigration_1_2())
     .Build();

var migrationResult = engine.ApplyMigrations("Person", personJsonV1);
```

#### Custom serialization options
If there is a need for more control over serialization process, custom JsonMigrationSerializationOptions can be specified in the engine definition.

```csharp
var options = new JsonMigrationSerializationOptions (
    serializerOptions: new JsonSerializerOptions { WriteIndented = true }
);

var engine = new JsonMigrationEngineBuilder()
     .WithSerializationOptions(options)
     .WithVersionPropertyName("schemaVersion")
     .WithMigration(new PersonMigration_1_2())
     .Build();

var migrationResult = engine.ApplyMigrations("Person", personJsonV1);
```

#### For more examples please check out integration tests.