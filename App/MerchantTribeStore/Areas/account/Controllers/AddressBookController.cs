using System.Collections.Generic;
using System.Web.Mvc;
using BVCommerce.Controllers.Shared;
using BVCommerce.Filters;
using BVCommerce.Models;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Membership;

namespace BVCommerce.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class AddressBookController : BaseStoreController
    {
        //
        // GET: /account/AddressBook/
        public ActionResult Index()
        {
            ViewBag.Title = "Address Book";
            ViewBag.MetaDescription = "Address Book | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountaddressbookpage";
            ViewBag.EditButtonUrl = BVApp.ThemeManager().ButtonUrl("edit", Request.IsSecureConnection);
            ViewBag.DeleteButtonUrl = BVApp.ThemeManager().ButtonUrl("x", Request.IsSecureConnection);
            ViewBag.AddNewButtonUrl = BVApp.ThemeManager().ButtonUrl("new", Request.IsSecureConnection);

            List<Address> addresses = LoadAddresses();

            return View(addresses);
        }
        private List<Address> LoadAddresses()
        {
            CustomerAccount u = BVApp.CurrentCustomer;
            if (u != null)
            {
                return u.Addresses;
            }
            return new List<Address>();
        }

        //
        // POST: /account/AddressBook/Delete/{id}
        [HttpPost]
        public ActionResult Delete(string id)
        {
            CustomerAccount u = BVApp.CurrentCustomer;
            if (u != null)
            {
                u.DeleteAddress(id);
                BVApp.MembershipServices.UpdateCustomer(u);
            }

            return RedirectToAction("Index");
        }

        private void EditSetup()
        {
            ViewBag.Title = "Edit Address";
            ViewBag.MetaDescription = "Edit Address | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountaddresseditpage";
            ViewBag.SaveButtonUrl = BVApp.ThemeManager().ButtonUrl("savechanges", Request.IsSecureConnection);
        }        
        // GET: /account/AddressBook/Edit/{id}
        public ActionResult Edit(string id)
        {
            EditSetup();

            Address a = LoadAddress(id);            
            return View(new AddressViewModel(a));
        }
        private Address LoadAddress(string bvin)
        {
            CustomerAccount u = BVApp.CurrentCustomer;
            if (u != null)
            {
                switch (bvin.ToLower())
                {
                    case "new":
                        BVSoftware.Commerce.Contacts.Address a = new BVSoftware.Commerce.Contacts.Address();
                        a.Bvin = System.Guid.NewGuid().ToString();
                        return a;                        
                    default:
                        foreach (BVSoftware.Commerce.Contacts.Address a2 in u.Addresses)
                        {
                            if (a2.Bvin == bvin)
                            {
                                return a2;                                
                            }
                        }
                        break;
                }
            }

            return new Address();
        }

        // 
        // POST: /account/AddressBook/Edit/{id}
        [ActionName("Edit")]
        [HttpPost]
        public ActionResult EditPosted(string id, FormCollection posted)
        {
            EditSetup();
                        
            AddressViewModel model = new AddressViewModel();
            if (TryUpdateModel(model))
            {
                CustomerAccount u = BVApp.MembershipServices.Customers.Find(SessionManager.GetCurrentUserId());
                if (u == null) return View(model);
                
                Address a = LoadAddress(id);
                model.CopyTo(a);
                                
                string slug = id;
                switch (slug.ToLower())
                {
                    case "new":
                        BVApp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(u, a);
                        BVApp.MembershipServices.UpdateCustomer(u);
                        break;
                    default:
                        u.UpdateAddress(a);
                        BVApp.MembershipServices.UpdateCustomer(u);
                        break;
                }
                return RedirectToAction("Index");
            }
                       
            return View(model);
        }

    }


}
