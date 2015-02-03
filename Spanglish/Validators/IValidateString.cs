using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models
{
    interface IValidateString
    {
        ICollection<string> ValidateString(string str);
    }
}
