using FlightControl.Models;
using FlightControl.Services;
using FlightControl.ViewModels;
using FlightControl.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FlightControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

		/// <summary>
		/// Configures services for the application. Registers ViewModel and Control Tower dependencies as transient services.
		/// </summary>
		private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<IControlTower, ControlTower>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainView = new MainView
            {
                DataContext = _serviceProvider.GetRequiredService<MainViewModel>()
            };
            mainView.Show();
        }
    }
}