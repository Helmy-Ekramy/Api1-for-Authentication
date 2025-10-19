using Api1.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api1.Context
{
    public class Api1Context: IdentityDbContext<ApplicationUser>
    {
        public Api1Context(DbContextOptions<Api1Context> options) : base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RefreshTokens> RefreshTokens { get; set; }

        public DbSet<FavouriteProduct> FavouriteProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FavouriteProduct>()
                .HasKey(fp => new { fp.UserId, fp.ProductId });

            builder.Entity<FavouriteProduct>()
                .HasOne(fp => fp.User)
                .WithMany(u => u.FavouriteProducts)
                .HasForeignKey(fp => fp.UserId);
            builder.Entity<FavouriteProduct>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.UserFavouriteProducts)
                .HasForeignKey(fp => fp.ProductId);
        }



    }
}
