using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search
{
    public class Lexicon
    {

        private Data.ILexiconProvider provider = null;
        private Dictionary<string, long> localWordCache = new Dictionary<string, long>();
        private bool useLocal = false;

        public Lexicon(Data.ILexiconProvider p)
        {
            provider = p;
        }
        public Lexicon(Data.ILexiconProvider p, bool useLocalCache)
        {
            useLocal = useLocalCache;
            provider = p;
        }

        public void PreloadLocalCache()
        {
            long totalWords = 0;
            List<Data.LexiconWord> pageZero = provider.FindAllWords(0, 1, ref totalWords);

            int pageSize = 100;
            int totPages = TotalPages(totalWords, pageSize);
            for (int i = 0; i < totPages; i++)
            {
                List<Data.LexiconWord> page = provider.FindAllWords(i, pageSize, ref totalWords);
                foreach (Data.LexiconWord l in page)
                {
                    localWordCache.Add(l.Word, l.Id);
                }
            }
        }

        public long AddOrCreateWord(string stemmedWord)
        {
            long id = FindWordId(stemmedWord);

            if (id > 0)
            {
                return id;
            }
            else
            {
                id = provider.InsertWord(stemmedWord);
                if (useLocal)
                {
                    localWordCache.Add(stemmedWord, id);
                }
                return id;
            }
        }

        public long FindWordId(string stemmedWord)
        {
            long id = 0;

            if (useLocal)
            {
                if (localWordCache.ContainsKey(stemmedWord))
                {
                    id = localWordCache[stemmedWord];
                }
            }
            else
            {
                id = provider.FindWordId(stemmedWord);
            }

            return id;
        }

        public List<long> FindAllWordIds(List<string> stemmedWords)
        {
            return provider.FindAllWordIds(stemmedWords);
        }


        private static int TotalPages(long totalRecords, int pageSize)
        {
            int result = 1;

            if (totalRecords < pageSize)
            {
                return result;
            }
            else
            {
                int wholePages = (int)Math.Floor((double)totalRecords / (double)pageSize);
                int partialPages = (int)((double)totalRecords % (double)pageSize);
                if (partialPages > 0)
                {
                    result = wholePages + 1;
                }
                else
                {
                    result = wholePages;
                }
            }

            return result;
        }
    }
}
