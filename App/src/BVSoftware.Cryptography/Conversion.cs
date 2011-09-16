using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Cryptography
{
    public sealed class Conversion
    {
        // Converts a plain text string into an array of bytes
        public static byte[] StringToBytes(string inputCharacters)
        {
            return System.Text.Encoding.UTF8.GetBytes(inputCharacters);            
        }

        // Converts an array of bytes to a string
        public static string BytesToString(byte[] utf8Bytes)
        {
            return System.Text.Encoding.UTF8.GetString(utf8Bytes);
        }


        
        // Converts as string containing hex numbers into an array of bytes
        // Example: "FF-06" would convert to a byte[] {0xFF, 0x06} OR byte[] {255, 6}
        public static byte[] HexToByteArray(String hexString)
        {
            string working = hexString.Replace("-", "");
            working = working.Replace(" ", "");

            int NumberChars = working.Length;

            byte[] bytes = new byte[NumberChars / 2];

            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(working.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static string ByteArrayToHex(byte[] input)
        {
            return BitConverter.ToString(input);
        }


    }
}
