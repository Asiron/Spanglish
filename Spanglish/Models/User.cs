using Spanglish.Misc;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models
{
    public class User
    {
        string _name = null;
        string _login = null;
        string _password = null;
        /// <summary>
        ///  TODO change password to hash
        /// </summary>

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length < Constants.MinUsernameLength)
                    throw new ArgumentException("ExceptionUsernameTooShort");
                _name = value;
            }
        }

        [Unique, MaxLength(20), NotNull]
        public string Login
        {
            get { return _login; }
            set
            {
                if (value.Length < Constants.MinLoginLength)
                    throw new ArgumentException("ExceptionLoginTooShort");
                _login = value;
            }
        }

        [MaxLength(20), NotNull]
        public string Password
        {
            get { return _password; }
            set
            {
                if (value.Length < Constants.MinPasswordLength)
                    throw new ArgumentException("ExceptionPasswordTooShort");
                _password = value;
            }
        }
    }
}
