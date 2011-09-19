using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public class RateTableLevel : IComparable<RateTableLevel>, IEquatable<RateTableLevel>
    {
        public decimal Level { get; set; }
        public decimal Rate { get; set; }

        public RateTableLevel()
        {
            Level = 0;
            Rate = 0;
        }

        public int CompareTo(RateTableLevel other)
        {
            return this.Level.CompareTo(other.Level);
        }

        public static decimal FindRateFromLevels(decimal levelValue, List<RateTableLevel> levels)
        {
            decimal result = -1;

            if (levels != null)
            {
                if (levels.Count > 0)
                {
                    // Make sure we're sorted
                    levels.Sort();

                    // Assign the last level in case we're above it
                    // but only if we qualify for it
                    if (levelValue >= levels[levels.Count - 1].Level)
                    {
                        result = levels[levels.Count - 1].Rate;
                    }

                    decimal PreviousLevel = decimal.MaxValue;

                    // Walk levels backwards
                    for (int i = levels.Count() - 1; i >= 0; i--)                    
                    {
                        if ((levelValue >= levels[i].Level) && (levelValue < PreviousLevel))
                        {
                            result = levels[i].Rate;                            
                        }
                        PreviousLevel = levels[i].Level;
                    }
                }
            }

            return result;
        }


        #region IEquatable<RateTableLevel> Members

        public bool Equals(RateTableLevel other)
        {
            if ((other.Level == this.Level) && (other.Rate == this.Rate)) return true;

            return false;
        }

        #endregion
    }
}
