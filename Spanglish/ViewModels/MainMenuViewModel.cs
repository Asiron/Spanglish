using Spanglish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class MainMenuViewModel : BaseViewModel, IBaseViewModel
    {
        public MainMenuViewModel()
        {

        }

        public string Name
        {
            get { return "Main Menu"; }
        }
    }
}
