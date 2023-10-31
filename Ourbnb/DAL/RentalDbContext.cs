using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using Ourbnb.Models;
namespace Ourbnb.DAL
{
	public class RentalDbContext : IdentityDbContext
	{
		// dbcontext that uses identity and initilizes rental, customers and orders
		public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
		{
			//Database.EnsureCreated();
		}
		public DbSet<Rental> Rentals { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}

