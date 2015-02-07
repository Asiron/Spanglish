using Spanglish.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spanglish.Validators
{
    /*
     * Validates Language definition of a dictionary
     *
     * Language name length cannot extend the boundries defined in Constants
     * It has to start with capital letter and follow with small letters
     * 
     */
    public class ValidateLanguageDefinitionService : IValidateString 
    {
        public ICollection<string> ValidateString(string lang)
        {
            ICollection<string> validationErrors = new List<string>();

            if (lang.Length > Constants.MaxLangNameLength || lang.Length < Constants.MinLangNameLength)
                validationErrors.Add(String.Format("The language name length must be between {0} and {1} characters.",
                    Constants.MinLoginLength, Constants.MaxLoginLength));

            if (!Regex.IsMatch(lang, @"^[A-Z][a-z]+$"))
                validationErrors.Add("Language name has to start with capital letter, and cannot contain anything else than letter");

            return validationErrors;
        }
    }
}
