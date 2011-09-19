using MerchantTribe.Commerce.Contacts;

namespace BVCommerce
{

    partial class BVModules_Controls_StaticAddressDisplay : System.Web.UI.UserControl
    {

        public void LoadFromAddress(Address address)
        {
            NameLabel.Text = address.FirstName + " " + address.LastName;
            if (address.Company.Trim() != string.Empty)
            {
                CompanyLabel.Text = address.Company;
                CompanyRow.Visible = true;
            }
            else
            {
                CompanyRow.Visible = false;
            }

            AddressLineOneLabel.Text = address.Line1;
            if (address.Line2.Trim() != string.Empty)
            {
                AddressLineTwoLabel.Text = address.Line2;
                LineTwoRow.Visible = true;
            }
            else
            {
                LineTwoRow.Visible = false;
            }
            if (address.Line3.Trim() != string.Empty)
            {
                AddressLineThreeLabel.Text = address.Line3;
                LineThreeRow.Visible = true;
            }
            else
            {
                LineThreeRow.Visible = false;
            }
            AddressLineFourLabel.Text = address.City + ", " + address.RegionName + " " + address.PostalCode;
        }
    }
}