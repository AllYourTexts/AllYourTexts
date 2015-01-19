using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using System.IO;
using System.Xml;
using Microsoft.Win32;

namespace AllYourTextsLib.DataReader
{
    public class PhoneDeviceInfoReader
    {
        public PhoneDeviceInfoReader()
        {
            ;
        }

        public static IEnumerable<IPhoneDeviceInfo> GetDevicesInfo()
        {
            List<IPhoneDeviceInfo> phoneDevices = new List<IPhoneDeviceInfo>();
            RegistryKey shellFolders = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders");
            string appDataPath = (string)shellFolders.GetValue("AppData");
            const string backupSubfolder = @"Apple Computer\MobileSync\Backup\";

            string backupPath = Path.Combine(appDataPath, backupSubfolder);
            
            string[] subDirs;
            try
            {
                subDirs = Directory.GetDirectories(backupPath);
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new MissingBackupPathException(backupPath, ex);
            }

            if (subDirs.Length == 0)
            {
                throw new NoBackupsFoundException(backupPath);
            }

            MissingBackupFileException missingFileException = null;

            foreach (string subDir in subDirs)
            {
                string infoXmlPath = Path.Combine(subDir, "Info.plist");
                if (!File.Exists(infoXmlPath))
                {
                    //
                    // Ignore missing info.plist file directories if a valid directory is eventually found, but
                    // if nothing is ever found, save the info to throw an exception.
                    //

                    if (missingFileException == null)
                    {
                        missingFileException = new MissingBackupFileException(infoXmlPath);
                    }
                    continue;
                }
                phoneDevices.Add(ReadPhoneDeviceInfo(infoXmlPath));
            }

            if (phoneDevices.Count == 0)
            {
                if (missingFileException != null)
                {
                    throw missingFileException;
                }
                else
                {
                    throw new NoBackupsFoundException(backupPath);
                }
            }

            return phoneDevices;
        }

        private static IPhoneDeviceInfo ReadPhoneDeviceInfo(string infoFilePath)
        {
            using (FileStream infoFileStream = File.OpenRead(infoFilePath))
            {
                StreamReader infoFileStreamReader = new StreamReader(infoFileStream);
                string infoFileContents = infoFileStreamReader.ReadToEnd();
                return PhoneDeviceInfoFromFileContents(infoFilePath, infoFileContents);
            }
        }

        private static IPhoneDeviceInfo PhoneDeviceInfoFromFileContents(string filePath, string infoFileContents)
        {
            string backupPath = Path.GetDirectoryName(filePath);

            IOsVersion DefaultOsVersion = new OsVersion("4.0.0");

            try
            {
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.DtdProcessing = DtdProcessing.Ignore;
                using (XmlReader infoXml = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes((infoFileContents))), readerSettings))
                {
                    return PhoneDeviceInfoFromXmlReader(backupPath, infoXml);
                }
            }
            catch (Exception)
            {
                return new PhoneDeviceInfo(backupPath, null, null, DefaultOsVersion, null);
            }
        }

        private static IPhoneDeviceInfo PhoneDeviceInfoFromXmlReader(string backupPath, XmlReader infoXmlReader)
        {
            string displayName = null;
            DateTime? lastBackupDate = null;
            IOsVersion osVersion = null;
            Guid? deviceGuid = null;

            try
            {
                infoXmlReader.ReadToDescendant("dict");
                if (!infoXmlReader.IsStartElement())
                {
                    throw new InvalidDataException("Value dictionary not found.");
                }
                infoXmlReader.ReadToDescendant("key");

                while (!infoXmlReader.EOF)
                {
                    try
                    {
                        string keyName = infoXmlReader.ReadString();

                        switch (keyName)
                        {
                            case "Display Name":
                                displayName = ReadNextStringNode(infoXmlReader);
                                break;
                            case "Last Backup Date":
                                lastBackupDate = ReadNextDateNode(infoXmlReader);
                                break;
                            case "Product Version":
                                string versionString = ReadNextStringNode(infoXmlReader);
                                osVersion = new OsVersion(versionString);
                                break;
                            case "GUID":
                                deviceGuid = new Guid(ReadNextStringNode(infoXmlReader));
                                break;
                            default:
                                break;
                        }
                        infoXmlReader.ReadToNextSibling("key");
                    }
                    catch (XmlException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        ;
                    }
                }
            }
            catch (Exception)
            {
                ;
            }

            return new PhoneDeviceInfo(backupPath, displayName, lastBackupDate, osVersion, deviceGuid);
        }

        private static string ReadNextStringNode(XmlReader xmlReader)
        {
            xmlReader.ReadEndElement();
            xmlReader.MoveToContent();
            if (xmlReader.Name != "string")
            {
                throw new InvalidDataException("Value not found after key.");
            }

            return xmlReader.ReadString();
        }

        private static DateTime ReadNextDateNode(XmlReader xmlReader)
        {
            xmlReader.ReadEndElement();
            xmlReader.MoveToContent();
            if (xmlReader.Name != "date")
            {
                throw new InvalidDataException("Value not found after key.");
            }

            return DateTime.Parse(xmlReader.ReadString());
        }
    }
}
