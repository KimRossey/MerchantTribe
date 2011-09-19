using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class ToDoItem
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public bool IsComplete { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }

        public ToDoItem()
        {
            Id = 0;
            AccountId = 0;
            IsComplete = false;
            SortOrder = 0;
            Title = "New Item";
            Details = string.Empty;
        }
    }
}
