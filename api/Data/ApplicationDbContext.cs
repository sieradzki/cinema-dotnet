using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Screening> Screenings { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add roles
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = ""
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = ""
                },
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasIndex(g => g.Name).IsUnique();
                entity.Property(g => g.Name).HasMaxLength(16);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(m => m.Title).HasMaxLength(256);
                entity.Property(m => m.Description).HasMaxLength(2048);
                entity
                    .HasMany(m => m.Genres)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("MovieGenres"));

            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity
                .Property(s => s.Type)
                .HasConversion<int>();

                entity
                .HasIndex(s => new { s.RoomId, s.Row, s.Number })
                .IsUnique();
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity
                .Property(r => r.Status)
                .HasConversion<int>();

                // TODO Remember to check unique (screeningid, seatid)!

                entity
                    .HasMany(r => r.Seats)
                    .WithMany()
                    .UsingEntity(j => j.ToTable("ReservationSeats"));
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(r => r.Name)
                    .HasMaxLength(16);
                entity.HasIndex(r => r.Name)
                    .IsUnique();
            });

            modelBuilder.Entity<Screening>()
                .Property(s => s.TimeStart)
                .HasColumnType("timestamp without time zone"); // all my homies hate dealing with time zones

            // Populate database for development // TODO do I even bother checking if it's dev or prod for this project? probably not

            // Room and seats
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "A1" }
            );

            var seats = new List<Seat>();
            int id = 1;
            for (int row = 1; row <= 10; row++)
            {
                for (int number = 1; number <= 20; number++)
                {
                    seats.Add(new Seat
                    {
                        Id = id++,
                        RoomId = 1,
                        Row = row,
                        Number = number
                    });
                }
            }
            modelBuilder.Entity<Seat>().HasData(seats.ToArray());

            // seed Genres
            var genres = new List<Genre>
            {
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Comedy" },
                new Genre { Id = 3, Name = "Drama" },
                new Genre { Id = 4, Name = "Horror" },
                new Genre { Id = 5, Name = "Science Fiction" },
                new Genre { Id = 6, Name = "Romance" },
                new Genre { Id = 7, Name = "Thriller" },
                new Genre { Id = 8, Name = "Animation" },
                new Genre { Id = 9, Name = "Fantasy" },
                new Genre { Id = 10, Name = "Documentary" }
            };

            modelBuilder.Entity<Genre>().HasData(genres.ToArray());


        }
    }
}