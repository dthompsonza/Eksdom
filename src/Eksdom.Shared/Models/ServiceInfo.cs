namespace Eksdom.Shared.Models;

public class ServiceInfo : SerializableMemoryFile, IEquatable<ServiceInfo>
{
    public DateTimeOffset Started { get; }

    public bool IsRunning { get; }

    public string? NotRunningReason { get; }

    public ServiceInfo(DateTimeOffset started, bool isRunning, string? notRunningReason)
    {
        Started = started;
        IsRunning = isRunning;
        NotRunningReason = notRunningReason;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        var instance = (ServiceInfo)obj;
        if (instance is null)
        {
            return false;
        }

        return Started.Equals(instance.Started) &&
            IsRunning == instance.IsRunning &&
            NotRunningReason == instance.NotRunningReason;
    }

    public bool Equals(ServiceInfo? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Started.GetHashCode();
        hash = hash * 23 + IsRunning.GetHashCode();
        hash = hash * 23 + NotRunningReason.GetHashCode();
        return hash;
    }

    public static bool operator ==(ServiceInfo x, ServiceInfo y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;

        return x.Equals(y);
    }

    public static bool operator !=(ServiceInfo x, ServiceInfo y)
    {
        return !(x == y);
    }
}
