using System;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using WpfTemplate.Extensions;
using WpfTemplate.Views;
using YE.Control.Helper;
using YE.Control.Log;
using YE.Control.MessageBox;

namespace WpfTemplate;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;

    private IServiceProvider _serviceProvider;

    App()
    {
        _serviceProvider = ConfigureServices();
    }

    #region override

    protected override void OnStartup(StartupEventArgs e)
    {
        LoggingExtensions.AllocConsole();

        if (GetService<ApplicationHelper>()?.OnStartup() == true)
        {
            MainWindow = GetService<MainView>();
            MainWindow.Visibility = Visibility.Visible;

            base.OnStartup(e);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        GetService<ApplicationHelper>()?.OnExit();

        LoggingExtensions.FreeConsole();

        NLog.LogManager.Shutdown();

        base.OnExit(e);
    }

    #endregion

    #region Method

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        /// View
        services.AddViews();

        /// ILogger
        services.AddLogger();

        /// Service
        services.AddSingleton<IMessageBoxService, Services.MessageBoxService>();

        services.AddSingleton(sp => new ApplicationHelper(
            sp.GetRequiredService<IMessageBoxService>(),
            sp.GetRequiredService<ILogger>(),
            "14d28ff8-e0a0-44c3-a19e-eb51a89e36f8"
        ));

        /// WeakReferenceMessenger
        services.AddSingleton<WeakReferenceMessenger>();
        services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider =>
            provider.GetRequiredService<WeakReferenceMessenger>()
        );

        /// Dispatcher
        services.AddSingleton(_ => Current.Dispatcher);

        return services.BuildServiceProvider();
    }

    public T? GetService<T>()
        where T : class
    {
        return _serviceProvider.GetService(typeof(T)) as T;
    }

    #endregion
}
