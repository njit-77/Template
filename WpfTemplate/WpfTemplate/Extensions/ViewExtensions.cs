using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTemplate.Extensions;

public static class ViewExtensions
{
    public static void AddViews(this IServiceCollection serviceCollection)
    {
        /// Views.MainView
        serviceCollection.AddSingleton(sp => new Views.MainView()
        {
            DataContext = sp.GetRequiredService<ViewModels.MainViewModel>()
        });
        serviceCollection.AddSingleton<ViewModels.MainViewModel>();
    }
}
