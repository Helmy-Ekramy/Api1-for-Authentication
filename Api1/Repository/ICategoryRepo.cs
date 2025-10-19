using Api1.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api1.Repository
{
    public interface ICategoryRepo
    {
        public List<Category> GetAll();
        public Category GetById(int id);

        public bool Create(Category category);

        public bool Update(int id, Category category);

        public bool Delete(int id);

        List<Product> GetProductsByCategoryId(int categoryId);
    }
}
