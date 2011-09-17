using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public static class CardValidator
    {
        public static CardType GetCardTypeFromNumber(string number)
        {
            string cleanNumber = CleanCardNumber(number);
            
            // Switch cards, starting with "4"
            if (cleanNumber.StartsWith("4903")) return CardType.Switch;
            if (cleanNumber.StartsWith("4905")) return CardType.Switch;
            if (cleanNumber.StartsWith("4911")) return CardType.Switch;
            if (cleanNumber.StartsWith("4936")) return CardType.Switch;
            if (cleanNumber.StartsWith("564182")) return CardType.Switch;            

            // Visa
            if (cleanNumber.StartsWith("4")) return CardType.Visa;
            
            // Maestro Cards starting with "5"
            if (cleanNumber.StartsWith("5018")) return CardType.Maestro;
            if (cleanNumber.StartsWith("5020")) return CardType.Maestro;
            if (cleanNumber.StartsWith("5038")) return CardType.Maestro;
            	

            // MasterCard
            if (cleanNumber.StartsWith("5")) return CardType.MasterCard;

            // Amex
            if (cleanNumber.StartsWith("34")) return CardType.Amex;
            if (cleanNumber.StartsWith("37")) return CardType.Amex;            


            // Discover Card
            if (cleanNumber.StartsWith("6011")) return CardType.Discover;            
            if (cleanNumber.StartsWith("64")) return CardType.Discover;

            // Could be Discover or China Union
            if (cleanNumber.StartsWith("6221")) return CardType.Unknown;

            // Solo
            if (cleanNumber.StartsWith("6334")) return CardType.Solo;
            if (cleanNumber.StartsWith("6767")) return CardType.Solo;

            // Switch cards starting with "6"
            if (cleanNumber.StartsWith("633110")) return CardType.Switch;
            if (cleanNumber.StartsWith("6333")) return CardType.Switch;
            
            // could be switch or maestro
            if (cleanNumber.StartsWith("6759")) return CardType.Unknown;

            // Maestro Cards Starting with "6"
            if (cleanNumber.StartsWith("6304")) return CardType.Maestro;
            if (cleanNumber.StartsWith("6761")) return CardType.Maestro;

            // JCB
            if (cleanNumber.StartsWith("35")) return CardType.JCB;

            // Diner's Club
            if (cleanNumber.StartsWith("300")) return CardType.DinersClub;
            if (cleanNumber.StartsWith("305")) return CardType.DinersClub;
            if (cleanNumber.StartsWith("36")) return CardType.DinersClub;
            if (cleanNumber.StartsWith("3852")) return CardType.DinersClub;

            return CardType.Unknown;                   
            
        }

        public static string CleanCardNumber(string number)
        {
            string result = string.Empty;

            if (number.Trim().Length < 1)
            {
                return result;
            }

            int counter = 0;
            char[] s;

            for (counter = 0; counter < number.Length; counter++)
            {
                s = number.ToCharArray(counter, 1);

                if ( ((int)s[0] >= 48) && ((int)s[0] <= 57) )
                {
                    result += s[0];
                }
            }
            return result;        
        }

        public static bool IsCardNumberValid(string number)
        {
            CardType type = GetCardTypeFromNumber(number);
            return IsCardNumberValid(number, type);
        }
        public static bool IsCardNumberValid(string number, CardType type)
        {
            string cleanCardNumber = CleanCardNumber(number);

            // If the card type doesn't match the number, it's not valid for the type
            if (!(GetCardTypeFromNumber(cleanCardNumber) == type))
            {
                return false;
            }

            if (!IsMod10(cleanCardNumber))
            {
                return false;
            }
            
            switch (type)
            {
                case CardType.Amex:
                    return LengthCheck(cleanCardNumber, 15);
                case CardType.DinersClub:
                    return LengthCheck(cleanCardNumber, 14);
                case CardType.Discover:
                    return LengthCheck(cleanCardNumber, 16);
                case CardType.JCB:
                    return LengthCheck(cleanCardNumber, 16);
                case CardType.Maestro:
                    return LengthCheck(cleanCardNumber, 12, 19);
                case CardType.MasterCard:
                    return LengthCheck(cleanCardNumber, 16);
                case CardType.Solo:
                    return LengthCheck(cleanCardNumber, 16, 19);            
                case CardType.Switch:
                    return LengthCheck(cleanCardNumber, 16, 19);            
                case CardType.Visa:
                    return LengthCheck(cleanCardNumber, 13, 16);
            }
            return false;
        }

        private static bool IsMod10(string cleanCardNumber)
        {
            // Make sure we have at least a number that looks kind of like a card
            if (cleanCardNumber.Length >= 12)
            {
                // If we have an uneven card number length, mod 2 should be 1 for double digits
                int doubleModResult = cleanCardNumber.Length % 2;

                int sum = 0;

                // Walk Number Backwards
                for (int i = cleanCardNumber.Length - 1; i > -1; i--)
                {
                    if (i % 2 == doubleModResult)
                    {
                        // Double the digit
                        int n = CharToInt(cleanCardNumber[i]) * 2;
                        if (n > 9)
                        {
                            // if the result as more than 1 digit, add the digits
                            n += 1;
                        }
                        sum += n;
                    }
                    else
                    {
                        sum += CharToInt(cleanCardNumber[i]);
                    }
                }

                if (sum % 10 == 0)
                {
                    return true;
                }
            }
            return false;
        }   

        private static int CharToInt(char input)
        {
            int output = 0;
            if (int.TryParse(input.ToString(), out output))
            {
                return output;
            }
            return 0;
        }
        private static bool LengthCheck(string cleanCardNumber, int exactLength)
        {
            return LengthCheck(cleanCardNumber, exactLength, exactLength);
        }
        private static bool LengthCheck(string cleanCardNumber, int min, int max)
        {            
            if ((cleanCardNumber.Length >= min) && (cleanCardNumber.Length <= max))
            {                
                return true;                
            }
            return false;
        }
    }
}
