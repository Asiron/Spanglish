using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Validators
{
    interface IValidateInteger
    {
        ICollection<string> ValidateInteger(int? val);
    }
}
