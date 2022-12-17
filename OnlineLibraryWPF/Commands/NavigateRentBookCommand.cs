using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class NavigateRentBookCommand : CommandBase
    {
        private readonly INavigationService _navigateRentBookCommand;
        private readonly UserStore _userStore;

        public NavigateRentBookCommand(INavigationService navigateRentBookCommand, UserStore userStore)
        {
            _navigateRentBookCommand = navigateRentBookCommand;
            _userStore = userStore;
        }

        public override void Execute(object? parameter)
        {
            _userStore.Book = null;
            _navigateRentBookCommand.Navigate("Rent book for customer " + _userStore.Customer.LoginName);
        }
    }
}
