namespace Eksdom.Shared;

public sealed class StatusNextStage : IEquatable<StatusNextStage>
{
    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Stage starts
    /// </summary>
    public DateTimeOffset Starts { get; }

    /// <inheritdoc/>
    public override string ToString() => $"Stage {StageLevel}";

    /// <summary>
    /// Constructor
    /// </summary>
    public StatusNextStage(int stageLevel,
        DateTimeOffset starts)
    {
        StageLevel = stageLevel;
        Starts = starts;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (StatusNextStage)obj;
        if (instance is null)
        {
            return false;
        }

        return StageLevel == instance.StageLevel && 
            Starts == instance.Starts;
    }

    public bool Equals(StatusNextStage? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + StageLevel.GetHashCode();
        hash = hash * 23 + Starts.GetHashCode();
        return hash;
    }

    public static bool operator ==(StatusNextStage x, StatusNextStage y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(StatusNextStage x, StatusNextStage y)
    {
        return !(x == y);
    }
}