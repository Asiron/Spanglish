using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Misc
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string,ICollection<string>>();


        public delegate ICollection<string> ValidationPredicate(object value);

        protected void ValidateProperty(string propertyName, object value, ValidationPredicate predicate)
        {
            ICollection<string> validationErrors = predicate(value);
            bool isValid = validationErrors != null && validationErrors.Count() == 0;
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

        public virtual void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}
