using Infrastructuur.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructuur.Database.Database
{
    public class WeedDbContext : DbContext
    {
        public WeedDbContext(DbContextOptions<WeedDbContext> options) : base(options)
        {

        }
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<CardEntity> Cards { get; set; }
        public DbSet<ReviewEntity> Reviews { get; set; }
        public DbSet<ShoppingCartEntity> ShoppingCartEntities { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<WeedEntity> Weeds { get; set; }
        public DbSet<UserAddressEntity> UserAddnresses { get; set; }
        public DbSet<UserWeedEntity> UserWeeds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<UserAddressEntity>().HasKey(sc => new { sc.UserId, sc.AddressId});
            //modelBuilder.Entity<UserWeedEntity>().HasKey(sc => new { sc.UserId, sc.WeedId   });


            modelBuilder.Entity<UserAddressEntity>()
    .HasOne<UserEntity>(sc => sc.User)
    .WithMany(s => s.UserAddress)
    .HasForeignKey(sc => sc.UserId);


            modelBuilder.Entity<UserAddressEntity>()
.HasOne<AddressEntity>(sc => sc.Address)
.WithMany(s => s.UserAddress)
.HasForeignKey(sc => sc.AddressId);

            // many to many users and weeds
            modelBuilder.Entity<UserWeedEntity>()
.HasOne<UserEntity>(sc => sc.User)
.WithMany(s => s.UserWeeds)
.HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserWeedEntity>()
.HasOne<WeedEntity>(sc => sc.Weed)
.WithMany(s => s.UserWeeds)
.HasForeignKey(sc => sc.WeedId);
        }

    }
}
