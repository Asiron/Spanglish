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
        public RelayCommand DeleteSelectedLessonCmd { set; get; }
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
                OnPropertyChanged("NewLessonName");
                ValidateProperty("NewLessonName", _newLessonName, _newLessonValidationService);
            }
            get { return _newLessonName; }
        }

        public Lesson CurrentLesson
        {
            get { return _currentLesson; }
            set
            {
                _currentLesson = value;
                OnPropertyChanged("CurrentLesson");
            }
        }
        public ObservableCollection<Lesson> Lessons { set; get; }
        public bool CurrentLessonWordsChanged { get; set; }

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
            DeleteSelectedLessonCmd = new RelayCommand((p) => DeleteSelectedLesson(p), (p) => CanDeleteSelectedLesson(p));
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
                else if ((p as string).Length < Constants.MinLessonNameLength ||
                    (p as string).Length > Constants.MaxLessonNameLength)
                    ret.Add(String.Format("Length has to be between {0} and {1}", Constants.MinLessonNameLength, Constants.MaxLessonNameLength));

                return ret;
            };

            ShowModifyLessonSubView = false;
            CurrentLessonWords = new ObservableCollection<Word>();
            CurrentEditingWord = new Word() {};

            CurrentLessonWords.CollectionChanged += CurrentLessonWords_CollectionChanged;
            CurrentLessonWordsChanged = false;
        }

        private bool CanDeleteSelectedLesson(object p)
        {
            return p != null && CurrentLesson != null && !CurrentLesson.Name.Equals(p as string);
        }

        private void DeleteSelectedLesson(object p)
        {
            Lesson lessonToRemove = Lessons.Where(l => l.Name.Equals(p as string)).First();
            Lessons.Remove(lessonToRemove);
            using(var db = Database.Instance.GetConnection())
            {
                foreach(Word word in CurrentLessonWords)
                {
                    RemoveWordIfExists(db, word);
                }
                RemoveLessonIfExists(db, lessonToRemove);
            }
        }

        void CurrentLessonWords_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CurrentLessonWordsChanged = true;
        }

        private bool CanSaveLesson()
        {
            return CurrentEditingWord != null && CurrentLesson != null &&
                !CurrentEditingWord.HasErrors && CurrentLessonWords.All<Word>((w) => !w.HasErrors) &&
                !String.IsNullOrWhiteSpace(CurrentLesson.FirstLangName) &&
                !String.IsNullOrWhiteSpace(CurrentLesson.SecondLangName) &&
                !CurrentLesson.HasErrors && CurrentLessonWords.Count() > 0;
        }

        private void SaveLesson()
        {
            if (CurrentLessonWords.Count() == 0 || CurrentLessonWordsChanged == false)
            {
                return;
            }

            using(var db = Database.Instance.GetConnection())
            {
                RemoveLessonIfExists(db, CurrentLesson);
                db.Insert(CurrentLesson);

                CurrentLesson = db.Table<Lesson>().Where(l => l.Name == CurrentLesson.Name).First();

                    
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
                CurrentEditingWord.Level != null && !CurrentEditingWord.HasErrors && !String.IsNullOrWhiteSpace(CurrentLesson.FirstLangName) &&
                !String.IsNullOrWhiteSpace(CurrentLesson.SecondLangName) && !CurrentLesson.HasErrors;
        }

        private void AddNewWordToLesson()
        {
            CurrentLessonWords.Add(Word.CopyFrom(CurrentEditingWord)); 
        }

        private void ShowLesson()
        {
            ShowModifyLessonSubView = true;
            using (var db = Database.Instance.GetConnection())
            {
                var lessonsInDatabase = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == CurrentLesson.Name);
                if (lessonsInDatabase.Count() != 1)
                {
                    CurrentLessonWords.Clear();
                    Lessons.Remove(CurrentLesson);
                    CurrentLesson = null; 
                    return;
                }
                CurrentLesson = lessonsInDatabase.First();


                CurrentLessonWords = new ObservableCollection<Word>();
                CurrentLessonWords.CollectionChanged += CurrentLessonWords_CollectionChanged;

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
            SaveLesson();
            CurrentLesson = new Lesson() { Name = NewLessonName, UserId = CurrentUser.Id };
            OnPropertyChanged("CurrentLesson");
            CurrentLessonWords = new ObservableCollection<Word>();
            CurrentLessonWords.CollectionChanged += CurrentLessonWords_CollectionChanged;
            CurrentLessonWordsChanged = false;
            ShowModifyLessonSubView = true;
            NewLessonName = "";
            if (Lessons.Contains(CurrentLesson) == false)
            {
                Lessons.Add(CurrentLesson);
            }
        }

        private void RemoveLessonIfExists(SQLiteConnection db,  Lesson lesson)
        {
            var lessonExistsInDB = db.Table<Lesson>().Where(l => l.Id == lesson.Id);
            if (lessonExistsInDB.Count() == 1)
            {
                db.Delete<Lesson>(lessonExistsInDB.First().Id);
            }
        }

        private void RemoveWordIfExists(SQLiteConnection db, Word word)
        {
            var wordExistsInDB = db.Table<Word>().Where(w => w.Id == word.Id);
            if (wordExistsInDB.Count() == 1)
            {
                db.Delete<Word>(wordExistsInDB.First().Id);
            }
        }

        private Word _currentEditingWord;
        private bool _showModifyLessonSubView;

        private string _newLessonName;
        private Lesson _currentLesson;

        private ValidationPredicate _newLessonValidationService;
        private ObservableCollection<Word> _currentLessonWords;
        private readonly IValidateString _langNameValidationService;
    } 
}
