using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spanglish.Validators
{
    public class WordDefinitionValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (String.IsNullOrWhiteSpace(value as string))
            {
                return new ValidationResult(false, "Please enter a non empty string");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
