using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public class BVUserControl: System.Web.UI.UserControl
    {
        public IMultiStorePage MyPage
        {
            get
            {
                return (IMultiStorePage)this.Page;
            }
        }
    }
}
