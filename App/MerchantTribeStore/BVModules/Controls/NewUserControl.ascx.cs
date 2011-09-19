using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;

namespace BVCommerce
{

    partial class BVModules_Controls_NewUserControl : MerchantTribe.Commerce.Content.BVUserControl
    {
        public delegate void LoginCompletedDelegate(object sender, MerchantTribe.Commerce.Controls.LoginCompleteEventArgs args);
        public event LoginCompletedDelegate LoginCompleted;

        public bool LoginAfterCreate
        {
            get
            {
                object val = null;
                val = ViewState["LoginAfterCreate"];
                if (val == null)
                {
                    return false;
                }
                else
                {
                    return (bool)val;
                }
            }
            set { ViewState["LoginAfterCreate"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
            if (!Page.IsPostBack)
            {
                this.btnSaveChanges.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("createaccount", Request.IsSecureConnection);
            }
            //PasswordReminder.InnerText = "Password must be at least " + WebAppSettings.PasswordMinimumLength + " characters long.";
            //AddDynamicQuestions();
        }

        //protected void AddDynamicQuestions()
        //{
        //    Collection<UserQuestion> questions = UserQuestion.FindAll();
        //    int count = 0;
        //    foreach (UserQuestion question in questions)
        //    {
        //        count += 1;
        //        HtmlTableRow row = new HtmlTableRow();
        //        row.ID = "DynamicRow" + count;
        //        if (question.Type == UserQuestionType.FreeAnswer)
        //        {
        //            Label questionLabel = new Label();
        //            questionLabel.ID = "questionLabel" + count.ToString();
        //            questionLabel.AssociatedControlID = "questionTextBox" + count.ToString();
        //            questionLabel.Text = question.Values[0].Value;
        //            TextBox questionTextBox = new TextBox();
        //            questionTextBox.ID = "questionTextBox" + count.ToString();
        //            questionTextBox.Text = "";
        //            questionTextBox.TabIndex = (short)(2009 + count);
        //            questionTextBox.Columns = 30;
        //            questionTextBox.CssClass = "forminput";

        //            HtmlTableCell cell = new HtmlTableCell();
        //            cell.Attributes.Add("class", "formlabel");
        //            cell.Controls.Add(questionLabel);
        //            row.Cells.Add(cell);
        //            cell = new HtmlTableCell();
        //            cell.Attributes.Add("class", "formfield");
        //            cell.Controls.Add(questionTextBox);
        //            row.Cells.Add(cell);

        //            MerchantTribe.Commerce.Controls.BVRequiredFieldValidator validator = new MerchantTribe.Commerce.Controls.BVRequiredFieldValidator();
        //            validator.Text = "*";
        //            validator.ErrorMessage = "Field is required.";
        //            validator.ControlToValidate = "questionTextBox" + count.ToString();
        //            validator.ValidationGroup = "NewUser";
        //            validator.CssClass = "errormessage";
        //            cell.Controls.Add(validator);
        //        }
        //        else if (question.Type == UserQuestionType.MultipleChoice)
        //        {
        //            Label questionLabel = new Label();
        //            questionLabel.ID = "questionLabel" + count.ToString();
        //            questionLabel.Text = question.Values[question.Values.Count - 1].Value.ToString();
        //            questionLabel.AssociatedControlID = "questionDropDownList" + count.ToString();
        //            question.Values.RemoveAt(question.Values.Count - 1);
        //            DropDownList questionDropDownList = new DropDownList();
        //            questionDropDownList.ID = "questionDropDownList" + count.ToString();
        //            questionDropDownList.TabIndex = (short)(2009 + count);
        //            questionDropDownList.CssClass = "forminput";
        //            foreach (UserQuestionOption questionOption in question.Values)
        //            {
        //                questionDropDownList.Items.Add(questionOption.Value);
        //            }
        //            HtmlTableCell cell = new HtmlTableCell();
        //            cell.Controls.Add(questionLabel);
        //            cell.Attributes.Add("class", "formlabel");
        //            row.Cells.Add(cell);
        //            cell = new HtmlTableCell();
        //            cell.Attributes.Add("class", "formfield");
        //            cell.Controls.Add(questionDropDownList);
        //            row.Cells.Add(cell);

        //            MerchantTribe.Commerce.Controls.BVRequiredFieldValidator validator = new MerchantTribe.Commerce.Controls.BVRequiredFieldValidator();
        //            validator.Text = "*";
        //            validator.ErrorMessage = "Field is required.";
        //            validator.ControlToValidate = "questionDropDownList" + count.ToString();
        //            validator.ValidationGroup = "NewUser";
        //            validator.CssClass = "errormessage";
        //            cell.Controls.Add(validator);
        //        }
        //        NewUserTable.Rows.Add(row);
        //    }
        //}

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                bool result = false;

                // Check password length
                if (this.PasswordField.Text.Trim().Length < WebAppSettings.PasswordMinimumLength)
                {
                    this.lblError.Visible = true;
                    this.lblError.Text = "Password must be at least " + WebAppSettings.PasswordMinimumLength + " characters long.";
                }
                else
                {
                    CustomerAccount u = new CustomerAccount();
                    //u = MyPage.BVApp.MembershipServices.Customers.Find(this.BvinField.Value);

                    if (u != null)
                    {

                        u.Email = this.EmailField.Text.Trim();
                        u.FirstName = this.FirstNameField.Text.Trim();
                        u.LastName = this.LastNameField.Text.Trim();

                        //int count = 0;
                        //foreach (HtmlTableRow row in NewUserTable.Rows)
                        //{
                        //    if (row.ID != null)
                        //    {
                        //        if (row.ID.StartsWith("DynamicRow"))
                        //        {
                        //            count += 1;
                        //            Label label = (Label)row.FindControl("questionLabel" + count.ToString());
                        //            object obj = row.FindControl("questionTextBox" + count.ToString());
                        //            if (obj != null)
                        //            {
                        //                u.CustomQuestionAnswers += " " + label.Text + ":" + ((TextBox)obj).Text + Environment.NewLine;
                        //            }
                        //            else
                        //            {
                        //                obj = row.FindControl("questionDropDownList" + count.ToString());
                        //                if (obj != null)
                        //                {
                        //                    u.CustomQuestionAnswers += " " + label.Text + ":" + ((DropDownList)obj).SelectedItem.Text + Environment.NewLine;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        CreateUserStatus s = CreateUserStatus.None;

                        // Create new user
                        result = MyPage.BVApp.MembershipServices.CreateCustomer(u, ref s, this.PasswordField.Text.Trim());
                                                

                        if (result == false)
                        {
                            switch (s)
                            {
                                case CreateUserStatus.DuplicateUsername:
                                    this.lblError.Visible = true;
                                    this.lblError.Text = "That email already exists. Select another email or login to your current account.";
                                    break;
                                default:
                                    this.lblError.Visible = true;
                                    this.lblError.Text = "Unable to save user. Unknown error.";
                                    break;
                            }
                        }
                        else
                        {
                            // Update bvin field so that next save will call updated instead of create
                            this.BvinField.Value = u.Bvin;
                            if (LoginAfterCreate)
                            {
                                MerchantTribe.Web.Cookies.SetCookieString(MerchantTribe.Commerce.WebAppSettings.CookieNameAuthenticationTokenCustomer(),
                                                                        u.Bvin,
                                                                        this.Request.RequestContext.HttpContext, false, new EventLog());
                                
                            }
                            MerchantTribe.Commerce.Controls.LoginCompleteEventArgs args = new MerchantTribe.Commerce.Controls.LoginCompleteEventArgs();
                            args.UserId = u.Bvin;
                            args.UserEmail = u.Email;
                            if (LoginCompleted != null)
                            {
                                LoginCompleted(this, args);
                            }
                        }

                    }
                }
            }
        }
    }
}