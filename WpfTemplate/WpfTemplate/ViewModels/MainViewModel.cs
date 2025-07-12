using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;

namespace WpfTemplate.ViewModels;

partial class MainViewModel : ObservableObject, IRecipient<string>
{
    public MainViewModel()
    {
        App.Current.GetService<IMessenger>().Register(this);
        Task.Delay(3_000).ContinueWith(_ =>
        {
            for (int i = 0; i < 3; i++)
            {
                App.Current.GetService<IMessenger>().Send($"Hello From MainViewModel {i}");
                System.Threading.Thread.Sleep(1_000);
            }
        });
    }

    public void Receive(string message)
    {
        App.Current.GetService<Serilog.ILogger>().Information(message);
    }
}
