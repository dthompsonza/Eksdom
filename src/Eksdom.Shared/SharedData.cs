using Eksdom.SharedMemory;

namespace Eksdom.Shared;

public static class SharedData
{
    private const string MemoryName = "Eksdom.SharedMemory.Memory";
    private static SharedMemoryAccess _sharedMemoryAccess;

    static SharedData()
    {
        _sharedMemoryAccess = new SharedMemoryAccess(MemoryName);
    }

    public static void SetAllowance(Allowance allowance)
    {
        _sharedMemoryAccess.Write(nameof(Allowance), allowance);
    }

    public static Allowance? GetAllowance()
    {
        var result = _sharedMemoryAccess.Read<Allowance>(nameof(Allowance));
        return result;
    }

    public static void SetAreaInformation(AreaInformation areaInformation)
    {
        _sharedMemoryAccess.Write(nameof(AreaInformation), areaInformation);
    }

    public static AreaInformation? GetAreaInformation()
    {
        var result = _sharedMemoryAccess.Read<AreaInformation>(nameof(AreaInformation));
        return result;
    }

    public static void SetStatus(Status status)
    {
        _sharedMemoryAccess.Write(nameof(Status), status);
    }

    public static Status? GetStatus()
    {
        var result = _sharedMemoryAccess.Read<Status>(nameof(Status));
        return result;
    }
}
