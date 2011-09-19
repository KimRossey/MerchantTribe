using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Controls
{
	public class BVCompareValidator : System.Web.UI.WebControls.CompareValidator
	{

		private string _errorMessageKey = string.Empty;

		public string ErrorMessageKey {
			get { return _errorMessageKey; }
			set { _errorMessageKey = value; }
		}

		private new void Init(object sender, System.EventArgs e)
		{
			if (this.Page is Controls.IBaseAdminPage) {
				this.Text = "*";
			}
			else {
				this.Text = Content.SiteTerms.GetTerm(Content.SiteTermIds.ValidatorFieldMarker);
			}

			if (!string.IsNullOrEmpty(_errorMessageKey)) {
				this.ErrorMessage = BVValidationController.GetErrorMessage(_errorMessageKey);
			}
		}

		protected override object SaveViewState()
		{
			object[] args = new object[2];
			args[0] = base.SaveViewState();
			args[1] = _errorMessageKey;
			return args;
		}

		protected override void LoadViewState(object savedState)
		{
			object[] args = (object[])savedState;
			_errorMessageKey = (string)args[1];
			base.LoadViewState(args[0]);
		}

	}
}
