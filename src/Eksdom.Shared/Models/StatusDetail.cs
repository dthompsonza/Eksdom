namespace Eksdom.Shared;

public sealed class StatusDetail : IEquatable<StatusDetail>
{
    /// <summary>
    /// Stage name (ie 'Cape Town' or 'Eskom')
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Stage updated
    /// </summary>
    public DateTimeOffset Updated { get; }

    /// <summary>
    /// Next stages
    /// </summary>
    public List<StatusNextStage> NextStages { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} (Stage {StageLevel})";

    /// <summary>
    /// Constructor
    /// </summary>
    public StatusDetail(string name,
        int stageLevel,
        DateTimeOffset updated,
        List<StatusNextStage> nextStages)
    {
        Name = name;
        StageLevel = stageLevel;
        Updated = updated;
        NextStages = nextStages;
    }

    public override bool Equals(object? obj)
    {
        if (this is null && obj is null)
        {
            return true;
        }
        if (obj is null)
        {
            return false;
        }

        var instance = (StatusDetail)obj;
        if (instance is null)
        {
            return false;
        }

        return Name == instance.Name && 
            StageLevel == instance.StageLevel && 
            Updated == instance.Updated &&
            NextStages.All(ns=>instance.NextStages.Contains(ns));
    }

    public bool Equals(StatusDetail? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Name.GetHashCode();
        hash = hash * 23 + StageLevel.GetHashCode();
        hash = hash * 23 + Updated.GetHashCode();
        hash = hash * 23 + NextStages.GetHashCode();
        return hash;
    }

    public static bool operator ==(StatusDetail x, StatusDetail y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(StatusDetail x, StatusDetail y)
    {
        return !(x == y);
    }
}