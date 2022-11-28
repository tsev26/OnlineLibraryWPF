using OnlineLibraryWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineLibraryWPF.Stores
{
    public class UserStore
    {
        private User? _loggedUser;
        public User? LoggedUser
        {
            get
            {
                return _loggedUser;
            }
            set
            {
                _loggedUser = value;
                OnLoggedUserChanged();
            }
        }

        public UserStore()
        {

        }

        public Customer? Customer => (LoggedUser is Customer customer ? customer : null);
        public bool IsLoggedInCustomer => Customer != null;
        public Librarian? Librarian => (LoggedUser is Librarian librarian ? librarian : null);
        public bool IsLoggedInLibrarian => Librarian != null;


        public event Action LoggedUserChanged;
        private void OnLoggedUserChanged()
        {
            LoggedUserChanged?.Invoke();
        }

        public void LogoutUser()
        {
            LoggedUser = null;
        }
    }
}
