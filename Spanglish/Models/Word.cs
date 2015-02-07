using Spanglish.Util;
using Spanglish.Validators;
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
    public class Word : ValidableObject, IEquatable<Word>
    {
        public Word()
        {
            _levelValidationService = new ValidateLevelService();
        }

        public static Word CopyFrom(Word other)
        {
            return new Word()
            {
                Id = other.Id,
                FirstLangDefinition = other.FirstLangDefinition,
                SecondLangDefinition = other.SecondLangDefinition,
                Level = other.Level,
                LessonId = other.LessonId
            };
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(80), NotNull]
        public string FirstLangDefinition
        {
            get { return _firstLangDefinition; }
            set 
            { 
                _firstLangDefinition = value;
                OnPropertyChanged("FirstLangDefinition");
                ValidateProperty("FirstLangDefinition", _firstLangDefinition,
                    SimpleValidationPredicate("Cannot be blank", (p) => String.IsNullOrWhiteSpace(p as string)));
            }
        }

        [MaxLength(80), NotNull]
        public string SecondLangDefinition
        {
            get { return _secondLangDefinition; }
            set
            {
                _secondLangDefinition = value; 
                OnPropertyChanged("SecondLangDefinition");
                ValidateProperty("SecondLangDefinition", _secondLangDefinition,
                    SimpleValidationPredicate("Cannot be blank", (p) => String.IsNullOrWhiteSpace(p as string)));
            }
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
               _level = value;
                OnPropertyChanged("Level");
                ValidateProperty("Level", _level, (p) => _levelValidationService.ValidateInteger(p as byte?));
            }
        }

        public bool Equals(Word other)
        {
            return FirstLangDefinition == other.FirstLangDefinition &&
                SecondLangDefinition == other.SecondLangDefinition &&
                Level == other.Level &&
                LessonId == other.LessonId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + this.Id.GetHashCode();
                hash = hash * 23 + this.FirstLangDefinition.GetHashCode();
                hash = hash * 23 + this.SecondLangDefinition.GetHashCode();
                hash = hash * 23 + this.LessonId.GetHashCode();
                return hash;
            }
        }

        string _firstLangDefinition = null;
        string _secondLangDefinition = null;
        string _imagePath = null;
        byte? _level = null;
        int? _lessonId = null;

        private readonly IValidateInteger _levelValidationService;
    }
}
