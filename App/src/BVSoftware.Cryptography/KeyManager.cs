using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Cryptography
{
    public static class KeyManager
    {
        public static string GenerateKey()
        {
            return KeyGenerator.Generate256bitKey();
        }

        public static string GetKey(long id)
        {
            // TODO: Support Multiple keys and switching keys
            return "88-5F-2E-6B-79-27-62-C2-DC-1B-C7-8A-60-37-E5-4E-D0-90-FB-4D-18-1E-66-6F-02-4F-A7-BA-16-B9-4E-3A";
        }
    }
}
