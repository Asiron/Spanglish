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
            Random rnd = new Random();
            List<Word> newWordList = new List<Word>();
            foreach (var word in LessonWordsAll.OrderBy(x => rnd.Next()).Take(5))
            {
                newWordList.Add(Word.CopyFrom(word));
            }
            if (LessonWords.Count > 0)
            {
                int index = rnd.Next(LessonWords.Count());
                CurrentWord = Word.CopyFrom(LessonWords[index]);
                LessonWords.RemoveAt(index);
                if (!newWordList.Contains(CurrentWord))
                {
                    newWordList.Add(CurrentWord);
                }
            }
            CurrentWordsToChoose.Clear();
            foreach (Word w in newWordList.OrderBy(item => rnd.Next())) 
            {
                CurrentWordsToChoose.Add(w);
            }
        }
    }
}
