using YE.Control.Log;
using YE.Control.MessageBox;

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
                    logger.Info("MessageBox:{msg}", message);

                    result = System.Windows.MessageBox.Show(
                        message,
                        "Information",
                        System.Windows.MessageBoxButton.OKCancel,
                        System.Windows.MessageBoxImage.Information,
                        System.Windows.MessageBoxResult.None,
                        System.Windows.MessageBoxOptions.DefaultDesktopOnly
                    );

                    logger.Info("MessageBoxResult:{ret}", result);
                }
                break;
            case MessageLevel.Warning:

                {
                    logger.Warn("MessageBox:{msg}", message);

                    result = System.Windows.MessageBox.Show(
                        message,
                        "Warning",
                        System.Windows.MessageBoxButton.OKCancel,
                        System.Windows.MessageBoxImage.Warning,
                        System.Windows.MessageBoxResult.None,
                        System.Windows.MessageBoxOptions.DefaultDesktopOnly
                    );

                    logger.Warn("MessageBoxResult:{ret}", result);
                }
                break;
            case MessageLevel.Error:

                {
                    logger.Error("MessageBox:{msg}", message);

                    result = System.Windows.MessageBox.Show(
                        message,
                        "Error",
                        System.Windows.MessageBoxButton.OKCancel,
                        System.Windows.MessageBoxImage.Error,
                        System.Windows.MessageBoxResult.None,
                        System.Windows.MessageBoxOptions.DefaultDesktopOnly
                    );

                    logger.Error("MessageBoxResult:{ret}", result);
                }
                break;
            default:
                break;
        }
        return result == System.Windows.MessageBoxResult.OK;
    }
}
