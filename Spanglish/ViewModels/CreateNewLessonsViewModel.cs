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

        private Lesson _currentLesson;
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

        private ObservableCollection<Word> _currentLessonWords;
        public ObservableCollection<Word> CurrentLessonWords
        {
            get { return _currentLessonWords; }
            set
            {
                _currentLessonWords = value;
                OnPropertyChanged("CurrentLessonWords");
            }
        }
        private Word _currentEditingWord;
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
            using (var db = Database.Instance.GetConnection())
            {
                Lessons = new ObservableCollection<Lesson>();
                foreach (var lesson in db.Table<Lesson>().Where(lesson => lesson.UserId == currentUser.Id))
                {
                    Lessons.Add(lesson);
                }
            }

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
            CurrentLessonWords.CollectionChanged += CurrentLessonWords_CollectionChanged;
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
                CurrentEditingWord.Level != null && !CurrentEditingWord.HasErrors;
        }

        private void AddNewWordToLesson()
        {
            CurrentLessonWords.Add(new Word()
            {
                FirstLangDefinition = CurrentEditingWord.FirstLangDefinition,
                SecondLangDefinition = CurrentEditingWord.SecondLangDefinition,
                Level = CurrentEditingWord.Level,
                LessonId = CurrentLesson.Id
            });
        }

        void CurrentLessonWords_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           // Console.WriteLine("COllection changed");
            using(var db = Database.Instance.GetConnection())
            {

            }
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
                CurrentLessonWords.CollectionChanged += CurrentLessonWords_CollectionChanged;
                CurrentEditingWord = new Word();
                OnPropertyChanged("CurrentEditingWord");
                OnPropertyChanged("CurrentLessonWords");
                OnPropertyChanged("CurrentLesson");
            }
        }
        /*
        private void SaveLessons(Lesson lessonToSave)
        {
            Console.WriteLine("Begin");
            if (lessonToSave == null)
            {
                Console.WriteLine("Previous null, returning");
                return;
            }
            Console.WriteLine("Saving " + lessonToSave.Name);

            try
            {
                using (var db = Database.Instance.GetConnection())
                {
                    var lessonToSaveInDatabase = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == lessonToSave.Name);
                    if (lessonToSaveInDatabase.Count() == 0)
                    {
                        db.Insert(new Lesson() { Name = lessonToSave.Name, UserId = CurrentUser.Id });
                    }
                    int lessonToSaveInDatabaseId = db.Table<Lesson>().Where(l => l.UserId == CurrentUser.Id && l.Name == lessonToSave.Name).First().Id;
                    Console.WriteLine(lessonToSaveInDatabaseId);
                    //db.RunInTransaction(() =>
                  //  {
                        try
                        {
                            //db.InsertOrReplaceAll(CurrentLessonWords.Where(w => w.IsModified));
                            foreach (var word in CurrentLessonWords.Where(w => w.IsModified))
                            {
                                db.InsertOrReplace(word);
                                Console.WriteLine("modified " + word.FirstLangDefinition);
                            }
                            foreach (var word in CurrentLessonWords.Where(w => !w.IsModified))
                            {
                                Console.WriteLine("not modified " + word.FirstLangDefinition);
                                word.IsModified = true;
                                word.LessonId = lessonToSaveInDatabaseId;
                                db.Insert(word);
                                Console.WriteLine(String.Format("{0} {1} {2} - Lesson {3}", word.FirstLangDefinition, word.SecondLangDefinition, word.Level, lessonToSaveInDatabaseId));
                                
                                int b = db.InsertOrReplace( new Word()
                                {
                                    FirstLangDefinition = word.FirstLangDefinition,
                                    SecondLangDefinition = word.SecondLangDefinition,
                                    LessonId = previousLessonInDatabaseId,
                                    Level = word.Level
                                });
                                
                                //Console.WriteLine(b);
                                Console.WriteLine("Before check");
                                foreach (var word1 in db.Table<Word>().Where(w => w.LessonId == lessonToSaveInDatabaseId))
                                {
                                    Console.WriteLine(String.Format("{0} {1} {2} - Lesson {3}", word1.FirstLangDefinition, word1.SecondLangDefinition, word1.Level, word1.LessonId));
                                }
                                Console.WriteLine("After check");
                            }
                        }
                        catch (SQLiteException e)
                        {
                            Console.WriteLine(e.Message + "KUPA");
                        }
                   // });
                    Console.WriteLine(" -- ");
                    foreach(var word in db.Table<Word>().Where(w => w.LessonId == lessonToSaveInDatabaseId))
                    {
                        Console.WriteLine(String.Format("{0} {1} {2} - Lesson {3}", word.FirstLangDefinition, word.SecondLangDefinition, word.Level, word.LessonId));
                    }
                }

            }
            catch (SQLiteException e)
            {
                Console.WriteLine(e.Message + "DUPA");
            }
            Console.WriteLine("End");
        }
        */
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
            CurrentLessonWords.CollectionChanged += CurrentLessonWords_CollectionChanged;
            ShowModifyLessonSubView = true;
        }
    } 
}
