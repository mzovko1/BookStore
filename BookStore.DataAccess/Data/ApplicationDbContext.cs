
using Microsoft.EntityFrameworkCore;
using BookStore.Models.Models;

namespace BookStore.DataAccess.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Company> Companies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Sci-Fi",
                DisplayOrder = 1
            },
            new Category
            {
                Id=2,
                Name="Novel",
                DisplayOrder=2
            },
            new Category
            {
                Id=3,
                Name="Thriller",
                DisplayOrder=3
            }
       );
        
    }
}
