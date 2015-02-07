using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Validators
{
    /*
     * IValidateInteger interface, returns ICollection of errors from string
     */
    interface IValidateString
    {
        ICollection<string> ValidateString(string str);
    }
}
