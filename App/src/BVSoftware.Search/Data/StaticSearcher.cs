using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Data
{
    public class StaticSearcher : ISearchObjectProvider
    {
        List<SearchObject> objects = new List<SearchObject>();
        List<SearchObjectWord> words = new List<SearchObjectWord>();

        #region ISearchObjectProvider Members

        public long ObjectIndexInsert(SearchObject s)
        {
            long max = 0;
            if (objects.Count > 0)
            {
                var max2 = (from o in objects
                            select o.Id).Max();
                max = (long)max2;
                max += 1;
            }
            else
            {
                max = 1;
            }

            s.Id = max;
            objects.Add(s);

            return max;
        }

        public SearchObject ObjectIndexFind(long id)
        {
            SearchObject result = null;

            var o = (from ob in objects
                     where ob.Id == id
                     select ob).FirstOrDefault();

            if (o != null)
            {
                return (SearchObject)o;
            }

            return result;
        }

        public List<SearchObject> ObjectIndexFindAllInList(List<long> ids)
        {
            List<SearchObject> result = new List<SearchObject>();

            try
            {
                var o = (from ob in objects
                         where ids.Contains(ob.Id)
                         select ob).ToList();
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
            var o = (from ob in objects
                     where ob.ObjectType == type &&
                     ob.ObjectId == objectId &&
                     ob.SiteId == siteId
                     select ob).FirstOrDefault();

            if (o != null)
            {
                return (SearchObject)o;
            }

            return null;
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
            SearchObject s = ObjectIndexFind(id);
            if (s != null)
            {
                objects.Remove(s);
                return true;
            }

            return false;
        }

        #endregion


        #region Object Word Index methods


        public void ObjectWordIndexDeleteForObject(long id)
        {
            words.RemoveAll(y => y.SearchObjectId == id);
        }

        public bool ObjectWordIndexDelete(SearchObjectWord w)
        {
            var d = (from wd in words
                     where wd.SearchObjectId == w.SearchObjectId &&
                     wd.WordId == w.WordId
                     select wd).FirstOrDefault();
            if (d != null)
            {
                words.Remove(d);
            }

            return true;
        }

        public bool ObjectWordIndexInsert(SearchObjectWord w)
        {
            words.Add(w);
            return true;
        }

        public List<SearchObjectWord> ObjectWordIndexFindAll()
        {
            return words;
        }
        #endregion

        public List<SearchObjectWord> ObjectWordIndexFindByWordId(long siteId, long wordId)
        {
            List<SearchObjectWord> result = new List<SearchObjectWord>();

            if (siteId > 0)
            {
                var wd = (from w in words
                          where w.WordId == wordId
                          && w.SiteId == siteId
                          orderby w.Score descending
                          select w).ToList();
                if (wd != null)
                {
                    return (List<SearchObjectWord>)wd;
                }
            }
            else
            {
                var wd = (from w in words
                          where w.WordId == wordId
                          orderby w.Score descending
                          select w).ToList();
                if (wd != null)
                {
                    return (List<SearchObjectWord>)wd;
                }
            }

            return result;
        }


        public List<SearchObject> DoSearch(List<long> wordIds, int pageNumber, int pageSize, ref int totalResults)
        {
            return DoSearchBySite(0, wordIds, pageNumber, pageSize, ref totalResults);
        }

        public List<SearchObject> DoSearchBySite(long siteId, List<long> wordIds, int pageNumber, int pageSize, ref int totalResults)
        {
            List<SearchObject> results = new List<SearchObject>();

            int skip = (pageNumber - 1) * pageSize;
            if (skip < 0) skip = 0;

            if (siteId > 0)
            {
                var step1a = (from w in words
                              where wordIds.Contains(w.WordId) &&
                              w.SiteId == siteId
                              group w by w.SearchObjectId into g
                              select new { ObjectId = g.Key, Score = g.Sum(y => y.Score), Count = g.Sum(y => 1) })
                             .Where(y => y.Count >= wordIds.Count).OrderByDescending(y => y.Score);                
                totalResults = step1a.Count();

                var step1 = step1a.Skip(skip).Take(pageSize).ToList();
                foreach (var s in step1)
                {
                    results.Add(ObjectIndexFind(s.ObjectId));
                }
            }
            else
            {
                var step1a = (from w in words
                             where wordIds.Contains(w.WordId)
                             group w by w.SearchObjectId into g
                             select new { ObjectId = g.Key, Score = g.Sum(y => y.Score), Count = g.Sum(y => 1) })
                             .Where(y => y.Count >= wordIds.Count).OrderByDescending(y => y.Score);
                totalResults = step1a.Count();
                var step1 = step1a.Skip(skip).Take(pageSize).ToList();

                foreach (var s in step1)
                {
                    results.Add(ObjectIndexFind(s.ObjectId));
                }
            }

            return results;
        }


    }
}
