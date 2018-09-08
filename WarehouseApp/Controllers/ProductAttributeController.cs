using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

using System.Net;
using System.Data.Entity;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class ProductAttributeController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // showing the list of attribute type list function 
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult ListArrtibuteType(AttributeSearchViewModel model)
        {
            var productAttributes = db.ProductAttributes.Where(x => (model.AttName == null || x.AttributeName.StartsWith(model.AttName))).OrderBy(x => x.AttributeName).ToList();
            model.Attributes = productAttributes.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/ProductAttribute/ListArrtibuteType", model);
        }

        // showing the list to attibute set functoin


        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult NewProductAttribute()
        {
            AttributeViewModel attribute=new AttributeViewModel();
            attribute.Values = new List<AttributeValue> { new AttributeValue { Value = "", IsDefault = false} };
            return View("../Shop/ProductAttribute/NewProductAttribute", attribute);
        }

        // cteate attibute set 

        // save attibute type into database when user save information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewProductAttribute(AttributeViewModel productAttribute)
        {
            if (ModelState.IsValid)
            {
                string values = " ";
                string defaultValue = " ";


                if (productAttribute.AttributeType == "Radio" || productAttribute.AttributeType == "Selectbox")
                {
                    if (productAttribute.Values.Count > 0 && (!productAttribute.Values.Any(x => String.IsNullOrEmpty(x.Value))))
                    {
                        values = String.Join(",", productAttribute.Values.Select(x => x.Value).ToArray());
                        var findDefault = productAttribute.Values.FirstOrDefault(x => x.IsDefault);
                        if (findDefault != null)
                            defaultValue = findDefault.Value;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Values should not be empty !");
                        return View("../Shop/ProductAttribute/NewProductAttribute", productAttribute);
                    }
                }
                ProductAttribute attribute = new ProductAttribute();
                attribute.AttributeName = productAttribute.AttributeName;
                attribute.AttributeType = productAttribute.AttributeType;
                attribute.IsRequired = productAttribute.IsRequired;
                attribute.Values = values;
                attribute.DefaultValue = defaultValue;
                attribute.Status = 1;
                attribute.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                attribute.CreatedDate = DateTime.Now;
                db.ProductAttributes.Add(attribute);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("ListArrtibuteType");
            }
            return View("../Shop/ProductAttribute/NewProductAttribute", productAttribute);
        }
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult EditProductAttribute(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAttribute productAttribute = db.ProductAttributes.Find(id);
            if (productAttribute == null)
            {
                return HttpNotFound();
            }
            AttributeViewModel model =new AttributeViewModel();
            model.AttributeId = productAttribute.AttributeId;
            model.AttributeName = productAttribute.AttributeName;
            model.AttributeType = productAttribute.AttributeType;
            model.IsRequired = productAttribute.IsRequired;
            model.Status = productAttribute.Status;

            List<AttributeValue> listOfValue = new List<AttributeValue>();
            if (!String.IsNullOrEmpty(productAttribute.Values))
            {
                string[] values = productAttribute.Values.Split(',');
                if (values.Length > 0)
                {
                    foreach (var item in values)
                    {
                        AttributeValue val = new AttributeValue();
                        val.Value = item;
                        if (item == productAttribute.DefaultValue && !string.IsNullOrEmpty(item))
                        {
                            val.IsDefault = true;
                        }
                        listOfValue.Add(val);
                    }
                }
                model.Values = listOfValue;
            }
            else
            {
                model.Values=new List<AttributeValue> { new AttributeValue { Value = "", IsDefault = false } };
            }
            

            return View("../Shop/ProductAttribute/EditProductAttribute", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductAttribute(AttributeViewModel productAttribute)
        {
            if (ModelState.IsValid)
            {
                string values = " ";
                string defaultValue = " ";

                if (productAttribute.AttributeType == "Radio" || productAttribute.AttributeType == "Selectbox")
                {
                    if (productAttribute.Values.Count > 0 && (!productAttribute.Values.Any(x => String.IsNullOrEmpty(x.Value))))
                    {
                        values = String.Join(",", productAttribute.Values.Select(x => x.Value).ToArray());
                        var findDefault = productAttribute.Values.FirstOrDefault(x => x.IsDefault);
                        if (findDefault != null)
                            defaultValue = findDefault.Value;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Values should not be empty !");
                        return View("../Shop/ProductAttribute/EditProductAttribute", productAttribute);
                    }
                }
                ProductAttribute attribute = db.ProductAttributes.Find(productAttribute.AttributeId);
                attribute.AttributeName = productAttribute.AttributeName;
                attribute.AttributeType = productAttribute.AttributeType;
                attribute.IsRequired = productAttribute.IsRequired;
                attribute.Values = values;
                attribute.DefaultValue = defaultValue;
                attribute.Status = productAttribute.Status;
                attribute.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                attribute.UpdatedDate = DateTime.Now;
                db.Entry(attribute).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("ListArrtibuteType");
            }
            return View("../Shop/ProductAttribute/NewProductAttribute", productAttribute);
        }
        // GET: /Product/Edit/5
      [Roles("Global_SupAdmin,Configuration")]
        public ActionResult AttributeSetList(AttributeSetSearchViewModel model)
        {
            var attributeSets = db.ProductAttributeSets.Where(x => (model.AttributeSetName == null || x.AttributeSetName.StartsWith(model.AttributeSetName))).OrderBy(x => x.AttributeSetName).ToList();
            model.AttributeSets = attributeSets.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/ProductAttribute/AttributeSetList", model);
        }
      [Roles("Global_SupAdmin,Configuration")]
        public ActionResult NewAttributeSet()
        {
            ViewBag.SelectedProductAttributeList = db.ProductAttributes.Where(x => x.Status != 0 && x.AttributeName.ToLower()=="unit").OrderBy(x=>x.AttributeName).ToList();
            ViewBag.NotSelectedProductAttributeList = db.ProductAttributes.Where(x => x.Status != 0 && x.AttributeName.ToLower() != "unit").OrderBy(x => x.AttributeName).ToList();
            return View("../Shop/ProductAttribute/NewAttributeSet");
        }
        // save attibute Set into database when user save information
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult NewAttributeSet(AttributeSetViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                    ProductAttributeSet attributeSet = new ProductAttributeSet()
                    {
                        AttributeSetName = model.AttributeSetName,
                        Status = 1,
                        CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                        CreatedDate = DateTime.Now
                    };
                    db.ProductAttributeSets.Add(attributeSet);
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());


                    foreach (var item in model.AttributeIds)
                    {
                        AttributeSetAttribute attributeSetAttribute = new AttributeSetAttribute()
                        {
                            AttributeSetId = attributeSet.AttributeSetId,
                            AttributeId = item,
                        };
                        db.AttributeSetAttributes.Add(attributeSetAttribute);
                    }
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                    var data = new { result = "Ok", returnUrl=@Url.Action("AttributeSetList","ProductAttribute") };
                    return Json(data, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("AttributeSetList");
                }

            ViewBag.ProductAttributeList = db.ProductAttributes.Where(x => x.Status != 0).OrderBy(x => x.AttributeName).ToList();
            return Json(false, JsonRequestBehavior.AllowGet);
                //return View("../Shop/ProductAttribute/NewAttributeSet");
            }

        public ActionResult EditAttributeSet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAttributeSet productAttributeSet = db.ProductAttributeSets.Find(id);
            if (productAttributeSet == null)
            {
                return HttpNotFound();
            }
            AttributeSetViewModel model = new AttributeSetViewModel();
            var attributeList = db.ProductAttributes.Where(x => x.Status == 1).ToList();
            List<ProductAttribute>unSelectedList=new List<ProductAttribute>();
            unSelectedList = db.ProductAttributes.Where(x => x.Status == 1).ToList();
            List<AttributeSetAttribute> attributeSetAttributes = db.AttributeSetAttributes.Where(x => x.AttributeSetId == id).ToList();
            List<ProductAttribute> selectedAttributeList = new List<ProductAttribute>();
            foreach (var item in attributeList)
            {
                if (attributeSetAttributes.Any(x => x.AttributeId == item.AttributeId))
                {
                   selectedAttributeList.Add(item);
                    unSelectedList.Remove(item);
                }

            }
            model.AttributeSetId = productAttributeSet.AttributeSetId;
            model.AttributeSetName = productAttributeSet.AttributeSetName;
            model.Status = productAttributeSet.Status;
            model.SelectedAttribteList = selectedAttributeList;
            ViewBag.UnselectedList = unSelectedList;
            return View("../Shop/ProductAttribute/EditAttributeSet", model);
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public ActionResult EditAttributeSet(AttributeSetViewModel model)
        {
            if (ModelState.IsValid)
            {
                    ProductAttributeSet attributeSet = db.ProductAttributeSets.Find(model.AttributeSetId);
                    attributeSet.AttributeSetName = model.AttributeSetName;
                    attributeSet.Status = model.Status;
                    attributeSet.UpdatedBy =Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                    attributeSet.UpdatedDate = DateTime.Now;

                    db.Entry(attributeSet).State = EntityState.Modified;
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    //====remove AttributeSetAttributes=======================
                    var attributeSetAttributes =db.AttributeSetAttributes.Where(x => x.AttributeSetId == attributeSet.AttributeSetId).ToList();
                    if(attributeSetAttributes.Count>0){
                            foreach (var removeItem in attributeSetAttributes)
                            {
                                db.AttributeSetAttributes.Remove(removeItem);
                            }
                            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                    }

                    //====add AttributeSetAttributes=======================
                    if (model.AttributeIds.Any()) { 
                    foreach (var item in model.AttributeIds)
                    {
                        AttributeSetAttribute attributeSetAttribute = new AttributeSetAttribute()
                        {
                            AttributeSetId = attributeSet.AttributeSetId,
                            AttributeId = item,
                        };
                        db.AttributeSetAttributes.Add(attributeSetAttribute);
                    }
                    }
                    db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                    var data = new { result = "Ok",returnUrl=@Url.Action("AttributeSetList","ProductAttribute") };
                    return Json(data, JsonRequestBehavior.AllowGet);
               
            }
            ViewBag.ProductAttributeList = db.ProductAttributes.Where(x => x.Status == 1).ToList();
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        public JsonResult IsNameUsed(string AttributeName, string InitialAttributeName)
        {
            bool isNotExist = true;
            if (AttributeName != string.Empty && InitialAttributeName == "undefined")
            {
                var isExist = db.ProductAttributes.Any(x => x.AttributeName.ToLower().Equals(AttributeName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (AttributeName != string.Empty && InitialAttributeName != "undefined")
                {
                    var isExist = db.ProductAttributes.Any(x => x.AttributeName.ToLower() == AttributeName.ToLower() && x.AttributeName.ToLower() != InitialAttributeName.ToLower());
                    if (isExist)
                    {
                        isNotExist = false;
                    }
                }
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsAttributeSetNameUsed(string AttributeSetName, string InitialAttributeSetName)
        {
            bool isNotExist = true;
            if (AttributeSetName != string.Empty && InitialAttributeSetName == "undefined")
            {
                var isExist = db.ProductAttributeSets.Any(x => x.AttributeSetName.ToLower().Equals(AttributeSetName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (AttributeSetName != string.Empty && InitialAttributeSetName != "undefined")
            {
                var isExist = db.ProductAttributeSets.Any(x => x.AttributeSetName.ToLower() == AttributeSetName.ToLower() && x.AttributeSetName.ToLower() != InitialAttributeSetName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }

        //Except Attribute for not taging in product name
        public static bool ExceptAttribute(int? id)
        {
            bool isExist = false;
            using (var cx = new WmsDbContext())
            {
                var attribute = cx.ProductAttributes.FirstOrDefault(x => x.AttributeId == id);
                if (attribute != null) { isExist = attribute.AttributeName.ToLower() == "unit"; }
                    
            }
            return isExist;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//