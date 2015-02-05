using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class StatisticsViewModel : ValidableObject, IBaseViewModel
    {
        public User CurrentUser { get; private set; }

        public StatisticsViewModel(User currentUser)
        {
            // TODO: Complete member initialization
            CurrentUser = currentUser;
        }
    }
}
