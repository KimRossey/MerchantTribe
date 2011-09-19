using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce
{

    partial class BVModules_Controls_ViewOrderItems : MerchantTribe.Commerce.Content.BVUserControl
    {

        private bool _DisableReturns = false;
        public bool DisableReturns
        {
            get { return _DisableReturns; }
            set { _DisableReturns = value; }
        }
        public bool HideShippingColumn { get; set; }

        protected override void OnLoad(System.EventArgs e)
        {

            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                this.ReturnItemsImageButton.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("returnitems", Request.IsSecureConnection);
            }
        }


        public void LoadItems(List<MerchantTribe.Commerce.Orders.LineItem> items)
        {
            this.ItemsGridView.DataSource = items;
            this.ItemsGridView.DataBind();

            if (HideShippingColumn)
            {
                this.ItemsGridView.Columns[2].Visible = false;
            }

            if (_DisableReturns == true)
            {
                this.pnlReturn.Visible = false;
                this.ItemsGridView.Columns[0].Visible = false;
            }

        }

        protected void ItemsGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LineItem lineItem = (LineItem)e.Row.DataItem;
                if (lineItem != null)
                {
                    CheckBox selectedCheckBox = (CheckBox)e.Row.FindControl("SelectedCheckBox");
                    if (selectedCheckBox != null)
                    {
                        //List<RMAItem> ReturnItems = RMAItem.FindByLineItemBvin(lineItem.Id);
                        //int quantityReturned = 0;
                        //foreach (RMAItem item in ReturnItems)
                        //{
                        //    quantityReturned += item.Quantity;
                        //}

                        //if (quantityReturned >= lineItem.Quantity)
                        //{
                        //    selectedCheckBox.Visible = false;
                        //}
                        //else
                        //{
                        //    selectedCheckBox.Visible = true;
                        //}
                    }
                    

                    Label SKUField = (Label)e.Row.FindControl("SKUField");
                    Label description = (Label)e.Row.FindControl("DescriptionField");
                    
                        if (SKUField != null)
                        {
                            SKUField.Text = lineItem.ProductSku;
                        }

                        if (description != null)
                        {
                            description.Text = lineItem.ProductName;
                        }
                    
                    Label ShippingStatusField = (Label)e.Row.FindControl("ShippingStatusField");
                    if (ShippingStatusField != null)
                    {
                        ShippingStatusField.Text = EnumToString.OrderShippingStatus(lineItem.ShippingStatus);
                    }

                    Literal LineTotalField = (Literal)e.Row.FindControl("LineTotalField");
                    if (LineTotalField != null)
                    {
                        LineTotalField.Text = string.Empty;
                        if (lineItem.LineTotal != lineItem.LineTotalWithoutDiscounts)
                        {

                            LineTotalField.Text += "<strike>" + lineItem.LineTotalWithoutDiscounts.ToString("c") + "</strike><br />";
                            
                            Literal litDiscounts = (Literal)e.Row.FindControl("litDiscounts");
                            if (litDiscounts != null)
                            {
                                litDiscounts.Text = "<div class=\"discounts\">" + lineItem.DiscountDetailsAsHtml() + "</div>";
                            }
                        }
                        LineTotalField.Text += lineItem.LineTotal.ToString("c");
                    }
                }
            }
        }

        protected void ReturnItemsImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            //RMA rma = new RMA();
            //foreach (GridViewRow row in ItemsGridView.Rows)
            //{
            //    object obj = row.FindControl("SelectedCheckBox");
            //    if (obj != null)
            //    {
            //        if (((CheckBox)obj).Checked)
            //        {
            //            LineItem li = LineItem.FindByBvin((string)ItemsGridView.DataKeys[row.RowIndex].Value);
            //            RMAItem rmaItem = new RMAItem();
            //            rmaItem.RMABvin = rma.Bvin;
            //            rmaItem.LineItemBvin = li.Bvin;
            //            rma.Items.Add(rmaItem);
            //        }
            //    }
            //}

            //If rma.Items.Count = 0 Then
            //RaiseEvent ThrowError("At least one item must be selected.")
            //returnErrorLabel.Text = "At least one item must be selected."
            //Return
            //End If

            //If Orders.RMA.Insert(rma) Then
            //Response.Redirect("~/RMA.aspx?orderId=" & HttpUtility.UrlEncode(Me.OrderBvin) & "&rmaId=" & HttpUtility.UrlEncode(rma.Bvin))
            //End If
        }

        public delegate void ThrowErrorDelegate();
        public event ThrowErrorDelegate ThrowError;

    }

}