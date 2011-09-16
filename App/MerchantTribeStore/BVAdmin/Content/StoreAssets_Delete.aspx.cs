﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Storage;

namespace BVCommerce
{

    public partial class BVAdmin_Content_StoreAssets_Delete : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);


            if (!Page.IsPostBack)
            {
                string assetname = Request.Form["assetname"];
                Remove(assetname);
            }

        }

        private void Remove(string asset)
        {
            if (DiskStorage.RemoveStoreAsset(BVApp.CurrentStore.Id, asset))
            {
                this.litOutput.Text = "{\"result\":true}";
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }
        }
    }
}