using System.Collections.ObjectModel;
using System.Text;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Utilities;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_Top_10_Products_view : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            LoadProducts();
        }

        private void LoadProducts()
        {
            System.DateTime s = new System.DateTime(1900, 1, 1);
            System.DateTime e = new System.DateTime(3000, 12, 31);
            List<Product> products = MyPage.BVApp.ReportingTopSellersByDate(s, e, 10);            

            RenderList(products);
        }

        private void RenderList(List<Product> products)
        {
            if (products != null)
            {
                this.ProductList.Controls.Clear();
                this.ProductList.Text = string.Empty;

                StringBuilder sb = new StringBuilder();
                if (products.Count > 0)
                {
                    sb.Append("<ol>");
                    for (int i = 0; i <= products.Count - 1; i++)
                    {
                        sb.Append(RenderProduct(products[i]));
                    }
                    sb.Append("</ol>");
                    this.ProductList.Text = sb.ToString();
                }
            }
        }

        private string RenderProduct(Product p)
        {
            string result = string.Empty;

            if (p != null)
            {
                result += "<li><a href=\"";
                result += UrlRewriter.BuildUrlForProduct(p, this.Page);
                result += "\" title=\"";
                result += p.ProductName;
                result += "\">";
                result += p.ProductName;
                result += " - ";
                result += p.SitePrice.ToString("c");
                result += "</a></li>";
            }

            return result;
        }


    }
}