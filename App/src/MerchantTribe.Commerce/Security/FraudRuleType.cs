
namespace MerchantTribe.Commerce.Security
{

	public enum FraudRuleType : int
	{
		None = 0,
		IPAddress = 1,
		DomainName = 2,
		EmailAddress = 3,
		PhoneNumber = 4,
		CreditCardNumber = 5
	}
}

