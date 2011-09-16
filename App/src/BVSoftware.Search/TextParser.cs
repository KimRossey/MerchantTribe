using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search
{
    public class TextParser
    {
        public static List<string> ParseText(string fullQuery)
        {
            return ParseText(fullQuery, true, 10);
        }

        public static List<string> ParseText(string fullQuery, bool removeStopWords, int limit)
        {
            List<string> result = new List<string>();

            string charsRemoved = ReplaceNonAlphaNumeric(fullQuery);

            char[] working = charsRemoved.Trim().ToCharArray();

            bool parsingword = false;
            string word = string.Empty;
            for (int i = 0; i < working.Length; i++)
            {
                if (parsingword)
                {
                    if (working[i] == ' ')
                    {
                        if (removeStopWords)
                        {
                            string temp = Stemmer.StemSingleWord(word);
                            if (!IsStopWord(temp))
                            {
                                result.Add(temp);
                            }
                        }
                        else
                        {
                            result.Add(Stemmer.StemSingleWord(word));
                        }
                        word = string.Empty;
                        parsingword = false;
                    }
                    else
                    {
                        word += working[i];
                    }
                }
                else
                {
                    if (working[i] != ' ')
                    {
                        parsingword = true;
                        word += working[i];
                    }
                }
            }

            if (word.Length > 0)
            {
                result.Add(Stemmer.StemSingleWord(word));
            }

            if (result.Count > limit)
            {
                result = result.Take(limit).ToList();
            }

            return result;
        }

        private static string ReplaceNonAlphaNumeric(string input)
        {
            if (input == null)
            {
                return string.Empty;
            }

            char[] working = input.Trim().ToLowerInvariant().ToCharArray();

            for (int i = 0; i < working.Length; i++)
            {
                if (!IsAlphaNumericUnicode(working[i]))
                {
                    working[i] = ' ';
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(working);
            return sb.ToString();
        }

        private static bool IsAlphaNumericUnicode(char input)
        {
            int v = Convert.ToInt32(input);

            // code 34 is a quote and we want to remove them for this version of the search engine
            //if (v == 32 || v == 34 || v == 95) { return true; }
            if (v == 32 || v == 95) { return true; }
            if ((v >= 48) && (v <= 57)) { return true; }
            if ((v >= 65) && (v <= 90)) { return true; }
            if ((v >= 97) && (v <= 122)) { return true; }
            if ((v >= 192) && (v <= 246)) { return true; }
            if ((v >= 248) && (v <= 447)) { return true; }
            if ((v >= 452) && (v <= 696)) { return true; }

            return false;
        }

        public static bool IsStopWord(string stemmed)
        {
            foreach (string stopword in StopWords())
            {
                if (stemmed == stopword)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> StopWords()
        {
            List<string> result = new List<string>();
            result.Add("i");
            result.Add("a");
            result.Add("about");
            result.Add("an");
            result.Add("and");
            result.Add("are");
            result.Add("as");
            result.Add("at");
            result.Add("be");
            result.Add("by");
            result.Add("com");
            result.Add("de");
            result.Add("en");
            result.Add("for");
            result.Add("from");
            result.Add("how	");
            result.Add("in");
            result.Add("is");
            result.Add("it");
            result.Add("la");
            result.Add("of");
            result.Add("on");
            result.Add("or");
            result.Add("that");
            result.Add("the");
            result.Add("thi");
            result.Add("to");
            result.Add("was");
            result.Add("what");
            result.Add("when");
            result.Add("where");
            result.Add("who");
            result.Add("will");
            result.Add("with");
            result.Add("und");
            result.Add("www");

            return result;
        }
    }
}
