namespace Eksdom.Shared;

public sealed class Info : IEquatable<Info>
{
    /// <summary>
    /// Name of area
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Province of area
    /// </summary>
    public string Region { get; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} ({Region})";

    /// <summary>
    /// Constructor
    /// </summary>
    public Info(string name, string region)
    {
        Name = name;
        Region = region;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (Info)obj;
        if (instance is null)
        {
            return false;
        }

        return Name == instance.Name && 
            Region == instance.Region;
    }

    public bool Equals(Info? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Name.GetHashCode();
        hash = hash * 23 + Region.GetHashCode();
        return hash;
    }

    public static bool operator ==(Info x, Info y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(Info x, Info y)
    {
        return !(x == y);
    }
}