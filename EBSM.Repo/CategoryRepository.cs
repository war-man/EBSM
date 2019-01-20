using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;

namespace EBSM.Repo
{
    public class CategoryRepository
    {
        private WmsDbContext db;
        public CategoryRepository(WmsDbContext context)
        {
            db = context;
        }
        public void Add(Category item)
        {
            db.Categories.Add(item);
        }
        public void Edit(Category item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
        public Category GetById(int id)
        {
            return db.Categories.Find(id); 
        }
        public IEnumerable<Category> GetAll()
        {
            return db.Categories;
        }
        public IEnumerable<Category> GetAll(string CategoryName)
        {
              return db.Categories.Where(x => (CategoryName == null || x.CategoryName.StartsWith(CategoryName))).OrderBy(x => x.CategoryName);
        }
        public IEnumerable<Category> GetAllRootCategories()
        {
            return db.Categories.Where(x => !x.CategoryParentId.HasValue);
        }
        public void DeleteFromDbById(int id)
        {
            var item = GetById(id);
            DeleteFromDbByItem(item);
        }
        public void DeleteFromDbByItem(Category item)
        {
            db.Categories.Remove(item);
        }
        public bool IsCategoryNameExist(string CategoryName, string InitialCategoryName)
        {
            bool isNotExist = true;
            if (CategoryName != string.Empty && InitialCategoryName == "undefined")
            {
                var isExist = db.Categories.Any(x => x.CategoryName.ToLower().Equals(CategoryName.ToLower()));
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            if (CategoryName != string.Empty && InitialCategoryName != "undefined")
            {
                var isExist = db.Categories.Any(x => x.CategoryName.ToLower() == CategoryName.ToLower() && x.CategoryName.ToLower() != InitialCategoryName.ToLower());
                if (isExist)
                {
                    isNotExist = false;
                }
            }
            return isNotExist;
        }
    }
}
