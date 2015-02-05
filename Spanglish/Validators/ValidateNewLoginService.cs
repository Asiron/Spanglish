using Spanglish.Util;
using Spanglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spanglish.Validators
{
    public class ValidateNewLoginService : IValidateString
    {
        public ICollection<string> ValidateString(string login)
        {
            ICollection<string>  validationErrors = new List<string>();
            int count = 0;
            using (var db = Database.Instance.GetConnection())
            {
                count = db.Table<User>().Where(u => u.Login == login).Count();
            }
           
            if (count > 0)
                validationErrors.Add("The supplied username is already in use. Please choose another one.");

            
            if (login.Length > Constants.MaxLoginLength || login.Length < Constants.MinLoginLength)
                validationErrors.Add(String.Format("The login length must be between {0} and {1} characters.",
                    Constants.MinLoginLength, Constants.MaxLoginLength));

            if (!Regex.IsMatch(login, @"^[a-zA-Z]+$"))
                validationErrors.Add("The login must only contain upper and lower case letters.");

            return validationErrors;
        }
    }
}
