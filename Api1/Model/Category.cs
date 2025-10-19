using System.ComponentModel.DataAnnotations;

namespace Api1.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Product>? Products { get; set; }

    }
}
