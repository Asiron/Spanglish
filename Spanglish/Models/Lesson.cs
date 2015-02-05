using Spanglish.Util;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models 
{
    public class Lesson : ObservableObject
    {
        string _name = null;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int? UserId { set; get; }

        [NotNull, MaxLength(20)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length < Constants.MinLessonNameLength)
                    throw new ArgumentException("ExceptionLessonNameTooShort");
                _name = value;
            }
        }
    }
}
