using System;
using System.IO;

namespace AllYourTextsLib.DataReader
{
    public class DatabaseFinder
    {
        public static string FindContactDatabasePath(string databaseFolderPath)
        {
            if (string.IsNullOrEmpty(databaseFolderPath))
            {
                throw new ArgumentException("Database path cannot be empty.");
            }

            const string ContactDatabaseFilename = @"31bb7ba8914766d4ba40d6dfb6113c8b614be442";
            string contactDatabasePath = Path.Combine(databaseFolderPath, ContactDatabaseFilename);

            if (!File.Exists(contactDatabasePath))
            {
                contactDatabasePath = AppendDatabaseFilenameExtension(contactDatabasePath);
            }

            return contactDatabasePath;
        }

        public static string FindTextMessageDatabasePath(string databaseFolderPath)
        {
            if (string.IsNullOrEmpty(databaseFolderPath))
            {
                throw new ArgumentException("Database path cannot be empty.");
            }

            const string SmsDatabaseFilename = @"3d0d7e5fb2ce288813306e4d4636395e047a3d28";
            string textMessageDatabasePath = Path.Combine(databaseFolderPath, SmsDatabaseFilename);

            if (!File.Exists(textMessageDatabasePath))
            {
                textMessageDatabasePath = AppendDatabaseFilenameExtension(textMessageDatabasePath);
            }

            return textMessageDatabasePath;
        }

        private static string AppendDatabaseFilenameExtension(string filename)
        {
            return filename + ".mddata";
        }
    }
}
