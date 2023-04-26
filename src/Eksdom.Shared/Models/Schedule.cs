namespace Eksdom.Shared;

/// <summary>
/// Loadshedding schedule
/// </summary>
public sealed class Schedule : IEquatable<Schedule>
{
    /// <summary>
    /// Schedule days
    /// </summary>
    public List<ScheduleDay> Days { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Schedule(List<ScheduleDay> days)
    {
        Days = days;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (Schedule)obj;
        if (instance is null)
        {
            return false;
        }

        return Days.All(d => instance.Days.Contains(d));
    }

    public bool Equals(Schedule? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Days.GetHashCode();
        return hash;
    }

    public static bool operator ==(Schedule x, Schedule y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(Schedule x, Schedule y)
    {
        return !(x == y);
    }
}