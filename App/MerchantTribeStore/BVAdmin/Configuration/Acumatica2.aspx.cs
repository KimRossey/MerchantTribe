using System;
using System.Web.UI;
using BVSoftware.Commerce.Membership;
using BVSoftware.AcumaticaTools;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BVSoftware.Commerce.Payment;
using BVSoftware.Commerce.Shipping;
using BVSoftware.Commerce.Accounts;

namespace BVCommerce.BVAdmin.Configuration
{
    public partial class Acumatica2 : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Acumatica Integration Settings - Mappings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AddPaymentControls();
            AddShippingControls();
        }
       
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)            
            {
                PopulateTaxClasses();
                if (lstNewItemTaxClass.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.NewItemTaxClassId) != null)
                {
                    lstNewItemTaxClass.ClearSelection();
                    lstNewItemTaxClass.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.NewItemTaxClassId).Selected = true;
                }

                PopulateWarehouses();
                if (lstWarehouses.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.NewItemWarehouseId) != null)
                {
                    lstWarehouses.ClearSelection();
                    lstWarehouses.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.NewItemWarehouseId).Selected = true;
                }
                if (lstWarehouses2.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.OrderLineItemWarehouseId) != null)
                {
                    lstWarehouses2.ClearSelection();
                    lstWarehouses2.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.OrderLineItemWarehouseId).Selected = true;
                }
                
                PopulatePaymentMethods();
                if (lstPayments.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.PaymentCCId) != null)
                {
                    lstPayments.ClearSelection();
                    lstPayments.Items.FindByValue(BVApp.CurrentStore.Settings.Acumatica.PaymentCCId).Selected = true;
                }

                PopulateShippingMethods();

                this.chkCustomerIdString.Checked = BVApp.CurrentStore.Settings.Acumatica.CustomerIdIsString;
                
            }
        }

        private void PopulateTaxClasses()
        {
            ServiceContext context = Connections.Login(BVApp.CurrentStore.Settings.Acumatica.Username,
                                                       BVApp.CurrentStore.Settings.Acumatica.Password,
                                                       BVApp.CurrentStore.Settings.Acumatica.SiteUrl);

            List<BVSoftware.AcumaticaTools.AccountDescriptor> classes = BVSoftware.AcumaticaTools.Accounts.ListAllTaxClasses(context);
            lstNewItemTaxClass.Items.Clear();
            foreach (AccountDescriptor a in classes)
            {
                lstNewItemTaxClass.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.Id));
            }
        }

        private void PopulateWarehouses()
        {
            ServiceContext context = Connections.Login(BVApp.CurrentStore.Settings.Acumatica.Username,
                                                       BVApp.CurrentStore.Settings.Acumatica.Password,
                                                       BVApp.CurrentStore.Settings.Acumatica.SiteUrl);
            List<BVSoftware.AcumaticaTools.AccountDescriptor> classes = BVSoftware.AcumaticaTools.Products.ListAllWarehouses(context);
            lstWarehouses.Items.Clear();
            lstWarehouses2.Items.Clear();
            foreach (AccountDescriptor a in classes)
            {
                lstWarehouses.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.Id));
                lstWarehouses2.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.Id));
            }
        }

        private void PopulatePaymentMethods()
        {
            ServiceContext context = Connections.Login(BVApp.CurrentStore.Settings.Acumatica.Username,
                                                       BVApp.CurrentStore.Settings.Acumatica.Password,
                                                       BVApp.CurrentStore.Settings.Acumatica.SiteUrl);

            List<BVSoftware.AcumaticaTools.AccountDescriptor> classes = BVSoftware.AcumaticaTools.Orders.ListAllPaymentMethods(context);
            this.lstPayments.Items.Clear();
            foreach (AccountDescriptor a in classes)
            {
                lstPayments.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.Id));
            }


            AvailablePayments availablePayments = new AvailablePayments();
            Collection<DisplayPaymentMethod> methods = availablePayments.AvailableMethodsForPlan(BVApp.CurrentStore.PlanId);
            foreach (DisplayPaymentMethod method in methods)
            {
                if (method.MethodId == BVSoftware.Commerce.Payment.Method.CreditCard.Id())
                {
                    PopulateList("lst" + method.MethodId + ((int)BVSoftware.Payment.CardType.Visa).ToString(), classes);
                    PopulateList("lst" + method.MethodId + ((int)BVSoftware.Payment.CardType.MasterCard).ToString(), classes);
                    PopulateList("lst" + method.MethodId + ((int)BVSoftware.Payment.CardType.Amex).ToString(), classes);
                    PopulateList("lst" + method.MethodId + ((int)BVSoftware.Payment.CardType.Discover).ToString(), classes);
                    PopulateList("lst" + method.MethodId + ((int)BVSoftware.Payment.CardType.DinersClub).ToString(), classes);
                    PopulateList("lst" + method.MethodId + ((int)BVSoftware.Payment.CardType.JCB).ToString(), classes);
                }
                else
                {
                    PopulateList("lst" + method.MethodId, classes);
                }
            }

            // Set Existing Settings
            foreach (BVSoftware.Commerce.Accounts.StoreIntegrationPaymentMapping m in BVApp.CurrentStore.Settings.Acumatica.PaymentMappings)
            {
                string listName = "lst" + m.BVPaymentMethodId + m.BVCardType;
                System.Web.UI.WebControls.DropDownList lst = (System.Web.UI.WebControls.DropDownList)phPayment.FindControl(listName);
                if (lst != null)
                {
                    if (lst.Items.FindByValue(m.IntegrationPaymentMethodId) != null)
                    {
                        lst.ClearSelection();
                        lst.Items.FindByValue(m.IntegrationPaymentMethodId).Selected = true;
                    }
                }
            }
        }

        private void PopulateList(string listId, List<BVSoftware.AcumaticaTools.AccountDescriptor> classes)
        {
            System.Web.UI.WebControls.DropDownList lst = (System.Web.UI.WebControls.DropDownList)this.phPayment.FindControl(listId);
            if (lst != null)
            {
                lst.Items.Add(new System.Web.UI.WebControls.ListItem("- Not Set -",""));
                foreach (AccountDescriptor a in classes)
                {
                    lst.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.Id));
                }
            }
        }

        private void PopulateShippingMethods()
        {
            ServiceContext context = Connections.Login(BVApp.CurrentStore.Settings.Acumatica.Username,
                                                       BVApp.CurrentStore.Settings.Acumatica.Password,
                                                       BVApp.CurrentStore.Settings.Acumatica.SiteUrl);

            List<BVSoftware.AcumaticaTools.AccountDescriptor> classes = BVSoftware.AcumaticaTools.Orders.ListAllShippingMethods(context);

            List<ShippingMethod> methods;
            methods = BVApp.OrderServices.ShippingMethods.FindAll(BVApp.CurrentStore.Id);

            foreach (ShippingMethod method in methods)
            {                
                    System.Web.UI.WebControls.DropDownList lst = (System.Web.UI.WebControls.DropDownList)this.phShipping.FindControl("lst" + method.Bvin);
                    if (lst != null)
                    {
                        lst.Items.Add(new System.Web.UI.WebControls.ListItem("- Not Set -", ""));
                        foreach (AccountDescriptor a in classes)
                        {
                            lst.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.Id));
                        }
                    }                
            }

            // Set Existing Settings
            foreach (BVSoftware.Commerce.Accounts.StoreIntegrationShippingMapping m in BVApp.CurrentStore.Settings.Acumatica.ShippingMappings)
            {
                string listName = "lst" + m.BVShippingMethodId + m.BVServiceCode;
                System.Web.UI.WebControls.DropDownList lst = (System.Web.UI.WebControls.DropDownList)phShipping.FindControl(listName);
                if (lst != null)
                {
                    if (lst.Items.FindByValue(m.IntegrationShippingMethodId) != null)
                    {
                        lst.ClearSelection();
                        lst.Items.FindByValue(m.IntegrationShippingMethodId).Selected = true;
                    }
                }
            }
           
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("acumatica.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Settings saved successfully.");
            }
        }

        private bool Save()
        {
            BVApp.CurrentStore.Settings.Acumatica.NewItemTaxClassId = this.lstNewItemTaxClass.SelectedItem.Value;
            BVApp.CurrentStore.Settings.Acumatica.NewItemWarehouseId = this.lstWarehouses.SelectedItem.Value;
            BVApp.CurrentStore.Settings.Acumatica.OrderLineItemWarehouseId = this.lstWarehouses2.SelectedItem.Value;
            BVApp.CurrentStore.Settings.Acumatica.PaymentCCId = this.lstPayments.SelectedItem.Value;
            BVApp.CurrentStore.Settings.Acumatica.CustomerIdIsString = this.chkCustomerIdString.Checked;

            SavePaymentMappings();
            SaveShippingMappings();

            BVApp.UpdateCurrentStore();
            return true;
        }

        private void SavePaymentMappings()
        {

            List<StoreIntegrationPaymentMapping> mappings = new List<StoreIntegrationPaymentMapping>();
           
            AvailablePayments availablePayments = new AvailablePayments();
            Collection<DisplayPaymentMethod> methods = availablePayments.AvailableMethodsForPlan(BVApp.CurrentStore.PlanId);
            foreach (DisplayPaymentMethod method in methods)
            {
                if (method.MethodId == BVSoftware.Commerce.Payment.Method.CreditCard.Id())
                {
                    AddPaySetting(method.MethodId, ((int)BVSoftware.Payment.CardType.Visa).ToString(), mappings);
                    AddPaySetting(method.MethodId, ((int)BVSoftware.Payment.CardType.MasterCard).ToString(), mappings);
                    AddPaySetting(method.MethodId, ((int)BVSoftware.Payment.CardType.Amex).ToString(), mappings);
                    AddPaySetting(method.MethodId, ((int)BVSoftware.Payment.CardType.Discover).ToString(), mappings);
                    AddPaySetting(method.MethodId, ((int)BVSoftware.Payment.CardType.DinersClub).ToString(), mappings);
                    AddPaySetting(method.MethodId, ((int)BVSoftware.Payment.CardType.JCB).ToString(), mappings);
                }
                else
                {
                    //AddPaySetting(method.MethodId,"", mappings);
                }
            }

            BVApp.CurrentStore.Settings.Acumatica.PaymentMappings = mappings;
            
        }
        private void AddPaySetting(string id, string extra, List<StoreIntegrationPaymentMapping> mappings)
        {
            System.Web.UI.WebControls.DropDownList lst = (System.Web.UI.WebControls.DropDownList)this.phShipping.FindControl("lst" + id + extra);
            if (lst != null)
            {
                StoreIntegrationPaymentMapping map = new StoreIntegrationPaymentMapping();
                map.BVPaymentMethodId = id;
                map.BVCardType = extra;
                map.IntegrationPaymentMethodId = lst.SelectedItem.Value;
                mappings.Add(map);
            }             
        }

        private void SaveShippingMappings()
        {

            List<StoreIntegrationShippingMapping> mappings = new List<StoreIntegrationShippingMapping>();

            List<ShippingMethod> methods = BVApp.OrderServices.ShippingMethods.FindAll(BVApp.CurrentStore.Id);            

            foreach (ShippingMethod method in methods)
            {                
                    System.Web.UI.WebControls.DropDownList lst = (System.Web.UI.WebControls.DropDownList)this.phShipping.FindControl("lst" + method.Bvin);
                    if (lst != null)
                    {
                        StoreIntegrationShippingMapping map = new StoreIntegrationShippingMapping();
                        map.BVShippingMethodId = method.Bvin;
                        map.IntegrationShippingMethodId = lst.SelectedItem.Value;
                        mappings.Add(map);
                    }                
            }

            BVApp.CurrentStore.Settings.Acumatica.ShippingMappings = mappings;
        }

        private void AddPaymentControls()
        {
            AvailablePayments availablePayments = new AvailablePayments();
            Collection<DisplayPaymentMethod> methods = availablePayments.AvailableMethodsForPlan(BVApp.CurrentStore.PlanId);
          
            this.phPayment.Controls.Add(new LiteralControl("<table>"));

            foreach (DisplayPaymentMethod method in methods)
            {                
                if (method.MethodId == BVSoftware.Commerce.Payment.Method.CreditCard.Id())
                {
                    BuildPayDrop(method.MethodId + ((int)BVSoftware.Payment.CardType.Visa).ToString(), method.MethodName + " - Visa", phPayment);
                    BuildPayDrop(method.MethodId + ((int)BVSoftware.Payment.CardType.MasterCard).ToString(), method.MethodName + " - MasterCard", phPayment);
                    BuildPayDrop(method.MethodId + ((int)BVSoftware.Payment.CardType.Amex).ToString(), method.MethodName + " - Amex", phPayment);
                    BuildPayDrop(method.MethodId + ((int)BVSoftware.Payment.CardType.Discover).ToString(), method.MethodName + " - Discover", phPayment);
                    BuildPayDrop(method.MethodId + ((int)BVSoftware.Payment.CardType.DinersClub).ToString(), method.MethodName + " - Diners Club", phPayment);
                    BuildPayDrop(method.MethodId + ((int)BVSoftware.Payment.CardType.JCB).ToString(), method.MethodName + " - JCB", phPayment);
                }
                else
                {
                    BuildPayDrop(method.MethodId, method.MethodName, phPayment);                                    
                }                                
            }
            
            this.phPayment.Controls.Add(new LiteralControl("</table>"));
        }

        private void BuildPayDrop(string id, string name, System.Web.UI.WebControls.PlaceHolder ph)
        {
            LiteralControl lit = new LiteralControl();
            lit.Text += "<tr><td>";
            lit.Text += name;
            lit.Text += "</td><td>";
            ph.Controls.Add(lit);

            System.Web.UI.WebControls.DropDownList lst = new System.Web.UI.WebControls.DropDownList();
            lst.ID = "lst" + id;
            ph.Controls.Add(lst);

            LiteralControl lit2 = new LiteralControl();
            lit2.Text += "</td></tr>";
            ph.Controls.Add(lit2);
        }

        private void AddShippingControls()
        {
            List<ShippingMethod> methods = BVApp.OrderServices.ShippingMethods.FindAll(BVApp.CurrentStore.Id);
            
            this.phShipping.Controls.Add(new LiteralControl("<table>"));

            foreach (ShippingMethod method in methods)
            {
                LiteralControl lit = new LiteralControl();
                lit.Text += "<tr><td>";
                lit.Text += method.Name;
                lit.Text += "</td><td>";
                phShipping.Controls.Add(lit);

                System.Web.UI.WebControls.DropDownList lst = new System.Web.UI.WebControls.DropDownList();
                lst.ID = "lst" + method.Bvin;
                phShipping.Controls.Add(lst);

                LiteralControl lit2 = new LiteralControl();
                lit2.Text += "</td></tr>";
                phShipping.Controls.Add(lit2);
            }

            this.phShipping.Controls.Add(new LiteralControl("</table>"));            
        }

        
        
    }
}