namespace Eksdom.Shared;

/// <summary>
/// Current and next loadshedding status
/// </summary>
public sealed class Status : SerializableMemoryFile, IEquatable<Status>
{
    /// <summary>
    /// Eskom status
    /// </summary>
    public StatusDetail Eskom { get; }

    /// <summary>
    /// Cape Town status
    /// </summary>
    public StatusDetail CapeTown { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public Status(StatusDetail eskom, 
        StatusDetail capeTown)
    {
        Eskom = eskom;
        CapeTown = capeTown;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (Status)obj;
        if (instance is null)
        {
            return false;
        }

        return Eskom == instance.Eskom && 
            CapeTown == instance.CapeTown;
    }

    public bool Equals(Status? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Eskom.GetHashCode();
        hash = hash * 23 + CapeTown.GetHashCode();
        return hash;
    }

    public static bool operator ==(Status x, Status y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(Status x, Status y)
    {
        return !(x == y);
    }
}