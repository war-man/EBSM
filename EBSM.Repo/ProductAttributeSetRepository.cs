using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductAttributeSetRepository
    {
        private WmsDbContext db;
        public ProductAttributeSetRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(ProductAttributeSet item)
        {
            db.ProductAttributeSets.Add(item);
        }
        public void Edit(ProductAttributeSet item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public ProductAttributeSet GetById(int id)
        {
            return db.ProductAttributeSets.Find(id);
        }
        public ProductAttributeSet GetProductAttributeSetByName(string name)
        {
            return db.ProductAttributeSets.FirstOrDefault(x => x.AttributeSetName.ToLower() == name.ToLower());
        }
        public IEnumerable<ProductAttributeSet> GetAll()
        {
            return db.ProductAttributeSets.OrderBy(x => x.AttributeSetName);
        }
        public IEnumerable<ProductAttributeSet> GetAll(string AttributeSetName)
        {
            return db.ProductAttributeSets.Where(x => (AttributeSetName == null || x.AttributeSetName.StartsWith(AttributeSetName))).OrderBy(x => x.AttributeSetName);
        }
        public bool IsAttributeSetNameExist(string attSetName){
           return db.ProductAttributeSets.Any(x => x.AttributeSetName.ToLower()==attSetName.ToLower());
    }
        public bool IsAttributeSetNameUsed(string AttributeSetName, string InitialAttributeSetName)
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
            return isNotExist;
        }
    }
}
