using YE.Control.Log;

namespace WpfTemplate.Services;

class Logger : ILogger
{
    enum LogLevel
    {
        All = 0,

        Debug,

        Info,

        Warn,

        Error,

        Fatal,

        Off = 0xff,
    }

    public void Debug(string format, params object[] args) =>
        Write(LogLevel.Debug, format, args);

    public void Info(string format, params object[] args) => Write(LogLevel.Info, format, args);

    public void Warn(string format, params object[] args) => Write(LogLevel.Warn, format, args);

    public void Error(string format, params object[] args) =>
        Write(LogLevel.Error, format, args);

    public void Fatal(string format, params object[] args) =>
        Write(LogLevel.Fatal, format, args);

    private void Write(LogLevel logLevel, string format, params object[] args)
    {
        var logger = NLog.LogManager.GetCurrentClassLogger();

        switch (logLevel)
        {
            case LogLevel.All:
                break;
            case LogLevel.Debug:

                {
                    logger.Debug(format, args);
                }
                break;
            case LogLevel.Info:

                {
                    logger.Info(format, args);
                }
                break;
            case LogLevel.Warn:

                {
                    logger.Warn(format, args);
                }
                break;
            case LogLevel.Error:

                {
                    logger.Error(format, args);
                }
                break;
            case LogLevel.Fatal:

                {
                    logger.Debug(format, args);
                }
                break;
            case LogLevel.Off:
                break;
            default:
                break;
        }
    }
}
