using System;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using WpfTemplate.Extensions;
using WpfTemplate.Services;
using YE.Control.Helper;
using YE.Control.IServers;

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
            MainWindow = GetService<Views.MainView>();
            MainWindow.Visibility = Visibility.Visible;

            base.OnStartup(e);
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        GetService<ApplicationHelper>()?.OnExit();

        base.OnExit(e);

        LoggingExtensions.FreeConsole();
    }

    #endregion


    #region Method

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        /// IServiceCollection
        services.AddSingleton<IServiceCollection>(services);

        /// WeakReferenceMessenger
        services.AddSingleton<WeakReferenceMessenger>();
        services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider =>
            provider.GetRequiredService<WeakReferenceMessenger>()
        );

        /// Dispatcher
        services.AddSingleton(_ => Current.Dispatcher);

        /// View
        services.AddViews();

        /// ILogger
        services.AddLogger();

        /// Service
        services.AddSingleton<IMessageBoxService, MessageBoxService>();

        services.AddSingleton(sp => new ApplicationHelper(
            "967bfbc5-fc46-401e-9b95-ad90953f0f13",
            sp.GetRequiredService<IMessageBoxService>(),
            sp.GetRequiredService<Serilog.ILogger>()
        ));

        return services.BuildServiceProvider();
    }

    public T? GetService<T>()
        where T : class
    {
        return _serviceProvider.GetService(typeof(T)) as T;
    }

    #endregion
}
