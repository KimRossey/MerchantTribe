
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVCommerce
{
    public class AddToWishlistClickedEventArgs
    {
        private IVariantDisplay _variantsDisplay;
        private IMessageBox _messageBox;
        private IProductPage _page;
        private int _quantity;
        private Label _itemAddedToCartLabel;
        private decimal _productOverridePrice;
        private bool _isValid = true;

        public IVariantDisplay VariantsDisplay
        {
            get { return _variantsDisplay; }
            set { _variantsDisplay = value; }
        }

        public IMessageBox MessageBox
        {
            get { return _messageBox; }
            set { _messageBox = value; }
        }

        public IProductPage Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public Label ItemAddedToCartLabel
        {
            get { return _itemAddedToCartLabel; }
            set { _itemAddedToCartLabel = value; }
        }

        public decimal ProductOverridePrice
        {
            get { return _productOverridePrice; }
            set { _productOverridePrice = value; }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

    }
}