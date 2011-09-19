
namespace MerchantTribe.Commerce
{
    public partial class WebAppSettings
    {

        const string BVADMINEMAIL = "marcus@bvsoftware.com";
        const string APPLICATION_NAME = "BV Commerce";
const string APPLICATION_VERSION = "6.0.65.125";

        public static string BvAdminEmail
        {
            get { return BVADMINEMAIL; }
        }
        public static string BvcProductName
        {
            get { return APPLICATION_NAME; }
        }
        public static string BvcVersionNumber
        {
            get { return APPLICATION_VERSION; }
        }
        public static int MaxProducts
        {
            get { return 32000000; }
        }
        public static int MaxVariants
        {
            get { return 150; }
        }
        public static bool CustomVersion
        {
            get { return false; }
        }

        public static string ApplicationEmail
        {
            get { return "no-reply@bvcommerce.com"; }
        }
        public static string BaseApplicationUrl
        {
            get
            {
                return ApplicationBaseUrl;                
            }
        }
        public static string BaseApplicationUrlSecure()
        {
            string result = BaseApplicationUrl;
            result = result.Replace("http://", "https://");
            return result;
        }

        public static string BaseImageUrl
        {
            get
            {
                return ApplicationBaseImageUrl;
            }
        }
        public static string BaseImageUrlSecure()
        {
            string result = BaseImageUrl;
            result = result.Replace("http://", "https://");
            return result;
        }

        public static string BaseImagePhysicalPath
        {
            get
            {
                return ApplicationBaseImagePhysicalPath;
            }
        }

        public static string CookieNameCartId(long storeId)
        {
            return "ecommrc-cartid-" + storeId.ToString();
        }
        public static string CookieNameCartItemCount(long storeId)
        {
            return "ecommrc-cartitemcount-" + storeId.ToString();
        }
        public static string CookieNameCartSubTotal(long storeId)
        {
            return "ecommrc-cartsubtotal-" + storeId.ToString();
        }
        public static string CookieNameAuthenticationTokenAdmin()
        {
            return "ecommrc-authtoken";
        }
        public static string CookieNameAuthenticationTokenCustomer()
        {
            return "ecommrc-authtokencustomer";
        }
        public static string CookieNameLastCategory(long storeId)
        {
            return "ecommrc-lastcategory-" + storeId.ToString();
        }

        public static int AuthenticationTokenValidForDays()
        {
            return 90;
        }
               
        public static int UserLockoutAttempts
        {
            get { return 5; }
        }
        public static int UserLockoutMinutes
        {
            get { return 15; }
        }

    }
}
