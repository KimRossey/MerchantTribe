using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Scheduling
{
    public class QueuedTask
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string FriendlyName { get; set; }
        public string TaskProcessorName { get; set; }
        public Guid TaskProcessorId { get; set; }
        public string Payload { get; set; }
        public QueuedTaskStatus Status { get; set; }
        public string StatusNotes { get; set; }
        public DateTime StartAtUtc { get; set; }

        public QueuedTask()
        {
            Id = 0;
            StoreId = 0;
            FriendlyName = string.Empty;
            TaskProcessorName = string.Empty;
            TaskProcessorId = new Guid();
            Payload = string.Empty;
            Status = QueuedTaskStatus.Pending;
            StatusNotes = string.Empty;
            StartAtUtc = DateTime.UtcNow;            
        }
    }
}
