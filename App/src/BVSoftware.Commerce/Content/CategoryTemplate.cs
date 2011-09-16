using System;

namespace BVSoftware.Commerce.Content
{
	public abstract class CategoryTemplate : BVModule
	{

		private Catalog.Category _ModuleCategory;

		public Catalog.Category ModuleCategory {
			get { return _ModuleCategory; }
			set { _ModuleCategory = value; }
		}

	}

}
