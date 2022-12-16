using OnlineLibraryWPF.Commands;
using OnlineLibraryWPF.MongoDB;
using OnlineLibraryWPF.Services;
using OnlineLibraryWPF.Stores;
using System.Windows.Input;

namespace OnlineLibraryWPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

		private string _loginName;
		public string LoginName
		{
			get
			{
				return _loginName;
			}
			set
			{
				_loginName = value;
				OnPropertyChanged(nameof(LoginName));
			}
		}

		private string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		public MessageStore MessageStore { get; set; }

        public HomeViewModel(UserStore userStore,
                            INavigationService navigateCustomerCommand,
                            INavigationService navigateLibrarianCommand,
							INavigationService navigateRegisterCommand,
							MongoDBService mongoDBService,
                            MessageStore messageStore)
        {
			MessageStore = messageStore;
            LoginCommand = new LoginCommand(userStore, navigateCustomerCommand, navigateLibrarianCommand, mongoDBService, this);
			RegisterCommand = new NavigateCommand(navigateRegisterCommand);

            MessageStore.MessageChanged += MessageStore_MessageChanged;
        }

        public override void Dispose()
        {
            MessageStore.MessageChanged -= MessageStore_MessageChanged;
            base.Dispose();
        }

        private void MessageStore_MessageChanged()
		{
            OnPropertyChanged(nameof(MessageStore));
        }
	}
}
