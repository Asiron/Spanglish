﻿using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;
using SQLite.Net;

namespace Spanglish.ViewModels
{
    /*
     * Base class for implementing different Lesson types
     * It provides lesson selection, view with current results,
     * and shows with which languages you're dealing with.
     * It leaves space for implementing your own method of choosing correct word
     * It can be e.g. choose from n different words, or type in the correct word.
     */
    public class LessonViewModel : ValidableObject, IBaseViewModel
    {
        public RelayCommand RevertToPreviousViewModelCmd { get; private set; }
        public RelayCommand StartStopSimpleLessonCmd { get; private set; }
        public RelayCommand AcceptCurrentWordCmd { get; private set; }
        public RelayCommand SkipCurrentWordCmd { get; private set; }

        public User CurrentUser { get; private set; }

        public int CorrectAnswers 
        { 
            get { return _correctAnswers; }
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
                _wrongAnswers = value;
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
            get { return CorrectAnswers + WrongAnswers + SkippedAnswers; }
        }

        public int LeftAnswers
        {
            get { return LessonWordsAll.Count - TotalAnswers; }
        }

        public String TimeElapsed
        {
            get { return _timeElapsed;  }
            private set { _timeElapsed = value; OnPropertyChanged("TimeElapsed"); }
        } 


        public string LessonFinishedText { 
            get { return _lessonFinishedText; } 
            set { _lessonFinishedText = value; OnPropertyChanged("LessonFinishedText"); }
        }

        public string StartStopButtonText
        {
            get { return _startStopButtonText; }
            private set { _startStopButtonText = value; OnPropertyChanged("StartStopButtonText"); }
        }

        public bool IsLessonRunning { get; private set; }

        public DispatcherTimer LessonDispatcherTimer { get; private set; }

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
            set { _lessonTitle = value; OnPropertyChanged("LessonTitle"); }
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
        public ObservableCollection<Word> LessonWordsAll { set; get; }
        public Dictionary<Word, History> CurrentUserLessonHistory { set; get; }
        public Word CurrentWord
        {
            get { return _currentWord; }
            set { _currentWord = value; OnPropertyChanged("CurrentWord"); }
        }

        public LessonViewModel(User currentUser)
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
            LessonWordsAll = new ObservableCollection<Word>();
            CurrentUserLessonHistory = new Dictionary<Word, History>();

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
        }

        private bool IsFinished()
        {
            return LessonWords.Count == 0;
        }

        private void FinishLesson()
        {
            StopActualLesson();
            ShowLessonPanel = false;
            ShowResultPanel = true;
            using (var db = Database.Instance.GetConnection())
            {
                foreach (var pair in CurrentUserLessonHistory)
                {
                    try
                    {
                        db.Delete<History>(pair.Value.Id);
                    }
                    catch (SQLiteException e)
                    {
                        Console.WriteLine(e.Message + " LOL");
                    }
                    db.Insert(History.CopyFrom(pair.Value));
                }
            }
            CurrentUserLessonHistory[CurrentWord].Correct++;

            LessonFinishedText = String.Format("Finished with {0:0.00}% accuracy", CorrectAnswers * 100 / TotalAnswers) ;
        }

        private bool CanSkipCurrentWord(object p)
        {
            return true;
        }

        private void SkipCurrentWord(object p)
        {
            SkippedAnswers++;
            CurrentUserLessonHistory[CurrentWord].Skipped++;
            if (IsFinished())
            {
                FinishLesson();
                return;
            }
            PrepareWord();
        }

        protected virtual bool CanAcceptCurrentWord(object p)
        {
            return false;
        }

        private void AcceptCurrentWord(object p)
        {
            bool isCorrect = CheckForWordCorrectness();

            if (isCorrect)
            {
                CorrectAnswers++;
                CurrentUserLessonHistory[CurrentWord].LastTimeCorrect = DateTime.Now;
                CurrentUserLessonHistory[CurrentWord].Correct++;
            }
            else
            {
                CurrentUserLessonHistory[CurrentWord].Wrong++;
                WrongAnswers++;
            }
            if (IsFinished())
            {
                FinishLesson();
                return;
            }
            PrepareWord();
        }

        protected virtual bool CheckForWordCorrectness()
        {
            return false;
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
            StartStopButtonText = "Stop";
            ShowLessonPanel = true;
            ShowResultPanel = true;
            IsLessonRunning = true; 
            LessonStartingTime = DateTime.Now;
            LessonDispatcherTimer.Start();
            using(var db = Database.Instance.GetConnection())
            {
                LessonWords.Clear();
                LessonWordsAll.Clear();
                CurrentUserLessonHistory.Clear();
                foreach( Word w in db.Table<Word>().Where(w => w.LessonId == CurrentLesson.Id) )
                {
                    LessonWords.Add(w);
                    LessonWordsAll.Add(w);
                    var fetchedHistory = db.Table<History>().Where(h => h.UserId == CurrentUser.Id && h.WordId == w.Id);
                    History wordHistory = null;
                    if (fetchedHistory.Count() == 0) {
                        wordHistory = new History() {UserId = CurrentUser.Id, WordId = w.Id};
                    } else
                    {
                        wordHistory = fetchedHistory.First();
                    }
                    CurrentUserLessonHistory[w] = wordHistory;
                }
            }
            PrepareWord();
            CorrectAnswers = 0;
            SkippedAnswers = 0;
            WrongAnswers = 0;
        }

        protected virtual void PrepareWord()
        {
        }

        private void StopActualLesson()
        {
            StartStopButtonText = "Start";
            ShowLessonPanel = false;
            ShowResultPanel = false;
            IsLessonRunning = false;
            LessonDispatcherTimer.Stop();
            LessonFinishedText = "Finished in " + TimeElapsed;
        }

        private void StartStopSimpleLesson(object p)
        {
            if (IsLessonRunning == false)
            {
                StartActualLesson();
            }
            else
            {
                StopActualLesson();
            }
            OnPropertyChanged("LessonTitle");
        }

        private string _lessonFinishedText;
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
