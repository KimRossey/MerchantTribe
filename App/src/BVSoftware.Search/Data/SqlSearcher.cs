using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Data
{
    public class SqlSearcher : ISearchObjectProvider
    {
        private Sql.LinqDataDataContext context = null;

        public SqlSearcher(string connString)
        {
            context = new BVSoftware.Search.Data.Sql.LinqDataDataContext(connString);
        }

        #region ISearchObjectProvider Members

        public long ObjectIndexInsert(SearchObject s)
        {
            long result = 0;

            try
            {
                Sql.bv_SearchObject dataObject = new BVSoftware.Search.Data.Sql.bv_SearchObject();
                dataObject.ObjectId = s.ObjectId;
                dataObject.ObjectType = s.ObjectType;
                dataObject.Title = s.Title;
                dataObject.SiteId = s.SiteId;
                dataObject.LastIndexUtc = DateTime.UtcNow;
                context.bv_SearchObjects.InsertOnSubmit(dataObject);
                context.SubmitChanges();
                s.Id = dataObject.Id;
                result = s.Id;
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        public SearchObject ObjectIndexFind(long id)
        {
            SearchObject result = null;

            try
            {
                var o = (from objects in context.bv_SearchObjects
                         where objects.Id == id
                         select objects).FirstOrDefault();
                if (o != null)
                {
                    result = new SearchObject();
                    result.Id = o.Id;
                    result.ObjectId = o.ObjectId;
                    result.ObjectType = o.ObjectType;
                    result.Title = o.Title;
                    result.SiteId = o.SiteId;
                    result.LastIndexUtc = o.LastIndexUtc;
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public List<SearchObject> ObjectIndexFindAllInList(List<long> ids)
        {
            List<SearchObject> result = new List<SearchObject>();

            try
            {
                var o = (from objects in context.bv_SearchObjects
                         where ids.Contains(objects.Id)
                         select objects).ToList();
                if (o != null)
                {
                    foreach (var ob in o)
                    {
                        SearchObject so = new SearchObject();
                        so.Id = ob.Id;
                        so.ObjectId = ob.ObjectId;
                        so.ObjectType = ob.ObjectType;
                        so.Title = ob.Title;
                        so.SiteId = ob.SiteId;
                        so.LastIndexUtc = ob.LastIndexUtc;
                        result.Add(so);
                    }
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public SearchObject ObjectIndexFindByTypeAndId(long siteId, int type, string objectId)
        {
            SearchObject result = null;

            try
            {
                var o = (from objects in context.bv_SearchObjects
                         where objects.ObjectType == type &&
                         objects.ObjectId == objectId &&
                         objects.SiteId == siteId
                         select objects).FirstOrDefault();
                if (o != null)
                {
                    result = new SearchObject();
                    result.Id = o.Id;
                    result.ObjectId = o.ObjectId;
                    result.ObjectType = o.ObjectType;
                    result.Title = o.Title;
                    result.SiteId = o.SiteId;
                    result.LastIndexUtc = o.LastIndexUtc;
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }

        public bool ObjectIndexObjectExists(long siteId, int type, string objectId)
        {
            SearchObject o = ObjectIndexFindByTypeAndId(siteId, type, objectId);
            if (o != null)
            {
                return true;
            }
            return false;
        }

        public bool ObjectIndexDelete(long id)
        {
            var s = (from so in context.bv_SearchObjects
                     where so.Id == id
                     select so).SingleOrDefault();
            if (s != null)
            {
                context.bv_SearchObjects.DeleteOnSubmit(s);
                context.SubmitChanges();
                return true;
            }
            return false;
        }

        public void ObjectWordIndexDeleteForObject(long id)
        {
            var words = (from w in context.bv_SearchObjectWords
                         where w.SearchObjectId == id
                         select w);
            if (words != null)
            {
                context.bv_SearchObjectWords.DeleteAllOnSubmit(words);
                context.SubmitChanges();
            }
        }

        public bool ObjectWordIndexDelete(SearchObjectWord w)
        {
            var s = (from word in context.bv_SearchObjectWords
                     where word.SearchObjectId == w.SearchObjectId &&
                     word.WordId == w.WordId
                     select word).SingleOrDefault();
            if (s != null)
            {
                context.bv_SearchObjectWords.DeleteOnSubmit(s);
                context.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ObjectWordIndexInsert(SearchObjectWord w)
        {
            bool result = false;

            try
            {
                Sql.bv_SearchObjectWord word = new BVSoftware.Search.Data.Sql.bv_SearchObjectWord();
                word.Score = w.Score;
                word.SearchObjectId = w.SearchObjectId;
                word.WordId = w.WordId;
                word.SiteId = w.SiteId;
                context.bv_SearchObjectWords.InsertOnSubmit(word);
                context.SubmitChanges();
                result = true;
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public List<SearchObjectWord> ObjectWordIndexFindAll()
        {
            List<SearchObjectWord> result = new List<SearchObjectWord>();

            foreach (Sql.bv_SearchObjectWord w in context.bv_SearchObjectWords)
            {
                SearchObjectWord r = new SearchObjectWord();
                r.Score = w.Score;
                r.SearchObjectId = w.SearchObjectId;
                r.WordId = w.WordId;
                r.SiteId = w.SiteId;
            }

            return result;
        }

        public List<SearchObjectWord> ObjectWordIndexFindByWordId(long siteId, long wordId)
        {
            List<SearchObjectWord> result = new List<SearchObjectWord>();

            var words = (from w in context.bv_SearchObjectWords
                         where w.WordId == wordId && w.SiteId == siteId
                         orderby w.Score descending
                         select w);

            if (words != null)
            {
                foreach (Sql.bv_SearchObjectWord w in words)
                {
                    SearchObjectWord r = new SearchObjectWord();
                    r.Score = w.Score;
                    r.SearchObjectId = w.SearchObjectId;
                    r.WordId = w.WordId;
                    r.SiteId = w.SiteId;
                }
            }

            return result;
        }

        public List<SearchObject> DoSearch(List<long> wordIds, int pageNumber, int pageSize, ref int totalResults)
        {
            return DoSearchBySite(-1, wordIds, pageNumber, pageSize, ref totalResults);
        }

        public List<SearchObject> DoSearchBySite(long siteId, List<long> wordIds, int pageNumber, int pageSize, ref int totalResults)
        {
            List<SearchObject> results = new List<SearchObject>();

            int skip = (pageNumber - 1) * pageSize;
            if (skip < 0) skip = 0;

            if (siteId > 0)
            {
                var step1a = (from w in context.bv_SearchObjectWords
                             where w.SiteId == siteId && wordIds.Contains(w.WordId)
                             group w by w.SearchObjectId into g
                             select new { Id = g.Key, Score = g.Sum(y => y.Score), Count = g.Sum(y => 1) })
                         .OrderByDescending(y => y.Count).ThenByDescending(y => y.Score);
                
                totalResults = step1a.Count();
                var step1 = step1a.Skip(skip).Take(pageSize).ToList();
                

                List<long> objectIds = new List<long>();
                foreach (var s in step1)
                {
                    objectIds.Add(s.Id);
                }

                // Now find all objects but they are unsorted 
                List<SearchObject> unsorted = ObjectIndexFindAllInList(objectIds);
                if (unsorted != null)
                {
                    // Make sure results are sorted by keyword values
                    foreach (var s in step1)
                    {
                        SearchObject temp = unsorted.Where(y => y.Id == s.Id).FirstOrDefault();
                        if (temp != null)
                        {
                            results.Add(temp);
                        }
                    }
                }
            }
            else
            {
                var step1a = (from w in context.bv_SearchObjectWords
                             where wordIds.Contains(w.WordId)
                             group w by w.SearchObjectId into g
                             select new { Id = g.Key, Score = g.Sum(y => y.Score), Count = g.Sum(y => 1) })
                         .OrderByDescending(y => y.Count).ThenByDescending(y => y.Score);
                
                totalResults = step1a.Count();
                var step1 = step1a.Skip(skip).Take(pageSize).ToList();                

                List<long> objectIds = new List<long>();
                foreach (var s in step1)
                {
                    objectIds.Add(s.Id);
                }

                // Now find all objects but they are unsorted 
                List<SearchObject> unsorted = ObjectIndexFindAllInList(objectIds);
                if (unsorted != null)
                {
                    // Make sure results are sorted by keyword values
                    foreach (var s in step1)
                    {
                        SearchObject temp = unsorted.Where(y => y.Id == s.Id).SingleOrDefault();
                        if (temp != null)
                        {
                            results.Add(temp);
                        }
                    }
                }
            }
                                                
            return results;
        }

        #endregion
    }
}
