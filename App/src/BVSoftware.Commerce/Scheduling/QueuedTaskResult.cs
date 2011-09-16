using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Scheduling
{
    public class QueuedTaskResult
    {
        public bool Success { get; set; }
        public string Notes { get; set; }

        public QueuedTaskResult()
        {
            Success = false;
            Notes = string.Empty;
        }
    }
}
