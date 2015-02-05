using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spanglish.Util
{
    public static class Constants
    {
        public const int MinUsernameLength = 4;
        public const int MaxUsernameLength = 20;
        public const int MinLoginLength = 4;
        public const int MaxLoginLength = 20;
        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 20;
        public const int MaxWordLevel = 10;
        public const int MinLessonNameLength = 2;
        public const string PathToRoot = "../../../";
        public const string DatabaseFileFolder = "database";
        public const string ImageFolder = "database/images";
        public const string ProductionDatabaseName = "spanglish_db.sqlite";
        public const string TestDatabaseName = "test_db.sqlite";
        public const string ProductionDatabasePath = Constants.PathToRoot + Constants.DatabaseFileFolder + "/" + Constants.ProductionDatabaseName;
    }
}
