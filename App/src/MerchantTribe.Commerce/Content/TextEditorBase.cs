using System;

namespace MerchantTribe.Commerce.Content
{

	public abstract class TextEditorBase : System.Web.UI.UserControl
	{

		public abstract int EditorWidth {
			get;
			set;
		}
		public abstract int EditorHeight {
			get;
			set;
		}
		public abstract bool EditorWrap {
			get;
			set;
		}
		public abstract string Text {
			get;
			set;
		}
		public abstract string PreTransformText {
			get;
			set;
		}
		public abstract bool SupportsTransform {
			get;
		}
		public abstract int TabIndex {
			get;
			set;
		}

	}
}
