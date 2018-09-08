using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private WmsDbContext db = new WmsDbContext();

        // GET: /Category/

        [Roles("Global_SupAdmin,Configuration")]
        [OutputCache(Duration = 30)]
        public ActionResult Index(CategorySearchViewModel model)
        {
            
            var categoriesTree = CategoryTree().Where(x => (model.CategoryName == null || x.CategoryName.StartsWith(model.CategoryName))).OrderBy(x => x.CategoryName).ToList();
            model.Categories = categoriesTree.ToPagedList(model.Page, model.PageSize);
            ViewBag.CategoryParentId = new SelectList(CategoryTree().Where(x => x.Status > 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName");

            return View("../Shop/Category/Index", model);
        } 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Count = 0;
                category.Status = 1;
                category.CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                category.CreatedDate = DateTime.Now;
                db.Categories.Add(category);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }
            ViewBag.CategoryParentId = new SelectList(CategoryTree().Where(x => x.Status > 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName", category.CategoryParentId);
            return View("../Shop/Category/Create",category);
        }

        // GET: /Product/Edit/5
        [Roles("Global_SupAdmin,Configuration")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var catsTree = CategoryTree().Where(x => x.Status > 0 && x.CategoryId != id && x.CategoryParentId!=id).OrderBy(x => x.CategoryName).ToList();
            List<Category> categoryParentIds =new List<Category>();
            if (catsTree.Count > 0)
            {
                foreach (var item in catsTree)
                {
                    if (item.CategoryParent==null|| item.CategoryParent.CategoryParentId != id)
                    {
                        categoryParentIds.Add(item);
                    }
                }
            } 
            ViewBag.CategoryParentId = new SelectList(categoryParentIds, "CategoryId", "CategoryName", category.CategoryParentId);           
            return View("../Shop/Category/Edit",category);
        }

        // POST: /Category/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                Category cat = db.Categories.Find(category.CategoryId);
                cat.CategoryName = category.CategoryName;
                cat.CategoryParentId = category.CategoryParentId;
                cat.Status = category.Status;
                cat.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                cat.UpdatedDate = DateTime.Now;
                db.Entry(cat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            var catsTree = CategoryTree().Where(x => x.Status > 0 && x.CategoryId != category.CategoryId && x.CategoryParentId != category.CategoryId).OrderBy(x => x.CategoryName).ToList();
            ViewBag.CategoryParentId = new SelectList(catsTree, "CategoryId", "CategoryName", category.CategoryParentId);
            return View("../Shop/Category/Edit", category);
        }

        // GET: /DailyFxRate/Delete/5
        public ActionResult DeleteRecord(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            db.Categories.Remove(category);
             db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
            return Json(new { Result = "OK" });
        }

        public static List<Category> CategoryTree()
        {
            var cx = new WmsDbContext();
            List<Category> categoryTree = new List<Category>();
            var allRootCategories = cx.Categories.Where(x => !x.CategoryParentId.HasValue).ToList();
            if (allRootCategories.Count > 0) { 
            foreach (var cat in allRootCategories)
            {
                categoryTree.Add(cat);
                if (cat.ChildCategories != null)
                {
                    var cateryList = BuildNestedItem(categoryTree, cat.ChildCategories.ToList());

                }
            }
            }
            return categoryTree;
        }

        private static List<Category> BuildNestedItem(List<Category> genratedItems, List<Category> items)
        {
            foreach (var childItem in items)
            {
                childItem.CategoryName = childItem.CategoryParent.CategoryName + " > " + childItem.CategoryName;
                genratedItems.Add(childItem);
                if (childItem.ChildCategories != null)
                {
                    BuildNestedItem(genratedItems, childItem.ChildCategories.ToList());
                }
            }

            return genratedItems;
        }

        public JsonResult IsNameUsed(string CategoryName, string InitialCategoryName)
        {
            bool isNotExist = true;
            if (CategoryName != string.Empty && InitialCategoryName == "undefined")
            {
                var isExist = db.Categories.Any(x => x.CategoryName.ToLower().Equals(CategoryName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (CategoryName != string.Empty && InitialCategoryName != "undefined")
            {
                var isExist = db.Categories.Any(x => x.CategoryName.ToLower() == CategoryName.ToLower() && x.CategoryName.ToLower() != InitialCategoryName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
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