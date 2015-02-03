using Spanglish.Misc;
using Spanglish.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class CreateAccountViewModel : BaseViewModel, IBaseViewModel
    {
        public string Name
        {
            get { return "Create new account screen"; }
        }

        private readonly IValidateString _loginValidatorService;
        private readonly IValidateString _passwordValidatorService;

        private string _newLogin;
        private string _newPassword;
        private string _newPasswordConfirmation;

        public string NewLogin
        {
            get { return _newLogin; }
            set
            {
                _newLogin = value;
                ValidateProperty("NewLogin", _newLogin, (p) => _loginValidatorService.ValidateString(p.ToString()));
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                ValidateProperty("NewPassword", _newPassword, (p) => _passwordValidatorService.ValidateString(p.ToString()));
            }
        }
        public string NewPasswordConfirmation
        {
            get { return _newPasswordConfirmation; }
            set
            {
                _newPasswordConfirmation = value;
                ValidateProperty("NewPasswordConfirmation",
                    _newPasswordConfirmation,
                    (p) =>
                    {
                        return p.ToString() != NewPassword  ? new List<string>{"Confirmation does not match password"} : new List<string>();
                    });
            }
        }

        public RelayCommand CreateNewAccountCmd { get; set;}
        public RelayCommand RevertToPreviousViewModelCmd { get; set; }

        public CreateAccountViewModel()
        {
            CreateNewAccountCmd = new RelayCommand((p) => ExecuteCreateNewAccount(p), (p) => CanExecuteCreateNewAccount(p));
            RevertToPreviousViewModelCmd = new RelayCommand((p) => ViewModelManager.Instance.ReturnToPreviousModel());
            _loginValidatorService = new ValidateNewLoginService();
            _passwordValidatorService = new ValidateNewPasswordService();

        }

        private void ExecuteCreateNewAccount(object param)
        {
            using(var db = Database.Instance.GetConnection())
            {
                db.Insert(new User() { Login = NewLogin, Hash = NewPassword });
            }
            ViewModelManager.Instance.ReturnToPreviousModel();
        }

        private bool CanExecuteCreateNewAccount(object param)
        {
            return !String.IsNullOrWhiteSpace(NewLogin) &&
                !String.IsNullOrWhiteSpace(NewPassword) &&
                !String.IsNullOrWhiteSpace(NewPasswordConfirmation) 
                 && !HasErrors;
        }

    }
}
