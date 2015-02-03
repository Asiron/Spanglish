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
    class CreateAccountViewModel : ObservableObject, IBaseViewModel, INotifyDataErrorInfo
    {
        public string Name
        {
            get { return "Create new account screen"; }
        }

        private readonly IValidateString _loginValidatorService;
        private readonly IValidateString _passwordValidatorService;
        private readonly Dictionary<string, ICollection<string>> _validationErrors;

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
            _validationErrors = new Dictionary<string, ICollection<string>>();

        }

        private void ExecuteCreateNewAccount(object param)
        {
            using(var db = Database.Instance.GetConnection())
            {
                db.Insert(new User() { Login = NewLogin, Hash = NewPassword });
            }

        }

        private bool CanExecuteCreateNewAccount(object param)
        {
            bool _nl = !String.IsNullOrWhiteSpace(NewLogin);
            bool _np = !String.IsNullOrWhiteSpace(NewPassword);
            bool _npc = !String.IsNullOrWhiteSpace(NewPasswordConfirmation);
            bool _same = NewPasswordConfirmation == NewPassword;
            bool _err = !HasErrors;
            Console.WriteLine(String.Format("nl {0} np {1} npc {2} same {3} err {4}", _nl, _np, _npc, _same, _err));
            return !String.IsNullOrWhiteSpace(NewLogin) &&
                !String.IsNullOrWhiteSpace(NewPassword) &&
                !String.IsNullOrWhiteSpace(NewPasswordConfirmation) &&
                NewPasswordConfirmation == NewPassword && !HasErrors;
        }

        delegate ICollection<string> ValidationPredicate(object value);

        private void ValidateProperty(string propertyName, object value, ValidationPredicate predicate)
        {
            ICollection<string> validationErrors = predicate(value);
            bool isValid = validationErrors != null && validationErrors.Count() == 0;
            Console.WriteLine(isValid);
            if (!isValid)
            {
                _validationErrors[propertyName] = validationErrors;
                RaiseErrorsChanged(propertyName);
            }
            else if (_validationErrors.ContainsKey(propertyName))
            {
                _validationErrors.Remove(propertyName);
                RaiseErrorsChanged(propertyName);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void RaiseErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
            || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }
    }
}
