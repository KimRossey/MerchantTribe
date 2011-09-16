using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Cryptography
{
    public sealed class Base64
    {

        public static string ConvertStringToBase64(string input)
        {
            byte[] data = Conversion.StringToBytes(input);
            string output = ConvertToBase64(data);
            return output;
        }
        public static string ConvertStringFromBase64(string input)
        {
            byte[] data = ConvertFromBase64(input);
            string output = Conversion.BytesToString(data);
            return output;
        }

        public static string ConvertToBase64(byte[] input)
        {
            string output = string.Empty;
            output = Convert.ToBase64String(input, 0, input.Length);                        
            return output;            
        }        

        public static byte[] ConvertFromBase64(string input)
        {
            byte[] output = Convert.FromBase64String(input);
            return output;
        }

    }

}
