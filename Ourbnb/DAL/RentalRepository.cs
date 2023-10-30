﻿using Microsoft.EntityFrameworkCore;
using Ourbnb.Models;
using System;

namespace Ourbnb.DAL
{
	public class RentalRepository : IRepository<Rental>
	{
		private readonly RentalDbContext _db;
		public RentalRepository(RentalDbContext db)
		{
			_db = db;
		}

        public async Task<bool> Create(Rental rental)
        {
            try { 
                _db.Rentals.Add(rental);
                await _db.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                _logger.LogError("[RentalRepository] rental creation failed for rental {@rental}, error message: {ex}", order, ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var rental = await _db.Rentals.FindAsync(id);
                if (rental == null)
                {
                    _logger.LogError("[RentalRepository] rental not found for the RentalId {RentalId:0000}", id);

                    return false;
                }

                _db.Rentals.Remove(rental);
                await _db.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                _logger.LogError("[RentalRepository] rental deletion failed for RentalId {RentalId:0000}, error message: {ex}", id, ex.Message);
                return false;
            }
        }

        public async Task<Rental?> getObjectById(int id)
        {
            try
            {
                return await _db.Rentals.FindAsync(id);
            }catch(Exception ex)
            {
                _logger.LogError("[RentalRepository] rental FindAsync(id) failed when GetObjectById for RentalId {RentalId:0000}, error message: {ex}", id, ex.Message);
                return null;
            }
            
        }

        public async Task<IEnumerable<Rental>?> GetAll()
        {
            try
            {
                return await _db.Rentals.ToListAsync();
            }catch(Exception ex)
            {
                logger.LogError("[RentalRepository] order ToListAsync() failed when GetAll(), error message: {ex}", ex.Message);
                return null;
            }
            
        }

        public async Task<bool> Update(Rental rental)
        {
            try
            {
                _db.Rentals.Update(rental);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("[RentalRepository] rental FindAync(id) failed when updating the RentalId {RentalId:0000}, error message: {ex}", order, ex.Message);
                return false;
            }
        }
    }
}

