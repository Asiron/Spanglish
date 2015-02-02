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
    class Database
    {
        private string _filename;

        public string Filename
        {
            get
            {
                return _filename;
            }
            private set
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
                database.createTable<User>();
            }
        }

    }
}
