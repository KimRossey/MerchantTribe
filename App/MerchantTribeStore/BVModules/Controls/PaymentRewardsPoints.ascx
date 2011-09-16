<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentRewardsPoints.ascx.cs" Inherits="BVCommerce.BVModules.Controls.PaymentRewardsPoints" %>
<asp:Label ID="lblPointsAvailable" runat="server"></asp:Label><br />
<asp:RadioButton ID="rbNoPoints" Checked="true" GroupName="RewardsPoints" Text="Do Not Use Points" runat="server" /><br />
<asp:RadioButton ID="rbUsePoints" GroupName="RewardsPoints" Text="Use Points" runat="server" />

