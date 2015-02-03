using Spanglish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class StatisticsViewModel : BaseViewModel
    {
        private string currentUser;

        public StatisticsViewModel(string currentUser)
        {
            // TODO: Complete member initialization
            this.currentUser = currentUser;
        }
    }
}
