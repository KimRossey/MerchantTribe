using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class AuthorizeNet: Method
    {

        private const string LiveUrl = "https://secure.authorize.net/gateway/transact.dll";
        private const string DeveloperUrl = "https://test.authorize.net/gateway/transact.dll";

        public override string Name
        {
            get { return "Authorize.Net"; }
        }

        public override string Id
        {
            get { return "828F3F70-EF01-4db6-A385-C5467CF91587"; }
        }

        public AuthorizeNetSettings Settings { get; set; }

        public AuthorizeNet()
        {
            Settings = new AuthorizeNetSettings();
        }

        public override void ProcessTransaction(Transaction t)
        {
            bool result = false;

            try
            {
                string url = LiveUrl;                
                if (Settings.DeveloperMode)
                {
                    url = DeveloperUrl;
                }
                
                // Build Data String
                // Card Number and Expiration
                string expDate = t.Card.ExpirationMonthPadded + t.Card.ExpirationYearTwoDigits;

                // Set Parameters
                StringBuilder sb = new StringBuilder();
                string postData = string.Empty;

                sb.Append("x_version=3.1");
                sb.Append("&x_login=");
                sb.Append(SafeWriteString(Settings.MerchantLoginId.Trim()));
                sb.Append("&x_tran_key=");
                sb.Append(SafeWriteString(Settings.TransactionKey.Trim()));
                sb.Append("&x_Amount=");
                sb.Append(SafeWriteString(t.Amount.ToString()));
                sb.Append("&x_Cust_ID=");
                sb.Append(SafeWriteString(t.Customer.Email));
                sb.Append("&x_Description=");
                sb.Append(SafeWriteString(t.MerchantDescription));
                sb.Append("&x_invoice_num=");
                sb.Append(SafeWriteString(t.MerchantInvoiceNumber));
                sb.Append("&x_Email_Customer=");
                sb.Append(SafeWriteBool(Settings.SendEmailToCustomer));
                sb.Append("&x_delim_data=");
                sb.Append(SafeWriteBool(true));
                sb.Append("&x_ADC_URL=");
                sb.Append(SafeWriteBool(false));
                sb.Append("&x_delim_char=");
                sb.Append(SafeWriteString(","));
                sb.Append("&x_relay_response=");
                sb.Append(SafeWriteBool(false));
                sb.Append("&x_Email=");
                sb.Append(SafeWriteString(t.Customer.Email));

                // Main Address
                sb.Append("&x_First_Name=");
                sb.Append(SafeWriteString(t.Customer.FirstName));
                sb.Append("&x_Last_Name=");
                sb.Append(SafeWriteString(t.Customer.LastName));
                sb.Append("&x_Company=");
                sb.Append(SafeWriteString(t.Customer.Company));
                sb.Append("&x_Address=");
                sb.Append(SafeWriteString(t.Customer.Street));
                sb.Append("&x_City=");
                sb.Append(SafeWriteString(t.Customer.City));
                
                sb.Append("&x_Country=");
                MerchantTribe.Web.Geography.Country country = MerchantTribe.Web.Geography.Country.FindByName(t.Customer.Country);
                if (country != null)
                {
                    sb.Append(SafeWriteString(country.IsoNumeric));
                }
                else
                {
                    sb.Append(SafeWriteString(t.Customer.Country));
                }

                // TODO: Add code to make sure we've got the correct state format
                if (t.Customer.Region != string.Empty)
                {
                    sb.Append("&x_State=");
                    sb.Append(SafeWriteString(t.Customer.Region));
                }
                sb.Append("&x_Zip=");
                sb.Append(SafeWriteString(t.Customer.PostalCode));
                sb.Append("&x_Phone=");
                sb.Append(SafeWriteString(t.Customer.Phone));

                // Ship To Address
                sb.Append("&x_Ship_To_First_Name=");
                sb.Append(SafeWriteString(t.Customer.ShipFirstName));
                sb.Append("&x_Ship_To_Last_Name=");
                sb.Append(SafeWriteString(t.Customer.ShipLastName));
                sb.Append("&x_Ship_To_Company=");
                sb.Append(SafeWriteString(t.Customer.ShipCompany));
                sb.Append("&x_Ship_To_Address=");
                sb.Append(SafeWriteString(t.Customer.ShipStreet));
                sb.Append("&x_Ship_To_City=");
                sb.Append(SafeWriteString(t.Customer.ShipCity));
                // TODO: Convert country codes to ISO Codes or
                // find a way to guarantee that we're getting an iso code
                sb.Append("&x_Ship_To_Country=");

                MerchantTribe.Web.Geography.Country shipcountry = MerchantTribe.Web.Geography.Country.FindByName(t.Customer.ShipCountry);
                if (country != null)
                {
                    sb.Append(SafeWriteString(shipcountry.IsoNumeric));
                }
                else
                {
                    sb.Append(SafeWriteString(t.Customer.ShipCountry));
                }

                // TODO: Add code to make sure we've got the correct state format
                if (t.Customer.ShipRegion != string.Empty)
                {
                    sb.Append("&x_Ship_To_State=");
                    sb.Append(SafeWriteString(t.Customer.ShipRegion));
                }
                sb.Append("&x_Ship_To_Zip=");
                sb.Append(SafeWriteString(t.Customer.ShipPostalCode));
                sb.Append("&x_Ship_To_Phone=");
                sb.Append(SafeWriteString(t.Customer.ShipPhone));

                sb.Append("&x_Method=");
                sb.Append(SafeWriteString("CC"));

                // Add Test Mode Flag if needed
                if (Settings.TestMode)
                {
                    sb.Append(SafeWriteString("&x_test_request=TRUE"));
                }

                switch (t.Action)
                {
                    case ActionType.CreditCardCharge:
                        // Charge
                        sb.Append("&x_Type=");
                        sb.Append(SafeWriteString("AUTH_CAPTURE"));
                        sb.Append("&x_customer_ip=");
                        sb.Append(SafeWriteString(t.Customer.IpAddress));

                        break;
                    case ActionType.CreditCardHold:
                        // Authorize
                        sb.Append("&x_Type=");
                        sb.Append(SafeWriteString("AUTH_ONLY"));
                        sb.Append("&x_customer_ip=");
                        sb.Append(SafeWriteString(t.Customer.IpAddress));
                        break;
                    case ActionType.CreditCardCapture:
                        // Capture, Post Authorize
                        sb.Append("&x_Type=");
                        sb.Append(SafeWriteString("PRIOR_AUTH_CAPTURE"));
                        sb.Append("&x_trans_id=");
                        sb.Append(SafeWriteString(t.PreviousTransactionNumber));                        
                        break;
                    case ActionType.CreditCardVoid:
                        // Void
                        sb.Append("&x_Type=");
                        sb.Append(SafeWriteString("VOID"));
                        sb.Append("&x_trans_id=");
                        sb.Append(SafeWriteString(t.PreviousTransactionNumber));                        
                        break;
                    case ActionType.CreditCardRefund:
                        // Refund, Credit
                        sb.Append("&x_Type=");
                        sb.Append(SafeWriteString("CREDIT"));
                        sb.Append("&x_trans_id=");
                        sb.Append(SafeWriteString(t.PreviousTransactionNumber));                        
                        break;
                }

                // Add Card Number, CVV Code and Expiration Date
                sb.Append("&x_Card_Num=");
                sb.Append(SafeWriteString(t.Card.CardNumber));
                if (t.Card.SecurityCode.Length > 0)
                {
                    sb.Append("&x_Card_Code=");
                    sb.Append(SafeWriteString(t.Card.SecurityCode));
                }
                sb.Append("&x_Exp_Date=");
                sb.Append(SafeWriteString(expDate));


                // Dump string builder to string to send to Authorize.Net
                postData = sb.ToString();

                string responseString = NetworkUtilities.SendRequestByPost(url, postData);

                // Split response string
                string[] output = responseString.Split(',');

                int counter = 0;
                System.Collections.Hashtable vars = new System.Collections.Hashtable();

                // Move strings into hash table for easy reference
                foreach (string var in output)
                {
                    vars.Add(counter, var);
                    counter += 1;
                }

                if (vars.Count < 7)
                {
                    result = false;
                }
                else
                {
                    string responseCode = (string)vars[0];
                    string responseDescription = (string)vars[3];
                    string responseAuthCode = (string)vars[4];
                    string responseAVSCode = (string)vars[5];
                    t.Result.AvsCode = ParseAvsCode(responseAVSCode);
                    string responseReferenceCode = (string)vars[6];
                    string responseSecurityCode = string.Empty;
                    if (vars.Count > 38)
                    {
                        responseSecurityCode = (string)vars[38];
                    }
                    t.Result.CvvCode = ParseSecurityCode(responseSecurityCode);


                    // Trim off Extra Quotes on response codes
                    responseCode = responseCode.Trim('"');

                    // Save result information to payment data object 
                    t.Result.ResponseCode = responseCode;
                    t.Result.ResponseCodeDescription = responseDescription;
                    t.Result.ReferenceNumber = responseReferenceCode;

                    switch (responseCode)
                    {
                        case "1":
                            // Approved
                            result = true;
                            break;
                        case "2":
                            // Declined
                            result = false;
                            t.Result.Messages.Add(new Message("Declined: " + responseDescription, responseCode, MessageType.Warning));
                            break;
                        case "3":
                            // UNKNOWN
                            result = false;
                            t.Result.Messages.Add(new Message("Authorize.Net Error: " + responseDescription, responseCode, MessageType.Error));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                t.Result.Messages.Add(new Message("Unknown Payment Error: " + ex.Message, "BVP_AN_1001", MessageType.Error));
                t.Result.Messages.Add(new Message("Stack Trace " + ex.StackTrace, "STACKTRACE", MessageType.Error));
            }

            t.Result.Succeeded = result;
         
        }
                
        public override MethodSettings BaseSettings
        {
            get { return Settings; }
        }

        #region Helper Methods
        private string SafeWriteBool(bool input)
        {
            if (input)
            {
                return System.Web.HttpUtility.UrlEncode("TRUE");
            }
            else
            {
                return System.Web.HttpUtility.UrlEncode("FALSE");
            }
        }
        private string SafeWriteString(string input)
        {
            return System.Web.HttpUtility.UrlEncode(input);
        }
        private AvsResponseType ParseAvsCode(string code)
        {
            AvsResponseType result = AvsResponseType.Unavailable;

            switch (code.ToUpper())
            {
                case "A":
                    result = AvsResponseType.PartialMatchAddress;
                    break;
                case "B":
                    result = AvsResponseType.Unavailable;
                    break;
                case "E":
                    result = AvsResponseType.Error;
                    break;
                case "G":
                    result = AvsResponseType.Unavailable;
                    break;
                case "N":
                    result = AvsResponseType.NoMatch;
                    break;
                case "P":
                    result = AvsResponseType.Unavailable;
                    break;
                case "R":
                    result = AvsResponseType.Unavailable;
                    break;
                case "S":
                    result = AvsResponseType.Unavailable;
                    break;
                case "U":
                    result = AvsResponseType.Unavailable;
                    break;
                case "W":
                    result = AvsResponseType.PartialMatchPostalCode;
                    break;
                case "X":
                    result = AvsResponseType.FullMatch;
                    break;
                case "Y":
                    result = AvsResponseType.FullMatch;
                    break;
                case "Z":
                    result = AvsResponseType.PartialMatchPostalCode;
                    break;
            }

            return result;
        }
        private CvnResponseType ParseSecurityCode(string code)
        {
            CvnResponseType result = CvnResponseType.Unavailable;

            switch (code.ToUpper())
            {
                case "M":
                    result = CvnResponseType.Match;
                    break;
                case "N":
                    result = CvnResponseType.NoMatch;
                    break;
                case "P":
                    result = CvnResponseType.Unavailable;
                    break;
                case "S":
                    result = CvnResponseType.Error;
                    break;
                case "U":
                    result = CvnResponseType.Unavailable;
                    break;
            }
            return result;
        }
        #endregion

    }
}
