<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="True" Inherits="BVCommerce.BVAdmin_Configuration_Wallpaper" Codebehind="Wallpaper.aspx.cs" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="headcontent" ContentPlaceHolderID="headcontent" runat="server">
<script type="text/javascript">

    function CheckChanged() {
        var chk = $('#uselogoimage').attr('checked');

        if (chk) {
            $('.logo-text-controls').hide();
            $('.logo-image-controls').show();
        }
        else {
            $('.logo-text-controls').show();
            $('.logo-image-controls').hide();
        }
        return true;
    }
</script>

<script type="text/javascript">
    // Jquery Setup
    $(document).ready(function () {

        $('#uselogoimage').click(function () {
            CheckChanged();
        });

        CheckChanged();
    });                      // End Doc Ready
    </script>

</asp:Content>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

        <h1>Admin Wallpaper</h1>       
        <table class="formtable">
        <tr>
            <td><asp:ImageButton ID="btnNebula" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Select.png" onclick="btnNebula_Click" /></td>
            <td><img src="../../images/system/NebulaPreview.jpg" alt="Nebula" /></td>
        </tr>
        <tr>
            <td><asp:ImageButton ID="btnLeather" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Select.png" onclick="btnLeather_Click" /></td>
            <td><img src="../../images/system/LeatherPreview.jpg" alt="Leather" /></td>
        </tr>
        <tr>
            <td><asp:ImageButton ID="btnBrown" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Select.png" onclick="btnBrown_Click" /></td>
            <td><img src="../../images/system/BrownStripesPreview.jpg" alt="Brown Stripes" /></td>
        </tr>
        <tr>
            <td><asp:ImageButton ID="btnBlue" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Select.png" onclick="btnBlue_Click" /></td>
            <td><img src="../../images/system/BlueStripesPreview.jpg" alt="Blue Stripes" /></td>
        </tr>
        <tr>
            <td><asp:ImageButton ID="btnPink" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Select.png" onclick="btnPink_Click" /></td>
            <td><img src="../../images/system/PinkStripesPreview.jpg" alt="Pink Stripes" /></td>
        </tr>
        <tr>
            <td><asp:ImageButton ID="btnPurple" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Select.png" 
                    onclick="btnPurple_Click"  /></td>
            <td><img src="../../images/system/PurpleStripesPreview.jpg" alt="Purple Stripes" /></td>
        </tr>
        
        </table>            
</asp:Content>

