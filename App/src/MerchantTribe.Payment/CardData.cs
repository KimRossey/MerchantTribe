using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class CardData
    {
        private string _CardNumber = string.Empty;
        private int _ExpirationMonth = 1;
        private int _ExpirationYear = DateTime.Now.Year;
        public string CardNumber {
            get { return _CardNumber; }
            set { _CardNumber = CardValidator.CleanCardNumber(value); }
        }
        public string SecurityCode { get; set; }
        public string CardHolderName { get; set; }

        public int ExpirationMonth
        {
            get { return _ExpirationMonth; }
            set { _ExpirationMonth = ValidateMonth(value); }
        }
        public string ExpirationMonthPadded
        {
            get
            {
                string result = ExpirationMonth.ToString();
                if (ExpirationMonth < 10) result = "0" + result;
                return result;
            }
        }
        public int ExpirationYear
        {
            get { return _ExpirationYear; }
            set { _ExpirationYear = ValidateYear(value); }
        }
        public string ExpirationYearTwoDigits
        {
            get
            {
                string result = ExpirationYear.ToString();
                if (result.Length > 2)
                {
                    result = result.Substring(result.Length - 2, 2);
                }
                return result;
            }
        }
        private int ValidateMonth(int requestedMonth)
        {
            int result = requestedMonth;


            if (result > 12)
            {
                result = 12;
            }

            if (result < 1)
            {
                result = 1;
            }

            return result;
        }

        private int ValidateYear(int requestedYear)
        {
            int result = requestedYear;

            // Make sure we have a four digit number            
            if (result < 1000)
            {
                result += 2000;
            }


            if (result > 9999)
            {
                result = 9999;
            }

            if (result < 2000)
            {
                result = 2000;
            }

            return result;
        }

        public CardData()
        {
            SecurityCode = string.Empty;
            CardHolderName = string.Empty;
        }

        public string CardNumberLast4Digits
        {
            get
            {
                string result = string.Empty;
                if (CardNumber.Trim().Length >= 4)
                {
                    result = CardNumber.Substring(CardNumber.Length - 4, 4);
                }
                return result;
            }
        }

        public string CardTypeName
        {
            get
            {
                CardType t = CardValidator.GetCardTypeFromNumber(CardNumber);
                switch (t)
                {
                    case CardType.Amex:
                        return "AMEX";
                    case CardType.DinersClub:
                        return "Diner's Club";
                    case CardType.Discover:
                        return "Discover";
                    case CardType.JCB:
                        return "JCB";
                    case CardType.Maestro:
                        return "Maestro";
                    case CardType.MasterCard:
                        return "MasterCard";
                    case CardType.Solo:
                        return "Solo";
                    case CardType.Switch:
                        return "Switch";
                    case CardType.Visa:
                        return "Visa";
                    default:
                        return "Unknown";
                }
            }
        }

        public bool IsCardValid(DateTime localTime)
        {

            if (!CardValidator.IsCardNumberValid(this.CardNumber))
            {
                return false;
            }

            if (CardHasExpired(localTime))
            {
                return false;
            }

            return true;
        }

        public bool CardHasExpired(DateTime localTime)
        {
            if (this.ExpirationYear < localTime.Year)
            {
                return true;
            }

            if (this.ExpirationYear == localTime.Year)
            {
                if (this.ExpirationMonth < localTime.Month)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
