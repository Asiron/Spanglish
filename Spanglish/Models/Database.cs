using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite.Net;
using SQLite.Net.Platform.Win32;
using Spanglish.Misc;

namespace Spanglish.Models
{
    public class Database : Singleton<Database>
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

        public Database() : this(Constants.ProductionDatabaseName)
        {
             
        }

        public Database(string filename)
        {
            Filename = filename;
            if (File.Exists(filename))
                return;

            using(var database = new SQLiteConnection(new SQLitePlatformWin32(), filename))
            {
                database.CreateTable<User>();
                database.CreateTable<Lesson>();
                database.CreateTable<Word>();
                database.CreateTable<History>();
            }               
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWin32(), Filename);
        }
    }
}
