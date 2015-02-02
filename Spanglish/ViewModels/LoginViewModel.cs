using Spanglish.Misc;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class LoginViewModel : ObservableObject, IBaseViewModel
    {
        public RelayCommand LoginCmd { get; set; }
        public RelayCommand CreateNewAccountCmd { get; set; }
        public RelayCommand SetCreateAccountViewCmd { get; set;}

        public string Name
        {
            get { return "Login Screen"; }
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand((p) => ExecuteLogin(p), (p) => CanLogin(p));
            CreateNewAccountCmd = new RelayCommand((p) => CreateNewAccount(p));
            SetCreateAccountViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new CreateAccountViewModel());
        }

        private object CreateNewAccount(object param)
        {
            throw new NotImplementedException();
        }

        private void ExecuteLogin(object param)
        {
            if (Authenticate(Login, Password))
            {
                Console.WriteLine("Authenticated");
            }
            else
                Console.WriteLine("NOT:");
        }

        private bool CanLogin(object param)
        {
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password);
        }



        private bool Authenticate(string login, string password)
        {
            bool auth = false;
            using(var db = Database.Instance.GetConnection())
            {
                auth = db.Table<User>().Any(u => u.Login == login && u.Hash == password);
            }
            return auth;
        }

    }
}
