using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BuildMaker.Core
{
    public class HashTools
    {
        public static string GetSHA1HashForFile(string fullFileName)
        {
            string result = string.Empty;

            using (FileStream fs = new FileStream(fullFileName, FileMode.Open))
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(fs);
                StringBuilder formatted = new StringBuilder(hash.Length);
                foreach (byte b in hash)
                {
                    formatted.AppendFormat("{0:X2}", b);
                }

                result = formatted.ToString();
            }

            return result;
        }
    }
}
