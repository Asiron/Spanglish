using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Util
{
    /*
     * ViewModelManager is reponsible for handling viewmodels using typical stack implementation
     * 
     */
    public class ViewModelManager : Singleton<ViewModelManager>, INotifyPropertyChanged
    {
        public ViewModelManager()
        {
            _viewModels = new Stack<IBaseViewModel>();
        }

        public IBaseViewModel CurrentModel
        {
            get { return _viewModels.Peek(); }
            set
            {
                _viewModels.Push(value);
                OnPropertyChanged();
            }
        }

        public void ReturnToPreviousModel()
        {
            _viewModels.Pop();
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Stack<IBaseViewModel> _viewModels;

    }
}
