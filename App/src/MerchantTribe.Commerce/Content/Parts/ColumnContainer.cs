using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using MerchantTribe.Web;

namespace MerchantTribe.Commerce.Content.Parts
{
    public class ColumnContainer : IContentPart, IColumnContainer
    {

        private IColumn _Parent = null;
        private ColumnSize _ParentSize
        {
            get { return _Parent.Size; }
        }

        public bool SpacerAbove { get; set; }

        List<Column> _Columns = new List<Column>();
        public List<Column> Columns
        {
            get
            {
                return _Columns;
            }
        }

        public ColumnContainer(IColumn parentColumn)
        {
            Id = System.Guid.NewGuid().ToString();
            if (parentColumn.Size < ColumnSize.Size2) throw new ArgumentOutOfRangeException("Column Containers must be sizes 2 through 12");
            _Parent = parentColumn;
            this.SpacerAbove = true;
            InitColumns();            
        }

        public void SetContainer(IColumn container)
        {
            this._Parent = container;
        }

        private void InitColumns()
        {
            _Columns.Add(new Column(){ Size=ColumnSize.Size1, NoGutter = false, Id = System.Guid.NewGuid().ToString()});
            _Columns.Add(new Column() { Size = ColumnSize.Size1, NoGutter = false, Id = System.Guid.NewGuid().ToString() });
            ResizeColumns((int)_ParentSize);
        }

        private Column FindColumn(int columnIndex)
        {
            if (columnIndex < 0) return null;
            if (columnIndex >= _Columns.Count) return null;

            return _Columns[columnIndex];
        }

        public ColumnSize MinimumSize()
        {
            int result = 0;
            foreach (Column c in this._Columns)
            {
                result += (int)c.MinimumSize();
            }

            if (result < 2)
            {
                result = 2;
            }

            return (ColumnSize)result;
        }

        public List<IContentPart> ListParts(int columnIndex)
        {
            Column col = FindColumn(columnIndex);
            if (col != null)
            {
                return col.Parts;
            }
            return new List<IContentPart>();
        }

        public bool AddColumns(int howMany)
        {
            if (!CanAddColumns(howMany)) return false;

            bool Success = true;

            for (int i = 1; i <= howMany; i++)
            {
                Success = AddColumnAndResize();
                if (!Success) return false;
            }
            return Success;
        }
        public bool CanAddColumns(int howMany)
        {
            if (howMany < 1 || howMany > 12)
            {
                return false;
            }

            // We can't add more than parent size 
            if ((_Columns.Count + howMany) > (int)_ParentSize)
            {
                return false;
            }

            return true;
        }
        public bool CanRemoveColumns(int howMany)
        {
            // Can't remove more than 10 columns, we need to keep 2 as a minimum
            // Must remove at least one
            if (howMany < 1 || howMany > 10)
            {
                return false;
            }

            if ((howMany + 2) > _Columns.Count)
            {
                return false;
            }

            return true;
        }
        public bool RemoveColumns(int howMany)
        {
            // Make sure we can remove columns
            if (!CanRemoveColumns(howMany)) return false;

            for (int i = 0; i < howMany; i++)
            {
                if (!RemoveColumn()) return false;
            }

            return true;
        }

        private int minColumns { get { return 2; } }
        private int maxColumns { get { return (int)_ParentSize; } }
        private int maxIndividualColumnSize
        {
            get
            {
                int result = 1;
                int otherColumns = (this.Columns.Count - 1);
                result = ((int)_ParentSize - otherColumns);
                return result;
            }
        }
        public bool SetNumberOfColumns(int howMany)
        {
            if (howMany == _Columns.Count) return true;

            if (howMany > maxColumns || howMany < minColumns) return false;

            if (howMany > _Columns.Count)
            {
                // Adding Columns
                return AddColumns(howMany - _Columns.Count);
            }
            else
            {
                // Removing Columns
                return RemoveColumns(_Columns.Count - howMany);
            }
        }

        // See SetColumnSizes for Shorthand Explaination
        public bool SetColumns(string columnShorthand)
        {
            char[] splitter = { ',' };
            string[] splitData = columnShorthand.Split(splitter);
            if (!SetNumberOfColumns(splitData.Length)) return false;
            return SetColumnSizes(columnShorthand);
        }

        // Set column sizes using a short-hand notation
        //
        //  "1,2w,1" = ColumnSize.Size1 + ColumnSize.Size2 (no gutter) + ColumnSize.Size1
        //
        // Data should be a list of column sizes as integers
        // separated by commas
        // An optional 'w' character at the end signifies no gutter mode = true
        // default is to set no gutter to false        
        // 
        public bool SetColumnSizes(string sizeData)
        {
            char[] splitter = {','};
            string[] splitData = sizeData.Split(splitter);
            
            if (splitData.Length != _Columns.Count) return false;

            // Parsing
            List<ResizeColumnData> data = new List<ResizeColumnData>();
            for (int i = 0; i < _Columns.Count; i++)
            {
                ResizeColumnData d = new ResizeColumnData();
                if (splitData[i].Contains('w'))
                {
                    d.NoGutter = true;
                }
                char[] remover = {'w'};
                string readyToParse = splitData[i].TrimEnd(remover);
                int s = 1;
                if (int.TryParse(readyToParse,out s))
                {
                    d.Size = s;
                }
                data.Add(d);
            }

            int sumOfRequestedSizes = 0;
            foreach (ResizeColumnData d in data)
            {
                sumOfRequestedSizes += d.Size;
            }

            if (sumOfRequestedSizes > (int)_ParentSize) return false;


            for (int i = 0; i < _Columns.Count; i++)
            {
                _Columns[i].Size = (ColumnSize)data[i].Size;
                _Columns[i].NoGutter = data[i].NoGutter;
            }

            return true;

        }
        private class ResizeColumnData
        {
            public int Size { get; set; }
            public bool NoGutter { get; set; }
        }


        private bool AddColumnAndResize()
        {
            int TotalSize = (int)_ParentSize;
            int NewColumnNumber = _Columns.Count;
            _Columns.Add(new Column() { NoGutter = false, Size = ColumnSize.Size1 });
            ResizeColumns(TotalSize);
            return true;
        }
        public bool RemoveColumn()
        {
            if (_Columns.Count < 3) return false;
            
            // Move Parts from last column to next to last so
            // they aren't destroyed
            foreach (IContentPart p in _Columns[_Columns.Count - 1].Parts)
            {
                _Columns[_Columns.Count - 2].AddPart(p);
            }

            // resize second to last and remove last column
            int nextToLastSize = (int)_Columns[_Columns.Count - 2].Size;
            int lastSize = (int)_Columns[_Columns.Count - 1].Size;
            _Columns[_Columns.Count - 2].Size = (ColumnSize)(nextToLastSize + lastSize);
            _Columns.RemoveAt(_Columns.Count - 1);

            return true;            
        }

        private void ResizeColumns(int totalSize)
        {
            // Determine and even column size
            int colSize = (int)Math.Floor((double)totalSize / (double)_Columns.Count);
            int remainder = (int)((double)totalSize % (double)_Columns.Count);

            // Adjust column size
            for (int i = 0; i < _Columns.Count; i++)
            {
                _Columns[i].Size = (ColumnSize)colSize;
            }

            // If we have a remaind, add it to the first column
            if (remainder > 0)
            {
                _Columns[0].Size = (ColumnSize)((int)_Columns[0].Size + 1);
            }
        }
        public bool ResizeColumn(int columnIndex, ColumnSize size)
        {
            // Can't set size when we only have one column, It's always size 12
            if (_Columns.Count == 1)
            {
                return false;
            }

            // We have to reserve a size of 1 for each existing column other than the one
            // we're attempting to resize. This keeps the columns filling up parent size
            int maxSize = (int)_ParentSize - (_Columns.Count - 1);
            if ((int)size > maxSize) return false;


            // Find the Column
            Column col = FindColumn(columnIndex);
            if (col == null) return false;

            // Save old size for rollback
            ColumnSize oldSize = col.Size;            

            // Resize
            col.Size = size;

            // Adjust the others so that we're always valid (column sizes = parent size)
            bool WasAdjusted = AdjustColumnsToFitParentSize();

            // If adjustment failed, rollback
            if (WasAdjusted == false)
            {
                col.Size = oldSize;
                return false;
            }

            return true;
        }
       
        private bool AdjustColumnsToFitParentSize()
        {
            int actualSize = SumOfSetColumnSizes();

            // We're in balance already, no need to adjust
            if (actualSize == (int)_ParentSize) return true;

            if (actualSize > (int)_ParentSize)
            {
                return ReduceColumnsToFit(actualSize, (int)_ParentSize);
            }
            else
            {
                return StretchColumnsToFit(actualSize, (int)_ParentSize);
            }
        }

        /// <summary>
        /// Walks through columns in reverse order and shrinks as needed to accomodate column resize
        /// </summary>
        /// <param name="actualSize"></param>
        /// <param name="targetSize"></param>
        /// <param name="stickyColumnIndex"></param>
        /// <returns></returns>
        private bool ReduceColumnsToFit(int actualSize, int targetSize)
        {
            int adjustment = actualSize - targetSize;

            for (int i = _Columns.Count-1; i > 0; i--)
            {
                if (adjustment < 1) break;

               
                    int possibleReduction = (int)_Columns[i].Size - (int)_Columns[i].MinimumSize();
                    if (possibleReduction >= 1)
                    {
                        int actualReduction = 0;

                        if (possibleReduction <= adjustment)
                        {
                            actualReduction = possibleReduction;
                        }
                        else
                        {
                            actualReduction = adjustment;
                        }

                        int current = (int)_Columns[i].Size;
                        _Columns[i].Size = (ColumnSize)(current - actualReduction);
                        adjustment -= actualReduction;
                    }
                
            }

            int afterSize = SumOfSetColumnSizes();
            if (afterSize == targetSize) return true;

            return false;
        }

        private bool StretchColumnsToFit(int actualSize, int targetSize)
        {
            int adjustment = targetSize - actualSize;
            int lastIndex = _Columns.Count - 1;
            int current = (int)_Columns[lastIndex].Size;
            _Columns[lastIndex].Size = (ColumnSize)(current + adjustment);

            int afterSize = SumOfSetColumnSizes();
            if (afterSize == targetSize) return true;
            
            return false;
        }

        private int SumOfSetColumnSizes()
        {
            int result = 0;
            foreach (Column c in _Columns)
            {
                result += (int)c.Size;
            }
            return result;
        }

        public string Id { get; set; }


        private string Render(RequestContext context, bool IsEditMode, Catalog.Category containerCategory)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"cols editable issortable");
            if (this.SpacerAbove)
            {
                sb.Append(" spacerabove");
            }
            sb.Append("\"");

            if (IsEditMode)
            {
                sb.Append(" id=\"part" + this.Id + "\"");
            }

            sb.Append(">");
            
            if (IsEditMode)
            {
                sb.Append(PartHelper.RenderEditTools(this.Id));                                                
                sb.Append("<div class=\"colholder\"><strong>Columns</strong></div>");                
            }

            for (int i = 0; i < _Columns.Count; i++)
            {
                bool isLast = (i == _Columns.Count - 1);

                sb.Append("<div class=\"grid_");
                sb.Append((int)_Columns[i].Size);
                if (isLast)
                {
                    if (_Columns[i].NoGutter)
                    {
                        if (_Parent.NoGutter)
                        {
                            sb.Append("w");
                        }
                        else
                        {
                            sb.Append("l");
                        }
                    }
                    else
                    {
                        sb.Append("l");
                    }
                }
                else
                {
                    if (_Columns[i].NoGutter)
                    {
                        sb.Append("w");
                    }
                }
                sb.Append("\" >");
                if (IsEditMode)
                {
                    sb.Append("<div class=\"droppable\" id=\"part" + _Columns[i].Id + "\">");
                    
                    sb.Append("<div class=\"coltools\"><strong>Col");
                    if ((int)_Columns[i].Size > 1)
                    {
                        sb.Append("umn");
                    }
                    sb.Append("</strong> [" + ((int)_Columns[i].Size).ToString() + "]</div>");                    

                    sb.Append(_Columns[i].RenderForEdit(context, containerCategory));
                    sb.Append("</div>");
                }
                else
                {
                    sb.Append(_Columns[i].RenderForDisplay(context, containerCategory));
                }
                sb.Append("</div>");
            }
            sb.Append("<div class=\"clearcol\"></div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        public string RenderForEdit(RequestContext context, Catalog.Category containerCategory)
        {
            return Render(context, true, containerCategory);
        }

        public string RenderForDisplay(RequestContext context, Catalog.Category containerCategory)
        {
            return Render(context, false, containerCategory);
        }



        public PartJsonResult ProcessJsonRequest(System.Collections.Specialized.NameValueCollection form,
                                                MerchantTribeApplication app, Catalog.Category containerCategory)
        {
            PartJsonResult result = new PartJsonResult();

            string action = form["partaction"];
            switch (action.ToLowerInvariant())
            {          
                case "showeditor":
                    result.IsFinishedEditing = false;
                    result.Success = true;
                    result.ResultHtml = BuildEditor();                                        
                    break;
                case "saveedit":

                    string colsRequestedString = form["totalColumnsRequested"];
                    int columnsRequested = this.Columns.Count;
                    if (int.TryParse(colsRequestedString, out columnsRequested))
                    {
                        this.SetNumberOfColumns(columnsRequested);
                    }
                    ParseColumnChanges(form);
                    if (form["spacerabove"] == "on")
                    {
                        this.SpacerAbove = true;
                    }
                    else
                    {
                        this.SpacerAbove = false;
                    }
                    app.CatalogServices.Categories.Update(containerCategory);
                    result.Success = true;
                    result.IsFinishedEditing = false;
                    result.ResultHtml = BuildEditor();
                    break;
                case "canceledit":
                    result.Success = true;
                    result.IsFinishedEditing = true;
                    result.ResultHtml = this.RenderForEdit(app.CurrentRequestContext, containerCategory);
                    break;
                case "deletepart":
                    containerCategory.GetCurrentVersion().Root.RemovePart(this.Id);                    
                    app.CatalogServices.Categories.Update(containerCategory);
                    result.Success = true;
                    break;

            }
            return result;
        }

       

        public IContentPart FindPart(string partId)
        {

            foreach (Column c in this._Columns)
            {
                if (c.Id == partId) return c;

                foreach (IContentPart p in c.Parts)
                {
                    if (p.Id == partId)
                    {
                        return p;
                    }
                    if (p is IColumnContainer)
                    {
                        IColumnContainer container = p as IColumnContainer;
                        IContentPart result = container.FindPart(partId);
                        if (result != null) return result;
                    }
                }
            }
            
            return null;
        }

        public bool RemovePart(string partId)
        {

            foreach (Column c in this._Columns)
            {
                IContentPart toRemove = null;

                foreach (IContentPart p in c.Parts)
                {
                    if (p.Id == partId)
                    {
                        toRemove = p;
                        break;
                    }
                    if (p is IColumnContainer)
                    {
                        IColumnContainer container = p as IColumnContainer;
                        bool result = container.RemovePart(partId);
                        if (result) return true;
                    }
                }

                if (toRemove != null)
                {
                    c.Parts.Remove(toRemove);
                    return true;
                }
            }

            return false;
        }


        public void SerializeToXml(ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement("part");

            xw.WriteElementString("id", this.Id);
            xw.WriteElementString("typecode", "columncontainer");
            xw.WriteStartElement("spacerabove"); xw.WriteValue(this.SpacerAbove); xw.WriteEndElement();

            xw.WriteStartElement("columns");
            foreach (IColumn c in this.Columns)
            {
                c.SerializeToXml(ref xw);
            }
            xw.WriteEndElement();

            xw.WriteEndElement();            
        }

        public void DeserializeFromXml(string xml)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            this.Columns.Clear();

            XmlNode spacerNode = xdoc.SelectSingleNode("/part/spacerabove");
            if (spacerNode != null)
            {
                bool spacerOut = false;
                if (bool.TryParse(spacerNode.InnerText, out spacerOut))
                {
                    this.SpacerAbove = spacerOut;
                }
            }

            XmlNodeList columnNodes = xdoc.SelectNodes("/part/columns/part");
            if (columnNodes != null)
            {
                foreach (XmlNode node in columnNodes)
                {
                    XmlNode idNode = node.SelectSingleNode("id");
                    if (idNode != null)
                    {
                        Column col = new Column();
                        col.Id = idNode.InnerText;
                        col.DeserializeFromXml(node.OuterXml);
                        this.Columns.Add(col);
                    }
                }
            }                        
        }

        private string BuildEditor()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"flexeditarea\">");

            sb.Append("How Many Columns? <select name=\"totalColumnsRequested\">");
            for (int i = 2; i <= this.maxColumns; i++)
            {                
                sb.Append("<option");
                if (i == this.Columns.Count)
                {
                    sb.Append(" selected=\"selected\"");
                }
                sb.Append(" value=\"" + i.ToString() + "\">" + i.ToString() + "</option>");
            }
            sb.Append("</select><br />&nbsp;<br/>");
            sb.Append("<b>COLUMNS</b>");
            sb.Append("<table>");

            // Size Row
            sb.Append("<tr><td><b>SIZE:</b></td>");
            for (int i = 0; i < this.Columns.Count; i++)
            {
                sb.Append("<td><select name=\"colsize" + i.ToString() + "\">");
                for (int q = 1; q <= this.maxIndividualColumnSize; q++)
                {
                    sb.Append("<option");
                    if ((int)this.Columns[i].Size == q)
                    {
                        sb.Append(" selected=\"selected\"");
                    }
                    sb.Append(" value=\"" + q.ToString() + "\">" + q.ToString() + "</option>");
                }
                sb.Append("</select></td>");
            }
            sb.Append("</tr>");

            // Wide Size
            sb.Append("<tr><td><b>WIDE:</b></td>");
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (i < (this.Columns.Count - 1))
                {
                    sb.Append("<td><input type=\"checkbox\" name=\"colwide" + i.ToString() + "\"");
                    if (this.Columns[i].NoGutter)
                    {
                        sb.Append(" checked=\"checked\"");
                    }
                    sb.Append(">");
                    sb.Append("</td>");
                }
                else
                {
                    // last column can't be wide
                    sb.Append("<td>&nbsp;</td>");
                }
            }
            sb.Append("</tr>");
                       

            sb.Append("</table>");
            sb.Append("<br />&nbsp;<br />");

            sb.Append("Spacer Above Columns? <input type=\"checkbox\" name=\"spacerabove\"");
            if (this.SpacerAbove)
            {
                sb.Append(" checked=\"checked\"");
            }
            sb.Append("><br />");            

            sb.Append("</div>");
            
            sb.Append("<div class=\"flexeditbuttonarea\">");
            sb.Append("<input type=\"hidden\" name=\"partaction\" class=\"editactionhidden\" value=\"saveedit\" />");
            sb.Append("<input type=\"submit\" name=\"canceleditbutton\" value=\"Close\">");
            sb.Append("<input type=\"submit\" name=\"savechanges\" value=\"Update Columns\">");
            sb.Append("</div>");

            
            return sb.ToString();
        }

        private void ParseColumnChanges(System.Collections.Specialized.NameValueCollection form)
        {
            for (int i = 0; i < this.Columns.Count; i++)
            {
                string tempSize = form["colsize" + i.ToString()];
                if (tempSize == null) continue;
                if (tempSize == string.Empty) continue;

                int size = (int)this.Columns[i].Size;
                if (int.TryParse(tempSize, out size))
                {
                    this.Columns[i].Size = (ColumnSize)size;
                }

                string tempGutter = form["colwide" + i.ToString()];
                if (tempGutter == "on")
                {
                    this.Columns[i].NoGutter = true;
                }
                else
                {
                    this.Columns[i].NoGutter = false;
                }

            }
            this.AdjustColumnsToFitParentSize();
        }
    }
}
