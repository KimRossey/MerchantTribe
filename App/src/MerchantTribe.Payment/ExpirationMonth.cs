using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class ExpirationMonth
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public ExpirationMonth()
        {
            Name = string.Empty;
            Number = 0;
        }

        public ExpirationMonth(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public static List<ExpirationMonth> ListMonths()
        {
            List<ExpirationMonth> result = new List<ExpirationMonth>();
            
            result.Add(new ExpirationMonth("1 JAN", 1));
            result.Add(new ExpirationMonth("2 FEB", 2));
            result.Add(new ExpirationMonth("3 MAR", 3));
            result.Add(new ExpirationMonth("4 APR", 4));
            result.Add(new ExpirationMonth("5 MAY", 5));
            result.Add(new ExpirationMonth("6 JUN", 6));
            result.Add(new ExpirationMonth("7 JUL", 7));
            result.Add(new ExpirationMonth("8 AUG", 8));
            result.Add(new ExpirationMonth("9 SEP", 9));
            result.Add(new ExpirationMonth("10 OCT", 10));
            result.Add(new ExpirationMonth("11 NOV", 11));
            result.Add(new ExpirationMonth("12 DEC", 12));

            return result;
        }
    }
}
