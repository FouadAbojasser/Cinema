

using System.Reflection.Metadata;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CinemaDB;Integrated Security=True;TrustServerCertificate=True");

            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
             .HasMany(t => t.Genres)
             .WithMany(t => t.Movies);

            modelBuilder.Entity<Series>()
            .HasMany(t => t.Genres)
            .WithMany(t => t.Series);

            modelBuilder.Entity<Movie>()
            .HasMany(e => e.Images)
            .WithOne(e => e.Movie)
            .IsRequired(false);

            modelBuilder.Entity<Series>()
            .HasMany(e => e.Images)
            .WithOne(e => e.Series)
            .IsRequired(false);

            modelBuilder.Entity<Movie>()
             .HasMany(t => t.Actors)
             .WithMany(t => t.Movies);

            modelBuilder.Entity<Series>()
            .HasMany(t => t.Actors)
            .WithMany(t => t.Series);

            modelBuilder.Entity<Director>()
            .HasMany(e => e.Movies)
            .WithOne(e => e.Director);

            modelBuilder.Entity<Director>()
            .HasMany(e => e.Series)
            .WithOne(e => e.Director);

        }




    }
}
