using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib.Framework;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AllYourTextsLib
{
    public class BugReportCreator : IBugReportCreator
    {
        public string CustomerEmail { get; set; }

        public string CustomerComments { get; set; }

        public Exception RelatedException { get; set; }

        public BugArea Area { get; set; }

        public bool ForceNewBugCreation { get; set; }

        public bool IncludesSystemInformation { get; set; }

        public bool IncludesVersionInformation { get; set; }

        public string BugServerResponse { get; private set; }

        public BugReportCreator()
        {
            CustomerEmail = null;
            Area = BugArea.FieldCrash;
            BugServerResponse = null;
        }

        public string EnvironmentInfo
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                if (IncludesSystemInformation)
                {
                    sb.AppendLine(GetSystemInformation());
                    sb.Append(Environment.NewLine);
                }
                if (IncludesVersionInformation)
                {
                    sb.AppendLine(GetVersionInformation());
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }
        }

        public string FullBugDetail
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (!string.IsNullOrEmpty(CustomerComments))
                {
                    sb.AppendLine("===Customer Comments===");
                    sb.AppendLine(CustomerComments);
                    sb.AppendLine();
                }

                if (!string.IsNullOrEmpty(EnvironmentInfo))
                {
                    sb.AppendLine("===Environment Information===");
                    sb.AppendLine(EnvironmentInfo);
                    sb.AppendLine();
                }

                if (RelatedException != null)
                {
                    sb.AppendLine("===Exception Information===");
                    sb.AppendLine(ExceptionToBugReportString(RelatedException));
                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }

        public bool Report(IBugReportCollector reportCollector)
        {
            const string BugProject = "Text Reader";
            const string BugDefaultMessage = "";

            reportCollector.Project = BugProject;
            reportCollector.Area = BugAreaToString(Area);
            reportCollector.DefaultMessage = BugDefaultMessage;
            
            if (CustomerEmail != null)
            {
                reportCollector.CustomerEmail = CustomerEmail;
            }
            reportCollector.Title = EscapeCharacters(GetBugReportTitle());
            reportCollector.ExtraInformation = EscapeCharacters(FullBugDetail);
            reportCollector.ForceNewBug = ForceNewBugCreation;

            string submitResponse = reportCollector.Submit();

            if (!string.IsNullOrEmpty(submitResponse))
            {
                BugServerResponse = submitResponse;
            }

            return string.IsNullOrEmpty(submitResponse);
        }

        private string GetBugReportTitle()
        {
            if (RelatedException != null)
            {
                return ExceptionToBugReportTitle(RelatedException);
            }
            else if (!string.IsNullOrEmpty(CustomerComments))
            {
                return GetBugTitleFromCustomerComments(CustomerComments);
            }
            else
            {
                Debug.Assert(false, "No information given to provide bug title.");
                ForceNewBugCreation = true;
                return "(no bug title)";
            }
        }

        private string GetSystemInformation()
        {
            string architecture;

            if (Environment.Is64BitOperatingSystem)
            {
                architecture = "x64";
            }
            else
            {
                architecture = "x86";
            }

            return string.Format("{0} ({1}){2}Machine Name: {3}", Environment.OSVersion.VersionString, architecture, Environment.NewLine, Environment.MachineName);
        }

        private string GetVersionInformation()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Assemblies");
            foreach (System.Reflection.Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                sb.AppendFormat("{0}    {1} (v.{2})", Environment.NewLine, asm.GetName().Name, asm.GetName().Version.ToString());
            }

            return sb.ToString();
        }

        private static string BugAreaToString(BugArea area)
        {
            switch (area)
            {
                case BugArea.CustomerFeedback:
                    return "CustomerFeedback";
                case BugArea.FieldCrash:
                    return "FieldCrash";
                default:
                    Debug.Assert(false, "Unrecognized bug area.");

                    //
                    // Default to FieldCrash if area is unknown.
                    //

                    return "FieldCrash";
            }
        }

        private static string ExceptionToBugReportTitle(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            // We first want the class name of the exception that occured
            sb.Append(ex.GetType().Name);

            // If the exception has a property called ErrorCode, add the value of it to the desc
            Regex rePropertyName = new Regex("^(ErrorCode|HResult)$", RegexOptions.IgnoreCase);
            foreach (System.Reflection.PropertyInfo property in ex.GetType().GetProperties())
            {
                if (rePropertyName.Match(property.Name).Success)
                {
                    // Only deal with readable properties
                    if (property.CanRead)
                    {
                        // Only deal with properties that aren't indexed
                        if (property.GetIndexParameters().Length == 0)
                        {
                            // Only add property values that are not null
                            Object propertyValue = property.GetValue(ex, new Object[] { });
                            if (propertyValue != null)
                            {
                                // If the property value converted to a string yields the same name as the class
                                // name of the value, it is uninteresting
                                string propertyValueString = propertyValue.ToString();
                                if (propertyValueString != propertyValue.GetType().FullName)
                                    sb.AppendFormat(" {0}={1}", property.Name, propertyValueString);
                            }
                        }
                    }
                }
            }

            // Work out the first source code reference in the stacktrace and add the unique value for it
            Regex reSourceReference = new Regex("at\\s+.+\\.(?<methodname>[^)]+)\\(.*\\)\\s+in\\s+.+\\\\(?<filename>[^:\\\\]+):line\\s+(?<linenumber>[0-9]+)", RegexOptions.IgnoreCase);
            bool gotReference = false;
            if (ex.StackTrace != null)
            {
                foreach (string line in ex.StackTrace.Split('\n', '\r'))
                {
                    Match ma = reSourceReference.Match(line);
                    if (ma.Success)
                    {
                        sb.AppendFormat(" ({0}:{1}:{2})",
                            ma.Groups["filename"].Value,
                            ma.Groups["methodname"].Value,
                            ma.Groups["linenumber"].Value);
                        gotReference = true;
                        break;
                    }
                }
            }


            // If we didn't get a source reference (release compile ?), try to find a non-System.* reference
            if (!gotReference)
            {
                Regex reMethodReference = new Regex("at\\s+(?<methodname>[^(]+)\\(.*\\)", RegexOptions.IgnoreCase);
                if (ex.StackTrace != null)
                {
                    foreach (string line in ex.StackTrace.Split('\n', '\r'))
                    {
                        Match ma = reMethodReference.Match(line);
                        if (ma.Success)
                        {
                            if (!ma.Groups["methodname"].Value.ToUpper().StartsWith("SYSTEM."))
                            {
                                sb.AppendFormat(" ({0})",
                                    ma.Groups["methodname"].Value);
                                gotReference = true;
                                break;
                            }
                        }
                    }
                }
            }

            // If we can get the entry assembly, add the version number of it
            System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                System.Reflection.AssemblyName entryAssemblyName = entryAssembly.GetName();
                if (entryAssemblyName != null)
                {
                    if (entryAssemblyName.Version != null)
                    {
                        string version = String.Format("{0}.{1}.{2}.{3}",
                            entryAssemblyName.Version.Major,
                            entryAssemblyName.Version.Minor,
                            entryAssemblyName.Version.Build,
                            entryAssemblyName.Version.Revision);
                        sb.AppendFormat(" V{0}", version);
                    }
                }
            }

            // Return result
            return sb.ToString();
        }

        private static string ExceptionToBugReportString(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(ex.GetType().ToString());

            sb.Append("Stacktrace" + Environment.NewLine);
            foreach (string line in ex.StackTrace.Split('\n', '\r'))
            {
                if (line != null && line.Length > 0)
                {
                    sb.AppendFormat("{0}" + Environment.NewLine, line.Trim());
                }
            }
            sb.Append(Environment.NewLine);

            Regex reUnwantedProperties = new Regex(@"^(StackTrace|Source|TargetSite)$", RegexOptions.IgnoreCase);
            while (ex != null)
            {
                bool any = false;
                foreach (System.Reflection.PropertyInfo pi in ex.GetType().GetProperties())
                {
                    if (!reUnwantedProperties.Match(pi.Name).Success)
                    {
                        Object value = pi.GetValue(ex, new Object[] { });
                        if (value != null)
                        {
                            if (IsInteger(value))
                            {
                                sb.AppendFormat("{0}={1} (0x{1:X})" + Environment.NewLine, pi.Name, value);
                            }
                            else
                            {
                                sb.AppendFormat("{0}={1}" + Environment.NewLine, pi.Name, value);
                            }
                            any = true;
                        }
                    }
                }
                if (ex.InnerException != null)
                {
                    if (any)
                    {
                        sb.Append(Environment.NewLine);
                    }
                }
                ex = ex.InnerException;
            }

            return sb.ToString();
        }

        private static bool IsInteger(Object x)
        {
            try
            {
                Convert.ToInt32(x);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetBugTitleFromCustomerComments(string customerComments)
        {
            return GetFirstLine(customerComments);
        }

        private static string GetFirstLine(string text)
        {
            char[] lineEnders = { '.', '?', '!', '\r', '\n' };
            int shortestLineIndex = Math.Min(text.Length, 100);

            foreach (char lineEnder in lineEnders)
            {
                int lineEndIndex = text.IndexOf(lineEnder, 1);

                if (lineEndIndex > 0)
                {
                    shortestLineIndex = Math.Min(shortestLineIndex, lineEndIndex);
                }
            }

            return text.Substring(0, shortestLineIndex);
        }

        private string EscapeCharacters(string inputString)
        {
            string encodedString = Uri.EscapeUriString(inputString);

            encodedString = encodedString.Replace("\'", "%27");
            encodedString = encodedString.Replace("\"", "%22");

            return encodedString;
        }
    }
}
