using System;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Payment;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Controls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using MerchantTribe.Shipping;

namespace BVCommerce
{

    partial class BVModules_PaymentMethods_Google_Checkout_Edit : BVModule
    {

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                SaveData();
                this.NotifyFinishedEditing();
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadShippingFields();
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            this.MerchantIdTextBox.Text = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.MerchantId;
            this.MerchantKeyTextBox.Text = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.MerchantKey;
            this.ModeRadioButtonList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.Mode;
            this.GoogleMonetaryFormatRadioButtonList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.Currency;
            this.CheckoutButtonSizeDropDownList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.ButtonSize;
            this.CheckoutButtonBackgroundDropDownList.SelectedValue = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.ButtonBackground;
            this.DebugModeCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DebugMode;
            this.DaysOldTextBox.Text = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.MinimumAccountDaysOld.ToString();
            this.CartValidMinutesTextBox.Text = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CartMinutes.ToString();

            this.AVSErrorCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSErrorPutHold;
            this.AVSFailsCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSFailsPutHold;
            this.AVSNotSupportedCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSNotSupportedPutHold;
            this.AVSPartialMatchCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSPartialMatchPutHold;

            this.CVNErrorCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CVNErrorPutHold;
            this.CVNNoMatchCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CVNNoMatchPutHold;
            this.CVNNotAvailableCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CVNNotAvailablePutHold;

            this.GoogleProtectionEligibleCheckBox.Checked = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.PaymentProtectionEligiblePutHold;

            this.BaseDefaultShippingTextBox.Text = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DefaultShippingAmount.ToString("c");

            this.DefaultShippingTypeRadioButtonList.SelectedValue = ((int)MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DefaultShippingType).ToString();

            MerchantTribe.Commerce.Utilities.SerializableDictionary<string, decimal> defaultShippingValues = MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DefaultShippingValues;
            foreach (System.Web.UI.Control control in ShippingSettingsPlaceHolder.Controls)
            {
                if (control is System.Web.UI.HtmlControls.HtmlTableRow)
                {
                    string method = string.Empty;
                    foreach (System.Web.UI.Control cell in control.Controls)
                    {
                        if (cell is System.Web.UI.HtmlControls.HtmlTableCell)
                        {
                            System.Web.UI.HtmlControls.HtmlTableCell currCell = (System.Web.UI.HtmlControls.HtmlTableCell)cell;
                            foreach (Control item in cell.Controls)
                            {
                                if (item is TextBox)
                                {
                                    TextBox currTextBox = (TextBox)item;
                                    decimal value = 0m;
                                    if (defaultShippingValues.TryGetValue(method, out value))
                                    {
                                        currTextBox.Text = value.ToString("c");
                                    }
                                    else
                                    {
                                        currTextBox.Text = value.ToString("c");
                                    }
                                }
                                else if (item is LiteralControl)
                                {
                                    method = ((LiteralControl)item).Text;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SaveData()
        {
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.MerchantId = this.MerchantIdTextBox.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.MerchantKey = this.MerchantKeyTextBox.Text.Trim();
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.Mode = this.ModeRadioButtonList.SelectedValue;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.Currency = this.GoogleMonetaryFormatRadioButtonList.SelectedValue;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.ButtonSize = this.CheckoutButtonSizeDropDownList.SelectedValue;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.ButtonBackground = this.CheckoutButtonBackgroundDropDownList.SelectedValue;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DebugMode = this.DebugModeCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.MinimumAccountDaysOld = int.Parse(this.DaysOldTextBox.Text);
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CartMinutes = int.Parse(this.CartValidMinutesTextBox.Text);

            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSErrorPutHold = this.AVSErrorCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSFailsPutHold = this.AVSFailsCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSNotSupportedPutHold = this.AVSNotSupportedCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.AVSPartialMatchPutHold = this.AVSPartialMatchCheckBox.Checked;

            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CVNErrorPutHold = this.CVNErrorCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CVNNoMatchPutHold = this.CVNNoMatchCheckBox.Checked;
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.CVNNotAvailablePutHold = this.CVNNotAvailableCheckBox.Checked;

            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.PaymentProtectionEligiblePutHold = this.GoogleProtectionEligibleCheckBox.Checked;

            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DefaultShippingAmount = decimal.Parse(this.BaseDefaultShippingTextBox.Text, System.Globalization.NumberStyles.Currency);
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DefaultShippingType = (GoogleDefaultShippingTypes)int.Parse(this.DefaultShippingTypeRadioButtonList.SelectedValue);

            MerchantTribe.Commerce.Utilities.SerializableDictionary<string, decimal> defaultShippingValues = new MerchantTribe.Commerce.Utilities.SerializableDictionary<string, decimal>();
            foreach (System.Web.UI.Control control in ShippingSettingsPlaceHolder.Controls)
            {
                if (control is System.Web.UI.HtmlControls.HtmlTableRow)
                {
                    string method = string.Empty;
                    foreach (Control cell in control.Controls)
                    {
                        if (cell is HtmlTableCell)
                        {
                            HtmlTableCell currCell = (HtmlTableCell)cell;
                            foreach (Control item in cell.Controls)
                            {
                                if (item is TextBox)
                                {
                                    TextBox currTextBox = (TextBox)item;
                                    decimal value = 0m;
                                    if (decimal.TryParse(currTextBox.Text, System.Globalization.NumberStyles.Currency, System.Threading.Thread.CurrentThread.CurrentUICulture, out value))
                                    {
                                        if (!defaultShippingValues.ContainsKey(method))
                                        {
                                            defaultShippingValues.Add(method, value);
                                        }
                                    }
                                }
                                else if (item is LiteralControl)
                                {
                                    method = ((LiteralControl)item).Text;
                                }
                            }
                        }
                    }
                }
            }
            MyPage.MTApp.CurrentStore.Settings.GoogleCheckout.DefaultShippingValues = defaultShippingValues;

            MyPage.MTApp.AccountServices.Stores.Update(MyPage.MTApp.CurrentStore);
        }

        public void LoadShippingFields()
        {
            Collection<string> methods = new Collection<string>();
            int count = 1;
            foreach (MerchantTribe.Commerce.Shipping.ShippingMethod method in MyPage.MTApp.OrderServices.ShippingMethods.FindAll(MyPage.MTApp.CurrentStore.Id))
            {
                IShippingService provider = MerchantTribe.Commerce.Shipping.AvailableServices.FindById(method.ShippingProviderId, CurrentStore);
                if (provider != null)
                {
                    //foreach (string newMethod in provider.GetSelectedShippingNames(method)) {
                    //    System.Web.UI.HtmlControls.HtmlTableRow tr = new System.Web.UI.HtmlControls.HtmlTableRow();
                    //    System.Web.UI.HtmlControls.HtmlTableCell cell1 = new System.Web.UI.HtmlControls.HtmlTableCell();
                    //    cell1.Attributes.Add("class", "formlabel");
                    //    System.Web.UI.HtmlControls.HtmlTableCell cell2 = new System.Web.UI.HtmlControls.HtmlTableCell();
                    //    cell2.Attributes.Add("class", "formfield");
                    //    tr.Controls.Add(cell1);
                    //    tr.Controls.Add(cell2);
                    //    cell1.InnerText = newMethod;
                    //    TextBox textBox = new TextBox();
                    //    textBox.ID = "TextBox" + count.ToString();
                    //    count += 1;
                    //    cell2.Controls.Add(textBox);

                    //    MerchantTribe.Commerce.Controls.BVCustomValidator validator = new MerchantTribe.Commerce.Controls.BVCustomValidator();
                    //    validator.ControlToValidate = textBox.ID;
                    //    validator.ErrorMessage = newMethod + " Default Shipping Amount Must Be A Valid Monetary Amount.";
                    //    validator.Text = " *";
                    //    validator.ServerValidate += ServerValidate;
                    //    cell2.Controls.Add(validator);
                    //    ShippingSettingsPlaceHolder.Controls.Add(tr);
                    //}
                }
            }
        }

        public void ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal temp;
            if (!decimal.TryParse(args.Value, System.Globalization.NumberStyles.Currency, System.Threading.Thread.CurrentThread.CurrentUICulture, out temp))
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

    }
}