using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Controls
{
	public class BVRegularExpressionValidator : System.Web.UI.WebControls.RegularExpressionValidator
	{

		private string _regularExpressionKey = string.Empty;
		private string _errorMessageKey = string.Empty;

		public string RegularExpressionKey {
			get { return _regularExpressionKey; }
			set { _regularExpressionKey = value; }
		}

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

			if (!string.IsNullOrEmpty(_regularExpressionKey)) {
				this.ValidationExpression = BVValidationController.GetRegularExpression(_regularExpressionKey);
			}

			if (!string.IsNullOrEmpty(_errorMessageKey)) {
				this.ErrorMessage = BVValidationController.GetErrorMessage(_errorMessageKey);
			}
		}

		protected override object SaveViewState()
		{
			object[] args = new object[3];
			args[0] = base.SaveViewState();
			args[1] = _regularExpressionKey;
			args[2] = _errorMessageKey;
			return args;
		}

		protected override void LoadViewState(object savedState)
		{
			object[] args = (object[])savedState;
			_regularExpressionKey = (string)args[1];
			_errorMessageKey = (string)args[2];
			base.LoadViewState(args[0]);
		}
	}
}
