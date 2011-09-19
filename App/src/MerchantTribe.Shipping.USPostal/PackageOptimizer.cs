using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Shipping;

namespace MerchantTribe.Shipping.USPostal
{
    class PackageOptimizer
    {
        private decimal _MaxWeight = 70m;
        public bool GirthCheck { get; set; }
        public decimal GirthMaxInInches {get;set;}
        public bool LengthPlusGirthCheck { get; set; }
        public decimal LengthPlusGirthMaxInInches {get;set;}

        public PackageOptimizer(decimal maxWeightInPounds)
        {
            _MaxWeight = maxWeightInPounds;
        }

        public List<IShippable> OptimizePackagesToMaxWeight(IShipment shipment)
        {
            List<IShippable> result = new List<IShippable>();
            List<IShippable> itemsToSplit = new List<IShippable>();

            // drop off oversize items right away
            foreach (IShippable item in shipment.Items)
            {
                if (IsOversized(item))
                {
                    result.Add(item.CloneShippable());
                }
                else
                {
                    itemsToSplit.Add(item);
                }
            }
            

            IShippable tempPackage = new Shippable();

            foreach (IShippable pak in itemsToSplit)
            {
                if (_MaxWeight - tempPackage.BoxWeight >= pak.BoxWeight)
                {
                    // add to current box
                    tempPackage.BoxWeight += pak.BoxWeight;
                    tempPackage.QuantityOfItemsInBox += pak.QuantityOfItemsInBox;
                    tempPackage.BoxValue += pak.BoxValue;
                }
                else
                {
                    // Save the temp package if it has items
                    if (tempPackage.BoxWeight > 0 || tempPackage.QuantityOfItemsInBox > 0)
                    {
                        result.Add(tempPackage.CloneShippable());
                        tempPackage = new Shippable();
                    }

                    // create new box
                    if (pak.BoxWeight > _MaxWeight)
                    {
                        //Loop to break up > maxWeight Packages
                        int currentItemsInBox = pak.QuantityOfItemsInBox;
                        decimal currentWeight = pak.BoxWeight;

                        while (currentWeight > 0)
                        {
                            if (currentWeight > _MaxWeight)
                            {
                                IShippable newP = pak.CloneShippable();
                                newP.BoxWeight = _MaxWeight;
                                if (currentItemsInBox > 0)
                                {
                                    currentItemsInBox -= 1;
                                    newP.QuantityOfItemsInBox = 1;
                                }
                                result.Add(newP);
                                currentWeight = currentWeight - _MaxWeight;
                                if (currentWeight < 0)
                                {
                                    currentWeight = 0;
                                }
                            }
                            else
                            {
                                // Create a new shippable box 
                                IShippable newP = pak.CloneShippable();
                                newP.BoxWeight = currentWeight;
                                if (currentItemsInBox > 0)
                                {
                                    newP.QuantityOfItemsInBox = currentItemsInBox;
                                }
                                result.Add(newP);
                                currentWeight = 0;
                            }
                        }
                    }
                    else
                    {
                        tempPackage = pak.CloneShippable();
                    }
                }
            }

            // Save the temp package if it has items
            if (tempPackage.BoxWeight > 0 || tempPackage.QuantityOfItemsInBox > 0)
            {
                result.Add(tempPackage.CloneShippable());
                tempPackage = new Shippable();
            }
            
            return result;
        }

        private bool IsOversized(IShippable prod)
        {
            decimal girth = (decimal)(prod.BoxLength + (2m * prod.BoxHeight) + (2m * prod.BoxWidth));
            if (GirthCheck)
            {
                if (girth > GirthMaxInInches) return true;
            }
            if (LengthPlusGirthCheck)
            {
                if ((girth + prod.BoxLength) > LengthPlusGirthMaxInInches) return true;
            }            
            return false;
        }
    }
}
