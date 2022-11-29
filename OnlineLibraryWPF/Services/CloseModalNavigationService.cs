using OnlineLibraryWPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Services
{
    public class CloseModalNavigationService : INavigationService
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly MessageStore _messageStore;
        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore, MessageStore messageStore)
        {
            _modalNavigationStore = modalNavigationStore;
            _messageStore = messageStore;
        }

        public void Navigate(string message = "")
        {
            _messageStore.ModalMessage = "";
            _modalNavigationStore.Close();
        }
    }
}
