using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Orders;
using System.Configuration;

namespace BVCommerce
{

    partial class BVModules_Reports_Customer_List_View : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Custom Report for Greenspring";
            this.CurrentTab = AdminTabType.Reports;
            ValidateCurrentUserHasPermission(SystemPermissions.ReportsView);
        }

        private List<Product> _SelectedProducts = new List<Product>();

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LoadUsers()
        {
            _SelectedProducts = new List<Product>();
            PopulateProducts();

            lblResult.Text = "";
            this.txtResults.Text = string.Empty;

            SqlConnection conn = new SqlConnection(WebAppSettings.ApplicationConnectionString);

            RenderHeader();

            List<CustomerAccount> users = BVApp.MembershipServices.Customers.FindAll();

            if (users != null)
            {
                foreach (CustomerAccount u in users)
                {
                    try
                    {
                        RenderUser(u);
                    }
                    catch (Exception ex)
                    {
                        EventLog.LogEvent(ex);
                    }
                }
            }

        }

        private void PopulateProducts()
        {
            string[] s = this.PurchasedSkuField.Text.Split(',');
            if (s != null)
            {
                for (int i = 0; i <= s.Length - 1; i++)
                {
                    Product p = BVApp.CatalogServices.Products.FindBySku(CleanInputForSQL(s[i]));
                    if (p != null)
                    {
                        _SelectedProducts.Add(p);
                    }
                }
            }
        }

        private void RenderHeader()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(AddString("Email"));
            //sb.Append(AddString("OnMailingList"))
            sb.Append(AddString("LastName"));
            sb.Append(AddString("FirstName"));
            sb.Append(AddString("Address"));
            sb.Append(AddString("City"));
            sb.Append(AddString("State"));
            sb.Append(AddString("Zip"));

            foreach (Product p in _SelectedProducts)
            {
                sb.Append(AddString(p.Sku));
            }

            sb.Append(System.Environment.NewLine);

            this.txtResults.Text += sb.ToString();
        }

        private void RenderUser(CustomerAccount u)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int totalCount = 0;
            List<OrderSnapshot> orders = BVApp.OrderServices.Orders.FindByUserId(u.Bvin, 1, int.MaxValue, ref totalCount);
            if (orders == null)
            {
                orders = new List<OrderSnapshot>();
            }

            sb.Append(AddString(u.Email));
            //If Contacts.MailingList.CheckMembership("8858e25b-d9a0-4ae7-b74b-bdecd0c77a8d", u.Email) Then
            //    sb.Append(AddString("YES"))
            //Else
            //    sb.Append(AddString("NO"))
            //End If

            sb.Append(AddString(u.LastName));
            sb.Append(AddString(u.FirstName));

            bool renderEmpty = false;

            if ((u.Addresses != null))
            {
                if (u.Addresses.Count > 0)
                {
                    sb.Append(AddString(u.Addresses[u.Addresses.Count - 1].Line1 + u.Addresses[u.Addresses.Count - 1].Line2));
                    sb.Append(AddString(u.Addresses[u.Addresses.Count - 1].RegionName));
                    sb.Append(AddString(u.Addresses[u.Addresses.Count - 1].City));
                    sb.Append(AddString(u.Addresses[u.Addresses.Count - 1].PostalCode));
                }
                else
                {
                    renderEmpty = true;
                }
            }
            else
            {
                if (orders.Count > 0)
                {
                    if (orders[0].ShippingAddress != null)
                    {
                        sb.Append(AddString(orders[0].ShippingAddress.Line1 + orders[0].ShippingAddress.Line2));
                        sb.Append(AddString(orders[0].ShippingAddress.RegionName));
                        sb.Append(AddString(orders[0].ShippingAddress.City));
                        sb.Append(AddString(orders[0].ShippingAddress.PostalCode));
                    }
                    else
                    {
                        renderEmpty = true;
                    }
                }
                else
                {
                    BVSoftware.Commerce.Contacts.Address ship = u.GetShippingAddress();
                    if (ship != null)
                    {
                        if (ship.Line1.Trim().Length > 0)
                        {
                            sb.Append(AddString(ship.Line1 + ship.Line2));
                            sb.Append(AddString(ship.RegionName));
                            sb.Append(AddString(ship.City));
                            sb.Append(AddString(ship.PostalCode));
                        }
                        else
                        {
                            renderEmpty = true;
                        }
                    }
                    else
                    {
                        renderEmpty = true;
                    }
                }

                if (renderEmpty)
                {
                    sb.Append(AddString(" "));
                    sb.Append(AddString(" "));
                    sb.Append(AddString(" "));
                    sb.Append(AddString(" "));
                }
            }


            foreach (Product p in _SelectedProducts)
            {
                sb.Append(AddString(CountPurchases(u.Bvin, p.Bvin, orders)));
            }

            sb.Append(System.Environment.NewLine);
            this.txtResults.Text += sb.ToString();
        }

        private string AddString(string data)
        {
            return AddString(data, true);
        }

        private string AddString(string data, bool withDelimeter)
        {
            string result = data.Replace("\t", " ");
            if (withDelimeter)
            {
                result += "\t";
            }
            return result;
        }

        private string CleanInputForSQL(string input)
        {
            string result = input;

            System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex("[^0-9a-zA-Z]");
            result = rx.Replace(result, "");
            result = result.Replace("drop ", "drop");
            result = result.Replace("update ", "update");
            result = result.Replace("select ", "select");
            result = result.Replace("delete ", "delete");
            result = result.Replace("xp_", "");

            return result;
        }

        protected void btnGo_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            LoadUsers();
        }

        private bool AlreadyPurchased(string uid, System.Collections.Specialized.StringCollection skus)
        {
            bool result = false;

            SqlConnection conn = new SqlConnection(WebAppSettings.ApplicationConnectionString);
            string outFilter = " ProductSku=''";

            if (skus.Count > 0)
            {
                outFilter = "";
                for (int i = 0; i <= skus.Count - 1; i++)
                {
                    outFilter += " ProductSku='" + skus[i] + "'";
                    if (i < skus.Count - 1)
                    {
                        outFilter += " OR ";
                    }
                }
            }
            string cmdText = "Select bvin,UserId from bvc_Order WHERE bvin IN (SELECT OrderBvin FROM bvc_LineItem WHERE " + outFilter + ") AND (IsPlaced=1) AND userID=@userID";
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@UserID", uid);
            DataTable dt = new DataTable();
            SqlDataAdapter adapt = new SqlDataAdapter(cmd);

            try
            {
                adapt.Fill(dt);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
            finally
            {
                conn.Dispose();
            }

            return result;
        }

        private string CountPurchases(string userId, string productId, List<OrderSnapshot> orders)
        {
            string result = "0";
            int counter = 0;

            foreach (OrderSnapshot o in orders)
            {
                if (o.IsPlaced)
                {
                    Order fullOrder = BVApp.OrderServices.Orders.FindForCurrentStore(o.bvin);

                    foreach (LineItem li in fullOrder.Items)
                    {
                        if (li.ProductId == productId)
                        {
                            counter += (int)li.Quantity;
                        }
                    }
                }

            }

            result = counter.ToString();
            return result;
        }

    }

}