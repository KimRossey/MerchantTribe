
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BVSoftware.Commerce.Controls
{
	[DefaultProperty("Text"), ToolboxData("<{0}:SplitButton runat=server></{0}:SplitButton>")]
	public class SplitButton : WebControl, IPostBackEventHandler
	{

		public event EventHandler<CommandEventArgs> Click;

		public void RaisePostBackEvent(string eventArgument)
		{

			string[] values = eventArgument.Split(new string[] { "###" }, System.StringSplitOptions.None);
			string name = string.Empty;
			string arg = string.Empty;

			if (values.Length > 0) {
				name = values[0];
			}

			if (values.Length > 1) {
				arg = values[1];
			}
			CommandEventArgs args = new CommandEventArgs(name, arg);
			OnClick(args);
		}

		protected virtual void OnClick(EventArgs e)
		{
			if (Click != null) {
				Click(this, (CommandEventArgs)e);
			}
		}


		private ListItemCollection _Items;

		public ListItemCollection Items {
			get {
				if (_Items == null) {
					_Items = new ListItemCollection();
					((IStateManager)_Items).TrackViewState();
				}
				return _Items;
			}
			set {
				_Items = value;
				((IStateManager)_Items).TrackViewState();
			}
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);

			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Body"));
			writer.AddAttribute("class", "splitbutton");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Body"));
			writer.AddAttribute("class", "splitbuttonbody");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			writer.RenderEndTag();

			writer.AddAttribute("class", "splitdropdown");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);


			writer.AddAttribute("class", "splitdropdownpanel");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			RenderSubMenuContents(writer);

			writer.RenderEndTag();


			writer.RenderEndTag();

			writer.RenderEndTag();
		}

		protected void RenderSubMenuContents(HtmlTextWriter writer)
		{
			bool initialized = false;

			foreach (ListItem item in this.Items) {
				if (!initialized) {
					writer.RenderBeginTag(HtmlTextWriterTag.Ul);
					initialized = true;
				}

				writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, item.Text + "###" + item.Value));
				writer.RenderBeginTag(HtmlTextWriterTag.Li);
				writer.Write(item.Text);
				writer.RenderEndTag();
			}

			if (initialized) {
				writer.RenderEndTag();
			}
		}

		protected override object SaveViewState()
		{
			return new Pair(((IStateManager)_Items).SaveViewState(), base.SaveViewState());
		}

		protected override void LoadViewState(object savedState)
		{
			Pair state = (Pair)savedState;
			if (state != null) {
				if (this._Items == null) {
					this._Items = new ListItemCollection();
				}
				((IStateManager)this._Items).LoadViewState(state.First);
				base.LoadViewState(state.Second);
			}
		}


	}
}
