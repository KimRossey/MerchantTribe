using System;

namespace MerchantTribe.Commerce.Content
{
	public abstract class CategoryEditorTemplate : BVModule
	{

		public void Page_Load(object sender, System.EventArgs e)		
        {            
			LoadFormData();
		}

		public abstract void LoadFormData();

		public abstract void SaveFormData();
	}

}

