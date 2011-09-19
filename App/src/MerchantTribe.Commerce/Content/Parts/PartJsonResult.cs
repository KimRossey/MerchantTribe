using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Parts
{
    public class PartJsonResult
    {
        public bool Success { get; set; }
        public bool IsFinishedEditing { get; set; }
        public string ResultHtml { get; set; }
        public string ScriptFunction { get; set; }

        public PartJsonResult()
        {
            this.Success = false;
            this.IsFinishedEditing = false;
            this.ResultHtml = string.Empty;
            this.ScriptFunction = string.Empty;
        }
    }
}
