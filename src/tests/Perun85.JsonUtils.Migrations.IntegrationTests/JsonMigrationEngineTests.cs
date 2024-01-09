using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perun85.JsonUtils.Migrations.Exceptions;
using System.Text.Json;

namespace Perun85.JsonUtils.Migrations.IntegrationTests;

[TestClass]
public sealed class JsonMigrationEngineTests
{
    [TestMethod]
    public void ApplyMigrations_WhenValidJsonAndValidMigrationsAreSupplied_ShouldMigrateSuccessfully()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithSerializationOptions(Constants.Serialization.Options)
            .WithMigration(new Migration_0_1())
            .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id, Constants.Document.Content.Valid);

        Assert.AreEqual((uint)1, migrationResult.CurrentDocumentVersion);
        Assert.AreEqual(Constants.Document.Content.SuccessfullyMigratedToVersion1, migrationResult.DocumentContent);
        Assert.IsTrue(migrationResult.IsDocumentMigrated);
    }

    [TestMethod]
    public void ApplyMigrations_WhenThereAreNoApplicableMigrationsForInitialDocumentVersion_ShouldThrowNoApplicableMigrationFoundException()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new Migration_1_2())
            .Build();

        Assert.ThrowsException<NoApplicableMigrationsFoundException>(() => engine.ApplyMigrations(
            Constants.Document.Id,
            Constants.Document.Content.Valid
        ));
    }

    [TestMethod]
    public void ApplyMigrations_WhenDocumentIsOnVersionHigherThanAnyRegisteredMigration_ShouldNotMigrate()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new Migration_0_1())
            .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id,
            Constants.Document.Content.ValidHigherVersionPropertyThanAnyMigration);

        Assert.IsFalse(migrationResult.IsDocumentMigrated);
        Assert.AreEqual((uint)25, migrationResult.CurrentDocumentVersion);
        Assert.AreEqual(Constants.Document.Content.ValidHigherVersionPropertyThanAnyMigration, migrationResult.DocumentContent);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void ApplyMigrations_WhenArgumentDocumentIdIsMissing_ShouldThrowArgumentException(string documentId)
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new Migration_0_1())
            .Build();

        var exception = Assert.ThrowsException<ArgumentException>(() => engine.ApplyMigrations(
            documentId, 
            Constants.Document.Content.Valid
        ));

        Assert.AreEqual(Constants.Exceptions.Messages.NullOrEmptyStringDocumentId, exception.Message);
    }

    [TestMethod]
    [DataRow(null)]
    [DataRow("")]
    public void ApplyMigrations_WhenArgumentCurrentDocumentContentIsNullOrEmptyString_ShouldThrowStringNullOrEmptyArgException(string documentContent)
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new Migration_0_1())
            .Build();

        var exception = Assert.ThrowsException<ArgumentException>(() => engine.ApplyMigrations(Constants.Document.Id, documentContent));

        Assert.AreEqual(Constants.Exceptions.Messages.NullOrEmptyStringCurrentDocumentContent, exception.Message);
    }

    [TestMethod]
    public void ApplyMigrations_WhenDocumentContentDoesNotContainVersionProperty_ShouldThrowVersionPropertyNotFoundException()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new Migration_0_1())
            .Build();

        Assert.ThrowsException<VersionPropertyNotFoundException>(() => engine.ApplyMigrations(
            Constants.Document.Id, 
            Constants.Document.Content.ValidNoVersionProperty
        ));
    }

    [TestMethod]
    public void ApplyMigrations_WhenCustomVersionPropertyIsUsed_ShouldMigrateSuccessfully()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithSerializationOptions(Constants.Serialization.Options)
            .WithVersionPropertyName(Constants.Document.Properties.CustomVersion.Name)
            .WithMigration(new Migration_0_1())
            .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id, Constants.Document.Content.ValidWithCustomVersionProperty);

        Assert.AreEqual((uint)1, migrationResult.CurrentDocumentVersion);
        Assert.AreEqual(Constants.Document.Content.SuccessfullyMigratedToVersion1CustomVersionProperty, migrationResult.DocumentContent);
        Assert.IsTrue(migrationResult.IsDocumentMigrated);
    }

    [TestMethod]
    public void ApplyMigrations_WhenCustomSerializationOptionsAreSupplied_ShouldApplyThemDuringMigration()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new Migration_0_1())
            .WithSerializationOptions(new JsonMigrationSerializationOptions(serializerOptions: new JsonSerializerOptions { WriteIndented = true }))
            .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id, Constants.Document.Content.ValidNonIndented);

        Assert.AreEqual(Constants.Document.Content.SuccessfullyMigratedToVersion1, migrationResult.DocumentContent);
        Assert.AreEqual((uint)1, migrationResult.CurrentDocumentVersion);
        Assert.IsTrue(migrationResult.IsDocumentMigrated);
    }

    [TestMethod]
    public void ApplyMigrations_WhenMultipleMigrationsAreRegistered_ShouldBeAppliedCorrectly()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithSerializationOptions(Constants.Serialization.Options)
            .WithMigration(new Migration_0_1())
            .WithMigration(new Migration_1_2())
            .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id, Constants.Document.Content.Valid);

        Assert.AreEqual((uint)2, migrationResult.CurrentDocumentVersion);
        Assert.AreEqual(Constants.Document.Content.SuccessfullyMigratedToVersion2, migrationResult.DocumentContent);
        Assert.IsTrue(migrationResult.IsDocumentMigrated);
    }

    [TestMethod]
    public void ApplyMigrations_WhenDocumentIsAlreadyAtHighestRegisteredVersion_ShouldNotApplyMigrations()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithSerializationOptions(Constants.Serialization.Options)
            .WithMigration(new Migration_0_1())
            .WithMigration(new Migration_1_2())
            .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id, Constants.Document.Content.SuccessfullyMigratedToVersion2);

        Assert.AreEqual((uint)2, migrationResult.CurrentDocumentVersion);
        Assert.AreEqual(Constants.Document.Content.SuccessfullyMigratedToVersion2, migrationResult.DocumentContent);
        Assert.IsFalse(migrationResult.IsDocumentMigrated);
    }

    [TestMethod]
    public void ApplyMigrations_WhenOnlySubsetOfRegisteredMigrationsNeedsToBeApplied_ShouldMigrateDocumentCorrectly()
    {
        var engine = new JsonMigrationEngineBuilder()
           .WithSerializationOptions(Constants.Serialization.Options)
           .WithMigration(new Migration_0_1())
           .WithMigration(new Migration_1_2())
           .Build();

        var migrationResult = engine.ApplyMigrations(Constants.Document.Id, Constants.Document.Content.SuccessfullyMigratedToVersion1);

        Assert.AreEqual((uint)2, migrationResult.CurrentDocumentVersion);
        Assert.AreEqual(Constants.Document.Content.SuccessfullyMigratedToVersion2, migrationResult.DocumentContent);
        Assert.IsTrue(migrationResult.IsDocumentMigrated);
    }

    [TestMethod]
    public void ApplyMigrations_WhenMigrationThrowsException_ShouldThrowErrorApplyingMigrationExceptionWithInnerExceptionPropertySetToOriginalOne()
    {
        var engine = new JsonMigrationEngineBuilder()
            .WithMigration(new MigrationWithException())
            .Build();

        var exception = Assert.ThrowsException<ErrorApplyingMigrationException>(() => engine.ApplyMigrations(
            Constants.Document.Id,
            Constants.Document.Content.Valid
        ));

        Assert.IsTrue(exception.InnerException is NotImplementedException);
    }
}