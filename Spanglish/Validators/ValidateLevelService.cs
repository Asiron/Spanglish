using Spanglish.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Spanglish.Validators
{
    /*
     * Validates level of a single word
     *
     * Level cannot be empty and has to be within the boundries defined in Constants
     * 
     */
    public class ValidateLevelService : IValidateInteger
    {
        public ICollection<string> ValidateInteger(int? value)
        {
            ICollection<string> validationErrors = new List<string>();
            if (value == null)
            {
                validationErrors.Add("Cannot be empty");
            }

            if (value > Constants.MaxWordLevel || value < 0)
            {
                validationErrors.Add(String.Format("Level must be between 0 and {0}", Constants.MaxWordLevel));
            }

            return validationErrors;
        }
    }
}
