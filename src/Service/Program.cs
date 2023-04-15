using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Integration.EskomSePush;
using EnsureThat;

namespace Eksdom.Service;

class Program
{
    static async Task Main(string[] args)
    {
        var licenceKey = Environment.GetEnvironmentVariable("EKSDOM_ESP_API_KEY", EnvironmentVariableTarget.Machine);
        Ensure.That(licenceKey).IsNotNullOrEmpty();
        var client = Client.Create(licenceKey!);

        //var allowance = await client.GetAllowanceAsync();

        var area = client.GetArea("westerncape-14-parklands");

        Console.ReadKey();
        //await Host.CreateDefaultBuilder(args)
        //    .UseWindowsService()
        //    .ConfigureServices((hostContext, services) =>
        //    {
        //        services.AddHttpClient();
        //        //services.AddHostedService<Eksdom.Service>();
        //    })
        //    .RunConsoleAsync();
    }
}