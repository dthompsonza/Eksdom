using System.Text.Json;

namespace Eksdom.Shared;

public static class Config
{
    private static string? _licenceKey;

    private static string? _areaId;

    private static AreaOverrides? _override;

    public static string? LicenceKey
    {
        get { Load(); return _licenceKey; }
        set { _licenceKey = value; Save(); }
    }

    public static string? AreaId
    {
        get { Load(); return _areaId; }
        set { _areaId = value; Save(); }
    }

    public static AreaOverrides Override
    {
        get { Load(); return _override ?? AreaOverrides.National; }
        set
        {
            if (_areaId is null) throw new InvalidOperationException("Set AreaId before setting override");
            _override = value;
            Save();
        }
    }

    public static bool IsValid()
    {
        Load();
        return LicenceKey is not null && AreaId is not null;
    }

    private static void Load()
    {
        var licenceKey = Environment.GetEnvironmentVariable(GlobalConstants.EnvironmentVarApiKey, EnvironmentVariableTarget.Machine);
        if (licenceKey is not null)
        {
            licenceKey.ValidateLicenceKey(out _licenceKey);
        }
        var areaIdJson = Environment.GetEnvironmentVariable(GlobalConstants.EnvironmentAreaId, EnvironmentVariableTarget.Machine);
        if (areaIdJson is not null)
        {
            var areaId = JsonSerializer.Deserialize<AreaIdDto>(areaIdJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            _areaId = areaId!.Id;
            _override = AreaOverrides.National;
            if (Enum.TryParse<AreaOverrides>(areaId!.Override, out var @override))
            {
                _override = @override;
            }

        }
    }

    private static void Save()
    {
        Environment.SetEnvironmentVariable(GlobalConstants.EnvironmentVarApiKey, _licenceKey, EnvironmentVariableTarget.Machine);

        string? areaIdJson = null;
        if (_areaId is not null)
        {
            var areaId = new AreaIdDto
            {
                Id = _areaId!,
                Override = _override.ToString(),
            };
            areaIdJson = JsonSerializer.Serialize(areaId);
        }

        Environment.SetEnvironmentVariable(GlobalConstants.EnvironmentAreaId, areaIdJson, EnvironmentVariableTarget.Machine);
    }

    class AreaIdDto
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string? Override { get; set; }
    }
}
