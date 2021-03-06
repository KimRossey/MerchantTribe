		
SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)

-- Add rows to [dbo].[bvc_SiteTerm]
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'AccountLocked', N'Account Locked. Contact Administrator')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'AddressBook', N'Address Book')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'AffiliateReport', N'Affiliate Report')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'AffiliateSignup', N'Affiliate Signup')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'AffiliateTermsAndConditions', N'Affiliate Terms And Conditions')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'AverageRating', N'Average Rating')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'BabyRegistry', N'Baby Registry')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'BreadcrumbTrailSeparator', N'&nbsp;::&nbsp;')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CartBackOrdered', N'Item(s) In Your Cart Are Back-Ordered.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CartNotEnoughQuantity', N'Only %Quantity% of %ProductName% is available for purchase at the moment. Please reduce the number ordered and update to checkout.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CartOutOfStock', N'%ProductName% Is Out Of Stock. Please Remove It.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CartPageMinimumQuantityError', N'Product %ProductName% has a minimum purchase quantity of %quantity%. Amount Adjusted.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Categories', N'Categories')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Category', N'Category')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ChangeEmail', N'Change Email')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ChangePassword', N'Change Password')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Checkout', N'Checkout')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ConfirmEmail', N'Confirm Email')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ConfirmNewPassword', N'Confirm New Password')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ConfirmNewUsername', N'Confirm New Email Address')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ConfirmPassword', N'Confirm Password')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ContactUs', N'Contact Us')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CouponDoesNotApply', N'Coupon code does not apply to this order.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CreateANewAccount', N'Create New Account')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CreateNewAddress', N'Create New Address')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CrossSellTitle', N'Additional Product Accessories')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CustomerReviews', N'Customer Reviews')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'CustomerService', N'Customer Service')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'DownloadFiles', N'Download Files')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'EditAddress', N'Edit Address')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'EmptyCart', N'Your Cart is Empty')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ErrorPageContentTextCategory', N'An error occurred while trying to find the specified category.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ErrorPageContentTextGeneric', N'An error occurred while trying to find the specified page.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ErrorPageContentTextProduct', N'An error occurred while trying to find the specified product.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ErrorPageHeaderTextCategory', N'Error finding category')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ErrorPageHeaderTextGeneric', N'Error finding page')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ErrorPageHeaderTextProduct', N'Error finding product')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'FAQ', N'Frequently Asked Questions')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'First', N'First')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ForgotPassword', N'Forgot Password')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'GoogleCheckoutCustomerError', N'')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Help', N'Help')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Home', N'Home')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ImageExtensionError', N'Images must be .jpg .gif or .png')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ItemFound', N'item found')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ItemsFound', N'items found')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Last', N'Last')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'LineItemsChanged', N'Line Items In Your Cart Were Modified Due To Low Stock.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ListPrice', N'List Price')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Login', N'Sign In')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'LoginIncorrect', N'Login incorrect, please try again.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Logout', N'Sign Out')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'LowStockLineItem', N'Item stock is lower than quantity requested.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'MailingList', N'Mailing List')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'MailingLists', N'Mailing Lists')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'MakeAnyChangesAbove', N'Make any changes above?')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'MyAccount', N'Your Account')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'NewEmail', N'New Email')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'NewPassword', N'New Password')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'NewUsername', N'New Email Address')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Next', N'Next')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'NoShippingRequired', N'No Shipping Required.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'NoValidShippingMethods', N'No Valid Shipping Methods Found.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'OrderAlreadyPlaced', N'Order has already been placed, or no cart exists.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'OrderDetails', N'Order Details')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'OrderHistory', N'Order History')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'OutOfStock', N'This Item is Out of Stock')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'OutOfStockAllowPurchase', N'Item is backordered.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'OutOfStockNoPurchase', N'Item is out of stock.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Page', N'Page')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Password', N'Password')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'PasswordAnswer', N'Password Answer')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'PasswordHint', N'Password Hint')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'PaypalCheckoutCustomerError', N'')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Previous', N'Prev')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'PrivacyPolicy', N'Privacy Policy')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'PrivateStoreNewUser', N'Need an account? Contact us.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Product', N'Product')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ProductCombinationNotAvailable', N'Currently selected product is not available.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ProductNotAvailable', N'%ProductName% is not available.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ProductPageMinimumQuantityError', N'Product Has A Minimum Purchase Quantity of %Quantity%')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Products', N'Products')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Quantity', N'Qty')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'QuantityChanged', N'Item''s Quantity Was Modified Due To Low Stock.')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'RecentlyViewedItems', N'Recently Viewed Items')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'RelatedItems', N'Related Items')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'RememberUser', N'Remember Me')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ReturnForm', N'Return Form')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ReturnPolicy', N'Return Policy')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Search', N'Search')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ShippingTermsAndConditions', N'Shipping Policy')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ShippingUnknown', N'To Be Determined. Contact Store for Details')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ShoppingCart', N'Shopping Cart')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'SiteMap', N'Site Map')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'SitePrice', N'Your Price')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'SiteTermsAgreementError', N'You Must Agree To The Site Terms Before You Can Proceed')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'SKU', N'SKU')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'SortOrder', N'Sort Order')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'SuggestedItems', N'Customers who purchased this item also purchased these items:')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'TermsAndConditions', N'Terms and Conditions')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'TermsAndConditionsAgreement', N'I Agree To This Sites Terms And Conditions')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Themes', N'Themes')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'UpSellTitle', N'Additional Product Information')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'Username', N'Username')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ValidatorFieldMarker', N'*')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ViewAll', N'&nbsp;View All')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ViewByPages', N'View By Pages')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'ViewCart', N'View Cart')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'WasThisReviewHelpful', N'Was this review helpful?')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'WeddingRegistry', N'Wedding Registry')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'WishList', N'Wish List')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'WriteAReview', N'Write a Review?')
INSERT INTO [dbo].[bvc_SiteTerm] ([SiteTerm], [SiteTermValue]) VALUES (N'YouSave', N'You Save')
-- Operation applied to 113 rows out of 113

-- Add row to [dbo].[bvc_WebAppSetting]
INSERT INTO [dbo].[bvc_WebAppSetting] ([SettingName], [SettingValue]) VALUES (N'Cryptography3DesKey', N'EDBE6BF8A92A417cBCD3DB23120861B5DE780BA44DB44166888707607A2A16FBBADFD3E111D54396A5701CE43E0EC3FFAE5543370AF54228B65CB87D7E346048')

-- Add rows to [dbo].[bvc_ContentColumn]
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('1                                   ', N'System Homepage 1', 1, '2005-09-07 22:50:35.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('101', N'System Admin Dashboard 1', 1, '2006-04-13 12:14:58.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('102', N'System Admin Dashboard 2', 1, '2006-04-13 12:15:11.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('103', N'System Admin Dashboard 3', 1, '2006-04-13 12:15:20.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('2                                   ', N'System Homepage 2', 1, '2005-09-07 22:50:35.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('201', N'System Account Pages', 1, '2006-05-16 15:47:56.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('202', N'System Service Pages', 1, '2006-05-15 17:40:25.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('3                                   ', N'System Homepage 3', 1, '2005-09-07 22:50:35.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('4                                   ', N'System Category Page', 1, '2005-09-07 22:50:35.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('5                                   ', N'System Product Page', 1, '2005-09-07 22:50:35.000', 0)
INSERT INTO [dbo].[bvc_ContentColumn] ([bvin], [DisplayName], [SystemColumn], [LastUpdated], [StoreId]) VALUES ('601', N'Checkout Page', 1, '2010-01-01 00:00:00.000', 0)
-- Operation applied to 11 rows out of 11

-- Add rows to [dbo].[bvc_HtmlTemplates]
SET IDENTITY_INSERT [dbo].[bvc_HtmlTemplates] ON
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (26, 0, '2011-02-19 16:12:17.810', N'Contact Us Form', N'[[Store.ContactEmail]]', N'Contact Form From [[Store.StoreName]]', N'<h1>Contact Form from [[Store.StoreName]]</h1>', N'', 102)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (27, 0, '2011-02-25 21:43:54.210', N'Drop Shipper Notice', N'[[Store.ContactEmail]]', N'Drop Ship Request: [[Order.OrderNumber]]', N'<html>
 <head>
 <style type="text/css"> A { text-decoration: none; }
	A:link { color: #3366cc; text-decoration: none; }
	A:visited { color: #663399; text-decoration: none; }
	A:active { color: #cccccc; text-decoration: none; }
	A:Hover { text-decoration: underline; }
	BODY, TD, CENTER, P { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.body { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.content { font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal; color: #000000; }
	.title { font-family: Helvetica, Arial, sans-serif; font-size: 10px; font-weight: normal; color: #000000; }
	.headline { font-family: Helvetica, Arial, sans-serif; font-size: 14px; font-weight: bold; color: #000000; }
	.message { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 9px; }
	</style>
 </head>
<body bgcolor="#ffffff" LINK="#3366cc" VLINK="#3366cc" ALINK="#3366cc" LEFTMARGIN="0" TOPMARGIN="0">
	<table cellSpacing=1 cellPadding=3 width="100%" border="0" runat="server">
	<tr>
		<td colSpan=2>[[Store.Logo]]</td>
	</tr>
		<TR>
		<TD class=FormLabel vAlign=top align=left width="50%">
		<b>Billed To:</b><br>
		<br>[[Order.BillingAddress]]
		[[Order.UserName]]
		</TD>
		<TD class=FormLabel vAlign=top align=left width="50%">
			<b>Order Number:</b> [[Order.OrderNumber]]<BR>
			<b>Order Time:</b> [[Order.TimeOfOrder]]<br>
			<b>Current Status:</b> [[Order.Status]]&nbsp;<BR>
			<b>Special Instructions:</b> [[Order.Instructions]]&nbsp;<br>
					</TD>		
	</TR>
	<tr>
		<td colspan="2">
			<table border="0" cellspacing="0" cellpadding="5" width="100%">
			<tr>
				<td colspan="3"><hr></td>
			</tr>
			<tr>
                                <td><b>Qty</b></td>
				<td><b>SKU</b></td>
				<td><b>Product Name</b></td>
			</tr>
			<tr>
				<td colspan="3"><hr></td>
			</tr>
			[[RepeatingSection]]
			<tr>
				<td colspan="3"><hr></td>
			</tr>
		</table>
				
		</td>
	</tr>
	<tr>
		<td colspan="2"><b>Please retain for your records.</b></td>
	</tr>						
</table>
</body>
</html>', N'<tr><td align=left valign=top>[[LineItem.Quantity]]</td><td align=left valign=top>[[LineItem.Sku]]</td><td align=left valign=top>[[LineItem.ProductName]]<br />[[LineItem.ProductDescription]]<br />[[LineItem.ShippingStatus]]</td></tr>', 200)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (28, 0, '2011-02-25 21:44:15.130', N'Email Friend', N'[[Store.ContactEmail]]', N'A friend has sent you a link from [[Store.StoreName]]', N'<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN"
        "http://www.w3.org/TR/html4/strict.dtd">
<html lang="en">
<head>
<style type="text/css" media="screen,print">
	html,
	body {
		margin:0;
		padding:0;
		font-family:"Trebuchet MS", Georgia, Verdana, serif;
		color:#000;
		background:#fff;
	}
	body {
		min-width:720px;
		text-align:center;
	}
	h1 {
		padding:0;
		margin:0;
		font-size:16px;
	}
	p {
		margin:9px 0 0 0;
		padding:0;
		font-size:12px;
		line-height:18px;
	}
	p+p {
		font-style:italic;
		font-size:11px;
	}
	#centerwrap {
		background:#ccc;
		width:720px;
		margin:10px auto;
		padding:10px 0;
		text-align:left;
	}
	#content {
		background:#fff;
		margin:0 10px;
		padding:10px;
	}
    .bottombox {
	    text-align:center;
	    padding:10px;
	    background-color:BlanchedAlmond;
	    border:1px solid black;
	    font-size:10px;
	    width: 200px;
        margin-left: auto;
        margin-right: auto;
	}
	
	#footer {
		text-align:center;
		clear:both;
	}
	</style>
</head>
<body>
<div id="centerwrap">
[[Store.Logo]] 
	<div id="content">
		<h1>Tell a Friend!</h1>
		<p>Greetings!  Your friend has suggested you look at this great deal from [[Store.StoreName]].&nbsp;
            Click on the image below to view this product in our store.</p>
        <p>
            <a href=''[[Store.StandardUrl]][[Product.ProductURL]]''>
               <IMG src=''[[Store.StandardUrl]][[Product.ImageFileSmall]]'' width=''120'' height=''120'' border=''0''>
            </a>
            <br />[[Product.LongDescription]]
            <br />[[Product.SitePrice]]
        </p>
        <p>
            Don''t worry, this e-mail is the only thing you will receive from us unless you subscribe to our [[Store.StoreName]] newsletter.
            This product, along with many others just like it can be found at our store.  These prices won''t last much longer, begin your shopping today.<br />
        </p>
    <div class="bottombox">
       <a href="[[Store.StandardUrl]]">CLICK HERE TO START SHOPPING.</a>
    </div>

</div>
<div id="footer">
	<a href="[[Store.StandardUrl]]">[[Store.StoreName]]</a>
</div>
</div>
</body>
</html>', N'', 101)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (29, 0, '2011-02-25 21:44:28.593', N'Forgot Password Template', N'[[Store.ContactEmail]]', N'Your password has been reset.', N'[[Store.Logo]]<br />
<font face="Arial">Your password has been reset.</font>
<br />&nbsp;<br />
<font face="Arial">Username: <strong>[[User.UserName]]<br />
</strong>Password: <strong>[[NewPassword]]
</strong>
<br />
<br />
You can login with your new information here: [[Store.StoreName]]&nbsp;[[Store.CurrentLocalTime]]
</font>', N'', 100)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (30, 0, '2011-02-25 21:44:38.073', N'Order New Receipt', N'[[Store.ContactEmail]]', N'Receipt for Order [[Order.OrderNumber]] from [[Store.StoreName]]', N'<html>
 <head>
 <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
<style type="text/css"> A { text-decoration: none; }
	A:link { color: #3366cc; text-decoration: none; }
	A:visited { color: #663399; text-decoration: none; }
	A:active { color: #cccccc; text-decoration: none; }
	A:Hover { text-decoration: underline; }
	BODY, TD, CENTER, P { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.body { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.content { font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal; color: #000000; }
	.title { font-family: Helvetica, Arial, sans-serif; font-size: 10px; font-weight: normal; color: #000000; }
	.headline { font-family: Helvetica, Arial, sans-serif; font-size: 14px; font-weight: bold; color: #000000; }
	.message { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 9px; }
	</style>
 </head>
<body bgcolor="#ffffff" LINK="#3366cc" VLINK="#3366cc" ALINK="#3366cc" LEFTMARGIN="0" TOPMARGIN="0">
	<table cellSpacing=1 cellPadding=3 width="100%" border="0" runat="server">
	<tr>
		<td colSpan=2>[[Store.Logo]]</td>
	</tr>
	<TR>
		<TD class=FormLabel vAlign=top align=left width="50%">
		<b>Billed To:</b><br>
		[[Order.BillingAddress]]<br>
		[[Order.UserName]]
		</TD>
		<TD class=FormLabel vAlign=top align=left width="50%">
			<b>Order Number:</b> [[Order.OrderNumber]]<BR>
			<b>Order Time:</b> [[Order.TimeOfOrder]]<br>
			<b>Current Status:</b> [[Order.Status]]&nbsp;<BR>			
			<b>Promotional Code(s):</b> [[Order.Coupons]]&nbsp;<br>
			<b>Special Instructions:</b> [[Order.Instructions]]&nbsp;<br>			
		</TD>		
	</TR>
	<tr>
		<td colspan="2">
			<table border="0" cellspacing="0" cellpadding="5" width="100%">
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			<tr>
                                <td><b>Qty</b></td>
				<td><b>SKU</b></td>
				<td><b>Product Name</b></td>
				<td align="right"><b>Unit Price</b></td>
			</tr>
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			[[RepeatingSection]]
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			<tr>
				<td colspan="4" align="right">
[[Order.TotalsAsTable]]					
				</td>
			</tr>
			<tr>
				<td colspan="4"><hr></td>
			</tr>
		</table>	
		</td>
	</tr>
	<tr>
		<td colspan="2"><b>Please retain for your records.</b></td>
	</tr>						
</table>
</body>
</html>', N'<tr><td align=left valign=top>[[LineItem.Quantity]]</td><td align=left valign=top>[[LineItem.Sku]]</td><td align=left valign=top>[[LineItem.ProductName]]<br />[[LineItem.ProductDescription]]<br />[[LineItem.ShippingStatus]]</td><td align=right valign=top>[[LineItem.AdjustedPrice]]</td></tr>', 1)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (31, 0, '2011-02-25 21:43:40.237', N'Admin Order Receipt', N'[[Store.ContactEmail]]', N'New Order [[Order.OrderNumber]] Received [[Site.SiteName]]', N'<html>
 <head>
 <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
<style type="text/css"> A { text-decoration: none; }
    A:link { color: #3366cc; text-decoration: none; }
    A:visited { color: #663399; text-decoration: none; }
    A:active { color: #cccccc; text-decoration: none; }
    A:Hover { text-decoration: underline; }
    BODY, TD, CENTER, P { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
    .body { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
    .content { font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal; color: #000000; }
    .title { font-family: Helvetica, Arial, sans-serif; font-size: 10px; font-weight: normal; color: #000000; }
    .headline { font-family: Helvetica, Arial, sans-serif; font-size: 14px; font-weight: bold; color: #000000; }
    .message { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 9px; }
    </style>
 </head>
<body bgcolor="#ffffff" LINK="#3366cc" VLINK="#3366cc" ALINK="#3366cc" LEFTMARGIN="0" TOPMARGIN="0">
    <table cellSpacing=1 cellPadding=3 width="100%" border="0" runat="server">
    <tr>
    	<td colSpan=2>[[Store.Logo]]</td>
    </tr>
    <tr>
    	<td class=FormLabel colSpan=2>
                <hr><a href=[[Order.AdminLink]]>View Order Details</a><br>
                &nbsp;
    	</td>
    </tr>
    <TR>
    	<TD class=FormLabel vAlign=top align=left width="50%">
    	<b>Billed To:</b><br>
    	[[Order.BillingAddress]]<br>
    	[[Order.UserName]]
    	</TD>
    	<TD class=FormLabel vAlign=top align=left width="50%">
    		<b>Order Number:</b> [[Order.OrderNumber]]<BR>
    		<b>Order Time:</b> [[Order.TimeOfOrder]]<br>
    		<b>Current Status:</b> [[Order.Status]]&nbsp;<BR>
    		<b>Promotional Code(s):</b> [[Order.Coupons]]&nbsp;<br>
    		<b>Special Instructions:</b> [[Order.Instructions]]&nbsp;<br>
    	</TD>
    </TR>
    <tr>
    	<td colspan="2">
    		<table border="0" cellspacing="0" cellpadding="5" width="100%">
    		<tr>
    			<td colspan="4"><hr></td>
    		</tr>
    		<tr>
                                <td><b>Qty</b></td>
    			<td><b>SKU</b></td>
    			<td><b>Product Name</b></td>
    			<td align="right"><b>Unit Price</b></td>
    		</tr>
    		<tr>
    			<td colspan="4"><hr></td>
    		</tr>
    		[[RepeatingSection]]
    		<tr>
    			<td colspan="4"><hr></td>
    		</tr>
    		<tr>
    			<td colspan="4" align="right">
    				[[Order.TotalsAsTable]]
    			</td>
    		</tr>
    		<tr>
    			<td colspan="4"><hr></td>
    		</tr>
    	</table>
    	</td>
    </tr>
    <tr>
    	<td colspan="2"><b>Please retain for your records.</b></td>
    </tr>
</table>
</body>
</html>', N'<tr>  <td align=left valign=top>[[LineItem.Quantity]]</td>  <td align=left valign=top>[[LineItem.Sku]]</td>  <td align=left valign=top>[[LineItem.ProductName]]<br />[[LineItem.ProductDescription]]<br />[[LineItem.ShippingStatus]]</td><td align=right valign=top>[[LineItem.AdjustedPrice]]</td></tr>', 2)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (32, 0, '2011-02-25 21:44:47.287', N'Order Shipment', N'[[Store.ContactEmail]]', N'Shipping update for order [[Order.OrderNumber]] from [[Store.StoreName]]', N'<html>
 <head>
<style type="text/css"> A { text-decoration: none; }
	A:link { color: #3366cc; text-decoration: none; }
	A:visited { color: #663399; text-decoration: none; }
	A:active { color: #cccccc; text-decoration: none; }
	A:Hover { text-decoration: underline; }
	BODY, TD, CENTER, P { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.body { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.content { font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal; color: #000000; }
	.title { font-family: Helvetica, Arial, sans-serif; font-size: 10px; font-weight: normal; color: #000000; }
	.headline { font-family: Helvetica, Arial, sans-serif; font-size: 14px; font-weight: bold; color: #000000; }
	.message { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 9px; }
        .packageitems { border-collapse: collapse; }
        .itemnamehead { background-color: #999999; }
        .itemquantityhead { background-color: #999999; }
        .alt .itemname { background-color: #aaaaaa; }
        .alt .itemquantity { background-color: #aaaaaa; }
        .itemname { background-color: #ffffff; }
        .itemquantity { background-color: #ffffff; text-align: right; border-left: solid 1px #aaaaaa; }
	</style>
 </head>
<body bgcolor="#ffffff" LINK="#3366cc" VLINK="#3366cc" ALINK="#3366cc" LEFTMARGIN="0" TOPMARGIN="0">
	<table cellSpacing=1 cellPadding=3 width="100%" border="0" runat="server">
	<tr>
		<td colSpan=2>[[Store.Logo]]</td>
	</tr>	
	<TR>
		<TD class=FormLabel vAlign=top align=left width="50%">
		<b>Billed To:</b><br>
		[[Order.BillingAddress]]<br>
		[[Order.UserName]]
		</TD>
		<TD class=FormLabel vAlign=top align=left width="50%">
			<b>Order Number:</b> [[Order.OrderNumber]]<BR>
			<b>Order Time:</b> [[Order.TimeOfOrder]]<br>
			<b>Current Status:</b> [[Order.Status]]&nbsp;<BR>
			<b>Payment Method:</b> [[Order.PaymentMethod]]&nbsp;<br>
			<b>Promotional Code(s):</b> [[Order.Coupons]]&nbsp;<br>
			<b>Special Instructions:</b> [[Order.Instructions]]&nbsp;<br>			
		</TD>		
	</TR>
	<tr>
		<td colspan="2">
			<table border="0" cellspacing="0" cellpadding="5" width="100%">
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			<tr>
                                <td><b>Shipper</b></td>
				<td><b>Shipping Method</b></td>
				<td><b>Tracking Number</b></td>
				<td><b>Ship Date</b></td>
			</tr>
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			[[RepeatingSection]]
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			<tr>
				<td colspan="4" align="right">
					[[Order.TotalsAsTable]]
				</td>
			</tr>
			<tr>
				<td colspan="4"><hr></td>
			</tr>
		</table>			
		</td>
	</tr>
	<tr>
		<td colspan="2"><b>Please retain for your records.</b></td>
	</tr>						
</table>
</body>
</html>', N'<tr>
  <td align=left valign=top>[[Package.ShipperName]]</td>
  <td align=left valign=top>[[Package.ShipperService]]</td>
  <td align=left valign=top><a href="[[Package.TrackingNumberLink]]">[[Package.TrackingNumber]]</a></td>    
  <td align=left valign=top>[[Package.ShipDate]]</td>
</tr>
<tr>
  <td></td>
  <td colspan="3" align=left valign=top>[[Package.Items]]</td>
</tr>', 4)
INSERT INTO [dbo].[bvc_HtmlTemplates] ([Id], [StoreId], [LastUpdatedUtc], [DisplayName], [FromEmail], [Subject], [Body], [RepeatingSection], [TemplateType]) VALUES (33, 0, '2011-02-25 21:44:56.053', N'Order Update', N'[[Store.ContactEmail]]', N'Order [[Order.OrderNumber]] Update from [[Store.StoreName]]', N'<html>
 <head>
 <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
<style type="text/css"> A { text-decoration: none; }
	A:link { color: #3366cc; text-decoration: none; }
	A:visited { color: #663399; text-decoration: none; }
	A:active { color: #cccccc; text-decoration: none; }
	A:Hover { text-decoration: underline; }
	BODY, TD, CENTER, P { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.body { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 10px; color: #333333; }
	.content { font-family: Arial, Helvetica, sans-serif; font-size: 11px; font-weight: normal; color: #000000; }
	.title { font-family: Helvetica, Arial, sans-serif; font-size: 10px; font-weight: normal; color: #000000; }
	.headline { font-family: Helvetica, Arial, sans-serif; font-size: 14px; font-weight: bold; color: #000000; }
	.message { font-family: Geneva, Verdana, Arial, Helvetica; font-size: 9px; }
        .trackingnumberlist {list-style:none;}
        .trackingnumberlinklist {list-style:none;}
	</style>
 </head>
<body bgcolor="#ffffff" LINK="#3366cc" VLINK="#3366cc" ALINK="#3366cc" LEFTMARGIN="0" TOPMARGIN="0">
	<table cellSpacing=1 cellPadding=3 width="100%" border="0" runat="server">
	<tr>
		<td colSpan=2>[[Store.Logo]]</td>
	</tr>	
	<TR>
		<TD class=FormLabel vAlign=top align=left width="50%">
		<b>Billed To:</b><br>
		[[Order.BillingAddress]]<br>
		[[Order.UserName]]
		</TD>
		<TD class=FormLabel vAlign=top align=left width="50%">
			<b>Order Number:</b> [[Order.OrderNumber]]<BR>
			<b>Order Time:</b> [[Order.TimeOfOrder]]<br>
			<b>Current Status:</b> [[Order.Status]]&nbsp;<BR>			
			<b>Promotional Code(s):</b> [[Order.Coupons]]&nbsp;<br>
			<b>Special Instructions:</b> [[Order.Instructions]]&nbsp;<br>			
		</TD>		
	</TR>
	<tr>
		<td colspan="2">
			<table border="0" cellspacing="0" cellpadding="5" width="100%">
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			<tr>
                                <td><b>Qty</b></td>
				<td><b>SKU</b></td>
				<td><b>Product Name</b></td>
				<td align="right"><b>Unit Price</b></td>
			</tr>
			<tr>
				<td colspan="4"><hr></td>
			</tr>
			[[RepeatingSection]]
			<tr>
				<td colspan="4"><hr></td>
			</tr>
                        <tr>
				<td colspan="4">Tracking Numbers:</td>
			</tr>
                        <tr>
				<td colspan="4">[[Order.TrackingNumberLinks]]</td>
			</tr>
			<tr>
				<td colspan="4" align="right">
					[[Order.TotalsAsTable]]
				</td>
			</tr>
			<tr>
				<td colspan="4"><hr></td>
			</tr>
		</table>			
		</td>
	</tr>
	<tr>
		<td colspan="2"><b>Please retain for your records.</b></td>
	</tr>						
</table>
</body>
</html>', N'<tr><td align=left valign=top>[[LineItem.Quantity]]</td><td align=left valign=top>[[LineItem.Sku]]</td><td align=left valign=top>[[LineItem.ProductName]]<br />[[LineItem.ProductDescription]]<br />[[LineItem.ShippingStatus]]</td><td align=right valign=top>[[LineItem.AdjustedPrice]]</td></tr>', 3)
SET IDENTITY_INSERT [dbo].[bvc_HtmlTemplates] OFF
-- Operation applied to 8 rows out of 16
