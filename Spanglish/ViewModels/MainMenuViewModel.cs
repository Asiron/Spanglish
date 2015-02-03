using Spanglish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class MainMenuViewModel : BaseViewModel
    {
        private string _currentUser;

        public RelayCommand RevertToPreviousViewModelCmd { set; get; }
        public RelayCommand SetCreateNewLessonsViewCmd { set; get; }
        public RelayCommand SetSimpleLessonViewCmd { set; get; }
        public RelayCommand SetTypingLessonViewCmd { set; get; }
        public RelayCommand SetStatisticsViewCmd { set; get; }    

        public String CurrentUser
        {
            get { return "Welcome " + _currentUser; }
            set
            {
                _currentUser = value;
            }
        }

        public MainMenuViewModel(string currentUser)
        {
            _currentUser = currentUser;
            RevertToPreviousViewModelCmd = new RelayCommand((p) => ViewModelManager.Instance.ReturnToPreviousModel());
            SetCreateNewLessonsViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new CreateNewLessonsViewModel(currentUser));
            SetSimpleLessonViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new SimpleLessonViewModel(currentUser));
            SetTypingLessonViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new TypingLessonViewModel(currentUser));
            SetStatisticsViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new StatisticsViewModel(currentUser));
        }
    }
}
