using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spanglish.Misc;

namespace Spanglish.Misc
{
    public class ViewModelManager : Singleton<ViewModelManager>, INotifyPropertyChanged
    {

        private Stack<BaseViewModel> _viewModels;

        public ViewModelManager()
        {
            _viewModels = new Stack<BaseViewModel>();
        }

        public BaseViewModel CurrentModel
        {
            get { return _viewModels.Peek(); }
            set
            {
                _viewModels.Push(value);
                //value.OnViewActivate();
                OnPropertyChanged();
            }
        }

        public void ReturnToPreviousModel()
        {
            _viewModels.Pop();
            //CurrentModel.OnViewActivate();
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
