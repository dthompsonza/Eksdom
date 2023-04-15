using Microsoft.Extensions.Hosting;

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

    public Service(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Make the RESTful API call here
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.BaseAddress = new Uri("https://developer.sepush.co.za/");
                var response = await httpClient.GetAsync("business/2.0/status");

                if (!response.IsSuccessStatusCode)
                {
                    // Retry the API call once
                    response = await httpClient.GetAsync("business/2.0/status");

                    if (!response.IsSuccessStatusCode)
                    {
                        // Log the error or take other actions
                    }
                }
            }

            // Wait for 8 hours before making the next API call
            await Task.Delay(TimeSpan.FromHours(8), stoppingToken);
        }
    }


}
