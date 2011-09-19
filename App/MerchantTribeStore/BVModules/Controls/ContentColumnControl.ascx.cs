
using MerchantTribe.Commerce;
using System.Collections.Generic;
using System.IO;
using MerchantTribe.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Controls_ContentColumnControl : MerchantTribe.Commerce.Content.BVUserControl
    {

        private string _ColumnID = string.Empty;
        private string _ColumnName = string.Empty;

        public string ColumnID
        {
            get { return _ColumnID; }
            set { _ColumnID = value; }
        }
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            LoadColumn();
        }

        public void LoadColumn()
        {
            this.phColumn.Controls.Clear();

            ContentColumn c = new ContentColumn();

            if (_ColumnID != string.Empty)
            {
                c = MyPage.MTApp.ContentServices.Columns.Find(_ColumnID);
            }
            else
            {
                c = MyPage.MTApp.ContentServices.Columns.FindByDisplayName(_ColumnName);
            }

            if (c != null)
            {
                for (int i = 0; i <= c.Blocks.Count - 1; i++)
                {
                    LoadContentBlock(c.Blocks[i]);
                }
            }

        }

        private void LoadContentBlock(ContentBlock b)
        {
            System.Web.UI.Control plugin = ModuleController.LoadContentBlock(b.ControlName, this.Page);
            if (plugin != null)
            {
                if (plugin is BVModule)
                {
                    ((BVModule)plugin).BlockId = b.Bvin;
                    this.phColumn.Controls.Add(plugin);
                }
            }
        }
    }

}