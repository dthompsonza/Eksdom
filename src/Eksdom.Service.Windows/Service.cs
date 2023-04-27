using Eksdom.Client;
using Eksdom.Service.Caching;
using Eksdom.Shared;
using Eksdom.Shared.Models;
using Microsoft.Extensions.Hosting;

namespace Eksdom.Service;

internal class Service : BackgroundService
{
    private readonly DateTimeOffset _started = DateTimeOffset.Now;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly List<DateTime> _shutdownTimes = new List<DateTime>
        {
            // Add the shutdown times here as DateTime objects
            DateTime.Today.AddDays(1).AddHours(2), // tomorrow at 2 AM
            DateTime.Today.AddDays(2).AddHours(18), // day after tomorrow at 6 PM
            DateTime.Today.AddDays(3).AddHours(10) // 3 days from now at 10 AM
        };

    private EspClient _espClient;
    private string? _badLicenceKey;

    public Service(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _espClient = CheckLicenceKeyAndResetClient(null);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetServiceInfo(isRunning: false, "Service is initializing");
        Allowance? allowance = null;
        AreaInformation? areaInformation = null;
        Status? status = null;
        int count = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            if (count++ > 1)
            {
                continue;
            }
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            
            if (string.IsNullOrEmpty(LoadLicenceKey()))
            {
                SetServiceInfo(isRunning: false, "No or invalid licence key configured");
                _espClient.ClearLicenceKey();
                continue;
            }
            if (_badLicenceKey is not null && _badLicenceKey.Equals(LoadLicenceKey()))
            {
                SetServiceInfo(isRunning: false, "Licence key rejected by server");
                _espClient.ClearLicenceKey();
                continue;
            }

            _espClient = CheckLicenceKeyAndResetClient(_espClient);
            
            if (!HasEspClient())
            {
                Console.WriteLine("This should never be displayed");
                continue;
            }

            Console.WriteLine("Loop");

            try
            {
                RefreshData(allowance, areaInformation,status);
                _badLicenceKey = null;
                SetServiceInfo(isRunning: true);
            }
            catch (EksdomException eksex) when (eksex.ExceptionType == ExceptionTypes.InvalidApiKey)
            {
                _badLicenceKey = LoadLicenceKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _espClient.ClearLicenceKey();
            }
        }
    }

    private void SetServiceInfo(bool isRunning, string? notRunningReason = null)
    {
        var serviceInfo = new ServiceInfo(_started, isRunning, notRunningReason);
        SharedData.SetServiceInfo(serviceInfo);
    }

    private void RefreshData(Allowance? oldAllowance, AreaInformation? oldArea, Status? oldStatus)
    {
        var status = _espClient!.GetStatus();
        if (status is not null &&
            (oldStatus is null || status != oldStatus))
        {
            oldStatus = status;
            SharedData.SetStatus(status);
            Console.WriteLine($"Status: {status}");
        }

        var area = _espClient!.GetAreaInformation("westerncape-14-parklands");
        if (area is not null &&
            (oldArea is null || area != oldArea))
        {
            oldArea = area;
            SharedData.SetAreaInformation(area);
            Console.WriteLine($"Set allowance: {area}");
        }

        var allowance = _espClient!.GetAllowance();
        if (allowance is not null &&
            (oldAllowance is null || allowance != oldAllowance))
        {
            oldAllowance = allowance;
            SharedData.SetAllowance(allowance);
            Console.WriteLine($"Set allowance: {allowance}");
        }
    }

    private bool HasEspClient() => _espClient != null;

    private static EspClient CheckLicenceKeyAndResetClient(EspClient? current)
    {
        var licenceKey = LoadLicenceKey();

        if (current is not null)
        {
            if (!current.LicenceKeyMatches(licenceKey!))
            {
                current.SetLicenceKey(licenceKey!);
                return current;
            }
        }
        
        var cache = new FileResponseCache("DISK");
        var options = new EspClientOptions(licenceKey!)
        {
            ResponseCache = cache,
        };

        return EspClient.Create(options);
    }

    private static string? LoadLicenceKey()
    {
        var licenceKey = Environment.GetEnvironmentVariable(Constants.EnvironmentVarApiKey, EnvironmentVariableTarget.Machine);
        if (licenceKey is not null)
        {
            if (!EspClient.ValidateLicenceKey(licenceKey, out _))
            {
                Console.WriteLine("Licence key in config is not valid");
                return null;
            }
        }
        return licenceKey;
    }

    public override void Dispose()
    {
        base.Dispose();
        _espClient.Dispose();
    }
}
