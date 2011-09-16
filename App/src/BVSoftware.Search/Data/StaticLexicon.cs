using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Data
{


    public class StaticLexicon : ILexiconProvider
    {

        private List<LexiconWord> words = new List<LexiconWord>();

        #region ILibraryProvider Members

        public long FindWordId(string stemmedWord)
        {
            var word = (from w in words
                        where w.Word == stemmedWord
                        select w.Id).SingleOrDefault();

            if (word < 1)
            {
                return 0;
            }
            else
            {
                return (long)word;
            }
        }

        public long InsertWord(string stemmedWord)
        {
            long max = 0;
            if (words.Count > 0)
            {
                var max2 = (from w in words
                            select w.Id).Max();
                max = (long)max2;
                max += 1;
            }
            else
            {
                max = 1;
            }

            words.Add(new LexiconWord() { Id = max, Word = stemmedWord });

            return max;
        }

        #endregion

        public List<long> FindAllWordIds(List<string> stemmedWords)
        {
            List<long> result = new List<long>();

            var word = (from w in words
                        where stemmedWords.Contains(w.Word)
                        select w.Id).ToList();

            if (word != null)
            {
                foreach (var l in word)
                {
                    result.Add(l);
                }
            }

            return result;
        }

        public List<LexiconWord> FindAllWords(int pageNumber, int pageSize, ref long totalCount)
        {
            List<LexiconWord> result = new List<LexiconWord>();

            var wd = (from w in words
                      select w);

            if (wd != null)
            {
                totalCount = wd.Count();

                var page = wd.Skip(pageNumber * pageSize).Take(pageSize);

                foreach (LexiconWord l in page)
                {
                    result.Add(l);
                }
            }

            return result;
        }

    }
}
