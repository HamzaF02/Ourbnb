using Microsoft.EntityFrameworkCore;
using Ourbnb.Migrations;
using Ourbnb.Models;

namespace Ourbnb.DAL
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly RentalDbContext _db;

        private readonly ILogger<OrderRepository> _logger;

        // constructor for database and logger
        public OrderRepository(RentalDbContext db, ILogger<OrderRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // function to create a new order 
        public async Task<bool> Create(Order order)
        {
            // try to add order to database
            try
            {
                // add order to database and save changes 
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                // failed to add order
                _logger.LogError("[OrderRepository] order creation failed for order {@order}, error message: {ex}", order, ex.Message);
                return false;
            }
        }

        // function to delete a order 
        public async Task<bool> Delete(int id)
        {
            // try to delete order to database
            try
            {
                // find order
                var order = await _db.Orders.FindAsync(id);
                if (order == null)
                {
                    // error if order is not found
                    _logger.LogError("[OrderRepository] order not found for the OrderId {OrderId:0000}", id);
                    return false;
                }
                // remove order from databse
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                // failed to delete from database
                _logger.LogError("[OrderRepository] order deletion failed for OrderId {OrderId:0000}, error message: {ex}", id, ex.Message);
                return false;
            }
        }

        // function to get all orders 
        public async Task<IEnumerable<Order>?> GetAll()
        {
            // try to get all order from database
            try
            {
                return await _db.Orders.ToListAsync();
            }
            catch(Exception ex)
            {
                // failed to get all orders
                _logger.LogError("[OrderRepository] order ToListAsync() failed when GetAll(), error message: {ex}", ex.Message);
                return null;
            }
        }

        // function to get a order by id
        public async Task<Order?> getObjectById(int id)
        {
            // try to get  order from database by id
            try
            {
                // return order by id
                return await _db.Orders.FindAsync(id);
            }
            catch (Exception ex)
            {
                // failed to get order by id
                _logger.LogError("[OrderRepository] order FindAsync(id) failed when GetObjectById for OrderId {OrderId:0000}, error message: {ex}", id, ex.Message);
                return null;
            }
        }

        // function to update order
        public async Task<bool> Update(Order order)
        {
            // try to update order from database 
            try
            {
                //update and save 
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                // failed to update order
                _logger.LogError("[OrderRepository] order FindAync(id) failed when updating the OrderId {OrderId:0000}, error message: {ex}", order, ex.Message);
                return false;
            }
        }
    }
}
