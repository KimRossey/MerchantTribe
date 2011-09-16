<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewOrderFromAcumatica.ascx.cs" Inherits="BVCommerce.BVModules.Controls.ViewOrderFromAcumatica" %>
<%@ Register Src="PrintThisPage.ascx" TagName="PrintThisPage" TagPrefix="uc1" %>
<%@ Register src="ViewOrderItems.ascx" tagname="ViewOrderItems" tagprefix="uc2" %>

<table cellspacing="0" cellpadding="3" width="100%">
    <tr>
        <td width="67%" colspan="2">
            <h1 id="OrderNumberHeader" runat="server">Order
                <asp:Label ID="OrderNumberField" runat="server" Text="000000"></asp:Label></h1>
            <asp:Label ID="PONumberLabel" runat="server" Text=""></asp:Label>
        </td>
        <td width="33%" valign="top" align="right">
            <uc1:PrintThisPage ID="PrintThisPage1" runat="server" /><br />            
            <asp:Label ID="StatusField" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <span class="lightlabel">Bill To:</span><br />
            <asp:Label ID="BillingAddressField" runat="server"></asp:Label>
        </td>
        <td valign="top">
            <asp:Panel ID="pnlShipTo" runat="server" Visible="true">
                <span class="lightlabel">Ship To:</span><br />
                <asp:Label ID="ShippingAddressField" runat="server"></asp:Label></asp:Panel>
            &nbsp;
        </td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td colspan="3"><uc2:ViewOrderItems ID="ViewOrderItems1" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="3" align="right">
            <table class="totaltable">
            <tr>
                <td class="totallabel">Items:</td>
                <td class="totalsub"><asp:Literal ID="litTotalSub" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="totallabel">Shipping:</td>
                <td class="totalsub"><asp:Literal ID="litTotalShipping" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="totallabel">Tax:</td>
                <td class="totalsub"><asp:Literal ID="litTotalTax" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="totallabel">Grand Total:</td>
                <td class="totalsub"><asp:Literal ID="litTotalGrand" runat="server"></asp:Literal></td>
            </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <asp:Panel ID="pnlInstructions" runat="server" Visible="false">
                <em>Customer's Instructions:</em><br />
                <asp:Label ID="InstructionsField" runat="server"></asp:Label>
            </asp:Panel> 
            <!--<em>Codes Used:</em><br />-->
            <asp:Label ID="CouponField" runat="server" CssClass="BVSmallText"></asp:Label>           
        </td>
        <td valign="top" colspan="2" align="right">
            <asp:literal ID="litTotals" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <div style="height: 20px;">
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="3">
        <div id="packagesdiv" runat="server">
                <em>Shipments:</em>   
                <asp:Literal ID="litPackages" runat="server"></asp:Literal>
                &nbsp;<br />
            </div>
        </td>
    </tr>
    <tr runat="Server" id="trNotes">
        <td valign="top" class="controlarea2">
            <em>Public Notes:</em>
            <asp:GridView GridLines="None" Style="margin-top: 8px;" Width="100%" ID="PublicNotesField"
                runat="server" ShowHeader="False" AutoGenerateColumns="False" DataKeyNames="Id">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblAuditDate" runat="server" Text='<%# Bind("AuditDate","{0:d}") %>'></asp:Label><br />
                            <asp:Label ID="NoteField" runat="server" Text='<%# Bind("Note") %>'></asp:Label>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle CssClass="row" />
                <AlternatingRowStyle CssClass="alternaterow" />
            </asp:GridView>
        </td>
        <td valign="top" class="controlarea2">
            &nbsp;
        </td>
        <td class="controlarea2" align="left" valign="top">
            <em>Payment Information:</em>
            <table class="controlarea1" style="margin-top: 8px; color: #666; font-size: 11px;"
                border="0" cellspacing="0" cellpadding="3" width="100%">
                <tr>
                    <td colspan="2" class="formfield" style="border-bottom: solid 1px #999;">
                        <asp:Label runat="server" ID="lblPaymentSummary"></asp:Label></td>
                </tr>                
                <tr>
                    <td class="formfield">
                        Payment Received:</td>
                    <td class="formlabel">
                        <asp:Label ID="PaymentChargedField" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="formfield">
                        Gift Card Total:</td>
                    <td class="formlabel">
                        <asp:Label ID="GiftCardAmountLabel" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="formfield">
                        Total:</td>
                    <td class="formlabel">
                        <asp:Label ID="PaymentTotalField" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="formfield" style="border-bottom: solid 1px #999; padding-bottom: 3px;">
                        Refunded:</td>
                    <td class="formlabel" style="border-bottom: solid 1px #999; padding-bottom: 3px;">
                        <asp:Label ID="PaymentRefundedField" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="formfield">
                        Amount Due:</td>
                    <td class="formlabel" style="color: #333;">
                        <strong>
                            <asp:Label ID="PaymentDueField" runat="server"></asp:Label></strong></td>
                </tr>
            </table>
            &nbsp;<br />
            
        </td>
    </tr>
</table>
