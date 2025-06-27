using Microsoft.EntityFrameworkCore;
using ShoesStoreApi.Model;

namespace ShoesStoreApi.DBModel
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Shoes> Shoes { get; set; }
        public DbSet<User> User { get; set; }
    }
}
