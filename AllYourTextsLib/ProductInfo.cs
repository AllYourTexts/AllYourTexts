using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AllYourTextsLib
{
    public static class ProductInfo
    {
        public static string ProductName
        {
            get
            {
                return GetAssemblyAttribute<AssemblyProductAttribute>(a => a.Product);
            }
        }

        public static string ProductDescription
        {
            get
            {
                return GetAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description);
            }
        }

        public static string PublisherName
        {
            get
            {
                return GetAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company);
            }
        }

        public static string Version
        {
            get
            {
                return System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            }
        }

        public static string Copyright
        {
            get
            {
                return GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);
            }
        }

        private static string GetAssemblyAttribute<T>(Func<T, string> value)
            where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }
    }
}
