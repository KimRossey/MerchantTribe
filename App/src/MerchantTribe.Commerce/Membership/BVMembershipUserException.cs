using System;
using System.Runtime.Serialization;

namespace MerchantTribe.Commerce.Membership
{

	public class BVMembershipUserException : System.Exception
	{

		private CreateUserStatus _status = CreateUserStatus.UserRejected;

		public CreateUserStatus Status {
			get { return _status; }
			set { _status = value; }
		}

		public BVMembershipUserException(): base()
		{            
		}		
		public BVMembershipUserException(string message): base(message)
		{			
		}
		public BVMembershipUserException(string message, CreateUserStatus userstatus): base(message)
		{			
			_status = userstatus;
		}		
		public BVMembershipUserException(string message, Exception inner): base(message,inner)
		{			
		}
		public BVMembershipUserException(string message, Exception inner, CreateUserStatus userstatus): base(message,inner)
		{
			_status = userstatus;
		}
		public BVMembershipUserException(SerializationInfo info, StreamingContext context): base(info, context)
		{
		}
		public BVMembershipUserException(SerializationInfo info, StreamingContext context, CreateUserStatus userstatus): base(info,context)
		{
			_status = userstatus;
		}

	}
}
