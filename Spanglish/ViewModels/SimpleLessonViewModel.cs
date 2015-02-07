using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    /*
     * Simple lesson - choose a correct word translation from n given words in combobox
     */ 
    public class SimpleLessonViewModel : LessonViewModel
    {
        public Word CurrentSelectedWord
        {
            get { return _currentSelectedWord; }
            set { _currentSelectedWord = value; OnPropertyChanged("CurrentSelectedWord"); }
        }

        public ObservableCollection<Word> CurrentWordsToChoose { get; private set; }


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

        private Word _currentSelectedWord;

    }
}
