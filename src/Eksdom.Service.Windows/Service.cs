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

    private EspClient? _espClient;
    private string? _badLicenceKey;

    public Service(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _espClient = CheckLicenceKeyAndResetClient(null);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _espClient?.Dispose();
        return base.StopAsync(cancellationToken);
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

            if (!Config.IsValid())
            {
                SetServiceInfo(isRunning: false, "Licence key and Area id are not configured");
                _espClient?.ClearLicenceKey();
                continue;
            }

            if (_badLicenceKey is not null && _badLicenceKey.Equals(Config.LicenceKey))
            {
                SetServiceInfo(isRunning: false, "Licence key rejected by server");
                _espClient?.ClearLicenceKey();
                continue;
            }

            _espClient = CheckLicenceKeyAndResetClient(_espClient);

            if (_espClient is null)
            {
                continue;
            }

            Console.WriteLine("Loop");

            try
            {
                RefreshData(allowance, areaInformation, status);
                _badLicenceKey = null;
                SetServiceInfo(isRunning: true, 
                    areaId: Config.AreaId, 
                    @override: Config.Override);
            }
            catch (EksdomException eksex) when (eksex.ExceptionType == ExceptionTypes.InvalidApiKey)
            {
                _badLicenceKey = Config.LicenceKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    private void SetServiceInfo(bool isRunning, string? notRunningReason = null, string? areaId = null, AreaOverrides? @override = null)
    {
        var serviceInfo = new ServiceInfo(_started, isRunning, notRunningReason, areaId, @override);
        SharedData.SetServiceInfo(serviceInfo);
    }


    private void RefreshData(Allowance? oldAllowance, AreaInformation? oldArea, Status? oldStatus)
    {
        if (!Config.IsValid())
        {
            return;
        }

        var status = _espClient!.GetStatus();
        if (status is not null &&
            (oldStatus is null || status != oldStatus))
        {
            oldStatus = status;
            SharedData.SetStatus(status);
            Console.WriteLine($"Status: {status}");
        }

        var area = _espClient!.GetAreaInformation(Config.AreaId!);
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

    private static EspClient? CheckLicenceKeyAndResetClient(EspClient? current)
    {
        if (current is not null)
        {
            if (!Config.IsValid())
            {
                current.Dispose();
                return null;
            }

            if (!current.LicenceKeyMatches(Config.LicenceKey!))
            {
                current.SetLicenceKey(Config.LicenceKey!);
            }
            return current;
        }

        var options = new EspClientOptions(Config.LicenceKey!)
        {
            ResponseCache = new FileResponseCache("DISK"),
        };

        return EspClient.Create(options);
    }

    public override void Dispose()
    {
        base.Dispose();
        _espClient?.Dispose();
    }
}

