using System;
using MerchantTribe.PaypalWebServices;
using com.paypal.sdk.services;
using com.paypal.soap.api;
using com.paypal;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Utilities
{

	public class PaypalExpressUtilities
	{

        public static PayPalAPI GetPaypalAPI(Accounts.Store currentStore)
		{            
			com.paypal.sdk.profiles.IAPIProfile APIProfile 
                = Utilities.PaypalExpressUtilities.CreateAPIProfile(currentStore.Settings.PayPal.UserName,
                                                                    currentStore.Settings.PayPal.Password,
                                                                    currentStore.Settings.PayPal.Signature,
                                                                    currentStore.Settings.PayPal.FastSignupEmail,
                                                                    currentStore.Settings.PayPal.Mode);
            
			return new PayPalAPI(APIProfile);
		}

		private static com.paypal.sdk.profiles.IAPIProfile CreateAPIProfile(string PayPalUserName, string PayPalPassword, string PayPalSignature, string subject, string mode)
		{
            
            com.paypal.sdk.profiles.IAPIProfile profile = null;

            try
            {
                 profile = com.paypal.sdk.profiles.ProfileFactory.createSignatureAPIProfile();

                if (profile != null)
                {

                    profile.Environment = mode;

                    EventLog.LogEvent("PayPal Express Get Api", "Getting Environment " + mode, EventLogSeverity.Information);

                    if (PayPalUserName.Trim().Length > 0)
                    {
                        profile.Subject = string.Empty;
                        profile.APISignature = PayPalSignature;
                        profile.APIUsername = PayPalUserName;
                        profile.APIPassword = PayPalPassword;
                    }
                    else
                    {
                        profile.Subject = subject;
                        profile.APISignature = WebAppSettings.ApplicationPayPalSignature;
                        profile.APIUsername = WebAppSettings.ApplicationPayPalUsername;
                        profile.APIPassword = WebAppSettings.ApplicationPayPalPassword;
                    }
                }
                else
                {
                    EventLog.LogEvent("Paypal API", "Paypal com.paypal.sdk.profiles.ProfileFactory.CreateAPIProfile has failed.",
                        EventLogSeverity.Error);
                }
            }
            catch (Exception ex)
            {                
                EventLog.LogEvent("PayPal Utilities", ex.Message + " | " + ex.StackTrace, EventLogSeverity.Warning);
            }

			return profile;
		}

	}
}
