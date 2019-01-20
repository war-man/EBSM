using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class AttributeSetAttributeRepository
    {
        private WmsDbContext db;
        public AttributeSetAttributeRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(AttributeSetAttribute item)
        {
            db.AttributeSetAttributes.Add(item);
        }
        public void Edit(AttributeSetAttribute item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public AttributeSetAttribute GetById(int id)
        {
            return db.AttributeSetAttributes.Find(id); 
        }
        public IEnumerable<AttributeSetAttribute> GetAllByAttributeSetId(int attSetId)
        {
            return db.AttributeSetAttributes.Where(x => x.AttributeSetId == attSetId);
        }
        public IEnumerable<AttributeSetAttribute> GetAll()
        {
            return db.AttributeSetAttributes;
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(AttributeSetAttribute item)
        {
            db.AttributeSetAttributes.Remove(item);
        }
        //public IEnumerable<AttributeSetAttribute> GetAll(string AttributeSetName)
        //{
        //    return db.ProductAttributeSets.Where(x => (AttributeSetName == null || x.AttributeSetName.StartsWith(AttributeSetName))).OrderBy(x => x.AttributeSetName);
        //}


    }
}
