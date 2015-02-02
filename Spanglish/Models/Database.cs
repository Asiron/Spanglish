using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite.Net;
using SQLite.Net.Platform.Win32;

namespace Spanglish.Models
{
    public class Database
    {
        private string _filename;

        public string Filename
        {
            get
            {
                return _filename;
            }
            set
            {
                _filename = value;
            }
        }

        public Database(string filename)
        {
            Filename = filename;
            if (File.Exists(filename))
                return;

            using(var database = new SQLite.Net.SQLiteConnection(new SQLitePlatformWin32(), filename))
            {
                database.CreateTable<User>();
            }
        }

    }
}
