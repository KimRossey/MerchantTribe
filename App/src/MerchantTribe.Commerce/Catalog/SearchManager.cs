using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Search;
using MerchantTribe.Web.Search.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Catalog
{
    public class SearchManager
    {
        private const int INDEXREBUILDPAGESIZE = 1000;

        Searcher searcher = null;

        public SearchManager()
        {
            Init(WebAppSettings.ApplicationConnectionString);
        }
        public SearchManager(string connString)
        {
            Init(connString);
        }

        private void Init(string connString)        
        {
            Lexicon lex = new Lexicon(new SqlLexicon(connString));
            SqlSearcher provider = new SqlSearcher(connString);
            searcher = new Searcher(lex, provider);
        }

        public List<MerchantTribe.Web.Search.SearchObject> DoSearchForAllStores(string query, int pageNumber, int pageSize, ref int totalResults)
        {
            return searcher.DoSearch(query, pageNumber, pageSize, ref totalResults);
        }
        public List<SearchObject> DoSearch(long siteId, string query, int pageNumber, int pageSize, ref int totalResults)
        {
            return searcher.DoSearch(siteId, query, pageNumber, pageSize, ref totalResults);
        }

        
        public void RebuildProductSearchIndex(MerchantTribeApplication app)
        {
            MerchantTribe.Web.Search.Indexers.ComplexIndexer indexer = new MerchantTribe.Web.Search.Indexers.ComplexIndexer(searcher);
            int totalProducts = app.CatalogServices.Products.FindAllForAllStoresCount();
            int totalPages = MerchantTribe.Web.Paging.TotalPages(totalProducts, INDEXREBUILDPAGESIZE);

            for (int i = 1; i <= totalPages; i++)
            {
                IndexAPage(i, indexer, app);
            }
        }

        private void IndexAPage(int pageNumber, MerchantTribe.Web.Search.Indexers.ComplexIndexer indexer, MerchantTribeApplication app)
        {            
            int startIndex = MerchantTribe.Web.Paging.StartRowIndex(pageNumber, INDEXREBUILDPAGESIZE);
            List<Product> page = app.CatalogServices.Products.FindAllPagedForAllStores(startIndex, INDEXREBUILDPAGESIZE);
            if (page != null)
            {
                foreach (Product p in page)
                {
                    IndexProduct(p, indexer);
                }
            }
        }

        private void ParseAndValue(string text, SearchManagerImportance importance, Dictionary<string, int> scoredParts, int maxWordsToParse)
        {
            List<string> parts = TextParser.ParseText(text, true, maxWordsToParse);
            if (parts == null)
            {
                return;
            }
            
            foreach (string s in parts)
            {
                if (scoredParts.ContainsKey(s))
                {
                    scoredParts[s] += (int)importance;
                }
                else
                {
                    scoredParts.Add(s, (int)importance);
                }
            }

        }
        
        // Products
        public void IndexSingleProduct(Catalog.Product p)
        {
            MerchantTribe.Web.Search.Indexers.ComplexIndexer indexer = new MerchantTribe.Web.Search.Indexers.ComplexIndexer(searcher);
            IndexProduct(p, indexer);
        }
        public void RemoveSingleProduct(long storeId, string bvin)
        {
            SearchObject o = searcher.ObjectIndexFindByTypeAndId(storeId, (int)SearchManagerObjectType.Product, bvin);
            if (o == null) return;
            if (o.Id < 1) return;
            searcher.ObjectIndexDelete(o.Id);
        }
        //private void IndexProduct(string productBvin, BVSoftware.Search.Indexers.ComplexIndexer indexer)
        //{
        //     Product p = Catalog.Product.FindByBvinForAllStores(productBvin);          
        //     IndexProduct(p, indexer);          
        //}
        private void IndexProduct(Product p, MerchantTribe.Web.Search.Indexers.ComplexIndexer indexer)
        {
            if (p != null)
            {
                long storeId = p.StoreId;
                string documentId = p.Bvin;
                int documentType = (int)SearchManagerObjectType.Product;
                string title = p.ProductName + " | " + p.Sku;                
                Dictionary<string, int> scoredparts = new Dictionary<string, int>();

                ParseAndValue(p.Sku, SearchManagerImportance.Highest, scoredparts, 10);
                ParseAndValue(p.ProductName, SearchManagerImportance.Highest, scoredparts, 20);
                ParseAndValue(p.MetaTitle, SearchManagerImportance.High, scoredparts, 20);
                ParseAndValue(p.MetaKeywords, SearchManagerImportance.High, scoredparts, 20);

                ParseAndValue(p.MetaDescription, SearchManagerImportance.Normal, scoredparts, 20);
                ParseAndValue(p.LongDescription, SearchManagerImportance.Normal, scoredparts, 100);
                ParseAndValue(p.Keywords, SearchManagerImportance.Normal, scoredparts, 20);

                if (p.HasVariants())
                {
                    foreach (Variant v in p.Variants)
                    {
                        if (v.Sku != p.Sku)
                        {
                            ParseAndValue(v.Sku, SearchManagerImportance.Highest, scoredparts, 10);                                                        
                        }
                    }

                }

                string optiontext = string.Empty;
                if (p.HasOptions())
                {
                    foreach (Option opt in p.Options)
                    {
                        optiontext += opt.Name + " ";
                        foreach (OptionItem item in opt.Items)
                        {
                            optiontext += item.Name + " ";
                        }
                    }
                }
                ParseAndValue(optiontext, SearchManagerImportance.NormalHigh, scoredparts, 10);

                indexer.Index(storeId, documentId, documentType, title, scoredparts);
            }
        }
    }
}
