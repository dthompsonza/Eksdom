namespace Eksdom.Shared;

/// <summary>
/// Area Information
/// </summary>
public sealed class AreaInformation : SerializableMemoryFile, IEquatable<AreaInformation>
{
    /// <summary>
    /// Area Information
    /// </summary>
    public Info Info { get; set; }

    /// <summary>
    /// Events
    /// </summary>
    public List<Event> Events { get; set; }

    /// <summary>
    /// Schedule
    /// </summary>
    public Schedule Schedule { get; set; }

    /// <inheritdoc/>
    public override string ToString() => $"{Info.Name} ({Info.Region})";

    

    /// <summary>
    /// Constructor
    /// </summary>
    public AreaInformation(Info info, List<Event> events, Schedule schedule)
    {
        Info = info;
        Events = events;
        Schedule = schedule;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (AreaInformation)obj;
        if (instance is null)
        {
            return false;
        }

        return Info == instance.Info &&
            Events.All(e => instance.Events.Contains(e) &&
            Schedule == instance.Schedule);
    }

    public bool Equals(AreaInformation? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Info.GetHashCode();
        hash = hash * 23 + Events.GetHashCode();
        hash = hash * 23 + Schedule.GetHashCode();
        return hash;
    }

    public static bool operator ==(AreaInformation x, AreaInformation y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(AreaInformation x, AreaInformation y)
    {
        return !(x == y);
    }
}