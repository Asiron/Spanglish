using Spanglish.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Spanglish.Validators
{
    /*
     * Validates new name of a user
     *
     * User's new name has to have its length within boundries defined in Constants
     * It must have first and last name, seperated with white spaces
     * and each part has to start with capital letter, followed by at least one small letter
     */
    class ValidateNewNameService : IValidateString
    {
        public ICollection<string> ValidateString(string name)
        {
            ICollection<string> validationErrors = new List<string>();

            if (name.Length > Constants.MaxUsernameLength || name.Length < Constants.MinUsernameLength)
                validationErrors.Add(String.Format("The password length must be between {0} and {1} characters.",
                    Constants.MinUsernameLength, Constants.MaxUsernameLength));

            if (!Regex.IsMatch(name, @"^[A-Z][a-z]*\s[A-Z][a-z]*$"))
                validationErrors.Add("The name must consist of first and last name and start with a capital letter");


            return validationErrors;
        }
    }
}
