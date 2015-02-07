using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Spanglish.Util
{
    /*
     * Implementation of ICommand interface, for the purpose of maintaining MVVM pattern
     */
    public class RelayCommand : ICommand
    {
        readonly Action<object> _executeAction;
        readonly Predicate<object> _canExecuteAction;

        public RelayCommand(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("Relay - execute");

            _executeAction = execute;
            _canExecuteAction = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteAction == null || _canExecuteAction(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _executeAction(parameter);
        }
    }
}
