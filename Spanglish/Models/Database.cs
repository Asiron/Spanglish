using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite.Net;
using SQLite.Net.Platform.Win32;
using Spanglish.Util;

namespace Spanglish.Models
{
    /*
     * Database object, handles connection to the database
     * and is implemented with Singleton pattern, yes I know it's an antipattern
     */ 
    public class Database : Singleton<Database>
    {
        public string Filename { get; set; }

        public Database() : this(Constants.ProductionDatabasePath)
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
