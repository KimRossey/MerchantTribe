using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Transactions;

namespace MerchantTribe.Commerce.Payment
{
    class RewardPointsProviderDatabase: IRewardPointsProvider
    {
        private Data.EF.EntityFrameworkDevConnectionString ef = null;

        public RewardPointsProviderDatabase()
        {
            ef = new Data.EF.EntityFrameworkDevConnectionString(WebAppSettings.ApplicationConnectionStringForEntityFramework);
        }

        public int RewardPointsTotalIssued(long storeId)
        {
            int result = 0;

            var data = ef.bvc_RewardsPoints.Where(y => y.StoreId == storeId);
            if (data.Count() > 0)
            {
                result = data.Sum(y => y.Points);
            }

            return result;
        }
        public int RewardPointsReservedForStore(long storeId)
        {
            int result = 0;

            var data = ef.bvc_RewardsPoints.Where(y => y.StoreId == storeId);
            if (data.Count() > 0)
            {
                result = data.Sum(y => y.PointsHeld);            
            }

            return result;
        }

        public int RewardPointsAvailableForUser(string userId, long storeId)
        {
            int result = 0;
            var data = ef.bvc_RewardsPoints.Where(y => y.StoreId == storeId).Where(y => y.UserId == userId);
            if (data.Count() > 0)
            {
                result = data.Sum(y => y.Points);
            }
            return result;
        }
        public int RewardPointsHeldForUser(string userId, long storeId)
        {
            int result = 0;
            var data = ef.bvc_RewardsPoints.Where(y => y.StoreId == storeId).Where(y => y.UserId == userId);
            if (data.Count() > 0)
            {
                result = data.Sum(y => y.PointsHeld);            
            }

            return result;
        }

        public int CapturePoints(string userId, long storeId, int points)
        {                        
            int currentPoints = RewardPointsHeldForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                ef.bvc_RewardsPoints.AddObject(new Data.EF.bvc_RewardsPoints() { PointsHeld = (-1 * points), StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                ef.SaveChanges();
                return 1;
            }
            return 0;                        
        }
        public int DecreasePoints(string userId, long storeId, int points)
        {
            int currentPoints = RewardPointsAvailableForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                ef.bvc_RewardsPoints.AddObject(new Data.EF.bvc_RewardsPoints() { Points = (-1 * points), StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                ef.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int HoldPoints(string userId, long storeId, int points)
        {
            int currentPoints = RewardPointsAvailableForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                ef.bvc_RewardsPoints.AddObject(new Data.EF.bvc_RewardsPoints() { Points = (-1 * points), PointsHeld = points, StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                ef.SaveChanges();
                return 1;
            }
            return 0;
        }
        public int UnHoldPoints(string userId, long storeId, int points)
        {
            int currentPoints = RewardPointsAvailableForUser(userId, storeId);
            if (currentPoints - points >= 0)
            {
                ef.bvc_RewardsPoints.AddObject(new Data.EF.bvc_RewardsPoints() { Points = points, PointsHeld = (-1 * points), StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
                ef.SaveChanges();
                return 1;
            }
            return 0;            
        }
        public int IssuePoints(string userId, long storeId, int points)
        {
            ef.bvc_RewardsPoints.AddObject(new Data.EF.bvc_RewardsPoints() { Points = points, StoreId = storeId, UserId = userId, TransactionTime = DateTime.UtcNow });
            ef.SaveChanges();
            return 1;
        }
    }
}
