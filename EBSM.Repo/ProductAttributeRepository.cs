using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class ProductAttributeRepository
    {
        private WmsDbContext db;
        public ProductAttributeRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(ProductAttribute item)
        {
            db.ProductAttributes.Add(item);
        }
        public void Edit(ProductAttribute item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public ProductAttribute GetById(int id)
        {
            return db.ProductAttributes.Find(id); 
        }
        public IEnumerable<ProductAttribute> GetAll()
        {
            return db.ProductAttributes.OrderBy(x => x.AttributeName);
        }
        public IEnumerable<ProductAttribute> GetAll(string AttName)
        {
            return db.ProductAttributes.Where(x => (AttName == null || x.AttributeName.StartsWith(AttName))).OrderBy(x => x.AttributeName);
        }
        public IEnumerable<ProductAttribute> GetAllByName(string AttName)
        {
            return db.ProductAttributes.Where(x => x.Status != 0 && x.AttributeName.ToLower() == AttName.ToLower()).OrderBy(x => x.AttributeName);
        }
        public IEnumerable<ProductAttribute> GetAllExceptThisName(string AttName)
        {
            return db.ProductAttributes.Where(x => x.Status != 0 && x.AttributeName.ToLower() != AttName.ToLower()).OrderBy(x => x.AttributeName);
        }
        public bool IsAttNameUsed(string AttributeName, string InitialAttributeName)
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
            return isNotExist;
        }
        public  bool IsExceptAttribute(int? id, string exceptAttName)
        {
            bool isExist = false;
            
                var attribute = db.ProductAttributes.FirstOrDefault(x => x.AttributeId == id);
                if (attribute != null) {
                isExist = attribute.AttributeName.ToLower() == exceptAttName.ToLower();
            }


            return isExist;
        }
    }
}
