﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Validators
{
    interface IValidateString
    {
        ICollection<string> ValidateString(string str);
    }
}
