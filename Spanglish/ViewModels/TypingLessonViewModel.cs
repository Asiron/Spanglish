using Spanglish.Misc;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class TypingLessonViewModel : BaseViewModel
    {
        public User CurrentUser {get; private set;}

        public TypingLessonViewModel(User currentUser)
        {
            CurrentUser = currentUser;
        }

    }
}
