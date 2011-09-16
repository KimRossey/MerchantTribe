<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Controls_AdditionalImages" Codebehind="AdditionalImages.ascx.cs" %>
                        
<div id="MorePictures">
    <a id="ZoomLink" onclick="JavaScript:void(0)" runat="server">
    <asp:Image 
    Style="cursor: hand" 
    ID="imgZoom" 
    ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/MorePictures.png" 
    runat="server"
    AlternateText="More Pictures">
    </asp:Image>
    </a>
</div>