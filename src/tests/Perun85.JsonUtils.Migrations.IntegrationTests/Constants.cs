using System.Text.Json;

namespace Perun85.JsonUtils.Migrations.IntegrationTests;

internal static class Constants
{
    internal static class Serialization
    {
        internal static readonly JsonMigrationSerializationOptions Options = new JsonMigrationSerializationOptions(
            serializerOptions: new JsonSerializerOptions { WriteIndented = true }
        );
    }

    internal static class Document
    {
        internal const string Id = "Student";

        internal static class Properties
        {
            internal static class Description
            {
                internal const string Name = "description";
                internal const string Value = "New description";
            }

            internal static class Active
            {
                internal const string Name = "active";
                internal const bool Value = true;
            }

            internal static class FirstName
            {
                internal const string Name = "firstName";
            }

            internal static class LastName
            {
                internal const string Name = "lastName";
            }

            internal static class FullName
            {
                internal const string Name = "fullName";
                internal const string ValueVersion2 = "John S. Smith";
            }

            internal static class Address
            {
                internal const string Name = "address";
                internal const string Value = """
                {
                  "street": "First Street",
                  "city": "Belgrade",
                  "country": "Serbia"
                }
                """;
            }

            internal static class CustomVersion
            {
                internal const string Name = "customVersion";
            }
        }

        internal static class Content
        {
            internal const string Valid = """
                {
                  "firstName": "John",
                  "lastName": "Smith",
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "Description",
                  "version": 0,
                  "active": true
                }
                """;

            internal const string ValidNonIndented = "{\"firstName\":\"John\",\"lastName\":\"Smith\",\"birthDate\":\"1985-01-31T09:00:00.594Z\",\"description\":\"Description\",\"version\":0,\"active\":true}";

            internal const string ValidHigherVersionPropertyThanAnyMigration = """
                {
                  "firstName": "John",
                  "lastName": "Smith",
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "Description",
                  "version": 25,
                  "active": true
                }
                """;

            internal const string ValidNoVersionProperty = """
                {
                  "firstName": "John",
                  "lastName": "Smith",
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "Description",
                  "active": true
                }
                """;

            internal const string ValidWithCustomVersionProperty = """
                {
                  "firstName": "John",
                  "lastName": "Smith",
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "Description",
                  "customVersion": 0,
                  "active": true
                }
                """;
            
            internal const string SuccessfullyMigratedToVersion1 = """
                {
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "New description",
                  "version": 1,
                  "active": true,
                  "fullName": "John Smith",
                  "address": {
                    "street": "First Street",
                    "city": "Belgrade",
                    "country": "Serbia"
                  }
                }
                """;

            internal const string SuccessfullyMigratedToVersion1CustomVersionProperty = """
                {
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "New description",
                  "customVersion": 1,
                  "active": true,
                  "fullName": "John Smith",
                  "address": {
                    "street": "First Street",
                    "city": "Belgrade",
                    "country": "Serbia"
                  }
                }
                """;

            internal const string SuccessfullyMigratedToVersion2 = """
                {
                  "birthDate": "1985-01-31T09:00:00.594Z",
                  "description": "New description",
                  "version": 2,
                  "active": true,
                  "fullName": "John S. Smith",
                  "address": {
                    "street": "First Street",
                    "city": "Belgrade",
                    "country": "Serbia"
                  }
                }
                """;
        }
    }

    internal static class Exceptions
    {
        internal static class Messages
        {
            internal const string NullOrEmptyString = "Null or empty string.";
            internal const string NullOrEmptyStringDocumentId = $"{NullOrEmptyString} (Parameter 'documentId')";
            internal const string NullOrEmptyStringCurrentDocumentContent = $"{NullOrEmptyString} (Parameter 'currentDocumentContent')";
        }
    }
}
