using Api1.Context;
using Api1.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api1.Repository
{
    public class CatergoryRepo : ICategoryRepo
    {
        Api1Context db;

        public CatergoryRepo( Api1Context db)
        {
            this.db = db;
        }


        public bool Create(Category category)
        {
            db.Categories.Add(category);

            if(category!= null)
            {
                 db.SaveChanges();
                return true;
            }
            return false;

        }

        public bool Delete(int id)
        {
            var category = db.Categories.Find(id);
            
            if(category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();

                return true;
            }
            return false;

        }

        public List<Category> GetAll()
        {
            var categories = db.Categories.ToList();
            return categories;
        }

        public Category GetById(int id)
        {
           return db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            var category = db.Categories.Where(c => c.Id == categoryId).Select(c => c.Products).FirstOrDefault();
            return category ?? new List<Product>();
        }


        public bool Update(int id, Category category)
        {
            var dbCategory = db.Categories.Find(id);
            if (dbCategory != null)
            {
                
                dbCategory.Name = category.Name;
                dbCategory.Products = category.Products;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
