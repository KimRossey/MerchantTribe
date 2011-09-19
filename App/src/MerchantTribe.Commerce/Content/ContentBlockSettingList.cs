using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public class ContentBlockSettingList
    {

        public List<ContentBlockSettingListItem> Items { get; set; }

        public ContentBlockSettingList()
        {
            Items = new List<ContentBlockSettingListItem>();
        }

        public List<ContentBlockSettingListItem> FindList(string listName)
        {
            return Items.Where(y => y.ListName == listName).OrderBy(y => y.SortOrder).ToList();
        }
        public ContentBlockSettingListItem FindSingleItem(string bvin)
        {
            return Items.Where(y => y.Id == bvin).SingleOrDefault();
        }
        public bool AddItem(ContentBlockSettingListItem item)
        {
            int maxSort = FindMaxSort(item.ListName);
            item.SortOrder = maxSort + 1;
            Items.Add(item);
            return true;
        }
        public bool RemoveItem(string bvin)
        {
            ContentBlockSettingListItem item = FindSingleItem(bvin);
            if (item != null)
            {
                return Items.Remove(item);
            }

            return false;
        }
        public bool DeleteList(string listName)
        {
            Items.RemoveAll(y => y.ListName == listName);
            return true;
        }
        public int FindMaxSort(string listName)
        {
            int result = 0;

            List<ContentBlockSettingListItem> current = FindList(listName);
            if (current != null)
            {
                if (current.Count > 0)
                {
                    int max = current.Max(y => y.SortOrder);
                    return max;
                }
            }

            return result;
        }        
        public bool MoveItemUp(string bvin, string listName)
        {
            List<ContentBlockSettingListItem> current = FindList(listName);

            int previous = 1;
            int currentSort = 0;
            string previousId = string.Empty;
            foreach (ContentBlockSettingListItem item in current)
            {
                if (item.Id == bvin)
                {
                    currentSort = item.SortOrder;
                    item.SortOrder = previous;
                    break;
                }
                else
                {
                    previous = item.SortOrder;
                    previousId = item.Id;
                }
            }

            ContentBlockSettingListItem prev = FindSingleItem(previousId);
            if (prev != null)
            {
                prev.SortOrder = currentSort;
            }

            return true;
        }
        public bool MoveItemDown(string bvin, string listName)
        {
            List<ContentBlockSettingListItem> current = FindList(listName);

            int next = 1;
            int currentSort = 0;
            bool foundCurrent = false;

            foreach (ContentBlockSettingListItem item in current)
            {
                if (foundCurrent)
                {
                    next = item.SortOrder;
                    item.SortOrder = currentSort;
                    break;
                }

                if (item.Id == bvin)
                {
                    currentSort = item.SortOrder;
                    foundCurrent = true;
                }
                
            }

            if (next > 1)
            {
                ContentBlockSettingListItem cur = FindSingleItem(bvin);
                if (cur != null)
                {
                    cur.SortOrder = next;
                }
            }

            return true;
        }

    }
}
