using Microsoft.VisualStudio.TestTools.UnitTesting;
using Perun85.JsonUtils.Migrations.Exceptions;

namespace Perun85.JsonUtils.Migrations.UnitTests;

[TestClass]
public sealed class JsonMigrationRegistryUnitTests
{
    private readonly IJsonMigrationRegistry _registry = new JsonMigrationRegistry();

    [TestMethod]
    public void Register_WhenMigrationIsNull_ShouldThrowArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _registry.Register(null));
    }

    [TestMethod]
    public void Register_WhenValidMigrationsAreSupplied_ShouldRegisterWithoutThrowingException()
    {
        _registry.Register(new DriverMigration_0_1());
        _registry.Register(new DriverMigration_1_5());
        _registry.Register(new VehicleMigration_0_1());
        _registry.Register(new VehicleMigration_1_5());

        var driverMigrations = _registry.GetOrderedMigrations(Constants.Documents.Driver.Id);
        var vehicleMigrations = _registry.GetOrderedMigrations(Constants.Documents.Vehicle.Id);

        Assert.AreEqual(2, driverMigrations.Count);
        Assert.AreEqual(2, vehicleMigrations.Count);
    }

    [TestMethod]
    public void Register_WhenOverlappingMigrationIsSupplied_ShouldThrowMigrationRangeOverlappingException()
    {
        Assert.ThrowsException<MigrationRangeOverlappingException>(() =>
        {
            _registry.Register(new DriverMigration_0_1());
            _registry.Register(new DriverMigration_2_3());
            _registry.Register(new DriverMigration_1_5());
        });
    }

    [TestMethod]
    public void GetOrderedMigrations_WhenValidMigrationsAreRegistered_ReturnsMigrationsInCorrectOrderTheyShouldBeApplied()
    {
        _registry.Register(new DriverMigration_0_1());
        _registry.Register(new DriverMigration_2_3());
        _registry.Register(new DriverMigration_4_7());

        _registry.Register(new VehicleMigration_0_1());
        _registry.Register(new VehicleMigration_2_3());

        var driverMigrations = _registry.GetOrderedMigrations(Constants.Documents.Driver.Id);
        var vehicleMigrations = _registry.GetOrderedMigrations(Constants.Documents.Vehicle.Id);

        Assert.AreEqual((uint)0, driverMigrations[0].VersionInfo.Initial);
        Assert.AreEqual((uint)1, driverMigrations[0].VersionInfo.Final);
        Assert.AreEqual((uint)2, driverMigrations[1].VersionInfo.Initial);
        Assert.AreEqual((uint)3, driverMigrations[1].VersionInfo.Final);
        Assert.AreEqual((uint)4, driverMigrations[2].VersionInfo.Initial);
        Assert.AreEqual((uint)7, driverMigrations[2].VersionInfo.Final);

        Assert.AreEqual((uint)0, vehicleMigrations[0].VersionInfo.Initial);
        Assert.AreEqual((uint)1, vehicleMigrations[0].VersionInfo.Final);
        Assert.AreEqual((uint)2, vehicleMigrations[1].VersionInfo.Initial);
        Assert.AreEqual((uint)3, vehicleMigrations[1].VersionInfo.Final);
    }
}