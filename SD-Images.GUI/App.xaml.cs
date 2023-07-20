using Microsoft.Extensions.DependencyInjection;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using SD_Images.GUI.Model;
using SD_Images.GUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SD_Images.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {                        
            ConfigureViewModels(services);

            services.AddSingleton<MainWindow>(s => new MainWindow
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
        }

        private void ConfigureViewModels(ServiceCollection services)
        {
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<CloseModalNavigationService>();

            services.AddSingleton(s => new NavigationService<HomePageViewModel>(
                s.GetRequiredService<NavigationStore>(),
                () => s.GetRequiredService<HomePageViewModel>()
            ));

            services.AddSingleton(s => new ParameterNavigationService<WebUIImage, ImagePageViewModel>
            (
                s.GetRequiredService<NavigationStore>(),
                img => new ImagePageViewModel(img, s.GetRequiredService<NavigationService<HomePageViewModel>>())
            ));

            services.AddSingleton(s => new HomePageViewModel
            (
                s.GetRequiredService<ParameterNavigationService<WebUIImage, ImagePageViewModel>>()
            ));

            services.AddSingleton<MainWindowViewModel>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            INavigationService initialNavigationService = _serviceProvider.GetRequiredService<NavigationService<HomePageViewModel>>();
            initialNavigationService.Navigate();

            var mainWindow = _serviceProvider.GetService<MainWindow>();
           
            mainWindow.Show();
        }
    }    
}
