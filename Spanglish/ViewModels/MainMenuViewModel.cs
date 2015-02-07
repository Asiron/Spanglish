using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    /*
     * Main Menu View Model accessible after logining in.
     * Gives access to
     *      - lesson creation/modifcation
     *      - simple lesson - choose from (n) words
     *      - typing lesson - type the correct word
     *      - statistics panel - that shows your answers statistics
     *      - log out button
     */
    class MainMenuViewModel : ValidableObject, IBaseViewModel
    {
        public RelayCommand RevertToPreviousViewModelCmd { get; private set; }
        public RelayCommand SetCreateNewLessonsViewCmd { set; private get; }
        public RelayCommand SetSimpleLessonViewCmd { set; private get; }
        public RelayCommand SetTypingLessonViewCmd { set; private get; }
        public RelayCommand SetStatisticsViewCmd { set; private get; }

        public User CurrentUser { get; private set; }

        public string UserWelcomeText
        {
            get { return "Welcome " + CurrentUser.Name; }
        }

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
