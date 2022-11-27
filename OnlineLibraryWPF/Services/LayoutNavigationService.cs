﻿using OnlineLibraryWPF.Stores;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Services
{
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;
        private readonly INavigationService _navigateHomeViewModel;

        public LayoutNavigationService(NavigationStore navigationStore,
            Func<TViewModel> createViewModel,
            INavigationService navigateHomeViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _navigateHomeViewModel = navigateHomeViewModel;
        }

        public void Navigate(string message = "")
        {
            _navigationStore.CurrentViewModel = new LayoutViewModel(_navigateHomeViewModel, _createViewModel());
        }
    }
}
