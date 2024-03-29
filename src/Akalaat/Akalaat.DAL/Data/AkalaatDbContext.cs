﻿using Akalaat.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Akalaat.DAL.Data
{
    public class AkalaatDbContext : IdentityDbContext<ApplicationUser>
    {
        public AkalaatDbContext(DbContextOptions<AkalaatDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Menu_Item_Size>().HasKey(MS => new { MS.Item_ID,MS.Item_Size_ID });

            builder.Entity<Order>()
             .HasMany(o => o.Items) 
             .WithMany(i => i.Orders) 
             .UsingEntity(m => m.ToTable("OrderItems"));

            //builder.Entity<ShoppingCart>()
            // .HasMany(o => o.Items)
            // .WithMany(i => i.ShoppingCarts)
            // .UsingEntity(m => m.ToTable("ShoppingCartItems"));

            builder.Entity<ShoppingCartItem>()
         .HasKey(sc => new { sc.ShoppingCartId, sc.ItemId }); 

            builder.Entity<Item>()
             .Property(i => i.Id)
             .ValueGeneratedOnAdd();

            builder.Entity<Branch>()
            .HasOne(b => b.Region)
            .WithMany(ada => ada.Branches)
            .HasForeignKey(ada => ada.Region_ID);

            builder.Entity<Region>()
                .HasMany(r => r.BranchesDelivery)
                .WithMany(b => b.DeliveryAreas)
                .UsingEntity<Available_Delivery_Area>(
                    j => j.HasOne(ada => ada.Branch).WithMany().HasForeignKey(ada => ada.BranchId),
                    j => j.HasOne(ada => ada.Region).WithMany().HasForeignKey(ada => ada.RegionId));


            builder.Entity<Branch>().Property(b => b.AddressDetails).IsRequired();


            base.OnModelCreating(builder);


        }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Address_Book> AddressBooks { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Available_Delivery_Area> AvailableDeliveryAreas {  get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Item_Size> ItemSizes { get; set; }
        public virtual DbSet<Menu_Item_Size> MenuItemSizes { get; set; }
        public virtual DbSet<Mood> Moods { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Resturant> Resturants { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }


    }
}
