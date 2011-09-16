using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVSoftware.Commerce;
using BVSoftware.AcumaticaTools;
using System.Text;
using System.Collections.ObjectModel;
using BVSoftware.Commerce.Accounts;
using MerchantTribe.Web;

namespace BVCommerce
{
    public class AcumaticaIntegration
    {
        public static Guid PullDataTaskProcessorId
        {
            get
            {
                return new Guid("B7455E95-EBB9-479E-AD52-E80A5783FD52");
            }
        }

        private BVApplication _bvapp = null;

        public static AcumaticaIntegration Factory(BVApplication bvapp)
        {
            AcumaticaIntegration acumatica = new AcumaticaIntegration(bvapp.CurrentStore.Settings.Acumatica.Username,
                                                    bvapp.CurrentStore.Settings.Acumatica.Password,
                                                    bvapp.CurrentStore.Settings.Acumatica.SiteUrl,
                                                    bvapp.CurrentStore.Settings.Acumatica.NewItemTaxClassId,
                                                    bvapp.CurrentStore.Settings.Acumatica.NewItemWarehouseId,
                                                    bvapp.CurrentStore.Settings.Acumatica.OrderLineItemWarehouseId,
                                                    bvapp.CurrentStore.Settings.Acumatica.PaymentCCId,
                                                    bvapp.CurrentStore.Settings.Acumatica.CustomerIdIsString,
                                                    bvapp.CurrentStore.Settings.Acumatica.PaymentMappings,
                                                    bvapp.CurrentStore.Settings.Acumatica.ShippingMappings);
            return acumatica;
        }

        ServiceContext _ServiceContext = new ServiceContext();

        public AcumaticaIntegration(string username, string password, string siteUrl, string newItemTaxClassId, 
                                    string newItemWarehouseId, string newLineItemWarehouseId,
                                    string defaultCustomerPaymentIdentifier, bool customerIdIsString,
                                    List<StoreIntegrationPaymentMapping> paymentMappings,
                                    List<StoreIntegrationShippingMapping> shippingMappings)
        {
            _ServiceContext.Password = password;
            _ServiceContext.Username = username;
            _ServiceContext.SiteAddress = siteUrl;
            _ServiceContext.NewItemTaxAccountId = newItemTaxClassId;
            _ServiceContext.NewItemWarehouseId = newItemWarehouseId;
            _ServiceContext.DefaultCustomerPaymentMethod = defaultCustomerPaymentIdentifier;
            _ServiceContext.UseFullCustomerNameInsteadOfAutoId = customerIdIsString;
            _ServiceContext.NewLineItemWarehouseId = newLineItemWarehouseId;

            List<IntegrationMapping> payments = new List<IntegrationMapping>();
            foreach (StoreIntegrationPaymentMapping pay in paymentMappings)
            {
                IntegrationMapping temp = new IntegrationMapping(pay.Serialize());
                payments.Add(temp);
            }
            _ServiceContext.PaymentMappings = payments;

            List<IntegrationMapping> shipments = new List<IntegrationMapping>();
            foreach (StoreIntegrationShippingMapping ship in shippingMappings)
            {
                IntegrationMapping temp = new IntegrationMapping(ship.Serialize());
                shipments.Add(temp);
            }
            _ServiceContext.ShippingMappings = shipments;

        }

        private bool EnsureLogin()
        {
                string newItemTaxClassId = _ServiceContext.NewItemTaxAccountId;
                string newItemWarehouseId = _ServiceContext.NewItemWarehouseId;
                string newLineItemWarehouseId = _ServiceContext.NewLineItemWarehouseId;
                string defaultPaymentId = _ServiceContext.DefaultCustomerPaymentMethod;
                bool customerIdIsString = _ServiceContext.UseFullCustomerNameInsteadOfAutoId;
                List<IntegrationMapping> payments = _ServiceContext.PaymentMappings;
                List<IntegrationMapping> shipments = _ServiceContext.ShippingMappings;

                if (_ServiceContext.HasLoggedIn)
                {
                    return true;
                }
                else
                {
                    _ServiceContext = Connections.Login(_ServiceContext.Username, _ServiceContext.Password, _ServiceContext.SiteAddress);
                    _ServiceContext.NewItemTaxAccountId = newItemTaxClassId;
                    _ServiceContext.NewItemWarehouseId = newItemWarehouseId;
                    _ServiceContext.NewLineItemWarehouseId = newLineItemWarehouseId;
                    _ServiceContext.DefaultCustomerPaymentMethod = defaultPaymentId;
                    _ServiceContext.UseFullCustomerNameInsteadOfAutoId = customerIdIsString;
                    _ServiceContext.PaymentMappings = payments;
                    _ServiceContext.ShippingMappings = shipments;

                    return _ServiceContext.HasLoggedIn;
                }
                        
        }

        public void OnCustomerAccountCreated(object sender, BVSoftware.Commerce.Membership.CustomerAccount account)
        {
            try
            {
                if (!EnsureLogin()) return;
            
                CustomerData data = new CustomerData() { Email = account.Email, FirstName = account.FirstName, LastName = account.LastName };
                BVSoftware.Commerce.Contacts.Address billing = account.GetBillingAddress();
                data.CompanyName = billing.Company;
                data.PhoneNumber = billing.Phone;
                data.BillingAddress = billing;
                BVSoftware.Commerce.Contacts.Address shipping = account.GetShippingAddress();
                data.ShippingAddress = shipping;
                if (data.CompanyName.Trim().Length < 1) data.CompanyName = shipping.Company;
                if (data.PhoneNumber.Trim().Length < 1) data.PhoneNumber = shipping.Phone;

                data.Bvin = account.Bvin;
                data.PaymentIdentifier = _ServiceContext.DefaultCustomerPaymentMethod;
                data.PaymentMethod = _ServiceContext.DefaultCustomerPaymentMethod;

                string customerId = Customers.GetOrCreateCustomer(data, _ServiceContext);

                if (customerId != string.Empty)
                {
                    EventLog.LogEvent("Acumatica Integration", "Create Acumatica Customer With ID " + customerId, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    Console.WriteLine("No Customer Found with that Email");
                    foreach (ServiceError e in _ServiceContext.Errors)
                    {
                        sb.Append("ERROR: " + e.Description + " | ");
                    }
                    EventLog.LogEvent("Acumatica Integration", "Failed to Create Acumatica Customer " + sb.ToString(), BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);                
            }
        }

        public void OnCustomerAccountUpdated(object sender, BVSoftware.Commerce.Membership.CustomerAccount account)
        {
            if (!EnsureLogin()) return;

            try
            {
                string AcumaticaId = Customers.GetCustomerIdByEmail(account.Email, _ServiceContext);

                CustomerData data = new CustomerData() { Email = account.Email, FirstName = account.FirstName, LastName = account.LastName };
                data.AcumaticaId = AcumaticaId;
                data.Bvin = account.Bvin;

                BVSoftware.Commerce.Contacts.Address billing = account.GetBillingAddress();
                data.CompanyName = billing.Company;
                data.PhoneNumber = billing.Phone;
                data.BillingAddress = billing;
                BVSoftware.Commerce.Contacts.Address shipping = account.GetShippingAddress();
                data.ShippingAddress = shipping;
                if (data.CompanyName.Trim().Length < 1) data.CompanyName = shipping.Company;
                if (data.PhoneNumber.Trim().Length < 1) data.PhoneNumber = shipping.Phone;
                
                data.PaymentIdentifier = _ServiceContext.DefaultCustomerPaymentMethod;
                data.PaymentMethod = _ServiceContext.DefaultCustomerPaymentMethod;

                bool result = Customers.UpdateCustomerData(data, _ServiceContext);

                if (result)
                {
                    EventLog.LogEvent("Acumatica Integration", "Customer Data Updated for Email " + account.Email, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    Console.WriteLine("Failed to Update Customer Record in Acumatica for Email " + account.Email);
                    foreach (ServiceError e in _ServiceContext.Errors)
                    {
                        sb.Append("ERROR: " + e.Description + " | ");
                    }
                    EventLog.LogEvent("Acumatica Integration", "Failed to Update Acumatica Customer " + sb.ToString(), BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

        }
        
        public void OnCustomerAccountDeleted(object sender, BVSoftware.Commerce.Membership.CustomerAccount account)
        {
            if (!EnsureLogin()) return;

        }

        public void OnCustomerAccountEmailChanged(object sender, string oldEmail, string newEmail)
        {
            if (!EnsureLogin()) return;

            try
            {
                bool result = false;

                string customerID = Customers.GetCustomerIdByEmail(oldEmail, _ServiceContext);
                if (customerID != string.Empty)
                {
                    result = Customers.UpdateCustomerEmail(customerID, newEmail, _ServiceContext);
                }

                if (result)
                {
                    EventLog.LogEvent("Acumatica Integration", "Customer Email Updated " + newEmail, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    Console.WriteLine("Failed to Update Customer Email in Acumatica for Email " + oldEmail);
                    foreach (ServiceError e in _ServiceContext.Errors)
                    {
                        sb.Append("ERROR: " + e.Description + " | ");
                    }
                    EventLog.LogEvent("Acumatica Integration", "Failed to Update Acumatica Customer Email " + sb.ToString(), BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
        }

        public void OnOrderReceived(object sender, BVSoftware.Commerce.Orders.Order order, BVApplication bvapp)
        {
            try
            {


                if (!EnsureLogin()) return;

                // Send Order to Acumatica;
                string acumaticaCustomerId = string.Empty;

                string companyName = order.BillingAddress.Company;
                if (companyName.Trim().Length < 1) companyName = order.ShippingAddress.Company;
                string phone = order.BillingAddress.Phone;
                if (phone.Trim().Length < 1) phone = order.ShippingAddress.Phone;

                if (order.UserID.Trim().Length > 0)
                {
                    // Customer has account in BV with id number
                   BVSoftware.Commerce.Membership.CustomerAccount customer = bvapp.MembershipServices.Customers.Find(order.UserID);
                   if (customer == null) customer = new BVSoftware.Commerce.Membership.CustomerAccount();
                        
                    acumaticaCustomerId = CreateAcumaticaAccountForCustomer(customer,companyName, phone, order.ShippingAddress, order.BillingAddress);
                }
                else
                {
                    // Customer is "anonymous" in BV
                    acumaticaCustomerId = CreateAcumaticaAccountForCustomer(order.UserEmail,                                                                            
                                                                            order.BillingAddress.FirstName,
                                                                            order.BillingAddress.LastName,
                                                                            companyName,
                                                                            phone,
                                                                            order.ShippingAddress,
                                                                            order.BillingAddress);

            }

            if (acumaticaCustomerId.Trim().Length < 1)
            {
                EventLog.LogEvent("Acumatica Integration On Order Created", "Unable to create acumatica account for customer " + order.UserEmail, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                StringBuilder sb = new StringBuilder();
                Console.WriteLine("Failed to Create/Find Customer In Acumatica");
                foreach (ServiceError e in _ServiceContext.Errors)
                {
                    sb.Append("ERROR: " + e.Description + " | ");
                }
                order.Notes.Add(new BVSoftware.Commerce.Orders.OrderNote() { Note = "Failed to send to Acumatica: " + sb.ToString(), IsPublic = false });
                bvapp.OrderServices.Orders.Update(order);
                return;
            }


                bool ItemsCreated = MakeSureAllItemsExist(order.Items);

                if (ItemsCreated == true)
                {




                    // Convert BV Order into generic ERP order
                    OrderData data = new OrderData()
                    {
                        BVOrderNumber = order.OrderNumber,
                        Customer = new CustomerData()
                        {
                            AcumaticaId = MerchantTribe.Web.Text.TrimToLength(acumaticaCustomerId, 10),
                            BillingAddress = order.BillingAddress,
                            ShippingAddress = order.ShippingAddress,
                            Bvin = order.bvin,
                            Email = order.UserEmail,
                            FirstName = order.BillingAddress.FirstName,
                            LastName = order.BillingAddress.LastName
                        },
                        ShippingAddress = order.ShippingAddress,
                        Shipping = new OrderShippingData()
                        {
                            ShippingMethodDescription = order.ShippingMethodDisplayName,
                            ShippingMethodId = order.ShippingMethodId,
                            ShippingTotal = order.TotalShippingAfterDiscounts
                        },
                        TaxTotal = order.TotalTax,
                        TimeOfOrder = order.TimeOfOrderUtc.ToLocalTime(),
                        GrandTotal = order.TotalGrand,
                        AcumaticaOrderNumber = string.Empty
                    };


                    // Loop through payments, take last credit card hold to pass to acumatica
                    //
                    // NOTE: Only supports a single transaction at the moment. Will need to 
                    // support gift cards and multiple payment transactions in the future.
                    //
                    BVSoftware.Commerce.Orders.OrderPaymentManager payManager 
                        = new BVSoftware.Commerce.Orders.OrderPaymentManager(order, bvapp);
                    List<BVSoftware.Commerce.Orders.OrderTransaction> creditCardHolds = payManager.CreditCardHoldListAll();
                    foreach (BVSoftware.Commerce.Orders.OrderTransaction t in creditCardHolds)
                    {
                        if (t.Success)
                        {
                            int actionNumber = (int)t.Action;
                            if (actionNumber > 99 && actionNumber < 200) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.CreditCard.Id();
                            if (actionNumber > 199 && actionNumber < 300) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.Check.Id();
                            if (actionNumber > 299 && actionNumber < 400) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.CashOnDelivery.Id();
                            if (actionNumber > 399 && actionNumber < 450) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.PurchaseOrder.Id();
                            if (actionNumber >= 450 && actionNumber < 500) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.CompanyAccount.Id();
                            if (actionNumber > 500 && actionNumber < 550) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.Cash.Id();
                            if (actionNumber > 600 && actionNumber < 700) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.PaypalExpress.Id();
                            if (actionNumber == 9999) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.Cash.Id();

                            data.Payment.Amount = t.Amount;
                            data.Payment.TransactionNumber = t.RefNum1;
                            data.Payment.AuthorizationCode = t.RefNum2;
                            data.Payment.Card = t.CreditCard;
                            data.Payment.Action = BVSoftware.Payment.ActionType.CreditCardHold;
                        }
                    }
                    if (data.Payment.TransactionNumber == string.Empty)
                    {
                        // Empty Auth, see if we have a charge                        
                        foreach (BVSoftware.Commerce.Orders.OrderTransaction t in payManager.CreditCardChargeOrCaptureListAll())
                        {
                            data.Payment.Amount = t.Amount;
                            data.Payment.TransactionNumber = t.RefNum1;
                            data.Payment.AuthorizationCode = t.RefNum2;
                            data.Payment.Card = t.CreditCard;
                            data.Payment.Action = t.Action;

                            int actionNumber = (int)t.Action;
                            if (actionNumber > 99 && actionNumber < 200) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.CreditCard.Id();
                            if (actionNumber > 199 && actionNumber < 300) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.Check.Id();
                            if (actionNumber > 299 && actionNumber < 400) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.CashOnDelivery.Id();
                            if (actionNumber > 399 && actionNumber < 450) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.PurchaseOrder.Id();
                            if (actionNumber >= 450 && actionNumber < 500) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.CompanyAccount.Id();
                            if (actionNumber > 500 && actionNumber < 550) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.Cash.Id();
                            if (actionNumber > 600 && actionNumber < 700) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.PaypalExpress.Id();
                            if (actionNumber == 9999) data.Payment.BVPaymentMethodId = BVSoftware.Commerce.Payment.Method.Cash.Id();
                        }
                    }

                    foreach (BVSoftware.Commerce.Orders.LineItem li in order.Items)
                    {
                        OrderItemData i = new OrderItemData()
                        {
                            LineTotal = li.LineTotal,
                            Quantity = li.Quantity,
                            Product = new ProductData()
                            {
                                BaseWeight = li.ProductShippingWeight,
                                UniqueId = li.ProductSku,
                                Bvin = li.ProductId,
                                Description = MerchantTribe.Web.Text.TrimToLength(li.ProductName, 59),
                                Price = li.AdjustedPricePerItem
                            }
                        };
                        data.Items.Add(i);
                    }

                    OrderData newOrder = Orders.CreateNewOrder(data, _ServiceContext);

                    if (newOrder.AcumaticaOrderNumber.Trim() != string.Empty)
                    {
                        EventLog.LogEvent("Acumatica Integration", "Created Acumatica Sales Order With ID " + newOrder.AcumaticaOrderNumber, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                        order.CustomProperties.SetProperty("bvsoftware", "acumaticaid", newOrder.AcumaticaOrderNumber);
                        order.Notes.Add(new BVSoftware.Commerce.Orders.OrderNote() { Note = "Created Acumatica Order Number: " + newOrder.AcumaticaOrderNumber});
                        bvapp.OrderServices.Orders.Update(order);                        
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        Console.WriteLine("Failed to Create Acumatica Sales Order");
                        foreach (ServiceError e in _ServiceContext.Errors)
                        {
                            sb.Append("ERROR: " + e.Description + " | ");
                        }
                        EventLog.LogEvent("Acumatica Integration", "Failed to Create Acumatica Sales Order for BV Order " + data.BVOrderNumber + " " + sb.ToString(), BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                        order.Notes.Add(new BVSoftware.Commerce.Orders.OrderNote(){ Note="Failed to send to Acumatica: " + sb.ToString()});
                        bvapp.OrderServices.Orders.Update(order);
                    }
                }
                
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            foreach (ServiceError e in _ServiceContext.Errors)
            {
                EventLog.LogEvent("Acumatica Integration", e.ErrorCode + " " + e.Description, BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
            }
        }

        private string CreateAcumaticaAccountForCustomer(BVSoftware.Commerce.Membership.CustomerAccount account,
                                                        string companyName, string phoneNumber,
                                                       MerchantTribe.Web.Geography.IAddress shippingAddress,
                                                       MerchantTribe.Web.Geography.IAddress billingAddress)
        {            
            return CreateAcumaticaAccountForCustomer(account.Email, account.FirstName, account.LastName,
                                                     companyName, phoneNumber,   
                                                    shippingAddress, billingAddress);
        }

        
        private string CreateAcumaticaAccountForCustomer(string email, string firstname, string lastname,
                                                        string companyName, string phone,
                                                       MerchantTribe.Web.Geography.IAddress shippingAddress,
                                                       MerchantTribe.Web.Geography.IAddress billingAddress)
        {
            if (!EnsureLogin())
            {
                EventLog.LogEvent("Acumatica CreateAccountForCustomer", "Logged in FAILED", BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                this._ServiceContext.Errors.Add(new ServiceError() { Description = "Acumatica Login Failed!", ErrorCode = "LOGIN" });
                return string.Empty;
            }
            else
            {
                EventLog.LogEvent("Acumatica CreateAccountForCustomer", "Logged in OK", BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
            }


            string acumaticaCustomerId = string.Empty;

            try
            {
                EventLog.LogEvent("Acumatica CreateAccountForCustomer", "Attempting " + email +"," + firstname +"," + lastname, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);


                CustomerData data = new CustomerData() { Email = email, FirstName = firstname, LastName = lastname };
                data.CompanyName = companyName;
                data.PhoneNumber = phone;
                data.ShippingAddress = shippingAddress;
                data.BillingAddress = billingAddress;
                data.Bvin = string.Empty; //account.Bvin;
                data.PaymentIdentifier = _ServiceContext.DefaultCustomerPaymentMethod;
                data.PaymentMethod = _ServiceContext.DefaultCustomerPaymentMethod;

                EventLog.LogEvent("Acumatica CreateAccountForCustomer", "Pre-GetOrCreate Call", BVSoftware.Commerce.Metrics.EventLogSeverity.Information);
                acumaticaCustomerId = Customers.GetOrCreateCustomer(data, _ServiceContext);
                EventLog.LogEvent("Acumatica CreateAccountForCustomer", "Post-GetOrCreate Call", BVSoftware.Commerce.Metrics.EventLogSeverity.Information);

                if (acumaticaCustomerId != string.Empty)
                {
                    EventLog.LogEvent("Acumatica Integration", "Create Acumatica Customer With ID " + acumaticaCustomerId, BVSoftware.Commerce.Metrics.EventLogSeverity.Information);

                    // Make sure billing/shipping is up to date with latest order
                    data.AcumaticaId = acumaticaCustomerId;
                    if (Customers.UpdateCustomerData(data, _ServiceContext))
                    {
                        //EventLog.LogEvent("Updated Customer Data");
                    }
                    else
                    {
                        EventLog.LogEvent("Acumatica Integration", "Unable to update customer data for Acumatica Account " + data.AcumaticaId, BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                    }

                }
                else
                {
                    
                    EventLog.LogEvent("Acumatica Integration", "Service Context Error Count = " + _ServiceContext.Errors.Count.ToString(), BVSoftware.Commerce.Metrics.EventLogSeverity.Information);

                    StringBuilder sb = new StringBuilder();
                    Console.WriteLine("No Customer Found with that Email");
                    foreach (ServiceError e in _ServiceContext.Errors)
                    {
                        sb.Append("ERROR: " + e.Description + " | ");
                    }
                    EventLog.LogEvent("Acumatica Integration", "Failed to Create Acumatica Customer " + sb.ToString(), BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                this._ServiceContext.Errors.Add(new ServiceError() { Description = ex.Message + " | " + ex.StackTrace, ErrorCode = "AC1000" });
                return string.Empty;
            }

            return acumaticaCustomerId;
        }

        private bool MakeSureAllItemsExist(List<BVSoftware.Commerce.Orders.LineItem> items)
        {
            bool result = true;

            foreach (BVSoftware.Commerce.Orders.LineItem li in items)
            {
                ProductData p = new ProductData()
                       {
                           BaseWeight = li.ProductShippingWeight,
                           UniqueId = li.ProductSku,
                           Bvin = li.ProductId,
                           Description = li.ProductName,
                           Price = li.AdjustedPricePerItem                           
                       };
                ProductData newP = Products.GetOrCreateProduct(p, _ServiceContext);
                if (_ServiceContext.Errors.Count > 0)
                {
                    return false;
                }
            }

            return result;
        }

        public List<OrderSummaryData> FindAllOrdersForCustomer(string email)
        {
            List<OrderSummaryData> orders = new List<OrderSummaryData>();

            try
            {
                if (!EnsureLogin()) return new List<OrderSummaryData>();
                
                string customerId = Customers.GetCustomerIdByEmail(email, _ServiceContext);
                MerchantTribe.Web.Text.TrimToLength(customerId, 10);
                if (customerId.Trim().Length > 0)
                {
                    orders = Orders.ListAllOrdersForCustomer(customerId, _ServiceContext);
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                throw new RemoteIntegrationException(ex.Message, ex.InnerException);
            }

            return orders;
        }

        public bool CustomerOwnsOrder(string email, string orderNumber)
        {
            try
            {
                if (!EnsureLogin()) return false;

                List<OrderSummaryData> orders = FindAllOrdersForCustomer(email);
                if (orders == null) return false;

                var found = orders.Where(y => y.Number == orderNumber).Count();
                if (found >= 1) return true;
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                throw new RemoteIntegrationException(ex.Message, ex);
            }

            return false;
        }

        public OrderData GetSingleOrder(string orderNumber)
        {
            try
            {
                if (!EnsureLogin()) return new OrderData();
                OrderData result = Orders.FindSalesOrderByAcumaticaId(orderNumber, _ServiceContext);
                return result;
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                throw new RemoteIntegrationException(ex.Message, ex.InnerException);                
            }            
        }

        public DateTime NextScheduledPullTime(BVApplication bvapp)
        {
            BVSoftware.Commerce.Scheduling.QueuedTask nextTask = bvapp.ScheduleServices.QueuedTasks.FindNextQueuedByProcessorId(AcumaticaIntegration.PullDataTaskProcessorId);
            if (nextTask == null) return new DateTime(1900, 1, 1);
            return nextTask.StartAtUtc;            
        }

        public BVSoftware.Commerce.Scheduling.QueuedTaskResult ProcessTaskPullData(BVApplication bvapp)
        {
            if (!EnsureLogin()) return new BVSoftware.Commerce.Scheduling.QueuedTaskResult() { Notes = "Login to Acumatica Failed", Success = false };
            
            DateTime lastPulledUtc = bvapp.CurrentStore.Settings.Acumatica.LastCustomerPullUtc;
            

            List<CustomerData> updatedCustomers = Customers.FindCustomersUpdatedAfter(lastPulledUtc.ToLocalTime(), _ServiceContext);
            foreach(CustomerData cd in updatedCustomers)
            {
                BVSoftware.Commerce.Membership.CustomerAccount customer = bvapp.MembershipServices.Customers.FindByEmail(cd.Email);
                if (customer == null || customer.Bvin == string.Empty)
                {
                    CreateCustomerFromAcumatica(cd.AcumaticaId, cd.Email, bvapp);
                }
                else
                {
                    UpdateCustomerFromAcumatica(cd.AcumaticaId, customer, bvapp);
                }
            }

            // Save last pulled time
            bvapp.CurrentStore.Settings.Acumatica.LastCustomerPullUtc = DateTime.UtcNow;
            bvapp.AccountServices.Stores.Update(bvapp.CurrentStore);

            return new BVSoftware.Commerce.Scheduling.QueuedTaskResult() { Notes = "Pulled Data From Acumatica at " + DateTime.UtcNow, Success = true };            
        }

        private void CreateCustomerFromAcumatica(string acumaticaId, string email, BVApplication bvapp)
        {
            try
            {
                if (email == string.Empty) return;

                BVSoftware.Commerce.Membership.CustomerAccount customer = new BVSoftware.Commerce.Membership.CustomerAccount();
                customer.Email = email;
                customer.FirstName = "Unknown";
                customer.LastName = "Unknown";
                string password = MerchantTribe.Web.PasswordGenerator.GeneratePassword(10);
                customer.Password = password;
                bvapp.MembershipServices.CreateCustomer(customer, password);
                UpdateCustomerFromAcumatica(acumaticaId, customer, bvapp);
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
        }
        private void UpdateCustomerFromAcumatica(string acumaticaId, BVSoftware.Commerce.Membership.CustomerAccount customer, BVApplication bvapp)
        {
            try
            {
                CustomerData acustomer = Customers.GetCustomer(acumaticaId, _ServiceContext);
                if (acustomer != null)
                {
                    if (acustomer.Email == customer.Email)
                    {
                        // update shipping addresses
                        if (acustomer.ShippingAddress.Street.Length > 0)
                        {
                            BVSoftware.Commerce.Contacts.Address shipAddress = customer.GetShippingAddress();
                            shipAddress.FirstName = (acustomer.FirstName.Trim().Length > 0) ? acustomer.FirstName : shipAddress.FirstName;
                            shipAddress.LastName = (acustomer.LastName.Trim().Length > 0) ? acustomer.LastName : shipAddress.LastName;
                            shipAddress.Phone = acustomer.PhoneNumber;
                            shipAddress.Company = acustomer.CompanyName;
                            shipAddress.Street = acustomer.ShippingAddress.Street;
                            shipAddress.Street2 = acustomer.ShippingAddress.Street2;
                            shipAddress.City = acustomer.ShippingAddress.City;
                            shipAddress.PostalCode = acustomer.ShippingAddress.PostalCode;
                            shipAddress.CountryData = TranslateCountry(acustomer.BillingAddress.CountryData);
                            shipAddress.RegionData = TranslateRegion(shipAddress.CountryData, acustomer.ShippingAddress.RegionData);
                            
                            bvapp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(customer, shipAddress);
                            customer.UpdateAddress(shipAddress);
                            bvapp.MembershipServices.CustomerMakeAddressShipping(customer, shipAddress.Bvin);
                            bvapp.MembershipServices.UpdateCustomer(customer);
                        }

                        // update billing addresses
                        if (acustomer.BillingAddress.Street.Length > 0)
                        {
                            BVSoftware.Commerce.Contacts.Address billAddress = customer.GetBillingAddress();
                            billAddress.Company = acustomer.CompanyName;
                            billAddress.Phone = acustomer.PhoneNumber;                            
                            billAddress.FirstName = (acustomer.FirstName.Trim().Length > 0) ? acustomer.FirstName : billAddress.FirstName;
                            billAddress.LastName = (acustomer.LastName.Trim().Length > 0) ? acustomer.LastName : billAddress.LastName;
                            billAddress.Street = acustomer.BillingAddress.Street;
                            billAddress.Street2 = acustomer.BillingAddress.Street2;
                            billAddress.City = acustomer.BillingAddress.City;
                            billAddress.PostalCode = acustomer.BillingAddress.PostalCode;
                            billAddress.CountryData = TranslateCountry(acustomer.BillingAddress.CountryData);
                            billAddress.RegionData = TranslateRegion(billAddress.CountryData, acustomer.BillingAddress.RegionData);

                            bvapp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(customer,billAddress);
                            customer.UpdateAddress(billAddress);
                            bvapp.MembershipServices.CustomerMakeAddressBilling(customer,billAddress.Bvin);
                            bvapp.MembershipServices.UpdateCustomer(customer);
                        }

                        if (acustomer.FirstName.Trim() != string.Empty)
                        {
                            customer.FirstName = acustomer.FirstName;
                        }
                        if (acustomer.LastName.Trim() != string.Empty)
                        {
                            customer.LastName = acustomer.LastName;
                        }
                        bvapp.MembershipServices.UpdateCustomer(customer);
                    }
                }                
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
        }

        private MerchantTribe.Web.Geography.CountrySnapShot TranslateCountry(MerchantTribe.Web.Geography.CountrySnapShot countryData)
        {
            MerchantTribe.Web.Geography.CountrySnapShot result = new MerchantTribe.Web.Geography.CountrySnapShot();

            string bvin = countryData.Bvin;
            if (bvin == string.Empty) bvin = countryData.Name;

            MerchantTribe.Web.Geography.Country c = MerchantTribe.Web.Geography.Country.FindByISOCode(bvin);
            if (c == null || c.Bvin == string.Empty)
            {
                c = MerchantTribe.Web.Geography.Country.FindByName(countryData.Name);
            }
            if (c == null) c = MerchantTribe.Web.Geography.Country.FindByBvin(MerchantTribe.Web.Geography.Country.UnitedStatesCountryBvin);
            if (c != null)
            {
                result.Bvin = c.Bvin;
                result.Name = c.DisplayName;
            }
            return result;
        }
        private MerchantTribe.Web.Geography.RegionSnapShot TranslateRegion(MerchantTribe.Web.Geography.CountrySnapShot countryData, 
                                                                        MerchantTribe.Web.Geography.RegionSnapShot regionData)
        {
            MerchantTribe.Web.Geography.RegionSnapShot result = new MerchantTribe.Web.Geography.RegionSnapShot();
            result.Abbreviation = regionData.Abbreviation;
            result.Name = regionData.Name;

            string abbreviation = regionData.Abbreviation;
            if (abbreviation == string.Empty) abbreviation = regionData.Name;
            
            MerchantTribe.Web.Geography.Country c = MerchantTribe.Web.Geography.Country.FindByBvin(countryData.Bvin);
            if (c == null || c.Bvin == string.Empty) c = MerchantTribe.Web.Geography.Country.FindByBvin(MerchantTribe.Web.Geography.Country.UnitedStatesCountryBvin);
            if (c != null)
            {
                MerchantTribe.Web.Geography.Region r = c.FindRegion(abbreviation);
                if (r != null && r.Abbreviation != string.Empty)
                {
                    result.Abbreviation = r.Abbreviation;
                    result.Name = r.Abbreviation;
                }
            }
            return result;
        }
        public BVSoftware.Commerce.Scheduling.QueuedTask GenerateQueuedTask(BVApplication bvapp)
        {
            BVSoftware.Commerce.Scheduling.QueuedTask task = new BVSoftware.Commerce.Scheduling.QueuedTask();


            int minutesBetween = bvapp.CurrentStore.Settings.Acumatica.MinutesBetweenDataPulls;
            
            // abort if set to "NEVER"
            if (minutesBetween < 0) return task;

            // if we're a positive number less than 5, set to 5 minutes between
            if (minutesBetween < 5)
            {
                minutesBetween = 5;
            }
            task.FriendlyName = "Acumatica Pull New Data";
            task.StartAtUtc = DateTime.UtcNow.AddMinutes(minutesBetween);
            task.Status = BVSoftware.Commerce.Scheduling.QueuedTaskStatus.Pending;
            task.StoreId = bvapp.CurrentStore.Id;
            task.TaskProcessorId = AcumaticaIntegration.PullDataTaskProcessorId;
            task.TaskProcessorName = "Acumatica Integration";
            bvapp.ScheduleServices.QueuedTasks.Create(task);

            return task;
        }
    }
}