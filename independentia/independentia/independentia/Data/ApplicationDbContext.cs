using independentia.Controllers;
using independentia.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace independentia.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<independentia.Models.Film> Films { get; set; } = default!;
    
    public DbSet<FilmPurchase> FilmPurchases { get; set; } = default!;
    
    public DbSet<FilmRating> FilmRatings { get; set; }
    
    public DbSet<CartItem> CartItems { get; set; }
    
    public DbSet<ForumPost> ForumPosts { get; set; }
    
    public DbSet<ForumTopic> ForumTopics { get; set; }
}
