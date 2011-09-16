﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Content.Parts
{
    public interface IColumn: IContentPart
    {
        ColumnSize Size { get; set; }
        bool NoGutter { get; set; }
        List<IContentPart> Parts {get;}
                
        bool AddPart(IContentPart part);        
        bool RemovePart(string partId);
                                        
    }
}
