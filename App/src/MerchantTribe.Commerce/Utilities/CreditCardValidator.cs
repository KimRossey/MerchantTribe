
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Utilities
{

	public class CreditCardValidator
	{

		private Collection<string> _errorMessages = new Collection<string>();
		private string _cardName = "unknown";

		/// <summary>
		/// Holds the last error message reported by the validator
		/// </summary>    
		public Collection<string> ErrorMessages {
			get { return _errorMessages; }
			set { _errorMessages = value; }
		}

		/// <summary>
		/// Holds the English name of the card type. For example: Visa
		/// </summary>    
		public string CardName {
			get { return _cardName; }
			set { _cardName = value; }
		}

		/// <summary>
		/// Removes all non-digit characters from a credit card string
		/// </summary>
		/// <param name="rawCardNumber">input string containing user input</param>
		/// <returns>A string containing only the digit characters of the credit card number</returns>
		public static string CleanCardNumber(string rawCardNumber)
		{
			string result = "";
			int counter = 0;
			char[] s;

			for (counter = 0; counter <= rawCardNumber.Length - 1; counter++) {

				s = rawCardNumber.ToCharArray(counter, 1);

				if (Convert.ToInt32(s[0]) >= 48 & Convert.ToInt32(s[0]) <= 57) {
					result += s[0];
				}
			}
			return result;
		}

		/// <summary>
		/// Validates that a given card number matches the format and mod10 formula for a given type
		/// </summary>
		/// <param name="rawCardNumber">Card number to check</param>
		/// <param name="cardType">Type of card to validate against (A = American Express, V = Visa, D = Discover, M = MasterCard, J = Japanese Credit Bureau, C = Diner's Club)</param>
		/// <returns></returns>
		public bool ValidateCard(string rawCardNumber, string cardType)
		{
			bool result = true;

			_errorMessages.Clear();
			_cardName = "";

			string workingCardNumber = CleanCardNumber(rawCardNumber);
			string length = "";
			string prefix = "";

			switch (cardType.ToUpper()) {
				case "A":
					_cardName = "American Express";
					length = "15";
					prefix = "34;37";
                    break;
				case "V":
					_cardName = "VISA";
					length = "13;16";
					prefix = "4";
                    break;
				case "M":
					_cardName = "MasterCard";
					length = "16";
					prefix = "51;52;53;54;55";
                    break;
				case "D":
					_cardName = "Discover";
					length = "16";
					prefix = "6011";
                    break;
				case "C":
					_cardName = "Diner's Club";
					length = "14";
					prefix = "300;301;302;303;304;305;36;38";
                    break;
				case "J":
					_cardName = "JCB";
					length = "15;16";
					prefix = "3;2131;1800";
                    break;
				case "SOLO":
					_cardName = "Solo";
					return true;
				case "SWITCH":
					_cardName = "Switch";
					return true;
				case "STAR":
					_cardName = "STAR Card";
					return true;
				case "MAESTRO":
					_cardName = "Maestro / Switch";
					return true;
				case "VISADELTA":
					_cardName = "Visa Delta";
					return true;
				case "VISAELECTRON":
					_cardName = "Visa Electron";
					return true;
				default:
					// Unknown Card Type so skip validation
					return true;
			}


			string[] lengths = length.Split(';');
			string[] prefixes = prefix.Split(';');

			bool prefixOK = false;
			bool lengthOK = false;
			bool modOK = false;

			// Check card length against acceptable lengths
			int i;
			for (i = 0; i <= lengths.Length - 1; i++) {
				if (workingCardNumber.Length == Convert.ToInt32(lengths[i])) {
					lengthOK = true;
				}
			}

			// Check to makes sure prefix is valid
			int j;
			for (j = 0; j <= prefixes.Length - 1; j++) {

				if (prefixes[j].Length <= workingCardNumber.Length) {
					if (workingCardNumber.Substring(0, prefixes[j].Length) == prefixes[j]) {
						prefixOK = true;
					}
				}
			}

			modOK = Mod10CheckCard(workingCardNumber);

			if (lengthOK == false) {
				_errorMessages.Add("Invalid length for " + _cardName);
				result = false;
			}
			if (prefixOK == false) {
				_errorMessages.Add("Invalid number for " + _cardName);
				result = false;
			}
			if (modOK == false) {
				_errorMessages.Add("This is not a valid credit card number.");
				result = false;
			}

			return result;
		}

		private static bool Mod10CheckCard(string cardNumber)
		{
			try {
				// Array to contain individual numbers
				int lastNumber;

				// So, get length of card
				int CardLength = cardNumber.Length;

				// Double the value of alternate digits, starting with the second digit
				// from the right, i.e. back to front.
				// Loop through starting at the end
				int i;
				bool odd = false;
				int sum = 0;

                char[] Chars = cardNumber.ToCharArray();

				for (i = CardLength - 1; i >= 0; i += -1) {
					if (odd) {
						lastNumber = (Int32.Parse(Chars[i].ToString()) * 2);
					}
					else {
						lastNumber = Int32.Parse(Chars[i].ToString());
					}

					if (lastNumber > 9) {
						lastNumber -= 9;
					}
					sum += lastNumber;

					odd = !odd;
				}

				return sum % 10 == 0;
			}
			catch {
                return false;
			}

		}

	}

}
