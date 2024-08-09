using System;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WpfTemplate.Services;

namespace WpfTemplate;

public partial class App : Application
{
    public static new App Current => (App)Application.Current;

    private IServiceProvider _serviceProvider;

    private readonly ILogger _log;

    private readonly IMessageBoxService _messageBoxService;

    App()
    {
        Startup += App_Startup;
        Exit += App_Exit;

        _serviceProvider = ConfigureServices();

        _log = GetService<ILogger>();
        _messageBoxService = GetService<IMessageBoxService>();

        /// Dependency Injection方式下，需要手动添加App.xaml中的资源
        /// 不能移动此函数调用位置，必须放到'MainWindow = GetService<Views.MainView>()'前面执行
        AddAppResources();

        MainWindow = GetService<MainWindow>();
        MainWindow.Visibility = Visibility.Visible;
    }

    #region Method

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        /// Services
        {
            /// IServiceCollection
            services.AddSingleton<IServiceCollection>(services);

            /// WeakReferenceMessenger
            services.AddSingleton<WeakReferenceMessenger>();
            services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider =>
                provider.GetRequiredService<WeakReferenceMessenger>()
            );

            /// Dispatcher
            services.AddSingleton(_ => Current.Dispatcher);

            /// ILogger
            services.AddSingleton<ILogger>(_ =>
            {
                return new LoggerConfiguration()
                    .Enrich.WithThreadId()
                    .MinimumLevel.Information()
                    .WriteTo.File(
                        "log.txt",
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Properties} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day
                    )
                    .CreateLogger();
            });

            services.AddSingleton<IMessageBoxService, MessageBoxService>();
        }

        /// View
        {
            /// MainWindow
            services.AddSingleton(sp => new MainWindow()
            {
                DataContext = sp.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
        }

        return services.BuildServiceProvider();
    }

    private void AddAppResources()
    {
        Current.Resources.MergedDictionaries.Add(
            new ResourceDictionary()
            {
                Source = new Uri(
                    "pack://application:,,,/HandyControl;component/Themes/SkinDefault.xaml"
                )
            }
        );
        Current.Resources.MergedDictionaries.Add(
            new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            }
        );
    }

    public T? GetService<T>()
        where T : class
    {
        return _serviceProvider.GetService(typeof(T)) as T;
    }

    #endregion


    #region Private Event

    private void App_Startup(object sender, StartupEventArgs e)
    {
        if (!EnsureAssemblySingletion())
        {
            _messageBoxService?.ShowMessage(
                $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name} 服务实例已在运行中。",
                MessageLevel.Information
            );

            System.Windows.Application.Current.Shutdown();

            return;
        }

        /// 设置UI线程发生异常时处理函数
        System.Windows.Application.Current.DispatcherUnhandledException +=
            App_DispatcherUnhandledException;

        /// 设置非UI线程发生异常时处理函数
        AppDomain.CurrentDomain.UnhandledException += App_UnhandledException;

        /// 设置托管代码异步线程发生异常时处理函数
        TaskScheduler.UnobservedTaskException += App_UnobservedTaskException;

        /// 设置非托管代码发生异常时处理函数
        callBack = new Unhandled_CallBack(Unhandled_ExceptionFilter);
        SetUnhandledExceptionFilter(callBack);
    }

    private void App_Exit(object sender, ExitEventArgs e)
    {
        mutex.ReleaseMutex();
        mutex.Dispose();
        mutex = null;
    }

    #endregion


    #region Singletion App

    /// <summary>
    /// 必须定义此变量
    /// </summary>
    /// <remarks>
    /// <para>
    /// 当EnsureAssemblySingletion()函数内部定义局部Mutex时，如果先启动软件再调试运行，此时判断单例模式失效。
    /// </para>
    /// </remarks>
    private System.Threading.Mutex mutex;

    private const string AssemblyGUID = "967bfbc5-fc46-401e-9b95-ad90953f0f13";

    private bool EnsureAssemblySingletion()
    {
        mutex = new System.Threading.Mutex(
            true,
            $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Name} - {AssemblyGUID}",
            out bool ret
        );
        return ret;
    }

    #endregion


    #region Try catch Exception

    private void App_DispatcherUnhandledException(
        object sender,
        System.Windows.Threading.DispatcherUnhandledExceptionEventArgs exception
    )
    {
        _log?.Error("[UI线程]异常：{0}.", exception.Exception);

        _messageBoxService?.ShowMessage(exception.Exception.ToString(), MessageLevel.Error);

        exception.Handled = true;
    }

    private void App_UnhandledException(object sender, UnhandledExceptionEventArgs exception)
    {
        _log?.Fatal("[非UI线程]异常：{0}.", exception);

        _messageBoxService?.ShowMessage("软件出现不可恢复错误，即将关闭。", MessageLevel.Error);

        System.Windows.Application.Current.Shutdown();
    }

    private void App_UnobservedTaskException(
        object sender,
        UnobservedTaskExceptionEventArgs exception
    )
    {
        _log?.Fatal($"Fatal - [Task]异常 Exception = {exception.Exception}.");

        exception.SetObserved();
    }

    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern Int32 SetUnhandledExceptionFilter(Unhandled_CallBack cb);

    private delegate int Unhandled_CallBack(ref long a);

    private Unhandled_CallBack callBack;

    private int Unhandled_ExceptionFilter(ref long a)
    {
        _log?.Fatal("[非托管代码]异常：{0}.", Environment.StackTrace);

        return 1;
    }

    #endregion
}
