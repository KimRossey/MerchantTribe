using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Data
{
    public interface ISearchObjectProvider
    {
        long ObjectIndexInsert(SearchObject s);
        SearchObject ObjectIndexFind(long id);
        List<SearchObject> ObjectIndexFindAllInList(List<long> ids);
        SearchObject ObjectIndexFindByTypeAndId(long siteId, int type, string objectId);
        bool ObjectIndexObjectExists(long siteId, int type, string objectId);
        bool ObjectIndexDelete(long id);

        void ObjectWordIndexDeleteForObject(long id);
        bool ObjectWordIndexDelete(SearchObjectWord w);
        bool ObjectWordIndexInsert(SearchObjectWord w);
        List<SearchObjectWord> ObjectWordIndexFindAll();
        List<SearchObjectWord> ObjectWordIndexFindByWordId(long siteId, long wordId);

        List<SearchObject> DoSearch(List<long> wordIds, int pageNumber, int pageSize, ref int totalResults);
        List<SearchObject> DoSearchBySite(long siteId, List<long> wordIds, int pageNumber, int pageSize, ref int totalResults);

    }
}
