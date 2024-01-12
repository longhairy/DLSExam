using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using System.Diagnostics;
using System.Reflection;

namespace Monitoring;

public static class MonitorService
{
    public static readonly string ServiceName = Assembly.GetCallingAssembly().GetName().Name ?? "unknown";
    public static TracerProvider TracerProvider;
    public static ActivitySource ActivitySource = new(ServiceName);
    public static ILogger Log => Serilog.Log.Logger;
    static MonitorService()
    {
        //Log
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.WithSpan()
            //.WriteTo.Console()
            .WriteTo.Seq("http://seq-service:80")
            .CreateLogger();

        //OpenTelemetry
        try
        {
            TracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(ActivitySource.Name)
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName))
            //.AddConsoleExporter()
            .AddZipkinExporter(options =>
            {
                options.Endpoint = new Uri("http://zipkin-service:9411/api/v2/spans");
            })
            .SetSampler(new AlwaysOnSampler())
            .Build();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Opentelemetry init failed: {ex}");
        }

    }
}
