using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBSM.Entities;
using EBSM.Repo;

namespace EBSM.Services
{
    public class CategoryService
    {
        private WmsDbContext _context;
        private CategoryUnitOfWork _categoryUnitOfWork;

        public CategoryService()
        {
            _context = new WmsDbContext();
            _categoryUnitOfWork = new CategoryUnitOfWork(_context);
        }
       
        public Category GetCategoryById(int id)
        {
            return _categoryUnitOfWork.CategoryRepository.GetById(id);
        }
       
        public int Save(Category category, int? loggedInUserId)
        {
            _categoryUnitOfWork.CategoryRepository.Add(category);
            _categoryUnitOfWork.Save(loggedInUserId.ToString());
            return category.CategoryId;
        }
        public void Edit(Category category, int? loggedInUserId)
        {
            _categoryUnitOfWork.CategoryRepository.Edit(category);
            _categoryUnitOfWork.Save(loggedInUserId.ToString());
        }
        public IEnumerable<Category> GetAll()
        {
            return _categoryUnitOfWork.CategoryRepository.GetAll();
        }
        public IEnumerable<Category> GetAll(string CategoryName)
        {
            return _categoryUnitOfWork.CategoryRepository.GetAll(CategoryName);
        }
        public IEnumerable<Category> GetAllRootCategories()
        {
            return _categoryUnitOfWork.CategoryRepository.GetAllRootCategories();
        }
        public void DeleteFromDbById(int id,int? loggedInUserId)
        {
            _categoryUnitOfWork.CategoryRepository.DeleteFromDbById(id);
            _categoryUnitOfWork.Save(loggedInUserId.ToString());

        }
        public void DeleteFromDbByItem(Category item, int? loggedInUserId)
        {
            _categoryUnitOfWork.CategoryRepository.DeleteFromDbByItem(item);
            _categoryUnitOfWork.Save(loggedInUserId.ToString());
        }
        public bool IsCategoryNameExist(string CategoryName, string InitialCategoryName)
        {
            return _categoryUnitOfWork.CategoryRepository.IsCategoryNameExist(CategoryName, InitialCategoryName);
        }
            public void Dispose()
        {
            _categoryUnitOfWork.Dispose();
        }
    }
}
