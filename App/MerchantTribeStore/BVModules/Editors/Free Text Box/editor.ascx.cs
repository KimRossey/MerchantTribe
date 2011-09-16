using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Editors_Free_Text_Box_editor : TextEditorBase
    {

        public override string PreTransformText
        {
            get { return string.Empty; }
            set { this.FreeTextBox1.Text = value; }
        }
        public override bool SupportsTransform
        {
            get { return false; }
        }
        public override string Text
        {
            get { return this.FreeTextBox1.Text; }
            set { this.FreeTextBox1.Text = value; }
        }
        public override int EditorWidth
        {
            get { return int.Parse(this.WidthField.Value); }
            set { this.WidthField.Value = value.ToString(); }
        }
        public override int EditorHeight
        {
            get { return int.Parse(this.HeightField.Value); }
            set { this.HeightField.Value = value.ToString(); }
        }
        public override bool EditorWrap
        {
            get
            {
                if (this.WrapField.Value == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value == true)
                {
                    this.WrapField.Value = "1";
                }
                else
                {
                    this.WrapField.Value = "0";
                }
            }
        }
        public override int TabIndex
        {
            get
            {
                if (this.ViewState["TabIndex"] != null)
                {
                    return (int)this.ViewState["TabIndex"];
                }
                else
                {
                    return -1;
                }
            }
            set { this.ViewState["TabIndex"] = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            //this.FreeTextBox1.DesignModeCss = "BVModules\\Themes\\" + PersonalizationServices.GetPersonalizedThemeName() + "\\Styles.css";
            if (!Page.IsPostBack)
            {

                this.FreeTextBox1.StylesMenuList = new string[] { "breadcrumbs", "Style2" };
                this.FreeTextBox1.StylesMenuNames = new string[] { "Breadcrumbs", "Style 2" };

                this.FreeTextBox1.Width = Unit.Pixel(EditorWidth);
                this.FreeTextBox1.Height = Unit.Pixel(EditorHeight);
                if (TabIndex != -1)
                {
                    this.FreeTextBox1.TabIndex = (short)this.TabIndex;
                }
            }
        }
    }
}