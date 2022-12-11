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

        private Customer? _customer;
        public Customer? Customer
        {
            get
            {
                return (LoggedUser is Customer cus ? cus : _customer);
            }
            set
            {
                _customer = value;
                OnCustomerChanged();
            }
        }

        private Book? _book;
        public Book? Book
        {
            get
            {
                return _book;
            }
            set
            {
                _book = value;
                OnBookChanged();
            }
        }

        public bool IsLoggedInCustomer => (LoggedUser is Customer customer ? customer : null) != null;
        public Librarian? Librarian => (LoggedUser is Librarian librarian ? librarian : null);
        public bool IsLoggedInLibrarian => Librarian != null;


        public event Action LoggedUserChanged;
        private void OnLoggedUserChanged()
        {
            LoggedUserChanged?.Invoke();
        }

        public event Action CustomerChanged;
        private void OnCustomerChanged()
        {
            CustomerChanged?.Invoke();
        }

        public event Action BookChanged;
        private void OnBookChanged()
        {
            BookChanged?.Invoke();
        }

        public void LogoutUser()
        {
            Book = null;
            LoggedUser = null;
            Customer = null;
        }
    }
}
