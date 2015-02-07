using Spanglish.Util;
using Spanglish.Validators;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models 
{
    public class Lesson : ValidableObject, IEquatable<Lesson>
    {
        string _name = null;
        private string _firstLangName;
        private string _secondLangName;

        private readonly IValidateString _langNameValidatorService;

        public Lesson()
        {
            _langNameValidatorService = new ValidateLanguageDefinitionService();
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int? UserId { set; get; }

        [NotNull, MaxLength(Constants.MaxLangNameLength)]
        public String FirstLangName
        {
            get { return _firstLangName; }
            set
            {
                _firstLangName = value;
                OnPropertyChanged("FirstLangName");
                ValidateProperty("FirstLangName", _firstLangName,
                    (p) => _langNameValidatorService.ValidateString(_firstLangName));
            }
        }

        [NotNull, MaxLength(Constants.MaxLangNameLength)]

        public String SecondLangName
        {
            get { return _secondLangName; }
            set
            {
                _secondLangName = value;
                OnPropertyChanged("SecondLangName");
                ValidateProperty("SecondLangName", _secondLangName,
                    (p) => _langNameValidatorService.ValidateString(_secondLangName));
            }
        }


        [NotNull, MaxLength(Constants.MaxLessonNameLength), Unique]
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

        public bool Equals(Lesson other)
        {
            return Name == other.Name &&
                UserId == other.UserId;
        }
    }
}
