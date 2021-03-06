﻿using Spanglish.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spanglish.Validators
{
    /*
     * Validates new passowrd of a user 
     *
     * Password has have its length within the boundries defined in Constants
     * 
     */
    public class ValidateNewPasswordService : IValidateString
    {
        public ICollection<string> ValidateString(string password)
        {
            ICollection<string> validationErrors = new List<string>();

            if (password.Length > Constants.MaxPasswordLength || password.Length < Constants.MinPasswordLength)
                validationErrors.Add(String.Format("The password length must be between {0} and {1} characters.",
                    Constants.MinPasswordLength, Constants.MaxPasswordLength));

            return validationErrors;
        }
    }
}
