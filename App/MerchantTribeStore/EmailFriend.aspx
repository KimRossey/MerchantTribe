<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.EmailFriend" title="Email a Friend" Codebehind="EmailFriend.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
			<div id="popupContainer">
				<asp:Panel ID="pnlMain" Runat="server" Visible="False">
					<H3>Email Page to a Friend</H3>
					<asp:Label id="lblErrorMessage" Runat="server" CssClass="errormessage" Visible="false" ></asp:Label>
					<asp:ValidationSummary id="valSummary" Runat="server" ForeColor="" EnableClientScript="True" CssClass="errormessage"></asp:ValidationSummary>					
					<strong><asp:Label id="lblResults" Runat="server" CssClass="SuccessMessage"></asp:Label></strong>
					<TABLE cellSpacing="0" cellPadding="5" width="100%" border="0">
						<TR>
							<TD class="FormLabel" vAlign="top" align="right">Your Email:</TD>
							<TD vAlign="top" align="left">
								<asp:Textbox id="FromEmailField" runat="server" Columns="30"></asp:Textbox>&nbsp;
								<bvc5:BVRequiredFieldValidator id="Requiredfieldvalidator1" Visible="True" Runat="server" ForeColor=" " EnableClientScript="True"
									CssClass="errormessage" ErrorMessage="Please enter an email address" Display="Dynamic" ControlToValidate="FromEmailField">*</bvc5:BVRequiredFieldValidator>
								<bvc5:BVRegularExpressionValidator id="Regularexpressionvalidator1" Runat="server" ForeColor=" " CssClass="errormessage"
									ErrorMessage="Please enter a valid email address" Display="Dynamic" ControlToValidate="FromEmailField" ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"></bvc5:BVRegularExpressionValidator></TD>
						</TR>
						<TR>
							<TD class="FormLabel" vAlign="top" align="right">Friend's Email:</TD>
							<TD vAlign="top" align="left">
								<asp:Textbox id="toEmailField" runat="server" Columns="30"></asp:Textbox>&nbsp;
								<bvc5:BVRequiredFieldValidator id="valEmail" Visible="True" Runat="server" ForeColor=" " EnableClientScript="True"
									CssClass="errormessage" ErrorMessage="Please enter an email address" Display="Dynamic" ControlToValidate="ToEmailField">*</bvc5:BVRequiredFieldValidator>
								<bvc5:BVRegularExpressionValidator id="valEmail2" Runat="server" ForeColor=" " CssClass="errormessage" ErrorMessage="Please enter a valid email address"
									Display="Dynamic" ControlToValidate="ToEmailField" ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"></bvc5:BVRegularExpressionValidator></TD>
						</TR>
						<TR>
							<TD vAlign="top" align="left">&nbsp;</TD>
							<TD vAlign="top" align="left">
								<asp:ImageButton id="btnSend" runat="server" 
                                    ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/submit.png" 
                                    onclick="btnSend_OnClick"></asp:ImageButton></TD>
						</TR>
					</TABLE>
					<asp:Textbox id="inMessage" runat="server" Visible="False" Columns="50"></asp:Textbox>
				</asp:Panel>
				<asp:Panel ID="pnlRegister" Runat="server" Visible="False">
				You must be a registered user of this site to send email.
				</asp:Panel>
				<div style="TEXT-ALIGN:center"><a href="javascript: self.close()">Close Window</a></div>
			</div>
</asp:Content>

