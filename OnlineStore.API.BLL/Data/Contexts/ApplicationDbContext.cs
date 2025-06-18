using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineStore.API.Core.Models;
using OnlineStore.API.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.API.BLL.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd(); // Ensures auto-generation
        }
    


        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> Types { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
