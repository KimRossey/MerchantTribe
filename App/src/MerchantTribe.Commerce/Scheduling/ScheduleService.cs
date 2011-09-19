using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Scheduling
{
    public class ScheduleService
    {
        private RequestContext context = null;

        public QueuedTaskRepository QueuedTasks { get; private set; }

        public static ScheduleService InstantiateForMemory(RequestContext c)
        {
            return new ScheduleService(c,
                                      QueuedTaskRepository.InstantiateForMemory(c)
                                      );

        }
        public static ScheduleService InstantiateForDatabase(RequestContext c)
        {
            return new ScheduleService(c,
                                    QueuedTaskRepository.InstantiateForDatabase(c)
                                    );
        }

        public ScheduleService(RequestContext c, 
                            QueuedTaskRepository queuedTasks)
        {
            context = c;
            this.QueuedTasks = queuedTasks;
        }

        public void RemoveAllTasksForProcessor(long storeId, Guid processorId)
        {
            List<QueuedTask> tasks = QueuedTasks.FindAllPaged(1, 1000);
            if (tasks == null) return;
            if (tasks.Count < 1) return;
            var toDelete = tasks.Where(y => y.TaskProcessorId == processorId).Where(y => y.Status == QueuedTaskStatus.Pending);
            foreach (QueuedTask t in toDelete)
            {
                QueuedTasks.Delete(t.Id);
            }
        }
    }
}
