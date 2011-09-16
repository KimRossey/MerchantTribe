using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Data
{
    public class SqlLexicon : ILexiconProvider
    {

        private string connectionString = string.Empty;
        private Sql.LinqDataDataContext context = null;

        public SqlLexicon(string connString)
        {
            connectionString = connString;
            context = new BVSoftware.Search.Data.Sql.LinqDataDataContext(connString);
        }

        #region ILexiconProvider Members

        public long InsertWord(string stemmedWord)
        {
            long result = 0;

            try
            {
                Sql.bv_SearchLexicon word = new BVSoftware.Search.Data.Sql.bv_SearchLexicon();
                word.Word = stemmedWord;
                context.bv_SearchLexicons.InsertOnSubmit(word);
                context.SubmitChanges();
                result = word.Id;
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        public long FindWordId(string stemmedWord)
        {
            long result = 0;

            try
            {
                var word = (from w in context.bv_SearchLexicons
                            where w.Word == stemmedWord
                            select w.Id).SingleOrDefault();
                if (word > 0)
                {
                    result = word;
                }
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        #endregion

        public List<long> FindAllWordIds(List<string> stemmedWords)
        {
            List<long> result = new List<long>();


            try
            {
                var words = (from w in context.bv_SearchLexicons
                             where stemmedWords.Contains(w.Word)
                             select w.Id);
                if (words != null)
                {
                    foreach (var l in words)
                    {
                        result.Add(l);
                    }
                }
            }
            catch
            {

            }

            return result;
        }

        public List<LexiconWord> FindAllWords(int pageNumber, int pageSize, ref long totalCount)
        {
            List<LexiconWord> result = new List<LexiconWord>();

            var wd = (from w in context.bv_SearchLexicons
                      select w);

            if (wd != null)
            {
                totalCount = wd.Count();

                var page = wd.Skip(pageNumber * pageSize).Take(pageSize);

                foreach (BVSoftware.Search.Data.Sql.bv_SearchLexicon l in page)
                {
                    result.Add(new LexiconWord() { Id = l.Id, Word = l.Word });
                }
            }

            return result;
        }
    }
}
