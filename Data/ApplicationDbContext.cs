using System.Reflection.Emit;
using System.Reflection.Metadata;
using Cinema.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    //optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=CinemaDB;Integrated Security=True;TrustServerCertificate=True");

        //}


        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<MovieTheater> MovieTheaters { get; set; }


       
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Must be Added to avoid EF skipping configuring Identity tables
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>()
             .HasMany(t => t.Genres)
             .WithMany(t => t.Movies);


            modelBuilder.Entity<Series>()
            .HasMany(t => t.Genres)
            .WithMany(t => t.Series);


            modelBuilder.Entity<Movie>()
            .HasMany(e => e.Images)
            .WithOne(e => e.Movie)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
            

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


            modelBuilder.Entity<MovieTheater>()
             .HasKey(mt => new { mt.MovieId, mt.TheaterId }); // Composite Primary Key


            modelBuilder.Entity<MovieTheater>()
            .HasOne(mt => mt.Movie)
            .WithMany(m => m.MovieTheaters)
            .HasForeignKey(mt => mt.MovieId);


            modelBuilder.Entity<MovieTheater>()
            .HasOne(mt => mt.Theater)
            .WithMany(t => t.MovieTheaters)
            .HasForeignKey(mt => mt.TheaterId);
        }

    }
}
