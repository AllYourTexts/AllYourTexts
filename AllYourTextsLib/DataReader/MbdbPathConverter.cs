using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace AllYourTextsLib.DataReader
{
    public class MbdbPathConverter
    {
        private static SHA1CryptoServiceProvider _cryptoTransformSHA1 = new SHA1CryptoServiceProvider();

        public static string MbdbPathToFilename(string mbdbPath)
        {
            if (mbdbPath == null)
            {
                return null;
            }

            string hashedPath = CalculateSHA1(mbdbPath);
            string filename = hashedPath.ToLower();

            return filename;
        }

        private static string CalculateSHA1(string text)
        {
            byte[] buffer = UTF8Encoding.UTF8.GetBytes(text);
            string hash = BitConverter.ToString(_cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }
    }
}
