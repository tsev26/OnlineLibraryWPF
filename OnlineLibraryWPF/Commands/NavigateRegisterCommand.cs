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
        private readonly INavigationService _navigationService;
        private readonly HomeViewModel _homeViewModel;

        public NavigateRegisterCommand(INavigationService navigationService, HomeViewModel homeViewModel)
        {
            _navigationService = navigationService;
            _homeViewModel = homeViewModel;
        }

        public override void Execute(object? parameter)
        {
            _homeViewModel.ButtonIsDefault = false;
            _navigationService.Navigate();
        }
    }
}
