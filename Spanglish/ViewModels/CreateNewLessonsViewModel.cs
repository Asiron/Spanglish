using Spanglish.Util;
using Spanglish.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spanglish.Validators;

namespace Spanglish.ViewModels
{
    class CreateNewLessonsViewModel : ValidableObject, IBaseViewModel
    {
        public RelayCommand RevertToPreviousViewModelCmd { set; get; }
        public RelayCommand AddNewLessonCmd { set; get; }
        public RelayCommand DeleteSelectedWordCmd { set; get; }
        public RelayCommand ShowLessonCmd { set; get; }
        public RelayCommand AddNewWordToLessonCmd { set; get; }
        public RelayCommand SaveLessonCmd { set; get; }
        public User CurrentUser { set; get; }

        public bool ShowModifyLessonSubView
        {
            get { return _showModifyLessonSubView; }
            set
            {
                _showModifyLessonSubView = value;
                OnPropertyChanged("ShowModifyLessonSubView");
            }
        }

        public string NewLessonName
        { 
            set
            {
                _newLessonName = value;
                ValidateProperty("NewLessonName", _newLessonName, _newLessonValidationService);
            }
            get { return _newLessonName; }
        }

        public Lesson CurrentLesson
        {
            set
            {
                PreviousLesson = _currentLesson;
                _currentLesson = value;
            }
            get { return _currentLesson; }
        }
        public Lesson PreviousLesson { set; get; }
        public ObservableCollection<Lesson> Lessons { set; get; }

        public ObservableCollection<Word> CurrentLessonWords
        {
            get { return _currentLessonWords; }
            set
            {
                _currentLessonWords = value;
                OnPropertyChanged("CurrentLessonWords");
            }
        }

        public Word CurrentEditingWord
        {
            get { return _currentEditingWord;  }
            set 
            { 
                _currentEditingWord = value;
                OnPropertyChanged("CurrentEditingWord");
            }
        }

        public String LessonFirstLangDef
        {
            get { return _lessonFirstLangDef; }
            set
            {
                _lessonFirstLangDef = value;
                OnPropertyChanged("LessonFirstLangDef");
                ValidateProperty("LessonFirstLangDef", _lessonFirstLangDef,
                    (p) => _langNameValidationService.ValidateString(_lessonFirstLangDef));
            }
        }

        public String LessonSecondLangDef
        {
            get { return _lessonSecondLangDef; }
            set
            {
                _lessonSecondLangDef = value;
                OnPropertyChanged("LessonSecondLangDef");
                ValidateProperty("LessonSecondLangDef", _lessonSecondLangDef,
                    (p) => _langNameValidationService.ValidateString(_lessonSecondLangDef));
            }
        }


        public CreateNewLessonsViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            RevertToPreviousViewModelCmd = new RelayCommand((p) => ViewModelManager.Instance.ReturnToPreviousModel());
            AddNewLessonCmd = new RelayCommand((p) => StartAddingNewLesson(), (p) => CanStartAddingNewLesson());
            ShowLessonCmd = new RelayCommand((p) => ShowLesson(), (p) => CurrentLesson != null);
            SaveLessonCmd = new RelayCommand((p) => SaveLesson(), (p) => CanSaveLesson());
            DeleteSelectedWordCmd = new RelayCommand((p) => CurrentLessonWords.Remove(CurrentEditingWord), 
                (p) => CurrentEditingWord != null && CurrentLessonWords.Count() > 1);
            AddNewWordToLessonCmd = new RelayCommand((p) => AddNewWordToLesson(), (p) => CanAddNewWordToLesson());
            using (var db = Database.Instance.GetConnection())
            {
                Lessons = new ObservableCollection<Lesson>();
                foreach (var lesson in db.Table<Lesson>().Where(lesson => lesson.UserId == currentUser.Id))
                {
                    Lessons.Add(lesson);
                }
            }
            _langNameValidationService = new ValidateLanguageDefinitionService();
            _newLessonValidationService = (p) =>
            {
                var ret = new List<string>();
                int count;
                using (var db = Database.Instance.GetConnection())
                {
                    count = db.Table<Lesson>().Where(l => l.UserId == currentUser.Id && l.Name.Equals(p)).Count();
                }
                if (count >= 1 || (CurrentLesson != null && CurrentLesson.Name.Equals(p as string)))
                    ret.Add("Lesson name has to be unique");
                else if ((p as string).Length < 4)
                    ret.Add("Lesson name has to be longer than 4");

                return ret;
            };

            ShowModifyLessonSubView = false;
            CurrentLessonWords = new ObservableCollection<Word>();
            CurrentEditingWord = new Word() {};
        }

        private bool CanSaveLesson()
        {
            return CurrentEditingWord != null && !CurrentEditingWord.HasErrors && CurrentLessonWords.All<Word>((w) => !w.HasErrors) ;
        }

        private void SaveLesson()
        {
            using(var db = Database.Instance.GetConnection())
            {
                foreach (var word in CurrentLessonWords)
                {
                    try
                    {
                        db.Delete<Word>(word.Id);
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine(e.Message + " LOL");
                    }
                    db.Insert(new Word()
                    {
                        FirstLangDefinition = word.FirstLangDefinition,
                        SecondLangDefinition = word.SecondLangDefinition,
                        Level = word.Level,
                        LessonId = CurrentLesson.Id
                    });
                }
            }
        }

        private bool CanAddNewWordToLesson()
        {

            return CurrentEditingWord != null &&  !String.IsNullOrWhiteSpace(CurrentEditingWord.FirstLangDefinition) &&
                !String.IsNullOrWhiteSpace(CurrentEditingWord.SecondLangDefinition) &&
                CurrentEditingWord.Level != null && !CurrentEditingWord.HasErrors && !HasErrors;
        }

        private void AddNewWordToLesson()
        {
            CurrentLessonWords.Add(Word.CopyFrom(CurrentEditingWord));
           
        }

        private void ShowLesson()
        {
            ShowModifyLessonSubView = true;
            //SaveLessons(PreviousLesson);
            using (var db = Database.Instance.GetConnection())
            {
                CurrentLesson = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == CurrentLesson.Name).First();
                CurrentLessonWords = new ObservableCollection<Word>();
                foreach( var word in db.Table<Word>().Where(w => w.LessonId == CurrentLesson.Id))
                {
                    CurrentLessonWords.Add(word);
                }
                CurrentEditingWord = new Word();
                OnPropertyChanged("CurrentEditingWord");
                OnPropertyChanged("CurrentLessonWords");
                OnPropertyChanged("CurrentLesson");
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

            using (var db = Database.Instance.GetConnection())
            {
                db.Insert(CurrentLesson);
            }

            OnPropertyChanged("CurrentLesson");
            CurrentLessonWords = new ObservableCollection<Word>();
            ShowModifyLessonSubView = true;
        }

        private Word _currentEditingWord;
        private string _lessonFirstLangDef;
        private string _lessonSecondLangDef;
        private bool _showModifyLessonSubView;

        private string _newLessonName;
        private Lesson _currentLesson;

        private ValidationPredicate _newLessonValidationService;
        private ObservableCollection<Word> _currentLessonWords;
        private readonly IValidateString _langNameValidationService;
    } 
}
