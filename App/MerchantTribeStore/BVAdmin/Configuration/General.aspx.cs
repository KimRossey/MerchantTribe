using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_General : BaseAdminPage
    {

        public string LogoImage = "";
        public string CNameRoot = "";

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "General Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            // Hide request form for individual mode
            if (WebAppSettings.IsIndividualMode) this.pnlRequestForm.Visible = false;

            if (!Page.IsPostBack)
            {
                if ((Request.QueryString["ok"] == "1"))
                {
                    this.MessageBox1.ShowOk("Changes Saved");
                }
                this.CustomDomainField.Text = BVApp.CurrentStore.CustomUrl;
                this.SiteNameField.Text = BVApp.CurrentStore.Settings.FriendlyName;
                this.LogoTextField.Text = BVApp.CurrentStore.Settings.LogoText;
                this.uselogoimage.Checked = BVApp.CurrentStore.Settings.UseLogoImage;
                this.chkHideGettingStarted.Checked = BVApp.CurrentStore.Settings.HideGettingStarted;
                CNameRoot = BVApp.CurrentStore.StoreName + ".bvcommerce.com";
                this.chkClosed.Checked = BVApp.CurrentStore.Settings.StoreClosed;
                this.ClosedMessageField.Text = BVApp.CurrentStore.Settings.StoreClosedDescription;
                this.GuestPassword.Text = BVApp.CurrentStore.Settings.StoreClosedGuestPassword;
            }
            UpdateLogoImage();
            LoadAlternateDomains();
        }

        private void UpdateLogoImage()
        {
            LogoImage = BVApp.CurrentStore.Settings.LogoImageFullUrl(Page.Request.IsSecureConnection);
            if (BVApp.CurrentStore.Settings.LogoImage.Trim() == string.Empty)
            {
                LogoImage = "../../content/admin/images/MissingImage.png";
            }
            BVApp.UpdateCurrentStore();
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Settings saved successfully.");
                Response.Redirect("General.aspx?ok=1");
            }
        }

        private bool Save()
        {
            bool result = false;
            BVApp.CurrentStore.Settings.HideGettingStarted = this.chkHideGettingStarted.Checked;
            BVApp.CurrentStore.Settings.UseLogoImage = this.uselogoimage.Checked;
            BVApp.CurrentStore.Settings.FriendlyName = this.SiteNameField.Text.Trim();
            BVApp.CurrentStore.Settings.LogoText = this.LogoTextField.Text.Trim();
            result = true;

            if ((imgupload.HasFile))
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(imgupload.FileName);
                string ext = System.IO.Path.GetExtension(imgupload.FileName);

                if (MerchantTribe.Commerce.Storage.DiskStorage.ValidateImageType(ext))
                {
                    fileName = MerchantTribe.Web.Text.CleanFileName(fileName);
                    if ((MerchantTribe.Commerce.Storage.DiskStorage.UploadStoreImage(BVApp.CurrentStore, this.imgupload.PostedFile)))
                    {
                        BVApp.CurrentStore.Settings.LogoImage = fileName + ext;
                    }
                }
                else
                {
                    result = false;
                    this.MessageBox1.ShowError("Only .PNG, .JPG, .GIF file types are allowed for logo images");
                }
            }
            BVApp.CurrentStore.Settings.StoreClosed = this.chkClosed.Checked;
            BVApp.CurrentStore.Settings.StoreClosedDescription = this.ClosedMessageField.Text.Trim();
            BVApp.CurrentStore.Settings.StoreClosedGuestPassword = this.GuestPassword.Text.Trim();

            BVApp.UpdateCurrentStore();
            return result;
        }
        protected void btnSend_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            string requestedDomain = this.RequestedDomain.Text;
            string ownsDomain = this.lstOwnAlready.SelectedItem.Text;
            string hasSSL = this.lstHaveSSL.SelectedItem.Text;
            string contactEmail = this.ContactEmail.Text;
            string contactPhone = this.ContactPhone.Text;
            string contactName = this.ContactName.Text;

            MailServices.SendCustomDomainRequest(contactName, contactEmail, contactPhone, requestedDomain, ownsDomain, hasSSL);
            this.MessageBox1.ShowOk("Domain Request Sent! Thank you. We'll be in touch soon.");
        }

        protected void btnUpdateCustomDomain_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            BVApp.CurrentStore.CustomUrl = this.CustomDomainField.Text.Trim();
            if (BVApp.UpdateCurrentStore())
            {
                this.MessageBox1.ShowOk("Custom Domain Name Updated");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to update custom domain name.");
            }
        }

        protected void lnkAddCustomDomain_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            StoreDomain d = new StoreDomain();
            d.StoreId = BVApp.CurrentStore.Id;
            d.DomainName = this.NewCustomDomain.Text.Trim();

            StoreDomainRepository repo = new StoreDomainRepository(this.BVApp.CurrentRequestContext);
            if (repo.Create(d))
            {
                this.MessageBox1.ShowOk("Added alternate domain");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to add alternate domain");
            }

            LoadAlternateDomains();
        }

        private void LoadAlternateDomains()
        {
            StoreDomainRepository repo = new StoreDomainRepository(this.BVApp.CurrentRequestContext);
            List<StoreDomain> domains = repo.FindForStore(BVApp.CurrentStore.Id);

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"redirects301\">");

            foreach (StoreDomain d in domains)
            {
                sb.Append("<li>");
                sb.Append(d.DomainName);
                sb.Append(" <a id=\"remove" + d.Id.ToString() + "\" href=\"#\" class=\"remove301\">Remove</a>");
                sb.Append("</li>");
            }

            sb.Append("</ul>");

            this.litCustomDomains.Text = sb.ToString();
        }
    }
}