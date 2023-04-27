namespace Eksdom.Shared;

/// <summary>
/// Loadshedding event
/// </summary>
public sealed class Event : IEquatable<Event>
{
    /// <summary>
    /// Event start
    /// </summary>
    public DateTimeOffset Start { get; }

    /// <summary>
    /// Event end
    /// </summary>
    public DateTimeOffset End { get; }

    /// <summary>
    /// Note (stage)
    /// </summary>
    public string Note { get; }

    /// <summary>
    /// Stage level
    /// </summary>
    public int StageLevel { get; }

    /// <summary>
    /// Length of event
    /// </summary>
    public TimeSpan Length => End - Start;

    /// <inheritdoc/>
    public override string ToString() => $"Stage {StageLevel} {Start} - {End}";

    /// <summary>
    /// Constructor
    /// </summary>
    public Event(DateTimeOffset start, DateTimeOffset end, string note, int stageLevel)
    {
        Start = start;
        End = end;
        Note = note;
        StageLevel = stageLevel;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (Event)obj;
        if (instance is null)
        {
            return false;
        }

        return Start == instance.Start && 
            End == instance.End && 
            Note == instance.Note && 
            StageLevel == instance.StageLevel;
    }

    public bool Equals(Event? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Start.GetHashCode();
        hash = hash * 23 + End.GetHashCode();
        hash = hash * 23 + Note.GetHashCode();
        hash = hash * 23 + StageLevel.GetHashCode();
        return hash;
    }

    public static bool operator ==(Event x, Event y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(Event x, Event y)
    {
        return !(x == y);
    }
}