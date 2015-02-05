using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class SimpleLessonViewModel : ValidableObject, IBaseViewModel
    {
        public User CurrentUser { get; private set; }

        public SimpleLessonViewModel(User currentUser)
        {
            // TODO: Complete member initialization
            CurrentUser = currentUser;
        }
    }
}
