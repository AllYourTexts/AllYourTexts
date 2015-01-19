using AllYourTextsUi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AllYourTextsLib.Framework;
using System.Collections.Generic;
using AllYourTextsLib;
using AllYourTextsTest.Mock_Objects;

namespace AllYourTextsTest
{
    /// <summary>
    ///This is a test class for PhoneSelectorTest and is intended
    ///to contain all PhoneSelectorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PhoneSelectorTest
    {
        IPhoneDeviceInfo _PhoneA;
        IPhoneDeviceInfo _PhoneB;
        IPhoneDeviceInfo _PhoneC;
        IPhoneDeviceInfo _PhoneD;

        private const string _PathA = @"c:\fakepath\1";
        private const string _PathB = @"c:\fakepath\2";
        private const string _PathC = @"c:\fakepath\3";
        private const string _PathD = @"c:\fakepath\4";
        
        private const string NonExistentPath = @"c:\doesntexist\abc";

        [TestInitialize()]
        public void TestInitialize()
        {
            _PhoneA = new PhoneDeviceInfo(_PathA, "1", new DateTime(2012, 5, 13, 10, 1, 15), new OsVersion("5.4.3"), null);
            _PhoneB = new PhoneDeviceInfo(_PathB, "2", new DateTime(2011, 2, 15, 20, 18, 45), new OsVersion("5.4.3"), null);
            _PhoneC = new PhoneDeviceInfo(_PathC, "3", new DateTime(2010, 8, 28, 19, 14, 52), new OsVersion("4.1.2"), null);
            _PhoneD = new PhoneDeviceInfo(_PathD, "4", new DateTime(2010, 4, 15, 14, 45, 59), new OsVersion("4.1.1"), null);
        }

        private PhoneSelector_Accessor GetSelector(IEnumerable<IPhoneDeviceInfo> phones)
        {
            MockPhoneSelectOptions options = new MockPhoneSelectOptions();
            return new PhoneSelector_Accessor(phones, options);
        }

        private PhoneSelector_Accessor GetSelector(IEnumerable<IPhoneDeviceInfo> phones, bool alwaysPrompt, string autoSelectPath)
        {
            return GetSelector(phones, alwaysPrompt, autoSelectPath, true);
        }

        private PhoneSelector_Accessor GetSelector(IEnumerable<IPhoneDeviceInfo> phones, bool alwaysPrompt, string autoSelectPath, bool warnAboutRecentSync)
        {
            MockPhoneSelectOptions options = new MockPhoneSelectOptions();
            options.PromptForPhoneChoice = alwaysPrompt;
            options.PhoneDataPath = autoSelectPath;
            options.WarnAboutMoreRecentSync = warnAboutRecentSync;
            return new PhoneSelector_Accessor(phones, options);
        }

        private void VerifyNewestPhoneMatchesExpected(IEnumerable<IPhoneDeviceInfo> phones, IPhoneDeviceInfo newestExpected)
        {
            PhoneSelector_Accessor selector = GetSelector(phones);
            IPhoneDeviceInfo newestActual = selector.GetMostRecentlySyncedDeviceInfo();
            Assert.AreEqual(newestExpected, newestActual);
        }

        [TestMethod()]
        public void GetMostRecentlySyncedDeviceInfoTest()
        {
            VerifyNewestPhoneMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, _PhoneA);
            VerifyNewestPhoneMatchesExpected(new IPhoneDeviceInfo[] { _PhoneD, _PhoneC, _PhoneB, _PhoneA }, _PhoneA);
            VerifyNewestPhoneMatchesExpected(new IPhoneDeviceInfo[] { _PhoneC, _PhoneB }, _PhoneB);
            VerifyNewestPhoneMatchesExpected(new IPhoneDeviceInfo[] { _PhoneC, _PhoneA, _PhoneB }, _PhoneA);
            VerifyNewestPhoneMatchesExpected(new IPhoneDeviceInfo[] { _PhoneD }, _PhoneD);
        }

        private void VerifyPhoneWithPathMatchesExpected(IEnumerable<IPhoneDeviceInfo> phones, string path, IPhoneDeviceInfo matchingExpected)
        {
            PhoneSelector_Accessor selector = GetSelector(phones);
            IPhoneDeviceInfo matchingActual = selector.FindDeviceInfoWithPath(path);
            Assert.AreEqual(matchingExpected, matchingActual);
        }

        private void VerifyPhoneWithPathIsNotFound(IEnumerable<IPhoneDeviceInfo> phones, string path)
        {
            bool found;

            try
            {
                PhoneSelector_Accessor selector = GetSelector(phones);
                selector.FindDeviceInfoWithPath(path);
                found = true;
            }
            catch (KeyNotFoundException)
            {
                found = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Assert.IsFalse(found);
        }

        [TestMethod()]
        public void FindDeviceInfoWithPathTest()
        {
            VerifyPhoneWithPathMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, _PathA, _PhoneA);
            VerifyPhoneWithPathMatchesExpected(new IPhoneDeviceInfo[] { _PhoneD, _PhoneC, _PhoneB, _PhoneA }, _PathA, _PhoneA);
            VerifyPhoneWithPathMatchesExpected(new IPhoneDeviceInfo[] { _PhoneC, _PhoneB }, _PathB, _PhoneB);
            VerifyPhoneWithPathIsNotFound(new IPhoneDeviceInfo[] { _PhoneC, _PhoneA, _PhoneB }, @"c:\nonexistent\path\2");
            VerifyPhoneWithPathIsNotFound(new IPhoneDeviceInfo[] { }, @"c:\abc");
        }

        private void VerifyAutoSelectMatchesExpected(IEnumerable<IPhoneDeviceInfo> phones, bool alwaysPrompt, string autoSelectPath, IPhoneDeviceInfo autoSelectedExpected)
        {
            PhoneSelector_Accessor phoneSelector = GetSelector(phones, alwaysPrompt, autoSelectPath);

            IPhoneDeviceInfo autoSelectedActual = phoneSelector.AutoSelectPhoneDevice();
            Assert.AreEqual(autoSelectedExpected, autoSelectedActual);
        }

        [TestMethod()]
        public void AutoSelectPhoneDeviceTest()
        {

            //
            // Can't auto select because there's no default path and always prompt is on.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, true, null, null);

            //
            // Can't auto select because there's no default path.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, false, null, null);

            //
            // Can't auto select because default path doesn't match anything.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, false, NonExistentPath, null);

            //
            // Can't auto select because always prompt is on even though there's an auto-select path.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, true, _PathB, null);

            //
            // Can auto select because always prompt is off and there's a matching path.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC, _PhoneD }, false, _PathB, _PhoneB);

            //
            // Can auto select because there's only one option.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneC }, true, null, _PhoneC);

            //
            // Can auto select even if the path doesn't match because there's only one option.
            //

            VerifyAutoSelectMatchesExpected(new IPhoneDeviceInfo[] { _PhoneC }, false, NonExistentPath, _PhoneC);
        }

        private void VerifyShouldWarnMatchesExpected(IEnumerable<IPhoneDeviceInfo> phones, bool alwaysPrompt, string autoSelectPath, bool warnForNewer, bool shouldWarnExpected)
        {
            PhoneSelector_Accessor phoneSelector = GetSelector(phones, alwaysPrompt, autoSelectPath, warnForNewer);

            bool shouldWarnActual = phoneSelector.ShouldWarnAboutLaterSyncedPhone();
            Assert.AreEqual(shouldWarnExpected, shouldWarnActual);
        }

        [TestMethod()]
        public void ShouldWarnAboutLaterSyncedPhoneTest()
        {
            //
            // Don't warn because auto-selected phone is most recent.
            //

            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneC }, false, _PathA, true, false);

            //
            // Should warn that _PhoneA is more recent even if _PhoneC is auto-selected.
            //

            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneC }, false, _PathC, true, true);

            //
            // Should not warn about more recent phone because warnings are suppressed.
            //

            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneC }, false, _PathC, false, false);

            //
            // Don't warn if phone is auto-selected and no other phones exist.
            //

            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA }, false, null, true, false);

            //
            // Don't warn in any situation where there's no auto-select opportunity anyway.
            //

            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC }, true, _PathB, true, false);
            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC }, false, NonExistentPath, true, false);
            VerifyShouldWarnMatchesExpected(new IPhoneDeviceInfo[] { _PhoneA, _PhoneB, _PhoneC }, false, null, true, false);
        }
    }
}
