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

            if (!context.Customers.Any())
            {
                var customer = new List<Customer>
                {
                    new Customer
                    {
                        FirstName="Hamza",
                        LastName="Ylli",
                        Address="Pilestreden 32",
                        Email="Hamza@gmail.com",
                        Phone=91824521
                    },

                    new Customer
                    {
                        FirstName="Mahdi",
                        LastName="Yusuf",
                        Address="Pilestredet 35",
                        Email="Mahdi123@gmail.com",
                        Phone=47782356
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
                        Price=2000,
                        OwnerId=1,
                        FromDate=new DateOnly(2023, 10, 30),
                        ToDate=new DateOnly(2023, 10, 21),
                        Bilder= "https://hellvikhytte.no/content/uploads/sites/14/2021/08/Fjellerke-3-scaled.jpg",
                        Rating=4.5,
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
