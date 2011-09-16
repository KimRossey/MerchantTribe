using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Shipping
{
    public interface IServiceCode
    {
        string Code { get; set; }
        string DisplayName { get; set; }
    }
}
