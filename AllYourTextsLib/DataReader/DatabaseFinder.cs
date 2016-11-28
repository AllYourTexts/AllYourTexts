using AllYourTextsLib.Framework;
using System;
using System.IO;

namespace AllYourTextsLib.DataReader
{
    public class DatabaseFinder
    {
        public static string FindContactDatabasePath(IPhoneDeviceInfo deviceInfo)
        {
            if (string.IsNullOrEmpty(deviceInfo.BackupPath))
            {
                throw new ArgumentException("Database path cannot be empty.");
            }

            const string ContactDatabaseFilename = @"31bb7ba8914766d4ba40d6dfb6113c8b614be442";
            string contactDatabasePath;

            // Starting with iOS 10 the database file was moved into a sub-directory
            // that starts with the same two characters as the database filename.
            //
            // Unsure if this is an > iOS 10 change or it's based on the iTunes version
            // but according to the bug report it *seems* to be iOS-based.

            if ((deviceInfo.OsVersion == null) || (deviceInfo.OsVersion.MajorVersion < 10))
            {
                contactDatabasePath = Path.Combine(deviceInfo.BackupPath, ContactDatabaseFilename);
            }
            else
            {
                contactDatabasePath = Path.Combine(deviceInfo.BackupPath, "31", ContactDatabaseFilename);
            }
            if (!File.Exists(contactDatabasePath))
            {
                contactDatabasePath = AppendDatabaseFilenameExtension(contactDatabasePath);
            }

            return contactDatabasePath;
        }

        public static string FindTextMessageDatabasePath(IPhoneDeviceInfo deviceInfo)
        {
            if (string.IsNullOrEmpty(deviceInfo.BackupPath))
            {
                throw new ArgumentException("Database path cannot be empty.");
            }

            const string SmsDatabaseFilename = @"3d0d7e5fb2ce288813306e4d4636395e047a3d28";
            string textMessageDatabasePath;

            // Starting with iOS 10 the database file was moved into a sub-directory
            // that starts with the same two characters as the database filename.
            //
            // Unsure if this is an > iOS 10 change or it's based on the iTunes version
            // but according to the bug report it *seems* to be iOS-based.
           
            if ((deviceInfo.OsVersion == null) || (deviceInfo.OsVersion.MajorVersion < 10))
            {
                textMessageDatabasePath = Path.Combine(deviceInfo.BackupPath, SmsDatabaseFilename);
            }
            else
            {
                textMessageDatabasePath = Path.Combine(deviceInfo.BackupPath, "3d", SmsDatabaseFilename);
            }

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
