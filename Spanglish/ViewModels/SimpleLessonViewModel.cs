using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace Spanglish.ViewModels
{
    class SimpleLessonViewModel : ValidableObject, IBaseViewModel
    {
        public User CurrentUser { get; private set; }
        public RelayCommand RevertToPreviousViewModelCmd { get; private set; }
        public RelayCommand StartStopSimpleLessonCmd { get; private set; }
        public RelayCommand AcceptCurrentWordCmd { get; private set; }
        public RelayCommand SkipCurrentWordCmd { get; private set; }

        public int CorrectAnswers 
        { 
            get
            {
                return _correctAnswers;
            }
            private set
            {
                _correctAnswers = value;
                OnPropertyChanged("CorrectAnswers");
                OnPropertyChanged("TotalAnswers");
                OnPropertyChanged("LeftAnswers");
            }
        }
        public int WrongAnswers
        {
            get
            {
                return _wrongAnswers;
            }
            private set
            {
                _correctAnswers = value;
                OnPropertyChanged("WrongAnswers");
                OnPropertyChanged("TotalAnswers");
                OnPropertyChanged("LeftAnswers");
            }
        }
        public int SkippedAnswers
        {
            get
            {
                return _skippedAnswers;
            }
            private set
            {
                _skippedAnswers = value;
                OnPropertyChanged("SkippedAnswers");
                OnPropertyChanged("TotalAnswers");
                OnPropertyChanged("LeftAnswers");
            }
        }
        public int TotalAnswers
        {
            get
            {
                return CorrectAnswers + WrongAnswers + SkippedAnswers;
            }
        }

        public int LeftAnswers
        {
            get
            {
                return AllLessonWord.Count - TotalAnswers;
            }
        }

        public String TimeElapsed
        {
            get { return _timeElapsed;  }
            private set { _timeElapsed = value; OnPropertyChanged("TimeElapsed"); }
        } 

        private string _lessonFinishedText;
        public string LessonFinishedText { 
            get { return _lessonFinishedText; } 
            set { _lessonFinishedText = value; OnPropertyChanged("LessonFinishedText"); }
        }

        public string StartStopButtonText
        {
            get { return _startStopButtonText; }
            set { _startStopButtonText = value; OnPropertyChanged("StartStopButtonText"); }
        }

        public bool IsLessonRunning { get; set; }

        public DispatcherTimer _lessonDispatcherTimer;
        public DispatcherTimer LessonDispatcherTimer { get; set; }

        public DateTime LessonStartingTime { set; private get; }

        public Lesson CurrentLesson
        {
            get { return _currentLesson; }
            set { _currentLesson = value; OnPropertyChanged("CurrentLesson"); }
        }

        public ObservableCollection<Lesson> Lessons
        {
            get { return _lessons; }
            set { _lessons = value; OnPropertyChanged("Lessons"); }
        }

        public string LessonTitle
        {
            get
            {
                if (!IsLessonRunning)
                {
                    return "Choose a lesson";
                }
                else
                {
                    return "Current lesson - " + CurrentLesson.Name;
                }
            }
            set
            {
                _lessonTitle = value;
                OnPropertyChanged("LessonTitle");
            }
        }

        public bool ShowLessonPanel
        {
            get { return _showLessonPanel; }
            set { _showLessonPanel = value; OnPropertyChanged("ShowLessonPanel"); }
        }

        public bool ShowResultPanel
        {
            get { return _showResultPanel; }
            set { _showResultPanel = value; OnPropertyChanged("ShowResultPanel"); }
        }

        public ObservableCollection<Word> LessonWords { set; get; }
        public ObservableCollection<Word> AllLessonWord { set; get; }
        public ObservableCollection<Word> CurrentWordsToChoose { set; get; }
        public Word CurrentWord
        {
            get { return _currentWord; }
            set { _currentWord = value; OnPropertyChanged("CurrentWord"); }
        }

        public SimpleLessonViewModel(User currentUser)
        {
            CurrentUser = currentUser;
            RevertToPreviousViewModelCmd = new RelayCommand((p => ViewModelManager.Instance.ReturnToPreviousModel()));
            StartStopSimpleLessonCmd = new RelayCommand((p) => StartStopSimpleLesson(p), (p) => CanStartStopSimpleLesson(p));
            AcceptCurrentWordCmd = new RelayCommand((p) => AcceptCurrentWord(p), (p) => CanAcceptCurrentWord(p));
            SkipCurrentWordCmd = new RelayCommand((p) => SkipCurrentWord(p), (p) => CanSkipCurrentWord(p));

            StartStopButtonText = "Start";
            IsLessonRunning = false;
            ShowLessonPanel = false;
            ShowResultPanel = false;

            Lessons = new ObservableCollection<Lesson>();
            LessonWords = new ObservableCollection<Word>();
            AllLessonWord = new ObservableCollection<Word>();
            CurrentWordsToChoose = new ObservableCollection<Word>();

            using(var db = Database.Instance.GetConnection())
            {
                foreach (var lesson in db.Table<Lesson>().Where(l => l.UserId == currentUser.Id))
                {
                    Lessons.Add(lesson);
                }
            }

            LessonDispatcherTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 10), DispatcherPriority.Normal, 
                delegate {
                    var elapsed = DateTime.Now - LessonStartingTime;
                    TimeElapsed = String.Format("{0:00}:{1:00}:{2:00}",
                        elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds / 10);
                }, Dispatcher.CurrentDispatcher);

            LessonWords.CollectionChanged += OnLessonWordsCollectionChanged;

        }

        private void OnLessonWordsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (LessonWords.Count == 0)
            {
                ShowLessonPanel = false;
                
                LessonFinishedText = "Finished in ";
            }
        }

        private bool CanSkipCurrentWord(object p)
        {
            return true;
        }

        private void SkipCurrentWord(object p)
        {
            return;
        }

        private bool CanAcceptCurrentWord(object p)
        {
            return false;
        }

        private void AcceptCurrentWord(object p)
        {
            throw new NotImplementedException();
        }

        private bool CanStartStopSimpleLesson(object p)
        {
            if (IsLessonRunning == false)
            {
                return CurrentLesson != null;
            }
            else
            {
                return true;
            }
        }

        private void StartActualLesson()
        {
            using(var db = Database.Instance.GetConnection())
            {
                LessonWords.Clear();
                AllLessonWord.Clear();
                foreach( Word w in db.Table<Word>().Where(w => w.LessonId == CurrentLesson.Id) )
                {
                    LessonWords.Add(w);
                    AllLessonWord.Add(w);
                }
                LessonStartingTime = DateTime.Now;
                LessonDispatcherTimer.Start();
            }
        }

        private void PrepareWord()
        {
            Random rnd = new Random();
            foreach( var word in LessonWords.OrderBy(x => rnd.Next()).Take(5))
            {
                CurrentWordsToChoose.Add(Word.CopyFrom(word));
            }
        }

        private void StopActualLesson()
        {
            LessonDispatcherTimer.Stop();
            LessonFinishedText = "Finished in " + TimeElapsed;
        }

        private void StartStopSimpleLesson(object p)
        {
            if (StartStopButtonText.Equals("Start"))
            {
                StartStopButtonText = "Stop";
                StartActualLesson();
            }
            else
            {
                StartStopButtonText = "Start";
                StopActualLesson();
            }
            IsLessonRunning ^= true;
            ShowLessonPanel ^= true;
            ShowResultPanel ^= true;
            OnPropertyChanged("LessonTitle");

        }

        private string _startStopButtonText;
        private ObservableCollection<Lesson> _lessons;
        private  Lesson _currentLesson;
        private string _lessonTitle;
        private bool _showLessonPanel;
        private Word _currentWord;
        private bool _showResultPanel;
        private int _correctAnswers;
        private int _wrongAnswers;
        private int _skippedAnswers;
        private string _timeElapsed;
    }
}
