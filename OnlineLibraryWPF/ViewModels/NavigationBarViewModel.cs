using OnlineLibraryWPF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        //public ICommand NavigateHomeCommand { get; }

        public NavigationBarViewModel() //INavigationService homeNavigationService
        {
            //NavigateHomeCommand = new NavigateCommand(homeNavigationService);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
