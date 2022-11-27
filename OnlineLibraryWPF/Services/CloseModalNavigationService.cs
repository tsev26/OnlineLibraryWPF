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

        public CloseModalNavigationService(ModalNavigationStore modalNavigationStore)
        {
            _modalNavigationStore = modalNavigationStore;
        }

        public void Navigate(string message = "")
        {
            _modalNavigationStore.Close();
        }
    }
}
