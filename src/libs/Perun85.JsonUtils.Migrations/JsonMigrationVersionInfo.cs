using JsonMigrator.Utils;

namespace Perun85.JsonUtils.Migrations;

/// <summary>
/// Abstraction that contains migration version details <see cref="Initial"/> and <see cref="Final"/>.
/// </summary>
///<remarks>Overlapping version ranges are not allowed.</remarks>
public sealed class JsonMigrationVersionInfo
{
    /// <summary>
    /// Version of the document to which this migration will be applied to.
    /// </summary>
    public uint Initial { get; }
    
    /// <summary>
    /// Version of the document after migrations are applied.
    /// </summary>
    public uint Final { get; }

    /// <summary>
    /// Creates new instance of <see cref="JsonMigrationVersionInfo"./>
    /// </summary>
    /// <param name="initial">Version of the document to which this migration will be applied to.</param>
    /// <param name="final">Version of the document after migrations are applied.</param>
    /// <exception cref="ArgumentException">Thrown in case argument <see cref="initial"/> is equal or grater than <see cref="final"/>.</exception>
    public JsonMigrationVersionInfo(uint initial, uint final)
    {
        Arg.Guard.AgainstGreaterOrEqualValue(initial, final);

        Initial = initial;
        Final = final;
    }

    /// <summary>
    /// Checks is range from another instance overlapping with its own.
    /// </summary>
    /// <param name="other">Version to compare overlap to.</param>
    /// <returns>Range overlap indicator.</returns>
    public bool RangeOverlaps(JsonMigrationVersionInfo other)
    {
        Arg.Guard.AgainstNull(other);

        var otherIsCoveringSameRange = Initial == other.Initial && Final == other.Final;
        var otherIsCoveringRightPartOfRange = other.Initial > Initial && other.Initial < Final;
        var otherIsInsideRangeOfThis = other.Initial > Initial && other.Final < Final;
        var thisIsInsideRangeOfOther = Initial > other.Initial && Final < other.Final;
        var otherIsCoveringLeftPartOfRange = other.Initial < Initial && (other.Final > Initial && other.Final <= Final);

        return otherIsCoveringSameRange || otherIsCoveringRightPartOfRange || otherIsInsideRangeOfThis || thisIsInsideRangeOfOther || otherIsCoveringLeftPartOfRange;
    }
}