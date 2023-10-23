using System;
using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;

namespace Ourbnb.DAL
{
	public class DBInit
	{
		public static void Seed(IApplicationBuilder app)
		{
            using var serviceScope = app.ApplicationServices.CreateScope();
            RentalDbContext context = serviceScope.ServiceProvider.GetRequiredService<RentalDbContext>();
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();


            if (!context.Customers.Any())
            {
                var customer = new List<Customer>
                {
                    new Customer
                    {
                        FirstName="Hamza",
                        LastName="Ylli",
                        Address="Pilestreden 32",
                        Phone=91824521,
                        Email="Hamza@gmail.com"
                       
                       
                    },

                    new Customer
                    {
                        FirstName="Mahdi",
                        LastName="Yusuf",
                        Address="Pilestredet 35",
                        Phone=47782356,
                        Email="Mahdi123@gmail.com"
                     

                    }
                
                };
                context.AddRange(customer);
                context.SaveChanges();
            }


            if (!context.Rentals.Any())
            {
                var rentals = new List<Rental>
                {
                    new Rental
                    {
                        Name="Hytte i Nordkapp",
                        Location="Nordkapp",
                        Description="Fin moderne hytte med mye plass",
                        Price=2500,
                        OwnerId=1,
                        FromDate=new DateOnly(2023, 10, 21),
                        ToDate=new DateOnly(2023, 10, 30),
                        Bilder = "https://hellvikhytte.no/content/uploads/sites/14/2021/08/Fjellerke-3-scaled.jpg",
                        Rating=4.5,
                    },
                    new Rental
                    {
                        Name = "House by the waterfalls",
                        Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                        FromDate = new DateOnly(2015, 10, 10),
                        ToDate = new DateOnly(2015, 10, 20),
                        Rating = 3.5,
                        Location = "Gran",
                        Price = 150,
                        Bilder = "/images/1.jpeg",
                        OwnerId = 2,
                    },
                    new Rental
                    {
                        Name = "House by the forrest",
                        Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                        FromDate = new DateOnly(2016, 04, 10),
                        ToDate = new DateOnly(2016, 04, 20),
                        Rating = 3.9,
                        Location = "Tomter",
                        Price = 150,
                        Bilder = "/images/modernhouse.jpg",
                        OwnerId = 1
                    },
                    new Rental
                    {
                        Name = "House by the mountains",
                        Description = "A nice place to be with friends and family to enjoy a calm and nice view",
                        FromDate = new DateOnly(2019, 08, 10),
                        ToDate = new DateOnly(2019, 08, 10),
                        Rating = 4.1,
                        Location = "Hafjell",
                        Price = 150,
                        Bilder = "/images/mountain.jpeg",
                        OwnerId = 2
                    }
                };
                context.AddRange(rentals);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var orders = new List<Order>
                {
                    new Order
                    {
                        CustomerId = 1,
                        RentalId = 1,
                        From = new DateOnly(2023, 10, 30),
                        To = new DateOnly(2023, 10, 21),
                    }
                };
                context.AddRange(orders);
                context.SaveChanges();
            }
			
        }
	}
}
