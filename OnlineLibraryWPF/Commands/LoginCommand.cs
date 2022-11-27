using OnlineLibraryWPF.Models;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using OnlineLibraryWPF.ViewModels;
using System.Threading.Tasks;
using static OnlineLibraryWPF.Services.HashFunction;

namespace OnlineLibraryWPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly UserStore _userStore;
        private readonly INavigationService _navigateCustomerCommand;
        private readonly INavigationService _navigateLibrarianCommand;
        private readonly UsersService _usersService;
        private readonly HomeViewModel _homeViewModel;

        public LoginCommand(UserStore userStore, 
                            INavigationService navigateCustomerCommand, 
                            INavigationService navigateLibrarianCommand, 
                            UsersService usersService,
                            HomeViewModel homeViewModel)
        {
            _userStore = userStore;
            _navigateCustomerCommand = navigateCustomerCommand;
            _navigateLibrarianCommand = navigateLibrarianCommand;
            _usersService = usersService;
            _homeViewModel = homeViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if ((_homeViewModel.LoginName ?? "") == "") 
            {
                _homeViewModel.MessageStore.Message = "Login name missing!";
                return;
            }
            if ((_homeViewModel.Password ?? "") == "")
            {
                _homeViewModel.MessageStore.Message = "Password missing!";
                return;
            }
            

            User user = await _usersService.GetByLoginAndPassAsync(_homeViewModel.LoginName, HashString(_homeViewModel.Password));

            if (user != null)
            {
                _userStore.LoggedUser = user;

                if (_userStore.IsLoggedInCustomer)
                {
                    _navigateCustomerCommand.Navigate();
                }
                else if (_userStore.IsLoggedInLibrarian)
                {
                    _navigateLibrarianCommand.Navigate();
                }
            } 
            else
            {
                _homeViewModel.MessageStore.Message = "User not found! Check your login name and password!";
            }
        }
    }
}
