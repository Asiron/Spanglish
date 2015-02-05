using Spanglish.Util;
using Spanglish.Models;
using Spanglish.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class CreateAccountViewModel : ValidableObject, IBaseViewModel
    {
        public string Name
        {
            get { return "Create new account screen"; }
        }

        private readonly IValidateString _loginValidatorService;
        private readonly IValidateString _passwordValidatorService;
        private readonly IValidateString _nameValidatorService;


        private string _newLogin;
        private string _newName;
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

        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                ValidateProperty("NewName", _newName, (p) => _nameValidatorService.ValidateString(p as string));
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                ValidateProperty("NewPassword", _newPassword, (p) => _passwordValidatorService.ValidateString(p as string));
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
                    SimpleValidationPredicate("Confirmation does not match password", (p) => !(p as string).Equals(NewPassword)));
            }
        }

        public RelayCommand CreateNewAccountCmd { get; set;}
        public RelayCommand RevertToPreviousViewModelCmd { get; set; }

        public CreateAccountViewModel()
        {
            CreateNewAccountCmd = new RelayCommand((p) => ExecuteCreateNewAccount(p), (p) => CanExecuteCreateNewAccount(p));
            RevertToPreviousViewModelCmd = new RelayCommand((p) => ViewModelManager.Instance.ReturnToPreviousModel());
            _loginValidatorService = new ValidateNewLoginService();
            _nameValidatorService = new ValidateNewNameService();
            _passwordValidatorService = new ValidateNewPasswordService();

        }

        private void ExecuteCreateNewAccount(object param)
        {
            using(var db = Database.Instance.GetConnection())
            {
                db.Insert(new User() { Login = NewLogin, Password = NewPassword, Name = NewName});
            }
            ViewModelManager.Instance.ReturnToPreviousModel();
        }

        private bool CanExecuteCreateNewAccount(object param)
        {
            return !String.IsNullOrWhiteSpace(NewLogin) &&
                !String.IsNullOrWhiteSpace(NewPassword) &&
                !String.IsNullOrWhiteSpace(NewPasswordConfirmation) &&
                !String.IsNullOrWhiteSpace(NewName) && !HasErrors;
        }
    }
}
