using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class TypingLessonViewModel : LessonViewModel
    {
        private string _currentWordToEdit;
        public string CurrentWordToEdit
        {
            get { return _currentWordToEdit; }
            set 
            { 
                _currentWordToEdit = value;
                OnPropertyChanged("CurrentWordToEdit");
                ValidateProperty("CurrentWordToEdit", _currentWordToEdit,
                    SimpleValidationPredicate("Cannot be empty!", (p) => String.IsNullOrWhiteSpace(p as string)));
            }
        }


        public TypingLessonViewModel(User currentUser)
            : base(currentUser)
        {
        }

        protected override bool CanAcceptCurrentWord(object p)
        {
            return CurrentWordToEdit != null && !HasErrors;
        }

        protected override bool CheckForWordCorrectness()
        {
            return CurrentWord.SecondLangDefinition.Equals(CurrentWordToEdit);
        }

        protected override void PrepareWord()
        {
            CurrentWordToEdit = "";
            if (LessonWords.Count > 0)
            {
                Random rnd = new Random();
                int index = rnd.Next(LessonWords.Count());
                CurrentWord = Word.CopyFrom(LessonWords[index]);
                LessonWords.RemoveAt(index);

            }
        }
    }
}
