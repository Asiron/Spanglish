using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Spanglish.Models;
using Spanglish.Util;
using System.IO;

using SQLite.Net;
using SQLite.Net.Platform.Win32;

namespace SpanglishTests
{
    [TestClass]
    public class ModelTests
    {
        private string _pathToDatabase = Constants.PathToRoot + Constants.DatabaseFileFolder + "/" + Constants.TestDatabaseName;

        [TestMethod]
        public void TestDatabaseAndTableCreation()
        {

            File.Delete(_pathToDatabase);
            Database d = new Database(_pathToDatabase);
            Assert.IsTrue(File.Exists(_pathToDatabase));

            using (var db = new SQLiteConnection(new SQLitePlatformWin32(), d.Filename))
            {
                Assert.AreEqual(0, db.Table<User>().Count());
                Assert.AreEqual(0, db.Table<Lesson>().Count());
                //Assert.AreEqual(0, db.Table<Database.Word>().Count());
            }
        }

        [TestMethod]
        public void TestUserTableInsertion()
        {
            Database d = new Database(_pathToDatabase);
            using(var db = new SQLiteConnection(new SQLitePlatformWin32(), d.Filename))
            {                

                try
                {
                    db.Insert(new User { Login = "aaa", Name = "Maciej Żurad", Password = "123456" });
                    Assert.Fail("Exception with short login should appear");
                }
                catch (System.ArgumentException e)
                {
                    StringAssert.Contains(e.Message, "ExceptionLoginTooShort");
                }

                try
                {
                    db.Insert(new User { Login = "aaaaaa", Name = "BBB", Password = "123456" });
                    Assert.Fail("Exception with short name should appear");
                }
                catch (System.ArgumentException e)
                {
                    StringAssert.Contains(e.Message, "ExceptionUsernameTooShort");
                }


                db.Insert(new User { Login = "aaaaaa", Name = "BBBBB", Password = "123456" });
                try
                {
                    db.Insert(new User { Login = "aaaaaa", Name = "BBBBB", Password = "123456" });
                    Assert.Fail("Exception with unique constraint violation should appear");
                }
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "Constraint");
                }

                try
                {
                    db.Insert(new User { Name = "BBBBB", Password = "123456" });
                    Assert.Fail("Exception with not null constraint violation should appear");
                }
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "NOT NULL");
                }

                try
                {
                    db.Insert(new User { Name = "BBBBB", Login = "abcdef" });
                    Assert.Fail("Exception with not null constraint violation should appear");
                }
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "NOT NULL");
                }


            }
        }

        [TestMethod]
        public void TestLessonTableInsertion()
        {
            Database d = new Database(_pathToDatabase);
            using (var db = new SQLiteConnection(new SQLitePlatformWin32(), d.Filename))
            {
                db.Insert(new Lesson() { Name = "aaaa", UserId = 1 });
                try
                {
                    db.Insert(new Lesson() { Name = "aaaa", UserId = 1});
                    Assert.Fail("Unique name constraint should appear");
                } 
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "Constraint");
                }
                try
                {
                    db.Insert(new Lesson() { Name = "asdasdasdasd"});
                    Assert.Fail("NOTNULL user id constraint should appear");
                }
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "NOT NULL");
                }
            }
        }

        [TestMethod]
        public void TestWordTableInsertion()
        {
            Database d = new Database(_pathToDatabase);
            using (var db = new SQLiteConnection(new SQLitePlatformWin32(), d.Filename))
            {
                Lesson l = new Lesson() { Name = "vehicles" };


                // Level null
                try
                {
                    db.Insert(new Word()
                    { 
                        FirstLangDefinition = "aaaa",
                        SecondLangDefinition = "asas",
                        LessonId = l.Id
                    });
                    Assert.Fail("Exception with NOT NULL constraint violation should appear");
                } 
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "NOT NULL constraint failed: Word.Level");
                }

                // Lesson null
                try
                {
                    db.Insert(new Word()
                    { 
                        FirstLangDefinition = "aaaa", 
                        SecondLangDefinition = "asas", 
                        Level = 1  
                    });
                    Assert.Fail("Exception with NOT NULL constraint violation should appear");
                }
                catch (SQLiteException e)
                {
                    StringAssert.Contains(e.Message, "NOT NULL constraint failed: Word.LessonId");
                }

                db.Insert(new Word()
                { 
                    FirstLangDefinition = "a house",
                    SecondLangDefinition = "la casa", 
                    ImagePath = null,
                    Level = 0, 
                    LessonId = l.Id
                });

                int i = db.Table<Word>().Where(w => w.LessonId == l.Id).Count();
                Assert.AreEqual(1, i);
            } 
        }

        [TestMethod]
        public void TestHistoryTableInsertion()
        {
            using (var database = new SQLite.Net.SQLiteConnection(new SQLitePlatformWin32(), _pathToDatabase))
            {
                User u = new User() { Login = "maciek", Password = "1234567" };
                Lesson l = new Lesson() { Name = "body parts"};
                Word w = new Word()
                {
                    FirstLangDefinition = "mouth",
                    SecondLangDefinition = "la boca",
                    LessonId = l.Id,
                    Level = 0,
                    ImagePath = null
                };
                History h = new History()
                {
                    UserId = u.Id,
                    WordId = w.Id,
                    LastTimeCorrect = DateTime.Now

                };
                Assert.AreEqual(0, h.Correct);
                Assert.AreEqual(0, h.Wrong);
            }
        }
    }
}
