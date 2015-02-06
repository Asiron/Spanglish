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
                CurrentWord = Word.CopyFrom(LessonWords[0]);
                LessonWords.RemoveAt(0);

            }
        }
    }
}
