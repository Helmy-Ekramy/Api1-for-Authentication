using System.Text.Json.Serialization;

namespace Api1.DTO
{
    public class FavProductsResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category_Name { get; set; }

        public DateTime AddedOn { get; set; }

    }

}
