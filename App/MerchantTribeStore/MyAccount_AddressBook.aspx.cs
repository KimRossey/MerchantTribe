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

namespace MerchantTribeStore
{
    public partial class MyAccount_AddressBook : BaseStorePage
    {       
        //protected void AddressList_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    CustomerAccount u = MTApp.CurrentCustomer;
        //    string editId = (string)AddressList.DataKeys[e.Item.ItemIndex];

        //    if (e.CommandName.ToUpper() == "BILLTO") 
        //    {
        //        if (u != null)
        //        {
        //            MTApp.MembershipServices.CustomerMakeAddressBilling(u,editId);
        //            MTApp.MembershipServices.UpdateCustomer(u);
        //        }                                
                
        //    }
        //    else if (e.CommandName.ToUpper() == "SHIPTO")
        //    {
        //        if (u != null)
        //        {
        //            MTApp.MembershipServices.CustomerMakeAddressShipping(u,editId);
        //            MTApp.MembershipServices.UpdateCustomer(u);
        //        }             
        //    }
        //    LoadAddresses();
        //}

    }
}