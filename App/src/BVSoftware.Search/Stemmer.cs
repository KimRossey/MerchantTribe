using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search
{
    public static class Stemmer
    {
        public static string StemSingleWord(string word)
        {
            PorterStemmer stemmer = new PorterStemmer();
            char[] parts = word.Trim().ToLowerInvariant().ToCharArray();
            stemmer.add(parts, parts.Length);
            stemmer.stem();
            return stemmer.ToString();
        }
    }
}
