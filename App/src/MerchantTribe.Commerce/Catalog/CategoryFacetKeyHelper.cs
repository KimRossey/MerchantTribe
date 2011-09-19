using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class CategoryFacetKeyHelper
    {
        // Converts a text key to a list of longs
        public static List<long> ParseKeyToList(string key)
        {
            List<long> results = new List<long>();

            char splitter = '-';
            if (key.Contains(',')) splitter = ',';

            string temp = key.Trim();
            string[] parts = key.Split(splitter);
            foreach (string part in parts)
            {
                long number = 0;
                if (long.TryParse(part, out number))
                {
                    results.Add(number);
                }
            }

            return results;
        }

        // Converts a string key to a list of non-zero
        // long values for passing to SQL 
        public static string ParseKeyToSqlList(string key)
        {
            string result = string.Empty;
            List<long> parts = ParseKeyToList(key);
            List<long> nonzeroparts = parts.Where(y => y > 0).ToList();
            result = BuildKey(nonzeroparts, ",");
            return result;
        }
     
        // Converts a list of longs to a string key using the default separator of "-"
        public static string BuildKey(List<long> numbers)
        {
            return BuildKey(numbers, "-");
        }

        // Converts a list of longs to a string key with a separator specified
        public static string BuildKey(List<long> numbers, string separator)
        {
            string result = string.Empty;

            for (int i = 0; i < numbers.Count; i++)
            {
                result += numbers[i].ToString();
                if (i < numbers.Count - 1)
                {
                    result += separator;
                }
            }

            return result;
        }
                
        /// <summary>
        /// Creates an empty string key with the specified number of values
        /// </summary>
        /// <param name="count">How many places to hold</param>
        /// <returns></returns>
        public static string BuildEmptyKey(int count)
        {
            string key = string.Empty;
            for (int i = 0; i < count; i++)
            {
                key += "0";
                if (i < count - 1)
                {
                    key += "-";
                }
            }
            return key;
        }

        /// <summary>
        /// takes a string based key and replaced the value at a specific index with a new value
        /// </summary>
        /// <param name="key">key to modify</param>
        /// <param name="indexToReplace">index of location to modify</param>
        /// <param name="newValue">value to replace</param>
        /// <returns></returns>
        public static string ReplaceKeyValue(string key, int indexToReplace, long newValue)
        {
            List<long> parts = ParseKeyToList(key);
            if (indexToReplace < parts.Count)
            {
                parts[indexToReplace] = newValue;
            }
            return BuildKey(parts);
        }
        public static string ClearSelfAndChildrenFromKey(string key, List<CategoryFacet> allFacets, long idToClear)
        {
            string result = key;

            // replace self
            for (int i = 0; i < allFacets.Count; i++)
            {
                if (allFacets[i].Id == idToClear)
                {
                    result = ReplaceKeyValue(key, i, 0);
                    break;
                }
            }

            //replace children
            for (int i = 0; i < allFacets.Count; i++)
            {
                if (allFacets[i].ParentPropertyId == idToClear)
                {
                    result = ClearSelfAndChildrenFromKey(result, allFacets, allFacets[i].Id);
                }
            }

            return result;
        }
    }
}
