<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_Shipping" Codebehind="Shipping.ascx.cs" %>
<div id="lstShipping"><asp:Literal ID="litMain" runat="server" ClientIDMode="Static" /></div>
<span class="shipping-changing" style="display: none;"><asp:Image runat="server" EnableViewState="false" ImageUrl="~/content/images/system/ajax-loader-small.gif" border="0" AlternateText="Loading..." /></span>
<div id="shipping-not-valid">Enter a shipping address first</div>
<div id="shipping-needs-refresh"></div>                    