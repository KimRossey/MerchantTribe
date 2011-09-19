
using System;
using System.Collections;

namespace MerchantTribe.Commerce.Utilities
{
	public class PasswordGenerator
	{

		public PasswordGenerator()
		{
		}

		/// <summary>
		/// Generates a random password string
		/// </summary>
		/// <returns>A string containing the new random password</returns>
		public static string GeneratePassword()
		{
			string result = "";

			ArrayList passwordColor = new ArrayList();
			ArrayList passwordFruit = new ArrayList();

			passwordColor.Add("yellow");
			passwordColor.Add("orange");
			passwordColor.Add("green");
			passwordColor.Add("red");
			passwordColor.Add("blue");
			passwordColor.Add("purple");

			passwordFruit.Add("apple");
			passwordFruit.Add("grape");
			passwordFruit.Add("pear");
			passwordFruit.Add("banana");
			passwordFruit.Add("pineapple");
			passwordFruit.Add("peach");
			passwordFruit.Add("plum");
			passwordFruit.Add("apricot");
			passwordFruit.Add("cherry");
			passwordFruit.Add("strawberry");
			passwordFruit.Add("grapefruit");
			passwordFruit.Add("orange");
			passwordFruit.Add("blackberry");
			passwordFruit.Add("blueberry");
			passwordFruit.Add("kiwi");
			passwordFruit.Add("starfruit");
			passwordFruit.Add("mango");
			passwordFruit.Add("watermellon");
			passwordFruit.Add("rasberry");
			passwordFruit.Add("papaya");
			passwordFruit.Add("lemon");
			passwordFruit.Add("lime");

			int MaxColor = passwordColor.Count - 1;
			int MaxFruit = passwordFruit.Count - 1;

			string myColor = passwordColor[Utilities.RandomNumbers.RandomInteger(0, MaxColor)].ToString();
			string myFruit = passwordFruit[Utilities.RandomNumbers.RandomInteger(0, MaxFruit)].ToString();
			int myNumber = Utilities.RandomNumbers.RandomInteger(0, 9999);

			result = myColor + myNumber.ToString() + myFruit;

			return result;
		}

	}

}
