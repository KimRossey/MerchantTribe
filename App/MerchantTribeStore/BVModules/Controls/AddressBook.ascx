<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_AddressBook" Codebehind="AddressBook.ascx.cs" %>
<asp:Panel ID="AddressGridViewPanel" Cssclass="addressbookpanel" runat="server">
<br />
    <asp:ImageButton ID="AddressBookImageButton" runat="server" AlternateText="Address Book" ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/AddressBook.png" CausesValidation="false" /> <br /><br />
    <asp:GridView ID="AddressGridView" runat="server" AutoGenerateColumns="False" 
        DataKeyNames="bvin" Visible="false" ShowHeader="false" ShowFooter="false" 
        AlternatingRowStyle-CssClass="alt" onrowcommand="AddressGridView_RowCommand" 
        onrowdatabound="AddressGridView_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <ul>
                        <li id="linename" runat="server"></li>
                        <li id="lineone" runat="server"></li>
                        <li id="linetwo" runat="server"></li>
                        <li id="linethree" runat="server"></li>
                        <li id="linefour" runat="server"></li>
                    </ul>
                </ItemTemplate>                
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:ImageButton ID="BillToAddressImageButton" runat="server" ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/BillTo.png" CommandName="BillTo" CausesValidation="false" AlternateText="Bill To" EnabledDuringCallBack="false" />
                    <asp:ImageButton ID="ShipToAddressImageButton" runat="server" ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/ShipTo.png" CommandName="ShipTo" CausesValidation="false" AlternateText="Ship To" EnabledDuringCallBack="false" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>    
</asp:Panel>