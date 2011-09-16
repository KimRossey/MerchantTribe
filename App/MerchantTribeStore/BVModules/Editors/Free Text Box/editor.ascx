<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Editors_Free_Text_Box_editor" Codebehind="editor.ascx.cs" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<div>FREE TEXT BOX EDITOR</div>
<FTB:FreeTextBox 
    AutoGenerateToolbarsFromString="true" 
    ToolbarLayout="Bold, Italic, Underline|InsertImage, CreateLink, Unlink, RemoveFormat|BulletedList, NumberedList|JustifyLeft, JustifyRight, JustifyCenter, JustifyFull|FontFacesMenu,FontSizesMenu|StylesMenu"
    SupportFolder="FtbWebResource.axd" 
    ID="FreeTextBox1" 
    AutoParseStyles="False"    
    runat="Server">   
</FTB:FreeTextBox>
<asp:HiddenField ID="WidthField" runat="server" Value="770" />
<asp:HiddenField ID="HeightField" runat="server" Value="300" />
<asp:HiddenField ID="WrapField" runat="server" Value="1" />