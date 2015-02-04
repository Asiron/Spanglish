using Spanglish.Misc;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class SimpleLessonViewModel : BaseViewModel
    {
        public User CurrentUser { get; private set; }

        public SimpleLessonViewModel(User currentUser)
        {
            // TODO: Complete member initialization
            CurrentUser = currentUser;
        }
    }
}
