using System;
using System.Collections.Generic;

using System.Data.Entity;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using PagedList;
using System.Text;
using System.Web.Security;
using WarehouseApp.Models;
using WarehouseApp.Models.ViewModels;
using EBSM.Entities;
using EBSM.Services;
namespace WarehouseApp.Controllers
{
    [Authorize]
    public class PurchaseController : Controller
    {
        private PurchaseService _purchaseService = new PurchaseService();
        private SupplierService _supplierService = new SupplierService();
        private WarehouseZoneService _warehouseZoneService = new WarehouseZoneService();

        // GET: /Purchase/
        [Roles("Global_SupAdmin,Purchase_Create,Purchase_Price,StockIn")]
        [OutputCache(Duration = 20)]
        public ActionResult Index(PurchaseSearchViewModel model)
        {
            ViewBag.DueTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
           
  
            var purchases =_purchaseService.GetAllPurchases(model.PurchaseNo, model.PurchaseDateFrom, model.PurchaseDateTo, model.SupplierId, model.TransactionMode);
            model.Purchases = purchases.ToPagedList(model.Page, model.PageSize);

            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName");
            return View("../Shop/Purchase/Index", model);
        }

        #region new purchase
         [Roles("Global_SupAdmin,Purchase_Create,Purchase_Price")]
        [OutputCache(Duration = 120)]
        public ActionResult NewPurchase()
        {
            //ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            //ViewBag.TransactionModeId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            ViewBag.PcTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            ViewBag.PcTransactionModeId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName");
            //ViewBag.ZoneId = new SelectList(db.WarehouseZones.OrderBy(x => x.ZoneName), "ZoneId", "ZoneName",1);
            return View("../Shop/Purchase/NewPurchase");
    
        }
        [Roles("Global_SupAdmin,StockIn")]
        [OutputCache(Duration = 120)]
        public ActionResult StockIn()
        {
            
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            ViewBag.PcTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            ViewBag.PcTransactionModeId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName");
            //ViewBag.ZoneId = new SelectList(db.WarehouseZones.OrderBy(x => x.ZoneName), "ZoneId", "ZoneName",1);
            return View("../Shop/Purchase/NewStockIn");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePurchase(PuchaseViewModel data)
        {
            //string result = "Error";
            if (ModelState.IsValid)
            {
                string fmt = "0000000.##";
                int lastPurchaseId = 0;
   
                Purchase purchase = new Purchase()
                {
                    PurchaseDate = Convert.ToDateTime(data.PurchaseDate),
                    PurchaseNumber = "P" + TransactionController.BillingMonthString(data.PurchaseDate) + (_purchaseService.GetCount() + 1).ToString(fmt),
                    TotalQuantity = data.PurchaseProducts.Sum(x => x.Quantity),
                    PurchaseDiscount = data.PurchaseDiscount ?? 0, 
                    TotalPrice = Math.Round(Convert.ToDouble(data.PurchaseProducts.Sum(x => x.TotalPrice)), 2)-data.PurchaseDiscount, 
                    SupplierId = data.SupplierId,
                    PurchaseCost = data.PurchaseCost,
                    PaidAmount = data.PaidAmount,
                    TransactionMode = data.TransactionMode,
                    TransactionModeId = data.TransactionModeId,
                    Status = 1,
                    CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId,
                    CreatedDate = DateTime.Now
                };
                _purchaseService.Save(purchase, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                PurchaseCost purchaseCost = new PurchaseCost();
                if (data.PurchaseCost > 0)
                {
                    purchaseCost.PurchaseId = purchase.PurchaseId;
                    purchaseCost.Amount = Convert.ToDouble(data.PurchaseCost);
                    purchaseCost.PaidAmount = data.PurchaseCost;
                    purchaseCost.TransactionMode = "Cash";
                    purchaseCost.TransactionModeId = BankAccountController.AllAccountByMode("Cash").FirstOrDefault().Id;
       
                    purchaseCost.Status = 1;
                    purchaseCost.CreatedBy =AuthenticatedUser.GetUserFromIdentity().UserId;
                    purchaseCost.CreatedDate = DateTime.Now;
                    _purchaseService.SavePurchaseCost(purchaseCost, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                }
                foreach (var item in data.PurchaseProducts)
                {
                    PurchaseProduct purchaseProduct = new PurchaseProduct()
                    {
                        PurchaseId = purchase.PurchaseId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        Barcode=item.Barcode,
                        UnitPrice = item.UnitPrice??0,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice ?? (item.UnitPrice * item.Quantity),
                        ZoneId = item.ZoneId,
                        Status = 1
                    };
                    _purchaseService.SavePurchaseProduct(purchaseProduct);
                   
                 //====update stock==============================================
                    StockController updateStock = new StockController();
                    if (string.IsNullOrEmpty(item.Barcode))
                    {
                        updateStock.AddToStock(Convert.ToInt32(purchaseProduct.ProductId), item.Quantity,Convert.ToInt32(item.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.AddToStock(Convert.ToInt32(purchaseProduct.ProductId), item.Quantity, Convert.ToInt32(item.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), item.Barcode);
                    }
                }

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
               TransactionController transaction = new TransactionController();
               transaction.WithdrawFromAccount(data.TransactionMode, Convert.ToInt32(data.TransactionModeId), Convert.ToDouble(data.PaidAmount), Convert.ToDateTime(data.PurchaseDate), "Purchases", "PurchaseId", purchase.PurchaseId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Purchase Payment");
                if (data.PurchaseCost > 0)
                {
                    transaction.WithdrawFromAccount(data.PcTransactionMode, Convert.ToInt32(data.PcTransactionModeId), Convert.ToDouble(data.PaidPurchaseCost), Convert.ToDateTime(data.PurchaseDate), "PurchaseCosts", "PurchaseCostId", purchaseCost.PurchaseCostId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Purchase Cost Payment");
                }
               
               
                //===update Supplier balance============================================================================
                var remaining = purchase.TotalPrice - data.PaidAmount;
                SupplierController supplier =new SupplierController();
                supplier.UpdateSupplierBalance(Convert.ToInt32(data.SupplierId), Convert.ToDouble(remaining), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
               
                //result = "OK";
                return RedirectToAction("Index", "Purchase");
            }


            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");

            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName",data.SupplierId);

            return View("../Shop/Purchase/NewPurchase");
        }
        #endregion

        #region purchase details
        [Roles("Global_SupAdmin,Purchase_Create,Purchase_Price,StockIn")]
        public ActionResult PurchaseDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase =_purchaseService.GetPurchaseById(id.Value);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View("../Shop/Purchase/PurchaseDetails", purchase);
        }
        #endregion
        #region edit purchase
        [Roles("Global_SupAdmin,Purchase_Create,Purchase_Price")]
        public ActionResult EditPurchase(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = _purchaseService.GetPurchaseById(id.Value);
            if (purchase == null || purchase.PurchasePayments.Count > 0)
            {
                return HttpNotFound();
            }
           
            PuchaseViewModel purchaseEditModel=new PuchaseViewModel();
            purchaseEditModel.PurchaseId = purchase.PurchaseId;
            purchaseEditModel.PurchaseNumber = purchase.PurchaseNumber;
            purchaseEditModel.PurchaseDate = purchase.PurchaseDate.ToString("dd-MM-yyyy");
            purchaseEditModel.TotalQuantity = purchase.TotalQuantity;
            purchaseEditModel.PurchaseDiscount = purchase.PurchaseDiscount;
            purchaseEditModel.TotalPurchasePrice = purchase.TotalPrice;
            purchaseEditModel.TransactionMode = purchase.TransactionMode;
            purchaseEditModel.TransactionModeId = purchase.TransactionModeId;
            purchaseEditModel.PaidAmount = purchase.PaidAmount;
            purchaseEditModel.SupplierId=purchase.SupplierId;
            if (purchase.PurchaseCosts.Count>0)
            {
                purchaseEditModel.PurchaseCostId = purchase.PurchaseCosts.First().PurchaseCostId;
                purchaseEditModel.PurchaseCost = purchase.PurchaseCosts.First().Amount;
                purchaseEditModel.PaidPurchaseCost = purchase.PurchaseCosts.First().PaidAmount;
                purchaseEditModel.PcTransactionMode = purchase.PurchaseCosts.First().TransactionMode;
                purchaseEditModel.PcTransactionModeId = purchase.PurchaseCosts.First().TransactionModeId;
            }
            if (purchase.PurchaseProducts.Count > 0)
            { 
                List<PurchaseProductViewModel> purchaseProductList=new List<PurchaseProductViewModel>();
             foreach(var item in purchase.PurchaseProducts){
                PurchaseProductViewModel purchaseProductVm=new PurchaseProductViewModel()
                {
                    PurchaseId = item.PurchaseId,
                    Purchase = item.Purchase,
                    ProductId = item.ProductId,
                    Product = item.Product,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice??0,
                    TotalPrice = item.TotalPrice ?? (item.Quantity * item.UnitPrice),
                    Barcode = item.Barcode,
                    ZoneId = item.ZoneId,
                };
                purchaseProductList.Add(purchaseProductVm);
            }
                purchaseEditModel.PurchaseProducts = purchaseProductList;
            }
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", purchaseEditModel.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(purchaseEditModel.TransactionMode), "Id", "Name", purchaseEditModel.TransactionModeId);
            ViewBag.PcTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", purchaseEditModel.PcTransactionMode);
            ViewBag.PcTransactionModeId = new SelectList(BankAccountController.AllAccountByMode(purchaseEditModel.TransactionMode), "Id", "Name",purchaseEditModel.PcTransactionModeId);
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName", purchaseEditModel.SupplierId);
            //ViewBag.ZoneId = new SelectList(db.WarehouseZones.OrderBy(x => x.ZoneName), "ZoneId", "ZoneName",1);
            ViewBag.ZoneId = _warehouseZoneService.GetAllWarehouseZone();
           
           return View("../Shop/Purchase/EditPurchase", purchaseEditModel);
            //return View("../Shop/Purchase/EditStockIn", purchaseEditModel);
        }
        [Roles("Global_SupAdmin,StockIn")]
        public ActionResult EditStockIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = _purchaseService.GetPurchaseById(id.Value);
            if (purchase == null || purchase.PurchasePayments.Count > 0)
            {
                return HttpNotFound();
            }

            PuchaseViewModel purchaseEditModel = new PuchaseViewModel();
            purchaseEditModel.PurchaseId = purchase.PurchaseId;
            purchaseEditModel.PurchaseNumber = purchase.PurchaseNumber;
            purchaseEditModel.PurchaseDate = purchase.PurchaseDate.ToString("dd-MM-yyyy");
            purchaseEditModel.TotalQuantity = purchase.TotalQuantity;
            purchaseEditModel.PurchaseDiscount = purchase.PurchaseDiscount;
            purchaseEditModel.TotalPurchasePrice = purchase.TotalPrice;
            purchaseEditModel.TransactionMode = purchase.TransactionMode;
            purchaseEditModel.TransactionModeId = purchase.TransactionModeId;
            purchaseEditModel.PaidAmount = purchase.PaidAmount;
            purchaseEditModel.SupplierId = purchase.SupplierId;
            if (purchase.PurchaseCosts.Count > 0)
            {
                purchaseEditModel.PurchaseCostId = purchase.PurchaseCosts.First().PurchaseCostId;
                purchaseEditModel.PurchaseCost = purchase.PurchaseCosts.First().Amount;
                purchaseEditModel.PaidPurchaseCost = purchase.PurchaseCosts.First().PaidAmount;
                purchaseEditModel.PcTransactionMode = purchase.PurchaseCosts.First().TransactionMode;
                purchaseEditModel.PcTransactionModeId = purchase.PurchaseCosts.First().TransactionModeId;
            }
            if (purchase.PurchaseProducts.Count > 0)
            {
                List<PurchaseProductViewModel> purchaseProductList = new List<PurchaseProductViewModel>();
                foreach (var item in purchase.PurchaseProducts)
                {
                    PurchaseProductViewModel purchaseProductVm = new PurchaseProductViewModel()
                    {
                        PurchaseId = item.PurchaseId,
                        Purchase = item.Purchase,
                        ProductId = item.ProductId,
                        Product = item.Product,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice??0,
                        TotalPrice = item.TotalPrice ?? (item.Quantity * item.UnitPrice),
                        Barcode = item.Barcode,
                        ZoneId = item.ZoneId,
                    };
                    purchaseProductList.Add(purchaseProductVm);
                }
                purchaseEditModel.PurchaseProducts = purchaseProductList;
            }
            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", purchaseEditModel.TransactionMode);
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode(purchaseEditModel.TransactionMode), "Id", "Name", purchaseEditModel.TransactionModeId);
            ViewBag.PcTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", purchaseEditModel.PcTransactionMode);
            ViewBag.PcTransactionModeId = new SelectList(BankAccountController.AllAccountByMode(purchaseEditModel.TransactionMode), "Id", "Name", purchaseEditModel.PcTransactionModeId);
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName", purchaseEditModel.SupplierId);
            //ViewBag.ZoneId = new SelectList(db.WarehouseZones.OrderBy(x => x.ZoneName), "ZoneId", "ZoneName",1);
            ViewBag.ZoneId = _warehouseZoneService.GetAllWarehouseZone();

            //return View("../Shop/Purchase/EditPurchase", purchaseEditModel);
            return View("../Shop/Purchase/EditStockIn", purchaseEditModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPurchase(PuchaseViewModel data)
        {
            //string result = "Error";
            if (ModelState.IsValid)
            {
                Purchase purchase = _purchaseService.GetPurchaseById(data.PurchaseId.Value);

                if (purchase == null)
                {
                    return HttpNotFound();
                }
                var oldTotalPurchasePrice = purchase.TotalPrice;
                var oldPaidPurchasePrice = purchase.PaidAmount;
                purchase.PurchaseDate = Convert.ToDateTime(data.PurchaseDate);
                purchase.TotalQuantity = data.PurchaseProducts.Sum(x => x.Quantity);
                purchase.PurchaseDiscount = data.PurchaseDiscount ?? 0;
                purchase.TotalPrice = Math.Round(Convert.ToDouble(data.PurchaseProducts.Sum(x => x.TotalPrice)), 2) -data.PurchaseDiscount;
                purchase.SupplierId = data.SupplierId;
                purchase.PurchaseCost = data.PurchaseCost;
                purchase.PaidAmount = data.PaidAmount;
                purchase.TransactionMode = data.TransactionMode;
                purchase.TransactionModeId = data.TransactionModeId;
                purchase.Status = 1;
                purchase.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                purchase.UpdatedDate = DateTime.Now;
                _purchaseService.Edit(purchase, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                PurchaseCost purchaseCost = new PurchaseCost();
                 
                double? oldPaidPurchaseCost = 0;
                if (data.PurchaseCost != null && data.PurchaseCost>0)
                {
                    purchaseCost =_purchaseService.GetCostByPurchaseId(data.PurchaseId.Value);
                    if (purchaseCost != null)
                    {
                        oldPaidPurchaseCost = purchaseCost.PaidAmount;
                        purchaseCost.PurchaseId = purchase.PurchaseId;
                        purchaseCost.Amount = Convert.ToDouble(data.PurchaseCost);
                        purchaseCost.PaidAmount = data.PurchaseCost;
                        //purchaseCost.TransactionMode = data.PcTransactionMode;
                        //purchaseCost.TransactionModeId = data.TransactionModeId;
                        purchaseCost.TransactionMode = "Cash";
                        purchaseCost.TransactionModeId = BankAccountController.AllAccountByMode("Cash").FirstOrDefault().Id;
                        purchaseCost.Status = 1;
                        purchaseCost.UpdatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                        purchaseCost.UpdatedDate = DateTime.Now;
                        _purchaseService.SavePurchaseCost(purchaseCost, AuthenticatedUser.GetUserFromIdentity().UserId);
                       
                    }
                    else
                    {
                            purchaseCost = new PurchaseCost();
                            purchaseCost.PurchaseId = purchase.PurchaseId;
                            purchaseCost.Amount = Convert.ToDouble(data.PurchaseCost);
                            purchaseCost.PaidAmount = data.PurchaseCost;
                            //purchaseCost.TransactionMode = data.PcTransactionMode;
                            //purchaseCost.TransactionModeId = data.TransactionModeId;
                            purchaseCost.TransactionMode = "Cash";
                            purchaseCost.TransactionModeId = BankAccountController.AllAccountByMode("Cash").FirstOrDefault().Id;
                            purchaseCost.Status = 1;
                            purchaseCost.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                            purchaseCost.CreatedDate = DateTime.Now;
               
                          
                        _purchaseService.EditPurchaseCost(purchaseCost, AuthenticatedUser.GetUserFromIdentity().UserId);

                  
                
                    }
                }
                StockController updateStock = new StockController();
                //====remove previous purchase products=======================
                var purchaseProducts =_purchaseService.GetAllProductByPurchaseId(purchase.PurchaseId).ToList();
                if (purchaseProducts.Count > 0)
                {
                    foreach (var removeItem in purchaseProducts)
                    {
                        //====update stock==============================================

                        if (string.IsNullOrEmpty(removeItem.Barcode))
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(removeItem.ProductId), removeItem.Quantity, Convert.ToInt32(removeItem.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                        }
                        else
                        {
                            updateStock.RemoveFromStock(Convert.ToInt32(removeItem.ProductId), removeItem.Quantity, Convert.ToInt32(removeItem.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), removeItem.Barcode);
                        
                        }
                        _purchaseService.DeletePurchaseProduct(removeItem);
                     
                    }
                  
                }
                //====Add new purchase products=======================
                foreach (var item in data.PurchaseProducts)
                {
                    PurchaseProduct purchaseProduct = new PurchaseProduct()
                    {
                        PurchaseId = purchase.PurchaseId,
                        ProductId = Convert.ToInt32(item.ProductId),
                        UnitPrice = item.UnitPrice??0,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice ?? (item.UnitPrice*item.Quantity),
                        Barcode = item.Barcode,
                        ZoneId = item.ZoneId,
                        Status = 1
                    };
                    _purchaseService.SavePurchaseProduct(purchaseProduct);
                   

                    //if (!string.IsNullOrEmpty(item.ExpiryDate.ToString())) { 
                    ////====update product Expiry Date==============================================
                    //ProductController.UpdateProductExpiryDate(Convert.ToInt32(purchaseProduct.ProductId), item.ExpiryDate, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    //}
                    //====update stock==============================================
                    if (string.IsNullOrEmpty(item.Barcode))
                    {
                        updateStock.AddToStock(Convert.ToInt32(purchaseProduct.ProductId), item.Quantity, Convert.ToInt32(item.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));
                    }
                    else
                    {
                        updateStock.AddToStock(Convert.ToInt32(purchaseProduct.ProductId), item.Quantity, Convert.ToInt32(item.ZoneId), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), item.Barcode);
                    }
                }

                //========add to account transaction as format DepositToAccount(transactionMode, accountId, amount, transactionDate, tableName, primaryKeyName, primaryKeyValue,currentUserId);============================================================================================================================
               
                TransactionController transaction = new TransactionController();
                if ((data.PaidAmount - oldPaidPurchasePrice) != 0)
                {
                    transaction.WithdrawFromAccount(data.TransactionMode, Convert.ToInt32(data.TransactionModeId), Convert.ToDouble(data.PaidAmount - oldPaidPurchasePrice), Convert.ToDateTime(data.PurchaseDate), "Purchases", "PurchaseId", purchase.PurchaseId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Purchase Payment");
                }
                if (data.PurchaseCost > 0 && (data.PaidPurchaseCost - oldPaidPurchaseCost)!=0)
                {
                    transaction.WithdrawFromAccount(data.PcTransactionMode, Convert.ToInt32(data.PcTransactionModeId), Convert.ToDouble(data.PaidPurchaseCost - oldPaidPurchaseCost), Convert.ToDateTime(data.PurchaseDate), "PurchaseCosts", "PurchaseCostId", purchaseCost.PurchaseCostId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey), "Purchase Cost Payment");
                }


                //===update Supplier balance============================================================================
                var oldRemaining = oldTotalPurchasePrice - oldPaidPurchasePrice;
                var remaining = purchase.TotalPrice - data.PaidAmount;

                SupplierController supplier = new SupplierController();
                supplier.UpdateSupplierBalance(Convert.ToInt32(data.SupplierId), Convert.ToDouble(remaining - oldRemaining), Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey));

                //result = "OK";
                return RedirectToAction("Index", "Purchase");
            }
            

            ViewBag.TransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            //ViewBag.PcTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text");
            //ViewBag.PcTransactionModeId = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.SupplierId = new SelectList(_supplierService.GetAllDistributor(), "SupplierId", "SupplierName", data.SupplierId);
            return View("../Shop/Purchase/EditPurchase");
            
        }
        #endregion
        #region purchase due payment 
        public ActionResult PurchaseDuePayment(int? id)
        {
             if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
             ViewBag.DueTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
             ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
        ViewBag.Purchase =_purchaseService.GetPurchaseById(id.Value);
        return View("../Shop/Purchase/PurchaseDuePayment");
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PurchaseDuePayment(PrchaseDuePaymentVm duePayment)
        {
            
            if (ModelState.IsValid)
            {
                
                PurchasePayment purchasePayment = new PurchasePayment();
                if (duePayment.PaymentFor == "1")
                {
                    Purchase purchase = _purchaseService.GetPurchaseById(duePayment.PurchaseId);
                    purchase.PaidAmount += duePayment.PaidAmount;
                    _purchaseService.Edit(purchase, AuthenticatedUser.GetUserFromIdentity().UserId);
                    
                }
                if (duePayment.PaymentFor == "2")
                {
                    PurchaseCost pc =_purchaseService.GetCostByPurchaseId(duePayment.PurchaseId);
                    pc.PaidAmount += duePayment.PaidAmount;
                    _purchaseService.EditPurchaseCost(pc, AuthenticatedUser.GetUserFromIdentity().UserId);
                   
                    purchasePayment.PurchaseCostId = pc.PurchaseCostId;
                }
                
                purchasePayment.PurchaseId = duePayment.PurchaseId;
                purchasePayment.PaidAmount = duePayment.PaidAmount;
                purchasePayment.PaymentDate = Convert.ToDateTime(duePayment.DuePaymentDate);
                purchasePayment.TransactionMode = duePayment.DueTransactionMode;
                purchasePayment.TransactionModeId = duePayment.TransactionModeId;
                purchasePayment.CreatedBy = AuthenticatedUser.GetUserFromIdentity().UserId;
                purchasePayment.CreatedDate = DateTime.Now;

                _purchaseService.EditPurchasePayment(purchasePayment, AuthenticatedUser.GetUserFromIdentity().UserId);
               
                TransactionController transaction = new TransactionController();
                transaction.WithdrawFromAccount(duePayment.DueTransactionMode, Convert.ToInt32(duePayment.TransactionModeId), Convert.ToDouble(duePayment.PaidAmount), Convert.ToDateTime(duePayment.DuePaymentDate), "PurchasePayments", "PurchasePaymentId", purchasePayment.PurchasePaymentId, Convert.ToInt32(Membership.GetUser(User.Identity.Name, true).ProviderUserKey),"Purchase Due Payment");
                return RedirectToAction("Index","Purchase");
            }

            ViewBag.DueTransactionMode = new SelectList(BankAccountController.TransactionModes(), "Value", "Text", "Cash");
            ViewBag.TransactionModeId = new SelectList(BankAccountController.AllAccountByMode("Cash"), "Id", "Name");
            ViewBag.Purchase =_purchaseService.GetPurchaseById(duePayment.PurchaseId);
            return View("../Shop/Purchase/PurchaseDuePayment", duePayment);
        }

        #endregion

        #region helping modules
        public ActionResult DeleteRecord(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            Purchase purchase = _purchaseService.GetPurchaseById(id.Value);
            if (purchase == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }
            var purchaseProducts =_purchaseService.GetAllProductByPurchaseId(id.Value).ToList();
            if (purchaseProducts.Count > 0)
            {
                _purchaseService.DeletePurchaseProductList(purchaseProducts);
            }
            _purchaseService.DeletePurchase(purchase);
           
            return Json(new { Result = "OK" });
        }

        public ActionResult GetPurchaseById(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            var x = _purchaseService.GetPurchaseById(id.Value);
            var purchase =  new { x.PurchaseId,x.PurchaseDate,x.Supplier.SupplierName,x.PaidAmount,x.TotalPrice,x.TotalQuantity,x.TransactionMode,x.TransactionModeId,x.PurchaseNumber };
            if (purchase==null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }

            var jsonData = new { result = purchase };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPurchaseCostById(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }

            var purchaseCost =_purchaseService.GetAllCostByPurchaseId(id.Value).Select(x => new { x.PurchaseId, PurchaseDate = x.Purchase.PurchaseDate, SupplierName = x.Purchase.Supplier.SupplierName, x.PaidAmount, TotalPrice = x.Amount, TotalQuantity = x.Purchase.TotalQuantity, x.TransactionMode, x.TransactionModeId, PurchaseNumber=x.Purchase.PurchaseNumber }).FirstOrDefault();
            if (purchaseCost == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { Result = "Error" });
            }

            var jsonData = new { result = purchaseCost };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _purchaseService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//