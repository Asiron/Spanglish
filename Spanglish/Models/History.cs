using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Models
{
    /*
     * History keeps track of users progress on particular word
     * It is tied to a lesson and a user. It keeps information about
     * number of correct, incorrect and skipped answers as well as 
     * last timestamp of correctly guessing the word
     * 
     */
    public class History
    {

        public History(History historyEntry)
        {
            Id = historyEntry.Id;
            UserId = historyEntry.UserId;
            WordId = historyEntry.WordId;
            Correct = historyEntry.Correct;
            LastTimeCorrect = historyEntry.LastTimeCorrect;
            Wrong = historyEntry.Wrong;
            Skipped = historyEntry.Skipped;
        }

        public History() {}


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public int UserId { get; set; }

        [NotNull]
        public int WordId { get; set; }

        public DateTime LastTimeCorrect { get; set; }

        public static History CopyFrom(History other)
        {
            return new History()
            {
                Id = other.Id,
                UserId = other.UserId,
                WordId = other.WordId,
                Correct = other.Correct,
                LastTimeCorrect = other.LastTimeCorrect,
                Wrong = other.Wrong,
                Skipped = other.Skipped
            };
        }
        public int Wrong
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

        public int Skipped
        {
            get
            {
                return _skipped;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Skipped count cannot be negative!");
                }
                _skipped = value;
            }
        }

        [Ignore]
        public int Total
        {
            get {
                return Wrong + Correct + Skipped;
            }
        }

        [Ignore]
        public float Accuracy
        {
            get {
                return Correct * 100 / (float)(Total);
            }
        }

        private int _correct = 0;
        private int _errors = 0;
        private int _skipped = 0;
    }
}
