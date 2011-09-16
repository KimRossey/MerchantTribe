using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Search.Indexers
{
    public class SimpleTextIndexer
    {
        private ComplexIndexer indexer = null;

        public SimpleTextIndexer(Searcher s)
        {            
            indexer = new ComplexIndexer(s);
        }

        public void Index(long siteId, string documentId, int documentType, string title, string documentText)
        {
            List<string> parts = TextParser.ParseText(title + " " + documentText);
            if (parts == null)
            {
                return;
            }

            Dictionary<string, int> scoredParts = new Dictionary<string, int>();
            foreach (string s in parts)
            {
                if (scoredParts.ContainsKey(s))
                {
                    scoredParts[s] += 1;
                }
                else
                {
                    scoredParts.Add(s, 1);
                }
            }

            indexer.Index(siteId, documentId, documentType, title, scoredParts);           
        }
    }
}
