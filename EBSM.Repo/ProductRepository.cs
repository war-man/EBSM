using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductRepository
    {
        private WmsDbContext db;
        public ProductRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Product product)
        {
            db.Products.Add(product);
        }
        public void Edit(Product product)
        {
            db.Entry(product).State = EntityState.Modified;
        }
        public Product GetById(int id)
        {
            return db.Products.Find(id); 
          
        }
        public IEnumerable<Product> GetActiveProducts()
        {
            return db.Products.Where(x => x.Status != 0).OrderBy(x => x.ProductFullName);
        }
        public IEnumerable<Product> GetAllByProductName(string term)
        {
            return db.Products.Where(x => (x.ProductName.StartsWith(term) || x.ProductName.Contains(" " + term)) && x.Status != 0).OrderBy(x => x.ProductFullName);
        }
        public IEnumerable<Product> GetAllByProductFullName(string term)
        {
            return db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName);
        } public IEnumerable<Product> GetAllByProductFullNameIsInStock(string term)
        {
            return db.Products.Where(p => (p.ProductFullName.StartsWith(term) || p.ProductFullName.Contains(" " + term)) && p.Status != 0 && p.Stocks.Sum(y => y.TotalQuantity) > 0).OrderBy(p => p.ProductFullName);
        }
        public IEnumerable<Product> GetAllByProductCode(string term)
        {
            return db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0).OrderBy(p => p.ProductFullName);
        }public IEnumerable<Product> GetAllByProductCodeIsInStock(string term)
        {
            return db.Products.Where(p => (p.ProductCode.StartsWith(term) || p.ProductCode.Contains(" " + term)) && p.Status != 0 && p.Stocks.Sum(y => y.TotalQuantity) > 0).OrderBy(p => p.ProductFullName);
        }
        public IEnumerable<Product> GetAll(string PName, string PCode, int? GroupNameId, int? ManufacId, int? CatId, byte? Status, double? Price)
        {
            //var fromDate = string.IsNullOrEmpty(TransferDateFrom) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateFrom);
            //var toDate = string.IsNullOrEmpty(TransferDateTo) ? DateTime.Now.Date : Convert.ToDateTime(TransferDateTo).AddDays(1);
            return db.Products.Where(x => (PName == null || (x.ProductFullName.StartsWith(PName) || x.ProductFullName.Contains(" " + PName)))
                    && (PCode == null || x.ProductCode.ToLower().StartsWith(PCode.ToLower()))
                    && (GroupNameId == null || x.GroupNameId == GroupNameId)
                    && (ManufacId == null || x.ManufacturerId == ManufacId)
                     && (CatId == null || x.ProductCategories.Any(y => y.CategoryId == CatId))
                    && (Status == null || x.Status == Status)
                    && (Price == null || x.Tp <= Price)).OrderBy(o => o.ProductName);

        }
        public bool CheckProductNameExist(string name)
        {
            return db.Products.Any(e => e.ProductName.ToLower() == name.ToLower());
        }
        public bool IsProductCodeExist(string ProductCode, string InitialProductCode)
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
            return isNotExist;
        }
    }
}
