using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models
{
    public class History
    {
        int _correct = 0;
        int _errors  = 0;
        /// <summary>
        ///  TODO change password to hash
        /// </summary>

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserId { get; set; }

        [NotNull]
        public int WordId { get; set; }

        [NotNull]
        public DateTime LastTimeStudied { get; set; }

        public int Errors
        {
            get
            {
                return _errors;
            }
            set
            {
                if ( value < 0 )
                {
                    throw new ArgumentException("Errors count cannot be negative!");
                }
                _errors = value;
            }
        }

        public int Correct
        {
            get
            {
                return _correct;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Correct count cannot be negative!");
                }
                _correct = value;
            }
        }

        public int Total
        {
            get {
                return Errors + Correct;
            }
        }

        public float Accuracy
        {
            get {
                return Correct / (float)(Total);
            }
        }
    }
}
