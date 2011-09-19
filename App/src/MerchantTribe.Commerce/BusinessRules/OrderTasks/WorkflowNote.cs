using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
    public class WorkflowNote: OrderTask
    {
        private string Note = string.Empty;

        public WorkflowNote()
        { 
        }
        public WorkflowNote(string note)
        {
            this.Note = note;
        }
        public override bool Execute(OrderTaskContext context)
        {
            Orders.OrderNote note = new Orders.OrderNote();
            note.IsPublic = false;
            note.Note = this.Note;            
            context.Order.Notes.Add(note);            
            return true;
        }

        public override bool Rollback(OrderTaskContext context)
        {
            return true;
        }

        public override string TaskName()
        {
            return "Workflow Note";
        }

        public override string TaskId()
        {
            return "EFF99A7E-5A2B-4216-8A49-13D70A78AAB2";
        }

        public override Task Clone()
        {
            return new WorkflowNote();
        }
    }
}
