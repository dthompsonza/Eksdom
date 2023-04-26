namespace Eksdom.Shared;

/// <summary>
/// Loadshedding stage
/// </summary>
public sealed class Stage : SerializableMemoryFile, IEquatable<Stage>
{
    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Stage events
    /// </summary>
    public List<TimePeriod> Events { get; }

    /// <inheritdoc/>
    public override string ToString() => $"Stage {StageLevel} ({Events.Count} events)";

    /// <summary>
    /// Constructor
    /// </summary>
    public Stage(int stageLevel, List<TimePeriod> events)
    {
        StageLevel = stageLevel;
        Events = events;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + StageLevel.GetHashCode();
        hash = hash * 23 + Events.GetHashCode();
        return hash;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (Stage)obj;
        if (instance is null)
        {
            return false;
        }

        return StageLevel == instance.StageLevel && 
            Events.All(tp => instance.Events.Contains(tp));
    }

    public bool Equals(Stage? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(Stage x, Stage y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(Stage x, Stage y)
    {
        return !(x == y);
    }
}