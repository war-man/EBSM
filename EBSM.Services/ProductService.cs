using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ProductService
    {
        private WmsDbContext _context;
        private ProductUnitOfWork _productUnitOfWork;

        public ProductService()
        {
            _context = new WmsDbContext();
            _productUnitOfWork = new ProductUnitOfWork(_context);
        }
        public IEnumerable<Product> GetActiveProducts()
        {
            return _productUnitOfWork.ProductRepository.GetActiveProducts();
        }
           
        public Product GetProductById(int id)
        {
            return _productUnitOfWork.ProductRepository.GetById(id);
        }
       
        public int Save(Product product, int? loggedInUserId)
        {
            _productUnitOfWork.ProductRepository.Add(product);
            _productUnitOfWork.Save(loggedInUserId.ToString());
            return product.ProductId;
        }
        public void Edit(Product product, int? loggedInUserId)
        {
            _productUnitOfWork.ProductRepository.Edit(product);
            _productUnitOfWork.Save(loggedInUserId.ToString());
        }

        public IEnumerable<Product> GetAllProducts(string PName, string PCode, int? GroupNameId, int? ManufacId, int? CatId, byte? Status, double? Price)
        {
            return _productUnitOfWork.ProductRepository.GetAll( PName,  PCode,   GroupNameId,  ManufacId,   CatId,   Status,   Price);
        }
        public bool IsProductCodeExist(string ProductCode, string InitialProductCode)
        {
            return _productUnitOfWork.ProductRepository.IsProductCodeExist(ProductCode, InitialProductCode);
        }
            //Product Attribute Relation
            public void SaveProductAttributeRelationList(IEnumerable<ProductAttributeRelation> productAttributeRelationList)
        {
            foreach(var item in productAttributeRelationList)
            {
                _productUnitOfWork.ProductAttributeRelationRepository.Add(item);
            }
            _productUnitOfWork.Save();
        }
        public IEnumerable<ProductAttributeRelation> GetAllAttributeByProductId(int productId)
        {
            return _productUnitOfWork.ProductAttributeRelationRepository.GetAllByProductId(productId);
        }
        public void DeleteProductAttributeRelationList(IEnumerable<ProductAttributeRelation> productAttributeRelationList)
        {
            foreach (var item in productAttributeRelationList)
            {
                _productUnitOfWork.ProductAttributeRelationRepository.DeleteFromDbByItem(item);
            }
            _productUnitOfWork.Save();
        }
        //Product Category Relation
        public void SaveProductCategoryRelationList(int ProductId, int[] CategoryIds )
        {
            foreach (var cat in CategoryIds)
            {
                var productCat = new ProductCategory()
                {
                    ProductId = ProductId,
                    CategoryId = cat,
                };
                _productUnitOfWork.ProductCategoryRepository.Add(productCat);
            }
            _productUnitOfWork.Save();
        }
        public IEnumerable<ProductCategory> GetAllCategoriesByProductId(int producId)
        {
            return _productUnitOfWork.ProductCategoryRepository.GetAllByProductId(producId);
        }
        //Product Customer Relation
        public void SaveProductCustomerRelationList(IEnumerable<ProductCustomerRelation> productCustomerRelationList)
        {
            foreach (var item in productCustomerRelationList)
            {
                _productUnitOfWork.ProductCustomerRalationRepository.Add(item);
            }
            _productUnitOfWork.Save();
        }
        public void Dispose()
        {
            _productUnitOfWork.Dispose();
        }
    }
}
