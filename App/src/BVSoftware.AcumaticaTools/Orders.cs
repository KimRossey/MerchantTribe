using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class Orders
    {
        public static OrderData CreateNewOrder(OrderData input, ServiceContext context)
        {
            context.Errors.Clear();
            string orderNumber = string.Empty;

            SO301000Content schema = context.SO301000_Schema;
            //try
            //{

                //adjusting data                
                BVSoftware.Payment.CardType cardType = BVSoftware.Payment.CardValidator.GetCardTypeFromNumber(input.Payment.Card.CardNumber);
                string acumaticaPaymentMethodId = TranslateBVPaymentIdToAcumaticaId(context.PaymentMappings, input.Payment.BVPaymentMethodId, ((int)cardType).ToString());

                // Set CVV to correct length
                string cvv = "000";
                if (cardType == BVSoftware.Payment.CardType.Amex) cvv = "0000";

                // Set Shipping Country Code
                string shipcountry = MerchantTribe.Web.Geography.Country.FindByBvin(input.ShippingAddress.CountryData.Bvin).IsoCode;
                if (shipcountry == string.Empty) shipcountry = "US";


                //I suggest to add orde by one request. It is faster.
                List<Command> commands = new List<Command>();

                //Oreder main info
                commands.AddRange(new Command[]
				{
					new Value {Value ="SO", LinkedCommand = schema.OrderSummary.OrderType },
					new Value {Value = "='" + input.AcumaticaOrderNumber + "'", LinkedCommand = schema.OrderSummary.OrderNbr },

					new Value {Value = input.Customer.AcumaticaId, LinkedCommand = schema.OrderSummary.Customer},
					new Value {Value = input.BVOrderNumber, LinkedCommand = schema.OrderSummary.CustomerOrder}                    
				});


                if (input.Payment.Card.CardNumber.Length > 0 && input.Payment.Action == Payment.ActionType.CreditCardHold)
                {
                    //credit card transaction info
                    schema.PaymentSettingsCardInfoDescription.Value.LinkedCommand = null;
                    commands.AddRange(new Command[]
                    {
                        new Value { Value = input.Payment.Card.CardNumber, LinkedCommand = schema.PaymentSettings.CCNumber },
                        new Value { Value = acumaticaPaymentMethodId, LinkedCommand = schema.PaymentSettingsCardInfo.CardType},  
         			
					    //new Key { Value = "='Card Number'", Commit = true,  ObjectName = schema.PaymentSettingsCardInfoDescription.Description.ObjectName, FieldName = schema.PaymentSettingsCardInfoDescription.Description.FieldName },
                        //new Value {Value = input.Payment.Card.CardNumber,  Commit = true, LinkedCommand = schema.PaymentSettingsCardInfoDescription.Value},

					    new Key { Value = "='Expiration Date'", ObjectName = schema.PaymentSettingsCardInfoDescription.Description.ObjectName, FieldName = schema.PaymentSettingsCardInfoDescription.Description.FieldName },
                        //new Value {Value = input.Payment.Card.ExpirationMonthPadded /*+ "/01/"*/ + input.Payment.Card.ExpirationYear.ToString(),  Commit = true, LinkedCommand = schema.PaymentSettingsCardInfoDescription.Value},
					    new Value {Value = "12/2010",  Commit = true, LinkedCommand = schema.PaymentSettingsCardInfoDescription.Value},

                        new Key { Value = "='Name on the Card'", ObjectName = schema.PaymentSettingsCardInfoDescription.Description.ObjectName, FieldName = schema.PaymentSettingsCardInfoDescription.Description.FieldName },
					    new Value {Value = input.Payment.Card.CardHolderName,  Commit = true, LinkedCommand = schema.PaymentSettingsCardInfoDescription.Value},

					    new Key { Value = "='Card Verification Code'", ObjectName = schema.PaymentSettingsCardInfoDescription.Description.ObjectName, FieldName = schema.PaymentSettingsCardInfoDescription.Description.FieldName },
                        new Value {Value = cvv,  Commit = true, LinkedCommand = schema.PaymentSettingsCardInfoDescription.Value},
                    
                        // Tranasaction Data
                        new Value {Value = input.Payment.TransactionNumber , LinkedCommand = schema.PaymentSettings.PreAuthNbr},                    
                        new Value {Value = input.Payment.Amount.ToString(), LinkedCommand = schema.PaymentSettings.PreAuthorizedAmount}, 
                        //new Value {Value = "<DATE>", LinkedCommand = schema.PaymentSettings.AuthExpiresOn}, 
                    });
                }
                else
                {

                    string message = "";

                    if (input.Payment.TransactionNumber != string.Empty)
                    {
                        message = "Charge already handled in BV, transaction # " + input.Payment.TransactionNumber + " for amount " + input.Payment.Amount.ToString("c");
                    }
                    else
                    {
                        message = "No Payment Authorization Exists in BV yet";
                    }

                    message = MerchantTribe.Web.Text.TrimToLength(message, 60);

                    commands.AddRange(new Command[]
                    {
                        new Value {Value = message, LinkedCommand = schema.OrderSummary.Description}
                    });
                }


                //Override Shipping Address
                commands.AddRange(new Command[]
                {
                    new Value {Value = "True", LinkedCommand = schema.ShippingSettingsOverrideAddress.OverrideAddress},          
                   
					new Value {Value = input.ShippingAddress.Street, LinkedCommand = schema.ShippingSettingsOverrideAddress.AddressLine1},
                    new Value {Value = input.ShippingAddress.Street2, LinkedCommand = schema.ShippingSettingsOverrideAddress.AddressLine2},
                    new Value {Value = input.ShippingAddress.City, LinkedCommand = schema.ShippingSettingsOverrideAddress.City},
                    new Value {Value = shipcountry, LinkedCommand = schema.ShippingSettingsOverrideAddress.Country},
                    new Value {Value = input.ShippingAddress.PostalCode, LinkedCommand = schema.ShippingSettingsOverrideAddress.PostalCode},
                    new Value {Value = input.ShippingAddress.RegionData.Abbreviation, LinkedCommand = schema.ShippingSettingsOverrideAddress.State},
                });

                //Shipping Charges
                commands.AddRange(new Command[]
                {
                    new Value {Value= input.Shipping.ShippingTotal.ToString(), LinkedCommand = schema.Totals.PremiumFreight},
                    
                    // Shipping Method Description
                    //new Value {Value= input.Shipping.ShippingMethodDescription, LinkedCommand = schema. ???},
                    // BV Shipping Method ID 
                    //new Value {Value= input.Shipping.ShippingMethodId, LinkedCommand = schema. ???},
                });

                //Transactions
                foreach (OrderItemData item in input.Items)
                {
                    commands.AddRange(new Command[]
					{
						new Value {Value = MerchantTribe.Web.Text.TrimToLength(item.Product.UniqueId, 59), LinkedCommand = schema.DocumentDetails.InventoryID},
						new Value {Value = MerchantTribe.Web.Text.TrimToLength(item.Product.Description, 59), LinkedCommand = schema.DocumentDetails.LineDescription },
						new Value {Value = item.Quantity.ToString(), LinkedCommand = schema.DocumentDetails.Quantity },
						new Value {Value = item.Product.Price.ToString(), LinkedCommand = schema.DocumentDetails.UnitPrice },
						new Value {Value = item.LineTotal.ToString(), LinkedCommand = schema.DocumentDetails.ExtPrice, Commit = true},
                        new Value {Value = context.NewLineItemWarehouseId, LinkedCommand = schema.DocumentDetails.Warehouse}
					});
                }

                //Saving
                commands.AddRange(new Command[]
				{
					//new Value {Value ="False", LinkedCommand = schema.OrderSummary.Hold },
					schema.Actions.Save,
					schema.OrderSummary.OrderType,
					schema.OrderSummary.OrderNbr,
					schema.OrderSummary.Hold,
				});

                //Processing all commands
                context.Gate.SO301000Clear();
                SO301000Content[] content = context.Gate.SO301000Submit(commands.ToArray());

                orderNumber = content[0].OrderSummary.OrderNbr.Value;
            //}
            //catch (Exception ex)
            //{
            //    context.Errors.Add(new ServiceError() { Description = ex.Message + " " + ex.StackTrace });
            //}

            OrderData result = FindSalesOrderByAcumaticaId(orderNumber, context);
            return result;
        }

        private static string TranslateBVPaymentIdToAcumaticaId(List<IntegrationMapping> mappings, string bvmethodId, string bvcardtype)
        {
            if (mappings == null) return string.Empty;
            foreach (IntegrationMapping map in mappings)
            {
                if (map.BVId == bvmethodId)
                {
                    if (map.BVExtra == bvcardtype)
                    {
                        return map.AcumaticaId;
                    }
                }
            }

            return string.Empty;
        }

        public static OrderData FindSalesOrderByAcumaticaId(string acumaticaId, ServiceContext context)
        {
            OrderData result = new OrderData();

            SO301000Content schema = context.SO301000_Schema;

            //try
            //{

                context.Gate.SO301000Clear();
                SO301000Content[] content = context.Gate.SO301000Submit(new Command[]
			{
                   
				new Key() { Value = "='SO'",
					FieldName = schema.OrderSummary.OrderType.FieldName,
					ObjectName = schema.OrderSummary.OrderType.ObjectName,
				},
				new Key() { Value = "='" + acumaticaId + "'", 
					FieldName = schema.OrderSummary.OrderNbr.FieldName,
					ObjectName = schema.OrderSummary.OrderNbr.ObjectName,
				},

                // Summary 
				schema.OrderSummary.OrderType,
				schema.OrderSummary.OrderNbr,
				schema.OrderSummary.Currency,
				schema.OrderSummary.Date,
				schema.OrderSummary.Customer,
				schema.OrderSummary.CustomerOrder,
                schema.OrderSummary.TaxTotal,
				schema.OrderSummary.OrderTotal,
                schema.OrderSummary.NoteText,
                schema.OrderSummary.Status,
                schema.OrderSummary.RequestedOn,

                // Shipping Costs
                schema.Totals.Freight,	
                schema.Totals.PremiumFreight,                                				

                
                schema.DocumentDetails.InventoryID,
                schema.DocumentDetails.Quantity,
                schema.DocumentDetails.QtyOnShipments,    
                schema.DocumentDetails.UnitPrice,
                schema.DocumentDetails.Discount,
                schema.DocumentDetails.DiscountAmount,
                schema.DocumentDetails.DiscUnitPrice,
                schema.DocumentDetails.ExtPrice,
			    schema.DocumentDetails.LineDescription,
                schema.DocumentDetails.ShipComplete,
                schema.DocumentDetails.ShipOn,
                schema.DocumentDetails.Canceled,
                

                // billing address
                schema.FinancialSettingsOverrideAddress.AddressLine1,
                schema.FinancialSettingsOverrideAddress.AddressLine2,
                schema.FinancialSettingsOverrideAddress.City,
                schema.FinancialSettingsOverrideAddress.Country,
                schema.FinancialSettingsOverrideAddress.PostalCode,
                schema.FinancialSettingsOverrideAddress.State,

                // Payment Method
                schema.PaymentSettings.PaymentMethod,
                schema.PaymentSettings.PaymentMethodIdentifier,

                // Shipping Address
                schema.ShippingSettingsOverrideAddress.AddressLine1,
                schema.ShippingSettingsOverrideAddress.AddressLine2,
                schema.ShippingSettingsOverrideAddress.City,
                schema.ShippingSettingsOverrideAddress.Country,
                schema.ShippingSettingsOverrideAddress.PostalCode,
                schema.ShippingSettingsOverrideAddress.State,

                schema.Shipments.ShipmentNbr,
                //schema.Shipments.ShipmentType,
                //schema.Shipments.Status,
                //schema.Shipments.ShippedQty,
                //schema.CarrierRatesPackages.BoxID,
                //schema.CarrierRatesPackages.LineNbr,                 
                //schema.SpecifyShipmentParameters.ShipmentDate               
                                                                           
			});

                //bool GetShippingFromCustomer = false;
                //bool GetBillingFromCustomer = false;

                if (content.Length > 0)
                {
                    //if (content[0].ShippingSettingsOverrideAddress == null) GetShippingFromCustomer = true;
                    //if (content[0].FinancialSettingsOverrideAddress == null) GetBillingFromCustomer = true;

                    result.LoadFromErp(content[0]);

                    foreach (SO301000Content li in content)
                    {
                        OrderItemData line = new OrderItemData();
                        line.Product.Description = li.DocumentDetails.LineDescription.Value ?? string.Empty;
                        line.Product.UniqueId = li.DocumentDetails.InventoryID.Value ?? string.Empty;

                        decimal tempQuantity = 0;
                        if (decimal.TryParse(li.DocumentDetails.Quantity.Value ?? "1", out tempQuantity))
                        {
                            line.Quantity = tempQuantity;
                        }

                        decimal tempPrice = 0;
                        if (decimal.TryParse(li.DocumentDetails.ExtPrice.Value ?? "0", out tempPrice))
                        {
                            line.Product.Price = tempPrice;
                        }

                        result.Items.Add(line);
                    }

                    result.Shipments.Clear();
                    foreach (SO301000Content shipment in content)
                    {
                        try
                        {
                            string shipNumber = shipment.Shipments.ShipmentNbr.Value ?? string.Empty;
                            OrderShipmentData s = GetShipmentData(shipNumber, context);
                            result.Shipments.Add(s);
                        }
                        catch
                        {
                            // yes, we're surpressing an error and that's not usually a good idea.
                        }
                    }
                }

            //}
            //catch (Exception ex)
            //{
            //    result.AcumaticaOrderNumber = "ERROR";
            //    context.Errors.Add(new ServiceError() { Description = "Exception getting order:" + ex.Message + "|" + ex.StackTrace, ErrorCode = "ERROR" });                
            //}

            return result;
        }

        private static OrderShipmentData GetShipmentData(string number, ServiceContext context)
        {
            OrderShipmentData result = new OrderShipmentData();

            SO302000Content schema = context.SO302000_Schema;

            context.Gate.SO302000Clear();
            SO302000Content[] content = context.Gate.SO302000Submit(new Command[]
			{
                   
				new Key() { Value = "='" + number + "'",
					FieldName = schema.ShipmentSummary.ShipmentNbr.FieldName,
					ObjectName = schema.ShipmentSummary.ShipmentNbr.ObjectName,
				},

                schema.ShipmentSummary.ShipmentNbr,
                schema.ShipmentSummary.Status,
                schema.ShipmentSummary.ShipmentDate,
                schema.ShipmentSummary.ShippedQuantity,
                schema.DocumentDetails.Description,
                schema.DocumentDetails.InventoryID,
                schema.DocumentDetails.LineNbr,
                schema.DocumentDetails.ShippedQty,
                schema.Packages.Confirmed,
                schema.Packages.NoteText,
                schema.Packages.TrackingNumber,
                schema.Packages.Weight,                                    			                                                                           
			});

            if (content == null) return null;
            if (content.Length < 1) return null;

            result.ShipmentNumber = content[0].ShipmentSummary.ShipmentNbr.Value ?? string.Empty;
            result.StatusCode = content[0].ShipmentSummary.Status.Value ?? string.Empty;
            if (content[0].ShipmentSummary.ShipmentDate != null)
            {
                DateTime tempShip = DateTime.Now;
                if (DateTime.TryParse(content[0].ShipmentSummary.ShipmentDate.Value, out tempShip))
                {
                    result.ShipDate = tempShip;
                }
            }

            foreach (SO302000Content shipment in content)
            {
                if (shipment.Packages.TrackingNumber.Value != null)
                {
                    result.TrackingNumber.Add(shipment.Packages.TrackingNumber.Value);
                }

                if (shipment.DocumentDetails.InventoryID.Value != null)
                {
                    OrderShipmentItem item = new OrderShipmentItem();
                    item.ItemId = shipment.DocumentDetails.InventoryID.Value;
                    item.Description = shipment.DocumentDetails.Description.Value ?? string.Empty;
                    decimal tempQty = 1;
                    if (decimal.TryParse(shipment.DocumentDetails.ShippedQty.Value, out tempQty))
                    {
                        item.Quantity = tempQty;
                    }
                    result.Items.Add(item);
                }
            }

            return result;
        }

        public static List<AccountDescriptor> ListAllPaymentMethods(ServiceContext context)
        {
            List<AccountDescriptor> methods = new List<AccountDescriptor>();

            CA204000Content schema = context.CA204000_Schema;

            context.Gate.CS207500Clear();
            CA204000Content[] result = context.Gate.CA204000Submit(new Command[]
			{
				schema.PaymentMethod.ServiceCommands.EveryPaymentMethodID,
				schema.PaymentMethod.PaymentMethodID,
				schema.PaymentMethod.Description,
			});

            foreach (CA204000Content carrier in result)
            {
                methods.Add(new AccountDescriptor() { Id = carrier.PaymentMethod.PaymentMethodID.Value, Description = carrier.PaymentMethod.Description.Value });
            }

            return methods;
        }

        public static List<AccountDescriptor> ListAllShippingMethods(ServiceContext context)
        {
            List<AccountDescriptor> methods = new List<AccountDescriptor>();

            CS207500Content schema = context.CS207500_Schema;

            context.Gate.CS207500Clear();
            CS207500Content[] result = context.Gate.CS207500Submit(new Command[]
            {            
                schema.CarrierSummary.ServiceCommands.EveryCarrierID,
                schema.CarrierSummary.CarrierID,
                schema.CarrierSummary.Description				
            });

            foreach (CS207500Content carrier in result)
            {
                methods.Add(new AccountDescriptor() { Id = carrier.CarrierSummary.CarrierID.Value, Description = carrier.CarrierSummary.Description.Value });
            }

            return methods;
        }

        public static List<OrderSummaryData> ListAllOrdersForCustomer(string customerId, ServiceContext context)
        {
            SO301000Content schema = context.SO301000_Schema;

            string[][] OrderIDs = context.Gate.SO301000Export(
                    new Command[]
                    {
                        schema.OrderSummary.ServiceCommands.EveryOrderNbr,
                        schema.OrderSummary.OrderNbr,
                        schema.OrderSummary.Date,
                        schema.OrderSummary.OrderTotal,
                        schema.OrderSummary.Status
                    },
                    new Filter[]
                    {
                        new Filter {
                            Field = schema.OrderSummary.Customer,
                            Condition = FilterCondition.Equals,
                            Value = customerId
                    }
                },
                0,
                false,
                false
                );

            List<OrderSummaryData> result = new List<OrderSummaryData>();

            if (OrderIDs != null)
            {
                if (OrderIDs.GetLength(0) > 0)
                {
                    for (int i = 0; i < OrderIDs.GetLength(0); i++)
                    {
                        try
                        {
                            OrderSummaryData d = new OrderSummaryData();

                            d.CustomerId = customerId;
                            d.Number = OrderIDs[i][0];
                            DateTime tofo = new DateTime();
                            if (DateTime.TryParse(OrderIDs[i][1], out tofo))
                            {
                                d.TimeOfOrder = tofo;
                            }
                            decimal temp = 0;
                            if (decimal.TryParse(OrderIDs[i][2], out temp))
                            {
                                d.Amount = temp;
                            }
                            d.Status = OrderIDs[i][3];

                            result.Add(d);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }

            return result;
        }
    }
}
