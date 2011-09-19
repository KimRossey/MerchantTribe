using System;
using System.Text;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using System.Linq;
using MerchantTribe.Commerce.Membership;
using System.Web.UI.WebControls;

namespace BVCommerce
{
    public partial class MyAccount_AddressBook : BaseStorePage
    {       
        //protected void AddressList_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    CustomerAccount u = BVApp.CurrentCustomer;
        //    string editId = (string)AddressList.DataKeys[e.Item.ItemIndex];

        //    if (e.CommandName.ToUpper() == "BILLTO") 
        //    {
        //        if (u != null)
        //        {
        //            BVApp.MembershipServices.CustomerMakeAddressBilling(u,editId);
        //            BVApp.MembershipServices.UpdateCustomer(u);
        //        }                                
                
        //    }
        //    else if (e.CommandName.ToUpper() == "SHIPTO")
        //    {
        //        if (u != null)
        //        {
        //            BVApp.MembershipServices.CustomerMakeAddressShipping(u,editId);
        //            BVApp.MembershipServices.UpdateCustomer(u);
        //        }             
        //    }
        //    LoadAddresses();
        //}

    }
}