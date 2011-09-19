
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Membership
{

	public enum CreateUserStatus : int
	{
		None = 0,
		Success = 1,
		UserNotFound = 2,
		InvalidPassword = 3,
		DuplicateUsername = 4,
		UserRejected = 5,
		UpdateFailed = 6
	}

}
