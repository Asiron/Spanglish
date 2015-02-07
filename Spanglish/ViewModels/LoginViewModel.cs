using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    /*
     * Handles login, upon correct credentials, Log in button should be activated and 
     * validation error's red frame should disappera
     */
    class LoginViewModel : ValidableObject, IBaseViewModel
    {
        public RelayCommand LoginCmd { get; private set; }
        public RelayCommand CreateNewAccountCmd { get; private set; }
        public RelayCommand SetCreateAccountViewCmd { get; private set;}

        public string CorrectCredentials {set; get;}

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                ValidateProperty("CorrectCredentials", _login, _validateLoginService);

            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                ValidateProperty("CorrectCredentials", _password, _validatePasswordService);
            }
        }

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand((p) => ExecuteLogin(p), (p) => CanLogin(p));
            SetCreateAccountViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new CreateAccountViewModel());
            _validateLoginService = (p) => {
                var ret = new List<string>();
                using (var db = Database.Instance.GetConnection())
                {
                    if (db.Table<User>().Count(u => (u.Login.Equals(p) && u.Password.Equals(Password))) == 0)
                    {
                        ret.Add("Login or password is not correct");
                    }
                }
                return ret;
            };

            _validatePasswordService = (p) =>
            {
                var ret = new List<string>();
                using (var db = Database.Instance.GetConnection())
                {
                    if (db.Table<User>().Count(u => (u.Login.Equals(Login) && u.Password.Equals(p))) == 0)
                    {
                        ret.Add("Login or password is not correct");
                    }
                }
                return ret;
            };
        }

        private void ExecuteLogin(object param)
        {
            User authUser = GetAuthenticatedUser(Login, Password);
            ViewModelManager.Instance.CurrentModel = new MainMenuViewModel(authUser);
        }

        private bool CanLogin(object param)
        {
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password) && !HasErrors;
        }

        private User GetAuthenticatedUser(string login, string password)
        {
            User authUser = null;
            using(var db = Database.Instance.GetConnection())
            {
                authUser = db.Table<User>().Where(u => u.Login.Equals(login) && u.Password.Equals(password)).First();
            }
            return authUser; 
        }

        private ValidationPredicate _validateLoginService;
        private ValidationPredicate _validatePasswordService;

        private string _login;
        private string _password;

    }
}
