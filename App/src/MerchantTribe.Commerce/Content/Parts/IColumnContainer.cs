using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Parts
{
    interface IColumnContainer
    {
        List<Column> Columns {get;}
        List<IContentPart> ListParts(int columnNumber);
        IContentPart FindPart(string partId);
        bool RemovePart(string partId);
    }
}
