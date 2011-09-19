using System;
using System.Runtime.Serialization;

namespace MerchantTribe.Commerce.Membership
{

	public class SystemMembershipUserException : System.Exception
	{

		private CreateUserStatus _status = CreateUserStatus.UserRejected;

		public CreateUserStatus Status {
			get { return _status; }
			set { _status = value; }
		}

		public SystemMembershipUserException(): base()
		{            
		}		
		public SystemMembershipUserException(string message): base(message)
		{			
		}
		public SystemMembershipUserException(string message, CreateUserStatus userstatus): base(message)
		{			
			_status = userstatus;
		}		
		public SystemMembershipUserException(string message, Exception inner): base(message,inner)
		{			
		}
		public SystemMembershipUserException(string message, Exception inner, CreateUserStatus userstatus): base(message,inner)
		{
			_status = userstatus;
		}
		public SystemMembershipUserException(SerializationInfo info, StreamingContext context): base(info, context)
		{
		}
		public SystemMembershipUserException(SerializationInfo info, StreamingContext context, CreateUserStatus userstatus): base(info,context)
		{
			_status = userstatus;
		}

	}
}
