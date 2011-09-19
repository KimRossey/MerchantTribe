using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Reporting
{
    public class WeeklySummary
    {
        public decimal Monday { get; set; }
        public decimal Tuesday { get; set; }
        public decimal Wednesday { get; set; }
        public decimal Thursday { get; set; }
        public decimal Friday { get; set; }
        public decimal Saturday { get; set; }
        public decimal Sunday { get; set; }
        public decimal Week
        {
            get
            {
                return Monday
                      + Tuesday
                      + Wednesday
                      + Thursday
                      + Friday
                      + Saturday
                      + Sunday;
            }
        }

        public decimal MondayLast { get; set; }
        public decimal TuesdayLast { get; set; }
        public decimal WednesdayLast { get; set; }
        public decimal ThursdayLast { get; set; }
        public decimal FridayLast { get; set; }
        public decimal SaturdayLast { get; set; }
        public decimal SundayLast { get; set; }
        public decimal WeekLast
        {
            get
            {
                return MondayLast
                      + TuesdayLast
                      + WednesdayLast
                      + ThursdayLast
                      + FridayLast
                      + SaturdayLast
                      + SundayLast;
            }
        }

        public decimal MondayChange { get { return Change(Monday, MondayLast); } }
        public decimal TuesdayChange { get { return Change(Tuesday, TuesdayLast); } }
        public decimal WednesdayChange { get { return Change(Wednesday, WednesdayLast); } }
        public decimal ThursdayChange { get { return Change(Thursday, ThursdayLast); } }
        public decimal FridayChange { get { return Change(Friday, FridayLast); } }
        public decimal SaturdayChange { get { return Change(Saturday, SaturdayLast); } }
        public decimal SundayChange { get { return Change(Sunday, SundayLast); } }
        public decimal WeekChange { get { return Change(Week, WeekLast); } }

        private decimal Change(decimal current, decimal previous)
        {
            decimal result = 0;

            decimal diff = current - previous;

            if (previous == 0) return 1m;

            result = (diff / previous);

            return result;
        }
    }

    

    public class SalesSummary
    {
        private long _StoreId = 0;

        public SalesSummary(long storeId)
        {
            _StoreId = storeId;           
        }

        public void AddSampleData(WeeklySummary result)
        {
            result.Monday = 500.23m;
            result.Tuesday = 750.01m;
            result.Wednesday = 421.75m;
            result.Thursday = 647.00m;
            result.Friday = 541.94m;
            result.Saturday = 354.11m;
            result.Sunday = 402.04m;

            result.MondayLast = 0m; // zero
            result.TuesdayLast = 610.44m; // less
            result.WednesdayLast = 422.01m; // more by a litte
            result.ThursdayLast = 567.17m; // less
            result.FridayLast = 541.94m; // same
            result.SaturdayLast = 414.55m; // more
            result.Sunday = 288.95m; // less
        }

        public WeeklySummary GetWeeklySummary(DateTime currentLocalTime, Orders.OrderService orderService)
        {
            WeeklySummary result = new WeeklySummary();

            AddThisWeekData(result, currentLocalTime, orderService);
            AddLastWeekData(result, currentLocalTime, orderService);
                                                                                            
            return result;
        }

        private void AddThisWeekData(WeeklySummary result, DateTime currentLocalTime, Orders.OrderService orderService)
        {
            
            Utilities.DateRange rangeData = new Utilities.DateRange();
            rangeData.RangeType = Utilities.DateRangeType.ThisWeek;
            rangeData.CalculateDatesFromType(currentLocalTime);

            int totalCount = 0;
            List<Orders.OrderTransaction> data = orderService.Transactions.FindForReportByDateRange(rangeData.StartDate.ToUniversalTime(),
                                                                    rangeData.EndDate.ToUniversalTime(),
                                                                    _StoreId, int.MaxValue, 1, ref totalCount);

            decimal m = 0;
            decimal t = 0;
            decimal w = 0;
            decimal r = 0;
            decimal f = 0;
            decimal s = 0;
            decimal y = 0;

            foreach (Orders.OrderTransaction ot in data)
            {
                switch (ot.TimeStampUtc.ToLocalTime().DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        m += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Tuesday:
                        t += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Wednesday:
                        w += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Thursday:
                        r += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Friday:
                        f += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Saturday:
                        s += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Sunday:
                        y += ot.AmountAppliedToOrder;
                        break;
                }
            }

            result.Monday = m;
            result.Tuesday = t;
            result.Wednesday = w;
            result.Thursday = r;
            result.Friday = f;
            result.Saturday = s;
            result.Sunday = y;

        }

        private void AddLastWeekData(WeeklySummary result, DateTime currentLocalTime, Orders.OrderService orderService)
        {
            Utilities.DateRange rangeData = new Utilities.DateRange();
            rangeData.RangeType = Utilities.DateRangeType.LastWeek;
            rangeData.CalculateDatesFromType(currentLocalTime);

            int totalCount = 0;
            List<Orders.OrderTransaction> data = orderService.Transactions.FindForReportByDateRange(rangeData.StartDate.ToUniversalTime(),
                                                                    rangeData.EndDate.ToUniversalTime(),
                                                                    _StoreId, int.MaxValue, 1, ref totalCount);

            decimal m = 0;
            decimal t = 0;
            decimal w = 0;
            decimal r = 0;
            decimal f = 0;
            decimal s = 0;
            decimal y = 0;

            foreach (Orders.OrderTransaction ot in data)
            {
                switch (ot.TimeStampUtc.ToLocalTime().DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        m += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Tuesday:
                        t += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Wednesday:
                        w += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Thursday:
                        r += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Friday:
                        f += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Saturday:
                        s += ot.AmountAppliedToOrder;
                        break;
                    case DayOfWeek.Sunday:
                        y += ot.AmountAppliedToOrder;
                        break;
                }
            }

            result.MondayLast = m;
            result.TuesdayLast = t;
            result.WednesdayLast = w;
            result.ThursdayLast = r;
            result.FridayLast = f;
            result.SaturdayLast = s;
            result.SundayLast = y;
        }
    }
}
