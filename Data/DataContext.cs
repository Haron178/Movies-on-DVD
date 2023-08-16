using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //fluent api
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MovieOnDvd>()
                .HasMany<Genre>(m => m.Genres)
                .WithMany(g => g.Movies)
                .UsingEntity(j => j.ToTable("MovieOnDvdToGenre"));
            modelBuilder.Entity<MovieOnDvd>()
                .HasMany<Owner>(m => m.Owners)
                .WithMany(o => o.Movies)
                .UsingEntity(j => j.ToTable("MovieOnDvdToOwner"));

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieOnDvd> MoviesOnDvd { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }
    }
}
