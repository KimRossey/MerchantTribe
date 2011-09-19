
using System;

namespace MerchantTribe.Commerce.Utilities
{

	public class RandomNumbers
	{

		public RandomNumbers()
		{
		}


		/// <summary>
		/// Generates a random integer that is between maxNumber and minNumber inclusive of maxNumber and minNumber
		/// </summary>
		/// <param name="maxNumber">The maximum possible value that the method will return</param>
		/// <param name="minNumber">The minimum possible value that the method will return</param>
		/// <returns>An integer between the max and min parameters inclusive of the parameters</returns>
		public static int RandomInteger(int maxNumber, int minNumber)
		{
			return RandomInteger(maxNumber, minNumber, System.DateTime.Now.Millisecond);
		}

		public static int RandomIntegerRepeatable(int maxNumber, int minNumber)
		{
			System.Threading.Thread.Sleep(10);
			return RandomInteger(maxNumber, minNumber, System.DateTime.Now.Millisecond);
		}

		/// <summary>
		/// Generates a random integer that is between maxNumber and minNumber inclusive of maxNumber and minNumber
		/// </summary>
		/// <param name="maxNumber">The maximum possible value that the method will return</param>
		/// <param name="minNumber">The minimum possible value that the method will return</param>
		/// <param name="seed">The random integer to use as a seed</param>
		/// <returns>An integer between the max and min parameters inclusive of the parameters</returns>
		public static int RandomInteger(int maxNumber, int minNumber, int seed)
		{
			if (maxNumber < minNumber) {
				int temp = maxNumber;
				maxNumber = minNumber;
				minNumber = temp;
			}
			Random r = new Random(seed);

			// .NET random function is not inclusive of max value so add one
			maxNumber = maxNumber + 1;

			return r.Next(minNumber, maxNumber);
		}

	}

}
