using System;
using System.Web.UI;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Controls_CategorySortOrder : System.Web.UI.UserControl
    {

        public CategorySortOrder SelectedSortOrder
        {
            get { return (CategorySortOrder)int.Parse(SortOrderDropDownList.SelectedValue); }
            set { SortOrderDropDownList.SelectedValue = ((int)value).ToString(); }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (Page.IsPostBack)
            {
                string val = this.Request.Params[this.SortOrderDropDownList.UniqueID];
                if (!string.IsNullOrEmpty(val))
                {
                    SetCategorySortOrder(val);
                }
            }
            else
            {
                string val = this.Request.QueryString["sortorder"];
                if (!string.IsNullOrEmpty(val))
                {
                    SetCategorySortOrder(val);
                }
            }

        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            string sortOrder = SiteTerms.GetTerm(SiteTermIds.SortOrder);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                SortOrderLabel.Text = sortOrder;
            }
            else
            {
                SortOrderLabel.Text = "Sort Order";
            }

            if (this.Page is BaseStoreCategoryPage)
            {
                BaseStoreCategoryPage basePage = (BaseStoreCategoryPage)this.Page;
                if (basePage.LocalCategory != null)
                {
                    if (!Page.IsPostBack)
                    {
                        if (basePage.LocalCategory.DisplaySortOrder == CategorySortOrder.ManualOrder)
                        {
                            SortOrderDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("Default", "1"));
                        }
                        SortOrderDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("Name", "2"));
                        SortOrderDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("Price (Low to High)", "3"));
                        SortOrderDropDownList.Items.Add(new System.Web.UI.WebControls.ListItem("Price (High to Low)", "4"));

                        if (basePage.SortOrder != CategorySortOrder.None)
                        {
                            SortOrderDropDownList.SelectedValue = ((int)basePage.SortOrder).ToString();
                        }
                        else
                        {
                            SortOrderDropDownList.SelectedValue = ((int)basePage.LocalCategory.DisplaySortOrder).ToString();
                        }

                    }

                    if (basePage.LocalCategory.CustomerChangeableSortOrder)
                    {
                        this.Visible = true;
                    }
                    else
                    {
                        this.Visible = false;
                    }
                }
                //SetCategorySortOrder()
            }
        }

        private void SetCategorySortOrder(string value)
        {
            if (this.Page is BaseStoreCategoryPage)
            {
                BaseStoreCategoryPage basePage = (BaseStoreCategoryPage)this.Page;
                if (value != string.Empty)
                {
                    int parsedInt = 0;
                    if (int.TryParse(value, out parsedInt))
                    {
                        if (Enum.IsDefined(typeof(CategorySortOrder), parsedInt))
                        {
                            basePage.SortOrder = (CategorySortOrder)parsedInt;
                        }
                    }
                }

                else
                {
                    int parsedInt = 0;
                    if (int.TryParse(((int)this.SelectedSortOrder).ToString(), out parsedInt))
                    {
                        if (Enum.IsDefined(typeof(CategorySortOrder), parsedInt))
                        {
                            basePage.SortOrder = (CategorySortOrder)parsedInt;
                        }
                    }
                }
            }
        }
    }
}