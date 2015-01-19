using System;
using AllYourTextsLib;
using AllYourTextsLib.DataReader;
using AllYourTextsLib.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllYourTextsTest
{
    
    /// <summary>
    ///This is a test class for PhoneDeviceInfoReaderTest and is intended
    ///to contain all PhoneDeviceInfoReaderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PhoneDeviceInfoReaderTest
    {
        const string DummyXmlFileLocation = @"C:\somepath\apple\backups\0982340870329834\Info.plist";
        const string DummyBackupPath = @"C:\somepath\apple\backups\0982340870329834";

        const string DummyInfoXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Build Version</key>
	<string>8C148a</string>
	<key>Device Name</key>
	<string>Mobile Pedro</string>
	<key>Display Name</key>
	<string>Mobile Pedro</string>
	<key>GUID</key>
	<string>78192F0126BDDB053F952AE9EEA52DBE</string>
	<key>IMEI</key>
	<string>011981006282394</string>
	<key>Last Backup Date</key>
	<date>2010-12-12T09:35:10Z</date>
	<key>Product Type</key>
	<string>iPhone2,1</string>
	<key>Product Version</key>
	<string>4.2.1</string>
	<key>Serial Number</key>
	<string>86931KN13NR</string>
	<key>Sync Settings</key>
	<dict>
		<key>Calendar Day Limit</key>
		<integer>30</integer>
		<key>Calendars Collections</key>
		<array>
			<dict>
				<key>AMSCollectionDisplayName</key>
				<string>Calendar</string>
				<key>AMSCollectionFiltered</key>
				<false/>
				<key>AMSCollectionName</key>
				<string>Calendar</string>
				<key>AMSCollectionReadOnly</key>
				<false/>
			</dict>
		</array>
		<key>Data Class Info</key>
		<array>
			<dict>
				<key>kAMSDataClassEnabled</key>
				<false/>
				<key>kAMSDataClassName</key>
				<string>com.apple.Bookmarks</string>
				<key>kAMSDataClassReset</key>
				<false/>
			</dict>
			<dict>
				<key>kAMSDataClassEnabled</key>
				<false/>
				<key>kAMSDataClassName</key>
				<string>com.apple.Calendars</string>
				<key>kAMSDataClassReset</key>
				<false/>
			</dict>
			<dict>
				<key>kAMSDataClassEnabled</key>
				<true/>
				<key>kAMSDataClassName</key>
				<string>com.apple.Contacts</string>
				<key>kAMSDataClassReset</key>
				<false/>
			</dict>
			<dict>
				<key>kAMSDataClassEnabled</key>
				<false/>
				<key>kAMSDataClassName</key>
				<string>com.apple.MailAccounts</string>
				<key>kAMSDataClassReset</key>
				<true/>
			</dict>
			<dict>
				<key>kAMSDataClassEnabled</key>
				<true/>
				<key>kAMSDataClassName</key>
				<string>com.apple.Notes</string>
				<key>kAMSDataClassReset</key>
				<false/>
			</dict>
		</array>
		<key>Mail Accounts Collections</key>
		<array>
			<dict>
				<key>AMSCollectionDisplayName</key>
				<string>michael.lynch@gmail.com (IMAP:michael.lynch@imap.gmail.com)</string>
				<key>AMSCollectionFiltered</key>
				<true/>
				<key>AMSCollectionName</key>
				<string>IMAP:michael.lynch@imap.gmail.com</string>
			</dict>
		</array>
		<key>New Record Calendar Name</key>
		<string>Home</string>
	</dict>
	<key>Target Identifier</key>
	<string>5b16381be5e5b1455712d683078566071b1a4bdf</string>
	<key>Target Type</key>
	<string>Device</string>
	<key>Unique Identifier</key>
	<string>5B16381BE5E5B1455712D683078566071B1A4BDF</string>
	<key>iTunes Files</key>
	<dict>
		<key>IC-Info.sidb</key>
		<data>
      dummy data
    </data>
		<key>IC-Info.sidv</key>
		<data>
      dummy data
    </data>
		<key>PhotosFolderAlbums</key>
		<data>
      dummy data
    </data>
		<key>PhotosFolderName</key>
		<data>
      dummy data
    </data>
		<key>PhotosFolderPrefs</key>
		<data>
      dummy data
    </data>
		<key>iTunesApplicationIDs</key>
		<data>
      dummy data
    </data>
		<key>iTunesPlaylists</key>
		<data>
		f+DbWHzBhVWO1m9mfwUBeMle/EawSQu3
		</data>
		<key>iTunesPodcasts</key>
		<data>
      dummy data
    </data>
		<key>iTunesPrefs</key>
		<data>
		  dummy data
		</data>
		<key>iTunesPrefs.plist</key>
		<data>
      dummy data
    </data>
	</dict>
	<key>iTunes Settings</key>
	<dict>
		<key>LibraryApplications</key>
		<array>
			<string>com.bumptechnologies.bump</string>
			<string>com.facebook.Facebook</string>
			<string>com.fandango.fandango</string>
			<string>com.firemint.flightcontrol</string>
			<string>com.google.GVDialer</string>
			<string>com.imapl.iRetouch</string>
			<string>com.lexwarelabs.goodmorning</string>
			<string>com.lucasarts.Monkey1</string>
			<string>com.microsoft.bing</string>
			<string>com.pandora</string>
			<string>com.shazam.Shazam</string>
			<string>com.skype.skype</string>
			<string>com.yelp.yelpiphone</string>
			<string>net.sax.Privately</string>
			<string>org.onebusaway.iphone</string>
		</array>
		<key>SyncedApplications</key>
		<array>
			<string>com.bumptechnologies.bump</string>
			<string>com.facebook.Facebook</string>
			<string>com.fandango.fandango</string>
			<string>com.firemint.flightcontrol</string>
			<string>com.google.GVDialer</string>
			<string>com.lexwarelabs.goodmorning</string>
			<string>com.lucasarts.Monkey1</string>
			<string>com.shazam.Shazam</string>
			<string>com.skype.skype</string>
			<string>com.yelp.yelpiphone</string>
			<string>net.sax.Privately</string>
			<string>org.onebusaway.iphone</string>
		</array>
	</dict>
	<key>iTunes Version</key>
	<string>10.1</string>
</dict>
</plist>
";

        private void VerifyXmlMatchesDeviceInfo(string xmlData, IPhoneDeviceInfo deviceInfoExpected)
        {
            IPhoneDeviceInfo deviceInfoActual;
            deviceInfoActual = PhoneDeviceInfoReader_Accessor.PhoneDeviceInfoFromFileContents(DummyXmlFileLocation, xmlData);
            Assert.AreEqual(deviceInfoExpected, deviceInfoActual);
        }

        /// <summary>
        ///A test for PhoneDeviceInfoFromFileContents
        ///</summary>
        [TestMethod()]
        [DeploymentItem("AllYourTextsLib.dll")]
        public void PhoneDeviceInfoFromFileContentsTest()
        {
            VerifyXmlMatchesDeviceInfo(DummyInfoXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           "Mobile Pedro",
                                                           new DateTime(2010, 12, 12, 4, 35, 10),
                                                           new OsVersion("4.2.1"),
                                                           new Guid("78192F0126BDDB053F952AE9EEA52DBE")));

            const string MinimalXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Display Name</key>
	<string>Phoney the Phone</string>
	<key>GUID</key>
	<string>78192F0126BDDB053F952AE9EEA52DBE</string>
	<key>Last Backup Date</key>
	<date>2011-10-09T03:31:15Z</date>
	<key>Product Version</key>
	<string>5.1.4</string>
</dict>
</plist>
";
            VerifyXmlMatchesDeviceInfo(MinimalXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           "Phoney the Phone",
                                                           new DateTime(2011, 10, 08, 23, 31, 15),
                                                           new OsVersion("5.1.4"),
                                                           new Guid("78192F0126BDDB053F952AE9EEA52DBE")));

            const string MalformedXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Display Name</key>
	<string>Phoney the Phone</string>
	<key>GUID</key>
	<string>78192F0126BDDB053F952AE9EEA52DBE</string>
	<key>Last Backup Date</key>
	<date>2011-10-09T03:31:15Z</date>
	<key>Product Version</key>
	<string>4.3.5</string>
</plist>
";
            VerifyXmlMatchesDeviceInfo(MalformedXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           "Phoney the Phone",
                                                           new DateTime(2011, 10, 08, 23, 31, 15),
                                                           new OsVersion("4.3.5"),
                                                           new Guid("78192F0126BDDB053F952AE9EEA52DBE")));

            VerifyXmlMatchesDeviceInfo("",
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           null,
                                                           null,
                                                           null,
                                                           null));

            const string EmptyXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
</plist>
";
            VerifyXmlMatchesDeviceInfo(EmptyXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           null,
                                                           null,
                                                           null,
                                                           null));

            const string MissingNameXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Display Name</key>
	<key>GUID</key>
	<string>78192F0126BDDB053F952AE9EEA52DBE</string>
	<key>Last Backup Date</key>
	<date>2011-10-09T03:31:15Z</date>
	<key>Product Version</key>
	<string>4.2.1</string>
</dict>
</plist>
";
            VerifyXmlMatchesDeviceInfo(MissingNameXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           null,
                                                           new DateTime(2011, 10, 08, 23, 31, 15),
                                                           new OsVersion("4.2.1"),
                                                           new Guid("78192F0126BDDB053F952AE9EEA52DBE")));

            const string MissingNameKeyXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<string>Phoney the Phone</string>
	<key>GUID</key>
	<string>78192F0126BDDB053F952AE9EEA52DBE</string>
	<key>Last Backup Date</key>
	<date>2011-10-09T03:31:15Z</date>
	<key>Product Version</key>
	<string>4.2.1</string>
</dict>
</plist>
";
            VerifyXmlMatchesDeviceInfo(MissingNameKeyXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           null,
                                                           new DateTime(2011, 10, 08, 23, 31, 15),
                                                           new OsVersion("4.2.1"),
                                                           new Guid("78192F0126BDDB053F952AE9EEA52DBE")));

            const string MissingBackupDateXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Display Name</key>
	<string>Phoney the Phone</string>
	<key>GUID</key>
	<string>78192F0126BDDB053F952AE9EEA52DBE</string>
	<key>Last Backup Date</key>
	<key>Product Version</key>
	<string>4.2.1</string>
</dict>
</plist>
";
            VerifyXmlMatchesDeviceInfo(MissingBackupDateXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           "Phoney the Phone",
                                                           null,
                                                           new OsVersion("4.2.1"),
                                                           new Guid("78192F0126BDDB053F952AE9EEA52DBE")));

            const string InvalidBackupDateXmlFile =
@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
<dict>
	<key>Display Name</key>
	<string>Phoney the Phone</string>
	<key>GUID</key>
	<string>8590AC9825FD5E00F21BCDA09E9EEA52</string>
	<key>Last Backup Date</key>
	<date>20a1-10-09T0b:31:15Z</date>
	<key>Product Version</key>
	<string>4.2.1</string>
</dict>
</plist>
";
            VerifyXmlMatchesDeviceInfo(InvalidBackupDateXmlFile,
                                       new PhoneDeviceInfo(DummyBackupPath,
                                                           "Phoney the Phone",
                                                           null,
                                                           new OsVersion("4.2.1"),
                                                           new Guid("8590AC9825FD5E00F21BCDA09E9EEA52")));
        }
    }
}
