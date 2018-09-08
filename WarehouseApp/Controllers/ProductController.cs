﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OfficeOpenXml;
using PagedList;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;


namespace WarehouseApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private  WmsDbContext db = new WmsDbContext();

        // GET: /Product/
        [Roles("Global_SupAdmin,Configuration")]
        [OutputCache(Duration = 20)]
        public ActionResult Index(ProductSearchViewModel model)
        {
            // To Bind the category drop down in search section
            ViewBag.CatId = new SelectList(CategoryController.CategoryTree().Where(x => x.Status != 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName");
            ViewBag.GroupNameId = new SelectList(db.Groups.Where(x => x.Status != 0).OrderBy(x => x.GroupName), "GroupNameId", "GroupName");
            ViewBag.ManufacId = new SelectList(db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 2).OrderBy(x=>x.SupplierName), "SupplierId", "SupplierName");
            
            // Get Products
            var products = db.Products.Where(x => (model.PName == null || (x.ProductFullName.StartsWith(model.PName) || x.ProductFullName.Contains(" " + model.PName)))
                    && (model.PCode == null || x.ProductCode.ToLower().StartsWith(model.PCode.ToLower()))
                    && (model.GroupNameId == null || x.GroupNameId==model.GroupNameId)
                    && (model.ManufacId == null || x.ManufacturerId == model.ManufacId)
                     && (model.CatId == null || x.ProductCategories.Any(y=>y.CategoryId== model.CatId))
                    &&(model.Status == null || x.Status==model.Status)
                    && (model.Price == null || x.Tp < model.Price)).OrderBy(o => o.ProductName).ToList();
      
            model.Products = products.ToPagedList(model.Page, model.PageSize);
            ViewBag.AttributeSetId = db.ProductAttributeSets.OrderBy(x => x.AttributeSetName).ToList();
            return View("../Shop/Product/Index",model);
        }

        #region product add
        [Roles("Global_SupAdmin,Configuration")]
        [HttpGet]
        public ActionResult AddProduct(int attributeSetId)
        {
            ViewBag.Attributes = db.AttributeSetAttributes.Where(x => x.AttributeSetId == attributeSetId).Select(x=>x.Attribute).ToList();
            ViewBag.Categories=db.Categories.Where(x => !x.CategoryParentId.HasValue).ToList();
            ViewBag.CategoryIds = new MultiSelectList(CategoryController.CategoryTree().Where(x => x.Status != 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName");
            ViewBag.GroupNameId = new SelectList(db.Groups.Where(x => x.Status != 0).OrderBy(x => x.GroupName), "GroupNameId", "GroupName");
            ViewBag.ManufacturerId = new SelectList(db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 2).OrderBy(x => x.SupplierName), "SupplierId", "SupplierName");
            ViewBag.ZoneId = new SelectList(db.WarehouseZones.Where(x => x.Status != 0).OrderBy(x => x.ZoneName), "ZoneId", "ZoneName");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.Status != 0).OrderBy(x => x.CustomerId), "CustomerId", "FullName");

            var attributeSet = db.ProductAttributeSets.Find(attributeSetId);
            if (attributeSet == null)
            {
                return HttpNotFound();
            }
            AddProductViewModel addProduct = new AddProductViewModel()
            {
                AttributeSetId = attributeSet.AttributeSetId,
                AttributeSet = attributeSet
                
            };

            addProduct.CustomerOptionList=new List<ProductCustomerOption>
            {
                new ProductCustomerOption{CustomerId = 0, ProductDescription = "", ProductCode = ""}
            };
            return View("../Shop/Product/AddProduct", addProduct);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(AddProductViewModel productModel)
        {
            if (ModelState.IsValid)
            {
                //append attribute with product name==================
                string productAttributes = "";
              if(productModel.ProductAtts.Any()){
                foreach (var att in productModel.ProductAtts)
                {
                    if (!String.IsNullOrEmpty(att.Value) && !ProductAttributeController.ExceptAttribute(att.AttributeId))
                    {
                        productAttributes+="-" + att.Value;
                    }
                }
            }
                 // add product information=================================
                var product = new Product()
                {
                    ProductCode = productModel.ProductCode,
                    ProductName = productModel.ProductName,
                    ProductFullName = productModel.ProductName + productAttributes,
                 
                    Details = productModel.Details,
                    Tp = productModel.Tp ?? 0,
                    Dp = productModel.Dp ?? 0,
                    DiscountType = productModel.DiscountType,
                    DiscountAmount = productModel.DiscountAmount ?? 0,
                    Vat = productModel.Vat ?? 0,
                    ExpiryDate = String.IsNullOrEmpty(productModel.ExpiryDate)?(DateTime?)null: Convert.ToDateTime(productModel.ExpiryDate),
                    DefaultZoneId = productModel.DefaultZoneId,
                    ManufacturerId = productModel.ManufacturerId,
                    MinStockLimit = productModel.MinStockLimit ?? 0,
                    GroupNameId = productModel.GroupNameId,
                    ProductImage = productModel.ProductImage,
                    AttributeSetId = productModel.AttributeSetId,
                    Status = productModel.Status,
                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                    CreatedDate = DateTime.Now,
                };
                db.Products.Add(product);
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                //add product attributes relationship===============================================
                if (productModel.ProductAtts.Any())
                {
                    foreach (var att in productModel.ProductAtts)
                    {
                        if (att.ProdAttCheckboxes != null)
                        {
                            foreach (var chk in att.ProdAttCheckboxes)
                            {
                                if (chk.IsChecked)
                                {
                                    att.Value += chk.Value + ",";

                                }
                            }
                            if (!String.IsNullOrEmpty(att.Value))
                            {
                                att.Value = att.Value.Trim(',');
                            }
                            //att.Value = att.Value.Remove(att.Value.LastIndexOf(','), 1);    //this is another way 
                        }

                        var productAtts = new ProductAttributeRelation()
                        {
                            ProductId = product.ProductId,
                            AttributeId = att.AttributeId,
                            Value = att.Value
                        };
                        db.ProductAttributeRelations.Add(productAtts);
                    }
                }
                //add product categories relationship===============================================
                if (productModel.CategoryIds!=null) { 
                foreach (var cat in productModel.CategoryIds)
                {
                    var productCat = new ProductCategory()
                    {
                        ProductId = product.ProductId,
                        CategoryId = cat,
                    };
                    db.ProductCategories.Add(productCat);
                }
                }

                //add product customer options===============================================
                if (productModel.CustomerOptionList != null)
                {
                    foreach (var customerOption in productModel.CustomerOptionList)
                    {
                        if (customerOption.CustomerId != null)
                        {
                            var customerRelation = new ProductCustomerRelation()
                            {
                                ProductId = product.ProductId,
                                CustomerId = Convert.ToInt32(customerOption.CustomerId),
                                ProductDescription = customerOption.ProductDescription,
                                ProductCode = customerOption.ProductCode,
                                UnitPrice = customerOption.UnitPrice,
                                Mrp = customerOption.Mrp,
                            };
                            db.ProductCustomerRelations.Add(customerRelation);
                        }
                       
                    }
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }

            ViewBag.Attributes = db.AttributeSetAttributes.Where(x => x.AttributeSetId == productModel.AttributeSetId).Select(x => x.Attribute).ToList();
            ViewBag.Categories = db.Categories.Where(x => !x.CategoryParentId.HasValue).ToList();
            ViewBag.CategoryIds = new MultiSelectList(CategoryController.CategoryTree().Where(x => x.Status != 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName", productModel.CategoryIds);
            ViewBag.GroupNameId = new SelectList(db.Groups.Where(x => x.Status != 0).OrderBy(x => x.GroupName), "GroupNameId", "GroupName", productModel.GroupNameId);
            ViewBag.ManufacturerId = new SelectList(db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 2).OrderBy(x => x.SupplierName), "SupplierId", "SupplierName", productModel.ManufacturerId);
            ViewBag.ZoneId = new SelectList(db.WarehouseZones.Where(x => x.Status != 0).OrderBy(x => x.ZoneName), "ZoneId", "ZoneName", productModel.DefaultZoneId);
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.Status != 0).OrderBy(x => x.CustomerId), "CustomerId", "FullName");

            var attributeSet = db.ProductAttributeSets.Find(productModel.AttributeSetId);
            productModel.AttributeSet = attributeSet;
       
            return View("../Shop/Product/AddProduct", productModel);
        }

        #endregion
        #region edit product
        public ActionResult SelectAttributeSet(int? productId)
        {
            if (productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (product.PurchaseProducts.Count > 0)
            {
                return RedirectToAction("EditProduct", "Product", new {productId =product.ProductId , attributeSetId =product.AttributeSetId });
                //ViewBag.AttributeSetId = new SelectList(db.ProductAttributeSets.Where(x => x.Status !=0 &&x.AttributeSetId==product.AttributeSetId),"AttributeSetId", "AttributeSetName", product.AttributeSetId);
            }
            ViewBag.AttributeSetId = new SelectList(db.ProductAttributeSets.Where(x => x.Status != 0).OrderBy(x=>x.AttributeSetName),"AttributeSetId", "AttributeSetName", product.AttributeSetId);
            return View("../Shop/Product/AddAttributeSet", product);
        }
        [Roles("Global_SupAdmin,Configuration")]
        [HttpGet]
        public ActionResult EditProduct(int?productId,int? attributeSetId)
        {
            if (productId == null || attributeSetId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var attributeSet = db.ProductAttributeSets.Find(attributeSetId);
            Product product = db.Products.FirstOrDefault(x => x.ProductId == productId);
             var productAttributeRelations = db.ProductAttributeRelations.Where(x => x.ProductId == product.ProductId).ToList();
            List<ProductAttributeViewModel> productAtts=new List<ProductAttributeViewModel>();
            if (productAttributeRelations.Any()) { 
            foreach (var pa in productAttributeRelations)
            {
                ProductAttributeViewModel prAttModel=new ProductAttributeViewModel()
                {
                    AttributeId = pa.AttributeId,
                   
                    Value = pa.Value,
                };
                productAtts.Add(prAttModel);
            }
            }
             var productCategories = db.ProductCategories.Where(x => x.ProductId == product.ProductId);
            List<int> catIds=new List<int>();
            foreach (var pc in productCategories)
            {
                catIds.Add(pc.CategoryId);
            }
            if (product == null)
            {
                return HttpNotFound();
            }

          
          
            AddProductViewModel productModel = new AddProductViewModel()
            {
                ProductId = product.ProductId,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                Details = product.Details,
                Tp = product.Tp,
                Dp = product.Dp,
                PerCarton = product.PerCarton,
                Vat = product.Vat,
                MinStockLimit = product.MinStockLimit,
                ExpiryDate = String.Format("{0:dd-MM-yyy}",product.ExpiryDate),
                ManufacturerId = product.ManufacturerId,
                Status = product.Status,
                DiscountAmount = product.DiscountAmount,
                ProductImage = product.ProductImage,
                DiscountType=product.DiscountType,
                AttributeSetId = Convert.ToInt32(attributeSetId),
                DefaultZoneId =Convert.ToInt32(product.DefaultZoneId),
                CategoryIds = catIds,
                GroupNameId =  product.GroupNameId,
                ProductAtts = productAtts,
                PurchaseProducts = product.PurchaseProducts,
                AttributeSet = attributeSet
            };
            if (attributeSetId == product.AttributeSetId)
            {
                ViewBag.Attributes = db.ProductAttributeRelations.Where(x => x.ProductId == product.ProductId).Select(x => new ProductAttributeValueViewModel { Attribute = x.Attribute, Value = String.IsNullOrEmpty(x.Value) ? "" : x.Value }).ToList();
            }
            else
            {
                var atts = db.AttributeSetAttributes.Where(x => x.AttributeSetId == attributeSetId).Select(x => new ProductAttributeValueViewModel { Attribute = x.Attribute, Value = String.IsNullOrEmpty(x.Attribute.DefaultValue) ? "" : x.Attribute.DefaultValue }).ToList();
                ViewBag.Attributes = atts;
                List<ProductAttributeViewModel> productAttsDiff = new List<ProductAttributeViewModel>();
                foreach (var pa in atts)
                {
                    ProductAttributeViewModel prAttModel = new ProductAttributeViewModel()
                    {
                        AttributeId = pa.Attribute.AttributeId,
                        Value = String.IsNullOrEmpty(pa.Attribute.DefaultValue) ? "" : pa.Attribute.DefaultValue,
                    };
                    productAttsDiff.Add(prAttModel);
                }
                productModel.ProductAtts = productAttsDiff;
            }
            productModel.CustomerOptionList = new List<ProductCustomerOption>();
            foreach (var customerRelation in product.CustomerOptions)
            {
                var customerOption = new ProductCustomerOption
                {
                    CustomerId = customerRelation.CustomerId,
                    ProductDescription = customerRelation.ProductDescription,
                    ProductCode = customerRelation.ProductCode,
                    UnitPrice = customerRelation.UnitPrice,
                    Mrp = customerRelation.Mrp,

                };
                productModel.CustomerOptionList.Add(customerOption);
            }
            ViewBag.GroupNameId = new SelectList(db.Groups.Where(x => x.Status != 0).OrderBy(x => x.GroupName), "GroupNameId", "GroupName", product.GroupNameId);
            ViewBag.ManufacturerId = new SelectList(db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 2).OrderBy(x => x.SupplierName), "SupplierId", "SupplierName", product.ManufacturerId);
            ViewBag.Categories = db.Categories.Where(x => !x.CategoryParentId.HasValue).ToList();
            
            ViewBag.CategoryIds = new MultiSelectList(CategoryController.CategoryTree().Where(x => x.Status > 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName", productModel.CategoryIds);
            ViewBag.ZoneId = new SelectList(db.WarehouseZones.Where(x => x.Status != 0).OrderBy(x => x.ZoneName), "ZoneId", "ZoneName",product.DefaultZoneId);
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.Status != 0).OrderBy(x => x.CustomerId), "CustomerId", "FullName");
            ViewBag.Customers = db.Customers.Where(x => x.Status != 0).OrderBy(x => x.CustomerId).ToList();

            return View("../Shop/Product/EditProduct", productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(AddProductViewModel productModel)
        {
            if (ModelState.IsValid)
            {
                //append attribute with product name==================
                string productAttributes = "";
                if (productModel.ProductAtts!=null)
                {
                    foreach (var att in productModel.ProductAtts)
                    {
                        if (!String.IsNullOrEmpty(att.Value) && !ProductAttributeController.ExceptAttribute(att.AttributeId))
                        {
                            productAttributes += "-" + att.Value;
                        }
                    }
                }

                // update product information=================================
                Product product = db.Products.Find(productModel.ProductId);
                product.ProductCode = productModel.ProductCode;
                product.ProductName = productModel.ProductName;
                product.ProductFullName = productModel.ProductName + productAttributes;
                product.Details = productModel.Details;
                product.Tp = productModel.Tp ?? 0;
                product.Dp = productModel.Dp ?? 0;
                product.GroupNameId = productModel.GroupNameId;
                product.PerCarton = productModel.PerCarton;
                product.DiscountType = productModel.DiscountType;
                product.DiscountAmount = productModel.DiscountAmount ?? 0;
                product.Vat = productModel.Vat ?? 0;
                product.ExpiryDate =  String.IsNullOrEmpty(productModel.ExpiryDate)?(DateTime?)null: Convert.ToDateTime(productModel.ExpiryDate);
                product.ManufacturerId = productModel.ManufacturerId;
                product.MinStockLimit = productModel.MinStockLimit ?? 0;
                product.ProductImage = productModel.ProductImage;
                product.AttributeSetId = productModel.AttributeSetId;
                product.DefaultZoneId = productModel.DefaultZoneId;
                product.Status = productModel.Status;
                product.UpdatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey);
                product.UpdatedDate = DateTime.Now;
                
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                // remove product attributes relationship==============================
                var productAttributeRelations = db.ProductAttributeRelations.Where(x => x.ProductId == product.ProductId);
                if (productAttributeRelations != null) { 
                foreach (var item in productAttributeRelations)
                {
                    db.ProductAttributeRelations.Remove(item);
                }
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                // remove product categories relationship==============================
                var productCategories = db.ProductCategories.Where(x => x.ProductId == product.ProductId);
                foreach (var item in productCategories)
                {
                    db.ProductCategories.Remove(item);
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                //add product attributes relationship===============================================
                if (productModel.ProductAtts!=null)
                {
                foreach (var att in productModel.ProductAtts)
                {
                    if (att.ProdAttCheckboxes != null)
                    {
                        foreach (var chk in att.ProdAttCheckboxes)
                        {
                            if (chk.IsChecked)
                            {
                                att.Value += chk.Value + ",";

                            }
                        }
                        if (!String.IsNullOrEmpty(att.Value))
                        {
                            att.Value = att.Value.Trim(',');
                        }
                        //att.Value = att.Value.Remove(att.Value.LastIndexOf(','), 1);    //this is another way 
                    }

                    ProductAttributeRelation productAtts = new ProductAttributeRelation()
                    {
                        ProductId = product.ProductId,
                        AttributeId = att.AttributeId,
                        Value = att.Value
                    };
                    db.ProductAttributeRelations.Add(productAtts);
                }}
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                 //add product categories relationship=============================================
                if (productModel.CategoryIds!=null && productModel.CategoryIds.Count > 0)
                {
                    foreach (ProductCategory productCat in productModel.CategoryIds.Select(cat => new ProductCategory()
                    {
                        ProductId = product.ProductId,
                        CategoryId = cat,
                    }))
                    {
                        db.ProductCategories.Add(productCat);
                    }
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                // remove product customer Options==============================
                var customerReleations = db.ProductCustomerRelations.Where(x => x.ProductId == product.ProductId);
                foreach (var item in customerReleations)
                {
                    db.ProductCustomerRelations.Remove(item);
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                //add product customer options===============================================
                if (productModel.CustomerOptionList != null)
                {
                    foreach (var customerOption in productModel.CustomerOptionList)
                    {
                        if (customerOption.CustomerId != null)
                        {
                            var customerRelation = new ProductCustomerRelation()
                            {
                                ProductId = product.ProductId,
                                CustomerId = Convert.ToInt32(customerOption.CustomerId),
                                ProductDescription = customerOption.ProductDescription,
                                ProductCode = customerOption.ProductCode,
                                UnitPrice = customerOption.UnitPrice,
                                Mrp = customerOption.Mrp,
                            };
                            db.ProductCustomerRelations.Add(customerRelation);
                        }

                    }
                }
                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                return RedirectToAction("Index");
            }


            ViewBag.GroupNameId = new SelectList(db.Groups.Where(x => x.Status != 0).OrderBy(x => x.GroupName), "GroupNameId", "GroupName", productModel.GroupNameId);
            ViewBag.ManufacturerId = new SelectList(db.Suppliers.Where(x => x.Status != 0 && x.SupplierType != 2).OrderBy(x => x.SupplierName), "SupplierId", "SupplierName", productModel.ManufacturerId);
            ViewBag.Attributes = db.ProductAttributeRelations.Where(x => x.ProductId == productModel.AttributeSetId).Select(x => new ProductAttributeValueViewModel { Attribute = x.Attribute, Value = x.Value }).ToList();
            ViewBag.Categories = db.Categories.Where(x => !x.CategoryParentId.HasValue).ToList();
            ViewBag.CategoryIds = new MultiSelectList(CategoryController.CategoryTree().Where(x => x.Status > 0).OrderBy(x => x.CategoryName), "CategoryId", "CategoryName", productModel.CategoryIds);
            ViewBag.ZoneId = new SelectList(db.WarehouseZones.Where(x => x.Status != 0).OrderBy(x => x.ZoneName), "ZoneId", "ZoneName", productModel.DefaultZoneId);
            ViewBag.CustomerId = new SelectList(db.Customers.Where(x => x.Status != 0).OrderBy(x => x.CustomerId), "CustomerId", "FullName");
            ViewBag.Customers = db.Customers.Where(x => x.Status != 0).OrderBy(x => x.CustomerId).ToList();
            return View("../Shop/Product/EditProduct", productModel);
        }

        #endregion

        #region product import export
        [Roles("Global_SupAdmin")]
        public ActionResult ProductImport()
        {
            return View("../Shop/Product/ProductImport");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductImport(FormCollection formCollection)
        {

            int? swapnoId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("swapno")).CustomerId;
            int? agoraId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("agora")).CustomerId;
            int? nandanId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("nandan")).CustomerId;
            int? csdSuperId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("csd super")).CustomerId;
            int? csdExclusiveId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("csd exclusive")).CustomerId;
            int? happyMartId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("happy mart")).CustomerId;
            int? oneStopId = db.Customers.FirstOrDefault(x => x.FullName.ToLower().Contains("one stop")).CustomerId;
            int? defaultWaregouseId =db.Warehouses.FirstOrDefault(x => x.WarehouseName.ToLower() == "default warehouse").WarehouseId;
            string ExcuteMsg = string.Empty;
            int NumberOfColume = 0;
            var attributeSet = db.ProductAttributeSets.FirstOrDefault(x => x.AttributeSetName.ToLower() == "General Items".ToLower());
            var attribute = db.ProductAttributes.FirstOrDefault(x => x.AttributeName.ToLower() == "Unit".ToLower());
            try
            {
                HttpPostedFileBase file = Request.Files["ProductExcel"];
                //Extaintion Check
                if (file.FileName.EndsWith("xls") || file.FileName.EndsWith("xlsx") ||
                    file.FileName.EndsWith("XLS") ||
                    file.FileName.EndsWith("XLSX"))
                {
                    //Null Exp Check
                    if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                        List<ProductImportViewModel> productImportList = new List<ProductImportViewModel>();
                        List<ProductImportViewModel> productImportListForLoop = new List<ProductImportViewModel>();

                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;
                            #region iterat excel to list
                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                            ProductImportViewModel productImport = new ProductImportViewModel
                                {
                                    ProductCode = workSheet.Cells[rowIterator, 1].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 1].Value.ToString(),
                                    ProductName = workSheet.Cells[rowIterator, 2].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 2].Value.ToString(),
                                    UoM = workSheet.Cells[rowIterator, 3].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 3].Value.ToString(),
                                    RetailPrice = workSheet.Cells[rowIterator, 4].Value == null
                                            ? 0
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 4].Value),2),
                                    DefaultStockZone = workSheet.Cells[rowIterator, 5].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 5].Value.ToString(),
                                    Category = workSheet.Cells[rowIterator, 6].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 6].Value.ToString(),
                                    MinimumStockWarning = workSheet.Cells[rowIterator, 7].Value == null
                                            ? 0
                                            : Convert.ToInt16(workSheet.Cells[rowIterator, 7].Value),
                                    //Swapno
                                    SwapnoCode = workSheet.Cells[rowIterator, 8].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 8].Value.ToString(),
                                    SwapnoItemDescription = workSheet.Cells[rowIterator, 9].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 9].Value.ToString(),
                                    SwapnoRetailPrice = workSheet.Cells[rowIterator, 10].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 10].Value),2),
                                    SwapnoMrp = workSheet.Cells[rowIterator, 11].Value == null
                                            ? (double?) null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 11].Value),2),

                                    //agora
                                    AgoraCode =
                                        workSheet.Cells[rowIterator, 12].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 12].Value.ToString(),
                                    AgoraItemDescription =
                                        workSheet.Cells[rowIterator, 13].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 13].Value.ToString(),
                                    AgoraRetailPrice =
                                        workSheet.Cells[rowIterator, 14].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 14].Value),2),
                                    AgoraMrp =
                                        workSheet.Cells[rowIterator, 15].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 15].Value),2),

                                    //Nandan
                                    NandanCode =
                                        workSheet.Cells[rowIterator, 16].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 16].Value.ToString(),
                                    NandanItemDescription =
                                        workSheet.Cells[rowIterator, 17].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 17].Value.ToString(),
                                    NandanRetailPrice =
                                        workSheet.Cells[rowIterator, 18].Value == null
                                            ? (double?)null
                                            :Math.Round( Convert.ToDouble(workSheet.Cells[rowIterator, 18].Value),2),
                                    NandanMrp =
                                        workSheet.Cells[rowIterator, 19].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 19].Value),2),

                                    //csd Super shop
                                    CsdSupperShopCode =
                                        workSheet.Cells[rowIterator, 20].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 20].Value.ToString(),
                                    CsdSupperShopItemDescription =
                                        workSheet.Cells[rowIterator, 21].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 21].Value.ToString(),
                                    CsdSupperShopRetailPrice =
                                        workSheet.Cells[rowIterator, 22].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 22].Value),2),
                                    CsdSupperShopMrp =
                                        workSheet.Cells[rowIterator, 23].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 23].Value),2),

                                    //csd Excluesive
                                    CsdExclusiveShopCode =
                                        workSheet.Cells[rowIterator, 24].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 24].Value.ToString(),
                                    CsdExclusiveShopItemDescription =
                                        workSheet.Cells[rowIterator, 25].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 25].Value.ToString(),
                                    CsdExclusiveShopRetailPrice =
                                        workSheet.Cells[rowIterator, 26].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 26].Value),2),
                                    CsdExclusiveShopMrp =
                                        workSheet.Cells[rowIterator, 27].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 27].Value),2),

                                    //Happy Mart

                                    HappyMartCode =
                                        workSheet.Cells[rowIterator, 28].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 28].Value.ToString(),
                                    HappyMartItemDescription =
                                        workSheet.Cells[rowIterator, 29].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 29].Value.ToString(),
                                    HappyMartMrp =
                                        workSheet.Cells[rowIterator, 30].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 30].Value),2),
                                    HappyMartRetailPrice =
                                        workSheet.Cells[rowIterator, 31].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 31].Value),2),

                                    //one Stop

                                    OneStopCode =
                                        workSheet.Cells[rowIterator, 32].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 32].Value.ToString(),
                                    OneStopItemDescription =
                                        workSheet.Cells[rowIterator, 33].Value == null
                                            ? null
                                            : workSheet.Cells[rowIterator, 33].Value.ToString(),
                                    OneStopMrp =
                                        workSheet.Cells[rowIterator, 34].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 34].Value),2),
                                    OneStopRetailPrice =
                                        workSheet.Cells[rowIterator, 35].Value == null
                                            ? (double?)null
                                            : Math.Round(Convert.ToDouble(workSheet.Cells[rowIterator, 35].Value),2)



                                };
                                if (!db.Products.Any(e => e.ProductName.ToLower() == productImport.ProductName.ToLower()))
                                {
                                    productImportList.Add(productImport);
                                }

                            }
                            #endregion
                            //List data saving
                            if (productImportList.Count != 0)
                            {
                                productImportListForLoop = productImportList;
                                foreach (var item in productImportList)
                                {
                                    int productId = 0;
                                    if (item.ProductName != null)
                                    {
                                        //Default Stock Zone
                                        if (!item.DefaultStockZoneId.HasValue && item.DefaultStockZone != null)
                                        {
                                            WarehouseZone defaultStockZone = db.WarehouseZones.FirstOrDefault(x => x.Status != 0 && x.ZoneName.ToLower() == item.DefaultStockZone.ToLower());

                                            if (defaultStockZone == null)
                                            {
                                                defaultStockZone = new WarehouseZone
                                                {
                                                    ZoneName = item.DefaultStockZone,
                                                    Status = 1,
                                                    WarehouseId = defaultWaregouseId,
                                                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                                                    CreatedDate = DateTime.Now
                                                };
                                                db.WarehouseZones.Add(defaultStockZone);
                                                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                                            }
                                            var sameZoneName = productImportList.Where(x => !x.DefaultStockZoneId.HasValue && x.DefaultStockZone != null && x.DefaultStockZone.Equals(item.DefaultStockZone)).ToList();
                                            if (sameZoneName.Count > 0)
                                                foreach (var sameItem in sameZoneName)
                                                {
                                                    sameItem.DefaultStockZoneId = defaultStockZone.ZoneId;
                                                }
                                        }

                                        //Catagories 
                                        if (!item.CategoryId.HasValue && item.Category != null)
                                        {
                                            Category category = db.Categories.FirstOrDefault(x => x.CategoryName.ToLower() == item.Category.ToLower());
                                            if (category == null)
                                            {
                                                category = new Category()
                                                {
                                                    CategoryName = item.Category,
                                                    Status = 1,
                                                    CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                                                    CreatedDate = DateTime.Now
                                                };
                                                db.Categories.Add(category);
                                                db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                                            }
                                            var sameCat = productImportList.Where(x => !x.CategoryId.HasValue && x.Category != null && x.Category.Equals(item.Category)).ToList();
                                            if (sameCat.Count > 0)
                                            {
                                                foreach (var sameItem in sameCat)
                                                {
                                                    sameItem.CategoryId = category.CategoryId;
                                                }
                                            }

                                        }
                                        if (attributeSet != null && attribute != null)
                                        {

                                            Product product = new Product()
                                            {
                                                ProductCode = item.ProductCode,
                                                ProductName = item.ProductName,
                                                ProductFullName = item.ProductName,
                                                Tp = item.Tp ?? 0,
                                                Dp = item.RetailPrice??0,

                                                DiscountType = "Flat",
                                                DiscountAmount = 0,
                                                Vat = 0,

                                                ManufacturerId = item.ManufacturerId,
                                                MinStockLimit = item.MinimumStockWarning??0,
                                                GroupNameId = item.GroupNameId,
                                                DefaultZoneId = item.DefaultStockZoneId,

                                                AttributeSetId = attributeSet.AttributeSetId,
                                                Status = 1,
                                                CreatedBy = Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),
                                                CreatedDate = DateTime.Now,
                                            };
                                            db.Products.Add(product);
                                            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());
                                            productId = product.ProductId;

                                            //add product attributes relationship===============================================

                                            ProductAttributeRelation productAtts = new ProductAttributeRelation()
                                            {
                                                ProductId = product.ProductId,
                                                AttributeId = attribute.AttributeId,
                                                Value = "Pcs"
                                            };
                                            db.ProductAttributeRelations.Add(productAtts);

                                            //add product categories relationship===============================================
                                            if (item.CategoryId != null)
                                            {

                                                ProductCategory productCat = new ProductCategory()
                                                {
                                                    ProductId = product.ProductId,
                                                    CategoryId = Convert.ToInt32(item.CategoryId),
                                                };
                                                db.ProductCategories.Add(productCat);

                                            }
                                            db.SaveChanges(Membership.GetUser(User.Identity.Name, true).ProviderUserKey.ToString());

                                            StockController updateStock = new StockController();

                                            updateStock.AddToStock(Convert.ToInt32(product.ProductId), 0, Convert.ToInt32(product.DefaultZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                                        
                                            #region customer relation

                                            //Swapno
                                            if (swapnoId!=null &&(item.SwapnoCode != null || item.SwapnoItemDescription != null ||
                                                item.SwapnoMrp > 0 || item.SwapnoRetailPrice > 0))
                                            {
                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation

                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(swapnoId),
                                                    ProductCode = item.SwapnoCode,
                                                    ProductDescription = item.SwapnoItemDescription,
                                                    Mrp = item.SwapnoMrp,
                                                    UnitPrice = item.SwapnoRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            //Agora
                                            if (agoraId!=null && (item.AgoraCode != null || item.AgoraItemDescription != null ||
                                                item.AgoraMrp >0 || item.AgoraRetailPrice >0))
                                            {
                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation

                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(agoraId),
                                                    ProductCode = item.AgoraCode,
                                                    ProductDescription = item.AgoraItemDescription,
                                                    Mrp = item.AgoraMrp,
                                                    UnitPrice = item.AgoraRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            //Nandan
                                            if (nandanId!=null &&(item.NandanCode != null || item.NandanItemDescription != null ||
                                                item.NandanMrp >0 || item.NandanRetailPrice >0))
                                            {
                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation
                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(nandanId),
                                                    ProductCode = item.NandanCode,
                                                    ProductDescription = item.NandanItemDescription,
                                                    Mrp = item.NandanMrp,
                                                    UnitPrice = item.NandanRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            //CSD Supper Shop
                                            if (csdSuperId !=null &&(item.CsdSupperShopCode != null ||
                                                item.CsdSupperShopItemDescription != null ||
                                                item.CsdSupperShopMrp> 0 || item.CsdSupperShopRetailPrice > 0))
                                            {

                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation
                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(csdSuperId),
                                                    ProductCode = item.CsdSupperShopCode,
                                                    ProductDescription = item.CsdSupperShopItemDescription,
                                                    Mrp = item.CsdSupperShopMrp,
                                                    UnitPrice = item.CsdSupperShopRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            //CSD Exclusive Shop

                                            if (csdExclusiveId!=null &&(item.CsdExclusiveShopCode != null ||
                                                item.CsdExclusiveShopItemDescription != null ||
                                                item.CsdExclusiveShopMrp > 0 || item.CsdExclusiveShopRetailPrice > 0))
                                            {

                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation

                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(csdExclusiveId),
                                                    ProductCode = item.CsdExclusiveShopCode,
                                                    ProductDescription = item.CsdExclusiveShopItemDescription,
                                                    Mrp = item.CsdExclusiveShopMrp,
                                                    UnitPrice = item.CsdExclusiveShopRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            //Happy_Mart
                                            if (happyMartId!=null &&(item.HappyMartCode != null || item.HappyMartItemDescription != null ||
                                                item.HappyMartMrp > 0 || item.HappyMartRetailPrice > 0))
                                            {
                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation

                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(happyMartId),
                                                    ProductCode = item.HappyMartCode,
                                                    ProductDescription = item.HappyMartItemDescription,
                                                    Mrp = item.HappyMartMrp,
                                                    UnitPrice = item.HappyMartRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            //One Stop
                                            if (oneStopId!=null &&(item.OneStopCode != null || item.OneStopItemDescription != null ||
                                                item.OneStopMrp > 0 || item.OneStopRetailPrice > 0))
                                            {
                                                ProductCustomerRelation productCustomerRelation = new ProductCustomerRelation

                                                {
                                                    ProductId = productId,
                                                    CustomerId = Convert.ToInt32(oneStopId),
                                                    ProductCode = item.OneStopCode,
                                                    ProductDescription = item.OneStopItemDescription,
                                                    Mrp = item.OneStopMrp,
                                                    UnitPrice = item.OneStopRetailPrice,

                                                };
                                                db.ProductCustomerRelations.Add(productCustomerRelation);
                                            }

                                            #endregion
                                            db.SaveChanges();
                                            NumberOfColume++;
                                        }
                                    }
                                }
                            }

                        }
                    }
                    ExcuteMsg = "Excel Input Successfully" + "  Number of Colume :" + NumberOfColume;
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ViewBag.Error = "File type is incorrect <br>";

                }
               
                
            }
            catch (Exception r)
            {
                ExcuteMsg = r.Message;
            }
            ViewBag.ExcuteMsg = ExcuteMsg;
            return View("../Shop/Product/ProductImport");
        }

         [Roles("Global_SupAdmin")]
        public ActionResult ExportProduct()
        {
            return View("../Shop/Product/ExportProduct");
        }
        #endregion
        #region helper modules
         public JsonResult GetProuctNameOnly(string term)
         {
             var products = db.Products.Where(p => (p.ProductName.StartsWith(term) || p.ProductName.Contains(" " + term)) && p.Status != 0).GroupBy(p => p.ProductName).ToList().Select(x => new { ProductFullName = x.First().ProductName,ProductName=x.First().ProductName, Flag = "ProductConfig" });
             return Json(products, JsonRequestBehavior.AllowGet);
         }

        public JsonResult GetProuctByproductId(int productId)
        {
            var product =
                db.Products.Where(p => p.ProductId == productId)
                    .ToList()
                    .Select(
                        x =>
                            new
                            {
                                ProductFullName = x.ProductFullName,
                                ProductName = x.ProductName,
                                x.ProductId,
                                ProductCode = x.ProductCode,
                                PurchasePrice = x.Tp,
                                SalePrice = x.Dp,
                                Unit = FindProductUnit(x.ProductId),
                                DiscountAmount = x.DiscountAmount ?? 0,
                                DiscountType = x.DiscountType,
                                Vat = x.Vat ?? 0,
                                DefaultWarehouseId = x.DefaultZoneId,
                                DefaultWarehouseName = x.DefaultZoneId != null ? x.WarehouseZone.ZoneName : " ",
                                Manufacturer = x.ManufacturerId != null ? x.Supplier.SupplierName : " ",
                                Discount =
                                    x.DiscountAmount > 0
                                        ? BankAccountController.DiscountCalculator(x.DiscountAmount, x.DiscountType,
                                            x.Dp).DiscountValueShow
                                        : "N/A"
                            })
                    .FirstOrDefault();
            var specifications =
                db.ProductAttributeRelations.Where(x => x.ProductId == productId && x.Value != null)
                    .Include(x => x.Attribute);
            StringBuilder specificationsSb = new StringBuilder("");
            if (specifications.Any())
            {
                foreach (var specification in specifications)
                {
                    specificationsSb.Append(specification.Attribute.AttributeName + " - " + specification.Value +", ");
                }
            }
            var pCats = db.ProductCategories.Where(x => x.ProductId == productId).Include(x => x.Category);
                StringBuilder pCatsSb = new StringBuilder("");
                if (pCats.Any())
                {
                    foreach (var pCat in pCats)
                    {
                        pCatsSb.Append(pCat.Category.CategoryName + ", ");
                    }
                }
                var customerOptions =
                    db.ProductCustomerRelations.Where(x => x.ProductId == productId).Include(x => x.Customer);
                StringBuilder customerOptionsSb =
                    new StringBuilder(
                        "<table class='table'><tr><td>Customer Name</td><td>Description</td><td>Code</td><td>Unit Price</td><td>MRP</td></tr>");
                if (customerOptions.Any())
                {
                    foreach (var customerOption in customerOptions)
                    {
                        customerOptionsSb.Append("<tr><td>" + customerOption.Customer.FullName +
                                                 "</td><td>" + customerOption.ProductDescription +
                                                 "</td><td>" + customerOption.ProductCode +
                                                 "</td><td>" + customerOption.UnitPrice +
                                                 "</td><td>" + customerOption.Mrp +
                                                 "</td></tr>");
                    }
                }
                customerOptionsSb.Append("</table>");
                var data =
                    new
                    {
                        product = product,
                        specifications = specificationsSb.ToString().Trim(',', ' '),
                        categories = pCatsSb.ToString().Trim(',', ' '),
                        customerOptions = customerOptionsSb.ToString()
                    };
                return Json(data, JsonRequestBehavior.AllowGet);
            
        }

        // product query for barcode----------------------
         public JsonResult GetProuctByBarcode(string barCode)
         {
             var products = db.Stocks.Where(p => p.Barcode.Equals(barCode)).OrderBy(p => p.Product.ProductFullName).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName, ProductName = x.Product.ProductName, x.ProductId, Barcode = x.Barcode, PurchasePrice = x.PurchasePrice ?? x.Product.Tp, SalePrice = x.SalePrice ?? x.Product.Dp, Unit = FindProductUnit(x.Product.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, DefaultWarehouse = x.Product.DefaultZoneId, x.Exp }).ToList();
             return Json(products, JsonRequestBehavior.AllowGet);
         }
         public JsonResult GetProuctByNameForBarcodeGenerate(string term)
         {

             var products = db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { x.ProductFullName,  x.ProductName, x.ProductId, UnitPrice = x.Tp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, x.DiscountType, Vat = x.Vat ?? 0, x.ExpiryDate, Flag = "Barcode" }).ToList();
             return Json(products, JsonRequestBehavior.AllowGet);
         }
         public JsonResult GetProuctbyCodeForBarcodeGenerate(string term)
         {
             var products = db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { x.ProductFullName, x.ProductName,x.ProductId, UnitPrice = x.Dp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, x.DiscountType, Vat = x.Vat ?? 0, x.ExpiryDate, Flag = "Barcode" }).ToList();
             return Json(products, JsonRequestBehavior.AllowGet);
         }
        public JsonResult GetProuctByNameForBarcodePrint(string term)
         {

             var products = db.Stocks.Where(p => p.Barcode != null && (p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, x.ProductId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, Flag = "PrintBarcode" }).ToList();
             return Json(products, JsonRequestBehavior.AllowGet);
         }
        public JsonResult GetProuctbyCodeForBarcodePrint(string term)
         {

             var products = db.Stocks.Where(p => p.Barcode != null && (p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, x.ProductId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, Flag = "PrintBarcode" }).ToList();
             return Json(products, JsonRequestBehavior.AllowGet);
         }

        public JsonResult GetProuctByBarcodeForPrint(string barCode)
        {
            var products = db.Stocks.Where(p => p.Barcode.Equals(barCode)).OrderBy(p => p.Product.ProductFullName).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName, ProductName = x.Product.ProductFullName, x.ProductId, Barcode = x.Barcode, PurchasePrice = x.PurchasePrice ?? x.Product.Tp, UnitPrice = x.SalePrice ?? x.Product.Dp, Unit = FindProductUnit(x.Product.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, x.Exp }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        // product query for purchase----------------------
        public JsonResult GetProuctForPurchase(string term)
        {
            var products = db.Stocks.Where(p => p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.Tp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Purchase" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Tp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Purchase" }).ToList();
                
            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProuctbyCodeForPurchase(string term)
        {
            var products = db.Stocks.Where(p => p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.Tp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Purchase" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Tp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Purchase" }).ToList();

            }


           return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProuctForStockIn(string term)
        {
            var products = db.Stocks.Where(p => p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.Tp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "StockIn" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Tp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "StockIn" }).ToList();

            }
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProuctbyCodeForStockIn(string term)
        {
            var products = db.Stocks.Where(p => p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.Tp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "StockIn" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Tp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "StockIn" }).ToList();

            }


            return Json(products, JsonRequestBehavior.AllowGet);
        }

        // product query for sell----------------------
        public JsonResult GetProuctForSell(string term)
        {
            var products = db.Stocks.Where(p => p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term) && p.Status != 0 && p.TotalQuantity > 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.Dp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Sell" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0 && p.Stocks.Sum(y => y.TotalQuantity) > 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Dp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Sell" }).ToList();

            }
          
            return Json(products, JsonRequestBehavior.AllowGet);
        }  public JsonResult GetProuctbyCodeForSell(string term)
        {
            var products = db.Stocks.Where(p => p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term) && p.Status != 0 && p.TotalQuantity > 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.Dp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Sell" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0 && p.Stocks.Sum(y => y.TotalQuantity) > 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Dp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Sell" }).ToList();

            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProuctForSellWithCustomerId(string term, int? id)
        {
            var products = db.Stocks.Where(p => p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term) && p.Status != 0 && p.TotalQuantity > 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.CustomerOptions.Any(y => y.CustomerId == id && y.UnitPrice > 0) ? x.Product.CustomerOptions.FirstOrDefault(y => y.CustomerId == id && y.UnitPrice > 0).UnitPrice : x.Product.Dp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Sell" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0 && p.Stocks.Sum(y => y.TotalQuantity) > 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.CustomerOptions.Any(y => y.CustomerId == id && y.UnitPrice > 0) ? x.CustomerOptions.FirstOrDefault(y => y.CustomerId == id && y.UnitPrice > 0).UnitPrice : x.Dp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Sell" }).ToList();

            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProuctbyCodeForSellWithCustomerId(string term, int? id)
        {
            var products = db.Stocks.Where(p => p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term) && p.Status != 0 && p.TotalQuantity > 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.ProductId, Barcode = x.Barcode, UnitPrice = x.Product.CustomerOptions.Any(y => y.CustomerId == id && y.UnitPrice > 0) ? x.Product.CustomerOptions.FirstOrDefault(y => y.CustomerId == id && y.UnitPrice > 0).UnitPrice : x.Product.Dp, Unit = FindProductUnit(x.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Sell" }).ToList();
            if (!products.Any())
            {
                products = db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0 && p.Stocks.Sum(y => y.TotalQuantity) > 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.CustomerOptions.Any(y => y.CustomerId == id && y.UnitPrice > 0) ? x.CustomerOptions.FirstOrDefault(y => y.CustomerId == id && y.UnitPrice > 0).UnitPrice : x.Dp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Sell" }).ToList();

            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }
       //---------- product query for damage--------------------------------------------------------------
        public JsonResult GetProuctByNameForDamage(string term)
        {

            var products = db.Stocks.Where(p => (p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.StockId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Damage" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProuctbyCodeForDamage(string term)
        {

            var products = db.Stocks.Where(p => (p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.StockId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "Damage" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProuctByBarcodeForDamage(string barCode)
        {
            var products = db.Stocks.Where(p => p.Barcode.Equals(barCode)).OrderBy(p => p.Product.ProductFullName).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName, ProductName = x.Product.ProductFullName, ProductId = x.StockId, Barcode = x.Barcode, PurchasePrice = x.PurchasePrice ?? x.Product.Tp, UnitPrice = x.SalePrice ?? x.Product.Dp, Unit = FindProductUnit(x.Product.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0,DefaultWarehouse = x.Product.DefaultZoneId, x.Exp }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

       //-----------------------------------------------------------------------------------------------------------------------------------
          //---------- product query for article  transfer--------------------------------------------------------------
        public JsonResult GetProuctByNameForArticleTransfer(string term)
        {

            var products = db.Stocks.Where(p => (p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.StockId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "ArticleTransfer" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProuctbyCodeForArticleTransfer(string term)
        {

            var products = db.Stocks.Where(p => (p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.StockId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "ArticleTransfer" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProuctByBarcodeForArticleTransfer(string barCode)
        {
            var products = db.Stocks.Where(p => p.Barcode.Equals(barCode)).OrderBy(p => p.Product.ProductFullName).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName, ProductName = x.Product.ProductFullName, ProductId = x.StockId, Barcode = x.Barcode, PurchasePrice = x.PurchasePrice ?? x.Product.Tp, UnitPrice = x.SalePrice ?? x.Product.Dp, Unit = FindProductUnit(x.Product.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0,DefaultWarehouse = x.Product.DefaultZoneId, x.Exp }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

       //-----------------------------------------------------------------------------------------------------------------------------------


        //---------- product query for product  transfer--------------------------------------------------------------
        public JsonResult GetProuctByNameForProductTransfer(string term)
        {

            var products = db.Stocks.Where(p => (p.Product.ProductFullName.StartsWith(term) || p.Product.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.StockId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "ProductTransfer" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProuctbyCodeForProductTransfer(string term)
        {

            var products = db.Stocks.Where(p => (p.Product.ProductCode.StartsWith(term) || p.Product.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.Product.ProductFullName).ThenByDescending(p => p.CreatedDate).ToList().Select(x => new { ProductFullName = string.IsNullOrEmpty(x.Barcode) ? x.Product.ProductFullName : x.Product.ProductFullName + "(" + x.Barcode + ")", ProductName = x.Product.ProductFullName, ProductId = x.StockId, x.Barcode, UnitPrice = x.Product.Dp, DiscountAmount = x.Product.DiscountAmount ?? 0, x.Product.DiscountType, ExpiryDate = x.Exp, DefaultWarehouse = x.Product.DefaultZoneId, Flag = "ProductTransfer" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProuctByBarcodeForProductTransfer(string barCode)
        {
            var products = db.Stocks.Where(p => p.Barcode.Equals(barCode)).OrderBy(p => p.Product.ProductFullName).ToList().Select(x => new { ProductFullName = x.Product.ProductFullName, ProductName = x.Product.ProductFullName, ProductId = x.StockId, Barcode = x.Barcode, PurchasePrice = x.PurchasePrice ?? x.Product.Tp, UnitPrice = x.SalePrice ?? x.Product.Dp, Unit = FindProductUnit(x.Product.ProductId), stock = x.TotalQuantity, DiscountAmount = x.Product.DiscountAmount ?? 0, DiscountType = x.Product.DiscountType, Vat = x.Product.Vat ?? 0, DefaultWarehouse = x.Product.DefaultZoneId, x.Exp }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------
        public JsonResult GetProuctForOthers(string term)
        {
            var products =  db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName).ToList().Select(x => new { ProductFullName = x.ProductFullName, ProductName = x.ProductName, ProductId = x.ProductId, Barcode = "", UnitPrice = x.Tp, Unit = FindProductUnit(x.ProductId), stock = x.Stocks.Count > 0 ? x.Stocks.FirstOrDefault().TotalQuantity : 0, DiscountAmount = x.DiscountAmount ?? 0, DiscountType = x.DiscountType, Vat = x.Vat ?? 0, ExpiryDate = x.ExpiryDate, DefaultWarehouse = x.DefaultZoneId, Flag = "Others" }).ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductDetails(int? productId)
        {
            var productinfo = db.Products.Where(p => p.ProductId == productId).Select(p => new { p.ProductId, p.ProductName, p.PerCarton, p.Tp });
            var data = new { product = productinfo };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public static void UpdateProductExpiryDate(int? productId, DateTime? expiryDate, int? currentUserId)
        {
            var cx = new WmsDbContext();
            Product product = cx.Products.Find(productId);
                product.ExpiryDate = Convert.ToDateTime(expiryDate);
                cx.Entry(product).State = EntityState.Modified;
                cx.SaveChanges(currentUserId.ToString());
            
        }

        public static string FindProductUnit(int productId)
        {
            var cx = new WmsDbContext();
            Product product = cx.Products.Find(productId);
            string unit = "";
            if (product != null)
            {
                 unit = product.ProductAttributeRelations.FirstOrDefault(x => x.Attribute.AttributeName.ToLower().Equals("unit"))!=null? product.ProductAttributeRelations.FirstOrDefault(x => x.Attribute.AttributeName.ToLower().Equals("unit")).Value : "";
            }
           
            return unit;
        }
        public static double FindStockByProductIdAndBarcode(int productId, string barcode)
        {
            var cx = new WmsDbContext();
            double stock = cx.Stocks.FirstOrDefault(x => x.ProductId == productId && x.Barcode == barcode).TotalQuantity ?? 0;
            return stock;
        }
        public JsonResult IsProductCodeExist(string ProductCode, string InitialProductCode)
        {
            bool isNotExist = true;
            if (ProductCode != string.Empty && InitialProductCode == "undefined")
            {
                var isExist = db.Products.Any(x => x.ProductCode.ToLower().Equals(ProductCode.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (ProductCode != string.Empty && InitialProductCode != "undefined")
            {
                var isExist = db.Products.Any(x => x.ProductCode.ToLower() == ProductCode.ToLower() && x.ProductCode.ToLower() != InitialProductCode.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return Json(isNotExist, JsonRequestBehavior.AllowGet);
        }
#endregion
       
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