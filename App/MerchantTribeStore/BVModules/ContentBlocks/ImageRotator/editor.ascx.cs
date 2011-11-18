//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.IO;
//using MerchantTribe.Commerce;
//using MerchantTribe.Commerce.Content;
//using System.Text;

//namespace MerchantTribeStore.BVModules.ContentBlocks.ImageRotator
//{
//    public partial class editor : BVModule
//    {
//         protected override void OnLoad(System.EventArgs e)
//        {
//            base.OnLoad(e);

//             this.RegisterWindowScripts();

//        if (!Page.IsPostBack)
//        {
//            LoadItems();
//            this.chkShowInOrder.Checked = SettingsManager.GetBooleanSetting("ShowInOrder");
//            this.PreHtmlField.Text = SettingsManager.GetSetting("PreHtml");
//            this.PostHtmlField.Text = SettingsManager.GetSetting("PostHtml");
//            this.cssclass.Text = SettingsManager.GetSetting("cssclass");


//            this.WidthField.Text = SettingsManager.GetIntegerSetting("Width");
//            if ((this.WidthField.Text.Trim() == String.Empty) || (this.WidthField.Text == "0"))
//            {
//                this.WidthField.Text = "220";
//            }

//            this.HeighField.Text = SettingsManager.GetIntegerSetting("Height");
//            if ((this.HeighField.Text.Trim() == String.Empty) || (this.HeighField.Text == "0"))
//            {
//                this.HeighField.Text = "220";
//            }

//            int seconds = this.SettingsManager.GetIntegerSetting("Pause");
//            if (seconds < 0) seconds = 3;
//            this.PauseField.Text = seconds.ToString();


//        }
         
//        }

//        private void LoadItems()
//        {
//            this.GridView1.DataSource = SettingsManager.GetSettingList("Images");
//            this.GridView1.DataBind();
//        }

//        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
//        {
//            ClearEditor();
//        }

//           private void ClearEditor()
//           {
//                this.EditBvinField.Value = String.Empty;
//                this.ImageUrlField.Text = String.Empty;
//                this.ImageLinkField.Text = String.Empty;
//                this.chkOpenInNewWindow.Checked = false;
//                this.AltTextField.Text = String.Empty;
//                this.btnNew.ImageUrl = "~/BVAdmin/Images/Buttons/New.png";
//           }

//          private void RegisterWindowScripts()
//          {
//        StringBuilder sb = new StringBuilder();

//        sb.Append("var w;");
//        sb.Append("function popUpWindow(parameters) {");
//        sb.Append("w = window.open('../ImageBrowser.aspx' + parameters, null, 'height=480, width=640');");
//        sb.Append("}");

//        sb.Append("function closePopup() {");
//        sb.Append("w.close();");
//        sb.Append("}");

//        sb.Append("function SetImage(fileName) {");
//        sb.Append("document.getElementById('");
//        sb.Append(this.ImageUrlField.ClientID);
//        sb.Append("').value = '~/' + fileName;");
//        sb.Append("w.close();");
//        sb.Append("}");
              
//        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "WindowScripts", sb.ToString(), true);

//        }
//    }
//}
    

//    Protected Sub btnOkay_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnOkay.Click
//        SettingsManager.SaveSetting("PreHtml", Me.PreHtmlField.Text.Trim, "bvsoftware", "Content Block", "Image Rotator jQuery")
//        SettingsManager.SaveSetting("PostHtml", Me.PostHtmlField.Text.Trim, "bvsoftware", "Content Block", "Image Rotator jQuery")
//        SettingsManager.SaveBooleanSetting("ShowInOrder", Me.chkShowInOrder.Checked, "bvsoftware", "Content Block", "Image Rotator jQuery")
//        SettingsManager.SaveSetting("cssclass", Me.cssclass.Text.Trim(), "bvsoftware", "Content Block", "Image Rotator jQuery")

//        Dim width As Integer = 0
//        Integer.TryParse(Me.WidthField.Text.Trim, width)
//        SettingsManager.SaveIntegerSetting("Width", width, "bvsoftware", "Content Block", "Image Rotator jQuery")
//        Dim height As Integer = 0
//        Integer.TryParse(Me.HeighField.Text.Trim, height)
//        SettingsManager.SaveIntegerSetting("Height", height, "bvsoftware", "Content Block", "Image Rotator jQuery")
//        Dim pause As Integer = 0
//        Integer.TryParse(Me.PauseField.Text, pause)
//        SettingsManager.SaveIntegerSetting("Pause", pause, "bvsoftware", "Content Block", "Image Rotator jQuery")

//        Me.NotifyFinishedEditing()
//    End Sub

//    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnNew.Click
//        Dim c As New Content.ComponentSettingListItem

//        If Me.EditBvinField.Value <> String.Empty Then
//            'Updating
//            c = SettingsManager.FindSettingListItem(EditBvinField.Value)
//            c.Setting1 = Me.ImageUrlField.Text.Trim
//            c.Setting2 = Me.ImageLinkField.Text.Trim
//            If Me.chkOpenInNewWindow.Checked = True Then
//                c.Setting3 = "1"
//            Else
//                c.Setting3 = "0"
//            End If
//            c.Setting4 = Me.AltTextField.Text.Trim
//            SettingsManager.UpdateSettingListItem(c, "bvsoftware", "Content Block", "Image Rotator jQuery")
//            ClearEditor()
//        Else
//            'Inserting
//            c.Setting1 = Me.ImageUrlField.Text.Trim
//            c.Setting2 = Me.ImageLinkField.Text.Trim
//            If Me.chkOpenInNewWindow.Checked = True Then
//                c.Setting3 = "1"
//            Else
//                c.Setting3 = "0"
//            End If
//            c.Setting4 = Me.AltTextField.Text.Trim
//            SettingsManager.InsertSettingListItem("Images", c, "bvsoftware", "Content Block", "Image Rotator jQuery")
//        End If
//        LoadItems()
//    End Sub

 

//    Protected Sub GridView1_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
//        Dim bvin As String = String.Empty
//        bvin = CType(sender, GridView).DataKeys(e.RowIndex).Value.ToString
//        SettingsManager.MoveSettingListItemDown(bvin, "Images")
//        LoadItems()
//    End Sub

//    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
//        Dim bvin As String = CType(Me.GridView1.DataKeys(e.RowIndex).Value, String)
//        SettingsManager.DeleteSettingListItem(bvin)
//        LoadItems()
//    End Sub

//    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
//        Dim bvin As String = CType(GridView1.DataKeys(e.NewEditIndex).Value, String)
//        Dim c As Content.ComponentSettingListItem
//        c = MyBase.SettingsManager.FindSettingListItem(bvin)
//        If c.Bvin <> String.Empty Then
//            Me.EditBvinField.Value = c.Bvin
//            Me.ImageLinkField.Text = c.Setting2
//            Me.ImageUrlField.Text = c.Setting1
//            If c.Setting3 = "1" Then
//                Me.chkOpenInNewWindow.Checked = True
//            Else
//                Me.chkOpenInNewWindow.Checked = False
//            End If
//            Me.AltTextField.Text = c.Setting4
//            Me.btnNew.ImageUrl = "~/BVAdmin/Images/Buttons/SaveChanges.png"
//        End If
//    End Sub

//    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating
//        Dim bvin As String = String.Empty
//        bvin = CType(sender, GridView).DataKeys(e.RowIndex).Value.ToString
//        SettingsManager.MoveSettingListItemUp(bvin, "Images")
//        LoadItems()
//    End Sub   