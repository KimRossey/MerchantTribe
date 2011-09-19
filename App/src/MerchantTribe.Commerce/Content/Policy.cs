using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace MerchantTribe.Commerce.Content
{
	
	public class Policy
	{
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }		        
		public string Title {get;set;}
		public bool SystemPolicy {get;set;}
		public List<PolicyBlock> Blocks {get;set;}
        public PolicyType Kind {get;set;}

        public Policy()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.Title = string.Empty;
            this.SystemPolicy = false;
            this.Blocks = new List<PolicyBlock>();
            this.Kind = PolicyType.TermsAndConditions;
        }

        public bool MoveBlockUp(string bvin)
        {
            bool bRet = false;


            try
            {
                if (Blocks.Count > 1)
                {
                    // we have more than 1 item so there is a chance to switch
                    int CurrentSort = -1;
                    int LowestSort;
                    int NewSort = 1;
                    string SwitchID = string.Empty;

                    // Find lowest sort order for type
                    LowestSort = Blocks[0].SortOrder;

                    int iCount;
                    for (iCount = 0; iCount <= Blocks.Count - 1; iCount++)
                    {
                        if (Blocks[iCount].Bvin == bvin)
                        {
                            CurrentSort = Blocks[iCount].SortOrder;
                        }

                        // no match yet
                        if (CurrentSort == -1)
                        {
                            NewSort = Blocks[iCount].SortOrder;
                            SwitchID = Blocks[iCount].Bvin;
                        }
                    }

                    // If we can move up, find the row to switch with
                    if (CurrentSort > LowestSort)
                    {
                        if (UpdateSortOrder(bvin, NewSort) == true)
                        {
                            if (UpdateSortOrder(SwitchID, CurrentSort) == true)
                            {
                                bRet = true;
                            }
                            else
                            {
                                bRet = false;
                                // Return current
                                UpdateSortOrder(bvin, CurrentSort);
                            }
                        }
                        else
                        {
                            bRet = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            return bRet;
        }
        public bool MoveBlockDown(string bvin)
        {
            bool bRet = false;

            try
            {
                if (Blocks.Count > 1)
                {
                    // we have more than 1 item so there is a chance to switch
                    int CurrentSort = -1;
                    int MaxSort = 0;
                    int NewSort = -1;
                    string SwitchID = string.Empty;

                    // Find lowest sort order for type
                    MaxSort = Blocks[Blocks.Count - 1].SortOrder;

                    int iCount;
                    for (iCount = 0; iCount <= Blocks.Count - 1; iCount++)
                    {

                        // got the match so grab this one
                        if (NewSort == -2)
                        {
                            NewSort = Blocks[iCount].SortOrder;
                            SwitchID = Blocks[iCount].Bvin;
                        }

                        if (Blocks[iCount].Bvin == bvin)
                        {
                            CurrentSort = Blocks[iCount].SortOrder;
                            // Trigger New Sort
                            NewSort = -2;
                        }
                    }

                    // If we can move up, find the row to switch with
                    if (CurrentSort < MaxSort)
                    {
                        if (UpdateSortOrder(bvin, NewSort) == true)
                        {
                            if (UpdateSortOrder(SwitchID, CurrentSort) == true)
                            {
                                bRet = true;
                            }
                            else
                            {
                                bRet = false;
                                // Return current
                                UpdateSortOrder(bvin, CurrentSort);
                            }
                        }
                        else
                        {
                            bRet = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            return bRet;
        }
        private bool UpdateSortOrder(string blockId, int newSort)
        {
            PolicyBlock b = this.Blocks.Where(y => y.Bvin == blockId).FirstOrDefault();
            if (b != null) b.SortOrder = newSort;
            return true;
        }
	}
}

