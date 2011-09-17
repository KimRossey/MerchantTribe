using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class TestGatewaySettings: MethodSettings
    {
        public bool ResponseForHold
        {
            get { return GetBoolSetting("ResponseForHold"); }
            set { SetBoolSetting("ResponseForHold", value); }
        }
        public bool ResponseForCapture
        {
            get { return GetBoolSetting("ResponseForCapture"); }
            set { SetBoolSetting("ResponseForCapture", value); }
        }
        public bool ResponseForCharge
        {
            get { return GetBoolSetting("ResponseForCharge"); }
            set { SetBoolSetting("ResponseForCharge", value); }
        }
        public bool ResponseForRefund
        {
            get { return GetBoolSetting("ResponseForRefund"); }
            set { SetBoolSetting("ResponseForRefund", value); }
        }
        public bool ResponseForVoid
        {
            get { return GetBoolSetting("ResponseForVoid"); }
            set { SetBoolSetting("ResponseForVoid", value); }
        }

    }
}
