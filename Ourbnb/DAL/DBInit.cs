using System;
using Castle.Core.Resource;
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
                        Bilder= new List<String> {"https://hellvikhytte.no/content/uploads/sites/14/2021/08/Fjellerke-3-scaled.jpg"},
                        Rating=4.5,
                    }
                };
                context.AddRange(rentals);
                context.SaveChanges();
            }

			
        }
	}
}
