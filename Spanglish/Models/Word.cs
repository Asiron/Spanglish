using Spanglish.Misc;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models
{
    public class Word
    {

        string _firstLangDefinition = null;
        string _secondLangDefinition = null;
        string _imagePath = null;
        byte? _level = null;
        int? _lessonId = null;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        [MaxLength(80), NotNull, Indexed(Name = "Def", Order = 1, Unique = true)]
        public string FirstLangDefinition
        {
            get { return _firstLangDefinition; }
            set { _firstLangDefinition = value; }
        }

        [MaxLength(80), NotNull, Indexed(Name = "Def", Order = 2, Unique = true)]
        public string SecondLangDefinition
        {
            get { return _secondLangDefinition; }
            set { _secondLangDefinition = value; }
        }

        [MaxLength(80)]
        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

        [NotNull]
        public int? LessonId
        {
            get { return _lessonId; }
            set { _lessonId = value; }
        }

        [NotNull]
        public byte? Level
        {
            get
            { 
                return _level;
            }
            set
            {
                if (value < 0 || value > Constants.MaxWordLevel)
                    throw new ArgumentException("ExceptionLevel");
                _level = value;
            }
        }

        public bool HasImage
        {
            get
            { 
                return !String.IsNullOrWhiteSpace(_imagePath);
            }
        }

    }
}
