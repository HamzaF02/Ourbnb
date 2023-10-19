using Microsoft.EntityFrameworkCore;
using Ourbnb.Migrations;
using Ourbnb.Models;

namespace Ourbnb.DAL
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly RentalDbContext _db;
        public OrderRepository(RentalDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Order order)
        {
            try
            {
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order?> getObjectById(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<bool> Update(Order order)
        {
            try
            {
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
