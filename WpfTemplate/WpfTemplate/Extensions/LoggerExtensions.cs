using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace WpfTemplate.Extensions;

public static class LoggingExtensions
{
    public static void AddLogger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Serilog.ILogger>(_ =>
        {
            return new Serilog.LoggerConfiguration()
                .Enrich.WithThreadId()
                .MinimumLevel.Verbose()
                .WriteTo.Console(
                    theme: AnsiConsoleTheme.Code,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Properties} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.File(
                    "./log/log.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Properties} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: Serilog.RollingInterval.Day
                )
                .CreateLogger();
        });
    }

    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();
}
