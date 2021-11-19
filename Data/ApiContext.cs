using Microsoft.EntityFrameworkCore;
using PracticeAPI.Models;

namespace PracticeAPI.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<PrivateClient> PrivateClients { get; set; }
        public DbSet<PublicClient> PublicClients { get; set; }
        
        public DbSet<ParentA> ParentAs { get; set; }
        public DbSet<ParentB> ParentBs { get; set; }
        public DbSet<ChildA> ChildAs { get; set; }
        public DbSet<ChildB> ChildBs { get; set; }
        public DbSet<ThingA> ThingAs { get; set; }
        public DbSet<ThingB> ThingBs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParentA>()
                .HasMany<ChildA>(p => p.Childs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParentB>()
                .HasMany<ChildB>(p => p.Childs)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChildA>()
                .HasMany<ThingA>(c => c.Things)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChildB>()
                .HasMany<ThingB>(c => c.Things)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}