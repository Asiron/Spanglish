using Spanglish.Misc;
using Spanglish.ViewModels;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models
{
    public class Word : ObservableObject
    {

        string _firstLangDefinition = null;
        string _secondLangDefinition = null;
        string _imagePath = null;
        byte? _level = null;
        int? _lessonId = null;

        bool _isNew = false;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(80), NotNull]
        public string FirstLangDefinition
        {
            get { return _firstLangDefinition; }
            set 
            { 
                if ( String.IsNullOrWhiteSpace(value) )
                    throw new ArgumentException("FirstLangExcept"); 
                _firstLangDefinition = value; 
                OnPropertyChanged("FirstLangDefinition"); }
        }

        [MaxLength(80), NotNull]
        public string SecondLangDefinition
        {
            get { return _secondLangDefinition; }
            set
            {
                if ( String.IsNullOrWhiteSpace(value) )
                    throw new ArgumentException("SecondLangExcept");
                _secondLangDefinition = value; 
                OnPropertyChanged("SecondLangDefinition");
            }
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


        public bool IsModified { set; get; }

        [Ignore]
        public bool HasImage
        {
            get
            { 
                return !String.IsNullOrWhiteSpace(_imagePath);
            }
        }
    }
}
