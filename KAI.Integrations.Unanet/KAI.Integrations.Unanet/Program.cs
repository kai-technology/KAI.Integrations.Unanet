using Azure.Monitor.OpenTelemetry.Exporter;
using KAI.Integrations.Unanet.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using Microsoft.Extensions.Configuration;
using KAI.Integrations.Unanet.Services;


var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING")))
{
    builder.Services.AddOpenTelemetry()
        .UseFunctionsWorkerDefaults()
        .UseAzureMonitorExporter();
}

builder.Configuration
    .AddJsonFile(
        "appsettings.json",
        optional: true,
        reloadOnChange: true)
    .AddJsonFile(
        "appsettings.Local.json",
        optional: true,
        reloadOnChange: true);

builder.Services.Configure<IntegrationSettings>(
    builder.Configuration.GetSection("IntegrationSettings"));


builder.Services.AddHttpClient<
    IUnanetService,
    UnanetService>(
    client =>
    {
        client.BaseAddress =
            new Uri(
                builder.Configuration[
                    "IntegrationSettings:UnanetBaseUrl"]!);
    });

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<
    IUnanetEventDeduplicator,
    MemoryUnanetEventDeduplicator>();

builder.Services.AddScoped<
    IUnanetEventIngressService,
    UnanetEventIngressService>();

builder.Services.AddHttpClient<
    IUnanetService,
    UnanetService>(
    client =>
    {
        string? baseUrl =
            builder.Configuration[
                "IntegrationSettings:UnanetBaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "IntegrationSettings:UnanetBaseUrl is required.");
        }

        client.BaseAddress =
            new Uri(
                baseUrl.TrimEnd('/') + "/");

        client.DefaultRequestHeaders.Accept.ParseAdd(
            "application/json");
    });

builder.Build().Run();
