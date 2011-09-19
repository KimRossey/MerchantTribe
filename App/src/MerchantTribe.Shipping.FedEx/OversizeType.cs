
namespace MerchantTribe.Shipping.FedEx
{

    public enum OversizeType
    {
        NA = 0,
        //package exceeds 84 inches and is equal to or less than 108 inches in length 
        //and girth combined and weighs less than 30 pounds
        OS1 = 1,
        //package exceeds 108 inches in length and 
        //girth combined and weighs less than 70 pounds
        OS2 = 2,
        OS3 = 3
    }
}

