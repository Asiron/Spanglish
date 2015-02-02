using Spanglish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.ViewModels
{
    class MainMenuViewModel : ObservableObject, IBaseViewModel
    {
        public ApplicationViewModel Parent { set; get; }

        public MainMenuViewModel(ApplicationViewModel parent)
        {
            Parent = parent;
        }

        public string Name
        {
            get { return "Main Menu"; }
        }
    }
}
