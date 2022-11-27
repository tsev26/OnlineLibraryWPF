using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OnlineLibraryWPF.ViewModels;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Stores;
using Microsoft.Extensions.Configuration;

namespace OnlineLibraryWPF
{
    public partial class App : Application
    {
        private readonly IServiceProvider _servicesProvider;

        public IConfigurationRoot Configuration { get; set; }
        public App()
        {

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            DatabaseSettings settings = config.GetRequiredSection("OnlineLibraryDatabase").Get<DatabaseSettings>();


            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<DatabaseSettings>(settings);
            services.AddSingleton<UsersService>();

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<UserStore>();
            services.AddSingleton<MessageStore>();
            
            services.AddSingleton<INavigationService>(CreateHomeNavigationService);
            services.AddSingleton<CloseModalNavigationService>();

            services.AddTransient<HomeViewModel>(CreateHomeViewModel);
            services.AddTransient<CustomerMenuViewModel>();
            services.AddTransient<LibrarianMenuViewModel>();
            services.AddTransient<RegisterViewModel>(CreateRegisterViewModel);
            


            services.AddTransient<NavigationBarViewModel>(); //CreateNavigationBarViewModel

            services.AddSingleton<MainViewModel>();

            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });

            _servicesProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            INavigationService initialNavigationService = _servicesProvider.GetRequiredService<INavigationService>();
            initialNavigationService.Navigate();


            MainWindow = _servicesProvider.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        private HomeViewModel CreateHomeViewModel(IServiceProvider serviceProvider)
        {
            return new HomeViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                CreateCustomerMenuNavigationService(serviceProvider),
                CreateLibrarianMenuNavigationService(serviceProvider),
                CreateRegisterNavigationService(serviceProvider),
                serviceProvider.GetRequiredService<UsersService>(),
                serviceProvider.GetRequiredService<MessageStore>()
                );
        }

        private RegisterViewModel CreateRegisterViewModel(IServiceProvider serviceProvider)
        {
            return new RegisterViewModel(
                serviceProvider.GetRequiredService<UsersService>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>());
        }

        private INavigationService CreateRegisterNavigationService(IServiceProvider serviceProvider)
        {
            return new ModalNavigationService<RegisterViewModel>(
                serviceProvider.GetRequiredService<ModalNavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<RegisterViewModel>());
        }

        private INavigationService CreateLibrarianMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<LibrarianMenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<LibrarianMenuViewModel>());
        }

        private INavigationService CreateCustomerMenuNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<CustomerMenuViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<CustomerMenuViewModel>());
        }

        private INavigationService CreateHomeNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<HomeViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<HomeViewModel>());
        }
    }
}
