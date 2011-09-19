using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Shipping
{
	public class DefaultDimensionCalculator : DimensionCalculator
	{

		public override void GenerateDimensions(Shipping.ShippingGroup @group)
		{
			if (@group.Items != null) {

				decimal longestDimension = 0;
				decimal totalVolume = 0;

				@group.Length = 0;
				@group.Weight = 0;
				@group.Height = 0;
				@group.Width = 0;

				bool dimensionsSet = false;
				if (@group.Items.Count == 1) {
					if (@group.Items[0].Quantity == 1) {
						@group.Length = @group.Items[0].ProductShippingLength;
                        @group.Height = @group.Items[0].ProductShippingHeight;
                        @group.Width = @group.Items[0].ProductShippingWidth;
						@group.Weight = @group.Items[0].GetTotalWeight();
						dimensionsSet = true;
					}
				}

				if (!dimensionsSet) {
					for (int i = 0; i <= @group.Items.Count - 1; i++) {





                        if (@group.Items[i].ProductShippingLength > longestDimension)
                            {
                                longestDimension = @group.Items[i].ProductShippingLength;
                            }
                        if (@group.Items[i].ProductShippingWidth > longestDimension)
                            {
                                longestDimension = @group.Items[i].ProductShippingWidth;
                            }
                        if ((@group.Items[i].ProductShippingHeight * (@group.Items[i].Quantity - @group.Items[i].QuantityShipped)) > longestDimension)
                            {
                                longestDimension = (@group.Items[i].ProductShippingHeight * (@group.Items[i].Quantity - @group.Items[i].QuantityShipped));
                            }

                        totalVolume += (@group.Items[i].Quantity - @group.Items[i].QuantityShipped) 
                                    * (@group.Items[i].ProductShippingLength 
                                        * @group.Items[i].ProductShippingWidth 
                                        * @group.Items[i].ProductShippingHeight);

                            @group.Weight += @group.Items[i].GetTotalWeight();
                        

					}

					//Estimate Package Size based on Volume
					@group.Length = longestDimension;

					if ((longestDimension > 0) & (totalVolume > 0)) {                                                
						@group.Width = (decimal)Math.Sqrt((double)(totalVolume/ longestDimension));
					}

					@group.Height = @group.Width;
				}

				if (@group.Width < 1) {
					@group.Width = 1;
				}
				if (@group.Height < 1) {
					@group.Height = 1;
				}
				if (@group.Length < 1) {
					@group.Length = 1;
				}

			}
		}

	}
}
