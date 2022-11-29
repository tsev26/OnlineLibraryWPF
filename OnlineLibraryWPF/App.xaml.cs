using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OnlineLibraryWPF.ViewModels;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Stores;
using Microsoft.Extensions.Configuration;
using System.Windows.Controls;
using System.Threading.Tasks;

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
            services.AddTransient<CustomerMenuViewModel>(CreateCustomerMenuViewModel);
            services.AddTransient<LibrarianMenuViewModel>(CreateLibrarianMenuViewModel);
            services.AddTransient<RegisterViewModel>(CreateRegisterViewModel);
            services.AddTransient<RentalsViewModel>();
            services.AddTransient<BooksViewModel>();
            services.AddTransient<CustomersViewModel>(CreateCustomersViewModel);



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

        private LibrarianMenuViewModel CreateLibrarianMenuViewModel(IServiceProvider serviceProvider)
        {
            return new LibrarianMenuViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                CreateCustomersNavigationService(serviceProvider),
                CreateBooksNavigationService(serviceProvider),
                CreateRentalsNavigationService(serviceProvider),
                CreateHomeNavigationService(serviceProvider),
                serviceProvider.GetRequiredService<MessageStore>()
                );
        }

        private CustomersViewModel CreateCustomersViewModel(IServiceProvider serviceProvider)
        {
            return CustomersViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<UsersService>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                CreateLibrarianMenuNavigationService(serviceProvider),
                CreateRegisterNavigationService(serviceProvider),
                CreateRentalsNavigationService(serviceProvider));
        }

        private INavigationService CreateRentalsNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<RentalsViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<RentalsViewModel>());
        }

        private INavigationService CreateBooksNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<BooksViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<BooksViewModel>());
        }

        private INavigationService CreateCustomersNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<CustomersViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                () => serviceProvider.GetRequiredService<CustomersViewModel>());
        }

        private CustomerMenuViewModel CreateCustomerMenuViewModel(IServiceProvider serviceProvider)
        {
            return new CustomerMenuViewModel();
        }

        private RegisterViewModel CreateRegisterViewModel(IServiceProvider serviceProvider)
        {
            CompositeNavigationService compositeCustomersNavigationService = new CompositeNavigationService(
                 serviceProvider.GetRequiredService<CloseModalNavigationService>(),
                 CreateCustomersNavigationService(serviceProvider)
                 );


            return RegisterViewModel.LoadViewModel(
                serviceProvider.GetRequiredService<UsersService>(),
                serviceProvider.GetRequiredService<UserStore>(),
                serviceProvider.GetRequiredService<MessageStore>(),
                serviceProvider.GetRequiredService<CloseModalNavigationService>(),
                compositeCustomersNavigationService
                );
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
