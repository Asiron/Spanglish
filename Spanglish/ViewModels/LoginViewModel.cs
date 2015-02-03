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
    class LoginViewModel : BaseViewModel
    {
        public RelayCommand LoginCmd { get; set; }
        public RelayCommand CreateNewAccountCmd { get; set; }
        public RelayCommand SetCreateAccountViewCmd { get; set;}


        private string _login;
        private string _password;

        public string CorrectCredentials
        {
            get;
            set;
        }
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

        private ValidationPredicate _validateLoginService;
        private ValidationPredicate _validatePasswordService;

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand((p) => ExecuteLogin(p), (p) => CanLogin(p));
            SetCreateAccountViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new CreateAccountViewModel());
            _validateLoginService = (p) => {
                var ret = new List<string>();
                using (var db = Database.Instance.GetConnection())
                {
                    if (db.Table<User>().Count(u => (u.Login.Equals(p) && u.Hash.Equals(Password))) == 0)
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
                    if (db.Table<User>().Count(u => (u.Login.Equals(Login) && u.Hash.Equals(p))) == 0)
                    {
                        ret.Add("Login or password is not correct");
                    }
                }
                return ret;
            };
        }

        private void ExecuteLogin(object param)
        {
            ViewModelManager.Instance.CurrentModel = new MainMenuViewModel(Login);
        }

        private bool CanLogin(object param)
        {
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password) && !HasErrors;
        }

        private bool Authenticate(string login, string password)
        {
            bool auth = false;
            using(var db = Database.Instance.GetConnection())
            {
                auth = db.Table<User>().Count(u => u.Login == login && u.Hash == password) == 1;
            }
            return auth;
        }

    }
}
