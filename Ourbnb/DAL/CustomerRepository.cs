using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;
using System;

namespace Ourbnb.DAL
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly RentalDbContext _db;
        private readonly ILogger<CustomerRepository> _logger;


        // constructor for database and logger
        public CustomerRepository(RentalDbContext db, ILogger<CustomerRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        // function to create a new customer 
        public async Task<bool> Create(Customer customer)
        {
            // try catch to add customer
            try
            {
                // adding a new customer to the database and save changes 
                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // an error occured when creating a new customer 
                _logger.LogError("[CustomerRepository] customer creation failed for customer {@customer}, error message: {ex}", customer, ex.Message);
                return false;
            }
        }

        // function to delete customer 
        public async Task<bool> Delete(int id)
        {
            // try catch to delete customer
            try
            {
                //find customer by id 
                var customer = await _db.Customers.FindAsync(id);
                if (customer == null)
                {
                    // if the customer is null it is not found
                    _logger.LogError("[CustomerRepository] customer not found for the CustomerId {CustomerId:0000}", id);
                    return false;
                }

                // if it exist, we remove customer and save changes
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // failed to delete customer
                _logger.LogError("[CustomerRepository] customer deletion failed for CustomerId {CusotmerId:0000}, error message: {ex}", id, ex.Message);
                return false;
            }
        }

        // function to get all customers
        public async Task<IEnumerable<Customer>?> GetAll()
        {
            // try catch to get all customers
            try
            {
                // return list of all customers
                return await _db.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                // failed to get all customers
                _logger.LogError("[CustomerRepository] customer ToListAsync() failed when GetAll(), error message: {ex}", ex.Message);
                return null;
            }
        }

        // function to get customer by id
        public async Task<Customer?> getObjectById(int id)
        {
            // try catch to get customer by id
            try
            {
                // return customer by id
                return await _db.Customers.FindAsync(id);
            }
            catch (Exception ex)
            {
                // failed to find customer
                _logger.LogError("[CustomerRepository] customer FindAsync(id) failed when GetObjectById() for CustomerId {CustomerId:0000}, error message: {ex}", id, ex.Message);
                return null;
            }
        }
        // function to update customer
        public async Task<bool> Update(Customer customer)
        {
            // try catch to update customer
            try
            {
                // update customer in database
                _db.Customers.Update(customer);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // failed to update customer
                _logger.LogError("[CustomerRepository] customer FindAync(id) failed when updating the CustomerId {CustomerId:0000}, error message: {ex}", customer, ex.Message);
                return false;
            }
        }
    }
}
