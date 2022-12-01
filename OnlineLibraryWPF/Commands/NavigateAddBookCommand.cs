using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class NavigateAddBookCommand : CommandBase
    {
        private INavigationService _navigateAddUpdateBookCommand;
        private UserStore _userStore;

        public NavigateAddBookCommand(INavigationService navigateAddUpdateBookCommand, UserStore userStore)
        {
            _userStore = userStore;
            _navigateAddUpdateBookCommand = navigateAddUpdateBookCommand;
        }

        public override void Execute(object? parameter)
        {
            _userStore.Book = null;
            _navigateAddUpdateBookCommand.Navigate();
        }
    }
}
