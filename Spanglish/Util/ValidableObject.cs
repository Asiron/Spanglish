using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spanglish.ViewModels;

namespace Spanglish.Util
{
    public abstract class ValidableObject : ObservableObject, INotifyDataErrorInfo
    {
        
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string,ICollection<string>>();

        public delegate ICollection<string> ValidationPredicate(object value);
        public ValidationPredicate SimpleValidationPredicate(string errorString, Predicate<object> predicate)
        {
            return (param) => {
                var list = new List<string>();
                if (predicate(param))
                {
                    list.Add(errorString);
                }
                return list;
            };
        }

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
    }
}
