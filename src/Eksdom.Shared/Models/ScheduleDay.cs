namespace Eksdom.Shared;

/// <summary>
/// Loadshedding schedule for a day
/// </summary>
public sealed class ScheduleDay : IEquatable<ScheduleDay>
{
    /// <summary>
    /// Schedule name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Schedule date
    /// </summary>
    public DateOnly Date { get; }

    /// <summary>
    /// Schedule stages
    /// </summary>
    public List<Stage> Stages { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} {Date:dd MMM yyyy}";

    /// <summary>
    /// Constructor
    /// </summary>
    public ScheduleDay(string name, DateOnly date, List<Stage> stages)
    {
        Name = name;
        Date = date;
        Stages = stages;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (ScheduleDay)obj;
        if (instance is null)
        {
            return false;
        }

        return Name == instance.Name && 
            Date == instance.Date &&
            Stages.All(s => instance.Stages.Contains(s));
    }

    public bool Equals(ScheduleDay? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Name.GetHashCode();
        hash = hash * 23 + Date.GetHashCode();
        hash = hash * 23 + Stages.GetHashCode();
        return hash;
    }

    public static bool operator ==(ScheduleDay x, ScheduleDay y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(ScheduleDay x, ScheduleDay y)
    {
        return !(x == y);
    }
}