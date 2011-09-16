using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BVSoftware.Cryptography
{
    public sealed class KeyGenerator
    {
        public static string Generate256bitKey()
        {
            byte[] k = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                k[i] = Convert.ToByte(RandomInteger(255));
            }
            return BitConverter.ToString(k);
        }

        private static int RandomInteger(int MaxValue)
        {
            // Create a byte array to hold the random value.
            byte[] randomNumber = new byte[1];

            // Create a new instance of the RNGCryptoServiceProvider.
            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // Fill the array with a random value.
            Gen.GetBytes(randomNumber);

            // Convert the byte to an integer value to make the modulus operation easier.
            int rand = Convert.ToInt32(randomNumber[0]);

            // Return the random number mod the number max value
            return rand % MaxValue;
        }
      
    }
}
