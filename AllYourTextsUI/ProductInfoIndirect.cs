using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AllYourTextsLib;

namespace AllYourTextsUi
{
    /// <summary>
    /// Workaround class until I figure out how to access ProgramInfo class from 
    /// </summary>
    public static class ProductInfoIndirect
    {
        public static string ProductName
        {
            get
            {
                return ProductInfo.ProductName;
            }
        }

        public static string ProductDescription
        {
            get
            {
                return ProductInfo.ProductDescription;
            }
        }

        public static string PublisherName
        {
            get
            {
                return ProductInfo.PublisherName;
            }
        }

        public static string Version
        {
            get
            {
                return ProductInfo.Version;
            }
        }

        public static string Copyright
        {
            get
            {
                return ProductInfo.Copyright;
            }
        }
    }
}
