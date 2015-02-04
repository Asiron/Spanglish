using Spanglish.Misc;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class MainMenuViewModel : BaseViewModel
    {
        public User CurrentUser { get; private set; }
        public string UserWelcomeText
        {
            get { return "Welcome " + CurrentUser.Name; }
        }

        public RelayCommand RevertToPreviousViewModelCmd { get; set; }
        public RelayCommand SetCreateNewLessonsViewCmd { set; get; }
        public RelayCommand SetSimpleLessonViewCmd { set; get; }
        public RelayCommand SetTypingLessonViewCmd { set; get; }
        public RelayCommand SetStatisticsViewCmd { set; get; }

        public MainMenuViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            RevertToPreviousViewModelCmd = new RelayCommand((p) => ViewModelManager.Instance.ReturnToPreviousModel());
            SetCreateNewLessonsViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new CreateNewLessonsViewModel(currentUser));
            SetSimpleLessonViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new SimpleLessonViewModel(currentUser));
            SetTypingLessonViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new TypingLessonViewModel(currentUser));
            SetStatisticsViewCmd = new RelayCommand((p) => ViewModelManager.Instance.CurrentModel = new StatisticsViewModel(currentUser));
        }
    }
}
