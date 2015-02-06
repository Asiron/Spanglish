using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    public class SimpleLessonViewModel : LessonViewModel
    {
        private Word _currentSelectedWord;
        public Word CurrentSelectedWord
        {
            get { return _currentSelectedWord; }
            set { _currentSelectedWord = value; OnPropertyChanged("CurrentSelectedWord"); }
        }

        public ObservableCollection<Word> CurrentWordsToChoose { set; get; }


        public SimpleLessonViewModel(User currentUser) : base(currentUser)
        {
            CurrentWordsToChoose = new ObservableCollection<Word>();
        }

        protected override bool CanAcceptCurrentWord(object p)
        {
            return CurrentWordsToChoose.Count() > 0 && CurrentSelectedWord != null;
        }

        protected override bool CheckForWordCorrectness()
        {
            return CurrentWord.FirstLangDefinition.Equals(CurrentSelectedWord.FirstLangDefinition);
        }

        protected override void PrepareWord()
        {
            CurrentWordsToChoose.Clear();
            Random rnd = new Random();
            foreach (var word in LessonWordsAll.OrderBy(x => rnd.Next()).Take(5))
            {
                CurrentWordsToChoose.Add(Word.CopyFrom(word));
            }
            if (LessonWords.Count > 0)
            {
                CurrentWord = Word.CopyFrom(LessonWords[0]);
                LessonWords.RemoveAt(0);
                if (!CurrentWordsToChoose.Contains(CurrentWord))
                {
                    CurrentWordsToChoose.Add(CurrentWord);
                }
            }
        }
    }
}
