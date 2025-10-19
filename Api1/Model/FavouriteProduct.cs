namespace Api1.Model
{
    public class FavouriteProduct
    {
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public DateTime AddedOn { get; set; } = DateTime.Now;



    }
}
