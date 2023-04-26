using Microsoft.Extensions.Hosting;
using Eksdom.Shared;
using Integration.EskomSePush;
using Eksdom.EskomSePush.Client;
using Eksdom.Service.Caching;
using EnsureThat;

namespace Eksdom.Service;

internal class Service : BackgroundService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly List<DateTime> _shutdownTimes = new List<DateTime>
        {
            // Add the shutdown times here as DateTime objects
            DateTime.Today.AddDays(1).AddHours(2), // tomorrow at 2 AM
            DateTime.Today.AddDays(2).AddHours(18), // day after tomorrow at 6 PM
            DateTime.Today.AddDays(3).AddHours(10) // 3 days from now at 10 AM
        };

    private readonly EspClient _espClient;

    public Service(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _espClient = CreateApiClient();
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Allowance? oldAllowance = null;
        AreaInformation? oldArea = null;

        while (!stoppingToken.IsCancellationRequested)
        {

            // Wait for 8 hours before making the next API call
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            Console.WriteLine("Loop");
            var allowance = _espClient.GetAllowance();

            if (allowance is not null && 
                (oldAllowance is null || allowance != oldAllowance))
            {
                oldAllowance = allowance;
                SharedData.SetAllowance(allowance);
                Console.WriteLine($"Set allowance: {allowance}");
                var a = SharedData.GetAllowance();
            }

            var area = _espClient.GetAreaInformation("westerncape-14-parklands");
            if (area is not null &&
                (oldArea is null || area != oldArea))
            {
                oldArea = area;
                SharedData.SetAreaInformation(area);
                Console.WriteLine($"Set allowance: {area}");
            }
        }
    }

    private EspClient CreateApiClient()
    {
        var licenceKey = Environment.GetEnvironmentVariable("EKSDOM_ESP_API_KEY", EnvironmentVariableTarget.Machine);
        Ensure.That(licenceKey).IsNotNullOrEmpty();
        var cache = new FileResponseCache();
        var options = new EspClientOptions(licenceKey!)
        {
            ResponseCache = cache,
        };
        var client = EspClient.Create(options);
        return client;
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
