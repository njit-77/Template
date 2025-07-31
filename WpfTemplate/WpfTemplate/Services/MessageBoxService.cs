using Serilog;
using YE.Control.IServers;

namespace WpfTemplate.Services;

class MessageBoxService : IMessageBoxService
{
    private readonly ILogger logger;

    public MessageBoxService(ILogger _logger)
    {
        logger = _logger;
    }

    public bool ShowMessage(string message, MessageLevel messageLevel)
    {
        System.Windows.MessageBoxResult result = System.Windows.MessageBoxResult.None;
        switch (messageLevel)
        {
            case MessageLevel.Information:

                {
                    logger.Information("MessageBox Message = {Message}", message);

                    result = System.Windows.MessageBox.Show(
                        message,
                        "Information",
                        System.Windows.MessageBoxButton.OKCancel,
                        System.Windows.MessageBoxImage.Information,
                        System.Windows.MessageBoxResult.None,
                        System.Windows.MessageBoxOptions.DefaultDesktopOnly
                    );

                    logger.Information("MessageBox Result = {Result}", result);
                }
                break;
            case MessageLevel.Warning:

                {
                    logger.Warning("MessageBox Message = {Message}", message);

                    result = System.Windows.MessageBox.Show(
                        message,
                        "Warning",
                        System.Windows.MessageBoxButton.OKCancel,
                        System.Windows.MessageBoxImage.Warning,
                        System.Windows.MessageBoxResult.None,
                        System.Windows.MessageBoxOptions.DefaultDesktopOnly
                    );

                    logger.Warning("MessageBox Result = {Result}", result);
                }
                break;
            case MessageLevel.Error:

                {
                    logger.Error("MessageBox Message = {Message}", message);

                    result = System.Windows.MessageBox.Show(
                        message,
                        "Error",
                        System.Windows.MessageBoxButton.OKCancel,
                        System.Windows.MessageBoxImage.Error,
                        System.Windows.MessageBoxResult.None,
                        System.Windows.MessageBoxOptions.DefaultDesktopOnly
                    );

                    logger.Error("MessageBox Result = {Result}", result);
                }
                break;
            default:
                break;
        }
        return result == System.Windows.MessageBoxResult.OK;
    }
}
