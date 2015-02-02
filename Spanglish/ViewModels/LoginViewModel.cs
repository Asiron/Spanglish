using Spanglish.Misc;
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

        public string Name
        {
            get { return "Login Screen"; }
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public LoginViewModel()
        {
            LoginCmd = new RelayCommand((p) => ExecuteLogin(p), (p) => CanLogin(p));
           // CreateNewAccountCmd = new RelayCommand(CreateNewAccount, CanCreateNewAccount);
        }

        private void ExecuteLogin(object param)
        {

            Console.WriteLine(Password);
        }

        private bool CanLogin(object param)
        {
            return !String.IsNullOrWhiteSpace(Login) && !String.IsNullOrWhiteSpace(Password);
        }



        

    }
}
