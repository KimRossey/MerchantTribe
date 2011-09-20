using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Content.Templates;
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_Template_Edit : BaseAdminPage
    {


        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Template";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }


        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            Collection<ParserMessage> validationResults = TemplateParser.ValidateProductPageTemplate(this.TemplateField.Text);
            if (validationResults != null)
            {
                if (validationResults.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (ParserMessage pm in validationResults)
                    {
                        switch (pm.MessageType)
                        {
                            case ParserMessageType.Error:
                                sb.Append("ERROR  : " + pm.MessageType + " LINE: " + pm.LineNumber + "<br />");
                                break;
                            case ParserMessageType.Information:
                                sb.Append("INFO   : " + pm.MessageType + " LINE: " + pm.LineNumber + "<br />");
                                break;
                            case ParserMessageType.Warning:
                                sb.Append("WARNING: " + pm.MessageType + " LINE: " + pm.LineNumber + "<br />");
                                break;
                        }
                    }
                    this.MessageBox1.ShowInformation(sb.ToString());
                }
                else
                {
                    if (TemplateParser.GenerateProductPage(this.TemplateField.Text))
                    {
                        this.MessageBox1.ShowOk("Template Saved!");
                    }
                    else
                    {
                        this.MessageBox1.ShowError("Unable to generate page. Unknown Error!");
                    }
                }
            }
            else
            {
                this.MessageBox1.ShowError("Unable to validate. Unknown error.");
            }

        }

    }
}