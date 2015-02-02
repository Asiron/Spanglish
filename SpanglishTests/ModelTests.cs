using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spanglish.Models;
using Spanglish.Misc;
using System.IO;

using SQLite.Net;
using SQLite.Net.Platform.Win32;

namespace SpanglishTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestUserDatabaseAndTableCreation()
        {

            File.Delete(Constants.TestDatabaseName);
            Database d = new Database(Constants.TestDatabaseName);
            Assert.IsTrue(File.Exists(Constants.TestDatabaseName));

            using (var db = new SQLiteConnection(new SQLitePlatformWin32(), d.Filename))
            {
                Assert.AreEqual(0, db.Table<User>().Count());
                //Assert.AreEqual(0, db.Table<Database.Lesson>().Count());
                //Assert.AreEqual(0, db.Table<Database.Word>().Count());
            }
        }
    }
}
