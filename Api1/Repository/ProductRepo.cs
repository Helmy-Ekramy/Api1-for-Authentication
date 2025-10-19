using Api1.Context;
using Api1.DTO;
using Api1.Model;

namespace Api1.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly Api1Context db;

        public ProductRepo( Api1Context db)
        {
            this.db = db;
        }
        public int Create(NewProductModel product)
        {
            var neww = new Product
            {
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            };

            db.Products.Add(neww);
            db.SaveChanges();

            return neww.Id;
        }

        public bool Delete(int id)
        {
            var product = db.Products.Find(id);
            bool deleted = false;
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                deleted = true;
            }
            return deleted;
        }

        public List<Product> GetAll()
        {
            return db.Products.ToList();
        }

        public Product GetById(int id)
        {
            var product = db.Products.Find(id);
            return product;
        }

        public bool Update(int id, Product product)
        {
            var existingProduct = db.Products.Find(id);
            bool updated = false;
            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;
                db.SaveChanges();
                updated = true;
            }

            return updated;
        }
    }
}
