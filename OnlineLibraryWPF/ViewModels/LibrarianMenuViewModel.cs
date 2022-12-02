using OnlineLibraryWPF.Commands;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class LibrarianMenuViewModel : ViewModelBase
    {
        public ICommand LogoutCommand { get; }
        public ICommand NavigateCustomersCommand { get; }
        public ICommand NavigateBooksCommand { get; }
        public ICommand NavigateRentalsCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand LoadInfoAbouCustomersCommand { get; }

        public MessageStore MessageStore { get; set; }

        public LibrarianMenuViewModel(UserStore userStore, 
                                      INavigationService navigateCustomersCommand,
                                      INavigationService navigateBooksCommand,
                                      INavigationService navigateRentalsCommand,
                                      INavigationService navigateHomeCommand,
                                      MessageStore messageStore,
                                      UsersService usersService)
        {
            MessageStore = messageStore;
            LogoutCommand = new LogoutCommand(navigateHomeCommand, userStore);
            NavigateCustomersCommand = new NavigateCommand(navigateCustomersCommand);
            NavigateBooksCommand = new NavigateCommand(navigateBooksCommand);
            NavigateRentalsCommand = new NavigateCommand(navigateRentalsCommand);
            LoadInfoAbouCustomersCommand = new LoadInfoAbouCustomersCommand(usersService, messageStore);
            ExportCommand = new ExportCommand();
            ImportCommand = new ImportCommand();
        }

        public static LibrarianMenuViewModel LoadViewModel(UserStore userStore,
                                      INavigationService navigateCustomersCommand,
                                      INavigationService navigateBooksCommand,
                                      INavigationService navigateRentalsCommand,
                                      INavigationService navigateHomeCommand,
                                      MessageStore messageStore,
                                      UsersService usersService)
        {
            LibrarianMenuViewModel viewModel = new LibrarianMenuViewModel(userStore,
                                                                          navigateCustomersCommand,
                                                                          navigateBooksCommand,
                                                                          navigateRentalsCommand,
                                                                          navigateHomeCommand,
                                                                          messageStore,
                                                                          usersService);

            viewModel.LoadInfoAbouCustomersCommand.Execute(null);

            return viewModel;
        }
    }
}
