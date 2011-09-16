using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search
{
    public class Searcher
    {
        private Lexicon lexicon = null;
        private Data.ISearchObjectProvider provider = null;

        public Lexicon Lexicon { get { return lexicon; } }

        public Searcher(Lexicon l, Data.ISearchObjectProvider p)
        {
            lexicon = l;
            provider = p;
        }

        #region Object Index

        public long ObjectIndexAddOrUpdate(SearchObject s)
        {
            SearchObject existing = provider.ObjectIndexFindByTypeAndId(s.SiteId, s.ObjectType, s.ObjectId);
            if (existing != null)
            {
                long existingId = existing.Id;
                this.ObjectIndexDelete(existingId);
                return provider.ObjectIndexInsert(s);
            }
            else
            {
                return provider.ObjectIndexInsert(s);
            }
        }
        public SearchObject ObjectIndexFind(long id)
        {
            return provider.ObjectIndexFind(id);
        }
        public SearchObject ObjectIndexFindByTypeAndId(long siteId, int type, string objectId)
        {
            return provider.ObjectIndexFindByTypeAndId(siteId, type, objectId);
        }
        public bool ObjectIndexObjectExists(long siteId, int type, string objectId)
        {
            return provider.ObjectIndexObjectExists(siteId, type, objectId);
        }
        public bool ObjectIndexDelete(long id)
        {
            provider.ObjectWordIndexDeleteForObject(id);
            return provider.ObjectIndexDelete(id);
        }

        #endregion

        #region Object Word Index

        public bool ObjectWordIndexUpdate(SearchObjectWord w)
        {
            provider.ObjectWordIndexDelete(w);
            return provider.ObjectWordIndexInsert(w);
        }

        public List<SearchObjectWord> ObjectWordIndexFindAll()
        {
            return provider.ObjectWordIndexFindAll();
        }

        #endregion

        public List<SearchObjectWord> ObjectWordIndexFindByWordId(long siteId, long wordId)
        {
            return provider.ObjectWordIndexFindByWordId(siteId, wordId);
        }

        public List<SearchObject> DoSearch(string query, int pageNumber, int pageSize, ref int totalResults)
        {
            return DoSearch(-1, query, pageNumber, pageSize, ref totalResults);
        }

        public List<SearchObject> DoSearch(long siteId, string query, int pageNumber, int pageSize, ref int totalResults)
        {
            // Parse Query into words
            List<string> parts = TextParser.ParseText(query);


            // Get wordIds for all words in query
            List<long> wordIds = lexicon.FindAllWordIds(parts);

            if (siteId > 0)
            {
                return provider.DoSearchBySite(siteId, wordIds, pageNumber, pageSize, ref totalResults);
            }
            else
            {
                return provider.DoSearch(wordIds, pageNumber, pageSize, ref totalResults);
            }
        }
    }
}
