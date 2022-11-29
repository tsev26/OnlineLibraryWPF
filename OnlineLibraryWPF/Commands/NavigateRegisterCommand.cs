using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Commands
{
    public class NavigateRegisterCommand : CommandBase
    {
        private INavigationService _navigateRegisterCommand;
        private CustomersViewModel _customersViewModel;

        public NavigateRegisterCommand(INavigationService navigateRegisterCommand, CustomersViewModel customersViewModel)
        {
            _navigateRegisterCommand = navigateRegisterCommand;
            _customersViewModel = customersViewModel;
        }

        public override void Execute(object? parameter)
        {
            _customersViewModel.UserStore.Customer = null;
            _navigateRegisterCommand.Navigate();
        }
    }
}
