using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace BVCommerce
{

    public partial class signup_features : System.Web.UI.Page
    {
        private class FeatureDetails
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageId { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "BV Commerce Features | Online Stores & Hosted Shopping Carts";
            this.MetaDescription = "BV Commerce Shopping Cart Features for Small Business Ecommerce.";
            this.MetaKeywords = "BVCommerce,BV Commerce,Features,Shopping Cart,Ecommerce,Business,Benefits";
            LoadLists();
        }

        private void LoadLists()
        {
            this.litFeatures.Text = string.Empty;
            RenderList("Categories", GetCategories());
            RenderList("Products", GetProducts());
            //RenderList("Marketing", GetMarketing());
            //RenderList("Development", GetDevelopment());
            RenderList("Search", GetSearch());
            RenderList("Customers", GetCustomers());
            RenderList("Loyalty", GetLoyalty());
            RenderList("Affiliates", GetAffiliateProgram());
            RenderList("Content Management", GetContentManagement());
            RenderList("Order Management", GetOrderManagement());
            RenderList("Taxes", GetTaxes());
            RenderList("Payment", GetPayment());
            RenderList("Shipping", GetShipping());
            RenderList("Security", GetSecurity());
            RenderList("Design", GetDesign());
            RenderList("Reports", GetReports());
            //RenderList("Integration", GetIntegration());
        }

        private void RenderList(string title, List<FeatureDetails> features)
        {
            StringBuilder sb = new StringBuilder();
            if (features != null)
            {
                sb.Append("<h3 class=\"featurelist\">" + title + "</h3><ul class=\"featurelist\">");
                foreach (FeatureDetails f in features)
                {
                    sb.Append("<li><strong>" + f.Name + "</strong><br />" + f.Description + "</li>");
                }
                sb.Append("</ul>");
                this.litFeatures.Text += sb.ToString();
            }
        }

        private List<FeatureDetails> GetCategories()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();

            result.Add(new FeatureDetails() { Name = "Drill-Down Categories", Description = "Help customers filter through thousands of products in seconds with drill-down categories. Assign properties like TV Screen Size or Year, Make and Model. BV Commerce automatically generates the search category pages." });
            result.Add(new FeatureDetails() { Name = "Unlimited Categories", Description = "Categorize your products any way you like whether you have 10 products, 10 thousand or 100 thousand.", ImageId = "A1" });
            result.Add(new FeatureDetails() { Name = "Unlimited Sub-Categories", Description = "Create departments and segment your products so customers can easily find what they're looking for. ", ImageId = "A2" });
            //result.Add(new FeatureDetails() { Name = "5 Built-in Templates + Unlimited Plug-ins", Description = "Choose from our included designs or easily add your own! Unlike in other software packages, you can have different page layouts for each category and subcategory. This is an amazing feature for merchants who sell different kinds of items on one store. ", ImageId = "A3" });
            //result.Add(new FeatureDetails() { Name = "Dynamic Categories", Description = "Smart categories update their products when you update your catalog. Create categories to automatically show your latest products, products from specific manufacturers and price ranges. ", ImageId = "A4" });            
            result.Add(new FeatureDetails() { Name = "Drag and Drop Sorting for Products in Categories", Description = "Arrange your products in categories in any order at any time to help shoppers find what they're looking for.", ImageId = "A5" });
            result.Add(new FeatureDetails() { Name = "Automatic Paging", Description = "When your categories get large we automatically break them up into pages for easy viewing and searching.", ImageId = "A6" });
            //result.Add(new FeatureDetails() { Name = "Grid View", Description = "Display products with large images for quick browsing and shopping.", ImageId = "A7" });
            //result.Add(new FeatureDetails() { Name = "List View", Description = "Show a quick text list for displaying many products at once or groups of similar products.", ImageId = "A8" });
            //result.Add(new FeatureDetails() { Name = "Details View", Description = "Display all your product information, including &quot;add to cart&quot; buttons on a single page, saving your customers a click or two. ", ImageId = "A9" });
            //result.Add(new FeatureDetails() { Name = "Category + Sub Category List", Description = "Display an intuitive directory of your top categories and departments with thumbnail images. ", ImageId = "A10" });
            //result.Add(new FeatureDetails() { Name = "Sub Category Navigation Template", Description = "Allow shoppers to quickly refine their product searches with a list of departments or subcategories available for shopping. ", ImageId = "A11" });
            result.Add(new FeatureDetails() { Name = "Custom Banners", Description = "Bring your categories and departments to life with custom banner graphics that set the mood! Easily upload banner images in our intuitive admin interface.", ImageId = "A12" });
            result.Add(new FeatureDetails() { Name = "Custom Icons", Description = "Display categories and departments in your products grids with custom icons.", ImageId = "A13" });
            result.Add(new FeatureDetails() { Name = "Html Content Before and After Templates", Description = "Customize your category pages with your own HTML. Insert flash, product manuals, custom links and more--all from our admin interface!", ImageId = "A14" });
            result.Add(new FeatureDetails() { Name = "Html Descriptions", Description = "Insert your own HTML in your description of categories. Include bolded words, hyperlinks and images for a totally custom experience. ", ImageId = "A15" });
            result.Add(new FeatureDetails() { Name = "Per-Category Meta Tags", Description = "Help shoppers find your store with awesome search engine friendly meta tags customized for EACH category. Other carts limit you to the same tag for every page in your store, making it difficult for the search engines to find your categories.  ", ImageId = "A16" });
            //result.Add(new FeatureDetails() { Name = "Show Count of Items Inside Category", Description = "Show customers how many items are in each category of your store.", ImageId = "A17" });
            result.Add(new FeatureDetails() { Name = "Automatic Breadcrumb Trail", Description = "Keep your shoppers from getting lost. Show them a clickable history of how they arrived at each category. ", ImageId = "A18" });

            return result;
        }
        private List<FeatureDetails> GetProducts()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            //result.Add(new FeatureDetails() { Name = "2 Built-in Product Templates + Unlimited Plug-ins", Description = "Choose from our two product page layouts or easily add your own without custom programming.", ImageId = "B1" });
            //result.Add(new FeatureDetails() { Name = "Clone Products", Description = "Copy existing products to make others, saving you time.", ImageId = "B2" });
            result.Add(new FeatureDetails() { Name = "Customize Product Types and Properties without Programming", Description = "Create a rich product catalog by adding information like &quot;author,&quot; &quot;published date,&quot; &quot;movie rating,&quot; &quot;track list,&quot; and more. In most other carts this requires expensive custom programming!", ImageId = "B3" });
            result.Add(new FeatureDetails() { Name = "Radio Button Choices", Description = "Let customers choose options for your products with radio buttons.", ImageId = "B4" });
            result.Add(new FeatureDetails() { Name = "Dropdown List Choices", Description = "Let customers choose options for your products with dropdown lists.", ImageId = "B5" });
            result.Add(new FeatureDetails() { Name = "Text Input Fields", Description = "Let customers customize your products with information for monograms, business cards, personalized gifts, and more.", ImageId = "B6" });
            result.Add(new FeatureDetails() { Name = "HTML Descriptions", Description = "Insert your own HTML in your description of products. Include bolded words, hyperlinks and images for a totally custom experience. ", ImageId = "B7" });
            result.Add(new FeatureDetails() { Name = "Per-Product Meta Tags", Description = "Help shoppers find your store with awesome search engine friendly meta tags customized for EACH product. Other carts limit you to the same tag for every page in your store making it difficult for the search engines to find your products.  ", ImageId = "B8" });
            result.Add(new FeatureDetails() { Name = "Product Ratings", Description = "Let customers or store owners rate your products with 1 to 5 stars.", ImageId = "B9" });
            result.Add(new FeatureDetails() { Name = "Product Reviews", Description = "Let customers or store owners add reviews for your products. Optionally, hide reviews until a moderator approves them.", ImageId = "B10" });
            //result.Add(new FeatureDetails() { Name = "Short/Long Descriptions", Description = "Provide one description of your product for the category page and a more detailed description for the product page. ", ImageId = "B11" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Images", Description = "Product images sell products. Unlike many other carts, add as many as you like. No limits! ", ImageId = "B12" });
            result.Add(new FeatureDetails() { Name = "Image Swap When Choices Change (i.e. size/color)", Description = "Automatically show the large red shirt picture when a customer selects the large red option. No clicking required with our AJAX enabled product page. ", ImageId = "B13" });
            result.Add(new FeatureDetails() { Name = "Electronic/ Intellectual Goods", Description = "Sell electronic goods or other information products and we'll make sure the downloads are available and no shipping is charged. ", ImageId = "B14" });
            result.Add(new FeatureDetails() { Name = "Drop Ship from Unlimited Vendors", Description = "Assign each product to a warehouse and our intelligent shipping system will automatically figure out the correct shipping cost, even if a customer orders products from different warehouses. ", ImageId = "B15" });
            result.Add(new FeatureDetails() { Name = "Size/Weight Fields", Description = "Enter product size and weight for accurate shipping cost.", ImageId = "B16" });
            result.Add(new FeatureDetails() { Name = "Manufacturer Field", Description = "Assign each product to a manufacturer for drop shipping and easy categorization. ", ImageId = "B17" });
            result.Add(new FeatureDetails() { Name = "Vendor Field", Description = "Assign each product to a vendor for drop shipping and easy categorization. ", ImageId = "B18" });
            result.Add(new FeatureDetails() { Name = "Minimum Quantity", Description = "Require customers to order at least a minimum amount of any product. Great for customized orders like business cards and calendars. ", ImageId = "B19" });
            result.Add(new FeatureDetails() { Name = "Activate/Deactivate", Description = "Turn products off temporarily while you update them or before they're ready to go on sale. ", ImageId = "B20" });
            result.Add(new FeatureDetails() { Name = "MSRP (list price)", Description = "Display suggested retail price for your products if your price is lower. We'll automatically calculate the savings and display them to your customers.", ImageId = "B21" });
            result.Add(new FeatureDetails() { Name = "Site Price", Description = "Set your store price lower than MSRP. ", ImageId = "B22" });
            result.Add(new FeatureDetails() { Name = "Site Cost", Description = "Set your internal cost to keep track of profitability. Coupons can be set to never go below your cost. ", ImageId = "B23" });
            result.Add(new FeatureDetails() { Name = "Tax Exempt Option", Description = "Easily mark items as tax exempt. ", ImageId = "B24" });
            result.Add(new FeatureDetails() { Name = "Tax Classes", Description = "Easily group products for tax rules. For instance, set food products to one set of tax rules and apparel to another.", ImageId = "B25" });
            //result.Add(new FeatureDetails() { Name = "Cross-Sells", Description = "Easily promote accesory products and add-ons. ", ImageId = "B26" });
            //result.Add(new FeatureDetails() { Name = "Up-Sells", Description = "Easily offer upgraded replacement products before the customer gets to the shopping cart. ", ImageId = "B27" });
            //result.Add(new FeatureDetails() { Name = "Auto Suggest Cross-Sells", Description = "Automatically display &quot;customers who bought this also bought.&quot;", ImageId = "B28" });
            result.Add(new FeatureDetails() { Name = "Inventory Tracking By Variants", Description = "Unlike most other e-commerce software, keep track of inventory for products with size, color, and other variations. When you run out of small, red t-shirts, the software can automatically remove it from the store.", ImageId = "B29" });
            result.Add(new FeatureDetails() { Name = "Custom Images, SKUs and Prices for Variants", Description = "Unlike most other e-commerce software, set different prices, pictures, SKUs and more for each size, color, or variation of a product. ", ImageId = "B30" });
            result.Add(new FeatureDetails() { Name = "Unlimited File Downloads Per-Product", Description = "Offer downloadable software, product manuals, documents and more with each product.", ImageId = "B31" });
            //result.Add(new FeatureDetails() { Name = "Batch Editing", Description = "Update multiple products at once from a single batch edit page. ", ImageId = "B32" });
            //result.Add(new FeatureDetails() { Name = "Batch Inventory Editing", Description = "Update inventory levels on multiple products from one screen. A great time saver!", ImageId = "B33" });
            //result.Add(new FeatureDetails() { Name = "Minimum Advertised Price Support (i.e. add to cart for price)", Description = "Hide prices until customers add them to their carts. Display a text message on product and category pages. ", ImageId = "B34" });

            return result;
        }
        private List<FeatureDetails> GetMarketing()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            //result.Add(new FeatureDetails() { Name = "Product Rotators", Description = "Show a different product each time a customer visits your homepage. ", ImageId = "C1" });
            //result.Add(new FeatureDetails() { Name = "Category Rotators", Description = "Show a different category each time a customer visits your home page. ", ImageId = "C2" });
            //result.Add(new FeatureDetails() { Name = "Flash Image Rotators (no programming required)", Description = "Show a slide show of products on your home page without programming!", ImageId = "C3" });
            //result.Add(new FeatureDetails() { Name = "HTML Content Rotators", Description = "Show different content on your home page each time a customer visits. For example, show a different press release or advertisement.", ImageId = "C4" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Pricing Groups", Description = "Provide some customers, like wholesalers, with storewide discounted pricing. No limits!", ImageId = "C5" });
            //result.Add(new FeatureDetails() { Name = "Cross-Sells", Description = "Easily promote accesory products and add-ons. ", ImageId = "C6" });
            //result.Add(new FeatureDetails() { Name = "Cross-Sells on Shopping Cart", Description = "Suggest accesory products after a customer puts an item in the shopping cart. ", ImageId = "C7" });
            //result.Add(new FeatureDetails() { Name = "Auto-Suggested Cross-Sells", Description = "Automatically display &quot;customers who bought this also bought.&quot; ", ImageId = "C8" });
            //result.Add(new FeatureDetails() { Name = "Up-Sells", Description = "Easily offer upgraded replacement products before the customer gets to the shopping cart. ", ImageId = "C9" });
            //result.Add(new FeatureDetails() { Name = "Wish List", Description = "Let customers keep track of items they would like to purchase in the future. ", ImageId = "C10" });
            //result.Add(new FeatureDetails() { Name = "Buy One Get One Free", Description = "Offer customers a free product when they buy one or more other products. ", ImageId = "C11" });
            //result.Add(new FeatureDetails() { Name = "Order Discounts", Description = "Offer customers a percentage or amount off an order with a coupon code or other rules.", ImageId = "C12" });
            //result.Add(new FeatureDetails() { Name = "Shipping Discounts", Description = "Offer customers free or discounted shipping with a coupon code or other rules. ", ImageId = "C13" });
            //result.Add(new FeatureDetails() { Name = "Product Discounts", Description = "Offer customers a percentage or amount off a prouct with a coupon code or other rules. ", ImageId = "C14" });
            //result.Add(new FeatureDetails() { Name = "Discount by % or $", Description = "Flexible discounts save you time when pricing changes. ", ImageId = "C15" });
            //result.Add(new FeatureDetails() { Name = "Number of Uses Limited by User or Store", Description = "Offer discounts to the first 500 customers or 1 time per customer.", ImageId = "C16" });
            //result.Add(new FeatureDetails() { Name = "Coupon/Promotional Codes", Description = "Optionally require coupon codes to qualify for discounts.", ImageId = "C17" });
            //result.Add(new FeatureDetails() { Name = "Limit Offers by Order Total ", Description = "Require customers to order a specific amount before discounts apply. ", ImageId = "C18" });
            //result.Add(new FeatureDetails() { Name = "Limit Offers by Quantity", Description = "Require customers to order a specific number of products before discounts apply. ", ImageId = "C19" });
            //result.Add(new FeatureDetails() { Name = "Custom Discount Plug-ins", Description = "Easily add your own custom discount rules without modifying our source code. ", ImageId = "C20" });
            //result.Add(new FeatureDetails() { Name = "Storewide Sales", Description = "Mark down every product in your store with a single click!", ImageId = "C21" });
            //result.Add(new FeatureDetails() { Name = "Sales by Product", Description = "Offer sales on specific products.", ImageId = "C22" });
            //result.Add(new FeatureDetails() { Name = "Sales by Category", Description = "Offer sales on all products in a specific category. ", ImageId = "C23" });
            //result.Add(new FeatureDetails() { Name = "Sales by Product Type", Description = "Offer sales on all products of a specific type.", ImageId = "C24" });
            //result.Add(new FeatureDetails() { Name = "Fixed Price Sales (i.e. everything is $0.99)", Description = "Mark down all products to a set price. ", ImageId = "C25" });
            result.Add(new FeatureDetails() { Name = "Volume Discounts", Description = "Offer lower prices when a customer orders 10, 20, 30 or more. ", ImageId = "C26" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Mailing Lists", Description = "Create multiple mailing lists to target groups of customers. Offer lists for weekly sales, product updates, general information and more. ", ImageId = "C27" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Email Templates", Description = "Create and save HTML e-mail templates for more effective e-mail marketing. Easily create e-mail templates from the admin!", ImageId = "C28" });
            //result.Add(new FeatureDetails() { Name = "Mailing List Bulk Import/Export", Description = "Import or export your customer list to programs like Constant Contact. We make it easy to port your lists over to other software. ", ImageId = "C29" });
            result.Add(new FeatureDetails() { Name = "Automatic URL Rewriting", Description = "Give your products search engine friendly URLs that are easy for customers to remember. URLs like www.mystore.com/products/large-red-tshirt.aspx make it easy for search engines to send traffic your way! Unlike other ecommerce software, BV Commerce builds these for you automatically!", ImageId = "C30" });
            result.Add(new FeatureDetails() { Name = "Custom URL Rewriting", Description = "For the ultimate control, we make it easy to set your own URLs for every product and category in your store. You can even create custom URLs for other store pages like privacy policies and FAQs. Most other carts don't offer this option at all!", ImageId = "C31" });
            //result.Add(new FeatureDetails() { Name = "BVC2004 Product/Category Link Backward Compatibility", Description = "Customers upgrading from BV Commerce 2004 will find all search engine links to their pages will continue to work in BV Commerce 5.", ImageId = "C32" });
            //result.Add(new FeatureDetails() { Name = "Recently Viewed Products", Description = "Automatically keep track of the products a customer recently viewed and offer the customer a quick link back to those products. ", ImageId = "C33" });

            return result;
        }
        private List<FeatureDetails> GetDevelopment()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Full Source Code Included", Description = "Customize the software to fit your needs. The best value for asp.net ecommerce with source code!", ImageId = "D1" });
            result.Add(new FeatureDetails() { Name = "Built for ASP.NET 2.0 (not just recompiled)", Description = "Our software uses .net 2.0 features like master pages and generics, unlike our competitors who just recompile their software and call it 2.0.", ImageId = "D2" });
            result.Add(new FeatureDetails() { Name = "Visual Studio .Net Project Files", Description = "Quickly load our source code into Visual Studio and get started customizing right away. ", ImageId = "D3" });
            result.Add(new FeatureDetails() { Name = "N-Tier Architecture", Description = "Clean separation of data layer, business logic and presentation layer make our software easy to modify and update. ", ImageId = "D4" });
            result.Add(new FeatureDetails() { Name = "Extensively Tested with Over 800 Unit Tests", Description = "Automated unit testing prevents bugs from getting into the software during updates. ", ImageId = "D5" });
            result.Add(new FeatureDetails() { Name = "SQL 2005", Description = "Optimized for the latest version of SQL server for more efficient database calls and paging.", ImageId = "D6" });
            result.Add(new FeatureDetails() { Name = "SQL Stored Procedures", Description = "Stored procedures provide enchanced security and performance. ", ImageId = "D7" });
            result.Add(new FeatureDetails() { Name = "Visual Basic .NET", Description = "Source code is provided in vb.net.", ImageId = "D8" });
            result.Add(new FeatureDetails() { Name = "Developer Friendly Architecture", Description = "Our exclusive modular architecture makes it easy for developers to add functionality without changing our source code. Add features but still use our service packs. ", ImageId = "D9" });
            result.Add(new FeatureDetails() { Name = "Business Logic Workflows", Description = "Configure or extend business rules without programming from our admin interface!", ImageId = "D10" });
            result.Add(new FeatureDetails() { Name = "Order Workflow Plug-Ins", Description = "Add custom order steps to export xml data, check inventory and more!", ImageId = "D11" });
            result.Add(new FeatureDetails() { Name = "Product Workflow Plug-Ins", Description = "Add custom pricing and presentation rules to products. ", ImageId = "D12" });
            result.Add(new FeatureDetails() { Name = "Payment Plug-Ins", Description = "Add your own credit card processors easily.", ImageId = "D13" });
            result.Add(new FeatureDetails() { Name = "Shipping Plug-Ins", Description = "Add support for additional shipping companies and custom shipping rules.", ImageId = "D14" });
            result.Add(new FeatureDetails() { Name = "Category Page Plug-Ins", Description = "Choose from our included designs or easily add your own! Unlike other software packages, adding a template is as easy as creating a new web page. ", ImageId = "D15" });
            result.Add(new FeatureDetails() { Name = "Product Page Plug-Ins", Description = "Choose from our included designs or easily add your own! Unlike other software packages, adding a template is as easy as creating a new web page. ", ImageId = "D16" });
            result.Add(new FeatureDetails() { Name = "Discount Plug-Ins", Description = "Add your own custom discount rules by extending our included sales and offers. ", ImageId = "D17" });
            result.Add(new FeatureDetails() { Name = "Content Block Plug-Ins", Description = "Create reusable blocks of content like rss feeds, product rotators and more!", ImageId = "D18" });
            result.Add(new FeatureDetails() { Name = "Theme Plug-Ins", Description = "Create your own custom look using HTML standards. Unlike other software, our themes use asp.net master pages and css. No custom xml language to learn!", ImageId = "D19" });
            result.Add(new FeatureDetails() { Name = "Checkout Plug-Ins", Description = "Customize the checkout process with your own pages like agreements, surveys, etc…", ImageId = "D20" });
            result.Add(new FeatureDetails() { Name = "Report Plug-Ins", Description = "Easily create custom reports that automatically show up in our admin interface.", ImageId = "D21" });
            result.Add(new FeatureDetails() { Name = "Text Editor Plug-Ins", Description = "Choose from our WYSIWYG editor or standard text boxes or add your own!", ImageId = "D22" });
            result.Add(new FeatureDetails() { Name = "Product Choice Plug-Ins", Description = "When drop down lists and radio buttons aren't enough, create your own product choices and easily add them!", ImageId = "D23" });
            result.Add(new FeatureDetails() { Name = "Product Input Plug-Ins", Description = "When text inputs aren't enough, add your own inputs for things like uploading files. Add your own types of inputs like photos for personalized gifts!", ImageId = "D24" });
            result.Add(new FeatureDetails() { Name = "GUID Values for Easier Integration", Description = "Global unique identfier fields make it easy to import, export and transfer data between stores. GUIDs make sure products stay connected to categories, users to orders and more. ", ImageId = "D25" });
            result.Add(new FeatureDetails() { Name = "Date Last Updated Fields for Synchronization", Description = "Time stamps make it easy for import and export tools to easily find the latest changes to your store. ", ImageId = "D26" });

            return result;
        }
        private List<FeatureDetails> GetSearch()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Advanced Keyword Search", Description = "A search box on every page of your store helps customers quickly find items by keyword. Intelligent linguistic stemming matches plural words, root words and more for more accurate results. The search engine indexes product choices too so a T-Shirt available in Red, Green and Blue is searchable as &quot;Green Shirt&quot; even if you don't put &quot;Green&quot; in the name or description.", ImageId = "E1" });
            //result.Add(new FeatureDetails() { Name = "Advanced Search", Description = "Allow customers to find products by keyword, price range, category, manufacturer, vendor and custom properties. ", ImageId = "E2" });
            //result.Add(new FeatureDetails() { Name = "Search by Price Range", Description = "Allow customers to quickly find products between two prices.", ImageId = "E3" });
            //result.Add(new FeatureDetails() { Name = "Search by Manufacturer", Description = "Allow customers to filter products by manufacturer name.", ImageId = "E4" });
            //result.Add(new FeatureDetails() { Name = "Search by Vendor", Description = "Allow customers to filter products by vendor name.", ImageId = "E5" });
            //result.Add(new FeatureDetails() { Name = "Search by Category", Description = "Allow customers to find products in a specific category.", ImageId = "E6" });
            //result.Add(new FeatureDetails() { Name = "Search by Product Type", Description = "Allow customers to find products of a specific type.", ImageId = "E7" });
            result.Add(new FeatureDetails() { Name = "Drill-Down Categories", Description = "Help customers filter through thousands of products in seconds with drill-down categories. Assign properties like TV Screen Size or Year, Make and Model. BV Commerce automatically generates the search category pages." });
            result.Add(new FeatureDetails() { Name = "Search by Merchant Definable Properties", Description = "Allow customers to search by your custom fields like author, published date, movie rating and more!", ImageId = "E8" });
            result.Add(new FeatureDetails() { Name = "Product Keyword Field for Misspellings or Similar Words", Description = "Add keywords for searches that don't display on the store. Great for common misspellings!", ImageId = "E9" });
            result.Add(new FeatureDetails() { Name = "Automatic Sitemap Page", Description = "A site map is automatically created, making it easy for search engines to index all of your pages. ", ImageId = "E10" });

            return result;
        }
        private List<FeatureDetails> GetCustomers()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Remember Billing and Shipping Addresses", Description = "Shoppers' billing and shipping addresses are saved, saving them time the next time they order!", ImageId = "F1" });
            result.Add(new FeatureDetails() { Name = "Unlimited Address Book", Description = "Automatically remember all addresses that customers have used in the past for quick reorders. ", ImageId = "F2" });
            result.Add(new FeatureDetails() { Name = "Order History", Description = "Customers can view the details of all past orders.", ImageId = "F3" });
            result.Add(new FeatureDetails() { Name = "File Download Links", Description = "Make it easy for customers to find their downloads again.", ImageId = "F4" });
            //result.Add(new FeatureDetails() { Name = "One Click Re-Order Button", Description = "Customers can duplicate an old order with a single click.", ImageId = "F5" });
            //result.Add(new FeatureDetails() { Name = "Email List Subscribe/Unsubscribe", Description = "Allow customers to add and remove themselves from e-mail lists, saving store owners support time. ", ImageId = "F6" });
            result.Add(new FeatureDetails() { Name = "Forgot Password Reset", Description = "Let customers reset their own passwords 24 hours a day without contacting a store owner.", ImageId = "F7" });
            result.Add(new FeatureDetails() { Name = "Change Email", Description = "Let customers update their own e-mail addresses. ", ImageId = "F8" });
            result.Add(new FeatureDetails() { Name = "Change Password", Description = "Let customers change their passwords at any time.", ImageId = "F9" });
            //result.Add(new FeatureDetails() { Name = "Select Personalized Themes", Description = "Optionally let customers select their own store theme. Great for affililates!", ImageId = "F10" });
            //result.Add(new FeatureDetails() { Name = "Custom Fields During Registration", Description = "Easily collect additional information from your customers when they register for an account. Add your own questions and options. Excellent for targeted advertising campaigns!", ImageId = "F11" });
            //result.Add(new FeatureDetails() { Name = "View Customers Keyword Search History", Description = "View a report to find out what keywords customers are searching for on your store. Great for optimizing your content and targeted advertising campaigns.", ImageId = "F12" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Pricing Groups", Description = "Provide some customers, like wholesalers, with storewide discounted pricing. No limits!", ImageId = "F13" });
            //result.Add(new FeatureDetails() { Name = "Mailing List Selections", Description = "Let customers browse all available e-mail lists and subscribe or unsubscribe. ", ImageId = "F14" });
            //result.Add(new FeatureDetails() { Name = "Password Encryption", Description = "Choose from several industry standard encryption methods to protect customer passwords. ", ImageId = "F15" });
            result.Add(new FeatureDetails() { Name = "Tax Exempt Option", Description = "Easily mark some customers as tax exempt. ", ImageId = "F16" });

            return result;
        }
        private List<FeatureDetails> GetLoyalty()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            //result.Add(new FeatureDetails() { Name = "Email a Friend", Description = "Share great product bargains with other customers via e-mail.", ImageId = "G1" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Mailing Lists", Description = "Create multiple mailing lists to target groups of customers. Offer lists for weekly sales, product updates, general information and more. ", ImageId = "G2" });
            result.Add(new FeatureDetails() { Name = "Rewards Points", Description = "Keep customers shopping with a custom rewards program. You pick the name and rules letting customers earn point with purchases and redeem points for store credit." });
            result.Add(new FeatureDetails() { Name = "Remember Shopping Carts", Description = "Automatically remember what customers have in their shopping carts, even if they close their web browsers and aren't logged in.", ImageId = "G3" });
            result.Add(new FeatureDetails() { Name = "Remember Users/Logins", Description = "Save customers' time by remembering their user names when they return to shop again.", ImageId = "G4" });
            result.Add(new FeatureDetails() { Name = "Remember Last Billing/Shipping Address", Description = "Save customers' time by remembering where they last had their orders shipped and billed to.", ImageId = "G5" });
            //result.Add(new FeatureDetails() { Name = "Fixed Amount Gift Certificates", Description = "Offer flat rate gift certificates like $10, $50, $100, etc…", ImageId = "G6" });
            //result.Add(new FeatureDetails() { Name = "Variable Amount Gift Certificates", Description = "Let customers enter the exact amount they wish to purchase a gift certificate for.", ImageId = "G7" });
            //result.Add(new FeatureDetails() { Name = "Auto-Generate Gift Certificate Codes", Description = "Define your own gift certificate code format. For example, MYSTORE123.", ImageId = "G8" });

            return result;
        }
        private List<FeatureDetails> GetAffiliateProgram()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Customizable Signup Form", Description = "Add your own questions and comment fields to affiliate sign up forms. ", ImageId = "H1" });
            result.Add(new FeatureDetails() { Name = "Remember Affiliates for Future Sales", Description = "Cookies are used to remember which affiliate referred a customer so affiliate credit can be given even if the customer doesn't purchase on the first visit.", ImageId = "H2" });
            result.Add(new FeatureDetails() { Name = "Affiliate Conflict Resolution", Description = "When a customer is referred from two different affiliates, you can decide if the first affiliate or last affiliate gets credit. ", ImageId = "H3" });
            result.Add(new FeatureDetails() { Name = "Commissions by Amount", Description = "Automatically calculate commissions at a flat rate per sale.", ImageId = "H4" });
            result.Add(new FeatureDetails() { Name = "Commissions by Percent", Description = "Automatically calculate commissions as a percentage of each sale.", ImageId = "H5" });
            //result.Add(new FeatureDetails() { Name = "Affiliates can Login to View Reports", Description = "Affiliates can view sales reports and commissions due from their store account pages. ", ImageId = "H6" });
            return result;
        }
        private List<FeatureDetails> GetContentManagement()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Unlimited Customizable Content Sections", Description = "Easily add and remove sections of HTML from our admin interface. ", ImageId = "I1" });
            result.Add(new FeatureDetails() { Name = "15+ Built-in Content Blocks", Description = "Choose from a variety of built in content blocks to customize your site's appearance.", ImageId = "I2" });
            result.Add(new FeatureDetails() { Name = "Unlimited Content Block Plug-Ins", Description = "Create your own content blocks and easily add them to our software without custom programming. ", ImageId = "I3" });
            result.Add(new FeatureDetails() { Name = "Category Rotators", Description = "Display a new category link every time a customer visits a page.", ImageId = "I4" });
            result.Add(new FeatureDetails() { Name = "Category Menus", Description = "Show a list of navigation links that are automatically generated from our admin interface.", ImageId = "I5" });
            //result.Add(new FeatureDetails() { Name = "Flash Image Rotators (no programming required)", Description = "Show a slide show of products with fade in/fade out transitions anywhere on your site.  ", ImageId = "I6" });
            result.Add(new FeatureDetails() { Name = "Image Rotators", Description = "Show a new image each time a customer visits a page.", ImageId = "I7" });
            result.Add(new FeatureDetails() { Name = "Recently Viewed Products", Description = "Automatically keep track of which products a customer views and provide quicklinks back to the products.", ImageId = "I8" });
            result.Add(new FeatureDetails() { Name = "RSS Feed Viewer", Description = "Display RSS news feeds anywhere in your store to keep content fresh.", ImageId = "I9" });
            result.Add(new FeatureDetails() { Name = "Search Box", Description = "Allow customers to quickly search by keyword from any page in your store.", ImageId = "I10" });
            result.Add(new FeatureDetails() { Name = "Side Menu", Description = "Create your own navigation menus using our intuitive forms. No HTML programming required.", ImageId = "I11" });
            result.Add(new FeatureDetails() { Name = "Sticky Notes", Description = "Display friendly reminder notes on the store or admin side.", ImageId = "I12" });
            result.Add(new FeatureDetails() { Name = "Top 10 Products", Description = "Automatically show a list of the top 10 selling products on your store. ", ImageId = "I13" });
            result.Add(new FeatureDetails() { Name = "Top Weekly Sellers", Description = "Automatically show a list of the top selling products this week.", ImageId = "I14" });
            result.Add(new FeatureDetails() { Name = "Store Contact Address", Description = "Add your store location for contact forms and shipping calculations.", ImageId = "I15" });
            //result.Add(new FeatureDetails() { Name = "Store Contact Form with Customizable Fields", Description = "Let customers contact you via a web form. Add your own questions and comment fields. ", ImageId = "I16" });
            result.Add(new FeatureDetails() { Name = "Store Meta Tags", Description = "Create a set of default meta tags to be used when individual product and category meta tags are not set.", ImageId = "I17" });
            //result.Add(new FeatureDetails() { Name = "Help/FAQ", Description = "Add frequently asked questions to your store from the admin without programming.", ImageId = "I18" });
            result.Add(new FeatureDetails() { Name = "Privacy Policy", Description = "Let your customers know what will happen to their information.", ImageId = "I19" });
            result.Add(new FeatureDetails() { Name = "Shipping Policy", Description = "Let customers know when their items will ship.", ImageId = "I20" });
            result.Add(new FeatureDetails() { Name = "Return Policy", Description = "Let customers know how and if you accept returns.", ImageId = "I21" });
            result.Add(new FeatureDetails() { Name = "Terms and Conditions", Description = "Optionally, require customers to agree to your site's terms and conditions before purchase.", ImageId = "I22" });
            //result.Add(new FeatureDetails() { Name = "Custom Store Polices", Description = "Create your own policies to display in the customer service area.", ImageId = "I23" });
            result.Add(new FeatureDetails() { Name = "Custom Pages", Description = "Add custom HTML pages to your store with one click. No programming required.", ImageId = "I24" });
            //result.Add(new FeatureDetails() { Name = "Open/Close Store for Updates", Description = "Temporarily close your store while you update pricing and products before a big event.", ImageId = "I25" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Email Templates", Description = "Create and save HTML e-mail templates for more effective e-mail marketing. Easily create e-mail templates from the admin!", ImageId = "I26" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Print Templates", Description = "Create and save print templates to customize packing slips and more.", ImageId = "I27" });
            result.Add(new FeatureDetails() { Name = "Custom URL Rewriting", Description = "For the ultimate control, we make it easy to set your own URLs for every product and category in your store. You can even create custom URLs for other store pages like privacy policies and FAQs. Most other carts don't offer this option at all!", ImageId = "I28" });
            result.Add(new FeatureDetails() { Name = "Administrator Dashboard", Description = "Easily see how your store is doing at a glance with links to commonly used features and alerts needing your attention.", ImageId = "I29" });
            return result;
        }
        private List<FeatureDetails> GetOrderManagement()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "New Order Emails", Description = "Automatically send an e-mail receipt to customers after they place orders. ", ImageId = "J1" });
            result.Add(new FeatureDetails() { Name = "Quick Tabs For Order Status", Description = "Quickly find orders that are paid, unpaid, ready for shipping etc…", ImageId = "J2" });
            //result.Add(new FeatureDetails() { Name = "Filter By Shipping Status", Description = "Quickly find orders that are shipped, unshipped, etc…", ImageId = "J3" });
            //result.Add(new FeatureDetails() { Name = "Filter By Order Status", Description = "Quickly find orders that are completed, on hold, etc…", ImageId = "J4" });
            //result.Add(new FeatureDetails() { Name = "Filter By Date Range", Description = "Quickly find orders from a specific date or date range.", ImageId = "J5" });
            //result.Add(new FeatureDetails() { Name = "Filter By Keyword (user, phone, credit card, etc.)", Description = "Quickly find orders by customer name, e-mail, phone number, etc…", ImageId = "J6" });
            result.Add(new FeatureDetails() { Name = "Batch Process Credit Cards", Description = "Receive payment from multiple orders with a single click.", ImageId = "J7" });
            //result.Add(new FeatureDetails() { Name = "Batch Print Invoices", Description = "Print invoices from multiple orders with a single click.", ImageId = "J8" });
            //result.Add(new FeatureDetails() { Name = "Batch Print Receipts", Description = "Print admin sales receipts from multiple orders with a single click.", ImageId = "J9" });
            //result.Add(new FeatureDetails() { Name = "Batch Print Packing Slips", Description = "Print packing slips from multiple orders with a single click.", ImageId = "J10" });
            result.Add(new FeatureDetails() { Name = "Flag Orders as On Hold", Description = "Mark suspicious orders for review. ", ImageId = "J11" });
            result.Add(new FeatureDetails() { Name = "Delete Orders", Description = "Permanently remove old or test orders.", ImageId = "J12" });
            //result.Add(new FeatureDetails() { Name = "Edit Orders (including variant products)", Description = "Add and remove products from orders, adjust shipping, add coupons, or adjust prices. Editing products even works with product choices and inputs, unlike competing software. ", ImageId = "J13" });
            result.Add(new FeatureDetails() { Name = "Receive Payments", Description = "Quickly receive payment via credit card, paypal, cash, cod, telephone and more.", ImageId = "J14" });
            result.Add(new FeatureDetails() { Name = "Issue Credits", Description = "Refund credit card payments without leaving the admin interface.", ImageId = "J15" });
            //result.Add(new FeatureDetails() { Name = "Generate Return Authorizations", Description = "Automatically generate return authorization numbers and allow customers to easily fill out return request forms.", ImageId = "J16" });
            //result.Add(new FeatureDetails() { Name = "Receive Returns", Description = "Approve or deny return requests and automatically return items to inventory.", ImageId = "J17" });
            //result.Add(new FeatureDetails() { Name = "Ship directly Through UPS and Print Labels", Description = "Generate UPS shipping labels and automatically send information to UPS via the admin interface. Tracking numbers are automatically added to orders and shipment e-mails are sent to customers. ", ImageId = "J18" });
            result.Add(new FeatureDetails() { Name = "Email Customers", Description = "Quickly e-mail customers with status updates on their orders.", ImageId = "J19" });
            result.Add(new FeatureDetails() { Name = "Report Customer IP Addresses", Description = "Keep track of IP addresses to help identify fraudulent transactions.", ImageId = "J20" });
            result.Add(new FeatureDetails() { Name = "Public Notes", Description = "Add order notes visible to both customer and admin on the order details screen.", ImageId = "J21" });
            result.Add(new FeatureDetails() { Name = "Private Notes", Description = "Add order notes visible to only the admin on the order details screen.", ImageId = "J22" });
            result.Add(new FeatureDetails() { Name = "Customer Instructions", Description = "Let customers add additional instructions and comments during checkout.", ImageId = "J23" });
            result.Add(new FeatureDetails() { Name = "Ship Suggested Packages with One Click", Description = "Our shipping algorithm suggests which items should go in which boxes. You can optionally ship our suggested boxes with a single click or create your own.", ImageId = "J24" });
            result.Add(new FeatureDetails() { Name = "Record Tracking Information", Description = "Record tracking information for FedEx, US Postal Service and more!", ImageId = "J25" });
            //result.Add(new FeatureDetails() { Name = "Create New Orders via Admin Web Site", Description = "Our telephone order entry page allows admins to create orders on behalf of customers. Find customer accounts, create customer accounts or place orders without customer accounts. ", ImageId = "J26" });
            //result.Add(new FeatureDetails() { Name = "Take Orders By Phone", Description = "Our telephone order entry page allows admins to create orders on behalf of customers. Find customer accounts, create customer accounts or place orders without customer accounts. ", ImageId = "J27" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Merchant Definable Order Status Codes", Description = "Create your own order status codes like &quot;ready for packing,&quot; &quot;need customer approval,&quot; or &quot;rush order.&quot; No programming required.", ImageId = "J28" });
            //result.Add(new FeatureDetails() { Name = "2 Included Checkouts with Unlimited Plug-Ins", Description = "Choose from multi-page or single page checkouts or create your own.", ImageId = "J29" });
            result.Add(new FeatureDetails() { Name = "Ajax Enabled One Page Checkout", Description = "Simplify the checkout process with automatically updating shipping and payment information all on a single screen.", ImageId = "J30" });
            //result.Add(new FeatureDetails() { Name = "Checkout without Registration", Description = "Allow customers to purchase without creating an account.", ImageId = "J31" });
            //result.Add(new FeatureDetails() { Name = "Require Registration Option", Description = "Optionally require that all customers create accounts to place orders.", ImageId = "J32" });

            return result;
        }
        private List<FeatureDetails> GetTaxes()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Unlimited Taxes", Description = "Create an unlimited number of tax zones. ", ImageId = "K1" });
            result.Add(new FeatureDetails() { Name = "Unlimited Tax Schedules", Description = "Create tax zones that apply to specific groups of products.", ImageId = "K2" });
            result.Add(new FeatureDetails() { Name = "Tax by Country", Description = "Charge tax by country. ", ImageId = "K3" });
            result.Add(new FeatureDetails() { Name = "Tax by State/Region", Description = "Charge tax by state or region.", ImageId = "K4" });
            //result.Add(new FeatureDetails() { Name = "Tax by County", Description = "Charge tax by county or municipality.", ImageId = "K5" });
            result.Add(new FeatureDetails() { Name = "Tax by Zip/Postal Code", Description = "Charge tax by zip code.", ImageId = "K6" });
            result.Add(new FeatureDetails() { Name = "Tax Exempt Products", Description = "Create tax exempt products with a single click.", ImageId = "K7" });
            //result.Add(new FeatureDetails() { Name = "Tax Exempt Customers", Description = "Create tax exempt customers with a single click.", ImageId = "K8" });
            return result;
        }
        private List<FeatureDetails> GetPayment()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Credit Cards", Description = "Receive credit card payments from more than 25 common credit card gateways.", ImageId = "L1" });
            //result.Add(new FeatureDetails() { Name = "Google Checkout", Description = "Fully integrated with Google Checkout. Enable with just a few clicks.", ImageId = "L2" });
            result.Add(new FeatureDetails() { Name = "PayPal Express Checkout", Description = "Allow customers to checkout through the PayPal website.", ImageId = "L3" });
            result.Add(new FeatureDetails() { Name = "PayPal Website Payments Pro", Description = "Process credit cards through PayPal.", ImageId = "L4" });
            result.Add(new FeatureDetails() { Name = "PayPal Website Payments Standard", Description = "Process credit cards on PayPal.com", ImageId = "L4" });
            result.Add(new FeatureDetails() { Name = "Pay Pal Instant Payment Notification (IPN)", Description = "Automatically update order status when PayPal payments clear.", ImageId = "L5" });
            result.Add(new FeatureDetails() { Name = "Telephone", Description = "Allow customers to place orders and phone in payment information.", ImageId = "L6" });
            result.Add(new FeatureDetails() { Name = "Company Account", Description = "Let employees pay using their company ID numbers.", ImageId = "L7" });
            result.Add(new FeatureDetails() { Name = "Purchase Order", Description = "Capture purchase order numbers to invoice clients for payment.", ImageId = "L8" });
            result.Add(new FeatureDetails() { Name = "Check By Mail", Description = "Update orders to note the receipt of checks by mail as payment.", ImageId = "L9" });
            result.Add(new FeatureDetails() { Name = "COD", Description = "Allow customers to order with cash on delivery as payment.", ImageId = "L10" });
            result.Add(new FeatureDetails() { Name = "Authorize.net", Description = "Process credit cards through Authorize.net.", ImageId = "L11" });
            //result.Add(new FeatureDetails() { Name = "LinkPoint", Description = "Process credit cards through LinkPoint.", ImageId = "L12" });
            //result.Add(new FeatureDetails() { Name = "Concord EFS Net", Description = "Process credit cards through Concord EFS Net.", ImageId = "L13" });
            //result.Add(new FeatureDetails() { Name = "Off-Line Processing", Description = "Capture credit card numbers for offline processing.", ImageId = "L14" });
            result.Add(new FeatureDetails() { Name = "Credit Card Simulator for Testing", Description = "Built in test processor allows you to try out charges, failures, refunds and more before your store opens.", ImageId = "L15" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Credit Card Plug-Ins", Description = "Choose from more than 25 common credit card gateways or easily add support for your own.", ImageId = "L16" });
            result.Add(new FeatureDetails() { Name = "CVV Codes", Description = "Capture security codes for lower processing fees.", ImageId = "L17" });
            //result.Add(new FeatureDetails() { Name = "PCI Compliance", Description = "BV Commerce currently meets all PCI compliance guidelines. PCI application compliance in progress.", ImageId = "L18" });

            return result;
        }
        private List<FeatureDetails> GetShipping()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            //result.Add(new FeatureDetails() { Name = "UPS Real Time Rates", Description = "Calculate UPS rates automatically based on size, weight and shipping address.", ImageId = "M1" });
            //result.Add(new FeatureDetails() { Name = "US Postal Service Real Time Rates", Description = "Calculate US Postal Service rates automatically based on size, weight and shipping address.", ImageId = "M2" });
            //result.Add(new FeatureDetails() { Name = "FedEx Real Time Rates", Description = "Calculate FedEx rates automatically based on size, weight and shipping address.", ImageId = "M3" });
            //result.Add(new FeatureDetails() { Name = "Canada Post (3rd Party Add-on)", Description = "Calculate Canada Post rates automatically based on size, weight and shipping address. Available from www.structured-solutions.net.", ImageId = "M4" });
            //result.Add(new FeatureDetails() { Name = "DHL (3rd Party Add-on)", Description = "Calculate DHL rates automatically based on size, weight and shipping address. Available from www.structured-solutions.net.", ImageId = "M5" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Shipping Provider Plug-ins", Description = "If your shipping company isn't supported, easily create your own shipping plug in. ", ImageId = "M6" });
            result.Add(new FeatureDetails() { Name = "Per-Item Charges", Description = "Charge shipping at a flat rate for each item order.", ImageId = "M7" });
            result.Add(new FeatureDetails() { Name = "Per-Order Amount Charges", Description = "Charge shipping based on the total amount of each order.", ImageId = "M8" });
            result.Add(new FeatureDetails() { Name = "By Item Count Charges", Description = "Charge shipping at a tiered rate based on the total number of items ordered.", ImageId = "M9" });
            result.Add(new FeatureDetails() { Name = "Handling Fees", Description = "Add handling fees of percentages or flat rate per order or per shipment.", ImageId = "M10" });
            //result.Add(new FeatureDetails() { Name = "Individual Product Shipping Fees", Description = "Add additional shipping charges to specific products.", ImageId = "M11" });
            //result.Add(new FeatureDetails() { Name = "Adjust Real Time Rates", Description = "Adjust calculated shipping rates plus or minus percentage or dollar amounts.", ImageId = "M12" });
            //result.Add(new FeatureDetails() { Name = "Ship packages by UPS", Description = "Automatically send shipment information to UPS.", ImageId = "M13" });
            //result.Add(new FeatureDetails() { Name = "Print UPS Labels", Description = "Automatically generate UPS shipping labels and tracking numbers.", ImageId = "M14" });
            //result.Add(new FeatureDetails() { Name = "Automatically Record UPS Tracking Numbers", Description = "Automatically update orders with UPS tracking information and alert customers via e-mail.", ImageId = "M15" });
            result.Add(new FeatureDetails() { Name = "Manually Add Tracking Information", Description = "Add tracking numbers for FedEx, US Postal Service, and more with automatic links to check status.", ImageId = "M16" });
            result.Add(new FeatureDetails() { Name = "Mix and Match Providers and Rates", Description = "Display rates from multiple shipping providers at the same time to offer customers the best prices.", ImageId = "M17" });
            result.Add(new FeatureDetails() { Name = "Set Options by Country/Regions", Description = "Limit shipping providers and rates to specific countries and regions.", ImageId = "M18" });
            result.Add(new FeatureDetails() { Name = "Shipping Estimator on Cart Page", Description = "Let customers quickly estimate shipping charges without going through the checkout process.", ImageId = "M19" });

            return result;
        }
        private List<FeatureDetails> GetSecurity()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            //result.Add(new FeatureDetails() { Name = "Fraud Screen by IP", Description = "Flag suspicious orders by known IP addresses. ", ImageId = "N2" });
            //result.Add(new FeatureDetails() { Name = "Fraud Screen by Email", Description = "Flag suspicious orders by e-mail.", ImageId = "N3" });
            //result.Add(new FeatureDetails() { Name = "Fraud Screen Phone", Description = "Flag suspicious orders by phone number.", ImageId = "N4" });
            //result.Add(new FeatureDetails() { Name = "Fraud Screen By Domain", Description = "Flag suspicious orders by website domain name.", ImageId = "N5" });
            //result.Add(new FeatureDetails() { Name = "Fraud Screen By Credit Card", Description = "Flag suspicious orders by credit card number.", ImageId = "N6" });
            //result.Add(new FeatureDetails() { Name = "Lock Out Users After x Failed Attempts", Description = "Lock user accounts when someone has entered too many incorrect passwords.", ImageId = "N7" });
            //result.Add(new FeatureDetails() { Name = "Lock Out Users for x Minutes After Failed Logins", Description = "Lock user accounts for a set number of minutes when someone has entered too many incorrect passwords.", ImageId = "N8" });
            //result.Add(new FeatureDetails() { Name = "Role Based Security", Description = "Create admin accounts that have limited access for things like order processing or catalog updates. ", ImageId = "N9" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Roles", Description = "Create your own administrative roles with rules about which areas they can access.", ImageId = "N10" });
            result.Add(new FeatureDetails() { Name = "Encrypted Passwords", Description = "Industry standard encryption algorithms are used to protect passwords.", ImageId = "N11" });
            result.Add(new FeatureDetails() { Name = "Advanced Encryption for Credit Cards", Description = "Industry standard high security encryption is used to protect credit card information.", ImageId = "N12" });
            result.Add(new FeatureDetails() { Name = "Full Credit Card Numbers Never Displayed", Description = "Only the last four digits of credit card numbers are displayed to prevent card number theft.", ImageId = "N13" });
            //result.Add(new FeatureDetails() { Name = "CVV Codes Never Stored", Description = "As required for PCI Compliance, security codes are never stores or displayed.", ImageId = "N14" });
            //result.Add(new FeatureDetails() { Name = "Option to Scrub Credit Card Number after Payment", Description = "Optionally clear out credit card numbers from processed orders to reduce the possibility of card number theft.", ImageId = "N15" });
            result.Add(new FeatureDetails() { Name = "SSL Support", Description = "Full support for SSL certificates to protect private information.", ImageId = "N16" });
            //result.Add(new FeatureDetails() { Name = "Shared SSL Support", Description = "Optionally use your web host's SSL certificate to protect private information.", ImageId = "N17" });

            return result;
        }
        private List<FeatureDetails> GetDesign()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Free Premium Themes Included", Description = "Choose from one of our included themes or create your own using ours as a starting point.", ImageId = "O1" });
            result.Add(new FeatureDetails() { Name = "One Click Install", Description = "Browse our theme gallery and install a new design with a single click", ImageId = "O2" });
            result.Add(new FeatureDetails() { Name = "Create Your Own", Description = "With one click you can create a copy of a standard design and modify it to fit your store. No need to start from scratch.", ImageId = "O3" });
            result.Add(new FeatureDetails() { Name = "Small Page Size", Description = "Optimized page size means a faster shopping experience.", ImageId = "O4" });
            //result.Add(new FeatureDetails() { Name = "Flexible Layouts", Description = "Choose from a variety of category and product page templates to make your store unique.", ImageId = "O5" });
            //result.Add(new FeatureDetails() { Name = "Custom Category Page Layouts", Description = "Choose from grid view, list view, details view and more when displaying products in their categories.", ImageId = "O6" });
            //result.Add(new FeatureDetails() { Name = "Custom Product Page Layouts", Description = "Choose from our built in product page templates or easily add your own using standard asp.net pages.", ImageId = "O7" });
            return result;
        }
        private List<FeatureDetails> GetReports()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Monthly Sales", Description = "Show order totals and order details on a month by month basis.", ImageId = "P1" });
            result.Add(new FeatureDetails() { Name = "Daily Sales", Description = "Show order totals and order details on a daily basis.", ImageId = "P2" });
            result.Add(new FeatureDetails() { Name = "Sales by Affiliate", Description = "Find out which orders came from which affiliates and what commissions are due.", ImageId = "P3" });
            //result.Add(new FeatureDetails() { Name = "Sales by Coupon", Description = "Find out which orders used a specific coupon code with details.", ImageId = "P4" });
            result.Add(new FeatureDetails() { Name = "Sales by Product", Description = "Find out which products are selling best.", ImageId = "P5" });
            //result.Add(new FeatureDetails() { Name = "Sales by Customer", Description = "Find out which customers are spending the most at your store.", ImageId = "P6" });
            //result.Add(new FeatureDetails() { Name = "Keyword Searches", Description = "Find out which keywords are used to search your store. Great for refining your content and meta tags!", ImageId = "P7" });
            //result.Add(new FeatureDetails() { Name = "Unlimited Custom Report Plug-Ins", Description = "Add your own custom reports by adding asp.net pages to the reports folder.", ImageId = "P8" });
            return result;
        }
        private List<FeatureDetails> GetIntegration()
        {
            List<FeatureDetails> result = new List<FeatureDetails>();
            result.Add(new FeatureDetails() { Name = "Full XML Web Services Included", Description = "Easily integrate your store with other applications using our XML web services. Web services allow import and export to always work even after service pack upgrades and database changes. ", ImageId = "Q1" });
            result.Add(new FeatureDetails() { Name = "Free Import/Export/Update Tools", Description = "Free update tools are available, including full source code, allowing you to import and export text files and more. ", ImageId = "Q2" });
            result.Add(new FeatureDetails() { Name = "Free QuickBooks Integration Add-On", Description = "Transfer orders from your store to Quickbooks and synchronize inventory levels, saving you time and preventing data entry mistakes.", ImageId = "Q3" });
            return result;
        }
    }
}