using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Utilities;
using System.Collections.ObjectModel;

namespace BVCommerce
{
    public class ViewUtilities
    {
        //public enum Sizes
        //{
        //    Small = 0,
        //    Medium = 1
        //}

        //public static void ForceImageSize(Control image, string imageFile, Sizes size, Page currentPage)
        //{
        //    // Force Image Size Code
        //    if (WebAppSettings.ForceImageSizes) {
        //        ImageInfo resizeInfo = new ImageInfo();
        //        if (imageFile.ToLower().StartsWith("http://")) {
        //            if (size == Sizes.Small) {
        //                resizeInfo.Width = WebAppSettings.ImagesSmallWidth;
        //                resizeInfo.Height = WebAppSettings.ImagesSmallHeight;
        //            }
        //            else if (size == Sizes.Medium) {
        //                resizeInfo.Width = WebAppSettings.ImagesMediumWidth;
        //                resizeInfo.Height = WebAppSettings.ImagesMediumHeight;
        //            }
        //        }
        //        else {
        //            ImageInfo iInfo = ImageHelper.GetImageInformation(currentPage.ResolveUrl(ImageHelper.GetValidImage(imageFile, true)));
        //            if (iInfo.Width == 0 & iInfo.Height == 0) {
        //                if (size == Sizes.Small) {
        //                    iInfo.Width = WebAppSettings.ImagesSmallWidth;
        //                    iInfo.Height = WebAppSettings.ImagesSmallHeight;
        //                }
        //                else if (size == Sizes.Medium) {
        //                    iInfo.Width = WebAppSettings.ImagesMediumWidth;
        //                    iInfo.Height = WebAppSettings.ImagesMediumHeight;
        //                }
        //            }
        //            if (size == Sizes.Small) {
        //                resizeInfo = ImageHelper.GetProportionalImageDimensionsForImage(iInfo, WebAppSettings.ImagesSmallWidth, WebAppSettings.ImagesSmallHeight);
        //            }
        //            else if (size == Sizes.Medium) {
        //                resizeInfo = ImageHelper.GetProportionalImageDimensionsForImage(iInfo, WebAppSettings.ImagesMediumWidth, WebAppSettings.ImagesMediumHeight);
        //            }

        //        }

        //        if (image is System.Web.UI.HtmlControls.HtmlImage) {
        //            System.Web.UI.HtmlControls.HtmlImage img = (System.Web.UI.HtmlControls.HtmlImage)image;
        //            img.Width = resizeInfo.Width;
        //            img.Height = resizeInfo.Height;
        //        }
        //        else if (image is System.Web.UI.WebControls.Image) {
        //            Image img = (Image)image;
        //            img.Width = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Width);
        //            img.Height = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Height);
        //        }
        //        else if (image is System.Web.UI.WebControls.ImageButton)
        //        {
        //            ImageButton img = (ImageButton)image;
        //            img.Width = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Width);
        //            img.Height = System.Web.UI.WebControls.Unit.Pixel(resizeInfo.Height);
        //        }
        //        else if (image is System.Web.UI.WebControls.HyperLink)
        //        {
        //            HyperLink link = (HyperLink)image;
        //            link.Width = resizeInfo.Width;
        //            link.Height = resizeInfo.Height;
        //        }
        //        else {
        //            throw new ArgumentException("Force Image Size Does Not Support A Control Of Type " + typeof(Image).Name);
        //        }
        //    }
        //}

        //private enum Location
        //{
        //    Store = 0,
        //    Admin = 1
        //}

        //public static void GetInputsAndModifiersForAdminLineItemDescription(Page p, GridView gv)
        //{
        //    GetInputsAndModifiersForLineItemDescription(p, gv, Location.Admin);
        //}

        //public static void GetInputsAndModifiersForLineItemDescription(Page p, GridView gv)
        //{
        //    GetInputsAndModifiersForLineItemDescription(p, gv, Location.Store);
        //}

        //public static void DisplayKitInLineItem(Page p, GridView gv, bool displayCollapsible)
        //{
        //    foreach (GridViewRow row in gv.Rows) {
        //        BVSoftware.Commerce.Orders.LineItem lineItem = null;
        //        if (row.DataItem != null) {
        //            if (lineItem is BVSoftware.Commerce.Orders.LineItem) {
        //                lineItem = (BVSoftware.Commerce.Orders.LineItem)row.DataItem;
        //            }
        //        }
        //        else {
        //            lineItem = BVSoftware.Commerce.Orders.LineItem.FindByBvin((string)gv.DataKeys[row.RowIndex].Value);
        //            if (string.IsNullOrEmpty(lineItem.Bvin)) {
        //                lineItem = null;
        //            }
        //        }

        //        PlaceHolder kitDisplayPlaceHolder = (PlaceHolder)row.FindControl("KitDisplayPlaceHolder");

        //        if ((kitDisplayPlaceHolder != null) && (lineItem != null)) {
        //            if ((lineItem.KitSelections != null) && (!lineItem.KitSelections.IsEmpty)) {
        //                Collection<BVSoftware.Commerce.Catalog.KitPart> parts = BVSoftware.Commerce.Services.KitService.GetKitPartsForKitSelections(lineItem.KitSelections);
        //                if (parts.Count > 0) {
        //                    LiteralControl literal = new LiteralControl();
        //                    string kitDetails = BVSoftware.Commerce.Content.SiteTerms.GetTerm("KitDetails");

        //                    if (displayCollapsible) {
        //                        if (WebAppSettings.KitDisplayCollapsed) {
        //                            literal.Text += "<h4 class=\"kit-detail-header\">" + kitDetails + " <img class=\"collapsed\" src=\"Images/plus.png\" /></h4>";
        //                        }
        //                        else {
        //                            literal.Text += "<h4 class=\"kit-detail-header\">" + kitDetails + " <img class=\"expanded\" src=\"Images/minus.png\" /></h4>";
        //                        }
        //                    }

        //                    else {
        //                        literal.Text += "<h4 class=\"kit-detail-header\">" + kitDetails + "</h4>";
        //                    }

        //                    if (displayCollapsible) {
        //                        if (WebAppSettings.KitDisplayCollapsed) {
        //                            literal.Text += "<ul class=\"kit-detail-display\" style=\"display: none;\">";
        //                        }
        //                        else {
        //                            literal.Text += "<ul class=\"kit-detail-display\">";
        //                        }
        //                    }

        //                    else {
        //                        literal.Text += "<ul class=\"kit-detail-display\">";
        //                    }

        //                    foreach (BVSoftware.Commerce.Catalog.KitPart part in parts) {
        //                        literal.Text += "<li>" + part.Description + "</li>";
        //                    }
        //                    literal.Text += "</ul>";
        //                    kitDisplayPlaceHolder.Controls.Add(literal);
        //                }
        //            }
        //        }
        //    }
        //}

        //private static void GetInputsAndModifiersForLineItemDescription(Page p, GridView gv, Location @where)
        //{
        //    foreach (GridViewRow row in gv.Rows) {
        //        BVSoftware.Commerce.Orders.LineItem lineItem = null;
        //        if (row.DataItem != null) {
        //            if (lineItem is BVSoftware.Commerce.Orders.LineItem) {
        //                lineItem = (BVSoftware.Commerce.Orders.LineItem)row.DataItem;
        //            }
        //        }
        //        else {
        //            lineItem = BVSoftware.Commerce.Orders.LineItem.FindByBvin((string)gv.DataKeys[row.RowIndex].Value);
        //            if (string.IsNullOrEmpty(lineItem.Bvin)) {
        //                lineItem = null;
        //            }
        //        }

        //        PlaceHolder inputModifiersPlaceHolder = (PlaceHolder)row.FindControl("CartInputModifiersPlaceHolder");

        //        if ((inputModifiersPlaceHolder != null) && (lineItem != null)) {
        //            BVSoftware.Commerce.Catalog.Product product = BVSoftware.Commerce.Catalog.Product.FindByBvin(lineItem.ProductId);


        //            //int count = 0;
        //            //For Each item As Object In lineItem.GetCustomerVisibleInputsAndModifiers()
        //            //If TypeOf item Is Orders.LineItemInput Then
        //            //Dim input As Catalog.ProductInput = Catalog.ProductInput.FindByBvin(item.InputBvin)
        //            //If input IsNot Nothing Then
        //            //Dim inputTemplate As Content.ProductInputTemplate = Nothing
        //            //If where = Location.Admin Then
        //            //inputTemplate = Content.ModuleController.LoadProductInputAdminLineItemView(input.Type, p)
        //            //ElseIf where = Location.Store Then
        //            //inputTemplate = Content.ModuleController.LoadProductInputLineItemView(input.Type, p)
        //            //End If
        //            //If inputTemplate Is Nothing Then
        //            //Dim url As String = String.Empty
        //            //If where = Location.Store Then
        //            //If product IsNot Nothing Then
        //            //url = Utilities.UrlRewriter.BuildUrlForProduct(product, p, "LineItemId=" & HttpUtility.UrlEncode(lineItem.Bvin))
        //            //End If
        //            //End If                                

        //            //Dim inputDiv As New HtmlGenericControl("div")
        //            //inputModifiersPlaceHolder.Controls.Add(inputDiv)
        //            //inputDiv.Attributes.Add("class", "inputvalue")                                
        //            //If where = Location.Admin Then
        //            //inputDiv.InnerHtml = GetInputModifierDisplay(item.InputName, item.InputDisplayValue)
        //            //ElseIf where = Location.Store Then
        //            //Dim inputOutput As New HtmlAnchor()
        //            //inputDiv.Controls.Add(inputOutput)
        //            //inputOutput.HRef = url
        //            //inputOutput.InnerHtml = GetInputModifierDisplay(item.InputName, item.InputDisplayValue)
        //            //End If                                
        //            //Else
        //            //inputModifiersPlaceHolder.Controls.Add(inputTemplate)
        //            //inputTemplate.BlockId = input.Bvin
        //            //inputTemplate.ID = "inputTemplate" & count.ToString()
        //            //inputTemplate.SetValue(lineItem, item)
        //            //inputTemplate.InitializeDisplay()
        //            //End If
        //            //Else
        //            //Dim url As String = String.Empty
        //            //If where = Location.Store Then
        //            //If product IsNot Nothing Then
        //            //url = Utilities.UrlRewriter.BuildUrlForProduct(product, p, "LineItemId=" & HttpUtility.UrlEncode(lineItem.Bvin))
        //            //End If
        //            //End If                            

        //            //Dim inputDiv As New HtmlGenericControl("div")
        //            //inputModifiersPlaceHolder.Controls.Add(inputDiv)
        //            //inputDiv.Attributes.Add("class", "inputvalue")
        //            //If where = Location.Admin Then
        //            //inputDiv.InnerHtml = GetInputModifierDisplay(item.InputName, item.InputDisplayValue)
        //            //ElseIf where = Location.Store Then
        //            //Dim inputOutput As New HtmlAnchor()
        //            //inputDiv.Controls.Add(inputOutput)
        //            //inputOutput.HRef = url
        //            //inputOutput.InnerHtml = GetInputModifierDisplay(item.InputName, item.InputDisplayValue)
        //            //End If
        //            //End If
        //            //ElseIf TypeOf item Is Orders.LineItemModifier Then
        //            //Dim modifier As Catalog.ProductModifier = Catalog.ProductModifier.FindByBvin(item.ModifierBvin)
        //            //If modifier IsNot Nothing Then
        //            //Dim modifierTemplate As Content.ProductModifierTemplate = Nothing
        //            //If where = Location.Store Then
        //            //modifierTemplate = Content.ModuleController.LoadProductModifierLineItemView(modifier.Type, p)
        //            //ElseIf where = Location.Admin Then
        //            //modifierTemplate = Content.ModuleController.LoadProductModifierAdminLineItemView(modifier.Type, p)
        //            //End If

        //            //If modifierTemplate Is Nothing Then
        //            //Dim modifierOption As Catalog.ProductModifierOption = Catalog.ProductModifierOption.FindByBvin(item.ModifierValue)
        //            //If (modifierOption IsNot Nothing) AndAlso (Not modifierOption.IsNull) Then
        //            //Dim url As String = String.Empty
        //            //If product IsNot Nothing Then
        //            //url = Utilities.UrlRewriter.BuildUrlForProduct(product, p, "LineItemId=" & HttpUtility.UrlEncode(lineItem.Bvin))
        //            //End If

        //            //Dim modifierDiv As New HtmlGenericControl("div")
        //            //inputModifiersPlaceHolder.Controls.Add(modifierDiv)
        //            //modifierDiv.Attributes.Add("class", "modifiervalue")
        //            //If where = Location.Admin Then
        //            //modifierDiv.InnerHtml = GetInputModifierDisplay(item.ModifierName, modifierOption.DisplayText)
        //            //ElseIf where = Location.Store Then
        //            //Dim modifierOutput As New HtmlAnchor()
        //            //modifierDiv.Controls.Add(modifierOutput)
        //            //modifierOutput.HRef = url
        //            //modifierOutput.InnerHtml = GetInputModifierDisplay(item.ModifierName, modifierOption.DisplayText)
        //            //End If                                    
        //            //End If
        //            //Else
        //            //modifierTemplate.BlockId = modifier.Bvin
        //            //inputModifiersPlaceHolder.Controls.Add(modifierTemplate)
        //            //modifierTemplate.ID = "modifierTemplate" & count.ToString()
        //            //modifierTemplate.Product = lineItem.AssociatedProduct
        //            //modifierTemplate.SetValue(lineItem, item)
        //            //modifierTemplate.InitializeDisplay()
        //            //End If
        //            //Else
        //            //Dim modifierOption As Catalog.ProductModifierOption = Catalog.ProductModifierOption.FindByBvin(item.ModifierValue)
        //            //If (modifierOption IsNot Nothing) AndAlso (Not modifierOption.IsNull) Then
        //            //Dim url As String = String.Empty
        //            //If product IsNot Nothing Then
        //            //url = Utilities.UrlRewriter.BuildUrlForProduct(product, p, "LineItemId=" & HttpUtility.UrlEncode(lineItem.Bvin))
        //            //End If

        //            //Dim modifierDiv As New HtmlGenericControl("div")
        //            //inputModifiersPlaceHolder.Controls.Add(modifierDiv)
        //            //modifierDiv.Attributes.Add("class", "modifiervalue")
        //            //If where = Location.Admin Then
        //            //modifierDiv.InnerHtml = GetInputModifierDisplay(item.ModifierName, modifierOption.DisplayText)
        //            //ElseIf where = Location.Store Then
        //            //Dim modifierOutput As New HtmlAnchor()
        //            //modifierDiv.Controls.Add(modifierOutput)
        //            //modifierOutput.HRef = url
        //            //modifierOutput.InnerHtml = GetInputModifierDisplay(item.ModifierName, modifierOption.DisplayText)
        //            //End If
        //            //End If
        //            //End If
        //            //End If
        //            //count += 1
        //            //Next
        //        }
        //    }
        //}

        //public static string GetInputModifierDisplay(string name, string displayValue)
        //{
        //    return "<span class=\"inputmodifiername\">" + HttpUtility.HtmlEncode(name) + " </span><span class=\"inputmodifiervalue\">" + HttpUtility.HtmlEncode(displayValue) + "</span>";
        //}

        public static string GetAdditionalImagesPopupJavascript(string productId, Page currentPage)
        {
            int w = 700;
            int h = 600;
            return "JavaScript:window.open('" + currentPage.ResolveUrl("~/ZoomImage.aspx") + "?productID=" + productId + "','Images','width=" + w + ", height=" + h + ", menubar=no, scrollbars=yes, resizable=yes, status=no, toolbar=no')";
        }

        public static bool DownloadFile(BVSoftware.Commerce.Catalog.ProductFile file)
        {
            string extension = System.IO.Path.GetExtension(file.FileName);
            string name = System.IO.Path.GetFileName(file.FileName);
            double fileSize = 0;

            long storeId = RequestContext.GetCurrentRequestContext().CurrentStore.Id;
            string diskFileName = file.Bvin + "_" + file.FileName + ".config";
            if (!BVSoftware.Commerce.Storage.DiskStorage.FileVaultFileExists(storeId, diskFileName)) return false;

            byte[] bytes = BVSoftware.Commerce.Storage.DiskStorage.FileVaultGetBytes(storeId, diskFileName);

            string type = "";
            type = BVSoftware.Commerce.Utilities.MimeTypes.FindTypeForExtension(extension);

            //System.IO.FileInfo f = new System.IO.FileInfo(filePath);
            fileSize = bytes.Length;// f.Length;

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();

            if (type != "")
            {
                HttpContext.Current.Response.ContentType = type;
            }

            //If type = "application/octet-stream" Then
            HttpContext.Current.Response.AppendHeader("content-disposition", "attachment; filename=" + name);
            //End If

            if (fileSize > 0)
            {
                HttpContext.Current.Response.AddHeader("Content-Length", fileSize.ToString());
            }

            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();

            return true;
        }
    }
}