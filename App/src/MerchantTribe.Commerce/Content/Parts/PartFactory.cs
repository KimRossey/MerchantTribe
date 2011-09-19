using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Parts
{
    class PartFactory
    {

        public static IContentPart Instantiate(string id, string typecode, IColumn parentColumn)
        {
            switch (typecode.Trim().ToLowerInvariant())
            {
                case "columncontainer":
                    ColumnContainer container = new ColumnContainer(parentColumn);
                    container.Id = id;
                    return container;                    
                case "column":
                    Column col = new Column();
                    col.Id = id;                    
                    return col;                    
                case "htmlpart":
                    Html htmlpart = new Html();
                    htmlpart.Id = id;
                    return htmlpart;
                case "image":
                    Image img = new Image();
                    return img;
            }

            return null;
        }
    }
}
