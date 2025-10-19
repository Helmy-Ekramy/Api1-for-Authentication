using Api1.DTO;
using Api1.Model;

namespace Api1.Repository
{
    public interface IProductRepo
    {
        public List<Product> GetAll();
        public Product GetById(int id);
        public int Create(NewProductModel product);
        public bool Update(int id, Product product);
        public bool Delete(int id);



    }
}
