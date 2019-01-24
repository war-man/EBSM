using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EBSM.Entities;
using PagedList;


namespace WarehouseApp.Models.ViewModels
{
    //Shop Module======================================================================================================
    public class ProductSearchViewModel
    {

        [Display(Name = "Product Code")]
        public String PCode { get; set; }
            [Display(Name = "Product Name")]
        public String PName { get; set; }
            [Display(Name = "Group")]
            public Int32? GroupNameId { get; set; }
            [Display(Name = "Manufacturer")]
            public Int32? ManufacId { get; set; }

            [Display(Name = "Category")]
            public Int32? CatId { get; set; }

            [Display(Name = "Price (Max.)")]
            public Double? Price { get; set; }
            public byte? Status { get; set; }
            public Int32 Page { get; set; }
            public Int32 PageSize { get; set; }
            public String Sort { get; set; }
            public String SortDir { get; set; }
            //public Int32 TotalRecords { get; set; }


            public IPagedList<Product> Products;

          public ProductSearchViewModel()
          {
              Page = 1;
              PageSize = 50;
             
          }
        

    }


    public class PurchaseSearchViewModel
    {
        [Display(Name = "Purchase Date From")]
        public String PurchaseDateFrom { get; set; }
        [Display(Name = "Purchase Date To")]
        public String PurchaseDateTo { get; set; }
        [Display(Name = "Purchase No")]
        public String PurchaseNo { get; set; } 

        [Display(Name = "Transaction Mode")]
        public String TransactionMode { get; set; }

        [Display(Name = "Supplier")]
        public Int32? SupplierId { get; set; }

        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Purchase> Purchases;

        public PurchaseSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }


    }

    public class SalesOrderSearchViewModel
    {
        [Display(Name = "Order Date From")]
        public String OrderDateFrom { get; set; }
        [Display(Name = "Order Date To")]
        public String OrderDateTo { get; set; }
        [Display(Name = "Invoice No")]
        public String OrderNo { get; set; }
 
        [Display(Name = "Porject")]
        public Int32? CustomerId { get; set; }
        public byte? Status { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<SalesOrder> SalesOrders;

        public SalesOrderSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
            Status = 1;
        }


    }
    public class InvoiceSearchViewModel
    {
        [Display(Name = "Purchase Date From")]
        public String InvoiceDateFrom { get; set; }
        [Display(Name = "Purchase Date To")]
        public String InvoiceDateTo { get; set; }
        [Display(Name = "Invoice No")]
        public String InvoiceNo { get; set; }
        [Display(Name = "Transaction Mode")]
        public String TransactionMode { get; set; }

        [Display(Name = "Salesman")]
        public Int32? SalesmanId { get; set; }
        [Display(Name = "Customer")]
        public Int32? CustomerId { get; set; }

        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Invoice> Invoices;

        public InvoiceSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }


    }

    public class SalesReturnSearchViewModel
    {
        [Display(Name = "Return Date From")]
        public String ReturnDateFrom { get; set; }
        [Display(Name = "Return Date To")]
        public String ReturnDateTo { get; set; }
        [Display(Name = "Invoice No")]
        public String InvoiceNo { get; set; }

        [Display(Name = "Project")]
        public Int32? CustomerId { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Return> SalesReturns;

        public SalesReturnSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
            
        }


    }
    public class BillSearchViewModel
    {

        public String BillDateFrom { get; set; }

        public String BillDateTo { get; set; }
        public String BillNo { get; set; }
         [Display(Name = "Project")]
        public Int32? Customer { get; set; }

        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Bill> Bills;

        public BillSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }


    }
    public class ExpenseSearchViewModel
    {
        [Display(Name = "Expense Date From")]
        public String ExpenseDateFrom { get; set; }
        [Display(Name = "Expense Date To")]
        public String ExpenseDateTo { get; set; }
        [Display(Name = "Transaction Mode")]
        public String TransactionMode { get; set; }

        [Display(Name = "ExpenseType")]
        public Int32? ExpenseTypeId { get; set; }

        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Expense> Expenses;

        public ExpenseSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }


    }

    public class StockSearchViewModel
    {

        [Display(Name = "Product")]
        public Int32? SelectedProductId { get; set; }
        public String PName { get; set; }
        public String PCode{ get; set; }
        public String StockLimitOut { get; set; }
        public Int32? StockZoneId { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Stock> Stocks;

        public StockSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class DamageSearchViewModel
    {

        [Display(Name = "Product")]
        public Int32? SelectedProductId { get; set; }
        public String ProductNameFull { get; set; }

        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Damage> Damages;

        public DamageSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class ArticleTransferSearchViewModel
    {

        [Display(Name = "Product")]
        public Int32? SelectedProductId { get; set; }
         [Display(Name = "Product Name")]
        public String PName{ get; set; }
        [Display(Name = "Transfer Date From")]
        public String TransferDateFrom { get; set; }
        [Display(Name = "Transfer Date To")]
        public String TransferDateTo { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<ArticleTransfer> ArticleTransfers;

        public ArticleTransferSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    } public class ProductTransferSearchViewModel
    {

        [Display(Name = "Product")]
        public Int32? SelectedProductId { get; set; }
        [Display(Name = "Product Name")]
        public String PName { get; set; }
         [Display(Name = "Transfer Date From")]
        public String TransferDateFrom { get; set; }
        [Display(Name = "Transfer Date To")]
        public String TransferDateTo { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<TransferProduct> ProductTransfers;

        public ProductTransferSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }

    public class GroupSearchViewModel
    {

        [Display(Name = "Group Name")]
        public string GrpName { get; set; }



        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Group> Groups;


        public GroupSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class CategorySearchViewModel
    {

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Category> Categories;


        public CategorySearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class ZoneSearchViewModel
    {

        [Display(Name = "Zone Name")]
        public string ZoneName { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<WarehouseZone> WarehouseZones;


        public ZoneSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class AttributeSearchViewModel
    {

        [Display(Name = "Attribute Name")]
        public string AttName { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<ProductAttribute> Attributes;


        public AttributeSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class AttributeSetSearchViewModel
    {

        [Display(Name = "Attribute Set Name")]
        public string AttributeSetName { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<ProductAttributeSet> AttributeSets;


        public AttributeSetSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class SupplierSearchViewModel
    {

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Supplier> Suppliers;


        public SupplierSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class SalsemanSearchViewModel
    {

        [Display(Name = "Salesman Name")]
        public string SalesmanName { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Salesman> SalesmanList;


        public SalsemanSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class UserSearchViewModel
    {

        [Display(Name = "Name")]
        public string UserFullName { get; set; }
 [Display(Name = "Department")]
        public int? DeptId { get; set; }
 [Display(Name = "Role")]
        public int? RoleId { get; set; }


        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<User> Users;


        public UserSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class CustomerSearchViewModel
    {

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

         [Display(Name = "Contact No")]
        public string ContactNo { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Customer> Customers;


        public CustomerSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }

    public class NoticeboardPaginationViewModel
    {

      
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Notice> Notices;



        public NoticeboardPaginationViewModel()
        {
            Page = 1;
            PageSize = 25;
        }
    }
    public class TransactionSearchViewModel
    {

        public string TransactionType { get; set; }
        public string TransactionTable { get; set; }
        public string TransactionMode { get; set; }
        [Display(Name = "Transaction Date From")]
        public String TransactionDateFrom { get; set; }
        [Display(Name = "Transaction Date To")]
        public String TransactionDateTo { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<Transaction> Transactions;



        public TransactionSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    public class AuditLogSearchViewModel
    {
        public string EventType { get; set; }
        public string AuditTable { get; set; }

        [Display(Name = "Audit Date From")]
        public String AuditDateFrom { get; set; }
        [Display(Name = "Audit Date To")]
        public String AuditDateTo { get; set; }
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }

        public IPagedList<AuditLog> AuditLogs;



        public AuditLogSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }

    public class BarcodeSearchViewModel
    {

        [Display(Name = "Product")]
        public string PName { get; set; }
        [Display(Name = "Product Code")]
        public string PCode { get; set; }
        [Display(Name = "Barcode")]
        public string BCode { get; set; }

        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }
        public String Sort { get; set; }
        public String SortDir { get; set; }
        //public Int32 TotalRecords { get; set; }


        public IPagedList<BarcodeViewModel> Barcodes;

        public BarcodeViewModel BarcodeModel { get; set; }
       
        public BarcodeSearchViewModel()
        {
            Page = 1;
            PageSize = 50;
        }
    }
    // End of Shop Module====================================================================================================


   
    public class AccountsView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Balance { get; set; }
        public byte? Status { get; set; }

    }
    public class Discount
    {
        public string DiscountValueShow { get; set; }
        public double? DiscountValue{ get; set; }
       

    }
    public class TopSellsMonthly
    {
        public Product Product { get; set; }
        public double? TotalSoldQuantity { get; set; }
        public double? TotalSoldAmount { get; set; }
    }
    public enum Status
    {
        Disabled=0,
        Enabled = 1,
        Higher = 2,

    }
//controller for webgrid====================================================================================================
//    public ActionResult Index(ProductSearchModels model)
//{
//    // To Bind the category drop down in search section
//    ViewBag.Categories = db.Categories.Where(x => x.IsActive == true);

//    // Get Products
//    model.Products = db.Products
//        .Where(
//            x=>
//            (model.ProductName == null || x.ProductName.Contains(model.ProductName))
//            && (model.Price == null || x.Price < model.Price)
//            && (model.Category == null || x.CategoryId == model.Category)
//           )
//        .OrderBy(model.Sort + " " + model.SortDir)
//        .Skip((model.Page - 1) * model.PageSize)
//        .Take(model.PageSize)                    
//        .ToList();

//    // total records for paging
//    model.TotalRecords = db.Products
//        .Count(x =>
//            (model.ProductName == null || x.ProductName.Contains(model.ProductName))
//            && (model.Price == null || x.Price < model.Price)
//            && (model.Category == null || x.CategoryId == model.Category)
//            );

//    return View(model);
//}
}


//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : December 2016
//=======================================================================================//