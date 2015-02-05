using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Util
{
    public class ViewModelManager : Singleton<ViewModelManager>, INotifyPropertyChanged
    {

        private Stack<IBaseViewModel> _viewModels;

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
