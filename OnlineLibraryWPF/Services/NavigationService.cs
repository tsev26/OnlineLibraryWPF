﻿using OnlineLibraryWPF.Stores;
using OnlineLibraryWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace OnlineLibraryWPF.Services
{
    public class NavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly MessageStore _messageStore;
        private readonly Func<TViewModel> _createViewModel;
        
        public NavigationService(NavigationStore navigationStore, MessageStore messageStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _messageStore = messageStore;
            _createViewModel = createViewModel;
        }

        public void Navigate(string message = "")
        {
            _messageStore.Message = message;
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
