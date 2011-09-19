using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Payment
{
    public interface IRewardPointsProvider
    {
        int RewardPointsTotalIssued(long storeId);
        int RewardPointsReservedForStore(long storeId);
        int RewardPointsAvailableForUser(string userId, long storeId);
        int RewardPointsHeldForUser(string userId, long storeId);
        int CapturePoints(string userId, long storeId, int points);
        int DecreasePoints(string userId, long storeId, int points);
        int HoldPoints(string userId, long storeId, int points);
        int UnHoldPoints(string userId, long storeId, int points);
        int IssuePoints(string userId, long storeId, int points);
    }
}
