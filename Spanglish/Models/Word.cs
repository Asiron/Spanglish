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

        int? _lessonId = null;
        string _firstLangDefinition = null;
        string _secondLangDefinition = null;
        string _imagePath = null;
        byte? _level = null;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        [MaxLength(80), NotNull]
        public string DefinitionLang1
        {
            get { return _firstLangDefinition; }
            set { _firstLangDefinition = value; }
        }

        [MaxLength(80), NotNull]
        public string DefinitionLang2
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
