using Spanglish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class CreateAccountViewModel : ObservableObject, IBaseViewModel
    {
        public string Name
        {
            get { return "Create new account screen"; }
        }

        public string NewLogin { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }

        public RelayCommand CreateNewAccountCmd { get; set;} 
        public CreateAccountViewModel()
        {
            CreateNewAccountCmd = new RelayCommand((p) => ExecuteCreateNewAccount(p), (p) => CanExecuteCreateNewAccount(p));
        }

        private void ExecuteCreateNewAccount(object param)
        {

        }

        private bool CanExecuteCreateNewAccount(object param)
        {
            return !String.IsNullOrWhiteSpace(NewLogin) &&
                !String.IsNullOrWhiteSpace(NewPassword) &&
                !String.IsNullOrWhiteSpace(NewPasswordConfirmation);
        }
    }
}
