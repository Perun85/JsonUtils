using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Perun85.JsonUtils.Migrations.UnitTests;

[TestClass]
public sealed class JsonMigrationVersionInfoTests
{
    [DataRow(2, 5, true)]
    [DataRow(3, 7, true)]
    [DataRow(3, 4, true)]
    [DataRow(1, 7, true)]
    [DataRow(1, 3, true)]
    [DataRow(5, 7, false)]
    [DataRow(1, 2, false)]
    [TestMethod]
    public void RangeOverlaps_ShouldReturnCorrectValue(int initial, int final, bool expectedOverlapping)
    {
        var versionInfoFrom1to2 = new JsonMigrationVersionInfo(2, 5);
        var versionInfoFrom2to3 = new JsonMigrationVersionInfo((uint)initial, (uint)final);

        var isOverlapping = versionInfoFrom1to2.RangeOverlaps(versionInfoFrom2to3);

        Assert.AreEqual(expectedOverlapping, isOverlapping);
    }

    [TestMethod]
    public void RangeOverlaps_WhenArgumentVersionInfoIsNull_ShouldThrowArgumentNullException()
    {
        var versionInfo = new JsonMigrationVersionInfo(0, 1);
        Assert.ThrowsException<ArgumentNullException>(() => versionInfo.RangeOverlaps(null));
    }

    [TestMethod]
    public void Ctor_WhenValidArgumentAreProvided_ShouldCreateInstance()
    {
        var versionInfo = new JsonMigrationVersionInfo(1, 2);

        Assert.AreEqual((uint)1, versionInfo.Initial);
        Assert.AreEqual((uint)2, versionInfo.Final);
    }

    [TestMethod]
    [DataRow(4, 1)]
    [DataRow(1, 1)]
    public void Ctor_WhenInitialAndFinalArgumentsAreInvalid_ShouldThrowArgumentException(int initial, int final)
    {
        Assert.ThrowsException<ArgumentException>(() => new JsonMigrationVersionInfo((uint)initial, (uint)final));
    }
}
