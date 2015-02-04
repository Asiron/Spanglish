using Spanglish.Misc;
using Spanglish.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class CreateNewLessonsViewModel : BaseViewModel
    {
        public RelayCommand RevertToPreviousViewModelCmd { set; get; }
        public RelayCommand AddNewLessonCmd { set; get; }
        public RelayCommand DeleteSelectedWordCmd { set; get; }
        public RelayCommand SaveLessonsCmd { set; get; }
        public RelayCommand ShowLessonCmd { set; get; }
        public User CurrentUser { set; get; }

        private ValidationPredicate _newLessonValidationService;

        private bool _showModifyLessonSubView;
        public bool ShowModifyLessonSubView
        {
            get { return _showModifyLessonSubView; }
            set
            {
                _showModifyLessonSubView = value;
                OnPropertyChanged("ShowModifyLessonSubView");
            }
        }

        private string _newLessonName;
        public string NewLessonName
        { 
            set
            {
                _newLessonName = value;
                ValidateProperty("NewLessonName", _newLessonName, _newLessonValidationService);
            }
            get { return _newLessonName; }
        }

        public Lesson CurrentLesson { set; get; }
        public ObservableCollection<Lesson> Lessons { set; get; }
        public ObservableCollection<Word> CurrentLessonWords { set; get; }

        private Word _currentEditingWord;
        public Word CurrentEditingWord
        {
            set
            {
                _currentEditingWord = value;
                if (_currentEditingWord != null)
                {
                    Console.WriteLine(_currentEditingWord.FirstLangDefinition); 
                }
            }
            get { return _currentEditingWord; }
        }

        public CreateNewLessonsViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            RevertToPreviousViewModelCmd = new RelayCommand((p) => ViewModelManager.Instance.ReturnToPreviousModel());
            AddNewLessonCmd = new RelayCommand((p) => StartAddingNewLesson(), (p) => CanStartAddingNewLesson());
            SaveLessonsCmd = new RelayCommand((p) => SaveLessons());
            ShowLessonCmd = new RelayCommand((p) => ShowLesson(), (p) => CurrentLesson != null);
            DeleteSelectedWordCmd = new RelayCommand((p) => CurrentLessonWords.Remove(CurrentEditingWord), 
                (p) => CurrentEditingWord != null && CurrentLessonWords.Count() > 1);
           
            _newLessonValidationService = (p) =>
                {
                    var ret = new List<string>();
                    int count;
                    using (var db = Database.Instance.GetConnection())
                    {
                        count = db.Table<Lesson>().Where(l => l.UserId == currentUser.Id && l.Name.Equals(p)).Count();
                    }
                    if (count == 1)
                        ret.Add("Lesson name has to be unique");
                    else if ((p as string).Length < 4)
                        ret.Add("Lesson name has to be longer than 4");
                    return ret;
                };

            using (var db = Database.Instance.GetConnection())
            {
                Lessons = new ObservableCollection<Lesson>(db.Table<Lesson>().Where(lesson => lesson.UserId == currentUser.Id));
            }
            ShowModifyLessonSubView = false;
            CurrentLessonWords = new ObservableCollection<Word>();
            CurrentEditingWord = new Word() {};
        }

        private void ShowLesson()
        {
            ShowModifyLessonSubView = true;
            SaveLessons();
            using (var db = Database.Instance.GetConnection())
            {
                CurrentLesson = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == CurrentLesson.Name).First();
                CurrentLessonWords = new ObservableCollection<Word>(db.Table<Word>().Where(w => w.LessonId == CurrentLesson.Id));
                OnPropertyChanged("CurrentLessonWords");
            }
        }

        private void SaveLessons()
        {
            try
            {
                using (var db = Database.Instance.GetConnection())
                {
                    var databaseLessons = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == CurrentLesson.Name);
                    if (databaseLessons.Count() == 0)
                    {
                        db.Insert(new Lesson() { Name = CurrentLesson.Name, UserId = CurrentUser.Id });
                    }
                    int lessonId = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == CurrentLesson.Name).First().Id;
                    Console.WriteLine(lessonId);
                    db.RunInTransaction(() =>
                    {
                        try
                        {
                            foreach (var word in CurrentLessonWords)
                            {
                                db.InsertOrReplace( new Word()
                                {
                                    FirstLangDefinition = word.FirstLangDefinition,
                                    SecondLangDefinition = word.SecondLangDefinition,
                                    LessonId = lessonId,
                                    Level = word.Level
                                });
                            }
                        }
                        catch (SQLiteException e)
                        {
                            Console.WriteLine(e.Message + "KUPA");
                        }
                    });
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message + "DUPA");
            }
        }

        private bool CanStartAddingNewLesson()
        {
            return !HasErrors && !String.IsNullOrWhiteSpace(NewLessonName);
        }

        private void StartAddingNewLesson()
        {
            CurrentLesson = new Lesson() { Name = NewLessonName, UserId = CurrentUser.Id };
            Lessons.Add(CurrentLesson);
            ShowModifyLessonSubView = true;
        }
    } 
}
