using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BVCommerce
{
    public static class IntegrationLoader
    {
        private static AcumaticaIntegration acumatica = null;

        public static void AddIntegrations(BVSoftware.Commerce.Integration integration, BVSoftware.Commerce.Accounts.Store currentStore)
        {
            if (currentStore.Settings.Acumatica.IntegrationEnabled)
            {
                acumatica = new AcumaticaIntegration(currentStore.Settings.Acumatica.Username,
                                                    currentStore.Settings.Acumatica.Password,
                                                    currentStore.Settings.Acumatica.SiteUrl,
                                                    currentStore.Settings.Acumatica.NewItemTaxClassId,
                                                    currentStore.Settings.Acumatica.NewItemWarehouseId,
                                                    currentStore.Settings.Acumatica.OrderLineItemWarehouseId,
                                                    currentStore.Settings.Acumatica.PaymentCCId,
                                                    currentStore.Settings.Acumatica.CustomerIdIsString,
                                                    currentStore.Settings.Acumatica.PaymentMappings,
                                                    currentStore.Settings.Acumatica.ShippingMappings);

                integration.OnCustomerAccountCreated += acumatica.OnCustomerAccountCreated;
                integration.OnCustomerAccountDeleted += acumatica.OnCustomerAccountDeleted;
                integration.OnCustomerAccountUpdated += acumatica.OnCustomerAccountUpdated;
                integration.OnOrderReceived += acumatica.OnOrderReceived;
                integration.OnCustomerAccountEmailChanged += acumatica.OnCustomerAccountEmailChanged;
            }
        }

    }
}