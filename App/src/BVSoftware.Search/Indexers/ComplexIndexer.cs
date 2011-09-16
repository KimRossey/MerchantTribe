using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Indexers
{
    public class ComplexIndexer
    {
        private Searcher searcher = null;

        public ComplexIndexer(Searcher s)
        {
            searcher = s;
        }

        public void Index(long siteId, string documentId, int documentType, string title, Dictionary<string, int> scoredParts)
        {           
            SearchObject doc = new SearchObject();
            doc.ObjectId = documentId;
            doc.ObjectType = documentType;
            doc.Title = title;
            doc.SiteId = siteId;
            long newObjectId = searcher.ObjectIndexAddOrUpdate(doc);

            foreach (KeyValuePair<string, int> scoredPart in scoredParts)
            {
                SearchObjectWord w = new SearchObjectWord();
                w.Score = scoredPart.Value;
                w.SearchObjectId = newObjectId;
                w.WordId = searcher.Lexicon.AddOrCreateWord(scoredPart.Key);
                w.SiteId = doc.SiteId;
                searcher.ObjectWordIndexUpdate(w);
            }
        }
    }
}
