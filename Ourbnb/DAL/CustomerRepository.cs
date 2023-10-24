using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;
using System;

namespace Ourbnb.DAL
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly RentalDbContext _db;
        public CustomerRepository(RentalDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _db.Customers.ToListAsync();
        }

        public async Task<Customer?> getObjectById(int id)
        {
            return await _db.Customers.FindAsync(id);
        }

        public async Task<bool> Update(Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
