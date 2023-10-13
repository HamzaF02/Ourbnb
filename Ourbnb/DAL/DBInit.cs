using System;
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
            }

			
        }
	}
}

public class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ItemDbContext context = serviceScope.ServiceProvider.GetRequiredService<ItemDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (!context.Items.Any())
        {
            var items = new List<Item>
            {
                new Item
                {
                    Name = "Pizza",
                    Price = 150,
                    Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
                    ImageUrl = "/images/pizza.jpg"
                },
                new Item
                {
                    Name = "Fried Chicken Leg",
                    Price = 20,
                    Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
                    ImageUrl = "/images/chickenleg.jpg"
                },
                new Item
                {
                    Name = "French Fries",
                    Price = 50,
                    Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
                    ImageUrl = "/images/frenchfries.jpg"
                },
                new Item
                {
                    Name = "Grilled Ribs",
                    Price = 250,
                    Description = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
                    ImageUrl = "/images/ribs.jpg"
                },
                new Item
                {
                    Name = "Tacos",
                    Price = 150,
                    Description = "Tortillas filled with various ingredients such as seasoned meat, vegetables, and salsa, folded into a delicious handheld meal.",
                    ImageUrl = "/images/tacos.jpg"
                },
                new Item
                {
                    Name = "Fish and Chips",
                    Price = 180,
                    Description = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
                    ImageUrl = "/images/fishandchips.jpg"
                },
                new Item
                {
                    Name = "Cider",
                    Price = 50,
                    Description = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
                    ImageUrl = "/images/cider.jpg"
                },
                new Item
                {
                    Name = "Coke",
                    Price = 30,
                    Description = "Popular carbonated soft drink known for its sweet and refreshing taste.",
                    ImageUrl = "/images/coke.jpg"
                },

            };
            context.AddRange(items);
            context.SaveChanges();
        }

        if (!context.Customers.Any())
        {
            var customers = new List<Customer>
            {
                new Customer { Name = "Alice Hansen", Address = "Osloveien 1"},
                new Customer { Name = "Bob Johansen", Address = "Oslomet gata 2"},
            };
            context.AddRange(customers);
            context.SaveChanges();
        }

        if (!context.Orders.Any())
        {
            var orders = new List<Order>
            {
                new Order {OrderData = DateTime.Today.ToString(), CustomerId = 1,},
                new Order {OrderData = DateTime.Today.ToString(), CustomerId = 2,},
            };
            context.AddRange(orders);
            context.SaveChanges();
        }

        if (!context.OrderItems.Any())
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem { ItemId = 1, Quantity = 2, OrderId = 1},
                new OrderItem { ItemId = 2, Quantity = 1, OrderId = 1},
                new OrderItem { ItemId = 3, Quantity = 4, OrderId = 2},
            };
            foreach (var orderItem in orderItems)
            {
                var item = context.Items.Find(orderItem.ItemId);
                orderItem.OrderItemPrice = orderItem.Quantity * item?.Price ?? 0;
            }
            context.AddRange(orderItems);
            context.SaveChanges();
        }

        var ordersToUpdate = context.Orders.Include(o => o.OrderItems);
        foreach (var order in ordersToUpdate)
        {
            order.TotalPrice = order.OrderItems?.Sum(oi => oi.OrderItemPrice) ?? 0;
        }
        context.SaveChanges();
    }
}
