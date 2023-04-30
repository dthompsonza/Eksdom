using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EnsureThat;
using Microsoft.Extensions.Caching.Memory;
using Eksdom.Client;
using Eksdom.Service.Caching;
using CliWrap;
using System.ServiceProcess;

namespace Eksdom.Service;

class Program
{
    const string ServiceName = "Eksdom Service";

    static async Task Main(string[] args)
    {
        //var licenceKey = Environment.GetEnvironmentVariable(Constants.EnvironmentVarApiKey, EnvironmentVariableTarget.Machine);
        //Ensure.That(licenceKey).IsNotNullOrEmpty();
        //var cache = new FileResponseCache();
        //var options = new ApiClientOptions(licenceKey!)
        //{
        //    ResponseCache = cache,
        //};
        //var client = ApiClient.Create(options);

        //var allowance = await client.GetAllowanceAsync();

        //var allowance = client.GetAllowance();
        //Console.WriteLine($"Before any calls - {allowance}");
        //var area = client.GetAreaInformation("westerncape-14-parklands");
        //Console.WriteLine(area);
        //allowance = client.GetAllowance();
        //Console.WriteLine($"After 1 call - {allowance}");
        //var area2 = client.GetAreaInformation("westerncape-14-parklands");
        //Console.WriteLine(area2);

        //allowance = client.GetAllowance();
        //Console.WriteLine($"After 2 calls - {allowance}");
        //Console.ReadKey();

        //var m = new MemoryCache(new MemoryCacheOptions());

        ////////////////////////////////
        ///

        if (args is { Length: 1 })
        {
            await SetupService(args);
            return;
        }

        await Host.CreateDefaultBuilder(args)
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddHostedService<Eksdom.Service.Service>();
            })
            .RunConsoleAsync();

        ////////////////////////////////

    }

    static async Task SetupService(string[] args)
    {
        var executablePath = Path.Combine(AppContext.BaseDirectory, "Eksdom.Service.Windows.exe");
        if (args[0] is "/Install")
        {
            await Cli.Wrap("sc")
                .WithArguments(new[] { "create", ServiceName, $"binPath={executablePath}", "start=auto" })
                .ExecuteAsync();
        }
        else if (args[0] is "/Uninstall")
        {
            await Cli.Wrap("sc")
                .WithArguments(new[] { "stop", ServiceName })
                .ExecuteAsync();
            await Cli.Wrap("sc")
                .WithArguments(new[] { "delete", ServiceName })
                .ExecuteAsync();
        }

        return;
    }
}