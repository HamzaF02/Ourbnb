using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;
using System;

namespace Ourbnb.DAL
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly RentalDbContext _db;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(RentalDbContext db, ILogger<CustomerRepository> logger)
        {
            _db = db;
            _logger = logger;
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
                _logger.LogError("[CustomerRepository] customer creation failed for customer {@customer}, error message: {ex}", customer, ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(id);
                if (customer == null)
                {
                    _logger.LogError("[CustomerRepository] customer not found for the CustomerId {CustomerId:0000}", id);
                    return false;
                }

                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[CustomerRepository] customer deletion failed for CustomerId {CusotmerId:0000}, error message: {ex}", id, ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Customer>?> GetAll()
        {
            try
            {
                return await _db.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("[CustomerRepository] customer ToListAsync() failed when GetAll(), error message: {ex}", ex.Message);
                return null;
            }
        }

        public async Task<Customer?> getObjectById(int id)
        {
            try
            {
                return await _db.Customers.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("[CustomerRepository] customer FindAsync(id) failed when GetObjectById() for CustomerId {CustomerId:0000}, error message: {ex}", id, ex.Message);
                return null;
            }
        }

        public async Task<bool> Update(Customer customer)
        {
            try
            {
                _db.Customers.Update(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[CustomerRepository] customer FindAync(id) failed when updating the CustomerId {CustomerId:0000}, error message: {ex}", customer, ex.Message);
                return false;
            }
        }
    }
}
