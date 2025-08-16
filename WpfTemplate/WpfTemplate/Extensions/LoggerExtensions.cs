using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;

namespace WpfTemplate.Extensions;

public static class LoggingExtensions
{
    public static void AddLogger(this IServiceCollection serviceCollection)
    {
        //NLog.LogManager.Setup()
        //    .LoadConfiguration(builder =>
        //    {
        //        var layout =
        //            "${longdate} [ThreadId:${threadid}] [${level}] ${message} ${all-event-properties} ${exception:format=tostring,stacktrace}";

        //        builder
        //            .ForLogger()
        //            .FilterMinLevel(LogLevel.Trace)
        //            .WriteToFile(fileName: "${basedir}/log/log${date:yyyyMMdd}.log", layout: layout)
        //            .WithAsync();

        //        builder
        //            .ForLogger()
        //            .FilterMinLevel(LogLevel.Trace)
        //            .WriteToConsole(encoding: System.Text.Encoding.UTF8, layout: layout)
        //            .WithAsync();
        //    });

        serviceCollection.AddSingleton<YE.Control.Log.ILogger, Services.Logger>();
    }

    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();
}
