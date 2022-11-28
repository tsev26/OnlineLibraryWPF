using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class LogoutCommand : CommandBase
    {
        private INavigationService _navigateHomeCommand;
        private UserStore _userStore;

        public LogoutCommand(INavigationService navigateHomeCommand, UserStore userStore)
        {
            _navigateHomeCommand = navigateHomeCommand;
            _userStore = userStore;
        }

        public override void Execute(object? parameter)
        {
            _userStore.LogoutUser();
            _navigateHomeCommand.Navigate("You have been Logout!");
        }
    }
}
