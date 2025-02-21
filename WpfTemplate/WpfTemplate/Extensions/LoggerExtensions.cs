using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace WpfTemplate.Extensions;

public static class LoggingExtensions
{
    public static void AddLogger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Serilog.ILogger>(_ =>
        {
            return new Serilog.LoggerConfiguration()
                .Enrich.WithThreadId()
                .MinimumLevel.Information()
                .WriteTo.File(
                    "log.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Properties} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: Serilog.RollingInterval.Day
                )
                .CreateLogger();
        });
    }
}
