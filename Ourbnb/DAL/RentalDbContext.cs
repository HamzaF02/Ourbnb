﻿using Microsoft.EntityFrameworkCore;
using System;
using Ourbnb.Models;
namespace Ourbnb.DAL
{
	public class RentalDbContext : DbContext
	{
		public RentalDbContext(DbContextOptions<RentalDbContext> options) : base(options)
		{
			//Database.EnsureCreated();
		}
		public DbSet<Rental> Rentals { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
	}
}

