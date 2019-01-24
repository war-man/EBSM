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
using EBSM.Entities;
using EBSM.Services;

namespace WarehouseApp.Controllers
{
    [Authorize]
    public class ProductAttributeController : Controller
    {
      private ProductAttributeService _productAttributeService = new ProductAttributeService();

        // showing the list of attribute type list function 
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult AttributeTypeList(AttributeSearchViewModel model)
        {
            var productAttributes = _productAttributeService.GetAllAttributes(model.AttName).ToList();
            model.Attributes = productAttributes.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/ProductAttribute/AttributeTypeList", model);
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
                attribute.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                attribute.CreatedDate = DateTime.Now;
                _productAttributeService.SaveAttribute(attribute, AuthenticatedUser.GetUserFromIdentity().UserId);
               
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
            ProductAttribute productAttribute = _productAttributeService.GetProductAttributeById(id.Value);
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
                ProductAttribute attribute = _productAttributeService.GetProductAttributeById(productAttribute.AttributeId); 
                attribute.AttributeName = productAttribute.AttributeName;
                attribute.AttributeType = productAttribute.AttributeType;
                attribute.IsRequired = productAttribute.IsRequired;
                attribute.Values = values;
                attribute.DefaultValue = defaultValue;
                attribute.Status = productAttribute.Status;
                attribute.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                attribute.UpdatedDate = DateTime.Now;
                _productAttributeService.EditAttribute(attribute, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                return RedirectToAction("ListArrtibuteType");
            }
            return View("../Shop/ProductAttribute/NewProductAttribute", productAttribute);
        }
        // GET: /Product/Edit/5
      [Roles("Global_SupAdmin,Configuration")]
        public ActionResult AttributeSetList(AttributeSetSearchViewModel model)
        {
            var attributeSets =_productAttributeService.GetAllAttributeSets(model.AttributeSetName).ToList();
            model.AttributeSets = attributeSets.ToPagedList(model.Page, model.PageSize);
            return View("../Shop/ProductAttribute/AttributeSetList", model);
        }
      [Roles("Global_SupAdmin,Configuration")]
        public ActionResult NewAttributeSet()
        {
            ViewBag.SelectedProductAttributeList = _productAttributeService.GetAllByName("unit").ToList();
            ViewBag.NotSelectedProductAttributeList = _productAttributeService.GetAllExceptThisName("unit").ToList(); 
            return View("../Shop/ProductAttribute/NewAttributeSet");
        }
        // save attibute Set into database when user save information
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult NewAttributeSet(AttributeSetViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<AttributeSetAttribute> SelectedAttributes = new List<AttributeSetAttribute>();
                ProductAttributeSet attributeSet = new ProductAttributeSet()
                    {
                        AttributeSetName = model.AttributeSetName,
                        Status = 1,
                        CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                        CreatedDate = DateTime.Now
                    };
              
                    foreach (var item in model.AttributeIds)
                    {
                        AttributeSetAttribute attributeSetAttribute = new AttributeSetAttribute()
                        {
                            AttributeSetId = attributeSet.AttributeSetId,
                            AttributeId = item,
                        };
                    SelectedAttributes.Add(attributeSetAttribute);
                    }
                attributeSet.AttributeSetAttributes = SelectedAttributes;
                _productAttributeService.SaveAttributeSet(attributeSet, AuthenticatedUser.GetUserFromIdentity().UserId);
                
                var data = new { result = "Ok", returnUrl=@Url.Action("AttributeSetList","ProductAttribute") };
                    return Json(data, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("AttributeSetList");
                }

            ViewBag.ProductAttributeList = _productAttributeService.GetAllAttributes().Where(x => x.Status != 0).OrderBy(x => x.AttributeName).ToList();
            return Json(false, JsonRequestBehavior.AllowGet);
                //return View("../Shop/ProductAttribute/NewAttributeSet");
            }

        public ActionResult EditAttributeSet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductAttributeSet productAttributeSet = _productAttributeService.GetProductAttributeSetById(id.Value);
            if (productAttributeSet == null)
            {
                return HttpNotFound();
            }
            AttributeSetViewModel model = new AttributeSetViewModel();
            var attributeList = _productAttributeService.GetAllAttributes().Where(x => x.Status != 0).ToList();
            List<ProductAttribute>unSelectedList=new List<ProductAttribute>();
            unSelectedList = _productAttributeService.GetAllAttributes().Where(x => x.Status != 0).ToList();
            List<AttributeSetAttribute> attributeSetAttributes =_productAttributeService.GetAttributesByAttributeSetId(id.Value).ToList();
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
                    ProductAttributeSet attributeSet = _productAttributeService.GetProductAttributeSetById(model.AttributeSetId);
                    attributeSet.AttributeSetName = model.AttributeSetName;
                    attributeSet.Status = model.Status;
                    attributeSet.UpdatedBy =AuthenticatedUser.GetUserFromIdentity().UserId;
                    attributeSet.UpdatedDate = DateTime.Now;
                _productAttributeService.EditAttributeSet(attributeSet, AuthenticatedUser.GetUserFromIdentity().UserId);
                    

                    //====remove AttributeSetAttributes=======================
                    var attributeSetAttributes = _productAttributeService.GetAttributesByAttributeSetId(attributeSet.AttributeSetId).ToList();
                    if(attributeSetAttributes.Count>0){
                    _productAttributeService.DeleteAttributeSetAttributeListFromDb(attributeSetAttributes);
                    }

                    //====add AttributeSetAttributes=======================
                    if (model.AttributeIds.Any()) {
                    _productAttributeService.SaveAttributeSetAttributeByAttributeId(attributeSet.AttributeSetId, model.AttributeIds, AuthenticatedUser.GetUserFromIdentity().UserId);
                    
                    }

                    var data = new { result = "Ok",returnUrl=@Url.Action("AttributeSetList","ProductAttribute") };
                    return Json(data, JsonRequestBehavior.AllowGet);
               
            }
            ViewBag.ProductAttributeList = _productAttributeService.GetAllAttributes().Where(x => x.Status != 0).OrderBy(x => x.AttributeName).ToList();
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        public JsonResult IsNameUsed(string AttributeName, string InitialAttributeName)
        {
            bool isNotExist = _productAttributeService.IsAttNameUsed(AttributeName, InitialAttributeName);
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsAttributeSetNameUsed(string AttributeSetName, string InitialAttributeSetName)
        {
            bool isNotExist = _productAttributeService.IsAttributeSetNameUsed(AttributeSetName, InitialAttributeSetName);
           
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }

        //Except Attribute for not taging in product name
        public static bool ExceptAttribute(int? id)
        {
            var cx = new ProductAttributeService();
            bool isExist = cx.IsExceptAttribute(id, "unit");
            return isExist;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productAttributeService.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//