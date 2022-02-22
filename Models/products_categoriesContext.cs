using Microsoft.EntityFrameworkCore;
using products_categories.Models;

namespace products_categories.Models
{
    public class products_categoriesContext : DbContext
    {
        public products_categoriesContext(DbContextOptions options) : base(options) { }

        // for every model / entity that is going to be part of the db
        // the names of these properties will be the names of the tables in the db
        public DbSet<Product> Products { get; set; }
        public DbSet<ProdCatMany> ProdCatManys { get; set; }
        public DbSet<Category> Categories { get; set; }

        // public DbSet<Widget> Widgets { get; set; }
        // public DbSet<Item> Items { get; set; }
    }
}
