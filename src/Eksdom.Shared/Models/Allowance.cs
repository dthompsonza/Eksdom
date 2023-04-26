namespace Eksdom.Shared;

/// <summary>
/// Check API allowance
/// </summary>
public sealed class Allowance : SerializableMemoryFile, IEquatable<Allowance>
{
    /// <summary>
    /// Credit used
    /// </summary>
    public int Count { get; }

    /// <summary>
    /// Credit limit
    /// </summary>
    public int Limit { get; }

    /// <summary>
    /// Credit balance
    /// </summary>
    public int Balance => Math.Max(Limit - Count, 0);

    /// <inheritdoc/>
    public override string ToString() => $"Used: {Count} Balance: {Balance}";

    /// <summary>
    /// Constructor
    /// </summary>
    public Allowance(int count, int limit)
    {
        Count = count;
        Limit = limit;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (Allowance)obj;
        if (instance is null)
        {
            return false;
        }

        return Count == instance.Count && 
            Limit == instance.Limit;
    }

    public bool Equals(Allowance? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Count.GetHashCode();
        hash = hash * 23 + Limit.GetHashCode();
        hash = hash * 23 + Balance.GetHashCode();
        return hash;
    }

    public static bool operator ==(Allowance x, Allowance y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(Allowance x, Allowance y)
    {
        return !(x == y);
    }
}
