using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class ProductAttributeService
    {
        private WmsDbContext _context;
        private ProductAttributeUnitOfWork _productAttributeUnitOfWork;

        public ProductAttributeService()
        {
            _context = new WmsDbContext();
            _productAttributeUnitOfWork = new ProductAttributeUnitOfWork(_context);
        }
       
        public ProductAttribute GetProductAttributeById(int id)
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.GetById(id);
        }
        public ProductAttribute GetProductAttributeByName(string AttName)
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.GetProductAttributeByName(AttName);
        }
       
        public int SaveAttribute(ProductAttribute productAttribute, int? loggedInUserId)
        {
            _productAttributeUnitOfWork.ProductAttributeRepository.Add(productAttribute);
            _productAttributeUnitOfWork.Save(loggedInUserId.ToString());
            return productAttribute.AttributeId;
        }
        public void EditAttribute(ProductAttribute productAttribute, int? loggedInUserId)
        {
            _productAttributeUnitOfWork.ProductAttributeRepository.Edit(productAttribute);
            _productAttributeUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<ProductAttribute> GetAllAttributes()
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.GetAll();
        }   public IEnumerable<ProductAttribute> GetAllAttributes(string AttName)
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.GetAll(AttName);
        }
        public IEnumerable<ProductAttribute> GetAllByName(string AttName)
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.GetAllByName(AttName);
        }
        public IEnumerable<ProductAttribute> GetAllExceptThisName(string AttName)
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.GetAllExceptThisName(AttName);
        }
        public bool IsAttNameUsed(string AttributeName, string InitialAttributeName)
        { return _productAttributeUnitOfWork.ProductAttributeRepository.IsAttNameUsed(AttributeName, InitialAttributeName);
        }
        public bool IsExceptAttribute(int? id, string exceptAttName)
        {
            return _productAttributeUnitOfWork.ProductAttributeRepository.IsExceptAttribute(id, exceptAttName);
        }

            //Attribute Set=============
            public ProductAttributeSet GetProductAttributeSetById(int id)
        {
            return _productAttributeUnitOfWork.ProductAttributeSetRepository.GetById(id);
        }
        public ProductAttributeSet GetProductAttributeSetByName(string AttSetName)
        {
            return _productAttributeUnitOfWork.ProductAttributeSetRepository.GetProductAttributeSetByName(AttSetName);
        }

        public int SaveAttributeSet(ProductAttributeSet productAttributeSet, int? loggedInUserId)
        {
            _productAttributeUnitOfWork.ProductAttributeSetRepository.Add(productAttributeSet);
            _productAttributeUnitOfWork.Save(loggedInUserId.ToString());
            return productAttributeSet.AttributeSetId;
        }
        public void EditAttributeSet(ProductAttributeSet productAttributeSet, int? loggedInUserId)
        {
            _productAttributeUnitOfWork.ProductAttributeSetRepository.Edit(productAttributeSet);
            _productAttributeUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<ProductAttributeSet> GetAllAttributeSets()
        {
            return _productAttributeUnitOfWork.ProductAttributeSetRepository.GetAll();
        }
        public IEnumerable<ProductAttributeSet> GetAllAttributeSets(string AttSetName)
        {
            return _productAttributeUnitOfWork.ProductAttributeSetRepository.GetAll(AttSetName);
        }
        public bool IsAttributeSetNameUsed(string AttributeSetName, string InitialAttributeSetName)
        {
            return _productAttributeUnitOfWork.ProductAttributeSetRepository.IsAttributeSetNameUsed(AttributeSetName, InitialAttributeSetName);
        }
        public bool IsAttributeSetNameExist(string attSetName)
        {
            return _productAttributeUnitOfWork.ProductAttributeSetRepository.IsAttributeSetNameExist( attSetName);
        }
        //Attribute Set Attrinbute Relation
        public IEnumerable<AttributeSetAttribute> GetAttributesByAttributeSetId(int attSetId)
        {
            return _productAttributeUnitOfWork.AttributeSetAttributeRepository.GetAllByAttributeSetId(attSetId).ToList();
           
        }
        public void DeleteAttributeSetAttributeListFromDb(IEnumerable<AttributeSetAttribute> items)
        {
            foreach (var removeItem in items)
            {
                _productAttributeUnitOfWork.AttributeSetAttributeRepository.DeleteFromDbByItem(removeItem);
            }
            _productAttributeUnitOfWork.Save();
        }
        public void SaveAttributeSetAttributeByAttributeId(int attributeSetId, int[]attributeIds, int? loggedInUserId)
        {
            foreach (var item in attributeIds)
            {
                AttributeSetAttribute attributeSetAttribute = new AttributeSetAttribute()
                {
                    AttributeSetId = attributeSetId,
                    AttributeId = item,
                };
                _productAttributeUnitOfWork.AttributeSetAttributeRepository.Add(attributeSetAttribute);
            }
            _productAttributeUnitOfWork.Save(loggedInUserId.ToString());
        }
        public void Dispose()
        {
            _productAttributeUnitOfWork.Dispose();
        }
    }
}
