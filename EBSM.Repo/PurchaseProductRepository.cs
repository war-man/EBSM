﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class PurchaseProductRepository
    {
        private WmsDbContext db;
        public PurchaseProductRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(PurchaseProduct item)
        {
            db.PurchaseProducts.Add(item);
        }
        public void Edit(PurchaseProduct item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public PurchaseProduct GetById(int id)
        {
            return db.PurchaseProducts.Find(id); 
        }
        public IEnumerable<PurchaseProduct> GetAll()
        {
            return db.PurchaseProducts;
        }
        public IEnumerable<PurchaseProduct> GetAllByPurchaseId(int purchaseId)
        {
           return db.PurchaseProducts.Where(x=>x.PurchaseId== purchaseId);
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(PurchaseProduct item)
        {
            db.PurchaseProducts.Remove(item);
        }
        public int GetCount()
        {
            return db.Purchases.Count();
        }

    }
}