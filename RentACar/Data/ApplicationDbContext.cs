using Microsoft.EntityFrameworkCore;
using RentACar.Models;

namespace RentACar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany()
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}


//using Microsoft.EntityFrameworkCore;
//using RentACar.Models;

//namespace RentACar.Data
//{
//    public class ApplicationDbContext : DbContext
//    {
//        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<Car> Cars { get; set; }
//        public DbSet<User> Users { get; set; }
//        public DbSet<Reservation> Reservations { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.Entity<Car>()
//                .Property(c => c.PricePerDay)
//                .HasPrecision(18, 2);

//            modelBuilder.Entity<Reservation>()
//                .Property(r => r.TotalPrice)
//                .HasPrecision(18, 2);

//            modelBuilder.Entity<User>()
//                .HasIndex(u => u.Username)
//                .IsUnique();

//            modelBuilder.Entity<User>()
//                .HasIndex(u => u.Email)
//                .IsUnique();

//            modelBuilder.Entity<User>()
//                .HasIndex(u => u.EGN)
//                .IsUnique();

//            modelBuilder.Entity<Reservation>()
//                .HasOne(r => r.User)
//                .WithMany(u => u.Reservations)
//                .HasForeignKey(r => r.UserId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<Reservation>()
//                .HasOne(r => r.Car)
//                .WithMany()
//                .HasForeignKey(r => r.CarId)
//                .OnDelete(DeleteBehavior.Restrict);

//            modelBuilder.Entity<User>().HasData(
//                new User
//                {
//                    Id = 1,
//                    Username = "admin",
//                    Password = "admin123",
//                    FirstName = "Admin",
//                    LastName = "User",
//                    EGN = "0000000001",
//                    Email = "admin@rentacar.com",
//                    PhoneNumber = "0888000001",
//                    Role = "Admin"
//                },
//                new User
//                {
//                    Id = 2,
//                    Username = "user1",
//                    Password = "user123",
//                    FirstName = "Normal",
//                    LastName = "User",
//                    EGN = "0000000002",
//                    Email = "user1@rentacar.com",
//                    PhoneNumber = "0888000002",
//                    Role = "User"
//                }
//            );

//            modelBuilder.Entity<Car>().HasData(
//                new Car
//                {
//                    Id = 1,
//                    Brand = "Toyota",
//                    Model = "Corolla",
//                    Year = 2020,
//                    Seats = 5,
//                    Description = "Economy car",
//                    PricePerDay = 80
//                },
//                new Car
//                {
//                    Id = 2,
//                    Brand = "BMW",
//                    Model = "320",
//                    Year = 2022,
//                    Seats = 5,
//                    Description = "Comfort sedan",
//                    PricePerDay = 140
//                },
//                new Car
//                {
//                    Id = 3,
//                    Brand = "Volkswagen",
//                    Model = "Golf",
//                    Year = 2021,
//                    Seats = 5,
//                    Description = "Compact hatchback",
//                    PricePerDay = 95
//                }
//            );
//        }
//    }
//}