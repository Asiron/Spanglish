using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Spanglish.ViewModels
{
    /*
     * View Model that shows statistics about seleceted lesson
     * Correct / Wrong and Skipped answers are shown as well as last timestamp of 
     * correctly guessing the password
     * 
     */
    class StatisticsViewModel : ValidableObject, IBaseViewModel
    {
        public User CurrentUser { get; private set; }

        public RelayCommand RevertToPreviousViewModelCmd { get; private set; }

        public ObservableCollection<Lesson> Lessons { get; private set; }

        public ObservableCollection<FetchedHistoryEntry> CurrentLessonHistory { get; private set; }

        public FetchedHistoryEntry SelectedLessonHistoryEntry { get; private set; }

        public Lesson CurrentLesson
        {
            get { return _currentLesson; }
            set { _currentLesson = value; OnPropertyChanged("CurrentLesson"); }
        }

        public int CurrentLessonIndex
        {
            get { return _currentLessonIndex; }
            set
            {
                _currentLessonIndex = value; 
                OnPropertyChanged("CurrentLessonIndex");
                ShowLessonHistory(Lessons[_currentLessonIndex]);
            }
        }

        private void ShowLessonHistory(Lesson currentLesson)
        {
            using(var db = Database.Instance.GetConnection())
            {
                CurrentLessonHistory.Clear();

                foreach(Word word in db.Table<Word>().Where(w => w.LessonId == currentLesson.Id))
                {
                    var currentWordHistoryIsInDB = db.Table<History>().Where(h => h.WordId == word.Id && h.UserId == CurrentUser.Id);
                    if (currentWordHistoryIsInDB.Count() == 1)
                    {
                        FetchedHistoryEntry newEntry = new FetchedHistoryEntry(currentWordHistoryIsInDB.First());
                        newEntry.AddWord(word);
                        CurrentLessonHistory.Add(newEntry);
                    } 
                }
            }
        }

        public StatisticsViewModel(User currentUser)
        {
            RevertToPreviousViewModelCmd = new RelayCommand((p => ViewModelManager.Instance.ReturnToPreviousModel()));
            CurrentUser = currentUser;

            Lessons = new ObservableCollection<Lesson>();
            CurrentLessonHistory = new ObservableCollection<FetchedHistoryEntry>();

            using(var db = Database.Instance.GetConnection())
            {
                foreach(Lesson lesson in db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id))
                {
                    Lessons.Add(lesson);
                }
            }
        }

        /*
         *  Simple aggregate class for easy binding history to datagrid
         *  Word has to be retrieved from database, because history only holds it's id.
         */
        public class FetchedHistoryEntry : History
        {
            public FetchedHistoryEntry(History historyEntry)
                : base(historyEntry)
            {
    
            }

            public void AddWord(Word corelatedWord)
            {
                FirstLangDefinition = corelatedWord.FirstLangDefinition;
                SecondLangDefinition = corelatedWord.SecondLangDefinition;
                Level = corelatedWord.Level;
            }

            public String FirstLangDefinition {get; private set;}
            public String SecondLangDefinition {get; private set;}
            public byte? Level { get; private set; }

            public String LastTimeCorrectStringified
            { 
                get
                {
                    if (LastTimeCorrect == DateTime.MinValue)
                    {
                        return "Never";
                    } else {
                        return LastTimeCorrect.ToShortDateString();
                    }
                }
            }
        }

        private Lesson _currentLesson;
        private int _currentLessonIndex;


    }
}
