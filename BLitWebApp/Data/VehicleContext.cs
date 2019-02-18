using System;
using System.Collections.Generic;
using BLitWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BLitWebApp.Data
{
    public class VehicleContext : DbContext
    {
        public VehicleContext(DbContextOptions<VehicleContext> options) : base(options)
        {
        }

        public DbSet<CarClass> CarClasses { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarClass>().ToTable("CarClass")
                .HasIndex(c => c.Name)
                .IsUnique();
            modelBuilder.Entity<CarModel>().ToTable("CarModel")
                .HasIndex(c => c.Name)
                .IsUnique(); ;
            modelBuilder.Entity<Manufacturer>().ToTable("Manufacturer")
                .HasIndex(m => m.Name)
                .IsUnique(); ;
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle");
        }
    }
}
