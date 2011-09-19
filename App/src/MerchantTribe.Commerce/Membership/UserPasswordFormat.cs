using System;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Membership
{

	public enum UserPasswordFormat : int
	{
		ClearText = 0,
		Hashed = 1,
		Encrypted = 2
	}
}
