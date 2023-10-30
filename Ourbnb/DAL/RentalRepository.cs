using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;
using System;

namespace Ourbnb.DAL
{
	public class RentalRepository : IRepository<Rental>
	{
		private readonly RentalDbContext _db;
        private readonly ILogger<RentalRepository> _logger;

        public RentalRepository(RentalDbContext db, ILogger<RentalRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        //Creation of a Rental
        public async Task<bool> Create(Rental rental)
        {
            //Try catch in case of error
            try { 
                //Adds it to database and saves
                _db.Rentals.Add(rental);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                //logs and returns false if something goes wrong
                _logger.LogError("[RentalRepository] rental creation failed for rental {@rental}, error message: {ex}", rental, ex.Message);
                return false;
            }
        }

        //Deletion of a Rental
        public async Task<bool> Delete(int id)
        {
            //Try catch in case of error
            try
            {
                // find rental by id
                var rental = await _db.Rentals.FindAsync(id);
                if (rental == null)
                {
                    //error if not found
                    _logger.LogError("[RentalRepository] rental not found for the RentalId {RentalId:0000}", id);

                    return false;
                }
                //deletion of rental and save
                _db.Rentals.Remove(rental);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                // failed to delete 
                _logger.LogError("[RentalRepository] rental deletion failed for RentalId {RentalId:0000}, error message: {ex}", id, ex.Message);
                return false;
            }
        }

        //get Rental by id 
        public async Task<Rental?> getObjectById(int id)
        {
            //Try catch in case of error
            try
            {
                // return rental by id
                return await _db.Rentals.FindAsync(id);
            }catch(Exception ex)
            {
                // failed to get rental by id
                _logger.LogError("[RentalRepository] rental FindAsync(id) failed when GetObjectById for RentalId {RentalId:0000}, error message: {ex}", id, ex.Message);
                return null;
            }
            
        }

        //get all rentals
        public async Task<IEnumerable<Rental>?> GetAll()
        {
            //Try catch in case of error
            try
            {
                // get all rentals
                return await _db.Rentals.ToListAsync();
            }catch(Exception ex)
            {
                // failed to get all
                _logger.LogError("[RentalRepository] order ToListAsync() failed when GetAll(), error message: {ex}", ex.Message);
                return null;
            }
            
        }

        //update a rental
        public async Task<bool> Update(Rental rental)
        {
            //Try catch in case of error
            try
            {
                //update rental and save
                _db.Rentals.Update(rental);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // failed to update rental
                _logger.LogError("[RentalRepository] rental FindAync(id) failed when updating the RentalId {RentalId:0000}, error message: {ex}", rental, ex.Message);
                return false;
            }
        }
    }
}

