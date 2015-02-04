using Spanglish.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spanglish.Validators
{
    public class LevelValidationrule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int i;
            if(int.TryParse(value.ToString(), out i) == false)
                return new ValidationResult(false, "Please enter a valid integer value");

            if (i > Constants.MaxWordLevel || i < 0)
                return new ValidationResult(false, String.Format("Level must be between 0 and {0}", Constants.MaxWordLevel));

            return new ValidationResult(true, null);
        }
    }
}
