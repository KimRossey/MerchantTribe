using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_ProductTypeDisplay : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (this.Page is IProductPage)
                {
                    IProductPage prodPage = (IProductPage)this.Page;
                    string productTypeId = prodPage.LocalProduct.ProductTypeId;
                    string productId = prodPage.LocalProduct.Bvin;

                    Collection<string> productTypePropertiesValues = new Collection<string>();
                    if (productTypeId.Trim() != string.Empty)
                    {
                        List<ProductProperty> props = MyPage.MTApp.CatalogServices.ProductPropertiesFindForType(productTypeId);
                        foreach (ProductProperty prop in props)
                        {
                            productTypePropertiesValues.Add(MyPage.MTApp.CatalogServices.ProductPropertyValues.GetPropertyValue(productId, prop.Id));
                        }
                        StringBuilder sb = new StringBuilder();
                        bool initialized = false;
                        for (int i = 0; i <= (props.Count - 1); i++)
                        {
                            if (props[i].DisplayOnSite)
                            {
                                string currentValue = MyPage.MTApp.CatalogServices.FormatProductPropertyChoiceValue(props[i], productTypePropertiesValues[i]);

                                //If text property is empty, do not display                            
                                if (!WebAppSettings.TypePropertiesDisplayEmptyProperties)
                                {
                                    if ((currentValue == string.Empty))
                                    {
                                        continue;
                                    }
                                }

                                if (!initialized)
                                {
                                    initialized = true;
                                    sb.Append("<ul class=\"typedisplay\">");
                                }

                                if (i % 2 == 0)
                                {
                                    sb.Append("<li>");
                                }
                                else
                                {
                                    sb.Append("<li class=\"alt\">");
                                }
                                sb.Append("<span class=\"productpropertylabel\">");
                                sb.Append(props[i].DisplayName);
                                sb.Append("</span>");
                                sb.Append("<span class=\"productpropertyvalue\">");
                                sb.Append(currentValue);
                                sb.Append("</span>");
                                sb.Append("</li>");
                            }
                        }
                        if (initialized)
                        {
                            sb.Append("</ul>");
                        }
                        TypeLiteral.Text = sb.ToString();
                    }
                }
            }
        }


    }
}