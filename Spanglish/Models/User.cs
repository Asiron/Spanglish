using Spanglish.Util;
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

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(Constants.MaxUsernameLength), NotNull]
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

        [Unique, MaxLength(Constants.MaxLoginLength), NotNull]
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

        [MaxLength(100), NotNull]
        public string Password
        {
            get;
            set;
        }
    }
}
