﻿using Spanglish.Util;
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
    public class Word : ValidableObject  
    {

        string _firstLangDefinition = null;
        string _secondLangDefinition = null;
        string _imagePath = null;
        byte? _level = null;
        int? _lessonId = null;

        private readonly IValidateInteger _levelValidationService;

        bool _isNew = false;

        public Word()
        {
            _levelValidationService = new ValidateLevelService();
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
               _level = value;
                OnPropertyChanged("Level");
                ValidateProperty("Level", _level, (p) => _levelValidationService.ValidateInteger(p as byte?));
            }
        }

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
